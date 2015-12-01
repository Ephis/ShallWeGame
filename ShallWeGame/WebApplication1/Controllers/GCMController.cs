using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class GCMController
    {
        private static String apiKey = "AIzaSyAxJqF3Wk9m8nCvLVOlllClcYpgYXCUWZA";


        [HttpPost]
        public void SendMessage(string message, string deviceId)
        {
            PushBroker pushBroker = new PushBroker();
            pushBroker.RegisterGcmService(new GcmPushChannelSettings(apiKey));
            pushBroker.QueueNotification(new GcmNotification().ForDeviceRegistrationId(device.RegistrationId)
                .WithJson(@"{""message"":""" + message + @"""}"));
            pushBroker.StopAllServices();

        }
    }
}