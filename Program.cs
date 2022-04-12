using GithubProjectDownloader.Models;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubProjectDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Analyzer analyzer = new Analyzer();
            //Reading All ProjectInfo
            analyzer.Test();
            List<ProjectInfo> projectList = analyzer.ReadAllProjectInfo();
            analyzer.DownloadProjects(projectList);
        }
    }
}
