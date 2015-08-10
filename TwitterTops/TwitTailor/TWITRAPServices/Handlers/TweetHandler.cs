using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TWITRAPServices.Model;

namespace TWITRAPServices.Handlers
{
    public class TweetHandler
    {
        private TweetServicesDataContext instance;
        private TweetServicesDataContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TweetServicesDataContext();
                }
                return instance;
            }
        }
        public List<USP_FetchCATLoveTweetsResult> get(int fType, int fromId)
        {
            List<USP_FetchCATLoveTweetsResult> _lists = Instance.USP_FetchCATLoveTweets(fType, fromId).ToList();
            return _lists;
        }

    }
}