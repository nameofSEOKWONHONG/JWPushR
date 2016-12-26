using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWPush.Abstract
{
    public interface IJWPush
    {
        void Initialize();
        void Push<T>(IEnumerable<T> pushList);
    }
}
