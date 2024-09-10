using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestsContainer
{
    public const string CartoonMonsters = "Cartoon monsters";
    public const string Dinosaurces = "Dinosaurces";
    public const string Dancing = "Dancing";
    public const string Magic = "Magic";
    public const string Space = "Space";
    public const string Cars = "Cars";
    public const string Drawing = "Drawing";
    public const string BuilgingBlocks = "Builging Blocks";
    public const string Animals = "Animals";
    public const string Singing = "Singing";
    public const string Adventures = "Adventures";
    public const string Videogames = "Videogames";
    public const string Experiments = "Experiments";
    public const string Candy = "Candy";
    public const string Nature = "Nature";
    public const string Science = "Science";

    public static readonly string[] Interests = { CartoonMonsters , Dinosaurces , Dancing , Magic , Space, Cars,Drawing,
        BuilgingBlocks, Animals, Singing, Adventures, Videogames,Experiments,Candy,Nature,Science};

    public string GetRandomInterest()
    {
        return Interests[Random.Range(0, Interests.Length)];
    }
}