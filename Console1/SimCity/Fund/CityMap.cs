using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1.SimCity.Fund
{
    class CityMap
    {
        private CityMap _instance;
        private Position[] pos;

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

        }
    }
}
