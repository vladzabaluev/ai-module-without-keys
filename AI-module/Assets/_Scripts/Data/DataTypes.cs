using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SocialPlatforms;

namespace NeuralNetworksContenet
{
    public class DataTypes
    {
        public enum Interest
        {
            CartoonMonsters, Dinosaurces, Dancing, Magic, Space, Cars, Drawing,
            BuilgingBlocks, Animals, Singing, Adventures, Videogames, Experiments, Candy, Nature, Science
        }

        public enum LogicImageCategoriesEnumeration
        {
            Color,
            Shape,
            Season,
            WildLife,
            Habitat,
            Room,
            LifeFormat,
            AnimalByHomeType,
            Residence,
            Profession,
            MovementMethod,
        }

        public static readonly LogicImageCategoriesEnumeration[] LogicCategoriesArray = {LogicImageCategoriesEnumeration.Color, LogicImageCategoriesEnumeration.Shape,
                LogicImageCategoriesEnumeration.Season, LogicImageCategoriesEnumeration.WildLife, LogicImageCategoriesEnumeration.Habitat,
            LogicImageCategoriesEnumeration.Room, LogicImageCategoriesEnumeration.LifeFormat, LogicImageCategoriesEnumeration.AnimalByHomeType,
            LogicImageCategoriesEnumeration.Residence, LogicImageCategoriesEnumeration.Profession, LogicImageCategoriesEnumeration.MovementMethod};

        public enum LifeType
        {
            LivingBeing,
            InanimateObject,
        }

        public enum Color
        {
            Red,
            Blue,
            Green,
            Orange,
            Yellow,
            Purple,
            Pink,
            Black,
            White,
            Brown
        }

        public class EnumArrays
        {
            public static readonly LifeType[] LifeFormats = { LifeType.LivingBeing, LifeType.InanimateObject };

            public static readonly Color[] ColorsArray = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Yellow,
                Color.Purple, Color.Pink, Color.Black, Color.White, Color.Brown };

            public static readonly Shape[] ShapesArray = { Shape.Circle, Shape.Square, Shape.Triangular };
            public static readonly Season[] SeasonArray = { Season.Winter, Season.Spring, Season.Summer, Season.Autumn };
            public static readonly Wildlife[] WildlifeArray = { Wildlife.LandAnimal, Wildlife.Fish, Wildlife.Bird, Wildlife.Plant };
            public static readonly AnimalByHomeType[] AnimalsByHomeTypeArray = { AnimalByHomeType.WildAnimal, AnimalByHomeType.Pet };
            public static readonly Habitat[] HabitatArray = { Habitat.Tropics, Habitat.Sea, Habitat.Forest, Habitat.Farm, Habitat.Desert, Habitat.Glaciers };
            public static readonly Residence[] ResidenceArray = { Residence.Farm, Residence.City };
            public static readonly Room[] RoomArray = { Room.Kitchen, Room.Bedroom, Room.Bathroom };
            public static readonly AnimalByMethodOfMovement[] MovementMethods = { AnimalByMethodOfMovement.Swimming, AnimalByMethodOfMovement.Flying, AnimalByMethodOfMovement.Walking };
        }

        public enum Shape
        {
            Circle, Square, Triangular
        }

        public enum Season
        {
            Winter, Spring, Summer, Autumn
        }

        public enum Wildlife
        {
            LandAnimal, Fish, Bird, Plant
        }

        public enum AnimalByHomeType
        {
            WildAnimal, Pet
        }

        public enum Habitat
        {
            Tropics, Sea, Forest, Farm, Desert, Glaciers
        }

        public enum Room
        {
            Kitchen, Bedroom, Bathroom
        }

        public enum Residence
        {
            Farm, City
        }

        public enum AnimalByMethodOfMovement
        {
            Flying,
            Swimming,
            Walking,
        }

        public class ProfessionsValues
        {
            public string Profession { get; set; }
            public static string[] ProfessionsArray { get; set; }
        }

        //public class WordType
        //{
        //    public const string CVC = "CVC";
        //    public const string CCVC = "CCVC";
        //    public const string CVCC = "CVCC";
        //    public const string CVCe = "CVCe";

        //    public const string Digraph = "Digraph";
        //    public const string Diphthong = "Diphthong";

        //    public const string WordsStartingWithLetters = "WordsStartingWithLetters";

        //    public static string[] CVCes = { CVC, CCVC, CVCC, CVCe };

        //    public static string[] Phonemes = { Digraph, Diphthong };
        //}

        public enum WordType
        {
            CVC, CCVC, CVCC, CVCe,

            Digraph, Diphthong, WordsStartingWithLetters
        }

        public class ImageInformation
        {
            public Texture2D Texture;
            public string Name;
            public Vector2Int Size;

            public ImageInformation(Texture2D texture, string name, string vectorSize)
            {
                Texture = texture;
                Name = name;
                Size = GetSizeFromString(vectorSize);
            }

            public Vector2Int GetSizeFromString(string size)
            {
                Vector2Int vectorSize = Vector2Int.zero;
                switch (size)
                {
                    case ImageSize.Size256:
                        vectorSize = new Vector2Int(256, 256);
                        break;

                    case ImageSize.Size512:
                        vectorSize = new Vector2Int(512, 512);
                        break;

                    case ImageSize.Size1024:
                        vectorSize = new Vector2Int(1024, 1024);
                        break;

                    case ImageSize.Size1792H:
                        vectorSize = new Vector2Int(1792, 1024);
                        break;

                    case ImageSize.Size1792V:
                        vectorSize = new Vector2Int(1024, 1792);
                        break;
                }
                return vectorSize;
            }
        }

        public class ImageType
        {
            public const string Background = "Background";
            public const string Object = "Object";
        }

        public enum ImageValidation
        {
            Correct,
            BadImage,
            BadWord,
            None
        }
    }
}