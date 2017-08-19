﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.WordGame.Creature
{
    public interface IAnimal
    {
        void Born(string name);
        void Die();
    }
}
