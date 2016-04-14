using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Helpers.Models
{
    public class TemporaryFolder
    {
        public string FolderName { get; set; }

        public string VirtualFolderPath { get; set; }

        public string FullFolderPath { get; set; }
    }
}