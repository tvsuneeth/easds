using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TWG.EASDataService.ClientTest.Core;
using TWG.EASDataService.ClientTest.Services;


namespace TWG.EASDataService.ClientTest.Services
{

    public interface IServiceRequester
    {
        object GetService(string baseUrl, string param);
        HttpWebResponse GetMediaContent(string baseUrl, string param);
        AuthToken GetToken(string baseUrl);
    }
    public class ServiceRequester : IServiceRequester
    {

        public object GetService(string baseUrl,string param)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                //If the request is for token, return generate a new token
                object result = null;
                if (param == "token")
                {
                    result = GenerateNewTokenFromService(baseUrl);
                }
                else
                {
                    var token = GetToken(baseUrl);
                    if (token == null || String.IsNullOrEmpty(token.Token))
                    {
                        return null;
                    }

                    string serviceUrl = baseUrl + param;
                    var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

                    result = MakeWebRequestAndReturnJasonObject(serviceUrl, "GET", headers, string.Empty);
                }
                return serializer.Deserialize<object>(result.ToString());
            }
            catch (Exception ex)
            {
               // return Json("Unable to process the request. Make sure you have set the correct endpoint Url and the requested resource exists", JsonRequestBehavior.AllowGet);
                return null;
            }
        }

        public HttpWebResponse GetMediaContent(string baseUrl, string param)
        {
            try
            {

                var token = GetToken(baseUrl);
                if (token == null || String.IsNullOrEmpty(token.Token))
                {
                    return null;                    
                }

                string serviceUrl = baseUrl + param;
                var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

                var response = MakeWebRequest(serviceUrl, "GET", headers, string.Empty);
               
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AuthToken GetToken(string baseUrl)
        {
            AuthToken token = null;
            var service = baseUrl.Remove(0, baseUrl.IndexOf("//") + 2);
            service = (service.EndsWith("/")) ? service.Remove(service.Length - 1, 1) : service;
            string sessionKey = "token_" + service;
            try
            {
                if (HttpContext.Current.Session[sessionKey] != null)
                {
                    token = (AuthToken)HttpContext.Current.Session[sessionKey];
                    if (DateTime.Now > token.ExpiryDate)
                    {
                        HttpContext.Current.Session[sessionKey] = token = BuildNewAuthToken(baseUrl);
                    }
                }
                else
                {
                    HttpContext.Current.Session[sessionKey] = token = BuildNewAuthToken(baseUrl);
                }
            }
            catch (Exception ex)
            {
                token = null;
            }
            return token;
        }

        public AuthToken BuildNewAuthToken(string baseUrl)
        {
            dynamic tokenJson = GenerateNewTokenFromService(baseUrl);

            var token = new AuthToken()
            {
                ExpiryDate = (DateTime)tokenJson[".expires"],
                Token = (String)tokenJson["access_token"],
            };
            return token;
        }

        public object GenerateNewTokenFromService(string baseUrl)
        {
            string grantType = "password";
            string username = GetConfigValue("username");
            string password = GetConfigValue("password");
            string tokenurl = baseUrl + "token";

            string body = String.Format("grant_type={0}&username={1}&password={2}", grantType, username, password);

            var obj = MakeWebRequestAndReturnJasonObject(tokenurl, "POST", null, body.ToString());
            return obj;
        }

        public object MakeWebRequestAndReturnJasonObject(string url, string method, Dictionary<string, string> header, string data)
        {
            var response = MakeWebRequest(url, method, header, data);
            string json = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                // Get the Response Stream
                json = reader.ReadLine();
                var result = JsonConvert.DeserializeObject<object>(json);
                return result;
            }
        }

        public HttpWebResponse MakeWebRequest(string url, string method, Dictionary<string, string> header, string data)
        {
            HttpWebResponse response = null;
            // Setup the Request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            if (!String.IsNullOrEmpty(data))
            {
                // Create a byte array of the data to be sent
                byte[] byteArray = Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteArray.Length;
                // Write data
                Stream postStream = request.GetRequestStream();
                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();
            }
            if (header != null && header.Count > 0)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            // Send Request & Get Response
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response;
        }


        public string GetConfigValue(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}