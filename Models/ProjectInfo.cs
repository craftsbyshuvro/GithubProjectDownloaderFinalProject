using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubProjectDownloader.Models
{
    public class ProjectInfo
    {
        public int Id { get; set; }
        public string Package { get; set; }
        public string PartialURL { get; set; }
        public string FullURL { get; set; }
        public int StarCount { get; set; }
        public int ContributorCount { get; set; }
        public string DateCreated { get; set; }
    }
}
