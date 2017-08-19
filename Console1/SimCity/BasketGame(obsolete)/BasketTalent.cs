using System;
using System.Collections;

namespace LifeGame.WordGame.BasketGame
{
    public class BasketTalent
    {
        public int NO;
        public float weight;
        public float height;
        public float armspan;
        public int jump;

        // Shot
        public int rimRange;
        public int midRange;
        public int longRange;

        public int dunk;
        public int FT;
        public int block;
        public int rebound;
        public int steal;
        public int pass;

        public int def;
        public int dribble;
        public int protectBall;
        // (strength)
        public int tough;
        public int speed;
        // 体力
        public int stamina;
        public float energy;
        // 稳定性 -- shot
        public int stability;
        public int IQ;
        public int magic;

        private float recoverFactor;
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        // random normal stat
        public BasketTalent()
        {
            ChangeTalent(2);
        }

        public BasketTalent(int type, Basket.BasketPos pos)
        {
            ChangeTalent(type, pos);
        }

        public BasketTalent(int type)
        {
            ChangeTalent(type);
        }

        public void ChangeTalent(int type)
        {
            switch (type)
            {
                case 1:
                    // low lv
                    weight = random.Next(45, 85) / 100f * StaticData.maxWeight;
                    height = random.Next(65, 85) / 100f * StaticData.maxHeight;

                    jump = random.Next(20, 70);
                    rimRange = random.Next(40, 60);
                    midRange = random.Next(30, 65);
                    longRange = random.Next(20, 60);

                    def = random.Next(20, 60);
                    dribble = random.Next(20, 50);
                    speed = random.Next(20, 60);
                    // 体力
                    stamina = random.Next(20, 65);
                    energy = 100f;
                    // 稳定性 -- shot
                    stability = random.Next(20, 65);
                    IQ = random.Next(10, 60);
                    magic = random.Next(0, 1000);
                    break;
                case 2:
                    // normal
                    weight = random.Next(52, 92) / 100f * StaticData.maxWeight;
                    height = random.Next(72, 92) / 100f * StaticData.maxHeight;

                    jump = random.Next(60, 80);
                    rimRange = random.Next(55, 85);
                    midRange = random.Next(60, 85);
                    longRange = random.Next(50, 80);

                    def = random.Next(50, 75);
                    dribble = random.Next(40, 75);
                    speed = random.Next(50, 80);
                    // 体力
                    stamina = random.Next(50, 80);
                    energy = 100f;
                    // 稳定性 -- shot
                    stability = random.Next(50, 80);
                    IQ = random.Next(50, 80);
                    magic = random.Next(0, 1000);
                    break;
                case 3:
                    // high lv
                    weight = random.Next(52, 100) / 100f * StaticData.maxWeight;
                    height = random.Next(72, 100) / 100f * StaticData.maxHeight;

                    jump = random.Next(60, 100);
                    rimRange = random.Next(60, 100);
                    midRange = random.Next(65, 100);
                    longRange = random.Next(55, 100);

                    def = random.Next(50, 100);
                    dribble = random.Next(40, 100);
                    speed = random.Next(50, 100);
                    // 体力
                    stamina = random.Next(50, 100);
                    energy = 100f;
                    // 稳定性 -- shot
                    stability = random.Next(50, 100);
                    IQ = random.Next(50, 100);
                    magic = random.Next(0, 1000);
                    break;
            }
            updateHighLvAb();
        }

        public void ChangeTalent(int type, Basket.BasketPos pos)
        {
            ChangeTalent(type);
            switch (pos)
            {
                case Basket.BasketPos.C:
                    height = random.Next(213, (int)(StaticData.maxHeight * 100)) / 100f;
                    break;
                case Basket.BasketPos.PF:
                    height = random.Next(207, 213) / 100f;
                    break;
                case Basket.BasketPos.SF:
                    height = random.Next(200, 207) / 100f;
                    break;
                case Basket.BasketPos.SG:
                    height = random.Next(190, 200) / 100f;
                    break;
                case Basket.BasketPos.PG:
                    height = random.Next(160, 193) / 100f;
                    break;
            }
            updateHighLvAb();
        }

        public void updateHighLvAb()
        {
            armspan = height * 0.95f + random.Next(10, 80) / 100f * (StaticData.maxArmspan - StaticData.maxHeight * 0.95f);
            protectBall = dribble * 7 / 10 + random.Next(40, 95) * 3 / 10;
            tough = (int)(weight / StaticData.maxWeight * 3 / 10) + random.Next(40, 95) * 7 / 10;
            dunk = jump * 5 / 10 + (int)(armspan / StaticData.maxArmspan * 500 / 10);
            FT = midRange * 7 / 10 + random.Next(40, 95) * 3 / 10;
            block = jump * 2 / 10 + (int)armspan * 3 / 10 + def * 4 / 10 + random.Next(40, 95) * 1 / 10;
            rebound = jump * 1 / 10 + (int)(armspan / StaticData.maxArmspan * 100 * 5 / 10) + (int)(height / StaticData.maxHeight * 100 * 3 / 10) + random.Next(40, 95) * 1 / 10;
            steal = speed * 3 / 10 + IQ * 2 / 10 + random.Next(40, 95) * 5 / 10;
            pass = IQ * 8 / 10 + random.Next(40, 95) * 2 / 10;
            recoverFactor = stamina / 100f;

        }

        public void ChangeHeight(float height)
        {
            if (height > StaticData.maxHeight)
                this.height = StaticData.maxHeight;
            else if (height < 1f)
                this.height = 1f;
            else
                this.height = height;

            updateHighLvAb();
        }

        public void ShotConsume()
        {
            ConsumeEnergy(6f);
        }

        public void ConsumeEnergy(float c)
        {
            if ((energy -= c) <= 1f)
                energy = 1f;
        }

        public void RecoverEnergy()
        {
            if ((energy += recoverFactor) > 100f)
                energy = 100f;
        }

        public void RecoverEnergy(float c)
        {
            if ((energy += c) > 100f)
                energy = 100f;
        }
    }
}
