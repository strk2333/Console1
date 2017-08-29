using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1.WordGame.Creature
{
    public class Talents
    {
        private BasketGame.BasketTalent basketTalent;

        public Talents()
        {
            basketTalent = new BasketGame.BasketTalent();
            basketTalent.NO = 1;
        }

        public Talents(int NO)
        {
            basketTalent = new BasketGame.BasketTalent();
            basketTalent.NO = NO;
        }

        public BasketGame.BasketTalent GetBasketTalent()
        {
            return basketTalent;
        }
    }
}
