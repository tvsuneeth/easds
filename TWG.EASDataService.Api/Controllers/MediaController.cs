﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using WebApi.OutputCache.V2;
using TWG.EASDataService.Api.Models;
using TWG.EASDataService.Business;
using TWG.EASDataService.Services;
using TWG.EASDataService.Api.Extensions;

namespace TWG.EASDataService.Api.Controllers
{
    public class MediaController : ApiController
    {
        private static readonly Dictionary<String, String> MIME_TYPES = new Dictionary<String, String>
        {
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"bmp", "image/bmp"},
            {"gif", "image/gif"},
            {"jfif", "image/pjpeg"},
            {"png", "image/png"},
            {"pdf", "application/pdf"},
            {"swf", "application/x-shockwave-flash"},
            {"doc", "application/msword"}
        };

        private IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        [Route("media/{id:int}/{fileName}", Name = "GetMedia")]
        [Authorize(Roles = "frontofficegroup")]
       // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public HttpResponseMessage GetMedia(int id, String fileName)
        {
            HttpResponseMessage message;
            var mediaContent = _mediaService.Get(id);
            if (mediaContent != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK);
                var stream = new MemoryStream(mediaContent.ContentBinary, 0, mediaContent.ContentBinary.Length, false);
                message.Content = new StreamContent(stream);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(MIME_TYPES[mediaContent.Extension]);
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return message;
        }

        [HttpGet]
        [Route("mediacontent/{id:int}", Name = "GetMediaContentById")]
        [Authorize(Roles = "frontofficegroup")]
        // [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 3600, AnonymousOnly = false)]
        public HttpResponseMessage GetMediaContentById(int id)
        {
            HttpResponseMessage message;
            var mediaContent = _mediaService.Get(id);
            if (mediaContent != null)
            {
                if (mediaContent.Type!=MediaContentType.Image)
                {
                    message = Request.CreateResponse(HttpStatusCode.OK);
                    message.Content = new ByteArrayContent(mediaContent.ContentBinary);                    
                }
                else
                {
                    message = Request.CreateResponse(HttpStatusCode.OK);
                    var stream = new MemoryStream(mediaContent.ContentBinary, 0, mediaContent.ContentBinary.Length, false);
                    message.Content = new StreamContent(stream);
                    
                }
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(MIME_TYPES[mediaContent.Extension]);
                
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return message;
        }

        [HttpGet]
        [Route("mediacontent/{id:int}/info", Name = "GetMediaContentInfo")]
        [Authorize(Roles = "frontofficegroup")]        
        public MediaContent GetMediaContentInfo(int id)
        {
            
            var mediaContent = _mediaService.Get(id);
            return mediaContent;
        }


        [HttpGet]
        [Route("mediacontent/modifiedsince/{dateString:regex(\\d{6}_\\d{6})}", Name = "GetMediaContentChangedSince")]
        [Authorize(Roles = "frontofficegroup")]        
        //[CacheOutput(NoCache=true  )]
        public List<ModifiedItem>  GetModifiedMediaContentItems (string dateString)
        {
            //date format should be yyyymmdd_hhmmss
            DateTime dt = dateString.GetDateFromString();
            
            return _mediaService.GetMediaContentItemsModifiedSince(dt);            
        }
            

    }
}
