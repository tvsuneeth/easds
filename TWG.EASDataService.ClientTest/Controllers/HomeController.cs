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
using TWG.EASDataService.ClientTest.Services;

namespace TWG.EASDataService.ClientTest.Controllers
{
    public class HomeController : Controller
    {
        public string baseUrl;
        public EndPointService endPointService;
        public HomeController()
        {
            endPointService = new EndPointService();  
        }

        public ActionResult GetService(string param)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                baseUrl = endPointService.GetEndPointUrl();
                if (baseUrl == string.Empty)
                {
                    return RedirectToAction("Index", "EndPoint");
                }

                //If the request is for token, return generate a new token
                object result = null;
                if (param == "token")
                {
                    result = GenerateNewTokenFromService();
                }
                else
                {
                    var token = GetToken();
                    if (token == null || String.IsNullOrEmpty(token.Token))
                    {
                        return Json("Error!!!!  unable to get a valid token. 1) try again and if it fails, see generating a new token works by requesting the token service method. 2) Make sure you have set the corret endpoint url ", JsonRequestBehavior.AllowGet);
                    }

                    string serviceUrl = baseUrl + param;
                    var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

                    result = MakeWebRequestAndReturnJasonObject(serviceUrl, "GET", headers, string.Empty);                   
                }
                return Json(serializer.Deserialize<object>(result.ToString()), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Unable to process. Make sure you have set the correct endpoint Url");
            }
            
            

        }

        public ActionResult GetMediaContent(string id)
        {
            try
            {
                baseUrl = endPointService.GetEndPointUrl();
                if (baseUrl == string.Empty)
                {
                    return RedirectToAction("Index", "EndPoint");
                }

                var token = GetToken();
                if (token == null || String.IsNullOrEmpty(token.Token))
                {
                    return Json("Error!!!!  unable to get a valid token. 1) try again and if it fails, see generating a new token works by requesting the token service method. 2) Make sure you have set the corret endpoint url ", JsonRequestBehavior.AllowGet);
                }

                string serviceUrl = baseUrl + "mediacontent/" + id;
                var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

                var response = MakeWebRequest(serviceUrl, "GET", headers, string.Empty);

                Stream respStr = response.GetResponseStream();
                MemoryStream stream = new MemoryStream();
                respStr.CopyTo(stream);

                return File(stream.ToArray(), response.ContentType);
            }
            catch (Exception ex)
            {
                return Json("Unable to process. Make sure you have set the correct endpoint Url");
            }
        }

        public AuthToken GetToken()
        {
            AuthToken token = null;
            try
            {
                if (Session["token"] != null)
                {
                    token = (AuthToken)Session["token"];
                    if (DateTime.Now > token.ExpiryDate)
                    {
                        Session["token"] = token = BuildNewAuthToken();
                    }
                }
                else
                {
                    Session["token"] = token = BuildNewAuthToken();
                }
            }
            catch (Exception ex)
            {
                token = null;
            }
            return token;
        }

        public AuthToken BuildNewAuthToken()
        {
            dynamic tokenJson = GenerateNewTokenFromService();
            var token = new AuthToken()
            {
                ExpiryDate = (DateTime)tokenJson[".expires"],
                Token = (String)tokenJson["access_token"]

            };
            return token;
        }

        public object GenerateNewTokenFromService()
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

    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
