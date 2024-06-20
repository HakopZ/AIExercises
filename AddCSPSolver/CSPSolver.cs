using System.Formats.Tar;
using System.Reflection.Metadata;
using System.Xml.Schema;

namespace AddCSPSolver
{
    public abstract class IVariable<TKey, TValue>
    {
        public TKey Representation { get; set; }
        public abstract HashSet<TValue> Domain { get; set; }
        public TValue Value { get; set; }
        public void ApplyUnaryConstraint(Predicate<TValue> valConstraint) => Domain.RemoveWhere(x => !valConstraint(x));
        public Dictionary<IVariable<TKey, TValue>[], Predicate<IVariable<TKey, TValue>[]>> Constraints { get; set; } = [];
        public Dictionary<IVariable<TKey, TValue>, Func<TValue, IVariable<TKey, TValue>, bool>> DomainConstraint { get; set; } = [];
        public void ApplyConstraint(IVariable<TKey, TValue>[] key, Predicate<IVariable<TKey, TValue>[]> predicate) => Constraints[key] = predicate;
        public void ApplyDomainConstraint(IVariable<TKey, TValue> key, Func<TValue, IVariable<TKey, TValue>, bool> predicate) => DomainConstraint[key] = predicate;
    }

    public class Variable : IVariable<char, int>
    {
        public override HashSet<int> Domain { get; set; } = Enumerable.Range(0, 10).ToHashSet();

        public Variable(char rep, List<Predicate<int>> unaryConstraints)
        {

            Representation = rep;

            foreach (var uConstraint in unaryConstraints)
            {
                ApplyUnaryConstraint(uConstraint);
            }

            Value = Domain.First();
        }

    }
    public static class CSPSolver<TRep, TValue>
    {
        public static List<List<(TRep, TValue)>> GenericSolver(List<IVariable<TRep, TValue>> variables)
        {
            List<List<(TRep, TValue)>> res = new();
            variables = variables.OrderBy(x => x.Constraints.Count + x.DomainConstraint.Count).ToList();
            Dictionary<IVariable<TRep, TValue>, HashSet<TValue>> initialDomain = [];

            foreach (var v in variables)
            {
                initialDomain[v] = new HashSet<TValue>(v.Domain);
            }
            int tracker = 0;
            helper(0, [], initialDomain, res);




            return res;

            void helper(int currVar, List<IVariable<TRep, TValue>> visited, Dictionary<IVariable<TRep, TValue>, HashSet<TValue>> domain, List<List<(TRep, TValue)>> result)
            {
                if (currVar >= variables.Count)
                {
                    List<(TRep rep, TValue value)> values = [];
                    foreach (var n in visited)
                    {
                        values.Add((n.Representation, n.Value));
                    }
                    result.Add(values);
                    return;
                }
                foreach (var constraint in variables[currVar].Constraints)
                {
                    if (constraint.Key.All(visited.Contains))
                    {
                        if (!constraint.Value(constraint.Key))
                        {
                            return;
                        }
                    }
                }
                
                

                foreach (var val in domain[variables[currVar]])
                {
                    variables[currVar].Value = val;
                    tracker++;
                    Dictionary<IVariable<TRep, TValue>, HashSet<TValue>> t = domain.Select(x => (x.Key, new HashSet<TValue>(x.Value))).ToDictionary();
                    foreach (var v in variables[currVar].DomainConstraint)
                    {
                        t[v.Key].RemoveWhere(x => !v.Value(x, variables[currVar]));
                    }

                    helper(currVar+1, [.. visited, variables[currVar]], t, result);
                }

                return;
            }
        }


        //static Dictionary<char, HashSet<int>> generatorMap = new Dictionary<char, HashSet<int>>();
        public static bool SolveAdder(string[] problem, List<char> letters, Dictionary<(char, char), List<Func<int, int, bool>>> BinaryConstraints, Dictionary<char, List<Predicate<int>>> unaryConstraints)
        {

            Dictionary<char, int> letterMapper = new();
            foreach (var l in letters)
            {
                letterMapper[l] = 0;
            }

            char[] val1 = problem[0].ToCharArray();
            char[] val2 = problem[1].ToCharArray();
            char[] result = problem[2].ToCharArray();



            for (int i = 0; i < 9; i++)
            {
                var c = new Dictionary<char, int>(letterMapper);
                c[letters[0]] = i;
                if (helper(c, 0))
                {

                    return true;
                }
            }

            return false;


            bool helper(Dictionary<char, int> map, int currKey)
            {
                if (unaryConstraints.ContainsKey(letters[currKey]))
                {

                    foreach (var constraints in unaryConstraints[letters[currKey]])
                    {
                        if (!constraints(map[letters[currKey]]))
                        {
                            return false;
                        }
                    }
                }
                for (int i = 0; i < currKey; i++)
                {

                    var bConstrains = BinaryConstraints[(letters[i], letters[currKey])];
                    foreach (var c in bConstrains)
                    {
                        if (!c(map[letters[i]], map[letters[currKey]]))
                        {
                            return false;
                        }
                    }
                }

                if (currKey + 1 == map.Count())
                {
                    if (getNumber(val1, map) + getNumber(val2, map) != getNumber(result, map))
                    {
                        return false;
                    }

                    foreach (var pair in map)
                    {
                        Console.WriteLine($"{pair.Key} : {pair.Value}");
                    }
                    Console.WriteLine();
                    return true;
                }



                for (int x = 0; x < 10; x++)
                {
                    var m = new Dictionary<char, int>(map);
                    m[letters[currKey + 1]] = x;
                    if (helper(m, currKey + 1))
                    {
                        return true;
                    }
                }


                return false;

            }

            int getNumber(char[] val, Dictionary<char, int> letterMap)
            {
                int num = 0;
                for (int i = 0; i < val.Length; i++)
                {
                    num += (int)Math.Pow(10, val.Length - i) * letterMap[val[i]];
                }
                return num;
            }
        }


    }
}
