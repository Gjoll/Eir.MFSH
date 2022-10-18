using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MFSH
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
            this.Remove(key);
            this.Add(key, value);
        }

        public String ReplaceText(String text)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            foreach (String key in this.variables.Keys)
            {
                String value = this.variables[key];
                if (value == null)
                    value = "";
                text = this.ReplaceText(text, key, value);
            }

            return text;
        }

        String ReplaceText(String text, String word, String byWhat)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            if (word[0] == '%')
                text = this.Replace(text, word, byWhat);
            else
                text = this.ReplaceWholeWord(text, word, byWhat);
            return text;
        }

        public String Replace(String s, String wordToReplace, String byWhat)
        {
            StringBuilder margin = new StringBuilder();
            {
                Int32 i = 0;
                while ((i < s.Length) && (Char.IsWhiteSpace(s[i])))
                    margin.Append(s[i++]);
            }

            StringBuilder sb = new StringBuilder();
            Int32 index = 0;
            while (true)
            {
                void CopyText(Int32 j)
                {
                    if (j <= index)
                        return;
                    sb.Append(s.Substring(index, j - index));
                }

                Int32 i = s.IndexOf(wordToReplace, index, s.Length - index);
                if (i < 0)
                {
                    CopyText(s.Length);
                    return sb.ToString();
                }

                CopyText(i);
                if (byWhat.Contains('\n') == false)
                {
                    sb.Append(byWhat);
                }
                else
                {
                    String[] byWhatLines = byWhat.Split('\n');
                    sb.AppendLine(byWhatLines[0]);
                    for (Int32 j = 1; j < byWhatLines.Length - 1; j++)
                    {
                        String line = byWhatLines[j].TrimStart();
                        sb.Append(margin);
                        sb.AppendLine(line);
                    }
                    {
                        String line = byWhatLines[byWhatLines.Length - 1].TrimStart();
                        sb.Append(margin);
                        sb.Append(line);
                    }
                }
                i += wordToReplace.Length;
                index = i;
            }

        }


        public String ReplaceWholeWord(String s, String wordToReplace, String byWhat)
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

            StringBuilder margin = new StringBuilder();
            while ((i < s.Length) && (Char.IsWhiteSpace(s[i])))
                margin.Append(s[i++]);
            i = 0;

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
                {
                    if (byWhat.Contains('\n') == false)
                    {
                        sb.Append(byWhat);
                    }
                    else
                    {
                        String[] byWhatLines = byWhat.Split('\n');
                        sb.AppendLine(byWhatLines[0]);
                        for (Int32 j = 1; j < byWhatLines.Length - 1; j++)
                        {
                            String line = byWhatLines[j].TrimStart();
                            sb.Append(margin);
                            sb.AppendLine(line);
                        }

                        {
                            String line = byWhatLines[byWhatLines.Length - 1].TrimStart();
                            sb.Append(margin);
                            sb.Append(line);
                        }
                    }
                }
                else
                    sb.Append(wholeWord);
            }
            return sb.ToString();
        }
    }
}
