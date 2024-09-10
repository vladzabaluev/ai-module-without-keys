using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetworksContenet
{
    public class StringDataTypes
    {
        public class LogicImageCategories
        {
            public const string Color = "Color";
            public const string Shape = "Shape";
            public const string Season = "Season";
            public const string Habitat = "Habitat";
            public const string Room = "Room";
            public const string LifeFormat = "LifeFormat";
            public const string AnimalByHomeType = "AnimalByHomeType";
            public const string Residence = "Residence";
            public const string Profession = "Profession";
            public const string WildLife = "WildLife";
            public const string AnimalByMovementMethod = "MovementMethod";
        }

        public class LifeType
        {
            public const string LivingBeing = "Living Being";
            public const string InanimateObject = "Inanimate Object";

            public static readonly string[] LifeFormatsArray = { LivingBeing, InanimateObject };
        }

        public class Color
        {
            public const string Red = "Red";
            public const string Blue = "Blue";
            public const string Green = "Green";
            public const string Orange = "Orange";
            public const string Yellow = "Yellow";
            public const string Purple = "Purple";
            public const string Pink = "Pink";
            public const string Black = "Black";
            public const string White = "White";
            public const string Brown = "Brown";

            public static readonly string[] ColorsArray = { Red, Blue, Green, Orange, Yellow, Purple, Pink, Black, White, Brown };
        }

        public class Shape
        {
            public const string Circle = "round";
            public const string Square = "square";
            public const string Triangular = "triangular";

            public static readonly string[] ShapesArray = { Circle, Square, Triangular };
        }

        public class Season
        {
            public const string Winter = "winter";
            public const string Spring = "spring";
            public const string Summer = "summer";
            public const string Autumn = "autumn";

            public static readonly string[] SeasonArray = { Winter, Spring, Summer, Autumn };
        }

        public class AnimalClass
        {
            public const string LandAnimal = "land animal";
            public const string Fish = "fish";
            public const string Bird = "bird";

            //public const string Rodent = "rodent";
            //public const string Reptile = "reptile";
            //public const string Insect = "insect";
            //public const string Mollusk = "mollusk";

            public static readonly string[] AnimalsTypeArray = { LandAnimal, Fish, Bird };
        }

        public class AnimalByHomeType
        {
            public const string WildAnimal = "wild animal";

            public const string Pet = "pet";
            public static readonly string[] AnimalsByHomeTypeArray = { WildAnimal, Pet };
        }

        public class Habitat
        {
            public const string Tropics = "tropics";
            public const string Sea = "sea";
            public const string Forest = "forest";
            public const string Farm = "farm";
            public const string Desert = "desert";
            public const string Glaciers = "glaciers";

            public static readonly string[] HabitatsArray = { Tropics, Sea, Forest, Farm, Desert, Glaciers };
        }

        public class Room
        {
            public const string Kitchen = "Kitchen";
            public const string Bedroom = "Bedroom";
            public const string Bathroom = "Bathroom";

            public static readonly string[] RoomsArray = { Kitchen, Bedroom, Bathroom };
        }

        public class Residence
        {
            public const string Farm = "farm";
            public const string City = "city";

            public static readonly string[] ResidencesArray = { Farm, City };
        }
    }
}