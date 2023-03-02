using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSimulator
{
    class Creature
    {
        private Habitat habitat;
        private int gene;
        private int xpos;
        private int ypos;
        public Creature(Habitat habitat, int xpos, int ypos)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.gene = 0;
        }

        public Creature(Habitat habitat, int xpos, int ypos, int gene)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.gene = gene;
        }

        public int Gene => this.gene;

        private float SenseUp()
        {
            if (habitat.GetPosition(xpos, ypos-1) == null)
            {
                return 0;
            }
            else
            { 
                return 1; 
            }
        }

        private float SenseDown()
        {
            if (habitat.GetPosition(xpos, ypos + 1) == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private float SenseRight()
        {
            if (habitat.GetPosition(xpos + 1, ypos) == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private float SenseLeft()
        {
            if (habitat.GetPosition(xpos - 1, ypos) == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void Action()
        {

        }
    }

    abstract class Habitat
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
        private int iModifier;
        private int modifier;
        private Creature[,] positions;


        public Habitat(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, int mutationChance, int modifier)
        {
            generation = 0;
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
            iModifier = modifier;
            this.modifier = modifier;
            positions = new Creature[length, width];
            Random randX;
            Random randY;
            int xpos;
            int ypos;
            for (int i = 0; i < creatureAmount; i++) {
                randX = new Random(i+modifier);
                randY = new Random((i + modifier) * 2);
                xpos = randX.Next(0, width);
                ypos = randY.Next(0, length);
                while (positions[ypos,xpos] != null) {
                    xpos += 1;
                    ypos += 1;
                    if (xpos >= width) { xpos = 0; }
                    if (ypos >= length) { ypos = 0; }
                }
                creatures.Add(new Creature(this, xpos, ypos));
                positions[ypos,xpos] = creatures[i];
            }
        }

        private int Modify()
        {
            Random rand = new Random(modifier);
            modifier = rand.Next(iModifier);
            return modifier;
        }

        public bool Survive(Creature creature)
        {
            return true;
        }

        public Creature GetCreature(int index)
        {
            return creatures[index];
        }

        public Creature GetPosition(int x, int y)
        {
            return positions[y,x];
        }

        public int GetGeneration()
        {
            return generation;
        }

        private int Mutate(int gene) {
            int newGene = gene;
            return newGene;
        }

        private void Reproduce(List<Creature> parents) 
        {
            creatures = new List<Creature>();
            positions = new Creature[length, width];
            Random randX;
            Random randY;
            Random rand;
            int xpos;
            int ypos;
            for (int i = 0; i < maxChild *parents.Count; i++) {
                rand = new Random(i + generation + Modify());
                if (rand.Next(0, 100) < reproduceChance && creatures.Count < creatureAmount)
                {
                    randX = new Random(i + generation + Modify());
                    randY = new Random(i + generation + Modify());
                   xpos = randX.Next(0, width);
                   ypos = randY.Next(0, length);
                   while (positions[ypos,xpos] != null) 
                   {
                       xpos += 1;
                       ypos += 1;
                       if (xpos >= width) { xpos = 0; }
                       if (ypos >= length) { ypos = 0; }
                   }
                   creatures.Add(new Creature(this, xpos, ypos, Mutate(parents[i%parents.Count].Gene)));
                   positions[ypos,xpos] = creatures[i];
                }
            }
        }

        public void LifeCycle()
        {
            for (int i = 0; i < maxTime; i++)
            {
                for (int j = 0; j < creatures.Count; j++)
                {
                    creatures[j].Action();
                }
            }

            for (int i = 0; i < creatures.Count; i++) 
            {
                if (!Survive(creatures[i])) 
                {
                    creatures.RemoveAt(i);
                    i--;
                }
            }

            List<Creature> parents = creatures;
            Reproduce(parents);
            
            
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
