﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRasta.Configuration;
using TwitTailor.Handlers;

namespace TwitTailor
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<string>()
                .AtUri("/bft?item={item}")
                .HandledBy<BFTwitterHandler>()
                .AsJsonDataContract();              

            }
        }
    }
}
