using System;
using Console1.WordGame.Creature;
using Basket;

namespace Console1.WordGame.BasketGame
{

    public class BasketMotion
    {
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public bool Shot(BasketTalent player, BasketTalent oppo, ShotType st = ShotType.Straight, RangeType rt = RangeType.MidRange)
        {
            if (TryShot(player, oppo, st, rt) > random.Next(0, 100))
                return true;
            else
                return false;
        }

        public bool Shot(float shot)
        {
            if (shot > random.Next(0, 100))
                return true;
            else
                return false;
        }

        public float TryShot(BasketTalent player, BasketTalent oppo, ShotType st = ShotType.Straight, RangeType rt = RangeType.MidRange)
        {
            // 100 - 50 + 10 (-40 / +10)
            float EnergyFactor = 1f;
            float playerResult;
            float oppoDef = (oppo.def * 6f + oppo.speed * 1.5f + oppo.armspan * 0.5f) / 16f;
            float playerEffect = (player.stability * 1.5f + player.stamina * 2.5f) / 40f;
            if (player.energy < 10)
            {
                EnergyFactor = .1f;
            }
            else if (player.energy < 30)
            {
                EnergyFactor = .2f;
            }
            else if (player.energy < 50)
            {
                EnergyFactor = .8f;
            }
            else if (player.energy < 70)
            {
                EnergyFactor = .9f;
            }

            switch (rt)
            {
                case RangeType.RimRange:
                    playerResult = player.rimRange;
                    return ((playerResult - oppoDef + playerEffect) + random.Next(-20, 20)) * EnergyFactor;
                case RangeType.MidRange:
                    playerResult = player.midRange;
                    return ((playerResult - oppoDef + playerEffect) + random.Next(-20, 10)) * EnergyFactor;
                case RangeType.LongRange:
                    playerResult = player.longRange;
                    oppoDef *= 1.2f;
                    return ((playerResult - oppoDef + playerEffect) + random.Next(-40, 20)) * EnergyFactor;
                default:
                    throw new Exception("RangeType Error!");
            }
        }

        // one two hands
        public bool Dunk()
        {
            Console.WriteLine("Dunk");
            return true;
        }

        public float Rebounding(BasketTalent player, BasketTalent oppo, int type = 0)
        {
            float playerEffect = (player.stability * 1.5f + player.stamina * 2.5f) / 20f; // 50
            if (type == 1)
            {
                // front
                return (player.rebound + playerEffect) * 0.9f - oppo.rebound / 2f;
            }

            return player.rebound + playerEffect - oppo.rebound / 2f;
        }
        public bool Pass()
        {
            Console.WriteLine("Pass");

            return true;
        }
        public bool Block()
        {
            Console.WriteLine("Block");

            return true;
        }
        public bool Steal()
        {
            Console.WriteLine("Steal");

            return true;
        }

        void SpecialDribble()
        {

        }

        void BoxOut()
        {

        }

        void Charge()
        {

        }

        void JumpBall()
        {

        }

        void Delay()
        {

        }

        void ForceFoul()
        {

        }

        // team
        void Pick()
        {

        }

        void Roll()
        {

        }

        void Give()
        {

        }

        void Go()
        {

        }

        void DoubleTeam()
        {

        }

        void TribbleTeam()
        {

        }

    }
}
