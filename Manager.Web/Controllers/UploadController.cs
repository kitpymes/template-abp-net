using Abp.IO;
using Abp.Localization;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Security.AntiForgery;
using log4net;
using Manager.Web.Controllers.Results;
using Manager.Web.Models.Upload;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Manager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UploadController : ManagerControllerBase
    {
        #region Avatar Profile

        public ActionResult _AvatarProfile(UploadConfigViewModel fileConfig)
        {
            try
            {
                if (fileConfig.HasInitialPreviewAsData)
                {
                    var directoryPathServer = Server.MapPath("~" + fileConfig.UploadsPath);
                    var initialPreview = new List<string>();
                    var initialPreviewConfig = new List<PreviewViewModel>();

                    if (Directory.Exists(directoryPathServer))
                    {
                        var list = GetFilesFrom(directoryPathServer,  fileConfig.FileExtensions.ToArray());

                        if (list != null && list.Count() > 0)
                        {
                            var key = AbpSession.UserId.Value;

                            for (int i = 0; i < list.Length; i++)
                            {
                                var path = fileConfig.UploadsPath + Path.GetFileName(list[i]);
                                initialPreview.Add(path);

                                var url = "Upload/DeleteFiles?deletePath=" + path;
                                initialPreviewConfig.Add(new PreviewViewModel
                                {
                                    url = url,
                                    key = key,
                                    showDelete = true,
                                    showDrag = false,
                                    showZoom = true
                                });
                            }
                        }

                    }

                    fileConfig.HasInitialPreviewAsData = initialPreview.Count() > 0;
                    fileConfig.InitialPreview = initialPreview;
                    fileConfig.InitialPreviewConfig = initialPreviewConfig;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(ex);
            }

            return PartialView(fileConfig);
        }

        #endregion

        [DisableAbpAntiForgeryTokenValidation]
        public JsonResult UploadsFiles(string pathFile)
        {
            try
            {
                foreach (var item in Request.Files.AllKeys)
                {
                    HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;

                    if (file.ContentLength > 0)
                    {
                        string pathFileServer = Server.MapPath(pathFile);
                        string pathFileServerDirectory = Path.GetDirectoryName(pathFileServer);

                        if (!Directory.Exists(pathFileServerDirectory))
                            Directory.CreateDirectory(pathFileServerDirectory);
                        else
                            FileHelper.DeleteIfExists(pathFileServer);

                        file.SaveAs(pathFileServer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(ex);
            }

            return Json("");
        }

        public JsonResult DeleteFiles(string pathFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(pathFile))
                {
                    string pathFileServer = Server.MapPath(pathFile);
                    FileHelper.DeleteIfExists(pathFileServer);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(ex);
            }

            return Json("");
        }

        private String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive = false)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if(filters.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
                }
            }
            else
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", "*"), searchOption));
            }

           
            return filesFound.ToArray();
        }
    }
}