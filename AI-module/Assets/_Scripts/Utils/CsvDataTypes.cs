using NeuralNetworksContenet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNetworksContenet.DataTypes;

public class CsvDataTypes
{
    public class GeneratedImage
    {
        public string Word { get; set; }
        public string ImageType { get; set; }

        public string ImageName { get; set; }

        public GeneratedImage(string word, string imageType, string name)
        {
            Word = word;
            ImageType = imageType;
            ImageName = name;
        }
    }

    public class ELAImage : GeneratedImage
    {
        public string WordType { get; set; }
        public string Interest { get; set; }

        public string StartSymbol { get; set; }

        public ELAImage(string word, string wordType, string interest, string startSymbol, string imageType, string imageName) : base(word, imageType, imageName)
        {
            WordType = wordType;
            Interest = interest;
            StartSymbol = startSymbol;
        }
    }

    public class LogicImage : GeneratedImage
    {
        public LogicImageCategoriesEnumeration Category { get; set; }
        public string ParameterValue { get; set; }

        public LogicImage(string imageType, string word, LogicImageCategoriesEnumeration category, string value, string imageName)
            : base(word, imageType, imageName)
        {
            Category = category;

            ParameterValue = value;
        }
    }

    public class TranslatedELAImage : ELAImage
    {
        public string Translation { get; set; }
        public string DeffectChar { get; set; }

        public TranslatedELAImage(string word, string wordType, string interest, string startSymbol, string imageType,
            string imageName, string translation, string deffectChar) : base(word, wordType, interest,
                startSymbol, imageType, imageName)
        {
            //Debug.Log($"Word {word}\nWordType {wordType}\nInterest {interest}\nStartSymbol " +
            //    $"{startSymbol}\nImageType {imageType}\nImageName {imageName}\nTranslation {translation}\nDeffectChair {deffectChar}");
            Translation = translation;
            DeffectChar = deffectChar;
        }
    }

    public class TranslatedLogicImage : LogicImage
    {
        public string Translation { get; set; }
        public string DeffectChar { get; set; }

        public TranslatedLogicImage(string imageType, string word, LogicImageCategoriesEnumeration parameterBeingCompared,
            string value, string imageName, string translation, string deffectChar) : base(imageType, word, parameterBeingCompared, value, imageName)
        {
            Translation = translation;
            DeffectChar = deffectChar;
        }

        public TranslatedLogicImage ChangeParameterValue(LogicImageCategoriesEnumeration newParameterBeingCompared, string newValue)
        {
            this.Category = newParameterBeingCompared;
            this.ParameterValue = newValue;
            return this;
        }
    }
}