using System.Collections.Generic;
using Console1.WordGame.Creature;
using Console1.WordGame.BasketGame;
using SimCity.Fund;

namespace SimCity
{
    public sealed class Starter
    {
        private BasketController basketGame;
        private BasketTeam team1;
        private BasketTeam team2;

        public void Init()
        {
            CityMap cityMap = CityMap.GetInstance();
        }

        public void StartGame()
        {
            Init();
        }

        #region basket
        private void StartBasketGame()
        {
            basketGame = new BasketController();

            Human[] mem1 = new Human[5];
            Human[] mem2 = new Human[5];

            mem1[0] = new Human();
            mem1[0].Born("Paul");
            mem1[0].GetBasketTalent().ChangeTalent(3);
            mem1[0].GetBasketTalent().ChangeHeight(1.83f);
            mem1[1] = new Human();
            mem1[1].Born("Harden");
            mem1[1].GetBasketTalent().ChangeTalent(3);
            mem1[1].GetBasketTalent().ChangeHeight(1.98f);
            mem1[2] = new Human();
            mem1[2].Born("Anthony");
            mem1[2].GetBasketTalent().ChangeTalent(3);
            mem1[2].GetBasketTalent().ChangeHeight(2.03f);
            mem1[3] = new Human();
            mem1[3].Born("Capel");
            mem1[3].GetBasketTalent().ChangeTalent(2);
            mem1[3].GetBasketTalent().ChangeHeight(2.08f);
            mem1[4] = new Human();
            mem1[4].Born("Qi");
            mem1[4].GetBasketTalent().ChangeTalent(3);
            mem1[4].GetBasketTalent().ChangeHeight(2.23f);

            mem2[0] = new Human();
            mem2[0].Born("Curry");
            mem2[0].GetBasketTalent().ChangeTalent(3);
            mem2[0].GetBasketTalent().ChangeHeight(1.91f);
            mem2[1] = new Human();
            mem2[1].Born("Klay");
            mem2[1].GetBasketTalent().ChangeTalent(3);
            mem2[1].GetBasketTalent().ChangeHeight(1.96f);
            mem2[2] = new Human();
            mem2[2].Born("Durant");
            mem2[2].GetBasketTalent().ChangeTalent(3);
            mem2[2].GetBasketTalent().ChangeHeight(2.10f);
            mem2[3] = new Human();
            mem2[3].Born("Green");
            mem2[3].GetBasketTalent().ChangeTalent(3);
            mem2[3].GetBasketTalent().ChangeHeight(2.03f);
            mem2[4] = new Human();
            mem2[4].Born("Zaza");
            mem2[4].GetBasketTalent().ChangeTalent(1);
            mem2[4].GetBasketTalent().ChangeHeight(2.13f);

            team1 = new BasketTeam(mem1, "Houston");
            team2 = new BasketTeam(mem2, "Worriors");
            team1.AutoAllocateDef(team2);
            team2.AutoAllocateDef(team1);
            basketGame.Start(team1, team2);
        }
        #endregion
    }
}
