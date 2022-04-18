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

        private bool isRightToLeft;
        public bool IsRightToLeft
        {
            get => isRightToLeft;
            set => isRightToLeft = value;
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

        private int id;
        public int ID
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

        public InsertRule(string insert, InsertPosition insertPos, int posInt, bool isRightToLeft)
        {
            InsertText = insert;
            InsertPos = insertPos;
            PosInt = posInt;
            IsRightToLeft = isRightToLeft;
        }

        public InsertRule(string insert, InsertPosition insertPos, string text, bool isRightToLeft)
        {
            InsertText = insert;
            InsertPos = insertPos;
            IsRightToLeft = isRightToLeft;

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
            StringBuilder sb = new StringBuilder(current);
            int index;

            switch (insertPos)
            {
                default:
                case InsertPosition.Prefix:
                    sb.Insert(0, insertText);
                    break;
                case InsertPosition.Suffix:
                    sb.Append(insertText);
                    break;
                case InsertPosition.Position:
                    // Only apply rule if the given position is valid
                    if (posInt <= sb.Length)
                    {
                        if (isRightToLeft)
                        {
                            sb.Insert(current.Length - posInt, insertText);
                        }
                        else
                        {
                            sb.Insert(posInt, insertText);
                        }
                    }
                    break;
                case InsertPosition.AfterText:
                    // Get the insert to where we need to insert the string
                    index = isRightToLeft ? current.LastIndexOf(afterTextString) : current.IndexOf(afterTextString);
                    index += afterTextString.Length;
                    sb.Insert(index, insertText);
                    break;
                case InsertPosition.BeforeText:
                    // Get the index to where we need to insert the string
                    index = isRightToLeft ? current.LastIndexOf(beforeTextString) : current.IndexOf(beforeTextString);
                    sb.Insert(index, insertText);

                    break;
                case InsertPosition.ReplaceCurrentName:
                    sb.Replace(current, insertText);
                    break;
            }

            return sb.ToString();
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
