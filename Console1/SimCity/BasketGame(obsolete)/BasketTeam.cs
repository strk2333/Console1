using System;
using System.Linq;
using Console1.WordGame.Creature;
using Basket;
using System.Collections.Generic;

namespace Console1.WordGame.BasketGame
{
    public class BasketTeam
    {
        public PlayerData[] playerData;

        private Human[] mem;
        private BasketPos[] memPos;
        private Human[] def;
        private Random random = new Random(Guid.NewGuid().GetHashCode());
        private string teamName;

        public BasketTeam(Human[] mem, string teamName)
        {
            this.teamName = teamName;
            playerData = new PlayerData[mem.Length];
            for (int j = 0; j < mem.Length; j++)
            {
                playerData[j] = new PlayerData();
            }
            var tmp = from mMem in mem orderby mMem.GetBasketTalent().height ascending select mMem;
            this.mem = new Human[mem.Length];
            int i = 0;
            foreach (Human m in tmp)
            {
                this.mem[i++] = m;
            }
            AutoAllocatePos();
        }

        private void AutoAllocatePos()
        {
            memPos = new BasketPos[mem.Length];
            for (int i = 0; i < mem.Length; i++)
            {
                if (mem[i].GetBasketTalent().height < 1.93f)
                {
                    memPos[i] = BasketPos.PG;
                }
                else if (mem[i].GetBasketTalent().height < 2.00f)
                {
                    memPos[i] = BasketPos.SG;
                }
                else if (mem[i].GetBasketTalent().height < 2.07f)
                {
                    memPos[i] = BasketPos.SF;
                }
                else if (mem[i].GetBasketTalent().height < 2.13f)
                {
                    memPos[i] = BasketPos.PF;
                }
                else
                {
                    memPos[i] = BasketPos.C;
                }
            }
        }

        public void AutoAllocateDef(BasketTeam t2)
        {
            def = new Human[mem.Length];
            for (int i = 0; i < mem.Length; i++)
            {
                def[i] = t2.GetPlayer(i);
            }
        }

        public Human GetBallJumper()
        {
            Human selectMan = mem[0];
            for (int i = 1; i < mem.Length; i++)
            {
                selectMan = compareJumpBall(selectMan, mem[i]);
            }
            return selectMan;
        }

        private Human compareJumpBall(Human mem1, Human mem2)
        {
            BasketTalent t1 = mem1.GetBasketTalent();
            BasketTalent t2 = mem2.GetBasketTalent();
            float m1JumpResult = t1.height / StaticData.maxHeight * 20f + t1.armspan / StaticData.maxArmspan * 30f +
                t1.jump / StaticData.maxJump * 50f;
            float m2JumpResult = t2.height / StaticData.maxHeight * 20f + t2.armspan / StaticData.maxArmspan * 30f +
                t2.jump / StaticData.maxJump * 50f;

            return (m1JumpResult > m2JumpResult) ? mem1 : mem2;
        }

        public void ShowStats()
        {
            Console.WriteLine("\tPOS\tNAME\tHEIGHT\tScore\tFG\t2-Pts\t3-Pts\tREB\tFREB\tBREB");
            for (int i = 0; i < mem.Length; i++)
            {
                Console.WriteLine("\t" + GetPos(i) + "\t" + mem[i].GetName() + "\t" + string.Format("{0:0.00}", mem[i].GetBasketTalent().height) + "\t" +
                    playerData[i].score + "\t" +
                    playerData[i].FG + "-" + playerData[i].FGA + "\t" +
                    playerData[i].score2 + "-" + playerData[i].score2A + "\t" +
                    playerData[i].score3 + "-" + playerData[i].score3A + "\t" +
                    playerData[i].REB + "\t" + playerData[i].FREB + "\t" + playerData[i].BREB);
            }
        }

        public void ShowAb()
        {
            Console.WriteLine("\tPOS\tNAME\tHEIGHT\tRIMSHOT\tMIDSHOT\t3SHOT\tREB");
            for (int i = 0; i < mem.Length; i++)
            {
                Console.WriteLine("\t" + GetPos(i) + "\t" + mem[i].GetName() + "\t" + string.Format("{0:0.00}", mem[i].GetBasketTalent().height) + "\t" +
                    mem[i].GetBasketTalent().rimRange + "\t" + mem[i].GetBasketTalent().midRange + "\t" +
                    mem[i].GetBasketTalent().longRange + "\t" + mem[i].GetBasketTalent().rebound);
            }
        }

        public int NoramlAttack(BasketTeam team2)
        {
            int attacker;
            float shotValue = 0;
            if ((attacker = GetAttacker(out shotValue, rt: RangeType.LongRange)) != -1)
            {
                mem[attacker].GetBasketTalent().ShotConsume();
                if (playerData[attacker].Shot3(mem[attacker].GetBasketMotion().Shot(shotValue)))
                {
                    Console.WriteLine("\t" + mem[attacker].GetName() + " For Three √\t| Defencer：" + GetDefencer(attacker).GetName());
                    Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                    return 3;
                }
                else
                {
                    Console.WriteLine("\t" + mem[attacker].GetName() + " For Three ×\t| Defencer：" + GetDefencer(attacker).GetName());
                    Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                    return 0;
                }
            }
            else
            {
                if ((attacker = GetAttacker(out shotValue, rt: RangeType.RimRange)) != -1)
                {
                    mem[attacker].GetBasketTalent().ShotConsume();

                    if (playerData[attacker].Shot2(mem[attacker].GetBasketMotion().Shot(shotValue)))
                    {
                        Console.WriteLine("\t" + mem[attacker].GetName() + " Bank Shot √\t| Defencer：" + GetDefencer(attacker).GetName());
                        Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                        return 2;
                    }
                    else
                    {
                        Console.WriteLine("\t" + mem[attacker].GetName() + " Bank Shot ×\t| Defencer：" + GetDefencer(attacker).GetName());
                        Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                        return 0;
                    }
                }
                else
                {
                    if ((attacker = GetAttacker(out shotValue, rt: RangeType.MidRange)) != -1)
                    {
                        mem[attacker].GetBasketTalent().ShotConsume();

                        if (playerData[attacker].Shot2(mem[attacker].GetBasketMotion().Shot(mem[attacker].GetBasketTalent(), team2.mem[0].GetBasketTalent(),
                            ShotType.CatchShot, RangeType.MidRange)))
                        {
                            Console.WriteLine("\t" + mem[attacker].GetName() + " Mid Shot √\t| Defencer：" + GetDefencer(attacker).GetName());
                            Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                            return 2;
                        }
                        else
                        {
                            Console.WriteLine("\t" + mem[attacker].GetName() + " Mid Shot ×\t| Defencer：" + GetDefencer(attacker).GetName());
                            Console.WriteLine("\t" + mem[attacker].GetName() + " present energy: " + mem[attacker].GetBasketTalent().energy);
                            return 0;
                        }
                    }
                }
            }

            return 0;
        }

        public bool Rebounding(BasketTeam team2, out int player1, out int player2)
        {
            player1 = GetRebounder();
            player2 = team2.GetRebounder();

            if (mem[player1].GetBasketMotion().Rebounding(mem[player1].GetBasketTalent(), team2.mem[player2].GetBasketTalent(), 1) >
                team2.mem[player2].GetBasketMotion().Rebounding(team2.mem[player2].GetBasketTalent(), mem[player1].GetBasketTalent()))
            {
                playerData[player1].Rebound(1);
                return true;
            }
            else
            {
                team2.playerData[player2].Rebound(0);
                return false;
            }
        }

        public void RecoverEnergy()
        {
            foreach (var m in mem)
            {
                m.GetBasketTalent().RecoverEnergy();
            }
        }

        public int GetAttacker(out float shotValue, ShotType st = ShotType.Straight, RangeType rt = RangeType.LongRange)
        {
            if (rt == RangeType.LongRange && random.Next(0, 100) > 30)
            {
                shotValue = 0;
                return -1;
            }

            if (rt == RangeType.RimRange && random.Next(0, 100) > 50)
            {
                shotValue = 0;
                return -1;
            }

            Dictionary<int, float> canShot = new Dictionary<int, float>();
            float tmp;
            for (int i = 0; i < mem.Length; i++)
            {
                if (rt == RangeType.LongRange && (tmp = mem[i].GetBasketMotion().TryShot(mem[i].GetBasketTalent(),
                    GetDefencer(i).GetBasketTalent(), st, rt)) >= 35f)
                {
                    canShot.Add(i, tmp);
                }

                if (rt == RangeType.RimRange && (tmp = mem[i].GetBasketMotion().TryShot(mem[i].GetBasketTalent(),
                    GetDefencer(i).GetBasketTalent(), st, rt)) >= 45f)
                {
                    canShot.Add(i, tmp);
                }

                if (rt == RangeType.MidRange && (tmp = mem[i].GetBasketMotion().TryShot(mem[i].GetBasketTalent(),
                    GetDefencer(i).GetBasketTalent(), st, rt)) >= 0f)
                {
                    canShot.Add(i, tmp);
                }
            }

            if (random.Next(0, 100) < 30)
            {
                if (rt == RangeType.LongRange && canShot.ContainsKey(mem.Length - 1))
                    canShot.Remove(mem.Length - 1);
                if (rt == RangeType.LongRange && mem.Length > 4 && canShot.ContainsKey(mem.Length - 2))
                    canShot.Remove(mem.Length - 2);
            }

            if (canShot.Count != 0)
            {
                //return (from m in canShot where m.Value == canShot.Max((n) => n.Value) select m.Key).GetEnumerator().Current;
                shotValue = canShot.Max((n) => n.Value);
                return canShot.First((m) => m.Value == canShot.Max((n) => n.Value)).Key;
            }

            shotValue = -1f;
            return -1;
        }

        public int GetRebounder()
        {
            Dictionary<int, float> rebounder = new Dictionary<int, float>();
            for (int i = 0; i < mem.Length; i++)
            {
                if (random.Next(0, 100) < 40)
                {
                    rebounder.Add(i, mem[i].GetBasketTalent().rebound);
                }
            }

            if (rebounder.Count == 0)
            {
                int randomNum = random.Next(0, 4);
                rebounder.Add(randomNum, mem[randomNum].GetBasketTalent().rebound);
            }

            return rebounder.First((m) => m.Value == rebounder.Max((n) => n.Value)).Key;
            //return rebounder.First((m) => true).Key;
        }

        public Human GetPlayer(int index)
        {
            return mem[index];
        }

        public BasketPos GetPos(int index)
        {
            return memPos[index];
        }

        public Human GetDefencer(int index)
        {
            return def[index];
        }

        public string GetTeamName()
        {
            return teamName;
        }
    }
}
