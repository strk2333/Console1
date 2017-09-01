using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1.SimCity.Fund
{
    public class CityMap
    {
        private static CityMap _instance;
        private int _floorArea;
        private TransMap _transMap;
        private Position[] _pos;

        private CityMap()
        {
            Init();
        }

        public CityMap GetInstance()
        {
            if (_instance == null)
                return new CityMap();
            return _instance;
        }

        private void Init()
        {
            _transMap = TransMap.GenRandomTransMap(1, 6);
        }
    }

    class TransMap
    {
        private static Position[] _pos;
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        private TransMap() { }

        /// <summary>
        /// Generate random transport map
        /// </summary>
        /// <param name="type">
        /// 1: one-core radiation
        /// 2: multi-core radiation
        /// 3: region
        /// 4: banding
        /// 5: circle
        /// </param>
        /// <param name="size">
        /// Transport point amount level. range: [1-20]. 
        /// </param>
        public static TransMap GenRandomTransMap(int type, int size)
        {
            _pos = new Position[size * 100];
            TransMap transMap = new TransMap();
            switch (type)
            {
                case 1:
                    for (int i = 0; i < size; i++)
                    {

                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
            }
            return transMap;
        }
    }
}
