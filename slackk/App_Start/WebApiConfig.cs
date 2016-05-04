using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Routing;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using slackk.Models;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Net;

namespace slackk
{
    public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private NameValueCollection _formData = new NameValueCollection();
        private List<HttpContent> _fileContents = new List<HttpContent>();


        private Collection<bool> _isFormData = new Collection<bool>();

        public NameValueCollection FormData
        {
            get { return _formData; }
        }


        public List<HttpContent> Files
        {
            get { return _fileContents; }
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {

            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {

                _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                return new MemoryStream();
            }

            throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
        }

        public override async Task ExecutePostProcessingAsync()
        {

            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];

                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    _fileContents.Add(Contents[index]);
                }
            }
        }

        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }

    public class MultipartFormFormatter : FormUrlEncodedMediaTypeFormatter
    {
        private const string StringMultipartMediaType = "multipart/form-data";

        public MultipartFormFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue(StringMultipartMediaType));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            try
            {
                var provider = await content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;
                HttpContent file1 = files[0];
                Stream file1Stream = await file1.ReadAsStreamAsync();
                var file = ReadFully(file1Stream);
                var CrowMessage = new CrowMessage()
                {
                    File = file,
                    Channel = formData.Get("channel"),
                    CrowToken = formData.Get("token"),
                    FileName = formData.Get("filename"),
                    Text = formData.Get("text")
                };
                return CrowMessage;
            }
            catch
            {
                return new CrowMessage()
                {
                    Text = "Please remove the multipartfromdata header from the requst and also make sure you have attached a file"
                };
            }
        }

        private byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    //public class FileModel
    //{
    //    public FileModel(string filename, int contentLength, byte[] content)
    //    {
    //        Filename = filename;
    //        ContentLength = contentLength;
    //        Content = content;
    //    }

    //    public string Filename { get; set; }

    //    public int ContentLength { get; set; }

    //    public byte[] Content { get; set; }

    //}
    public static class WebApiConfig
    {


        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();

            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //config.Formatters.Clear
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            // Web API routes
            config.Formatters.Add(new MultipartFormFormatter());

            config.MapHttpAttributeRoutes();
            //GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //    name: "Crow Api",
            //    routeTemplate: "api/{controller}/"
            //    );

            //config.Routes.MapHttpRoute(

            //);

            //config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
        }
    }
}
