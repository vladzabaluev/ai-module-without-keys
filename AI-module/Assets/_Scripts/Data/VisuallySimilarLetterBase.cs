using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisuallySimilarLetterBase
{
    public class PairLetters
    {
        public List<string> similiarLetters = new List<string>();

        public PairLetters(string[] letters)
        {
            similiarLetters.AddRange(letters);
        }

        public string GetRandomLetter()
        {
            return similiarLetters[Random.Range(0, similiarLetters.Count)];
        }
    }

    private static List<PairLetters> allPairs;
    private static List<PairLetters> uncompletedPair;
    public static List<PairLetters> UncompletedPair => uncompletedPair;

    public static void Initialize()
    {
        allPairs = new List<PairLetters>();

        allPairs.Add(new PairLetters(new string[] { "p", "q" }));
        allPairs.Add(new PairLetters(new string[] { "m", "n" }));
        allPairs.Add(new PairLetters(new string[] { "b", "d" }));
        allPairs.Add(new PairLetters(new string[] { "h", "n" }));
        allPairs.Add(new PairLetters(new string[] { "i", "j" }));
        allPairs.Add(new PairLetters(new string[] { "f", "t" }));
        allPairs.Add(new PairLetters(new string[] { "m", "w" }));

        uncompletedPair = allPairs;
    }

    public static List<string> GetPair()
    {
        int index = Random.Range(0, uncompletedPair.Count);
        PairLetters pair = uncompletedPair[index];
        uncompletedPair.Remove(pair);
        return pair.similiarLetters;
    }
}