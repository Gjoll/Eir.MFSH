using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using FSHpp.Nodes;

namespace FSHpp
{
    class FSHListener : FSHBaseListener
    {
        public NodeRule Head;

        #region Tokens
        //+ TokenNumbers
        const Int32 T__0Num = 1;                                                                                                            // Generate.cs:120
        const Int32 T__1Num = 2;                                                                                                            // Generate.cs:120
        const Int32 KW_ALIASNum = 3;                                                                                                        // Generate.cs:120
        const Int32 KW_PROFILENum = 4;                                                                                                      // Generate.cs:120
        const Int32 KW_EXTENSIONNum = 5;                                                                                                    // Generate.cs:120
        const Int32 KW_INSTANCENum = 6;                                                                                                     // Generate.cs:120
        const Int32 KW_INSTANCEOFNum = 7;                                                                                                   // Generate.cs:120
        const Int32 KW_INVARIANTNum = 8;                                                                                                    // Generate.cs:120
        const Int32 KW_VALUESETNum = 9;                                                                                                     // Generate.cs:120
        const Int32 KW_CODESYSTEMNum = 10;                                                                                                  // Generate.cs:120
        const Int32 KW_RULESETNum = 11;                                                                                                     // Generate.cs:120
        const Int32 KW_MAPPINGNum = 12;                                                                                                     // Generate.cs:120
        const Int32 KW_MIXINSNum = 13;                                                                                                      // Generate.cs:120
        const Int32 KW_PARENTNum = 14;                                                                                                      // Generate.cs:120
        const Int32 KW_IDNum = 15;                                                                                                          // Generate.cs:120
        const Int32 KW_TITLENum = 16;                                                                                                       // Generate.cs:120
        const Int32 KW_DESCRIPTIONNum = 17;                                                                                                 // Generate.cs:120
        const Int32 KW_EXPRESSIONNum = 18;                                                                                                  // Generate.cs:120
        const Int32 KW_XPATHNum = 19;                                                                                                       // Generate.cs:120
        const Int32 KW_SEVERITYNum = 20;                                                                                                    // Generate.cs:120
        const Int32 KW_USAGENum = 21;                                                                                                       // Generate.cs:120
        const Int32 KW_SOURCENum = 22;                                                                                                      // Generate.cs:120
        const Int32 KW_TARGETNum = 23;                                                                                                      // Generate.cs:120
        const Int32 KW_MODNum = 24;                                                                                                         // Generate.cs:120
        const Int32 KW_MSNum = 25;                                                                                                          // Generate.cs:120
        const Int32 KW_SUNum = 26;                                                                                                          // Generate.cs:120
        const Int32 KW_TUNum = 27;                                                                                                          // Generate.cs:120
        const Int32 KW_NORMATIVENum = 28;                                                                                                   // Generate.cs:120
        const Int32 KW_DRAFTNum = 29;                                                                                                       // Generate.cs:120
        const Int32 KW_FROMNum = 30;                                                                                                        // Generate.cs:120
        const Int32 KW_EXAMPLENum = 31;                                                                                                     // Generate.cs:120
        const Int32 KW_PREFERREDNum = 32;                                                                                                   // Generate.cs:120
        const Int32 KW_EXTENSIBLENum = 33;                                                                                                  // Generate.cs:120
        const Int32 KW_REQUIREDNum = 34;                                                                                                    // Generate.cs:120
        const Int32 KW_CONTAINSNum = 35;                                                                                                    // Generate.cs:120
        const Int32 KW_NAMEDNum = 36;                                                                                                       // Generate.cs:120
        const Int32 KW_ANDNum = 37;                                                                                                         // Generate.cs:120
        const Int32 KW_ONLYNum = 38;                                                                                                        // Generate.cs:120
        const Int32 KW_ORNum = 39;                                                                                                          // Generate.cs:120
        const Int32 KW_OBEYSNum = 40;                                                                                                       // Generate.cs:120
        const Int32 KW_TRUENum = 41;                                                                                                        // Generate.cs:120
        const Int32 KW_FALSENum = 42;                                                                                                       // Generate.cs:120
        const Int32 KW_EXCLUDENum = 43;                                                                                                     // Generate.cs:120
        const Int32 KW_CODESNum = 44;                                                                                                       // Generate.cs:120
        const Int32 KW_WHERENum = 45;                                                                                                       // Generate.cs:120
        const Int32 KW_VSREFERENCENum = 46;                                                                                                 // Generate.cs:120
        const Int32 KW_SYSTEMNum = 47;                                                                                                      // Generate.cs:120
        const Int32 KW_UNITSNum = 48;                                                                                                       // Generate.cs:120
        const Int32 KW_EXACTLYNum = 49;                                                                                                     // Generate.cs:120
        const Int32 KW_MACRONum = 50;                                                                                                       // Generate.cs:120
        const Int32 EQUALNum = 51;                                                                                                          // Generate.cs:120
        const Int32 STARNum = 52;                                                                                                           // Generate.cs:120
        const Int32 COLONNum = 53;                                                                                                          // Generate.cs:120
        const Int32 COMMANum = 54;                                                                                                          // Generate.cs:120
        const Int32 ARROWNum = 55;                                                                                                          // Generate.cs:120
        const Int32 STRINGNum = 56;                                                                                                         // Generate.cs:120
        const Int32 MULTILINE_STRINGNum = 57;                                                                                               // Generate.cs:120
        const Int32 NUMBERNum = 58;                                                                                                         // Generate.cs:120
        const Int32 UNITNum = 59;                                                                                                           // Generate.cs:120
        const Int32 CODENum = 60;                                                                                                           // Generate.cs:120
        const Int32 CONCEPT_STRINGNum = 61;                                                                                                 // Generate.cs:120
        const Int32 DATETIMENum = 62;                                                                                                       // Generate.cs:120
        const Int32 TIMENum = 63;                                                                                                           // Generate.cs:120
        const Int32 CARDNum = 64;                                                                                                           // Generate.cs:120
        const Int32 REFERENCENum = 65;                                                                                                      // Generate.cs:120
        const Int32 CARET_SEQUENCENum = 66;                                                                                                 // Generate.cs:120
        const Int32 REGEXNum = 67;                                                                                                          // Generate.cs:120
        const Int32 COMMA_DELIMITED_CODESNum = 68;                                                                                          // Generate.cs:120
        const Int32 COMMA_DELIMITED_SEQUENCESNum = 69;                                                                                      // Generate.cs:120
        const Int32 SEQUENCENum = 70;                                                                                                       // Generate.cs:120
        const Int32 WHITESPACENum = 71;                                                                                                     // Generate.cs:120
        const Int32 BLOCK_COMMENTNum = 72;                                                                                                  // Generate.cs:120
        const Int32 LINE_COMMENTNum = 73;                                                                                                   // Generate.cs:120
        String GetTokenName(Int32 tokenIndex)                                                                                               // Generate.cs:93
        {                                                                                                                                   // Generate.cs:94
            switch (tokenIndex)                                                                                                             // Generate.cs:95
            {                                                                                                                               // Generate.cs:96
                case T__0Num: return "T__0";                                                                                                // Generate.cs:124
                case T__1Num: return "T__1";                                                                                                // Generate.cs:124
                case KW_ALIASNum: return "KW_ALIAS";                                                                                        // Generate.cs:124
                case KW_PROFILENum: return "KW_PROFILE";                                                                                    // Generate.cs:124
                case KW_EXTENSIONNum: return "KW_EXTENSION";                                                                                // Generate.cs:124
                case KW_INSTANCENum: return "KW_INSTANCE";                                                                                  // Generate.cs:124
                case KW_INSTANCEOFNum: return "KW_INSTANCEOF";                                                                              // Generate.cs:124
                case KW_INVARIANTNum: return "KW_INVARIANT";                                                                                // Generate.cs:124
                case KW_VALUESETNum: return "KW_VALUESET";                                                                                  // Generate.cs:124
                case KW_CODESYSTEMNum: return "KW_CODESYSTEM";                                                                              // Generate.cs:124
                case KW_RULESETNum: return "KW_RULESET";                                                                                    // Generate.cs:124
                case KW_MAPPINGNum: return "KW_MAPPING";                                                                                    // Generate.cs:124
                case KW_MIXINSNum: return "KW_MIXINS";                                                                                      // Generate.cs:124
                case KW_PARENTNum: return "KW_PARENT";                                                                                      // Generate.cs:124
                case KW_IDNum: return "KW_ID";                                                                                              // Generate.cs:124
                case KW_TITLENum: return "KW_TITLE";                                                                                        // Generate.cs:124
                case KW_DESCRIPTIONNum: return "KW_DESCRIPTION";                                                                            // Generate.cs:124
                case KW_EXPRESSIONNum: return "KW_EXPRESSION";                                                                              // Generate.cs:124
                case KW_XPATHNum: return "KW_XPATH";                                                                                        // Generate.cs:124
                case KW_SEVERITYNum: return "KW_SEVERITY";                                                                                  // Generate.cs:124
                case KW_USAGENum: return "KW_USAGE";                                                                                        // Generate.cs:124
                case KW_SOURCENum: return "KW_SOURCE";                                                                                      // Generate.cs:124
                case KW_TARGETNum: return "KW_TARGET";                                                                                      // Generate.cs:124
                case KW_MODNum: return "KW_MOD";                                                                                            // Generate.cs:124
                case KW_MSNum: return "KW_MS";                                                                                              // Generate.cs:124
                case KW_SUNum: return "KW_SU";                                                                                              // Generate.cs:124
                case KW_TUNum: return "KW_TU";                                                                                              // Generate.cs:124
                case KW_NORMATIVENum: return "KW_NORMATIVE";                                                                                // Generate.cs:124
                case KW_DRAFTNum: return "KW_DRAFT";                                                                                        // Generate.cs:124
                case KW_FROMNum: return "KW_FROM";                                                                                          // Generate.cs:124
                case KW_EXAMPLENum: return "KW_EXAMPLE";                                                                                    // Generate.cs:124
                case KW_PREFERREDNum: return "KW_PREFERRED";                                                                                // Generate.cs:124
                case KW_EXTENSIBLENum: return "KW_EXTENSIBLE";                                                                              // Generate.cs:124
                case KW_REQUIREDNum: return "KW_REQUIRED";                                                                                  // Generate.cs:124
                case KW_CONTAINSNum: return "KW_CONTAINS";                                                                                  // Generate.cs:124
                case KW_NAMEDNum: return "KW_NAMED";                                                                                        // Generate.cs:124
                case KW_ANDNum: return "KW_AND";                                                                                            // Generate.cs:124
                case KW_ONLYNum: return "KW_ONLY";                                                                                          // Generate.cs:124
                case KW_ORNum: return "KW_OR";                                                                                              // Generate.cs:124
                case KW_OBEYSNum: return "KW_OBEYS";                                                                                        // Generate.cs:124
                case KW_TRUENum: return "KW_TRUE";                                                                                          // Generate.cs:124
                case KW_FALSENum: return "KW_FALSE";                                                                                        // Generate.cs:124
                case KW_EXCLUDENum: return "KW_EXCLUDE";                                                                                    // Generate.cs:124
                case KW_CODESNum: return "KW_CODES";                                                                                        // Generate.cs:124
                case KW_WHERENum: return "KW_WHERE";                                                                                        // Generate.cs:124
                case KW_VSREFERENCENum: return "KW_VSREFERENCE";                                                                            // Generate.cs:124
                case KW_SYSTEMNum: return "KW_SYSTEM";                                                                                      // Generate.cs:124
                case KW_UNITSNum: return "KW_UNITS";                                                                                        // Generate.cs:124
                case KW_EXACTLYNum: return "KW_EXACTLY";                                                                                    // Generate.cs:124
                case KW_MACRONum: return "KW_MACRO";                                                                                        // Generate.cs:124
                case EQUALNum: return "EQUAL";                                                                                              // Generate.cs:124
                case STARNum: return "STAR";                                                                                                // Generate.cs:124
                case COLONNum: return "COLON";                                                                                              // Generate.cs:124
                case COMMANum: return "COMMA";                                                                                              // Generate.cs:124
                case ARROWNum: return "ARROW";                                                                                              // Generate.cs:124
                case STRINGNum: return "STRING";                                                                                            // Generate.cs:124
                case MULTILINE_STRINGNum: return "MULTILINE_STRING";                                                                        // Generate.cs:124
                case NUMBERNum: return "NUMBER";                                                                                            // Generate.cs:124
                case UNITNum: return "UNIT";                                                                                                // Generate.cs:124
                case CODENum: return "CODE";                                                                                                // Generate.cs:124
                case CONCEPT_STRINGNum: return "CONCEPT_STRING";                                                                            // Generate.cs:124
                case DATETIMENum: return "DATETIME";                                                                                        // Generate.cs:124
                case TIMENum: return "TIME";                                                                                                // Generate.cs:124
                case CARDNum: return "CARD";                                                                                                // Generate.cs:124
                case REFERENCENum: return "REFERENCE";                                                                                      // Generate.cs:124
                case CARET_SEQUENCENum: return "CARET_SEQUENCE";                                                                            // Generate.cs:124
                case REGEXNum: return "REGEX";                                                                                              // Generate.cs:124
                case COMMA_DELIMITED_CODESNum: return "COMMA_DELIMITED_CODES";                                                              // Generate.cs:124
                case COMMA_DELIMITED_SEQUENCESNum: return "COMMA_DELIMITED_SEQUENCES";                                                      // Generate.cs:124
                case SEQUENCENum: return "SEQUENCE";                                                                                        // Generate.cs:124
                case WHITESPACENum: return "WHITESPACE";                                                                                    // Generate.cs:124
                case BLOCK_COMMENTNum: return "BLOCK_COMMENT";                                                                              // Generate.cs:124
                case LINE_COMMENTNum: return "LINE_COMMENT";                                                                                // Generate.cs:124
                default: throw new Exception($"unknown token index {tokenIndex}");                                                          // Generate.cs:98
            }                                                                                                                               // Generate.cs:99
        }                                                                                                                                   // Generate.cs:100
        //- TokenNumbers
        #endregion

        String text;
        Int32 textIndex;
        readonly Stack<NodeRule> nodeStack = new Stack<NodeRule>();
        NodeRule Current => this.nodeStack.Peek();

        public FSHListener(String text)
        {
            this.text = text;
            this.textIndex = 0;
            this.Head = new NodeRule("head");
            this.nodeStack.Push(this.Head);
        }

        NodeRule PushRule(String ruleName, Int32 startIndex)
        {
            Trace.WriteLine($"Enter {ruleName}");
            NodeRule retVal = new NodeRule(ruleName);
            this.Current.ChildNodes.Add(retVal);
            this.nodeStack.Push(retVal);
            return retVal;
        }

        void PopRule(String ruleName, Int32 startIndex)
        {
            Trace.WriteLine($"Exit {ruleName}");
            Debug.Assert(this.nodeStack.Peek().NodeType == ruleName);
            this.nodeStack.Pop();
        }

        /// <summary>
        /// Copy all bytes from current position up to but not
        /// including index position and return it.
        /// Update index
        /// </summary>
        String Consume(Int32 index)
        {
            Int32 length = index - this.textIndex;

            if (length < 0)
                throw new Exception("Internal index error");
            String retVal = this.text.Substring(this.textIndex, length);
            this.textIndex = index;
            //Trace.WriteLine($"  consumed {this.textIndex} .. {index}");
            return retVal;
        }

        /// <summary>
        /// Append a comment of all chars up to but not including index. 
        /// </summary>
        void AppendComment(Int32 index)
        {
            String comment = Consume(index);
            if (String.IsNullOrEmpty(comment))
                return;

            NodeComment node = new NodeComment("comment")
            {
                Comment = comment
            };
            this.Current.ChildNodes.Add(node);
        }

        /// <summary>
        /// Appends a NodeToken to current, and sets the
        /// text of that token.
        /// </summary>
        NodeToken AppendToken(ITerminalNode node)
        {
            this.AppendComment(node.Symbol.StartIndex);
            NodeToken retVal = new NodeToken("token");
            retVal.TokenValue = this.Consume(node.Symbol.StopIndex + 1);
            retVal.TokenName = GetTokenName(node.Symbol.Type);
            this.Current.ChildNodes.Add(retVal);
            return retVal;
        }

        /// <summary>
        /// Append a comment of all chars up to but not including index.
        /// then create new node and append it.
        /// Does not read the value of the node in any way.
        /// The caller must set that up.
        /// </summary>
        //T AppendNode<T>(Int32 index)
        //    where T : NodeBase, new()
        //{
        //    this.AppendComment(index);
        //    T retVal = new T();
        //    this.current.ChildNodes.Add(retVal);
        //    return retVal;
        //}

        //public override void EnterDoc(FSHParser.DocContext context)
        //{
        //    this.AppendComment(context.Start.StartIndex);
        //}

        //public override void ExitDoc(FSHParser.DocContext context)
        //{
        //}

        //+ VisitorMethods
        public override void EnterTargetType(FSHParser.TargetTypeContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("TargetType", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitTargetType(FSHParser.TargetTypeContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("TargetType", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterDoc(FSHParser.DocContext context)                                                                         // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Doc", context.Start.StartIndex);                                                                                 // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitDoc(FSHParser.DocContext context)                                                                          // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Doc", context.Stop.StopIndex);                                                                                    // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterEntity(FSHParser.EntityContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Entity", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitEntity(FSHParser.EntityContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Entity", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterAlias(FSHParser.AliasContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Alias", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitAlias(FSHParser.AliasContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Alias", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterProfile(FSHParser.ProfileContext context)                                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Profile", context.Start.StartIndex);                                                                             // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitProfile(FSHParser.ProfileContext context)                                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Profile", context.Stop.StopIndex);                                                                                // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterExtension(FSHParser.ExtensionContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Extension", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitExtension(FSHParser.ExtensionContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Extension", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterSdMetadata(FSHParser.SdMetadataContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("SdMetadata", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitSdMetadata(FSHParser.SdMetadataContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("SdMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterSdRule(FSHParser.SdRuleContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("SdRule", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitSdRule(FSHParser.SdRuleContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("SdRule", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterInstance(FSHParser.InstanceContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Instance", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitInstance(FSHParser.InstanceContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Instance", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterInstanceMetadata(FSHParser.InstanceMetadataContext context)                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("InstanceMetadata", context.Start.StartIndex);                                                                    // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitInstanceMetadata(FSHParser.InstanceMetadataContext context)                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("InstanceMetadata", context.Stop.StopIndex);                                                                       // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterInvariant(FSHParser.InvariantContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Invariant", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitInvariant(FSHParser.InvariantContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Invariant", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterInvariantMetadata(FSHParser.InvariantMetadataContext context)                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("InvariantMetadata", context.Start.StartIndex);                                                                   // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitInvariantMetadata(FSHParser.InvariantMetadataContext context)                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("InvariantMetadata", context.Stop.StopIndex);                                                                      // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterValueSet(FSHParser.ValueSetContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("ValueSet", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitValueSet(FSHParser.ValueSetContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("ValueSet", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsMetadata(FSHParser.VsMetadataContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsMetadata", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsMetadata(FSHParser.VsMetadataContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCodeSystem(FSHParser.CodeSystemContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("CodeSystem", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCodeSystem(FSHParser.CodeSystemContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("CodeSystem", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCsMetadata(FSHParser.CsMetadataContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("CsMetadata", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCsMetadata(FSHParser.CsMetadataContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("CsMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterRuleSet(FSHParser.RuleSetContext context)                                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("RuleSet", context.Start.StartIndex);                                                                             // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitRuleSet(FSHParser.RuleSetContext context)                                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("RuleSet", context.Stop.StopIndex);                                                                                // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterMapping(FSHParser.MappingContext context)                                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Mapping", context.Start.StartIndex);                                                                             // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitMapping(FSHParser.MappingContext context)                                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Mapping", context.Stop.StopIndex);                                                                                // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterMappingMetadata(FSHParser.MappingMetadataContext context)                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("MappingMetadata", context.Start.StartIndex);                                                                     // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitMappingMetadata(FSHParser.MappingMetadataContext context)                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("MappingMetadata", context.Stop.StopIndex);                                                                        // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterParent(FSHParser.ParentContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Parent", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitParent(FSHParser.ParentContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Parent", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterId(FSHParser.IdContext context)                                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Id", context.Start.StartIndex);                                                                                  // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitId(FSHParser.IdContext context)                                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Id", context.Stop.StopIndex);                                                                                     // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterTitle(FSHParser.TitleContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Title", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitTitle(FSHParser.TitleContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Title", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterDescription(FSHParser.DescriptionContext context)                                                         // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Description", context.Start.StartIndex);                                                                         // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitDescription(FSHParser.DescriptionContext context)                                                          // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Description", context.Stop.StopIndex);                                                                            // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterExpression(FSHParser.ExpressionContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Expression", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitExpression(FSHParser.ExpressionContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Expression", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterXpath(FSHParser.XpathContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Xpath", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitXpath(FSHParser.XpathContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Xpath", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterSeverity(FSHParser.SeverityContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Severity", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitSeverity(FSHParser.SeverityContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Severity", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterInstanceOf(FSHParser.InstanceOfContext context)                                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("InstanceOf", context.Start.StartIndex);                                                                          // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitInstanceOf(FSHParser.InstanceOfContext context)                                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("InstanceOf", context.Stop.StopIndex);                                                                             // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterUsage(FSHParser.UsageContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Usage", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitUsage(FSHParser.UsageContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Usage", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterMixins(FSHParser.MixinsContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Mixins", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitMixins(FSHParser.MixinsContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Mixins", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterSource(FSHParser.SourceContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Source", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitSource(FSHParser.SourceContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Source", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterTarget(FSHParser.TargetContext context)                                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Target", context.Start.StartIndex);                                                                              // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitTarget(FSHParser.TargetContext context)                                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Target", context.Stop.StopIndex);                                                                                 // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCardRule(FSHParser.CardRuleContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("CardRule", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCardRule(FSHParser.CardRuleContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("CardRule", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterFlagRule(FSHParser.FlagRuleContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("FlagRule", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitFlagRule(FSHParser.FlagRuleContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("FlagRule", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterValueSetRule(FSHParser.ValueSetRuleContext context)                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("ValueSetRule", context.Start.StartIndex);                                                                        // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitValueSetRule(FSHParser.ValueSetRuleContext context)                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("ValueSetRule", context.Stop.StopIndex);                                                                           // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterFixedValueRule(FSHParser.FixedValueRuleContext context)                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("FixedValueRule", context.Start.StartIndex);                                                                      // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitFixedValueRule(FSHParser.FixedValueRuleContext context)                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("FixedValueRule", context.Stop.StopIndex);                                                                         // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterContainsRule(FSHParser.ContainsRuleContext context)                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("ContainsRule", context.Start.StartIndex);                                                                        // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitContainsRule(FSHParser.ContainsRuleContext context)                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("ContainsRule", context.Stop.StopIndex);                                                                           // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterOnlyRule(FSHParser.OnlyRuleContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("OnlyRule", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitOnlyRule(FSHParser.OnlyRuleContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("OnlyRule", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterObeysRule(FSHParser.ObeysRuleContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("ObeysRule", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitObeysRule(FSHParser.ObeysRuleContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("ObeysRule", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCaretValueRule(FSHParser.CaretValueRuleContext context)                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("CaretValueRule", context.Start.StartIndex);                                                                      // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCaretValueRule(FSHParser.CaretValueRuleContext context)                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("CaretValueRule", context.Stop.StopIndex);                                                                         // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterMappingRule(FSHParser.MappingRuleContext context)                                                         // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("MappingRule", context.Start.StartIndex);                                                                         // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitMappingRule(FSHParser.MappingRuleContext context)                                                          // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("MappingRule", context.Stop.StopIndex);                                                                            // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterMacroRule(FSHParser.MacroRuleContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("MacroRule", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitMacroRule(FSHParser.MacroRuleContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("MacroRule", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsComponent(FSHParser.VsComponentContext context)                                                         // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsComponent", context.Start.StartIndex);                                                                         // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsComponent(FSHParser.VsComponentContext context)                                                          // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsComponent", context.Stop.StopIndex);                                                                            // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsConceptComponent(FSHParser.VsConceptComponentContext context)                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsConceptComponent", context.Start.StartIndex);                                                                  // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsConceptComponent(FSHParser.VsConceptComponentContext context)                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsConceptComponent", context.Stop.StopIndex);                                                                     // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFilterComponent(FSHParser.VsFilterComponentContext context)                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFilterComponent", context.Start.StartIndex);                                                                   // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFilterComponent(FSHParser.VsFilterComponentContext context)                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFilterComponent", context.Stop.StopIndex);                                                                      // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsComponentFrom(FSHParser.VsComponentFromContext context)                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsComponentFrom", context.Start.StartIndex);                                                                     // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsComponentFrom(FSHParser.VsComponentFromContext context)                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsComponentFrom", context.Stop.StopIndex);                                                                        // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFromSystem(FSHParser.VsFromSystemContext context)                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFromSystem", context.Start.StartIndex);                                                                        // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFromSystem(FSHParser.VsFromSystemContext context)                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFromSystem", context.Stop.StopIndex);                                                                           // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFromValueset(FSHParser.VsFromValuesetContext context)                                                   // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFromValueset", context.Start.StartIndex);                                                                      // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFromValueset(FSHParser.VsFromValuesetContext context)                                                    // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFromValueset", context.Stop.StopIndex);                                                                         // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFilterList(FSHParser.VsFilterListContext context)                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFilterList", context.Start.StartIndex);                                                                        // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFilterList(FSHParser.VsFilterListContext context)                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFilterList", context.Stop.StopIndex);                                                                           // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFilterDefinition(FSHParser.VsFilterDefinitionContext context)                                           // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFilterDefinition", context.Start.StartIndex);                                                                  // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFilterDefinition(FSHParser.VsFilterDefinitionContext context)                                            // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFilterDefinition", context.Stop.StopIndex);                                                                     // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFilterOperator(FSHParser.VsFilterOperatorContext context)                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFilterOperator", context.Start.StartIndex);                                                                    // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFilterOperator(FSHParser.VsFilterOperatorContext context)                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFilterOperator", context.Stop.StopIndex);                                                                       // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterVsFilterValue(FSHParser.VsFilterValueContext context)                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("VsFilterValue", context.Start.StartIndex);                                                                       // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitVsFilterValue(FSHParser.VsFilterValueContext context)                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("VsFilterValue", context.Stop.StopIndex);                                                                          // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterPath(FSHParser.PathContext context)                                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Path", context.Start.StartIndex);                                                                                // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitPath(FSHParser.PathContext context)                                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Path", context.Stop.StopIndex);                                                                                   // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterPaths(FSHParser.PathsContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Paths", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitPaths(FSHParser.PathsContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Paths", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCaretPath(FSHParser.CaretPathContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("CaretPath", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCaretPath(FSHParser.CaretPathContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("CaretPath", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterFlag(FSHParser.FlagContext context)                                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Flag", context.Start.StartIndex);                                                                                // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitFlag(FSHParser.FlagContext context)                                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Flag", context.Stop.StopIndex);                                                                                   // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterStrength(FSHParser.StrengthContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Strength", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitStrength(FSHParser.StrengthContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Strength", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterValue(FSHParser.ValueContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Value", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitValue(FSHParser.ValueContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Value", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterItem(FSHParser.ItemContext context)                                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Item", context.Start.StartIndex);                                                                                // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitItem(FSHParser.ItemContext context)                                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Item", context.Stop.StopIndex);                                                                                   // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterCode(FSHParser.CodeContext context)                                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Code", context.Start.StartIndex);                                                                                // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitCode(FSHParser.CodeContext context)                                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Code", context.Stop.StopIndex);                                                                                   // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterConcept(FSHParser.ConceptContext context)                                                                 // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Concept", context.Start.StartIndex);                                                                             // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitConcept(FSHParser.ConceptContext context)                                                                  // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Concept", context.Stop.StopIndex);                                                                                // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterQuantity(FSHParser.QuantityContext context)                                                               // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Quantity", context.Start.StartIndex);                                                                            // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitQuantity(FSHParser.QuantityContext context)                                                                // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Quantity", context.Stop.StopIndex);                                                                               // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterRatio(FSHParser.RatioContext context)                                                                     // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Ratio", context.Start.StartIndex);                                                                               // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitRatio(FSHParser.RatioContext context)                                                                      // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Ratio", context.Stop.StopIndex);                                                                                  // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterReference(FSHParser.ReferenceContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Reference", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitReference(FSHParser.ReferenceContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Reference", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterRatioPart(FSHParser.RatioPartContext context)                                                             // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("RatioPart", context.Start.StartIndex);                                                                           // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitRatioPart(FSHParser.RatioPartContext context)                                                              // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("RatioPart", context.Stop.StopIndex);                                                                              // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        public override void EnterBool(FSHParser.BoolContext context)                                                                       // Generate.cs:59
        {                                                                                                                                   // Generate.cs:60
            this.PushRule("Bool", context.Start.StartIndex);                                                                                // Generate.cs:61
        }                                                                                                                                   // Generate.cs:62
        public override void ExitBool(FSHParser.BoolContext context)                                                                        // Generate.cs:72
        {                                                                                                                                   // Generate.cs:73
            this.PopRule("Bool", context.Stop.StopIndex);                                                                                   // Generate.cs:74
        }                                                                                                                                   // Generate.cs:75
        //- VisitorMethods

        public override void VisitTerminal(ITerminalNode node)
        {
            if (node.Symbol.StartIndex > node.Symbol.StopIndex)
            {
                if (node.GetText() != "<EOF>")
                    throw new Exception("Unexpected EOF");
                this.AppendComment(node.Symbol.StartIndex);
                return;
            }
            AppendToken(node);
            base.VisitTerminal(node);
        }
    }
}
