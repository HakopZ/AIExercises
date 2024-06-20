using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;

namespace AddCSPSolver
{

    internal class Program
    {

        static bool GreaterThan5(int val) => val > 5;
        static bool IsEven(int val) => val % 2 == 0;
        static bool LessThanTwo(int val) => val < 2;
        static bool NonZero(int val) => val != 0;

        static bool NotSame(int val1, int val2) => val1 != val2;
        static bool DoubleTheVal(int val1, int val2) => val1 * 2 == val2;
        static bool DoubleTheValFindDigit(int val1, int val2) => (val1 * 2 % 10) == val2;

        static bool NotSame(IVariable<char, int>[] t) => t[0].Value != t[1].Value;

        static void SendMoreMoney()
        {
            Predicate<int> notNine = (int x) => x != 9;
            Predicate<int> between1and9 = (int x) => x > 1 && x < 9;
            Predicate<int> isOne = (x) => x == 1;
            Predicate<int> isZero = (x) => x == 0;
            Variable S = new Variable('S', [(x => !notNine(x))]);
            Variable E = new Variable('E', [between1and9]);
            Variable N = new Variable('N', [between1and9]);
            Variable D = new Variable('D', [between1and9]);
            Variable M = new Variable('M', [isOne]);

            Variable O = new Variable('O', [(x => x == 0)]);
            Variable R = new Variable('R', [between1and9]);
            Variable Y = new Variable('Y', [NonZero]);

            Variable C1 = new Variable('C', [LessThanTwo]);
            Variable C2 = new Variable('X', [isZero]);
            Variable C3 = new Variable('Z', [isOne]);

        }

        static void TwoPlusTwo()
        {
            Variable T = new Variable('T', [GreaterThan5, NonZero]);
            Variable W = new Variable('W', []);
            Variable O = new Variable('O', [NonZero]);
            Variable F = new Variable('F', [NonZero, LessThanTwo]);
            Variable U = new Variable('U', []);
            Variable R = new Variable('R', [IsEven]);
            Variable c1 = new Variable('C', [LessThanTwo]);
            Variable c2 = new Variable('D', [LessThanTwo]);


            T.ApplyConstraint([T, W], NotSame);
            T.ApplyConstraint([T, O], NotSame);
            T.ApplyConstraint([T, F], NotSame);
            T.ApplyConstraint([T, U], NotSame);
            T.ApplyConstraint([T, R], NotSame);

            T.ApplyConstraint([T, O, c2], x => ((x[0].Value * 2) + x[2].Value) % 10 == x[1].Value);
            O.ApplyConstraint([T, O, c2], x => ((x[0].Value * 2) + x[2].Value) % 10 == x[1].Value);
            c2.ApplyConstraint([c2, T, O], x => (x[0].Value + (x[1].Value * 2)) % 10 == x[2].Value);



            W.ApplyConstraint([W, T], NotSame);
            W.ApplyConstraint([W, O], NotSame);
            W.ApplyConstraint([W, F], NotSame);
            W.ApplyConstraint([W, U], NotSame);
            W.ApplyConstraint([W, R], NotSame);

            W.ApplyConstraint([W, U, c1], x => ((x[0].Value * 2) + x[2].Value) % 10 == x[1].Value);
            U.ApplyConstraint([c1, W, U], x => (x[0].Value + (x[1].Value * 2)) % 10 == x[2].Value);
            c1.ApplyConstraint([c1, W, U], x => (x[0].Value + (x[1].Value * 2)) % 10 == x[2].Value);

            O.ApplyConstraint([O, T], NotSame);
            O.ApplyConstraint([O, W], NotSame);
            O.ApplyConstraint([O, F], NotSame);
            O.ApplyConstraint([O, U], NotSame);
            O.ApplyConstraint([O, R], NotSame);
            O.ApplyConstraint([O, R], x => x[0].Value * 2 % 10 == x[1].Value);

            F.ApplyConstraint([F, T], NotSame);
            F.ApplyConstraint([F, W], NotSame);
            F.ApplyConstraint([F, O], NotSame);
            F.ApplyConstraint([F, U], NotSame);
            F.ApplyConstraint([F, R], NotSame);

            U.ApplyConstraint([U, T], NotSame);
            U.ApplyConstraint([U, W], NotSame);
            U.ApplyConstraint([U, F], NotSame);
            U.ApplyConstraint([U, O], NotSame);
            U.ApplyConstraint([U, R], NotSame);

            R.ApplyConstraint([R, W], NotSame);
            R.ApplyConstraint([R, T], NotSame);
            R.ApplyConstraint([R, F], NotSame);
            R.ApplyConstraint([R, U], NotSame);
            R.ApplyConstraint([R, O], NotSame);
            R.ApplyConstraint([R, O], x => x[1].Value * 2 % 10 == x[0].Value);

            c1.ApplyConstraint([c1, O], x => x[1].Value * 2 / 10 == x[0].Value);

            c2.ApplyConstraint([c2, W, c1], x => (x[1].Value * 2 + x[2].Value) / 10 == x[0].Value);

            List<char> letters = ['T', 'W', 'O', 'F', 'U', 'R', 'C', 'D'];
            //var result = AdderSolver.SolveAdder(problem, letters, binaryConstraints, constraints);

            //var res = CSPSolver.GenericSolver([T, W, O, F, U, R, c1, c2]);

            //  Console.WriteLine(res);
        }

        static void ColorGraph()
        {
            IVariable<Point, Color>[,] colorVariables = new IVariable<Point, Color>[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    colorVariables[i, j] = new ColorVariable(new Point(i, j));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j + 1 < 3)
                    {
                        colorVariables[i, j].ApplyConstraint([colorVariables[i, j], colorVariables[i, j + 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i, j].ApplyDomainConstraint(colorVariables[i, j + 1], (x, y) => y.Value != x);
                        colorVariables[i, j + 1].ApplyConstraint([colorVariables[i, j], colorVariables[i, j + 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i, j + 1].ApplyDomainConstraint(colorVariables[i, j], (x, y) => y.Value != x);
                    }
                    if (i + 1 < 3)
                    {
                        colorVariables[i, j].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j]], x => x[0].Value != x[1].Value);
                        colorVariables[i, j].ApplyDomainConstraint(colorVariables[i + 1, j], (x, y) => y.Value != x);

                        colorVariables[i + 1, j].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j]], x => x[0].Value != x[1].Value);
                        colorVariables[i + 1, j].ApplyDomainConstraint(colorVariables[i, j], (x, y) => y.Value != x);
                    }
                    if (i + 1 < 3 && j + 1 < 3)
                    {
                        colorVariables[i, j].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j + 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i, j].ApplyDomainConstraint(colorVariables[i + 1, j + 1], (x, y) => y.Value != x);

                        colorVariables[i + 1, j + 1].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j + 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i + 1, j + 1].ApplyDomainConstraint(colorVariables[i, j], (x, y) => y.Value != x);
                    }
                    if (i + 1 < 3 && j - 1 > -1)
                    {
                        colorVariables[i, j].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j - 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i, j].ApplyDomainConstraint(colorVariables[i + 1, j - 1], (x, y) => y.Value != x);

                        colorVariables[i + 1, j - 1].ApplyConstraint([colorVariables[i, j], colorVariables[i + 1, j - 1]], x => x[0].Value != x[1].Value);
                        colorVariables[i + 1, j - 1].ApplyDomainConstraint(colorVariables[i, j], (x, y) => y.Value != x);
                    }
                }
            }

            var res = CSPSolver<Point, Color>.GenericSolver(colorVariables.Cast<IVariable<Point, Color>>().ToList());
            ;
        }
        static void AdvancedGraph()
        {

            //List<Point, Color> 
        }
        static void Main(string[] args)
        {


            ColorGraph();


        }
    }
}
