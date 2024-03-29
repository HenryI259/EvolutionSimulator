 using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        private double weight;
        private int encoding;

        public Gene(int seed)
        {
            Random rand = new Random(seed);
            this.encoding = rand.Next(0, 268435455);

            int nums = encoding;

            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 7; i++)
            {
                int digit = (int)(nums % 0x10);
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
                nums /= 0x10;
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
            this.weight /= 512;
            this.weight -= -4;
        }

        public Gene(String encoding)
        {
            this.encoding = Int32.Parse(encoding);

            int nums = this.encoding;

            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 7; i++)
            {
                int digit = (int)(nums % 0x10);
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
                nums /= 0x10;
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
            this.weight /= 512;
            this.weight -= 4;
        }
        
        public void Mutate(int num)
        {
            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int nums = encoding;

            for (int i = 0; i < 7; i++)
            {
                int digit = (int)(nums % 0x10);
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
                nums /= 0x10;
            }

            bits[num] = bits[num] == 0 ? 1 : 0;

            int mult = 1;
            encoding = 0;
            for (int i = 0; i < 28; i++)
            {
                encoding += bits[i] * mult;
                mult *= 2;
            }

            this.startType = bits[27];
            this.endType = bits[19];

            mult = 1;
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
            this.weight /= 512;
            this.weight -= 4;
        }

        public int StartType => this.startType;

        public int EndType => this.endType;

        public int StartID => this.startID;

        public int EndID => this.endID;

        public double Weight => this.weight;

        public int Encoding => this.encoding;

    }

    class Creature
    {
        private static bool canKill = false;
        // Always the amount of input functions
        private static int inputAmount = 7;
        // Can vary
        private static int neuronAmount = 20;
        // Always the amount of output functions
        private static int outputAmount = 4 + (canKill ? 1 : 0);
        private static double[] neurons = new double[neuronAmount];
        // Can vary
        private static int connections = 100;
        private Habitat habitat;
        private Gene[] genes;
        private int xpos;
        private int ypos;
        private int facing = 0;
        private int modifier;
        
        public Creature(Habitat habitat, int xpos, int ypos, int seed)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.genes = new Gene[connections];
            this.modifier = seed;
            for (int i = 0; i < connections; i++)
            {
                this.genes[i] = new Gene(RandNum(0, 268435455));
            }

            for (int i = 0; i < neuronAmount; i++)
            {
                neurons[i] = 0;
            }
        }

        public Creature(Habitat habitat, int xpos, int ypos, Gene[] genes)
        {
            this.habitat = habitat;
            this.xpos = xpos;
            this.ypos = ypos;
            this.genes = genes;

            for (int i = 0; i < neuronAmount; i++)
            {
                neurons[i] = 0;
            }
        }

        public Gene[] Genes => genes;

        public int X => xpos;

        public int Y => ypos;

        public int RandNum(int min, int max) 
        {
            Random rand = new Random(modifier);
            modifier = rand.Next(1000000000);
            return (int)(((double)modifier / 1000000000) * (max - min)) + min;
        }

        public bool doAction(double Signal)
        {
            return (double)RandNum(0, 10000)/10000 < Signal;
        }

        public String GetGenome()
        {
            int sum = 0;
            for (int i = 0; i < connections; i++)
            {
                sum += genes[i].Encoding;
            }
            return sum.ToString("X");
        }

        private double SenseUp()
        {
            if (ypos > 0)
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
            else
            {
                return 0;
            }
        }

        private double SenseDown()
        {
            if (ypos < habitat.Length-1)
            {
                if (habitat.GetPosition(xpos, ypos+1) == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        private double SenseRight()
        {
            if (xpos < habitat.Width - 1)
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
            else
            {
                return 0;
            }
        }

        private double SenseLeft()
        {
            if (xpos > 0)
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
            else
            {
                return 0;
            }
        }

        private double SenseAge() => habitat.Time/habitat.MaxTime;

        private double SenseX() => xpos / habitat.Width;

        private double SenseY() => ypos / habitat.Length;

        private void MoveUp(bool doAction)
        {
            if (doAction)
            {
                habitat.SetPosition(xpos, ypos, null);
                if (ypos > 0 && habitat.GetPosition(xpos,ypos-1) == null) { ypos -= 1; facing = 0; }
                habitat.SetPosition(xpos, ypos, this);
            }
        }

        private void MoveDown(bool doAction)
        {
            if (doAction)
            {
                habitat.SetPosition(xpos, ypos, null);
                if (ypos < habitat.Length - 1 && habitat.GetPosition(xpos, ypos+1) == null) { ypos += 1; facing = 2; }
                habitat.SetPosition(xpos, ypos, this);
            }
        }

        private void MoveRight(bool doAction)
        {
            if (doAction)
            {
                habitat.SetPosition(xpos, ypos, null);
                if (xpos < habitat.Width - 1 && habitat.GetPosition(xpos+1, ypos) == null) { xpos += 1; facing = 1; }
                habitat.SetPosition(xpos, ypos, this);
            }
        }

        private void MoveLeft(bool doAction)
        {
            if (doAction)
            {
                habitat.SetPosition(xpos, ypos, null);
                if (xpos > 0 && habitat.GetPosition(xpos-1, ypos) == null) { xpos -= 1; facing = 3; }
                habitat.SetPosition(xpos, ypos, this);
            }
        }

        private void Kill(bool doAction)
        {
            if (doAction && facing == 1 && this.X < this.habitat.Width-1)
            {
                habitat.creatures.Remove(habitat.GetPosition(xpos + 1, ypos));
                habitat.SetPosition(xpos + 1, ypos, null);
            }

            if (doAction && facing == 3 && this.X > 0)
            {
                habitat.creatures.Remove(habitat.GetPosition(xpos - 1, ypos));
                habitat.SetPosition(xpos - 1, ypos, null);
            }

            if (doAction && facing == 2 && this.Y < this.habitat.Length - 1)
            {
                habitat.creatures.Remove(habitat.GetPosition(xpos, ypos+1));
                habitat.SetPosition(xpos, ypos + 1, null);
            }

            if (doAction && facing == 0 && this.Y > 0)
            {
                habitat.creatures.Remove(habitat.GetPosition(xpos, ypos - 1));
                habitat.SetPosition(xpos, ypos - 1, null);
            }
            
        }

        private double Input(int ID)
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
            else if (ID == 5)
            {
                return SenseX();
            }
            else if (ID == 6)
            {
                return SenseY();
            }
            else
            {
                return 0;
            }
        }

        private void Output(double[] Signals)
        {
            MoveUp(doAction(Signals[0]));
            MoveDown(doAction(Signals[1]));
            MoveRight(doAction(Signals[2]));
            MoveLeft(doAction(Signals[3]));
            if (canKill) { Kill(doAction(Signals[4])); }
        }

        public void Action()
        {
            double[] Signals = new double[outputAmount];
            for (int i = 0; i < outputAmount; i++)
            {
                Signals[i] = 0;
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 0)
                {
                    if (genes[i].EndType == 0)
                    {
                        Signals[genes[i].EndID%outputAmount] += genes[i].Weight * Input(genes[i].StartID%inputAmount);
                    }  
                    else
                    {
                        neurons[genes[i].EndID%neuronAmount] += genes[i].Weight * Input(genes[i].StartID%inputAmount);
                    }
                }
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 1 && genes[i].EndType == 1)
                {
                    neurons[genes[i].EndID%neuronAmount] += genes[i].Weight * Math.Tanh(neurons[genes[i].StartID%neuronAmount]);
                }
            }
            for (int i = 0; i < connections; i++)
            {
                if (genes[i].StartType == 1 && genes[i].EndType == 0)
                {
                    Signals[genes[i].EndID%outputAmount] += genes[i].Weight * Math.Tanh(neurons[genes[i].StartID%neuronAmount]);
                }
            }
            for (int i = 0; i < outputAmount; i++)
            {
                Signals[i] = Math.Tanh(Signals[i]);
            }
            Output(Signals);
        }
    }

    class Habitat
    {
        public List<Creature> creatures = new List<Creature>();
        private int generation;
        private int maxTime;
        private int time;
        private double mutationChance;
        private int reproduceChance;
        private int creatureAmount;
        private int maxChild;
        private int length;
        private int width;
        private int modifier;
        private Creature[,] positions;


        public Habitat(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier)
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
            modifier = rand.Next(1000000000);
            return (int)(((double)modifier/1000000000)*(max-min)) + min;
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

        public void PrintGenomes()
        {
            for (int i = 0; i < creatures.Count; i++)
            {
                Console.WriteLine(this.GetCreature(i).GetGenome());
            }
        }

        public int Generation => generation;

        public int Time => time;

        public int MaxTime => maxTime;

        public int Length => length;

        public int Width => width;

        public Gene[] Mutate(Gene[] genes) {
            Gene[] newGenes = new Gene[genes.Length];

            for (int i = 0; i < newGenes.Length; i++)
            {
                newGenes[i] = new Gene(genes[i].Encoding.ToString());
            }

            if (RandNum(0, 10000) < mutationChance*100)
            {
                newGenes[RandNum(0, newGenes.Length)].Mutate(RandNum(0, 28));
                
            }
            return newGenes;
        }

        public virtual void Reproduce(List<Creature> parents) 
        {
            creatures = new List<Creature>();
            positions = new Creature[length, width];
            int xpos;
            int ypos;
            for (int i = 0; i < maxChild*parents.Count; i++) {
                if (RandNum(0, 100) < reproduceChance && creatures.Count < creatureAmount)
                {
                   xpos = RandNum(0, width);
                   ypos = RandNum(0, length);
                   while (positions[ypos,xpos] != null) 
                   {
                       xpos += 1;
                       ypos += 1;
                       if (xpos >= width) { xpos = 0; }
                       if (ypos >= length) { ypos = 0; }
                   }
                   Creature c = new Creature(this, xpos, ypos, Mutate(parents[i % parents.Count].Genes));
                   creatures.Add(c);
                   positions[ypos,xpos] = c;
                }
            }
        }

        public void LifeCycle(bool stats)
        {
            int deaths = 0;
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
                    deaths++;
                }
            }

            List<Creature> parents = creatures;
            if (stats)
            {
                Console.WriteLine("Generation " + Generation);
                Console.WriteLine("Survived: " + parents.Count);
                Console.WriteLine("Died: " + deaths);
                Console.WriteLine();
            }
            Reproduce(parents);
            generation += 1;
        }

        public void GetToGeneration(int gen, bool stats)
        {
            for (int i = generation; i < gen; i++)
            {
                LifeCycle(stats);
            }
        }

        public void VisualLifeCycle()
        {
            for (int i = 0; i < maxTime; i++)
            {
                ShowHabitat();
                for (int j = 0; j < creatures.Count; j++)
                {
                    creatures[j].Action();
                    time = j;
                }
                //Console.Read();
                //Console.Clear();
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
            char[,] hab = new char[this.length, this.width];

            for (int i = 0; i < this.length; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (positions[i,j] != null)
                    {
                        Console.Write("o");
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }
                Console.WriteLine();
            }
            for (int i = 0;i < this.width; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    class Habitat1 : Habitat
    {
        public Habitat1(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier) : base(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier)
        {
            
        }
        public override bool Survive(Creature creature)
        {
            return creature.X < this.Width/2;
        }
    }

    class Habitat2 : Habitat
    {
        public Habitat2(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier) : base(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier)
        {

        }
        public override bool Survive(Creature creature)
        {
            if (creature.Y > 0)
            {
                if (this.GetPosition(creature.X, creature.Y - 1) != null)
                {
                    return false;
                }
            }

            if (creature.Y < this.Length - 1)
            {
                if (this.GetPosition(creature.X, creature.Y + 1) != null)
                {
                    return false;
                }
            }

            if (creature.X > 0)
            {
                if (this.GetPosition(creature.X-1, creature.Y) != null)
                {
                    return false;
                }
            }

            if (creature.X < this.Width - 1)
            {
                if (this.GetPosition(creature.X + 1, creature.Y) != null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    class Habitat3 : Habitat
    {
        public Habitat3(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier) : base(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier)
        {

        }
        public override bool Survive(Creature creature)
        {
            if (creature.Y > 0)
            {
                if (this.GetPosition(creature.X, creature.Y - 1) != null)
                {
                    return true;
                }
            }

            if (creature.Y < this.Length - 1)
            {
                if (this.GetPosition(creature.X, creature.Y + 1) != null)
                {
                    return true;
                }
            }

            if (creature.X > 0)
            {
                if (this.GetPosition(creature.X - 1, creature.Y) != null)
                {
                    return true;
                }
            }

            if (creature.X < this.Width - 1)
            {
                if (this.GetPosition(creature.X + 1, creature.Y) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class Habitat4 : Habitat 
    {
        public Habitat4(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier) : base(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier)
        {

        }

        public override bool Survive(Creature creature)
        {
            return ((creature.Y - this.Length / 2) * (creature.Y - this.Length / 2) + (creature.X - this.Width / 2) * (creature.X - this.Width / 2)) > 600;
        }
    }

    class Habitat5 : Habitat
    {
        public Habitat5(int length, int width, int maxTime, int creatureAmount, int maxChild, int reproduceChance, double mutationChance, int modifier) : base(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier)
        {

        }

        public override bool Survive(Creature creature)
        {
            return (this.Width - creature.X) < 5 || (creature.X) < 5;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            int length = 100;
            int width = 100;
            int maxTime = 200;
            int creatureAmount = 1000;
            int maxChild = 100;
            int reproduceChance = 100;
            double mutationChance = 0.1;
            int modifier = 1;
            Habitat5 habitat = new Habitat5(length, width, maxTime, creatureAmount, maxChild, reproduceChance, mutationChance, modifier);
            habitat.GetToGeneration(4000, true);
            habitat.VisualLifeCycle();
            Console.ReadLine();
        }
    }
}
//Ikigai