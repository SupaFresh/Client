using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Octokit;

namespace Client.Logic.Updater
{
    public class GitHubUpdater
    {
        GitHubClient client;

        string baseDirectory;

        public GitHubUpdater()
        {
            this.client = new GitHubClient(new ProductHeaderValue("pmdcp"));
            this.baseDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public async Task<GitHubUpdateResult> CheckForUpdates()
        {
            var release = await client.Repository.Release.GetLatest("PMDShift", "Client");

            ReleaseAsset clientPackage = release.Assets.Where(x => x.Name == "PMDShift.zip").FirstOrDefault();

            if (clientPackage == null)
            {
                return new GitHubUpdateResult(false);
            }

            if (IO.Options.LastUpdateTime < clientPackage.CreatedAt)
            {
                return new GitHubUpdateResult(true, clientPackage.BrowserDownloadUrl, clientPackage.CreatedAt.DateTime, clientPackage.Size);
            } else
            {
                return new GitHubUpdateResult(false);
            }
        }

        public async Task PerformUpdate(GitHubUpdateResult updateResult, Action<string> statusCallback)
        {
            var packageTempFile = Path.GetTempFileName();

            using (var webClient = new WebClient())
            {
                await webClient.DownloadFileTaskAsync(updateResult.PackageDownloadUrl, packageTempFile);
            }

            var binaries = Directory.EnumerateFiles(baseDirectory, "*.dll", SearchOption.TopDirectoryOnly).Concat(Directory.EnumerateFiles(baseDirectory, "*.exe", SearchOption.TopDirectoryOnly));
            foreach (var binary in binaries)
            {
                File.Move(binary, binary + ".ToDelete");
            }

            using (var fileStream = new FileStream(packageTempFile, System.IO.FileMode.Open))
            {
                using (var zipArchive = new ZipArchive(fileStream))
                {
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (entry.FullName.EndsWith("/") && string.IsNullOrEmpty(entry.Name))
                        {
                            if (!Directory.Exists(Path.Combine(baseDirectory, entry.FullName)))
                            {
                                Directory.CreateDirectory(Path.Combine(baseDirectory, entry.FullName));
                            }
                        }
                        else
                        {
                            try
                            {
                                statusCallback($"Extracting: {entry.Name}");
                                using (var entryFileSteam = new FileStream(Path.Combine(baseDirectory, entry.FullName), System.IO.FileMode.Create))
                                {
                                    using (var entryStream = entry.Open())
                                    {
                                        entryStream.CopyTo(entryFileSteam);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            IO.Options.LastUpdateTime = updateResult.PublishDate;
            IO.Options.SaveXml();
        }
    }
}
