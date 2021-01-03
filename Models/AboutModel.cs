using System;
using System.Collections.Generic;

namespace Venjix.Models
{
    public class AboutModel
    {
        public string Version { get; set; }
        public List<Contributor> Contributors { get; set; }
        public bool IsUpdateAvailable { get; set; }
    }
}
