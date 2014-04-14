using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using WebApi.OutputCache.V2;

using twg.chk.DataService.Business;
using twg.chk.DataService.api;

namespace twg.chk.DataService.FrontOffice.Controllers
{
    public class MediaController : ApiController
    {
        private static readonly Dictionary<String, String> IMAGE_MIME_TYPES = new Dictionary<String, String>
        {
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"bmp", "image/bmp"},
            {"gif", "image/gif"},
            {"jfif", "image/pjpeg"},
            {"png", "image/png"}
        };

        private IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        [Route("media/{id:int}/{fileName}", Name = "GetMedia")]
        [Authorize(Roles = "frontofficegroup")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public HttpResponseMessage GetMedia(int id, String fileName)
        {
            HttpResponseMessage message;
            var mediaContent = _mediaService.Get(id);
            if (mediaContent != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK);
                var stream = new MemoryStream(mediaContent.ContentBinary, 0, mediaContent.ContentBinary.Length, false);
                message.Content = new StreamContent(stream);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(IMAGE_MIME_TYPES[mediaContent.Extension]);
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return message;
        }
    }
}
