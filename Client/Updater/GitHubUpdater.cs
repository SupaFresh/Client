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

        UpdatePackageDefinition[] packages = new UpdatePackageDefinition[]
        {
            new UpdatePackageDefinition("client", "PMDShift", "Client", "PMDShift.zip"),
            new UpdatePackageDefinition("gfx", "PMDShift", "GFX", "GFX.zip")
        };

        public GitHubUpdater()
        {
            this.client = new GitHubClient(new ProductHeaderValue("pmdcp"));
            this.baseDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public async Task<List<GitHubUpdateResult>> CheckForUpdates()
        {
            var result = new List<GitHubUpdateResult>();

            foreach (var package in packages)
            {
                var release = await client.Repository.Release.GetLatest(package.Owner, package.Repository);

                ReleaseAsset clientPackage = release.Assets.Where(x => x.Name == package.PackageName).FirstOrDefault();

                if (clientPackage == null)
                {
                    continue;
                }

                if (GetLastUpdateTime(package.Id) < clientPackage.CreatedAt.UtcDateTime)
                {
                    result.Add(new GitHubUpdateResult(package.Id, package.Repository, clientPackage.BrowserDownloadUrl, clientPackage.CreatedAt.UtcDateTime, clientPackage.Size));
                }
            }

            return result;
        }

        private DateTime GetLastUpdateTime(string packageId)
        {
            switch (packageId)
            {
                case "client":
                    return IO.Options.LastClientUpdateTime;
                case "gfx":
                    return IO.Options.LastGFXUpdateTime;
            }

            throw new InvalidOperationException();
        }

        private void SetLastUpdateTime(string packageId, DateTime dateTime)
        {
            switch (packageId)
            {
                case "client":
                    IO.Options.LastClientUpdateTime = dateTime;
                    break;
                case "gfx":
                    IO.Options.LastGFXUpdateTime = dateTime;
                    break;
            }
        }

        public async Task PerformUpdate(IReadOnlyList<GitHubUpdateResult> updateResults, Action<string> statusCallback)
        {
            for (var i = 0; i < updateResults.Count; i++)
            {
                await PerformUpdate(i, updateResults.Count, updateResults[i], statusCallback);
            }
        }

        public async Task PerformUpdate(int index, int total, GitHubUpdateResult updateResult, Action<string> statusCallback)
        {
            var packageTempFile = Path.GetTempFileName();

            statusCallback($"Downloading update {index + 1} of {total}...");

            using (var webClient = new WebClient())
            {
                await webClient.DownloadFileTaskAsync(updateResult.PackageDownloadUrl, packageTempFile);
            }

            statusCallback("Extracting files... please wait...");
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
                                string fullEntryPath;
                                if (updateResult.PackageId == "gfx")
                                {
                                    fullEntryPath = Path.Combine(baseDirectory, "GFX", entry.FullName);
                                }
                                else
                                {
                                    fullEntryPath = Path.Combine(baseDirectory, entry.FullName);
                                }

                                if (!Directory.Exists(Path.GetDirectoryName(fullEntryPath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(fullEntryPath));
                                }

                                switch (Path.GetExtension(fullEntryPath))
                                {
                                    case ".dll":
                                    case ".exe":
                                        {
                                            var targetFile = $"{fullEntryPath}.ToDelete";
                                            if (File.Exists(targetFile))
                                            {
                                                File.Delete(targetFile);
                                            }
                                            File.Move(fullEntryPath, targetFile);
                                        }
                                        break;
                                }

                                using (var entryFileSteam = new FileStream(fullEntryPath, System.IO.FileMode.Create))
                                {
                                    using (var entryStream = entry.Open())
                                    {
                                        await entryStream.CopyToAsync(entryFileSteam);
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

            SetLastUpdateTime(updateResult.PackageId, updateResult.PublishDate);
            IO.Options.SaveXml();

            File.Delete(packageTempFile);
        }
    }
}
