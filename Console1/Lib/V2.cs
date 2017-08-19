using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    
    class V2<T, V>
    {
        public T x;
        public V y;

        public V2(T x, V y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
