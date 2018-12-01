using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Updater
{
    public class GitHubUpdateResult
    {
        public string PackageId { get; }
        public string PackageName { get; }
        public string PackageDownloadUrl { get; }
        public DateTime PublishDate { get; set; }
        public int Size { get; set; }

        public GitHubUpdateResult(string packageId, string packageName, string packageDownloadUrl, DateTime publishDate, int size)
        {
            this.PackageId = packageId;
            this.PackageName = packageName;
            this.PackageDownloadUrl = packageDownloadUrl;
            this.PublishDate = publishDate;
            this.Size = size;
        }
    }
}
