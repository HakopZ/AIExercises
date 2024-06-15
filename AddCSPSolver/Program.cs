using System.Xml.Schema;

namespace AddCSPSolver
{
    interface IGenerator<T>
    {
        T MaxValue { get; set; }
        T MinValue { get; set; }
        T CurrentValue { get; set; }
        bool GenerateValue(out T result);
    }
    class NumberGenerator : IGenerator<int>
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int CurrentValue { get; set; }

        public NumberGenerator(int maxValue, int minValue)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            CurrentValue = minValue;
        }
        public bool GenerateValue(out int result)
        {
            result = default;

            if (CurrentValue > MaxValue) return false;

            result = CurrentValue;
            CurrentValue += 1;
            return true;
        }
    }
    class AdderSolver
    {
        static Dictionary<char, IGenerator<int>> constraintMap = new Dictionary<char, IGenerator<int>>();
        
        static void SolveAdder(string[] problem)
        {
            if (Math.Max(problem[0].Length, problem[1].Length) < problem[2].Length)
            {
                
            //    constraintMap.Add(problem[2], )
            }
            for (int i = 0; i < problem.Length; i++)
            {
             //   constraintMap.Add(problem[0])
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
