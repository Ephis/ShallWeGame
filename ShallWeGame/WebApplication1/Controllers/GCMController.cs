using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PushSharp.Google;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GCMController
    {
        private static String apiKey = "AIzaSyARrfdLE2VW9RYgzn3wDqN4CfdTRVkCyrs";
        private GcmServiceBroker pushBroker;

        private GCMController()
        {
            pushBroker = new GcmServiceBroker(new GcmConfiguration(apiKey));
        }

        
        public void SendFriendNewRequestMessage(FriendRequest request, string deviceId)
        {
            pushBroker.Start();
            GcmNotification notification = new GcmNotification
            {
                Data = JObject.FromObject(request),
                To = deviceId
            };
            pushBroker.QueueNotification(notification);
            pushBroker.Stop();

        }
    }
}