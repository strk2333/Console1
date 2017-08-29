using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1.WordGame.Creature
{
    public class Motions
    {
        private BasketGame.BasketMotion basketMotion;

        public Motions()
        {
            basketMotion = new BasketGame.BasketMotion();
        }

        public BasketGame.BasketMotion GetBacketMotion()
        {
            return basketMotion;
        }
    }
}
