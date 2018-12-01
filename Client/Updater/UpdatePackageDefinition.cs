using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Updater
{
    public class UpdatePackageDefinition
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string PackageName { get; set; }

        public UpdatePackageDefinition(string id, string owner, string repository, string packageName)
        {
            this.Id = id;
            this.Owner = owner;
            this.Repository = repository;
            this.PackageName = packageName;
        }
    }
}
