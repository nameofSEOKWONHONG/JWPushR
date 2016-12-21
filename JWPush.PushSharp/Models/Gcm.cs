using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush.Models
{
    public sealed class GcmCert
    {
        public string GcmSenderId { get; set; }
        public string AuthToken { get; set; }
        public string OptionalApplicationIdPackageName { get; set; }
    }

    public sealed class Gcm
    {
        public string CollapseKey { get; set; }        
        public bool? ContentAvailable { get; set; }        
        public JObject Data { get; set; }        
        public bool? DelayWhileIdle { get; set; }        
        public bool? DryRun { get; set; }        
        public string MessageId { get; }        
        public JObject Notification { get; set; }        
        public string NotificationKey { get; set; }
        public int? Priority { get; set; } //Normal = 5, High = 10        
        public List<string> RegistrationIds { get; set; }        
        public string RestrictedPackageName { get; set; }        
        public object Tag { get; set; }        
        public int? TimeToLive { get; set; }        
        public string To { get; set; }
    }
}
