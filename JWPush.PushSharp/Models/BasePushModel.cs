using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush.Models
{
    public class BasePushModel
    {
        public DateTime ReserveDate { get; set; }
        public string SendYn { get; set; }
        public DateTime SendDate { get; set; }     
        public string EditId { get; set; }   
        public DateTime EditDtm { get; set; }
    }
}
