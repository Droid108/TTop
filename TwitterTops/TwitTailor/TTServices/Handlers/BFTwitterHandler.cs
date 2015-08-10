using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TTServices.Model;
using TTServices.Utilities;

namespace TwitTailor.Handlers
{
    public class BFTwitterHandler
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

        public string Get()
        {
            try
            {
                getCATLove();
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
                // You need to set your own keys and screen name
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

        private string getCATLove()
        {
            try
            {
                if (twitAuthResponse == null)
                {
                    createAuthToken();
                }
                string scNames = ConfigurationManager.AppSettings["CATLOVE"].ToString();
                scNames = scNames.Replace("@", "");
                string screenname = buildScreenNames(scNames);
                // Do the Search call
                var timelineFormat = ConfigurationManager.AppSettings["TwitSearchAPI"].ToString();
                if (string.IsNullOrEmpty(timelineFormat))
                    throw new Exception("Twitter Search API Url is not found or invalid");
                var timelineUrl = string.Format(timelineFormat, screenname);
                timelineUrl = timelineUrl + "&count=100";
                HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                var timelineHeaderFormat = "{0} {1}";
                timeLineRequest.Headers.Add("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
                timeLineRequest.Method = "Get";
                WebResponse timeLineResponse = timeLineRequest.GetResponse();
                var timeLineJson = string.Empty;
                tblCatLove tblObj = null;
                using (timeLineResponse)
                {
                    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                    {
                        timeLineJson = reader.ReadToEnd();
                    }
                }
                dynamic jsonObj = JsonConvert.DeserializeObject(timeLineJson);

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
