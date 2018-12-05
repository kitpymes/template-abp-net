using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Web.Models.Upload
{
    public class PreviewViewModel
    {
        public string caption { get; set; }
        public int size { get; set; }
        public string width { get; set; }
        public string url { get; set; }
        public long key { get; set; }
        public bool showDelete { get; set; }
        public bool showZoom { get; set; }
        public bool showDrag { get; set; }
    }
}