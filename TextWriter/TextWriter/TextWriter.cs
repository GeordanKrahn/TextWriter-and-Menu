//Author: Geordan Krahn
//Initial Date of Creation: October 24 2020
//date last modified: October 29 2020
//Purpose: Create context windows to display formatted text to the user

using System;
using System.Collections.Generic;
using System.Text;

namespace TextWriter
{
    class TextWriter
    {

        private string text;
        public const string indent = "     ";
        public const string lower = "\n";
        public const string HORIZONTAL_BORDER = "#==================================================#";
        public const string NEW_LINE_SIDE = "|";


        public void CreateNewLine(string lineText)
        {
            string newLine = "";
            int lineMax = HORIZONTAL_BORDER.Length - 4;
            if (lineText.Length > lineMax)
            {
                newLine = lineText.Substring(0, lineMax);
                lineText = lineText.Remove(0, lineMax);
                WriteNewText(indent + FormatLine(newLine, lineMax));
                CreateNewLine(lineText);
            }
            else
            {
                WriteNewText(indent + FormatLine(lineText, lineMax));
            }
        }

        virtual public string GetText()
        {
            return text;
        }

        private string FormatLine(string line, int lineMaxLength)
        {
            int difference = lineMaxLength - line.Length;
            string newLine = NEW_LINE_SIDE + " " + line;
            if (difference <= 0)
                return newLine += (" " + NEW_LINE_SIDE);
            else
            {
                for (int i = 0; i < difference; i++)
                    newLine += " ";
                return newLine += (" " + NEW_LINE_SIDE);
            }
        }

        private void SetText(string textBody)
        {
            text = textBody;
        }
        public TextWriter()
        {
        }

        public TextWriter(string textBody)
        {
            SetText(textBody);
        }

        public void OverwriteText(string newText)
        {
            SetText(newText);
        }

        //This is where text is written to the screen, format here
        public void WriteText()
        {
            Console.WriteLine(text);
        }

        public void WriteNewText(string newText)
        {
            SetText(newText);
            WriteText();
        }

        public void DisplayWindow(string title, string[] lines)
        {
            string startingPoint = lower + indent;
            //After the console has been cleared, draw the title region
            WriteNewText(startingPoint + HORIZONTAL_BORDER);
            CreateNewLine(title);
            WriteNewText(indent + HORIZONTAL_BORDER);
            //Draw the body section and the bottom border
            for (int count = 0; count < lines.Length; count++)
                CreateNewLine(lines[count]);
            WriteNewText(indent + HORIZONTAL_BORDER);
        }

        public void PauseThenClear()
        {
            Console.ReadKey();
            Console.Clear();
        }

        public void DisplayWindow(string title)
        {
            string startingPoint = lower + indent;
            //After the console has been cleared, draw the title region
            WriteNewText(startingPoint + HORIZONTAL_BORDER);
            CreateNewLine(title);
            WriteNewText(indent + HORIZONTAL_BORDER);
        }
    }
}
