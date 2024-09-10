using NeuralNetworksContenet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNetworksContenet.DataTypes;
using static NeuralNetworksContenet.StringDataTypes;

public class EnumStringConverter
{
    //public static DataTypes.Color StringToColorConvert(string color)
    //{
    //    switch (color)
    //    {
    //        case StringDataTypes.Color.Red:
    //            {
    //                return DataTypes.Color.Red;
    //            }
    //        case StringDataTypes.Color.Blue:
    //            {
    //                return DataTypes.Color.Blue;
    //            }
    //        case StringDataTypes.Color.Green:
    //            {
    //                return DataTypes.Color.Green;
    //            }
    //        case StringDataTypes.Color.Orange:
    //            {
    //                return DataTypes.Color.Orange;
    //            }
    //        case StringDataTypes.Color.Yellow:
    //            {
    //                return DataTypes.Color.Yellow;
    //            }
    //        case StringDataTypes.Color.Purple:
    //            {
    //                return DataTypes.Color.Purple;
    //            }
    //        case StringDataTypes.Color.Pink:
    //            {
    //                return DataTypes.Color.Pink;
    //            }
    //        case StringDataTypes.Color.Black:
    //            {
    //                return DataTypes.Color.Black;
    //            }
    //        case StringDataTypes.Color.White:
    //            {
    //                return DataTypes.Color.White;
    //            }
    //        case StringDataTypes.Color.Brown:
    //            {
    //                return DataTypes.Color.Brown;
    //            }
    //    }
    //    Debug.LogError("“акого типа изображений дл€ логики не существует");
    //    throw new NullReferenceException();
    //}

    public static LogicImageCategoriesEnumeration StringToLogicCategoryConvert(string logicImageCategory)
    {
        switch (logicImageCategory)
        {
            case LogicImageCategories.Color:
                {
                    return LogicImageCategoriesEnumeration.Color;
                }
            case LogicImageCategories.Shape:
                {
                    return LogicImageCategoriesEnumeration.Shape;
                }
            case LogicImageCategories.Season:
                {
                    return LogicImageCategoriesEnumeration.Season;
                }
            case LogicImageCategories.WildLife:
                {
                    return LogicImageCategoriesEnumeration.WildLife;
                }
            case LogicImageCategories.Habitat:
                {
                    return LogicImageCategoriesEnumeration.Habitat;
                }
            case LogicImageCategories.Room:
                {
                    return LogicImageCategoriesEnumeration.Room;
                }
            case LogicImageCategories.LifeFormat:
                {
                    return LogicImageCategoriesEnumeration.LifeFormat;
                }
            case LogicImageCategories.AnimalByHomeType:
                {
                    return LogicImageCategoriesEnumeration.AnimalByHomeType;
                }
            case LogicImageCategories.Residence:
                {
                    return LogicImageCategoriesEnumeration.Residence;
                }
            case LogicImageCategories.Profession:
                {
                    return LogicImageCategoriesEnumeration.Profession;
                }
            case LogicImageCategories.AnimalByMovementMethod:
                {
                    return LogicImageCategoriesEnumeration.MovementMethod;
                }
        }
        Debug.LogError($"“акого типа изображений дл€ логики не существует {logicImageCategory}");
        throw new NullReferenceException();
    }

    public static DataTypes.ImageValidation StringToImageValidation(string imageValidation)
    {
        switch (imageValidation)
        {
            case "Correct":
                {
                    return DataTypes.ImageValidation.Correct;
                }
            case "BadImage":
                {
                    return DataTypes.ImageValidation.BadImage;
                }
            case "BadWord":
                {
                    return DataTypes.ImageValidation.BadWord;
                }
            default:
                {
                    return DataTypes.ImageValidation.None;
                }
        }
        Debug.LogError("“акого типа оценки изображений не существует");
        throw new NullReferenceException();
    }
}