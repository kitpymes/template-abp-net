using Manager.Web.Controllers;
using System.Collections.Generic;

namespace Manager.Web.Models.Upload
{
    public class UploadConfigViewModel
    {
        public long UserId { get; set; }
        public string Languaje { get; set; }
        public long MaxFileSize { get; set; }
        public List<string> FileExtensions { get; set; }
        public string UploadsPath { get; set; }
        public bool HasInitialPreviewAsData { get; set; }
        public List<string> InitialPreview { get; set; }
        public List<PreviewViewModel> InitialPreviewConfig { get; set; }
        public string StaticName { get; set; }
        public string StaticExtension { get; set; }
        public int MaxFileCount { get; set; }
        public string ImageDefaultPath { get; set; }
        public string DefaultPreviewContent { get; set; }
    }
}
