namespace Beyova.Web
{
    /// <summary>
    /// Class ContentTypeMap.
    /// </summary>
    public static class ContentTypeMap
    {
        /// <summary>
        /// Maps the type of the extension to content.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static string MapExtensionToContentType(string extension)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(extension))
            {
                switch (extension.SubStringBeforeLastMatch('.').ToLowerInvariant())
                {
                    case "apk":
                        result = HttpConstants.ContentType.Apk;
                        break;

                    case "css":
                        result = HttpConstants.ContentType.Css;
                        break;

                    case "gif":
                        result = HttpConstants.ContentType.GifImage;
                        break;

                    case "html":
                        result = HttpConstants.ContentType.Html;
                        break;

                    case "ipa":
                        result = HttpConstants.ContentType.Ipa;
                        break;

                    case "js":
                        result = HttpConstants.ContentType.JavaScript;
                        break;

                    case "jpeg":
                    case "jpg":
                        result = HttpConstants.ContentType.JpegImage;
                        break;

                    case "json":
                        result = HttpConstants.ContentType.Json;
                        break;

                    case "mp3":
                        result = HttpConstants.ContentType.Mp3Audio;
                        break;

                    case "mp4":
                        result = HttpConstants.ContentType.Mp4Video;
                        break;

                    case "pdf":
                        result = HttpConstants.ContentType.Pdf;
                        break;

                    case "png":
                        result = HttpConstants.ContentType.PngImage;
                        break;

                    case "txt":
                        result = HttpConstants.ContentType.Text;
                        break;

                    case "xml":
                        result = HttpConstants.ContentType.Xml;
                        break;

                    case "zip":
                        result = HttpConstants.ContentType.ZipFile;
                        break;

                    case "md":
                        result = HttpConstants.ContentType.Markdown;
                        break;

                    case "xls":
                        result = HttpConstants.ContentType.Excel;
                        break;

                    case "xlsx":
                        result = HttpConstants.ContentType.ExcelX;
                        break;

                    case "ppt":
                        result = HttpConstants.ContentType.PowerPoint;
                        break;

                    case "pptx":
                        result = HttpConstants.ContentType.PowerPointX;
                        break;

                    case "doc":
                        result = HttpConstants.ContentType.Word;
                        break;

                    case "docx":
                        result = HttpConstants.ContentType.WordX;
                        break;

                    default:
                        result = HttpConstants.ContentType.BinaryDefault;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Maps the content type to extension.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static string MapContentTypeToExtension(string contentType)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                switch (contentType.ToLowerInvariant())
                {
                    case HttpConstants.ContentType.Apk:
                        result = "apk";
                        break;

                    case HttpConstants.ContentType.Css:
                        result = "css";
                        break;

                    case HttpConstants.ContentType.GifImage:
                        result = "gif";
                        break;

                    case HttpConstants.ContentType.Html:
                        result = "html";
                        break;

                    case HttpConstants.ContentType.Ipa:
                        result = "ipa";
                        break;

                    case HttpConstants.ContentType.JavaScript:
                        result = "js";
                        break;

                    case HttpConstants.ContentType.JpegImage:
                        result = "jpg";
                        break;

                    case HttpConstants.ContentType.Json:
                        result = "json";
                        break;

                    case HttpConstants.ContentType.Mp3Audio:
                        result = "mp3";
                        break;

                    case HttpConstants.ContentType.Mp4Video:
                        result = "mp4";
                        break;

                    case HttpConstants.ContentType.Pdf:
                        result = "pdf";
                        break;

                    case HttpConstants.ContentType.PngImage:
                        result = "png";
                        break;

                    case HttpConstants.ContentType.Text:
                        result = "txt";
                        break;

                    case HttpConstants.ContentType.Xml:
                        result = "xml";
                        break;

                    case HttpConstants.ContentType.ZipFile:
                        result = "zip";
                        break;

                    case HttpConstants.ContentType.Markdown:
                        result = "md";
                        break;

                    case HttpConstants.ContentType.Excel:
                        result = "xls";
                        break;

                    case HttpConstants.ContentType.ExcelX:
                        result = "xlsx";
                        break;

                    case HttpConstants.ContentType.PowerPoint:
                        result = "ppt";
                        break;

                    case HttpConstants.ContentType.PowerPointX:
                        result = "pptx";
                        break;

                    case HttpConstants.ContentType.Word:
                        result = "doc";
                        break;

                    case HttpConstants.ContentType.WordX:
                        result = "docx";
                        break;

                    default:
                        break;
                }
            }

            return result;
        }
    }
}