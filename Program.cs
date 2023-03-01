using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSimulator
{
    class Creature
    {
        private int gene;
        private int xpos;
        private int ypos;
        public Creature(int xpos, int ypos)
        {
            this.xpos = xpos;
            this.ypos = ypos;
            this.gene = 0;
        }

        public Creature(int xpos, int ypos, int gene)
        {
            this.xpos = xpos;
            this.ypos = ypos;
            this.gene = gene;
        }

        public int getGene()
        {
            return this.gene;
        }

        public void Action()
        {

        }
    }

    abstract class Environment
    {
        private List<Creature> creatures = new List<Creature>();
        private int generation;
        private int maxTime;
        private int mutationChance;
        private int reproduceChance;
        private int creatureAmount;
        private int maxChild;
        private int length;
        private int width;
        private int modifier;
        private Creature[][] positions;


        public Environment(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, int mutationChance, int modifier)
        {
            this.generation = 0;
            this.maxTime = maxTime;
            this.mutationChance = mutationChance;
            this.reproduceChance = reproduceChance;
            if (creatureAmount > length*width) 
            { 
                this.creatureAmount = length*width; 
            }
            else 
            {
                this.creatureAmount = creatureAmount;
            }
            this.maxChild = maxChild;
            this.length = length;
            this.width = width;
            this.positions = new Creature[length][width];
            Random randX;
            Random randY;
            int xpos;
            int ypos;
            for (int i = 0; i < creatureAmount; i++) {
                randX = new Random(i+modifier);
                randY = new Random((i+modifier)*2)
                xpos = randX.Next(0, width);
                ypos = randY.Next(0, length);
                while (positions[ypos][xpos] != null) {
                    xpos += 1;
                    ypos += 1;
                    if (xpos >= width) { xpos = 0; }
                    if (ypos >= length) { ypos = 0; }
                }
                creatures.Add(new Creature(xpos, ypos));
                positions[ypos][xpos] = creatures[i];
            }
        }

        public bool survive(Creature creature)
        {
            return true;
        }

        public Creature GetCreature(int index)
        {
            return this.creatures[index];
        }

        public Creature GetPosition(int x, int y)
        {
            return this.positions[y][x];
        }

        public int getGeneration()
        {
            return this.generation;
        }

        private int mutate(int gene) {
            int newGene = gene;
            return newGene;
        }

        private int reproduce(List<Creature> parents) 
        {
              this.creatures = new List<Creature>();
              Random randX;
              Random randY;
              Random rand;
              int xpos;
              int ypos;
              for (int i = 0; i < this.maxChild*parents.Count; i++) {
                  rand = new Random(i+this.generation+this.modifier);
                  if (rand.Next(0, 100) < this.reproduceChance && this.creatures.Count < this.creatureAmount)
                  {
                      randX = new Random((i+this.generation+this.modifier)*2);
                      randY = new Random((i+this.generation+this.modifier)*3)
                      xpos = randX.Next(0, this.width);
                      ypos = randY.Next(0, this.length);
                      while (this.positions[ypos][xpos] != null) {
                          xpos += 1;
                          ypos += 1;
                          if (xpos >= this.width) { xpos = 0; }
                          if (ypos >= this.length) { ypos = 0; }
                      }
                      this.creatures.Add(new Creature(xpos, ypos, mutate(parents[i%parents.Count].getGene()));
                      this.positions[ypos][xpos] = this.creatures[i];
                  }
              }
        }

        public void lifeCycle()
        {
            for (int i = 0; i < this.maxTime; i++)
            {
                for (int j = 0; j < this.creatures.Count; j++)
                {
                    this.creatures[j].Action();
                }
            }

            for (int i = 0; i < this.creatures.Count; i++) 
            {
                if (!survive(this.creatures[i])) 
                {
                  this.creatures.RemoveAt(i);
                  i--;
                }
            }

            List<Creature> parents = this.creatures;
            reproduce(parents);

            
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
        }
    }
}
