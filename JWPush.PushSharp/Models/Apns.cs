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

        /// <summary>
        ///장치 확인
        ///공백 제거된 16진수 문자열
        /// </summary>
        public string DeviceToken { get; set; }


        public DateTime? Expiration { get; set; }
        public int Identifier { get; }
        public bool LowPriority { get; set; }

        /// <summary>
        /// json string : 
        /// {
        ///     "aps" : { "alert" : { // 스트링 혹은 딕셔너리 형태로 값이 들어갈 수 있으며 스트링으로 할 경우 보내고자하는 메시지를 적어 넣어주시면 됩니다.
        ///         "body" : "내용을 적어넣으세요", //전달할 내용을 입력합니다.
        ///         "action-loc-key" : "PLAY", //LocalizedString의 키값을 참조해 AlertView의 View Detail이라는 버튼을 언어에 맞게 변경시켜줍니다.
        ///         "loc-key" : "CONTENT", //LocalizedString의 키값을 참조해 내용을 채워줍니다.
        ///         "loc-args" : [ "Jenna", "Frank" ]
        ///     },
        ///     "badge" : 5, //배지의 숫자를 입력합니다.
        ///     },
        ///     "acme1" : "bar",
        ///     "acme2" : [ "bang", "whiz" ] //커스터마이징 변수값으로 임의의 문자열로 키와 값을 만들어 스트링 혹은 배열의 형태로 전달 할 수 있습니다.
        /// }
        /// </summary>
        public string Payload { get; set; }

        public object Tag { get; set; }
    }
}
