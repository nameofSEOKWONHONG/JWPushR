using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWPush.Models
{
    [Table("PushApps")]
    public class PushApp : BasePushModel
    {
        [Key]
        public int PushNo { get; set; }

        public string ApplicationName { get; set; }

        public byte[] iOSCert { get; set; }
        public string iOSCertPass { get; set; }
        public string iOSUseYn { get; set; } = "N";

        public string GcmSenderId { get; set; }
        public string GcmAuthToken { get; set; }
        public string GcmOptionalApplicationIdPackageName { get; set; }
        public string GcmUseYn { get; set; } = "N";

        public string WnsPackageName { get; set; }
        public string WnsPackageSid { get; set; }
        public string WnsClientSecret { get; set; }
        public string WnsUseYn { get; set; } = "N";
    }
}
