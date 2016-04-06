using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MatekGyakorlas
{
    class Equation
    {
        private char Operator = '+';
        private int Size = 2;
        private int[] Constants;
        public int Result = 0;
        Random random;

        public Equation()
        {
            random = GetThreadRandom();
        }

        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref seed))
        );

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }

        public string NewEquation(int Size, char Operator)
        {
            this.Operator = Operator;
            this.Size = Size;
            this.Constants = new int[6];

            Result = 0;
            for (int i = 0; i < Size; i++)
            {
                Constants[i] = random.Next(99) + 1;

                switch (Operator)
                {
                    case '-':
                        if (i == 0)
                            Result += Constants[i];
                        else
                            Result -= Constants[i];
                        break;
                    default:
                        Result += Constants[i];
                        break;
                }

            }
            string ReturnEq = "";
            for (int i = 0; i < Size; i++)
                ReturnEq += " " + Constants[i] + " " + Operator;
            ReturnEq = ReturnEq.Remove(ReturnEq.Length - 1, 1) + "=";
            // ReturnEq += " " + Result; // debug
            return ReturnEq;
        }
    }
}
