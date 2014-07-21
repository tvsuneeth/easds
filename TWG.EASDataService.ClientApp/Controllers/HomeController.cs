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

namespace TWG.EASDataService.ClientApp.Controllers
{
    public class HomeController : Controller
    {
        public string baseUrl;

        public HomeController()
        {
            baseUrl = GetConfigValue("serviceUrl");
        }

        public JsonResult GetService(string param)
        {
            //If the request is for token, return generate a new token
            if (param == "token")
            {
                return Json(GenerateNewTokenFromService(), JsonRequestBehavior.AllowGet);
            }

            var token = GetToken();
            if (token == null || String.IsNullOrEmpty(token.Token))           
            {
                return Json("Error!!!! unable to get a valid token, try again and if it fails, see generating a new token works ", JsonRequestBehavior.AllowGet);
            }

            string serviceUrl = baseUrl + param;
            var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

            var obj = MakeWebRequestAndReturnJasonObject(serviceUrl, "GET", headers, string.Empty);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Json(serializer.Deserialize<object>(obj.ToString()), JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult GetMediaContent(string id)
        {           
            var token = GetToken();
            if (token == null || String.IsNullOrEmpty(token.Token))
            {
                return Json("Error!!!! unable to get a valid token, try again and if it fails, see generating a new token works ", JsonRequestBehavior.AllowGet);
            }

            string serviceUrl = baseUrl + "mediacontent/"+id;
            var headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token.Token } };

            var response = MakeWebRequest(serviceUrl, "GET", headers, string.Empty);

            Stream respStr = response.GetResponseStream();
            MemoryStream stream = new MemoryStream();
            respStr.CopyTo(stream);                                    

            return File(stream.ToArray(), response.ContentType);            
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

            string body = String.Format("grant_type={0}&username={1}&password={2}",grantType,username,password);

            var obj = MakeWebRequestAndReturnJasonObject(tokenurl, "POST", null, body.ToString());
            return obj;
        }

        public object MakeWebRequestAndReturnJasonObject(string url, string method, Dictionary<string, string> header, string data)
        {
            var response = MakeWebRequest(url, method, header,data);
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
            if(header!=null && header.Count>0)
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
