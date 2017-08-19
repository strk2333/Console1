using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.WordGame.BasketGame
{
    public class PlayerData
    {
        public int score;
        public int FG; // field goal
        public int FGA; // field goal all
        public int score2; // 2 points ball
        public int score2A; // all
        public int score3;
        public int score3A;
        public int REB;
        public int FREB;
        public int BREB;

        public PlayerData()
        {
            score = 0;
            FG = 0;
            FGA = 0;
            score2 = 0;
            score2A = 0;
            score3 = 0;
            score3A = 0;
            REB = 0;
            FREB = 0;
            BREB = 0;
        }

        public bool Shot2(bool res)
        {
            FGA++;
            score2A++;
            if (res)
            {
                FG++;
                score2++;
                score += 2;
            }

            return res;
        }

        public bool Shot3(bool res)
        {
            FGA++;
            score3A++;
            if (res)
            {
                FG++;
                score3++;
                score += 3;
            }

            return res;
        }

        public int Rebound(int type)
        {
            if (type == 0)
            {
                BREB++;
            }
            else
            {
                FREB++;
            }
            return ++REB;
        }
    }
}
