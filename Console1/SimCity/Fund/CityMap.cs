using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using System.Diagnostics;

namespace SimCity.Fund
{
    public class CityMap
    {
        private static CityMap _instance;
        private V2<float, float> _floorArea;
        private TransMap _transMap;
        private Position[] _pos;

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
            _floorArea = new V2<float, float>(20, 20);
            _transMap = new TransMap(1, 10, new V2<int, int>(), _floorArea);
            ConsolePrintMap(_transMap.GetPos());
        }

        private void ConsolePrintMap(Position[] pos)
        {
            List<V2<int, int>> points = new List<V2<int, int>>();
            foreach (Position p in pos)
            {
                points.Add(p.GetPoint());
            }
            for (int y = (int)_floorArea.y; y > -_floorArea.y; y--)
            {
                for (int x = -(int)_floorArea.x; x < _floorArea.x; x++)
                {
                    if (points.Find(item => item.x == y && item.y == x) != null)
                    {
                        if (y == 0 && y == x)
                        {
                            Console.Write("θ");
                        }
                        else
                            Console.Write("C ");
                    }
                    else
                    {
                        if (y == 0 && y == x)
                        {
                            Console.Write("O");
                        }
                        else
                            Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    class TransMap
    {
        private Position[] _pos;
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
        /// 3: region
        /// 4: banding
        /// 5: circle
        /// </param>
        /// <param name="size">
        /// Transport point amount level. range: [1-20]. 
        /// </param>
        /// <param name="refPoint">
        /// Reference point for reference(NOT MUST the center point, BUT USUALLY).
        /// <para>
        /// Notice: refPoint is a coordiation, Not a real Position in the city.
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
                            SetPointByRadius(pos, factor, factorx, factory, refPoint, area / 2);
                            Debug.WriteLine("-" + pos.GetPoint());
                        } while (ListCheck(list, pos));
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

        private void SetPointByRadius(Position pos, float factor, float factorx, float factory, V2<int, int> refPoint, V2<float, float> area)
        {
            int x0, y0;

            if (factor < 0.1f)
            {
                // center area
                GetRandomPoint(0.0f, 0.2f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            }
            else if (factor < 0.3f)
            {
                GetRandomPoint(0.2f, 0.4f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            }
            else if (factor < 0.6f)
            {
                GetRandomPoint(0.4f, 0.6f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            }
            else if (factor < 0.8f)
            {
                GetRandomPoint(0.6f, 0.8f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            }
            else
            {
                // edge area
                GetRandomPoint(0.8f, 1f, Math.Abs(factorx), Math.Abs(factory), area, out x0, out y0);
            }
            if (factorx >= 0 && factory >= 0)
                pos.SetPoint(new V2<int, int>(refPoint.x + x0, refPoint.y + y0));
            else if (factorx >= 0 && factory <= 0)
                pos.SetPoint(new V2<int, int>(refPoint.x + x0, refPoint.y - y0));
            else if (factorx <= 0 && factory >= 0)
                pos.SetPoint(new V2<int, int>(refPoint.x - x0, refPoint.y + y0));
            else if (factorx <= 0 && factory <= 0)
                pos.SetPoint(new V2<int, int>(refPoint.x - x0, refPoint.y - y0));

            //pos.SetPoint(new V2<int, int>(refPoint.x + x0, refPoint.y + y0));

        }



        private void GetRandomPoint(float factorr1, float factorr2, float factorx, float factory, V2<float, float> area, out int x0, out int y0)
        {
            float r1 = area.x > area.y ? area.x * factorr1 : area.y * factorr1;
            float r2 = area.x > area.y ? area.x * factorr2 : area.y * factorr2;
            x0 = (int)(factorx * r2);
            // y^2 == r1^2 - x^2
            int y1 = r1 * r1 <= x0 * x0 ? 0 : (int)Math.Sqrt(r1 * r1 - x0 * x0);
            int y2 = (int)Math.Sqrt(r2 * r2 - x0 * x0);
            y0 = (int)(y1 + (y2 - y1) * factory);
            Debug.WriteLine(r1 + "," + r2);
            Debug.WriteLine(x0 + "," + y1);
        }

        private bool ListCheck(List<Position> list, Position pos)
        {
            return list.Find(item => item.GetPoint().x == pos.GetPoint().x && item.GetPoint().y == pos.GetPoint().y) == null;
        }

        private void InitPos(int size)
        {
            _pos = new Position[size * 10];
            for (int i = 0; i < _pos.Length; i++)
            {
                _pos[i] = new Position();
            }
        }

        public Position[] GetPos()
        {
            return _pos;
        }
    }
}
