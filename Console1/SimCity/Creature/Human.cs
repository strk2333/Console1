using System;
using Console1.WordGame.BasketGame;

namespace Console1.WordGame.Creature
{
    public class Human : Animal, IComparable
    {
        private Talents talents;
        private Motions motions;

        private void Init()
        {
            talents = new Talents();
            motions = new Motions();
        }

        public override void Born(string name)
        {
            base.Born(name);
            Init();
        }

        public override void Die()
        {
            HandleDeath();
        }

        private void HandleDeath()
        {

        }

        public Talents GetTalents()
        {
            return talents;
        }

        public BasketTalent GetBasketTalent()
        {
            return talents.GetBasketTalent();
        }

        public BasketMotion GetBasketMotion()
        {
            return motions.GetBacketMotion();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
