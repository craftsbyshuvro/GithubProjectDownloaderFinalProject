using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubProjectDownloader.Models
{
    public class ProjectInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string URL { get; set; }
    }
}
