using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DemoBeacon.Models;

namespace DemoBeacon.Controllers
{
    public class LogsController : ApiController
    {

        [HttpGet]
        public string PutLog(int logtype, string deviceName, string zoneName)
        {
            try
            {
                tblDemoBeacon beaconObj = new tblDemoBeacon();
                beaconObj.DeviceName = deviceName;
                beaconObj.ZoneName = zoneName;
                beaconObj.LogTime = DateTime.Now;
                beaconObj.Type = logtype;
                TwitRapDBEntities context = new TwitRapDBEntities();
                context.tblDemoBeacons.Add(beaconObj);
                context.SaveChanges();
                return "true";
            }
            catch (Exception ex )
            {

                return "Error in application: " + ex;
            }
        }

       
    }
}
