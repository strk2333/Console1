using System;
using System.Collections.Generic;
using System.Linq;
using LifeGame.WordGame.Creature;
using System.Threading.Tasks;
using Basket;

namespace LifeGame.WordGame.BasketGame
{
    class BasketController
    {
        BasketGameState state;
        int team1Point;
        int team2Point;
        int round;
        BasketTeam team1, team2;
        System.Timers.Timer timer;
        readonly int TOTROUND = 120;

        public void Start(BasketTeam team1, BasketTeam team2)
        {
            this.team1 = team1;
            this.team2 = team2;
            round = -1;
            state = BasketGameState.JumpBall;
            team1Point = 0;
            team2Point = 0;

            timer = new System.Timers.Timer(0.1f);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Console.WriteLine("-------------------- Basket Game ------------------");
            Console.WriteLine("-------------------- {0} VS {1} -------------------", team1.GetTeamName(), team2.GetTeamName());

        }

        private Task taskPreSecond()
        {
            System.Timers.Timer timer = new System.Timers.Timer(5f);

            return Task.Run(() =>
            {
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            });
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (++round <= TOTROUND)
            {
                PlayGameStep();
            }
            else
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("----------------------- Final ------------------------");
                Console.WriteLine("\t\t" + team1.GetTeamName() + " " + team1Point + " : " + team2Point + " " + team2.GetTeamName());
                team1.ShowStats();
                team2.ShowStats();
                Console.WriteLine("------------------------------------------------------");
                team1.ShowAb();
                team2.ShowAb();
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
            }
        }

        private void PlayGameStep()
        {
            switch (state)
            {
                case BasketGameState.JumpBall:
                    Console.WriteLine("------------------------------------------------------");
                    JumpBall();
                    break;

                case BasketGameState.T1Ball:
                    Console.WriteLine("------------------------------------------------------");
                    T1Attak();
                    break;

                case BasketGameState.T2Ball:
                    Console.WriteLine("------------------------------------------------------");
                    T2Attak();
                    break;

                default:
                    Console.Read();
                    //Console.WriteLine("------------------------------------------------------");
                    break;
            }

            team1.RecoverEnergy();
            team2.RecoverEnergy();
        }

        private void changeState(BasketGameState gs)
        {
            state = gs;
        }

        #region Game Process
        private void JumpBall()
        {
            BasketTalent p1 = team1.GetBallJumper().GetTalents().GetBasketTalent();
            BasketTalent p2 = team2.GetBallJumper().GetTalents().GetBasketTalent();

            float p1JumpResult = p1.height / StaticData.maxHeight * 20f + p1.armspan / StaticData.maxArmspan * 30f +
                p1.jump / StaticData.maxJump * 50f;
            float p2JumpResult = p2.height / StaticData.maxHeight * 20f + p2.armspan / StaticData.maxArmspan * 30f +
                p2.jump / StaticData.maxJump * 50f;
            Console.WriteLine("----------------------- Jump Ball -------------------------");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("\t{2}\t{0}\tJump Ball Point： {1}", team1.GetBallJumper().GetName(), p1JumpResult, team1.GetTeamName());
            Console.WriteLine("\t{2}\t{0}\tJump Ball Point： {1}", team2.GetBallJumper().GetName(), p2JumpResult, team2.GetTeamName());

            if (p1JumpResult > p2JumpResult)
            {
                changeState(BasketGameState.T1Ball);
                Console.WriteLine("\t{1}\t{0}\tGet Ball！", team1.GetBallJumper().GetName(), team1.GetTeamName());
            }
            else
            {
                changeState(BasketGameState.T2Ball);
                Console.WriteLine("\t{1}\t{0}\tGet Ball！", team2.GetBallJumper().GetName(), team2.GetTeamName());
            }
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("--------------------- Game Start -----------------------");
        }

        private void T1Attak()
        {
            Console.WriteLine("\t\t\tRound：" + round);
            int roundPoint = team1.NoramlAttack(team2);
            team1Point += roundPoint;
            Console.WriteLine("\t\t" + team1.GetTeamName() + " " + team1Point + " : " + team2Point + " " + team2.GetTeamName());
            if (roundPoint == 0 && round != TOTROUND)
            {
                int p1;
                int p2;
                if (team1.Rebounding(team2, out p1, out p2))
                {
                    changeState(BasketGameState.T1Ball);
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("\t{0} Get Front Rebound, Defeat {1}, {2}'s Ball", team1.GetPlayer(p1).GetName(), team2.GetPlayer(p2).GetName(), team1.GetTeamName());
                }
                else
                {
                    changeState(BasketGameState.T2Ball);
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("\t{0} Get Back Rebound, Defeat {1}, {2}'s Ball", team2.GetPlayer(p2).GetName(), team1.GetPlayer(p1).GetName(), team2.GetTeamName());
                }
            }
            else
                changeState(BasketGameState.T2Ball);
        }

        private void T2Attak()
        {
            Console.WriteLine("\t\t\tRound：" + round);
            int roundPoint = team2.NoramlAttack(team1);
            team2Point += roundPoint;
            Console.WriteLine("\t\t" + team1.GetTeamName() + " " + team1Point + " : " + team2Point + " " + team2.GetTeamName());
            if (roundPoint == 0 && round != TOTROUND)
            {
                int p1;
                int p2;
                if (team2.Rebounding(team1, out p1, out p2))
                {
                    changeState(BasketGameState.T2Ball);
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.WriteLine("\t{0} Get Front Rebound, Defeat {1}, {2}'s Ball", team2.GetPlayer(p1).GetName(), team1.GetPlayer(p2).GetName(), team2.GetTeamName());
                }
                else
                {
                    changeState(BasketGameState.T1Ball);
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.WriteLine("\t{0} Get Back Rebound, Defeat {1}, {2}'s Ball", team1.GetPlayer(p2).GetName(), team2.GetPlayer(p1).GetName(), team1.GetTeamName());
                }
            }
            else
                changeState(BasketGameState.T1Ball);
        }
        #endregion
    }
}
