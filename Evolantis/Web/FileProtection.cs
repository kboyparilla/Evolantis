using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Evolantis.Web
{
    public class FileProtection : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string requestedFile = context.Server.MapPath(context.Request.FilePath);
            SendContentTypeAndFile(context, requestedFile);
        }

        private HttpContext SendContentTypeAndFile(HttpContext context, String strFile)
        {
            context.Response.ContentType = GetContentType(strFile);
            context.Response.TransmitFile(strFile);
            context.Response.End();

            return context;
        }

        private string GetContentType(string filename)
        {
            FileInfo fileinfo = new FileInfo(filename);
            if (fileinfo.Exists)
            {
                return MimeTypes.GetMimeType(fileinfo.Extension);
            }

            return null;
        }
    }
}
