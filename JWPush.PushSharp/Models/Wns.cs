using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JWPush.Models
{
    public class WnsCert
    {
        public string WnsPackageName { get; set; }
        public string WnsPackageSid { get; set; }
        public string WnsClientSecret { get; set; }
    }

    public sealed class Wns
    {
        public string ChannelUri { get; set; }
        public string Payload { get; set; }
        public bool? RequestForStatus { get; set; }
        public object Tag { get; set; }
        public int? TimeToLive { get; set; }
    }
}