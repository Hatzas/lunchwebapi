using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Lunch.WebApi.Controllers;
using Lunch.WebApi.Helpers.CustomException;
using Lunch.WebApi.Helpers.Models;
using Lunch.WebApi.Models;
using Newtonsoft.Json;

namespace Lunch.WebApi.Helpers
{
    public class ControllerHelper
    {
        public static TemporaryFolder GetNewTemporaryFolder()
        {
            var folderName = Guid.NewGuid().ToString();
            var virtualFolderPath = ConfigurationManager.AppSettings["LocalTemporaryFolder"] + "\\" + folderName;
            var fullFolderPath = HttpContext.Current.Server.MapPath(virtualFolderPath);

            return new TemporaryFolder
            {
                FolderName = folderName,
                VirtualFolderPath = virtualFolderPath,
                FullFolderPath = fullFolderPath
            };
        }

        public static Dictionary<string, string> GetFormDataDictionaryFromJson(NameValueCollection nameValueCollection)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();
            if (nameValueCollection.AllKeys.Any())
            {
                var firstKey = nameValueCollection.AllKeys.FirstOrDefault();

                formData = JsonConvert.DeserializeObject<Dictionary<string, string>>(nameValueCollection[firstKey]);
            }

            return formData;
        }
        
        public static MultipartFileData ValidateFileUpload(Collection<MultipartFileData> collection)
        {
            if (!collection.Any())
            {
                throw new CustomValidationException("No input file specified", HttpStatusCode.NotAcceptable);
            }
            else if (collection.Count > 1)
            {
                throw new CustomValidationException("Only one file allowed", HttpStatusCode.NotAcceptable);
            }

            return collection.FirstOrDefault();
        }

        public static FormData ValidateFormData(Dictionary<string, string> formData)
        {
            var formDataErrors = new List<string>();
            var thumbnailFormData = new FormData();


            if (formData.ContainsKey("DishId") && !string.IsNullOrEmpty(formData["DishId"]))
                thumbnailFormData.DishId = formData["DishId"];
            else
                formDataErrors.Add(string.Format("'{0}' field is is required", "DishId"));


            if (formDataErrors.Any())
                throw new CustomValidationException(String.Join(";", formDataErrors), HttpStatusCode.NotAcceptable);

            return thumbnailFormData;
        }

        public static void DirectoryCleanup(List<string> pathList)
        {
            foreach (var path in pathList)
            {
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                    Directory.Delete(path, true);
            }
        }

        public static string GetFileNameFromHeader(ContentDispositionHeaderValue contentDispositionHeader)
        {
            var fileName = contentDispositionHeader.FileName;
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }

            return fileName;
        }
    }
}