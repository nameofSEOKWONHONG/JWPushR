using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JWPush.Models
{
    public class PushWork
    {
        public int Id { get; set; }
        public string WorkName { get; set; }
        public int ThreadId { get; set; }
        public Task WorkTask { get; set; }
        public CancellationTokenSource TokenSource { get; set; }        
    }
}
