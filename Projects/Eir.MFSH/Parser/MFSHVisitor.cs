using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Eir.MFSH;
using Microsoft.VisualBasic.CompilerServices;

namespace Eir.MFSH.Parser
{
    class MFSHVisitor : MFSHParserBaseVisitor<Object>
    {
        public bool DebugFlag { get; set; } = false;
        MFsh mfsh;

        public List<String> Usings = new List<string>();
        public Stack<ParseBlock> state = new Stack<ParseBlock>();
        public string SourceName;

        public ParseBlock Current => this.state.Peek();
        void PushState(ParseBlock s) => this.state.Push(s);
        ParseBlock PopState()
        {
            ParseBlock s = this.state.Peek();
            this.state.Pop();
            return s;
        }

        public MFSHVisitor(MFsh mfsh,
            string sourceName)
        {
            this.SourceName = sourceName;
            this.mfsh = mfsh;
            ParseBlock f = new ParseBlock("base");
            this.PushState(f);
        }

        void TraceMsg(ParserRuleContext context, String fcn)
        {
            if (!this.DebugFlag)
                return;
            String text = context
                .GetText()
                .Replace("\r", "")
                .Replace("\n", "");
            Trace.WriteLine($"{fcn}. {this.SourceName}, #{context.Start.Line} '{text}'");
        }

        public override object VisitDocument(MFSHParser.DocumentContext context)
        {
            const String fcn = "VisitDocument";
            this.TraceMsg(context, fcn);
            this.VisitChildren(context);
            return null;
        }

        public override object VisitTextA(MFSHParser.TextAContext context)
        {
            const String fcn = "VisitTextA";
            this.TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextB(MFSHParser.TextBContext context)
        {
            const String fcn = "VisitTextB";
            this.TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextC(MFSHParser.TextCContext context)
        {
            const String fcn = "VisitTextC";
            this.TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTextD(MFSHParser.TextDContext context)
        {
            const String fcn = "VisitTextD";
            this.TraceMsg(context, fcn);

            String line = context.GetText();
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = context.GetText()
            };
            this.Current.Items.Add(t);
            return null;
        }

        public override object VisitTickText(MFSHParser.TickTextContext context)
        {
            const String fcn = "VisitTickText";
            this.TraceMsg(context, fcn);

            String line = context.GetText();
            Int32 tickIndex = line.IndexOf('`');
            line = line.Substring(tickIndex + 1);
            MIText t = new MIText(this.SourceName, context.Start.Line)
            {
                Line = line
            };
            this.Current.Items.Add(t);
            return null;
        }


        public override object VisitMacro(MFSHParser.MacroContext context)
        {
            const String fcn = "VisitMacro";
            this.TraceMsg(context, fcn);
            String[] names = context
                .NAME()
                .Select((a) => a.GetText())
                .ToArray();
            MIMacro macro = new MIMacro(this.SourceName, context.Start.Line, names[0], names.Skip(1));
            MacroBlock macroBlock = new MacroBlock("macro", macro);
            this.PushState(macroBlock);

            var redirectContext = context.redirect();
            if (redirectContext != null)
                macro.Redirect = (String)(this.Visit(redirectContext.singleString()));
            macro.OnceFlag = context.ONCE() != null;

            return null;
        }

        public override object VisitCall(MFSHParser.CallContext context)
        {
            StringBuilder path = new();
            MFSHParser.PathContext pathContext = context.path();
            foreach (MFSHParser.NameStringContext pathItem in pathContext.nameString())
            {
                if (path.Length > 0)
                    path.Append("\\");
                path.Append(pathItem.GetText());
            }

            List<String> parameters = new();
            for (Int32 i = 1; i < context.nameString().Length; i++)
                parameters.Add(context.nameString(i).GetText());

            MICall call = new MICall(this.SourceName, context.Start.Line)
            {
                Name = path.ToString(),
                Parameters = parameters
            };

            this.Current.Items.Add(call);
            return null;
        }

        public override object VisitSet(MFSHParser.SetContext context)
        {
            String s = context.GetText();
            String setName = context.NAME().GetText();
            String setValue = (String)this.Visit(context.anyString());
            MISet set = new MISet(this.SourceName, context.Start.Line)
            {
                Name = setName,
                Value = setValue
            };

            this.Current.Items.Add(set);
            return null;
        }

        public override object VisitFrag(MFSHParser.FragContext context)
        {
            const String fcn = "VisitFrag";

            this.TraceMsg(context, fcn);
            String fragName = context.NAME().GetText();

            String fragmentDefinition = String.Empty;

            MIFragment frag = new MIFragment(this.SourceName, context.Start.Line, fragName);
            MacroBlock macroBlock = new MacroBlock("frag", frag);

            frag.OnceFlag = context.ONCE() != null;

            this.PushState(macroBlock);

            return null;
        }

        public MIFragment GetFragmentState(String fcn, Int32 lineNumber, String visitName)
        {
            void Err()
            {
                this.Error(fcn,
                    lineNumber.ToString(),
                    $"Unexpected '#parent'");
                throw new Exception($"Unexpected '#{visitName}'");
            }

            switch (this.state.Peek())
            {
                case MacroBlock mBlock:
                    MIFragment f = mBlock.Item as MIFragment;
                    if (f == null)
                        Err();
                    return f;

                default:
                    Err();
                    break;
            }

            return null;
        }

        public override object VisitParent(MFSHParser.ParentContext context)
        {
            const String fcn = "VisitFragParent";

            this.TraceMsg(context, fcn);
            MIFragment f = this.GetFragmentState(fcn, context.Start.Line, "Parent");
            f.Parent = context.NAME().GetText();
            return null;
        }

        public override object VisitDescription(MFSHParser.DescriptionContext context)
        {
            const String fcn = "VisitFragDescription";

            this.TraceMsg(context, fcn);
            MIFragment f = this.GetFragmentState(fcn, context.Start.Line, "Title");
            f.Description = (String)this.Visit(context.anyString());
            return null;
        }

        public override object VisitTitle(MFSHParser.TitleContext context)
        {
            const String fcn = "VisitFragTitle";

            this.TraceMsg(context, fcn);
            MIFragment f = this.GetFragmentState(fcn, context.Start.Line, "Title");
            f.Title = (String)this.Visit(context.anyString());
            return null;
        }

        public override object VisitIncompatible(MFSHParser.IncompatibleContext context)
        {
            //const String fcn = "VisitIncompatible";
            String macroName = context.NAME().GetText();

            MIIncompatible incompatible = new MIIncompatible(this.SourceName, context.Start.Line)
            {
                Name = macroName
            };

            this.Current.Items.Add(incompatible);
            return null;
        }

        public override object VisitApply(MFSHParser.ApplyContext context)
        {
            //const String fcn = "VisitApply";
            MIApply apply = new MIApply(this.SourceName, context.Start.Line);
            apply.Name = context.NAME().GetText();


            foreach (MFSHParser.AnyStringContext varContext in context.anyString())
            {
                String s = (String)this.VisitChildren(varContext);
                apply.Parameters.Add(s);
            }

            apply.OnceFlag = (context.ONCE() != null);
            apply.Usings.AddRange(this.Usings);

            this.Current.Items.Add(apply);
            return null;
        }

        public override object VisitUse(MFSHParser.UseContext context)
        {
            const String fcn = "VisitUse";
            this.TraceMsg(context, fcn);

            String useName = context.NAME().GetText();
            this.Usings.Add(useName);
            UseBlock useBlock = new UseBlock("macro", useName);
            this.PushState(useBlock);

            return null;
        }

        public override Object VisitIf([NotNull] MFSHParser.IfContext context)
        {
            const String fcn = "VisitIf";
            this.TraceMsg(context, fcn);
            ConditionalBlock conditionalBlock = new ConditionalBlock(this.SourceName,
                context.Start.Line);
            this.PushState(conditionalBlock);
            MIConditional.Condition condition = new MIConditional.Condition();
            condition.State = (MIConditional.CState)this.Visit(context.condition());
            conditionalBlock.AddCondition(condition);
            return null;
        }

        public override Object VisitElse([NotNull] MFSHParser.ElseContext context)
        {
            const String fcn = "VisitElse";
            this.TraceMsg(context, fcn);
            ConditionalBlock conditionalBlock = this.Current as ConditionalBlock;
            if (conditionalBlock == null)
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Error '#else' found with no starting '#if'");
            }
            MIConditional.Condition condition = new MIConditional.Condition();
            condition.State = new MIConditional.CStateTrue();
            conditionalBlock.AddCondition(condition);
            return null;
        }

        public override Object VisitElseIf([NotNull] MFSHParser.ElseIfContext context)
        {
            const String fcn = "VisitElseIf";
            this.TraceMsg(context, fcn);
            ConditionalBlock conditionalBlock = this.Current as ConditionalBlock;
            if (conditionalBlock == null)
            {
                this.Error(fcn,
                    context.Start.Line.ToString(),
                    $"Error '#else if' found with no starting '#if'");
            }

            MIConditional.Condition condition = new MIConditional.Condition();
            condition.State = (MIConditional.CState)this.Visit(context.condition());
            conditionalBlock.AddCondition(condition);
            return null;
        }

        public override Object VisitConditionStrEq([NotNull] MFSHParser.ConditionStrEqContext context)
        {
            MFSHParser.AnyStringContext[] values = context.anyString();
            MIConditional.CStateIsString retVal = new MIConditional.CStateIsString
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
            return retVal;
        }

        public override Object VisitConditionNumEq([NotNull] MFSHParser.ConditionNumEqContext context)
        {
            MFSHParser.ConditionValueNumContext[] values = context.conditionValueNum();
            return new MIConditional.CStateNumEq
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
        }

        public override Object VisitConditionNumLt([NotNull] MFSHParser.ConditionNumLtContext context)
        {
            MFSHParser.ConditionValueNumContext[] values = context.conditionValueNum();
            return new MIConditional.CStateNumLt
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
        }

        public override Object VisitConditionNumLe([NotNull] MFSHParser.ConditionNumLeContext context)
        {
            MFSHParser.ConditionValueNumContext[] values = context.conditionValueNum();
            return new MIConditional.CStateNumLe
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
        }

        public override Object VisitConditionNumGt([NotNull] MFSHParser.ConditionNumGtContext context)
        {
            MFSHParser.ConditionValueNumContext[] values = context.conditionValueNum();
            return new MIConditional.CStateNumGt
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
        }

        public override Object VisitConditionNumGe([NotNull] MFSHParser.ConditionNumGeContext context)
        {
            MFSHParser.ConditionValueNumContext[] values = context.conditionValueNum();
            return new MIConditional.CStateNumGe
            {
                Lhs = values[0].GetText(),
                Rhs = values[1].GetText()
            };
        }

        public override object VisitEnd(MFSHParser.EndContext context)
        {
            const String fcn = "VisitEnd";
            this.TraceMsg(context, fcn);

            ParseBlock s = this.PopState();
            switch (s)
            {
                case UseBlock useBlock:
                    this.Usings.Remove(useBlock.Use);
                    this.Current.Items.AddRange(useBlock.Items);
                    break;

                case MacroBlock macroBlock:

                    if (this.mfsh.MacroMgr.TryAddItem(macroBlock.Item.Name, macroBlock.Item) == false)
                    {
                        this.Error(fcn,
                            context.Start.Line.ToString(),
                            $"Error adding macro '{macroBlock.Item.Name}'. Does macro already exist?");
                    }
                    break;

                case ConditionalBlock conditionalBlock:
                    this.Current.Items.Add(conditionalBlock.Conditional);
                    break;

                default:
                    this.Error(fcn,
                        context.Start.Line.ToString(),
                        $"Unexpected '#end'");
                    throw new Exception($"Unexpected '#end'");
            }
            return null;
        }


        public override object VisitAnyString(MFSHParser.AnyStringContext context)
        {
            return (String)this.VisitChildren(context);
        }

        public override object VisitSingleString(MFSHParser.SingleStringContext context)
        {
            String s = context.GetText();
            s = s
                .Substring(1, s.Length - 2)
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\")
                ;
            return s;
        }

        public override object VisitMultiLineString(MFSHParser.MultiLineStringContext context)
        {
            String text = context.GetText().Trim();
            List<String> lines = context.GetText().Split('\n').ToList();
            if (lines.Count == 0)
                return null;

            for (Int32 i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                {
                    Regex r = new Regex("^[ ]*#");
                    Match m = r.Match(line);
                    if (m.Success == true)
                        line = line.Substring(m.Length);
                }
                lines[i] = line;
            }

            if (lines[0].StartsWith("\"\"\""))
                lines[0] = lines[0].Substring(3);
            if (lines[^1].EndsWith("\"\"\""))
                lines[^1] = lines[^1].Substring(0, lines[^1].Length - 3);
            while ((lines.Count > 0) && (String.IsNullOrWhiteSpace(lines[0])))
                lines.RemoveAt(0);
            while ((lines.Count > 0) && (String.IsNullOrWhiteSpace(lines[^1])))
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return null;

            Int32 minMargin = Int32.MaxValue;
            for (Int32 i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                Regex r = new Regex("^[ ]*");
                Match m = r.Match(line);
                if ((m.Success == true) && (m.Length < minMargin))
                    minMargin = m.Length;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0].Substring(minMargin));
            foreach (String line in lines.Skip(1))
            {
                sb.Append("\n");
                sb.Append(line.Substring(minMargin));
            }
            return sb.ToString();
        }

        void Error(String fcn,
            String location,
            String msg)
        {
            String fullMsg = $"{this.SourceName}, line {location}. {msg}";
            this.mfsh.ConversionError("mfsh", fcn, fullMsg);
        }
    }
}
