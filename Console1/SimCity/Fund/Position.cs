using System;
using System.Collections.Generic;
using Lib;

namespace SimCity.Fund
{
    class Position
    {
        private string _name;
        private V2<int, int> _point;
        private List<Position> _neighbors;

        public Position()
        {
            _name = "Spot";
            _point = new V2<int, int>();
            _neighbors = new List<Position>();
        }

        public Position(string name, V2<int, int> point, List<Position> neighbors)
        {
            _name = name;
            _point = point;
            _neighbors = neighbors;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public V2<int, int> GetPoint()
        {
            return _point;
        }

        public void SetPoint(V2<int, int> point)
        {
            _point = point;
        }

        public List<Position> GetNeighbors()
        {
            return _neighbors;
        }

        public bool RemoveNeighbor(Position pos)
        {
            return _neighbors.Remove(pos);
        }

        public void AddNeighbor(Position pos)
        {
            if (pos != null)
                _neighbors.Add(pos);
        }
    }
}
