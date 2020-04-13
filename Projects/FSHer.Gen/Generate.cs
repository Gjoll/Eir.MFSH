using Eir.DevTools;
using FSHer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FSHerGen
{
    public class Generate
    {
        Dictionary<Int32, String> ruleDict = new Dictionary<int, string>();
        Dictionary<Int32, String> tokenDict = new Dictionary<int, string>();
        private String antlrDir;
        private CodeEditor code;

        public void GenerateListener()
        {
            this.code = new CodeEditor();
            antlrDir = Path.Combine(
                DirHelper.FindParentDir("Projects"),
                "FSHer.Antlr");

            String listenerPath = Path.Combine(
                DirHelper.FindParentDir("Projects"),
                "FSHer",
                "FSHListener.cs");
            this.code.Load(listenerPath);
            this.ProcessFSHTokens();
            this.ProcessRules();
            this.code.Save();
        }

        /// <summary>
        /// Process FSH.tokens file
        /// </summary>
        void ProcessRules()
        {
            CodeBlockNested ruleNamesBlock = this.code.Blocks.Find("RuleNames");
            ruleNamesBlock.Clear();

            CodeBlockNested methodBlock = this.code.Blocks.Find("VisitorMethods");
            methodBlock.Clear();

            // use reflection to override all rule enter/exit methods.
            foreach (var methodInfo in typeof(FSHBaseListener).GetMethods())
            {
                if (methodInfo.Name.StartsWith("Enter"))
                {
                    String ruleName = methodInfo.Name.Substring(5);
                    if (ruleName != "EveryRule")
                    {
                        String ruleNameStr = $"{ruleName}Str";
                        ruleNamesBlock
                            .AppendCode($"public const String {ruleNameStr} = \"{ruleName}\";")
                            ;

                        methodBlock
                            .AppendCode($"public override void Enter{ruleName}(FSHParser.{ruleName}Context context)")
                            .OpenBrace()
                            .AppendCode($"this.PushRule({ruleNameStr}, context.Start.StartIndex);")
                            .CloseBrace()
                            ;
                    }
                }
                else if (methodInfo.Name.StartsWith("Exit"))
                {
                    String ruleName = methodInfo.Name.Substring(4);
                    if (ruleName != "EveryRule")
                    {
                        methodBlock
                            .AppendCode($"public override void Exit{ruleName}(FSHParser.{ruleName}Context context)")
                            .OpenBrace()
                            .AppendCode($"this.PopRule(\"{ruleName}\", context.Stop.StopIndex);")
                            .CloseBrace()
                            ;
                    }
                }
            }
        }


        /// <summary>
        /// Process FSH.tokens file
        /// </summary>
        void ProcessFSHTokens()
        {
            CodeBlockNested tokens = this.code.Blocks.Find("TokenNumbers");
            tokens.Clear();

            tokens
                .DefineBlock(out CodeBlockNested tokenBlock)
                .AppendCode($"String GetTokenName(Int32 tokenIndex)")
                .OpenBrace()
                .AppendCode("switch (tokenIndex)")
                .OpenBrace()
                .DefineBlock(out CodeBlockNested tokenName)
                .AppendCode("default: throw new Exception($\"unknown token index {tokenIndex}\");")
                .CloseBrace()
                .CloseBrace()
                ;

            String tokenFile = Path.Combine(this.antlrDir, "FSH.tokens");
            String[] lines = File.ReadAllLines(tokenFile);
            foreach (String line in lines)
            {
                String line2 = line.Trim();
                if (line2.StartsWith("'") == false)
                {
                    String[] parts = line2.Split("=");
                    if (parts.Length != 2)
                        throw new Exception("Error parsing FSH.tokens");
                    Int32 tokenNum = Int32.Parse(parts[1]);
                    String token = parts[0];

                    // rules that start with ' are actually repeats of earlier rules.
                    if (token[0] != '\'')
                        tokenDict.Add(tokenNum, token);
                    tokenBlock
                        .AppendCode($"const Int32 {token}Num = {tokenNum};")
                        ;

                    tokenName
                        .AppendCode($"case {token}Num: return \"{token}\";")
                        ;

                }
            }
        }
    }
}
