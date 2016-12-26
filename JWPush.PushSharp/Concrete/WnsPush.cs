using JWPush.Abstract;
using JWPush.Infrastructure;
using JWPush.Models;
using PushSharp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JWPush.Concrete
{
    public class WnsPush : Disposable, IJWPush
    {
        private WnsConfiguration _config;
        private WnsServiceBroker _broker;
        
        public WnsPush(WnsCert wnsCert)
        {
            if (wnsCert == null) throw new Exception("WNS CERT is null");

            _config = new WnsConfiguration(wnsCert.WnsPackageName,
                wnsCert.WnsPackageSid,
                wnsCert.WnsClientSecret);
        }

        public void Initialize()
        {
            if (_config == null) throw new Exception("config is null.");

            _broker = new WnsServiceBroker(_config);

            _broker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is WnsNotificationException)
                    {
                        var notificationException = (WnsNotificationException)ex;
                        Console.WriteLine($"WNS Notification Failed :{notificationException}");
                    }
                    else
                    {
                        Console.WriteLine("WNS Notification Failed for some (Unknown Reason)");
                    }

                    return true;
                });
            };

            _broker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine($"WNS Notification Sent! ({notification})");
            };

            _broker.Start();
        }

        public void Push<T>(IEnumerable<T> pushList)
        {
            IEnumerable<Wns> apnsList = (IEnumerable<Wns>)pushList;

            if (apnsList != null)
            {
                foreach (var item in apnsList)
                {
                    _broker.QueueNotification(new WnsToastNotification
                    {
                        ChannelUri = item.ChannelUri,
                        Payload = XElement.Parse(item.Payload),
                        RequestForStatus = item.RequestForStatus,
                        TimeToLive = item.TimeToLive,
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
            if (_broker != null)
            {
                _broker.Stop();
                _broker = null;
            }

            if (_config != null)
            {
                _config = null;
            }

            base.DisposeCore();
        }
    }
}
