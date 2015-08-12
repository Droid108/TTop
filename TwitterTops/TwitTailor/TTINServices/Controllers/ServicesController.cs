using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Script.Serialization;
using TTINServices.Models;
using TTINServices.Utilities;
namespace TTINServices.Controllers
{
    public class ServicesController : ApiController
    {
        public const string Const_TwitterDateTemplate = "ddd MMM dd HH:mm:ss zzz yyyy";
        private TwitRapDBEntities _entityObj;

        public TwitRapDBEntities EntityObj
        {
            get
            {
                if (_entityObj == null)
                {
                    _entityObj = new TwitRapDBEntities();
                }
                return _entityObj;
            }
        }

        private TwitAuthenticateResponse twitAuthResponse;
        [Route("/services/iget")]
        public string Get()
        {
            try
            {
                return "true";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }


        private void createAuthToken()
        {
            try
            {
                var oAuthConsumerKey = "lpdiWZCCPkC5CSwdZJ91xLsrf";
                var oAuthConsumerSecret = "8cdgZjEfw8snsEqPV3ntPkYn5Klw432kroJCQFxEzqYJOo2bPY";
                var oAuthUrl = ConfigurationManager.AppSettings["TwitAuthAPI"].ToString();
                if (string.IsNullOrEmpty(oAuthUrl))
                    throw new Exception("Twitter AUTH API Url is not found or invalid");

                // Do the Authenticate
                var authHeaderFormat = "Basic {0}";

                var authHeader = string.Format(authHeaderFormat,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                    Uri.EscapeDataString((oAuthConsumerSecret)))
                ));

                var postBody = "grant_type=client_credentials";

                HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
                authRequest.Headers.Add("Authorization", authHeader);
                authRequest.Method = "POST";
                authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (Stream stream = authRequest.GetRequestStream())
                {
                    byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }

                authRequest.Headers.Add("Accept-Encoding", "gzip");

                WebResponse authResponse = authRequest.GetResponse();
                // deserialize into an object
                using (authResponse)
                {
                    using (var reader = new StreamReader(authResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var objectText = reader.ReadToEnd();
                        twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private string buildScreenNames(string screenName)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (string strs in screenName.Split(','))
            {
                if (i == 0)
                {
                    sb.Append("from:" + strs);
                }
                else
                {
                    sb.Append(" OR from:" + strs);
                }
                i++;
            }
            return sb.ToString();
        }

        private dynamic makeRequestForData(string scNames)
        {
            if (twitAuthResponse == null)
            {
                createAuthToken();
            }
            var timelineFormat = ConfigurationManager.AppSettings["TwitSearchAPI"].ToString();
            scNames = scNames.Replace("@", "");
            var timelineHeaderFormat = "{0} {1}";
            string screenname = buildScreenNames(scNames);
            if (string.IsNullOrEmpty(timelineFormat))
                throw new Exception("Twitter Search API Url is not found or invalid");
            var timelineUrl = string.Format(timelineFormat, screenname);
            timelineUrl = timelineUrl + "&count=500";
            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
            //    using (var response = await httpClient.GetAsync(timelineUrl))
            //    {
            //        string responseData = await response.Content.ReadAsStringAsync();
            //    }
            //}



            HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            timeLineRequest.Headers.Add("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
            timeLineRequest.Method = "Get";
            WebResponse timeLineResponse = timeLineRequest.GetResponse();
            var timeLineJson = string.Empty;
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                }
            }
            return JsonConvert.DeserializeObject(timeLineJson);
        }

        private string getCATLove()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_LOVE"].ToString();
                tblCatLove tblObj = null;

                dynamic jsonObj = makeRequestForData(scNames);
                foreach (JObject res in jsonObj.statuses)
                {
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatLove tbObj = EntityObj.tblCatLoves.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatLove();
                        tblObj.IsVerified = res["user"]["verified"].Value<bool>();
                        tblObj.location = res["user"]["location"].Value<string>();
                        tblObj.text = res["text"].Value<string>();
                        tblObj.TwitID = res["id"].Value<decimal>();
                        tblObj.userid = res["user"]["id"].Value<decimal>();
                        tblObj.name = res["user"]["name"].Value<string>();
                        tblObj.screenname = res["user"]["screen_name"].Value<string>();
                        tblObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                        tblObj.RT_Count = res["retweet_count"].Value<decimal>();
                        tblObj.Fav_Count = res["favorite_count"].Value<decimal>();
                        DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AdjustToUniversal);
                        tblObj.created_at = createdAt;
                        EntityObj.tblCatLoves.Add(tblObj);
                    }
                    else
                    {
                        tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                        tbObj.location = res["user"]["location"].Value<string>();
                        tbObj.text = res["text"].Value<string>();
                        tbObj.TwitID = res["id"].Value<decimal>();
                        tbObj.userid = res["user"]["id"].Value<decimal>();
                        tbObj.name = res["user"]["name"].Value<string>();
                        tbObj.screenname = res["user"]["screen_name"].Value<string>();
                        tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                        tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                        tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                        DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AdjustToUniversal);
                        tbObj.created_at = createdAt;
                        EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    }
                }
                EntityObj.SaveChanges();
                return "";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATLove", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATLove", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATAuto()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_AUTO"].ToString();
                tblCatAuto tblObj = null;

                dynamic jsonObj = makeRequestForData(scNames);
                foreach (JObject res in jsonObj.statuses)
                {
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatAuto tbObj = EntityObj.tblCatAutoes.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatAuto();
                        tblObj.IsVerified = res["user"]["verified"].Value<bool>();
                        tblObj.location = res["user"]["location"].Value<string>();
                        tblObj.text = res["text"].Value<string>();
                        tblObj.TwitID = res["id"].Value<decimal>();
                        tblObj.userid = res["user"]["id"].Value<decimal>();
                        tblObj.name = res["user"]["name"].Value<string>();
                        tblObj.screenname = res["user"]["screen_name"].Value<string>();
                        tblObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                        tblObj.RT_Count = res["retweet_count"].Value<decimal>();
                        tblObj.Fav_Count = res["favorite_count"].Value<decimal>();
                        DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AdjustToUniversal);
                        tblObj.created_at = createdAt;
                        EntityObj.tblCatAutoes.Add(tblObj);
                    }
                    else
                    {
                        tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                        tbObj.location = res["user"]["location"].Value<string>();
                        tbObj.text = res["text"].Value<string>();
                        tbObj.TwitID = res["id"].Value<decimal>();
                        tbObj.userid = res["user"]["id"].Value<decimal>();
                        tbObj.name = res["user"]["name"].Value<string>();
                        tbObj.screenname = res["user"]["screen_name"].Value<string>();
                        tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                        tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                        tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                        DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AdjustToUniversal);
                        tbObj.created_at = createdAt;
                        EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    }
                }
                EntityObj.SaveChanges();
                return "";
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATAuto", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATLove", "MANUAL", "");
                throw ex;
            }
        }

        private Exception getEFException(DbEntityValidationException dbEx)
        {
            Exception raise = dbEx;
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                    //raise a new exception inserting the current one as the InnerException
                    raise = new InvalidOperationException(message, raise);
                }
            }
            return raise;
        }
    }
    public class TwitAuthenticateResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }

}
