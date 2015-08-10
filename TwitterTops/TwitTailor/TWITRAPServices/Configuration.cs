using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenRasta.Configuration;
using TWITRAPServices.Handlers;
using TWITRAPServices.Model;

namespace TWITRAPServices
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<List<USP_FetchCATLoveTweetsResult>>()
                .AtUri("/oget?fType={fType}&fromId={fromId}")
                .HandledBy<TweetHandler>()
                .AsJsonDataContract();
            }
        }
    }
}