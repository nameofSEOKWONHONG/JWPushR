using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush.Models
{
    public class ApnsCert
    {
        public byte[] Cert { get; set; }
        public string CertPass { get; set; }
    }

    public sealed class Apns : BasePushModel
    {
        public string DeviceToken { get; set; }
        public DateTime? Expiration { get; set; }
        public int Identifier { get; }
        public bool LowPriority { get; set; }

        /// <summary>
        /// json string
        /// </summary>
        public string Payload { get; set; }

        public object Tag { get; set; }
    }
}
