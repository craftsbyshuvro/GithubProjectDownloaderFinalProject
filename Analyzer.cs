using GithubProjectDownloader.Models;
using LibGit2Sharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GithubProjectDownloader
{
    public class Analyzer
    {
        private static HttpClient client;

        public Analyzer()
        {
            client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true, PreAuthenticate = true });
        }

        public List<ProjectInfo> ReadAllProjectInfo()
        {
            var projectFile = new LinqToExcel.ExcelQueryFactory(@"D:\Study\Final Project\Dataset\Metadata\10Projects.csv");

            List<ProjectInfo> projectList = (from row in projectFile.Worksheet("10Projects")
                                             let item = new ProjectInfo()
                                             {
                                                 Id = row["id"].Cast<string>(),
                                                 Name = row["name"].Cast<string>(),
                                                 URL = row["url"].Cast<string>()
                                             }
                                             select item).ToList();

            return projectList;
        }

        public void Test()
        {


        }

        public List<ProjectDetails> DownloadProjects(List<ProjectInfo> projectList)
        {
            List<ProjectDetails> activeProjectDetailsList = new List<ProjectDetails>();
            int rowCount = 0;

            foreach (var item in projectList)
            {
                string projectUrl = item.URL;
                try
                {
                    int currentRow = rowCount++;
                    Console.WriteLine(currentRow + " " + item.Name + " started");


                    //Creating Directory for the project
                    string vProjectDirectory = @"D:\Study\Final Project\Dataset\Projects\" + item.Name;
                    System.IO.Directory.CreateDirectory(vProjectDirectory);

                    string toBeSearched = "/repos/";
                    string projectName = projectUrl.Substring(projectUrl.IndexOf(toBeSearched) + toBeSearched.Length);
                    string vGitCloneUrl = "https://github.com/" + projectName + ".git";

                    Task.Run(() =>
                    {
                        try
                        {
                            Repository.Clone(vGitCloneUrl, vProjectDirectory,
                                new CloneOptions { OnTransferProgress = Analyzer.TransferProgress });
                        }
                        catch (Exception ex)
                        {
                            string ErrMsg = ex.GetBaseException().Message;
                            Console.WriteLine(ErrMsg);
                            File.AppendAllText(@"D:\Study\Final Project\Dataset\Projects\DownloadException.txt", item.Name + " <==>" + ErrMsg + Environment.NewLine);

                        }

                    }).Wait();


                    Console.WriteLine(currentRow + " " + item.Name + " finished");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }
            }

            return activeProjectDetailsList;
        }

        public static bool TransferProgress(TransferProgress progress)
        {
            Console.WriteLine($"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}");
            return true;
        }
    }
}
