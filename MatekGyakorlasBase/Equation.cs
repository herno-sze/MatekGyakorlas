using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

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

        public string NewEquation(int Size, char Operator, int MaxResult)
        {
            this.Operator = Operator;
            this.Size = Size;
            this.Constants = new int[6];

            Result = 0;
            int a = 0;
            int b = 0;
            for (int i = 0; i < Size; i++)
            {
                Constants[i] = random.Next(MaxResult - 1) + 1;

                switch (Operator)
                {
                    case '-':
                        if (i == 0)
                            Result += Constants[i];
                        else
                        {
                            Constants[i] = Constants[i] / ((i + 1) * 2);
                            Result -= Constants[i];
                        }
                        break;
                    case '*':
                        if (i == 0)
                        {
                            a = random.Next(10 - 1) + 1; // 1..9
                            b = random.Next(1000 / a) + 1; // a * b = 1..999
                            Debug.Print(a + "  * " + b + " = " + (a * b));
                            Constants[0] = b;
                            //Constants[1] = a;
                            Result = b * a;
                        }
                        else if (i == 1)
                        {
                            Constants[1] = a;
                        }
                        //else
                        //    Result *= Constants[i];
                        break;
                    case '%':
                        if (i == 0)
                        {
                            a = random.Next(10 - 2) + 2; // 2..9
                            b = random.Next(1000 / a) + 1; // a * b = 1..999
                            //Debug.Print(a + "  * " + b + " = " + (a * b));
                            Constants[0] = a * b;
                            Result = b;
                        }
                        else if (i == 1)
                        {
                            Constants[1] = a;
                        }
                        break;
                    default: //összeadás
                        Result += Constants[i];
                        break;
                }

            }
            // ha negatív lenne a kivonások eredménye
            if (Result < 0 && Operator == '-')
            {
                Constants[0] += -2 * Result;
                Result += -2 * Result;
            }

            string ReturnEq = "";
            for (int i = 0; i < Size; i++)
                ReturnEq += " " + Constants[i] + " " + Operator;
            ReturnEq = ReturnEq.Remove(ReturnEq.Length - 1, 1) + "=";
            //ReturnEq += " " + Result; // debug
            return ReturnEq;
        }
    }
}
