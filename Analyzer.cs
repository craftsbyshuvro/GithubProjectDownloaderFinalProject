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
            var projectFile = new LinqToExcel.ExcelQueryFactory(@"D:\Dataset\Metadata\common-io.csv");

            List<ProjectInfo> projectList = (from row in projectFile.Worksheet("common-io")
                                             let item = new ProjectInfo()
                                             {
                                                 Id = row["id"].Cast<int>(),
                                                 Package = row["package_name"].Cast<string>(),
                                                 PartialURL = row["name_with_owner"].Cast<string>(),
                                                 StarCount = row["stars_count"].Cast<int>(),
                                                 ContributorCount = row["contributors_count"].Cast<int>(),
                                                 DateCreated = row["created_date"].Cast<string>()
                                             }
                                             select item).ToList();

            return projectList;
        }
         

        public List<ProjectDetails> DownloadProjects(List<ProjectInfo> projectList)
        {
            List<ProjectDetails> activeProjectDetailsList = new List<ProjectDetails>();
            int rowCount = 0;

            foreach (var item in projectList)
            {
                try
                {
                    int currentRow = rowCount++;
                    Console.WriteLine(currentRow + " " + item.PartialURL + " started");

                     //Creating Directory for the project
                    string vProjectDirectory = @"D:\Dataset\Projects\" + item.PartialURL.Replace("/","-");
                    System.IO.Directory.CreateDirectory(vProjectDirectory);


                    string vGitCloneUrl = "https://github.com/" + item.PartialURL + ".git";
                    
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
                            File.AppendAllText(@"D:\Dataset\Projects\DownloadException.txt", "Package: " + item.Package+ " Project: "+ item.PartialURL + " <==>" + ErrMsg + Environment.NewLine);

                        }

                    }).Wait();

                    
                    Console.WriteLine(currentRow + " " + item.PartialURL + " finished");
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
