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
        /// <summary>
        /// 메시지 타입을 그룹화하는 기능으로 해당 단말이 off 일 경우 가장 최신 메세지만 전달되는 형태
        /// </summary>
        public string CollapseKey { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public bool? ContentAvailable { get; set; }

        /// <summary>
        /// 4kb 미만의 메세지 Json형태(key-value pair)
        /// </summary>    
        public string Data { get; set; }

        /// <summary>
        /// message 가 바로 전송되는 것이 아니라, phone 이 on 되었을 때 collapse_key 의 가장 마지막 메시지만 전송되도록 설정
        /// </summary>
        public bool? DelayWhileIdle { get; set; }

        /// <summary>
        /// true일 경우 dry run mode가 활성화됩니다. 기본 값은 false 입니다. dry mode란, code가 로컬에서 실행은 되지만, GA 서버로는 데이터를 전송하지 않는 mode입니다. 이것은 기록된 데이터를 더럽히지 않고 GA SDK에 대한 호출을 디버깅하는데 유용합니다.
        /// </summary>
        public bool? DryRun { get; set; } = false;

        public string MessageId { get; }        
        public string Notification { get; set; }    
        
        //not use    
        public string NotificationKey { get; set; }

        /// <summary>
        /// 우선순위
        /// </summary>
        public int? Priority { get; set; } //Normal = 5, High = 10        

        /// <summary>
        /// 필수사항 array로 1~1000 개의 아이디 입력 가능
        /// </summary>
        public List<string> RegistrationIds { get; set; }        
        public string RestrictedPackageName { get; set; }        
        public object Tag { get; set; }

        /// <summary>
        ///  단말이 off 일 때 GCM storage 에서 얼마나 저장되어야 하는지 설정하는 값으로 collapse_key 와 반드시 함께 설정되야 함
        /// </summary>  
        public int? TimeToLive { get; set; }   
             
        public string To { get; set; }
    }
}
