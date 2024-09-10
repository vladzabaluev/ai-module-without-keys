using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace NeuralNetworksContenet
{
    public class TextGenerator
    {
        private static OpenAIApi openai = new OpenAIApi();

        private static List<ChatMessage> messages = new List<ChatMessage>();

        private const string LogicGuidlines = "- Guidelines:" +
                " * Children should know these words," +
                " * The word must be easily recognizable for children," +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Do not use emojis, stickers, or any other types of images.";

        [SerializeField] private TMP_Text text;

        private static void SetGPTRole(string role)
        {
            var setRoleMessage = new ChatMessage()
            {
                Role = "system",
                Content = $"You are {role}"
            };
            messages.Add(setRoleMessage);
        }

        public static async Task<string> GetRewrittenTask(string task, string name, int age)
        {
            string newTaskText = "";

            SetGPTRole("the playmate.");
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = $"Rewrite the following task for a {age}-year-old child using child name: " +
                $"-Name: {name}" +
                $"- Task: {task}" +
                "- Guidelines:" +
                "* Add some playfulness to the text of the task" +
                "* Answer must contain only the text of the rewritten task." +
                "* Maximum answer length is 100 characters." +
                "* Do not use emojis, stickers, or any other types of images." +
                "* Use simple words"
            };

            messages.Add(userMessage);
            newTaskText = await GetAnswerFromGPT(0.8f, 2f, 0.7f);
            messages.Clear();
            //�������� �������� �� ������ ���������
            //Text.Text = newTaskText;
            return newTaskText;
        }

        public static async Task<string> GetSubtask(string subtask, int excessWordCount)
        {
            string newTaskText = "";
            if (!subtask.Contains('*'))
            {
                Debug.LogError("Subtask doesn't contain * symbolizing the number ");
                return subtask;
            }
            string fullSubTask = subtask.Replace("*", excessWordCount.ToString());
            SetGPTRole("the playmate.");
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = $"Rewrite the following task to make it easy-to-understand for a baby : " +
                $"- Task: {fullSubTask}" +
                "- Guidelines:" +
                "* Add some playfulness to the text of the task" +
                "* Answer must contain only the text of the rewritten task." +
                "* Maximum answer length is 100 characters." +
                "* Don't write any comments, don't write \"Task :\"," +
                "* Do not use emojis, stickers, or any other types of images." +
                "* Use simple words"
            };

            messages.Add(userMessage);
            newTaskText = await GetAnswerFromGPT(0.8f, 1f, 0.7f);
            messages.Clear();

            return newTaskText;
        }

        #region ELA words

        public static async Task<string> GetWorldStartingWithLetter(int generatedExamplesCount, string letter, List<string> exludingWords)
        {
            SetGPTRole("a children's dictionary.");
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = $"Generate {generatedExamplesCount} word examples according to the following criteria and will be easy to understand for six-years-old child." +
                "- Guidelines:" +
                $"* Starting with '{letter}'" +
                " * Easy to understand for a baby " +
                " * Maximum word length is 7 symbols," +
                " * Appropriate and safe for a child audience" +
                " * The words must be a noun in singular form, " +
                " * Answer must contain only numbered list of generated words without any comments " +
                " * Without using emojis, stickers or images. " +
                $"* The answer must not contain: {TextHandler.GetStringFromList(exludingWords)}"
            };

            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 2f, 0.5f);

            return GetOutputData(words, exludingWords);
        }

        public static async Task<string> GetWordByType(int generatedExamplesCount, string wordType, List<string> exludingWords)
        {
            SetGPTRole("a knowledge facilitator.");
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {generatedExamplesCount} of {wordType} NOUNS that will be easy to understand for a child. And follow the next guidlines: " +
                "Appropriate and safe for a child audience." +
                "The words must be a noun in singular form, please don't write any verbs." +
                "The nouns must be easy to draw." +
                $"Answer must contain only numbered list of {wordType}." +
                "Please don't write any comments. Don't use emojis, stickers or images" +
                 $"* The answer must not contain: {TextHandler.GetStringFromList(exludingWords)}"
            };
            messages.Add(userMessage);
            Debug.Log(userMessage.Content);
            string words = await GetAnswerFromGPT(0.7f, 1f, 0.5f);

            return GetOutputData(words, exludingWords);
        }

        public static async Task<string> GetPhoneme(int generatedExamplesCount, string wordType, List<string> exludingWords)
        {
            SetGPTRole("a children's dictionary.");

            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write a list of {generatedExamplesCount} {wordType}s that will be easy for a six-years-old child to understand." +
                "- Guidelines:" +
                " * Appropriate and safe for a child audience'" +
                " * The words must be a noun in singular form, " +
                " * Answer must contain only numbered list of words " +
                " * Don't write any comments just list of words" +
                " * Without using emojis, stickers or images. " +
                $"* The answer must not contain: {TextHandler.GetStringFromList(exludingWords)}"
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 2f, 0.5f);

            return GetOutputData(words, exludingWords);
        }

        #endregion ELA words

        #region VisualDescriptions

        public static async Task<string> GetObjectDescriptionByInterest(int exampleCount, string interest, List<string> descriptions)
        {
            SetGPTRole("the drawer");
            var getPreferenceExample = new ChatMessage()
            {
                Role = "user",

                Content = $"Write {exampleCount} very short visual description of an object or character based on the provided child's interest" +
                $"- Interest: {interest} " +
                "- Guidelines:" +
                    "* Kids safe, " +
                    "* Max length of description: 10 words, " +
                    "* Use only singular form, " +
                    "* Don't use uncountable nouns and mass nouns in answer, " +
                    "* Answer must contain only one described object or character," +
                    "* Do not use emojis, stickers, or any other types of images."
            };
            messages.Add(getPreferenceExample);
            string description = await GetAnswerFromGPT(0.8f, 2, 0.5f);

            messages.Clear();
            return description;
        }

        //public static async Task<string> GetShortDescriptionOf(string drawedObject)
        //{
        //    SetGPTRole("the drawer");
        //    string description = "";
        //    var getPreferenceExample = new ChatMessage()
        //    {
        //        Role = "user",
        //        Content = $"Write a very short visual description of {drawedObject}" +
        //        "- Guidelines:" +
        //            "* Kids safe, " +
        //            "* Max length of description: 10 words, " +
        //            "* Use only singular form, " +
        //            //"* Don't use uncountable nouns and mass nouns in answer, " +
        //            //"* Answer must contain only one described object or character," +

        //            "* Do not use emojis, stickers, or any other types of images."
        //    };

        //    messages.Add(getPreferenceExample);
        //    description = await GetAnswerFromGPT(0.8f, 2, 0.5f);
        //    Debug.Log("������ �����...");
        //    messages.Clear();
        //    return description;
        //}

        public static async Task<string> GetShortDescriptionOf(string drawedObject, List<string> additionalParameters)
        {
            SetGPTRole("an Art Director");
            string description = "";

            var getVisualDescriptionMessage = new ChatMessage()
            {
                Role = "user",
                Content = $"Describe a natural picture of the {TextHandler.GetStringFromList(additionalParameters)} {drawedObject} as if you're asking an artist to illustrate it. " +
                $"Capture its three-dimensional essence, depicting it realistically for a child. Place it against a white background. " +
                $"Keep the description within 50 words.Center the object in the image, avoiding any overlap with the borders."
            };

            messages.Add(getVisualDescriptionMessage);
            description = await GetAnswerFromGPT(0.6f, 1, 0.5f);

            messages.Clear();
            return description;
        }

        public static async Task<string> GetObjectDescriptionByInterest(string interest)
        {
            SetGPTRole("an Art Director");
            var getPreferenceExample = new ChatMessage()
            {
                Role = "user",
                Content = $"Describe a natural picture of the any object accociated with {interest} as if you're asking an artist to illustrate it. " +
                $"Capture its three-dimensional essence, depicting it realistically for a child. Place it against a white background. " +
                $"Keep the description within 50 words. Center the object in the image, avoiding any overlap with the borders."
            };
            messages.Add(getPreferenceExample);
            string description = await GetAnswerFromGPT(0.8f, 1, 0.5f);

            messages.Clear();
            return description;
        }

        public static async Task<string> GetBackgroundDescriptionByInterest(string interest)
        {
            string description = "";
            var getPreferenceExample = new ChatMessage()
            {
                Role = "user",
                Content = "Write visual description for the picture based on the provided child's interest " +
                $"- Interest: {interest} " +
                "- Guidelines:" +
                    "* Kids safe, " +
                    "* Max answer length is 2500 symbols," +
                    "* Do not use emojis, stickers, or any other types of images."
            };
            Debug.Log($"The interest is {interest}");
            messages.Add(getPreferenceExample);
            description = await GetAnswerFromGPT(0.8f, 1, 0.5f);
            messages.Clear();

            return description;
        }

        public static async Task<string> GetBackgroundDescription(string sphere)
        {
            string description = "";
            var getPreferenceExample = new ChatMessage()
            {
                Role = "user",
                Content = $"Write visual description of the painting that depicts the {sphere} " +
                "- Guidelines:" +
                    "* Kids safe, " +
                    "* Max answer length is 2500 symbols," +
                    "* Do not use emojis, stickers, or any other types of images."
            };
            Debug.Log($"The sphere is {sphere}");
            messages.Add(getPreferenceExample);
            description = await GetAnswerFromGPT(0.8f, 1, 0.5f);
            messages.Clear();

            return description;
        }

        #endregion VisualDescriptions

        public static async Task<string> GetLivingBeing(int examplesCount)
        {
            SetGPTRole(" a connoisseur of all living creatures and characters.");

            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of living beings or characters based on the child's interest " +
                "- Guidelines:" +
                " * Don't use uncountable nouns and mass nouns in answer," +
                " * Kids safe, " +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Without using emojis, stickers or images. "
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 2f, 0.5f);
            return GetOutputData(words, new List<string>());
        }

        public static async Task<string> GetInanimateObject(int examplesCount)
        {
            SetGPTRole("an expert on non-living things.");

            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of inanimate objects based on the child's interest " +
                "- Guidelines:" +
                " * Don't use uncountable nouns and mass nouns in answer," +
                " * Kids safe" +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Without using emojis, stickers or images. "
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 2f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        private static async Task<string> GetExamplesOf(int examplesCount, string field)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} the most popular representatives of {field}s " +
                "- Guidelines:" +
                $" * �hildren should know these {field}s," +
                $" * The {field} must be easily recognizable for children," +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Do not use emojis, stickers, or any other types of images."
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="examplesCount"></param>
        /// <param name="animalType">����� ����� �� WildlifeArray</param>
        /// <returns></returns>
        public static async Task<string> GetWildLifeExamples(int examplesCount, DataTypes.Wildlife wildlifeType)
        {
            SetGPTRole("a preschool biology teacher. ");

            return await GetExamplesOf(examplesCount, wildlifeType + "");
        }

        public static async Task<string> GetAnimalsByHomeType(int examplesCount, DataTypes.AnimalByHomeType homeType)
        {
            SetGPTRole("a preschool biology teacher. ");

            return await GetExamplesOf(examplesCount, homeType + " animal");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="examplesCount"></param>
        /// <param name="habitat">DataTypes.Habitat</param>
        /// <returns></returns>
        public static async Task<string> GetAnimalByHabitat(int examplesCount, DataTypes.Habitat habitat)
        {
            SetGPTRole("a preschool biology teacher. ");

            return await GetExamplesOf(examplesCount, habitat + " animal");
        }

        public static async Task<string> GetAnimalByMovementMethod(int examplesCount, DataTypes.AnimalByMethodOfMovement movementType)
        {
            SetGPTRole("a preschool biology teacher. ");

            List<DataTypes.AnimalByMethodOfMovement> animalsByMethodOfMovement = DataTypes.EnumArrays.MovementMethods.Where(x => x != movementType).ToList();
            string bannedCategories = "";
            foreach (var type in animalsByMethodOfMovement)
            {
                bannedCategories += type + ", ";
            }

            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} the most popular representatives of {movementType} animals " +
                "- Guidelines:" +
                " * Сhildren should know these animals," +
                " * The animal must be easily recognizable for children," +
                $" * The animal should not belong to the following categories: {bannedCategories}" +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Do not use emojis, stickers, or any other types of images."
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        private static async Task<string> GetObjectsAssociatedWith(int examplesCount, string fieldOfInterest)
        {
            SetGPTRole("a preschool teacher. ");

            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of things, objects or items which are associated with the {fieldOfInterest} "
                + LogicGuidlines
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="examplesCount"></param>
        /// <param name="season">����� �� DataTypes.Season</param>
        /// <returns></returns>
        public static async Task<string> GetSeasonsExamples(int examplesCount, DataTypes.Season season)
        {
            return await GetObjectsAssociatedWith(examplesCount, season + "");
        }

        public static async Task<string> GetRoomExamples(int examplesCount, DataTypes.Room room)
        {
            return await GetObjectsAssociatedWith(examplesCount, room + "");
        }

        public static async Task<string> GetResidenceExamples(int examplesCount, DataTypes.Residence residence)
        {
            return await GetObjectsAssociatedWith(examplesCount, residence + "");
        }

        public static async Task<string> GetProfessionsExamples(int examplesCount)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of professions that children in the USA, Britain, Canada and Australia know " +
                "- Guidelines:" +
                " * �hildren should know these professions," +
                " * The profession must be easily recognizable for children," +
                " * Use only singular form, " +
                " * Answer must contain only numbered list of words without any comments, " +
                " * Do not use emojis, stickers, or any other types of images."
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        public static async Task<string> GetProfessionalObjectsExamples(int examplesCount, string profession)
        {
            return await GetObjectsAssociatedWith(examplesCount, profession);
        }

        public static async Task<string> GetProfessionExamples(int examplesCount, string profession)
        {
            return await GetObjectsAssociatedWith(examplesCount, profession);
        }

        public static async Task<string> GetExamplesByColor(int examplesCount, DataTypes.Color color)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of {color} things, objects or items by default" +
                LogicGuidlines
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        public static async Task<string> GetExamplesByShape(int examplesCount, DataTypes.Shape shape)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content =
                $"Write {examplesCount} examples of things, objects or items of {shape} shape" +
                LogicGuidlines
            };
            messages.Add(userMessage);
            string words = await GetAnswerFromGPT(0.8f, 1f, 0.5f);

            return GetOutputData(words, new List<string>());
        }

        private static string GetOutputData(string words, List<string> savedWords)
        {
            savedWords.AddRange(TextHandler.GetListFromString(TextHandler.GetCleanString(words)));

            //Text.Text = words;

            messages.Clear();

            return TextHandler.GetCleanString(words);
        }

        private static async Task<string> GetAnswerFromGPT(float temperature = 1, float presencePenalty = 0, float frequencyPenalty = 0)
        {
            string answer = "";
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = messages,
                Temperature = temperature,
                PresencePenalty = presencePenalty,
                FrequencyPenalty = frequencyPenalty,
            });
            //foreach (var message in messages)
            //{
            //    Debug.Log(message.Content);
            //}
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                ChatMessage chatGPTAnswer = completionResponse.Choices[0].Message;
                chatGPTAnswer.Content = chatGPTAnswer.Content.Trim();

                messages.Add(chatGPTAnswer);
                answer = chatGPTAnswer.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            Debug.Log($"GPT ANSWER: {answer}");
            return answer;
        }
    }
}