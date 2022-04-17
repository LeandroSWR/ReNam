using System;
using System.Collections.Generic;
using System.Text;

namespace ReNam
{
    class InsertRule : IRule
    {
        private string insertText;
        public string InsertText 
        { 
            get => insertText;
            set => insertText = value;
        }

        private InsertPosition insertPos;
        public InsertPosition InsertPos
        {
            get => insertPos;
            set => insertPos = value;
        }

        private int posInt;
        public int PosInt
        {
            get => posInt;
            set => posInt = value;
        }

        private bool isLeftToRight;
        public bool IsLeftToRight
        {
            get => isLeftToRight;
            set => isLeftToRight = value;
        }

        private string afterTextString;
        public string AfterTextString
        {
            get => afterTextString;
            set => afterTextString = value;
        }

        private string beforeTextString;
        public string BeforeTextString
        {
            get => beforeTextString;
            set => beforeTextString = value;
        }

        private string id;
        public string ID
        {
            get => id; set => id = value;
        }
        public string Rule
        {
            get => "Insert";
        }
        public string Statement
        {
            get => this.ToString();
        }

        public InsertRule(string insert, InsertPosition insertPos)
        {
            InsertText = insert;
            InsertPos = insertPos;
        }

        public InsertRule(string insert, InsertPosition insertPos, int posInt, bool isLeftToRight)
        {
            InsertText = insert;
            InsertPos = insertPos;
            PosInt = posInt;
            IsLeftToRight = IsLeftToRight;
        }

        public InsertRule(string insert, InsertPosition insertPos, string text)
        {
            InsertText = insert;
            InsertPos = insertPos;

            switch (insertPos)
            {
                default:
                case InsertPosition.AfterText:
                    AfterTextString = text;
                    break;
                case InsertPosition.BeforeText:
                    BeforeTextString = text;
                    break;
            }
        }

        public string ApplyRule(string current)
        {
            // Applies rules here

            return current;
        }

        /// <summary>
        /// Override ToString to display the statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
