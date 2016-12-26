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

        public string ManageName { get; set; }
        public byte[] iOSCert { get; set; }
        public string iOSCertPass { get; set; }
        public string GcmSenderId { get; set; }
        public string GcmAuthToken { get; set; }
        public string GcmOptionalApplicationId { get; set; }
        public string WnsPackageName { get; set; }
        public string WnsPackageSid { get; set; }
        public string WnsClientSecret { get; set; }
    }
}
