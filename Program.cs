 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSimulator
{
    class Gene
    {
        private int startType;
        private int endType;
        private int startID;
        private int endID;
        private int weight;
        private int encoding;

        public Gene(int seed)
        {
            Random rand = new Random(seed);
            this.encoding = rand.Next(0, 268435456);
            

            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 7; i++)
            {
                int digit = (int)(encoding % 0x10);
                if (digit >= 8)
                {
                    bits[i * 4 + 3] = 1;
                    digit -= 8;
                }
                if (digit >= 4)
                {
                    bits[i * 4 + 2] = 1;
                    digit -= 4;
                }
                if (digit >= 2)
                {
                    bits[i * 4 + 1] = 1;
                    digit -= 2;
                }
                if (digit >= 1)
                {
                    bits[i * 4] = 1;
                }
                encoding /= 0x10;
            }

            this.startType = bits[27];
            this.endType = bits[19];

            int mult = 1;
            this.startID = 0;
            this.endID = 0;
            for (int i = 12; i < 19; i++)
            {
                this.startID += bits[i + 8] * mult;
                this.endID += bits[i] * mult;
                mult *= 2;
            }

            mult = 1;
            this.weight = 0;
            for (int i = 0; i < 12; i++)
            {
                this.weight += bits[i] * mult;
                mult *= 2;
            }
        }
        
        public Mutate(int num)
        {
            int mutation = 1;
            for (int i = 0; i < num; i++){
              mutation *= 2;
            }
            encoding += mutation;
            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 7; i++)
            {
                int digit = (int)(encoding % 0x10);
                if (digit >= 8)
                {
                    bits[i * 4 + 3] = 1;
                    digit -= 8;
                }
                if (digit >= 4)
                {
                    bits[i * 4 + 2] = 1;
                    digit -= 4;
                }
                if (digit >= 2)
                {
                    bits[i * 4 + 1] = 1;
                    digit -= 2;
                }
                if (digit >= 1)
                {
                    bits[i * 4] = 1;
                }
                encoding /= 0x10;
            }

            this.startType = bits[27];
            this.endType = bits[19];

            int mult = 1;
            this.startID = 0;
            this.endID = 0;
            for (int i = 12; i < 19; i++)
            {
                this.startID += bits[i + 8] * mult;
                this.endID += bits[i] * mult;
                mult *= 2;
            }

            mult = 1;
            this.weight = 0;
            for (int i = 0; i < 12; i++)
            {
                this.weight += bits[i] * mult;
                mult *= 2;
            }
        }

        public int StartType => this.startType;

        public int EndType => this.endType;

        public int StartID => this.startID;

        public int EndID => this.endID;

        public int Weight => this.weight;

    }
    
    class Creature
    {
        private Habitat habitat;
        private int[] genes;
        private int inputAmount = 5;
        private int neuronAmount = 3;
        private int outputAmount = 4;
        private int[] neurons = new int[neuronAmount];
        private int connections = 8;
        private int xpos;
        private int ypos;
        public Creature(Habitat habitat, int xpos, int ypos, int seed)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.genes = new int[connections];
            Random rand;
            for (int i = 0; i < connections; i++)
            {
                rand = new Random(seed);
                this.genes[i] = new Gene(rand.Next(0, 268435456));
                seed += 1;
            }

            for (int i = 0; i < this.neuronAmount, i++)
            {
                neurons[i] = 0;
            }
        }

        public Creature(Habitat habitat, int xpos, int ypos, int[] genes)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.genes = genes;

            for (int i = 0; i < this.neuronAmount, i++)
            {
                neurons[i] = 0;
            }
        }

        public int[] Genes => genes;

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

        private float SenseAge() => habitat.Time/habitat.MaxTime;

        private void MoveUp(int Signal)
        {
            habitat.SetPosition(xpos, ypos, null);
            ypos -= 1;
            habitat.SetPosition(xpos, ypos, this);
        }

        private void MoveDown(int Signal)
        {
            habitat.SetPosition(xpos, ypos, null);
            ypos += 1;
            habitat.SetPosition(xpos, ypos, this);
        }

        private void MoveRight(int Signal)
        {
            habitat.SetPosition(xpos, ypos, null);
            xpos += 1;
            habitat.SetPosition(xpos, ypos, this);
        }

        private void MoveLeft(int Signal)
        {
            habitat.SetPosition(xpos, ypos, null);
            xpos -= 1;
            habitat.SetPosition(xpos, ypos, this);
        }

        private float Input(int ID)
        {
            if (ID == 0)
            {
                return SenseUp();
            }
            else if (ID == 1)
            {
                return SenseDown();
            }
            else if (ID == 2)
            {
                return SenseRight();
            }
            else if (ID == 3)
            {
                return SenseLeft();
            }
            else if (ID == 4)
            {
                return SenseAge();
            }
            else
            {
                return 0;
            }
        }

        private void Output(int[] Signals)
        {
            MoveUp(Signals[0]);
            MoveDown(Signals[1]);
            MoveRight(Signals[2]);
            MoveLeft(Signals[3]);
        }

        public void Action()
        {
            int[] Signals = new int[outputAmount];
            for (int i = 0; i < this.outputAmount, i++)
            {
                Signals[i] = 0;
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 0) 
                {
                    if (genes[i].EndType == 0)
                    {
                        Signals[genes[i].EndID%outputAmount] += genes[i].weight * Input(genes[i].StartID%inputAmount);
                    }  
                    else
                    {
                        neurons[genes[i].EndID%neuronAmount] += genes[i].weight * Input(genes[i].StartID%inputAmount);
                    }
                }
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 1 && genes[i].EndType == 1)
                {
                    neurons[genes[i].EndID%neuronAmount] += genes[i].weight * neurons[genes[i].StartID%neuronAmount];
                }
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 1 && genes[i].EndType == 0)
                {
                    Signals[genes[i].EndID%outputAmount] += genes[i].weight * neurons[genes[i].StartID%neuronAmount];
                }
            }
            Output(Signals);
        }
    }

    virtual class Habitat
    {
        private List<Creature> creatures = new List<Creature>();
        private int generation;
        private int maxTime;
        private int time;
        private int mutationChance;
        private int reproduceChance;
        private int creatureAmount;
        private int maxChild;
        private int length;
        private int width;
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
            this.modifier = modifier;
            positions = new Creature[length, width];
            int xpos;
            int ypos;
            for (int i = 0; i < creatureAmount; i++) {
                xpos = RandNum(0, width);
                ypos = RandNum(0, length);
                while (positions[ypos,xpos] != null) {
                    xpos += 1;
                    ypos += 1;
                    if (xpos >= width) { xpos = 0; }
                    if (ypos >= length) { ypos = 0; }
                }
                creatures.Add(new Creature(this, xpos, ypos, RandNum(0, 1000000)));
                positions[ypos,xpos] = creatures[i];
            }
        }

        private int RandNum(int min, int max)
        {
            Random rand = new Random(modifier);
            modifier = rand.Next(1000000);
            return (int)((modifier/1000000)*(max-min)) + min;
        }

        public virtual bool Survive(Creature creature)
        {
            return true;
        }

        public Creature GetCreature(int index) => creatures[index];

        public Creature GetPosition(int x, int y) => positions[y, x];

        public void SetPosition(int x, int y, Creature creature)
        {
            positions[y, x] = creature;
        }

        public int Generation => generation;

        public int Time => time;

        public int MaxTime => maxTime;

        private int[] Mutate(int[] genes) {
            if (RandNum(0, 10000) < mutationChance*100)
            {
                 genes[i].Mutate(RandNum(0, 32));
            }
            return newGene;
        }

        private virtual void Reproduce(List<Creature> parents) 
        {
            creatures = new List<Creature>();
            positions = new Creature[length, width];
            int xpos;
            int ypos;.
            for (int i = 0; i < maxChild *parents.Count; i++) {
                if (RandNum(0, 100) < reproduceChance && creatures.Count < creatureAmount)
                {
                   xpos = RandNext(0, width);
                   ypos = RandNum.(0, length);
                   while (positions[ypos,xpos] != null) 
                   {
                       xpos += 1;
                       ypos += 1;
                       if (xpos >= width) { xpos = 0; }
                       if (ypos >= length) { ypos = 0; }
                   }
                   creatures.Add(new Creature(uyfuy..this, xpos, ypos, Mutate(parents[i%parents.Count].Genes)));.
                   positions[ypos,xpos] = creatures[i];
                }
            }
        }

        private void LifeCycle()
        {
            for (int i = 0; i < maxTime; i++)
            {
                for (int j = 0; j < creatures.Count; j++)
                {
                    creatures[j].Action();
                    time = j;
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
            generation += 1;
        }

        public void GetToGeneration(int gen)
        {
            for (int i = generation; i < gen; i++)
            {
                LifeCycle();
            }
        }

        public void VisualLifeCycle()
        {
            for (int i = 0; i < maxTime; i++)
            {
                ShowHabitat();
                for (int j = .0; j < creatures.Count; j++)
                {
                    creatures[j].Action();
                    time = j;
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
            generation += 1;
            
        }

        private void ShowHabitat()
        {
          
        }
    }

    class Habitat1 : Habitat
    {
        private override bool Survive(Creature creature)
        {
            return creature.xpos > this.length/2;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Habitat1 habitat = new Habitat1(500, 500, 500, 250, 3, 50, 1);
            habitat.
            
            Console.WriteLine(0xFFFFFFF);
        }
    }
}
