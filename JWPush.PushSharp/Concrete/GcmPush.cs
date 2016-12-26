using JWPush.Abstract;
using JWPush.Infrastructure;
using JWPush.Models;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush.Concrete
{
    public class GcmPush : Disposable, IJWPush
    {
        public GcmConfiguration _config;
        public GcmServiceBroker _broker;

        public GcmPush(GcmCert gcmCert)
        {
            if (gcmCert == null) throw new Exception("Gcm Certification is null.");

            _config = new GcmConfiguration(gcmCert.GcmSenderId,
                gcmCert.AuthToken,
                gcmCert.OptionalApplicationIdPackageName);
        }

        public void Initialize()
        {
            if (_config == null) throw new Exception("config is null.");

            _broker = new GcmServiceBroker(_config);

            _broker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is GcmNotificationException)
                    {
                        var notificationException = (GcmNotificationException)ex;

                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;

                        Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        var multicastException = (GcmMulticastResultException)ex;

                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            Console.WriteLine($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;

                            Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Message}");
                        }
                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        var expiredException = (DeviceSubscriptionExpiredException)ex;

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException)
                    {
                        var retryException = (RetryAfterException)ex;
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        Console.WriteLine("GCM Notification Failed for some unknown reason");
                    }

                    return true;
                });
            };

            _broker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine($"GCM Notification Sent! {notification}");
            };

            _broker.Start();
        }

        public void Push<T>(IEnumerable<T> pushList)
        {
            IEnumerable<Gcm> gcmList = (IEnumerable<Gcm>)pushList;

            if(gcmList != null)
            {
                foreach(var item in gcmList)
                {
                    _broker.QueueNotification(new GcmNotification
                    {
                        RegistrationIds = item.RegistrationIds,
                        Data = JObject.Parse(item.Data),
                        CollapseKey = item.CollapseKey,
                        ContentAvailable = item.ContentAvailable,
                        DelayWhileIdle = item.DelayWhileIdle,
                        DryRun = item.DryRun,
                        Notification = JObject.Parse(item.Notification),
                        //NotificationKey = item.NotificationKey, //no more use;
                        Priority = (GcmNotificationPriority?)item.Priority,
                        RestrictedPackageName = item.RestrictedPackageName,
                        TimeToLive = item.TimeToLive,
                        Tag = item.Tag,
                        To = item.To
                    });
                }
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
