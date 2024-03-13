using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerAutoUpdate
{
    public class FileInfo
    {
        public string? Key { get; set; }

        public string? Version { get; set; }

        public bool? Update { get; set; }

        public int NumberVersion { get; set; }
    }
}
