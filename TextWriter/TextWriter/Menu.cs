//Author: Geordan Krahn
//Initial Date of Creation: October 24 2020
//date last modified: October 29 2020
//Purpose: Create context menus to for user input and handle that input

using System;
using System.Collections.Generic;

namespace TextWriter
{
    class Menu : TextWriter
    {
        string text, prompt;
        List<string> elements;

        public Menu() { }
        public Menu(List<string> menuElements, string inputPrompt, string textBody)
        {
            elements = menuElements;
            prompt = inputPrompt;
            this.text = textBody;
        }

        public Menu(string inputPrompt, string textBody)
        {
            prompt = inputPrompt;
            this.text = textBody;
        }

        public List<string> GetElements()
        {
            return elements;
        }

        public void SetPrompt(string newPrompt)
        {
            prompt = newPrompt;
        }
        public void SetElements(List<string> menuElements)
        {
            elements = menuElements;
        }

        override public string GetText()
        {
            return text;
        }

        void SetText(string newText)
        {
            this.text = newText;
        }

        public bool RequestYesNo()
        {
            DisplayWindow(text);
            return YesNoMenu(prompt);
        }

        public string RequestValidName(string title)
        {
            DisplayWindow(title);
            return GetValidName();
        }

        public int IntegerMenu()
        {
            //Prepend with selection identifier (int)
            string[] lines = elements.ToArray();
            string temp = "";
            for(int i = 0; i < lines.Length; i++)
            {
                temp = lines[i];
                lines[i] = $"-+- {i + 1} -+- " + $"{temp}";
            }
            DisplayWindow(text, lines);
            return GetValidInputInt(lines);
        }

        public char AlphaMenu()
        {
            //Prepend with selection identifier (int)
            string[] lines = elements.ToArray();
            string temp = "";
            for (int i = 0; i < lines.Length; i++)
            {
                temp = lines[i];
                lines[i] = $"-+- {(char)(i + 97)} -+- " + $"{temp}";
            }
            DisplayWindow(text, lines);
            return GetValidInputChar("\n" + indent + prompt, elements.Count);
        }

        public double RequestDoubleValue(double minValue, double maxValue)
        {
            DisplayWindow(text);
            return GetValidInputDouble(prompt, minValue, maxValue);
        }

        public int RequestIntValue(int minValue, int maxValue)
        {
            DisplayWindow(text);
            return GetValidInputInt(minValue, maxValue);
        }

        public string RequestCommand()
        {
            DisplayWindow(text, elements.ToArray());
            return GetValidInputString(elements);
        }

        public string RequestHiddenCommand()
        {
            DisplayWindow(text);
            return GetValidInputString(elements);
        }

        int GetValidInputInt(string[] lines)
        {
            bool isValid = false;
            int selection;
            do 
            {
                Console.Write("\n" + indent + prompt);
                isValid = int.TryParse(Console.ReadLine(), out selection) && (selection > 0 && selection <= lines.Length);
                if (!isValid)
                {
                    Console.Write(indent + (isValid ? $"Entered {selection}" : "Error, Invalid Input!"));
                    PauseThenClear();
                    DisplayWindow(text, lines);
                }
            } while (!isValid);
            return selection;
        }

        int GetValidInputInt(double minRange, double maxRange)
        {
            bool isValid = false;
            int selection;
            do
            {
                Console.Write(prompt + " ");
                isValid = int.TryParse(Console.ReadLine(), out selection) && (selection >= minRange && selection <= maxRange);
                Console.Write(indent + (isValid ? $"Entered {selection}" : "Error, Invalid Input!"));
                if (!isValid)
                {
                    Console.Write(indent + (isValid ? $"Entered {selection}" : "Error, Invalid Input!"));
                    PauseThenClear();
                    DisplayWindow(text);
                }
            } while (!isValid);
            return selection;
        }

        char GetValidInputChar(string prompt, int numberOfSelections)
        {
            bool isValid = false;
            char selection;
            do
            {
                Console.Write(prompt + " ");
                isValid = char.TryParse(Console.ReadLine(), out selection) && (selection >= 97 && selection < 97 + numberOfSelections);
                Console.Write(indent + (isValid ? $"Entered {selection}" : "Error, Invalid Input!"));
            } while (!isValid);
            return selection;
        }

        double GetValidInputDouble(string prompt, double minRange, double maxRange)
        {
            bool isValid = false;
            double selection;
            do
            {
                Console.Write(prompt + " ");
                isValid = double.TryParse(Console.ReadLine(), out selection) && (selection >= minRange && selection <= maxRange);
                Console.Write(indent + (isValid ? $"Entered {selection}" : "Error, Invalid Input!"));
            } while (!isValid);
            return selection;
        }

        /// <summary>
        /// Returns a valid capitalized name with no spaces or special characters.
        /// </summary>
        /// <returns>string</returns>
        public string GetValidName()
        {
            Console.Write(lower + indent + prompt);
            char[] badCharacter = { '~', '"', '#', '%', '&', '*', ':', '<', '>', '?', '/', '\\', '{', '|', '}', '_', '!', '@', '^', '(', ')', '=', '+', ',', ';', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };
            char[] checkName = Console.ReadLine().ToCharArray();
            string name = ""; //Append this with success
            for (int i = 0; i < checkName.Length; i++)
            {
                for (int j = 0; j < badCharacter.Length; j++)
                    if (checkName[i] == badCharacter[j])//If the name contains any of the bad characters
                        return GetValidName(); // This will only ever return a correct string.
                    else if (i == 0)
                        checkName[0] = char.ToUpper(checkName[0]); // Capitalize the first letter
                name += checkName[i];
            }
            return checkName.Length == 0 ? GetValidName() : name;
        }

        /// <summary>
        /// return valid y or n for yes/no menus
        /// </summary>
        /// <param name="prompt">The text for the input request</param>
        /// <returns>bool</returns>
        public bool YesNoMenu(string prompt)
        {
            char inputChar;
            bool isValidInputChar = false;
            do
            {
                //Validate and Verify
                Console.Write(indent + prompt);
                isValidInputChar = char.TryParse(Console.ReadLine(), out inputChar) && (inputChar == 'N' || inputChar == 'n' || inputChar == 'Y' || inputChar == 'y');
            }
            while (!isValidInputChar);
            return char.ToLower(inputChar) == 'y';
        }

        string GetValidInputString(List<string> commands)
        {
            bool isValid = false;
            string selection;
            
            string[] words = commands.ToArray();
            bool[] isInList = new bool[words.Length];
            for (int i = 0; i < isInList.Length; i++)
                isInList[i] = false;
            do
            {
                Console.Write(prompt + " ");
                selection = Console.ReadLine();
                for (int i = 0; i < words.Length; i++)
                    if (selection == words[i])
                        isInList[i] = true; 
                foreach (bool exists in isInList)
                    if (exists)
                        isValid = true;
                if (!isValid)
                    Console.WriteLine("Not Valid Input!");
            } while (!isValid);
            return selection;
        }
    }
}