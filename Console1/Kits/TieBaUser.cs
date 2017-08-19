using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kits
{
    class TieBaUser : IComparable
    {
        public string name;
        public int postCount;
        public int remarkCount;

        public TieBaUser(string name, int postCount)
        {
            this.name = name;
            this.postCount = postCount;
        }

        public int CompareTo(object obj)
        {
            return ~postCount.CompareTo((obj as TieBaUser).postCount);
        }
    }
}
