using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib;
using SimCity.Fund;

namespace SimCity.Fund
{
    public class CityMap
    {
        private static CityMap _instance;
        private static Route _route;
        private V2<float, float> _floorArea;
        private TransMap _transMap;
        private Position[] _pos;
        private List<Position> posList;

        private CityMap()
        {
            Init();
        }

        public static CityMap GetInstance()
        {
            if (_instance == null)
                return new CityMap();
            return _instance;
        }

        private void Init()
        {
            _floorArea = new V2<float, float>(15, 15);
            _route = new Route();
            _transMap = new TransMap(2, 20, new V2<int, int>(), _floorArea);
            _pos = _transMap.GetPos();
            posList = new List<Position>(_pos);
            DefaultNamePos();
            SetFixedNeighbors(_floorArea);
            ConsolePrintMap(_pos);
            _route.FindRoute("A0", "B0", posList);
            ConsolePrintNeibors();
        }

        private void DefaultNamePos()
        {
            for (int i = 0; i < _pos.Length; i++)
                _pos[i].SetName(((char)('A' + i % 26)).ToString() + i / 26);
        }

        private void SetFixedNeighbors(V2<float, float> floorArea)
        {
            V2<float, float> area = new V2<float, float>(floorArea);
            area /= 2;
            V2<int, int> tmpV2 = new V2<int, int>();
            Position tmpPos;
            foreach (Position pos in _pos)
            {
                // ← ↑ → ↓
                tmpV2.x = pos.GetPoint().x - 1;
                tmpV2.y = pos.GetPoint().y;
                if (pos.GetPoint().x - 1 > -area.x && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x;
                tmpV2.y = pos.GetPoint().y + 1;
                if (pos.GetPoint().y + 1 < area.y && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x + 1;
                tmpV2.y = pos.GetPoint().y;
                if (pos.GetPoint().x + 1 < area.x && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x;
                tmpV2.y = pos.GetPoint().y - 1;
                if (pos.GetPoint().y - 1 > -area.y && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }

                // ↖ ↗ ↘ ↙
                tmpV2.x = pos.GetPoint().x - 1;
                tmpV2.y = pos.GetPoint().y + 1;
                if (pos.GetPoint().x - 1 > -area.x && pos.GetPoint().y + 1 < area.y && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x + 1;
                tmpV2.y = pos.GetPoint().y + 1;
                if (pos.GetPoint().y + 1 < area.y && pos.GetPoint().x + 1 < area.x && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x + 1;
                tmpV2.y = pos.GetPoint().y - 1;
                if (pos.GetPoint().x + 1 < area.x && pos.GetPoint().y - 1 > -area.y && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
                tmpV2.x = pos.GetPoint().x - 1;
                tmpV2.y = pos.GetPoint().y - 1;
                if (pos.GetPoint().y - 1 > -area.y && pos.GetPoint().y - 1 > -area.y && (tmpPos = _transMap.ListContain(tmpV2)) != null)
                {
                    pos.AddNeighbor(tmpPos);
                }
            }
        }

        private void ConsolePrintMap(Position[] pos)
        {
            int count = 0;
            List<Position> points = new List<Position>(pos);

            Position p;
            // (-_floorArea.x, _floorArea.y)
            for (int y = (int)_floorArea.y; y > -_floorArea.y; y--)
            {
                for (int x = -(int)_floorArea.x; x < _floorArea.x; x++)
                {
                    if ((p = points.Find(item => item.GetPoint().x == x && item.GetPoint().y == y)) != null)
                    {
                        if (y == 0 && x == 0)
                        {
                            Console.Write(p.GetName() + " ");
                        }
                        else
                            Console.Write(p.GetName() + " ");

                        count++;
                    }
                    else
                    {
                        if (y == 0 && x == 0)
                        {
                            Console.Write("OO ");
                        }
                        else
                            Console.Write("[] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(count);
        }

        public void ConsolePrintNeibors()
        {
            foreach (Position pos in _pos)
            {
                Console.Write(pos.GetName() + ":");
                foreach (Position nei in pos.GetNeighbors())
                    Console.Write(nei.GetName() + " ");
                Console.WriteLine();
            }
        }
    }



    class TransMap
    {
        private Position[] _pos;
        private List<Position> _posList;
        private Random random = new Random(Guid.NewGuid().GetHashCode());
        public TransMap() { }

        public TransMap(int type, int size, V2<int, int> refPoint, V2<float, float> area)
        {
            GenRandomTransMap(type, size, refPoint, area);
        }

        /// <summary>
        /// Generate random transport map
        /// </summary>
        /// <param name="type">
        /// 1: one-core radiation
        /// 2: multi-core radiation
        /// 3: circle
        /// 4: banding
        /// 5: region (cores can use different types)
        /// </param>
        /// <param name="size">
        /// Transport point amount level. range: [1-20]. 
        /// </param>
        /// <param name="refPoint">
        /// Reference point for reference(NOT MUST the center point, BUT USUALLY).
        /// <para>
        /// Notice: refPoint is a coordiation, Not a real object Position in the city.
        /// </para>
        /// </param>
        public TransMap GenRandomTransMap(int type, int size, V2<int, int> refPoint, V2<float, float> area)
        {
            InitPos(size);
            List<Position> list = new List<Position>(_pos);
            TransMap transMap = new TransMap();
            float factor, factorx, factory;
            switch (type)
            {
                case 1:
                    foreach (Position pos in _pos)
                    {
                        do
                        {
                            factor = random.Next(0, 100) / (float)100;
                            factorx = random.Next(-1000, 1000) / (float)1000;
                            factory = random.Next(-1000, 1000) / (float)1000;
                        } while (!SetPointByRadius(list, pos, factor, factorx, factory, refPoint, area / 2));
                    }
                    break;
                case 2:
                    int cores = 2;

                    for (int i = 0; i < cores; i++)
                    {
                        float factorRef = random.Next(-1000, 1000) / (float)1000;
                        V2<int, int> tmpRef = new V2<int, int>((int) (area.x * factorRef), (int)(area.y * factorRef));
                        for (int j = i * _pos.Length / cores; j < (i + 1) * _pos.Length / cores; j++)
                        {
                            do
                            {
                                factor = random.Next(0, 100) / (float)100;
                                factorx = random.Next(-1000, 1000) / (float)1000;
                                factory = random.Next(-1000, 1000) / (float)1000;
                            } while (!SetPointByRadius(list, _pos[j], factor, factorx, factory, tmpRef, area / 2 / 5));
                        }
                    }
                    
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

        int count1 = 0;
        private bool SetPointByRadius(List<Position> posList, Position pos, float factor, float factorx, float factory, V2<int, int> refPoint, V2<float, float> area)
        {
            int x0, y0;

            if (factor < 0.2f)
                // center area
                GetRoundRandomPoint(0.0f, 0.2f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            else if (factor < 0.4f)
                GetRoundRandomPoint(0.2f, 0.4f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            else if (factor < 0.6f)
                GetRoundRandomPoint(0.4f, 0.6f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            else if (factor < 0.8f)
                GetRoundRandomPoint(0.6f, 0.8f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            else
                // edge area
                GetRoundRandomPoint(0.8f, 1f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);

            if (x0 >= area.x || x0 < 0 || y0 >= area.y || y0 < 0)
                return false;

            V2<int, int> result = new V2<int, int>();
            result.x = factorx >= 0 ? refPoint.x + x0 : refPoint.x - x0;
            result.y = factory >= 0 ? refPoint.y + y0 : refPoint.y - y0;

            if (ListContain(result) == null)
            {
                pos.SetPoint(result);
                //Console.WriteLine(++count1 + " " + x0 + ", " + y0);
                //Console.WriteLine(++count1 + " " + result);
                return true;
            }
            else
                return false;
        }

        private void GetRoundRandomPoint(float factorr1, float factorr2, float factorx, float factory, V2<float, float> area, out int x0, out int y0)
        {
            float r1 = area.x > area.y ? area.x * factorr1 : area.y * factorr1;
            float r2 = area.x > area.y ? area.x * factorr2 : area.y * factorr2;
            x0 = (int)(factorx * r2);
            // y^2 == r1^2 - x^2
            int y1 = r1 * r1 <= x0 * x0 ? 0 : (int)Math.Sqrt(r1 * r1 - x0 * x0);
            int y2 = (int)Math.Sqrt(r2 * r2 - x0 * x0);
            y0 = (int)(y1 + (y2 - y1) * factory);
        }

        // return true if there is no such pos in _posList
        public Position ListContain(V2<int, int> pos)
        {
            return _posList.Find(item => item.GetPoint().x == pos.x && item.GetPoint().y == pos.y);
        }

        private void InitPos(int size)
        {
            _pos = new Position[size * 10];
            for (int i = 0; i < _pos.Length; i++)
            {
                _pos[i] = new Position();
            }
            _posList = new List<Position>(_pos);
        }

        public Position[] GetPos()
        {
            return _pos;
        }
    }
}
