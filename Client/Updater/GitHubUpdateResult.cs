using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Updater
{
    public class GitHubUpdateResult
    {
        public bool UpdateAvailable { get; }
        public string PackageDownloadUrl { get; }
        public DateTime PublishDate { get; set; }
        public int Size { get; set; }

        public GitHubUpdateResult(bool updateAvailable)
        {
            this.UpdateAvailable = updateAvailable;
        }

        public GitHubUpdateResult(bool updateAvailable, string packageDownloadUrl, DateTime publishDate, int size)
        {
            this.UpdateAvailable = updateAvailable;
            this.PackageDownloadUrl = packageDownloadUrl;
            this.PublishDate = publishDate;
            this.Size = size;
        }
    }
}
