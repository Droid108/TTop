using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TTServices
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterTweetGetterService("DoTask", 60);
        }

        public void DoTask()
        {
            string URI = "http://iserv.tweetrap.com/iget";
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
        }

        private void RegisterTweetGetterService(string name, int seconds)
        {

            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, OnCacheRemove);

        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            DoTask();
            RegisterTweetGetterService(k, Convert.ToInt32(v));
        }
    }
}