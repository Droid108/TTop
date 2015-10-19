using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

        public string Get(int id)
        {
            //dataDumper(id);
            return "true";
        }

        private TwitAuthenticateResponse twitAuthResponse;
        private string resultString = string.Empty;
        public string Get()
        {
            try
            {
                //connectionString = EntityObj.Database.Connection.ConnectionString;
                getCATLove();
                Thread.Sleep(1000);                
                getCATAuto();
                Thread.Sleep(1000);
                getCATBusiness();
                Thread.Sleep(1000);
                getCATFacts();
                Thread.Sleep(1000);
                getCATScience();
                Thread.Sleep(1000);
                getCATJokes();
                Thread.Sleep(1000);
                getCATTechnology();
                Thread.Sleep(1000);
                getCATSports();
                Thread.Sleep(1000);
                getCATNews();
                //Thread.Sleep(1000);
                //getCATAuto();
                ////dataDumper(1);
                return resultString;
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        int i;
        private void dataDumper(int categoryId)
        {
            try
            {
                i = 0;
                //finalUrlString = null;
                for (i = 0; i < 100; i++)
                {
                    switch (categoryId)
                    {
                        case 1:
                            getCATLove();
                            break;
                        case 2:
                            getCATAuto();
                            break;
                        case 3:
                            getCATBusiness();
                            break;
                        case 4:
                            getCATFacts();
                            break;
                        case 5:
                            getCATScience();
                            break;
                        case 6:
                            getCATJokes();
                            break;
                        case 7:
                            getCATTechnology();
                            break;
                        case 8:
                            getCATSports();
                            break;
                        case 9:
                            getCATNews();
                            break;
                        default:
                            getCATLove();
                            break;
                    }
                    Thread.Sleep(1000);
                }
                i = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        string connectionString = "";

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
        //private string finalUrlString;
        private dynamic makeRequestForData(string scNames)
        {
            if (twitAuthResponse == null)
            {
                createAuthToken();
            }
            var timelineFormat = ConfigurationManager.AppSettings["TwitSearchAPI"].ToString();
            scNames = scNames.Replace("@", "");
            string screenname = buildScreenNames(scNames);
            if (string.IsNullOrEmpty(timelineFormat))
                throw new Exception("Twitter Search API Url is not found or invalid");
            var timelineUrl = "";
            //if (finalUrlString != null)
            //{
            //    screenname = finalUrlString;
            //    timelineFormat = timelineFormat.Replace("?q=", "");
            //}

            timelineUrl = string.Format(timelineFormat, screenname);
            //if (finalUrlString == null)
                timelineUrl = timelineUrl + "&count=200";
            HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            var timelineHeaderFormat = "{0} {1}";
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
        string media_url = null;
        string media_type = null;
        string Url = null;
        string display_url = null;
        string expanded_url = null;
        private string getCATLove()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_LOVE"].ToString();
                tblCatLove tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    //connectionString = EntityObj.Database.Connection.ConnectionString.ToString();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatLoves.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATLove &nbsp;<br/>";
                return resultString;
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

        private string getCATFacts()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_FACTS"].ToString();
                tblCatFact tblObj = null;

                dynamic jsonObj = makeRequestForData(scNames);
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatFact tbObj = EntityObj.tblCatFacts.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatFact();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatFacts.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATFacts &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATFacts", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATFacts", "MANUAL", "");
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
                //if (jsonObj.search_metadata != null && jsonObj.search_metadata["next_results"] != null)
                //{
                //   finalUrlString = jsonObj.search_metadata["next_results"].Value;
                //}
                    
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatAutoes.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATAuto &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATAuto", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATAuto", "MANUAL", "");
                throw ex;
            }
        }

        private string getCATJokes()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_JOKES"].ToString();
                tblCatJoke tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatJoke tbObj = EntityObj.tblCatJokes.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatJoke();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatJokes.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATJokes &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATJokes", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATJokes", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATScience()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_SCIENCE"].ToString();
                tblCatScience tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                string media_url = null;
                string media_type = null;
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatScience tbObj = EntityObj.tblCatSciences.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatScience();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatSciences.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "tblCatScience &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "tblCatScience", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "tblCatScience", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATSports()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_SPORTS"].ToString();
                tblCatSport tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                string media_url = null;
                string media_type = null;
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatSport tbObj = EntityObj.tblCatSports.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatSport();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatSports.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATSports &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATSports", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATSports", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATNews()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_NEWS"].ToString();
                tblCatNew tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatNew tbObj = EntityObj.tblCatNews.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatNew();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatNews.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "getCATNews &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "getCATNews", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "getCATNews", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATBusiness()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_BUSINESS"].ToString();
                tblCatBusiness tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);
                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatBusiness tbObj = EntityObj.tblCatBusinesses.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatBusiness();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatBusinesses.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "tblCatBusiness &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "tblCatBusiness", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "tblCatBusiness", "MANUAL", "");
                throw ex;
            }

        }

        private string getCATTechnology()
        {
            try
            {
                string scNames = ConfigurationManager.AppSettings["CAT_TECHNOLOGY"].ToString();
                tblCatTech tblObj = null;
                dynamic jsonObj = makeRequestForData(scNames);

                //if (jsonObj.search_metadata != null)
                //    finalUrlString = jsonObj.search_metadata["next_results"].Value;
                foreach (JObject res in jsonObj.statuses)
                {
                    media_url = string.Empty;
                    media_type = string.Empty;
                    Url = string.Empty;
                    display_url = string.Empty;
                    expanded_url = string.Empty;
                    if (res["entities"]["media"] != null)
                    {
                        JArray jObjects = res["entities"]["media"].Value<JArray>();
                        media_url = jObjects[0]["media_url"] == null ? "" : jObjects[0]["media_url"].Value<string>();
                        media_type = jObjects[0]["type"] == null ? "" : jObjects[0]["type"].Value<string>();
                        Url = jObjects[0]["url"] == null ? "" : jObjects[0]["url"].Value<string>();
                        display_url = jObjects[0]["display_url"] == null ? "" : jObjects[0]["display_url"].Value<string>();
                        expanded_url = jObjects[0]["expanded_url"] == null ? "" : jObjects[0]["expanded_url"].Value<string>();
                    }
                    decimal tweetId = res["id"].Value<decimal>();
                    tblCatTech tbObj = EntityObj.tblCatTeches.Where(x => x.TwitID.Equals(tweetId)).FirstOrDefault();
                    if (tbObj == null)
                    {
                        tblObj = new tblCatTech();
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
                        if (media_url.Length > 0)
                            tblObj.MediaUrl = media_url;
                        if (media_type.Length > 0)
                            tblObj.MediaType = media_type;
                        if (Url.Length > 0)
                            tblObj.Url = Url;
                        if (display_url.Length > 0)
                            tblObj.display_url = display_url;
                        if (expanded_url.Length > 0)
                            tblObj.expanded_url = expanded_url;
                        EntityObj.tblCatTeches.Add(tblObj);
                    }
                    //else
                    //{
                    //    tbObj.IsVerified = res["user"]["verified"].Value<bool>();
                    //    tbObj.location = res["user"]["location"].Value<string>();
                    //    tbObj.text = res["text"].Value<string>();
                    //    tbObj.TwitID = res["id"].Value<decimal>();
                    //    tbObj.userid = res["user"]["id"].Value<decimal>();
                    //    tbObj.name = res["user"]["name"].Value<string>();
                    //    tbObj.screenname = res["user"]["screen_name"].Value<string>();
                    //    tbObj.ProfileUrl = res["user"]["profile_image_url"].Value<string>();
                    //    tbObj.RT_Count = res["retweet_count"].Value<decimal>();
                    //    tbObj.Fav_Count = res["favorite_count"].Value<decimal>();
                    //    DateTime createdAt = DateTime.ParseExact((string)res["created_at"], Const_TwitterDateTemplate, CultureInfo.InvariantCulture,
                    //              DateTimeStyles.AdjustToUniversal);
                    //    tbObj.created_at = createdAt;
                    //    if (media_url.Length >= 0)
                    //        tbObj.MediaUrl = media_url;
                    //    if (media_type.Length >= 0)
                    //        tbObj.MediaType = media_type;
                    //    if (Url.Length >= 0)
                    //        tbObj.Url = Url;
                    //    if (display_url.Length >= 0)
                    //        tbObj.display_url = display_url;
                    //    if (expanded_url.Length >= 0)
                    //        tbObj.expanded_url = expanded_url;
                    //    EntityObj.Entry(tbObj).State = System.Data.EntityState.Modified;
                    //}
                }
                EntityObj.SaveChanges();
                resultString += "tblCatTech &nbsp;<br/>";
                return resultString;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                ExceptionHandler.SendExceptionEmail(dbEx, "tblCatTech", "MANUAL", "");
                throw getEFException(dbEx);
            }
            catch (Exception ex)
            {

                ExceptionHandler.SendExceptionEmail(ex, "tblCatTech", "MANUAL", "");
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


    //    --Truncate table tblcatauto
    //--sp_help 'tblcatauto'

    //--ALter table tblcatauto ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatlove ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatbusiness ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatfacts ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatgadgets ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatHF ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatJokes ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatMovies ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatmusic ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatnews ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcatsports ALTER COLUMN text Nvarchar(500)
    //--ALter table tblcattravel ALTER COLUMN text Nvarchar(500)

    //    Alter table tblCatLove Add Url Nvarchar(300)
    //Alter table tblCatLove Add display_url Nvarchar(300)
    //Alter table tblCatLove Add expanded_url Nvarchar(500)

    //Alter table tblCatAuto Add Url Nvarchar(300)
    //Alter table tblCatAuto Add display_url Nvarchar(300)
    //Alter table tblCatAuto Add expanded_url Nvarchar(500)

    //Alter table tblCatBusiness Add Url Nvarchar(300)
    //Alter table tblCatBusiness Add display_url Nvarchar(300)
    //Alter table tblCatBusiness Add expanded_url Nvarchar(500)

    //Alter table tblCatFacts Add Url Nvarchar(300)
    //Alter table tblCatFacts Add display_url Nvarchar(300)
    //Alter table tblCatFacts Add expanded_url Nvarchar(500)

    //Alter table tblCatScience Add Url Nvarchar(300)
    //Alter table tblCatScience Add display_url Nvarchar(300)
    //Alter table tblCatScience Add expanded_url Nvarchar(500)

    //Alter table tblCatJokes Add Url Nvarchar(300)
    //Alter table tblCatJokes Add display_url Nvarchar(300)
    //Alter table tblCatJokes Add expanded_url Nvarchar(500)

    //Alter table tblCatTech Add Url Nvarchar(300)
    //Alter table tblCatTech Add display_url Nvarchar(300)
    //Alter table tblCatTech Add expanded_url Nvarchar(500)

    //Alter table tblCatSports Add Url Nvarchar(300)
    //Alter table tblCatSports Add display_url Nvarchar(300)
    //Alter table tblCatSports Add expanded_url Nvarchar(500)

    //Alter table tblCatNews Add Url Nvarchar(300)
    //Alter table tblCatNews Add display_url Nvarchar(300)
    //Alter table tblCatNews Add expanded_url Nvarchar(500)
    public class TwitAuthenticateResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }

//    select count( *) from tblcatlove where url is not null and Len(Url) > 0

//select count( *) from tblcatAuto where url is not null and Len(Url) > 0

//select count( *) from tblcatBusiness where url is not null and Len(Url) > 0

//select count( *) from tblcatFacts where url is not null and Len(Url) > 0

//select count( *) from tblcatScience where url is not null and Len(Url) > 0

//select count( *) from tblcatJokes where url is not null and Len(Url) > 0

//select count( *) from tblcatTech where url is not null and Len(Url) > 0

//select count( *) from tblcatSports where url is not null and Len(Url) > 0

//select count( *) from tblcatNews where url is not null and Len(Url) > 0
}
