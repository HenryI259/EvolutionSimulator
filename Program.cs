using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSimulator
{
    class Creature
    {
        private int encoding;
        public Creature()
        {
            this.encoding = 0;
        }

        public Creature(int encoding)
        {
            this.encoding = encoding;
        }

        public void Action()
        {

        }
    }

    abstract class Environment
    {
        private List<Creature> creatures = new List<Creature>();
        private int generation = 0;
        private int maxTime;
        private int mutationChance;
        private int creatureAmount;
        private int length;
        private int width;
        private Creature[][] positions;


        public Environment()
        {

        }

        public bool survive(Creature creature)
        {
            return true;
        }

        public Creature GetCreature(int index)
        {
            return creatures[index];
        }

        public Creature GetCreature(int x, int y)
        {
            return positions[y][x];
        }

        public int getGeneration()
        {
            return generation;
        }

        private void turn()
        {
            for (int i = 0; i < creatures.Count; i++)
            {
                creatures[i].Action();
            }
        }

        public void lifeCycle()
        {
            for (int i = 0; i < maxTime; i++)
            {
                turn();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
