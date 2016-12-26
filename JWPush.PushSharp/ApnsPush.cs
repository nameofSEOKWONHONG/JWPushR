using JWPush.Models;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Google;
using PushSharp.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush
{
    public class ApnsPush
    {
        private ApnsCert _apnsCert;
        private ApnsConfiguration _config;
        private ApnsServiceBroker _apnsBroker;

        public ApnsPush(ApnsCert apnsCert)
        {
            if (_apnsCert == null) throw new Exception("Apns Certification is null.");

            _apnsCert = apnsCert;
        }

        public void Initialize()
        {
            if (_apnsCert == null) throw new Exception("Apns Certification is null.");

            _config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, _apnsCert.Cert, _apnsCert.CertPass);

            if (_config != null)
            {
                var apnsBroker = new ApnsServiceBroker(_config);

                _apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {
                    aggregateEx.Handle(ex =>
                    {
                        if (ex is ApnsNotificationException)
                        {
                            var notificationException = (ApnsNotificationException)ex;

                            var apnsNotification = notificationException.Notification;
                            var statusCode = notificationException.ErrorStatusCode;

                            Debug.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                        }
                        else
                        {
                            Debug.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        }

                        return true;
                    });
                };

                _apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    Debug.WriteLine("Apple Notification Sent!");
                };

                _apnsBroker.Start();
            }
        }

        public void Push(IList<Apns> apnsList)
        {
            foreach(var item in apnsList)
            {
                _apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = item.DeviceToken,
                    Payload = JObject.Parse(item.Payload),
                    Expiration = item.Expiration,
                    LowPriority = item.LowPriority,
                    Tag = item.Tag
                });
            }
        }
    }
}
