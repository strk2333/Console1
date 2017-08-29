using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1.WordGame.Creature
{
    public abstract class Animal : IAnimal
    {
        private string name;

        public virtual void Born(string name)
        {
            this.name = name;
        }

        public abstract void Die();

        public string GetName()
        {
            return name;
        }
    }
}
