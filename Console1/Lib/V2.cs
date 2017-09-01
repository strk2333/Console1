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

        public V2()
        {
        }

        public V2(T x, V y)
        {
            this.x = x;
            this.y = y;
        }

        public static V2<T, V> operator /(V2<T, V> n1, V2<T, V> n2)
        {
            V2<int, int> o1 = (V2<int, int>)Convert.ChangeType(n1, typeof(V2<int, int>));
            V2<int, int> o2 = (V2<int, int>)Convert.ChangeType(n2, typeof(V2<int, int>));
            o1.x /= o2.x;
            o1.y /= o2.y;

            return (V2<T, V>)Convert.ChangeType(o1, typeof(V2<T, V>));
        }

        public static V2<T, V> operator /(V2<T, V> n1, int n2)
        {
            int x = (int)Convert.ChangeType(n1.x, typeof(int));
            int y = (int)Convert.ChangeType(n1.y, typeof(int));
            x /= n2;
            y /= n2;
            T x1 = (T)Convert.ChangeType(n1.x, typeof(T));
            V y1 = (V)Convert.ChangeType(n1.y, typeof(V));

            return new V2<T, V>(x1, y1);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
