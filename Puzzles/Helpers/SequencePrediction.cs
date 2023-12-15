using System.Collections;
using NUnit.Framework.Constraints;

namespace TestProject1.Helpers;

public static class SequencePrediction
{
    public static IEnumerable<long> Differences(IEnumerable<long> input)
    { 
        var inputArray = input.ToArray();
        
        var result = new List<long>();
        for (var i = 1; i < inputArray.Length; i++)
        {
            var diff = inputArray[i] - inputArray[i-1];
            result.Add(diff);
        }

        return result;
    }

    public static IEnumerable<IEnumerable<long>> Recurse(string sequence)
    {
        var result = new List<IEnumerable<long>>();
        var sequenceNumbers = sequence.Split(" ").Select(long.Parse).ToArray();
        return Recurse(sequenceNumbers, result);
    }

    private static IEnumerable<IEnumerable<long>> Recurse(IEnumerable<long> sequenceNumbers, ICollection<IEnumerable<long>> list)
    {
        var array = sequenceNumbers.ToArray();
        
        var differences = Differences(array).ToArray();
        if (differences.All(i => i == 0))
        {
            if (!list.Any())
            {
                list.Add(array);
            }
            return list;
        }
        
        if(list.Count == 0) list.Add(array);
        list.Add(differences);
        return Recurse(differences,list);
    }

    public static long Prediction(IEnumerable<IEnumerable<long>> differences)
    {
        var arr = differences.ToArray();
        if (arr.Length == 1) return arr.First().Last();
        
        return arr.First().Last() + Prediction(arr.Skip(1));
    }


    public static long PredictionPrevious(IEnumerable<IEnumerable<long>> differences)
    {
        var arr = differences.ToArray();
        if (arr.Length == 1) return arr.First().First();
        return arr.First().First() - PredictionPrevious(arr.Skip(1));
    }
}