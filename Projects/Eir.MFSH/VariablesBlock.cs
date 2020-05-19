using System;
using System.Collections.Generic;
using System.Text;

namespace Eir.MFSH
{
    public class VariablesBlock
    {
        Dictionary<String, String> variables =
            new Dictionary<String, string>();

        public VariablesBlock()
        {
        }
        public void Remove(String name) => this.variables.Remove(name);
        public void Add(String key, String value) => this.variables.Add(key, value);
        public Int32 Count => this.variables.Count;

        public void Set(String key, String value)
        {
            Remove(key);
            Add(key, value);
        }
        public String ReplaceText(String text)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            foreach (String key in this.variables.Keys)
            {
                String value = this.variables[key];
                text = ReplaceText(text, key, value);
            }

            return text;
        }

        String ReplaceText(String text, String word, String byWhat)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            if (word[0] == '%')
                text = text.Replace(word, byWhat);
            else
                text = ReplaceWholeWord(text, word, byWhat);
            return text;
        }

        public String ReplaceWholeWord(String s, String wordToReplace, String bywhat)
        {
            bool IsBreakChar(char c)
            {
                if (Char.IsLetterOrDigit(c))
                    return false;
                if (c == '%')
                    return false;
                return true;
            }
            StringBuilder sb = new StringBuilder();
            Int32 i = 0;
            Int32 length = s.Length;
            void SkipLeading()
            {
                while (i < length)
                {
                    Char c = s[i];
                    if (IsBreakChar(c) == false)
                        return;
                    sb.Append(c);
                    i += 1;
                }
            }

            String GetWholeWord()
            {
                StringBuilder w = new StringBuilder();
                while (i < length)
                {
                    Char c = s[i];
                    if (IsBreakChar(c) == true)
                        break;
                    w.Append(c);
                    i += 1;
                }
                return w.ToString();
            }

            while (i < length)
            {
                SkipLeading();
                String wholeWord = GetWholeWord();
                if (String.Compare(wholeWord, wordToReplace, StringComparison.Ordinal) == 0)
                    sb.Append(bywhat);
                else
                    sb.Append(wholeWord);
            }
            return sb.ToString();
        }
    }
}
