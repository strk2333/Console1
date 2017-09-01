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
                Debug.Write(p.GetPoint());
            }
            for (int i = -(int)_floorArea.x; i < _floorArea.x; i++)
            {
                for (int j = -(int)_floorArea.y; j < _floorArea.y; j++)
                {
                    if (points.Find(item => item.x == i && item.y == j) != null)
                    {
                        Console.Write("C ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
                Debug.WriteLine("");
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
            int factor, factorx, factory;
            switch (type)
            {
                case 1:
                    foreach (Position pos in _pos)
                    {
                        do
                        {
                            factor = random.Next(0, 100);
                            factorx = random.Next(-1000, 1000);
                            factory = random.Next(-1000, 1000);
                            SetPoint(pos, factor, factorx, factory, refPoint, area / 2);
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

        private void SetPoint(Position pos, int factor, int factorx, int factory, V2<int, int> refPoint, V2<float, float> area)
        {
            if (factor < 40)
            {
                // center area
                pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factorx * 0.001f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f)));
            }
            else if (factor < 60)
            {
                if (factorx >= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factorx * 0.001f + area.x * 0.2f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.2f)));
                else if (factorx >= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f + area.x * 0.2f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.2f)));
                else if (factorx <= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.2f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.2f)));
                else if (factorx <= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.2f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.2f)));
            }
            else if (factor < 70)
            {
                if (factorx >= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factorx * 0.001f + area.x * 0.4f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.4f)));
                else if (factorx >= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f + area.x * 0.4f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.4f)));
                else if (factorx <= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.4f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.4f)));
                else if (factorx <= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.4f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.4f)));
            }
            else if (factor < 80)
            {
                if (factorx >= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factorx * 0.001f + area.x * 0.6f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.6f)));
                else if (factorx >= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f + area.x * 0.6f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.6f)));
                else if (factorx <= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.6f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.6f)));
                else if (factorx <= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.6f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.6f)));
            }
            else
            {
                // edge area
                if (factorx >= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factorx * 0.001f + area.x * 0.8f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.8f)));
                else if (factorx >= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f + area.x * 0.8f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.8f)));
                else if (factorx <= 0 && factory >= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.8f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f + area.y * 0.8f)));
                else if (factorx <= 0 && factory <= 0)
                    pos.SetPoint(new V2<int, int>((int)(refPoint.x + area.x * 0.2f * factory * 0.001f - area.x * 0.8f), (int)(refPoint.y + area.y * 0.2f * factory * 0.001f - area.y * 0.8f)));
            }
        }

        private void SetPointByRadius(int r1, int r2, Position pos, int factor, int factorx, int factory, V2<int, int> refPoint, V2<float, float> area)
        {
            int x0 = (int)(factorx * 0.001) * r2 + r1;
            int y0 = (int)(factory * 0.001) * r2 + r1;
            double c = 2 * Math.PI * (float)r1;
            // y^2 == r1^2 - x^2
            int y1 = (int)Math.Sqrt(r1 * r1 - x0 * x0);
            int y2 = (int)Math.Sqrt(r2 * r2 - x0 * x0);
            y0 = y1 + (y2 - y1) * factory;
            if (factor < 40)
            {
                // center area

            }
            else if (factor < 60)
            {

            }
            else if (factor < 70)
            {
            }
            else if (factor < 80)
            {
            }
            else
            {
                // edge area
            }
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
