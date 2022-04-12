using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubProjectDownloader.Models
{
    public class ProjectDetails
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "private")]
        public string PrivacyStatus{ get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "clone_url")]
        public string CloneURL { get; set; }
    }
}
