using JWPush.Abstract;
using JWPush.Infrastructure;
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

namespace JWPush.Concrete
{
    public class ApnsPush : Disposable, IJWPush
    {        
        private ApnsConfiguration _config;
        private ApnsServiceBroker _broker;

        public ApnsPush(ApnsCert apnsCert)
        {
            if (apnsCert == null) throw new Exception("Apns Certification is null.");

            _config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, apnsCert.Cert, apnsCert.CertPass);
        }

        public void Initialize()
        {            
            if (_config == null) throw new Exception("config is null.");

            _broker = new ApnsServiceBroker(_config);

            _broker.OnNotificationFailed += (notification, aggregateEx) =>
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

            _broker.OnNotificationSucceeded += (notification) =>
            {
                Debug.WriteLine("Apple Notification Sent!");
            };

            _broker.Start();
        }

        public void Push<T>(IEnumerable<T> pushList)
        {
            IEnumerable<Apns> apnsList = (IEnumerable<Apns>)pushList;

            if(apnsList != null)
            {
                foreach (var item in apnsList)
                {
                    _broker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = item.DeviceToken,
                        Payload = JObject.Parse(item.Payload),
                        Expiration = item.Expiration,
                        LowPriority = item.LowPriority,
                        Tag = item.Tag
                    });
                }
            }
            else
            {
                Console.WriteLine("push list is empty.");
            }
        }

        protected override void DisposeCore()
        {
            if(_broker != null)
            {
                _broker.Stop();
                _broker = null;
            }

            if(_config != null)
            {
                _config = null;
            }
            
            base.DisposeCore();
        }
    }
}
