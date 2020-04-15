using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Eir.FSHer.Antlr;

namespace Eir.FSHer
{
    class FSHListener : FSHBaseListener
    {
        public NodeRule Head;
        public string FileName;

        #region Tokens
        //+ RuleNames
        public const String RatioPartStr = "RatioPart";                                                                                     // Generate.cs:56
        public const String BoolStr = "Bool";                                                                                               // Generate.cs:56
        public const String TargetTypeStr = "TargetType";                                                                                   // Generate.cs:56
        public const String DocStr = "Doc";                                                                                                 // Generate.cs:56
        public const String EntityStr = "Entity";                                                                                           // Generate.cs:56
        public const String AliasStr = "Alias";                                                                                             // Generate.cs:56
        public const String ProfileStr = "Profile";                                                                                         // Generate.cs:56
        public const String ExtensionStr = "Extension";                                                                                     // Generate.cs:56
        public const String SdMetadataStr = "SdMetadata";                                                                                   // Generate.cs:56
        public const String SdRuleStr = "SdRule";                                                                                           // Generate.cs:56
        public const String InstanceStr = "Instance";                                                                                       // Generate.cs:56
        public const String InstanceMetadataStr = "InstanceMetadata";                                                                       // Generate.cs:56
        public const String InvariantStr = "Invariant";                                                                                     // Generate.cs:56
        public const String InvariantMetadataStr = "InvariantMetadata";                                                                     // Generate.cs:56
        public const String ValueSetStr = "ValueSet";                                                                                       // Generate.cs:56
        public const String VsMetadataStr = "VsMetadata";                                                                                   // Generate.cs:56
        public const String CodeSystemStr = "CodeSystem";                                                                                   // Generate.cs:56
        public const String CsMetadataStr = "CsMetadata";                                                                                   // Generate.cs:56
        public const String RuleSetStr = "RuleSet";                                                                                         // Generate.cs:56
        public const String MacroDefStr = "MacroDef";                                                                                       // Generate.cs:56
        public const String MacroDefMetadataStr = "MacroDefMetadata";                                                                       // Generate.cs:56
        public const String MappingStr = "Mapping";                                                                                         // Generate.cs:56
        public const String MappingMetadataStr = "MappingMetadata";                                                                         // Generate.cs:56
        public const String ParentStr = "Parent";                                                                                           // Generate.cs:56
        public const String IdStr = "Id";                                                                                                   // Generate.cs:56
        public const String TitleStr = "Title";                                                                                             // Generate.cs:56
        public const String DescriptionStr = "Description";                                                                                 // Generate.cs:56
        public const String ExpressionStr = "Expression";                                                                                   // Generate.cs:56
        public const String XpathStr = "Xpath";                                                                                             // Generate.cs:56
        public const String SeverityStr = "Severity";                                                                                       // Generate.cs:56
        public const String InstanceOfStr = "InstanceOf";                                                                                   // Generate.cs:56
        public const String UsageStr = "Usage";                                                                                             // Generate.cs:56
        public const String MixinsStr = "Mixins";                                                                                           // Generate.cs:56
        public const String SourceStr = "Source";                                                                                           // Generate.cs:56
        public const String TargetStr = "Target";                                                                                           // Generate.cs:56
        public const String CardRuleStr = "CardRule";                                                                                       // Generate.cs:56
        public const String FlagRuleStr = "FlagRule";                                                                                       // Generate.cs:56
        public const String ValueSetRuleStr = "ValueSetRule";                                                                               // Generate.cs:56
        public const String FixedValueRuleStr = "FixedValueRule";                                                                           // Generate.cs:56
        public const String ContainsRuleStr = "ContainsRule";                                                                               // Generate.cs:56
        public const String OnlyRuleStr = "OnlyRule";                                                                                       // Generate.cs:56
        public const String ObeysRuleStr = "ObeysRule";                                                                                     // Generate.cs:56
        public const String CaretValueRuleStr = "CaretValueRule";                                                                           // Generate.cs:56
        public const String MappingRuleStr = "MappingRule";                                                                                 // Generate.cs:56
        public const String MacroRuleStr = "MacroRule";                                                                                     // Generate.cs:56
        public const String VsComponentStr = "VsComponent";                                                                                 // Generate.cs:56
        public const String VsConceptComponentStr = "VsConceptComponent";                                                                   // Generate.cs:56
        public const String VsFilterComponentStr = "VsFilterComponent";                                                                     // Generate.cs:56
        public const String VsComponentFromStr = "VsComponentFrom";                                                                         // Generate.cs:56
        public const String VsFromSystemStr = "VsFromSystem";                                                                               // Generate.cs:56
        public const String VsFromValuesetStr = "VsFromValueset";                                                                           // Generate.cs:56
        public const String VsFilterListStr = "VsFilterList";                                                                               // Generate.cs:56
        public const String VsFilterDefinitionStr = "VsFilterDefinition";                                                                   // Generate.cs:56
        public const String VsFilterOperatorStr = "VsFilterOperator";                                                                       // Generate.cs:56
        public const String VsFilterValueStr = "VsFilterValue";                                                                             // Generate.cs:56
        public const String PathStr = "Path";                                                                                               // Generate.cs:56
        public const String PathsStr = "Paths";                                                                                             // Generate.cs:56
        public const String CaretPathStr = "CaretPath";                                                                                     // Generate.cs:56
        public const String FlagStr = "Flag";                                                                                               // Generate.cs:56
        public const String StrengthStr = "Strength";                                                                                       // Generate.cs:56
        public const String ValueStr = "Value";                                                                                             // Generate.cs:56
        public const String ItemStr = "Item";                                                                                               // Generate.cs:56
        public const String CodeStr = "Code";                                                                                               // Generate.cs:56
        public const String ConceptStr = "Concept";                                                                                         // Generate.cs:56
        public const String QuantityStr = "Quantity";                                                                                       // Generate.cs:56
        public const String RatioStr = "Ratio";                                                                                             // Generate.cs:56
        public const String ReferenceStr = "Reference";                                                                                     // Generate.cs:56
        //- RuleNames

        //+ TokenNumbers
        const Int32 KW_ALIASNum = 1;                                                                                                        // Generate.cs:121
        public const String KW_ALIASStr = "KW_ALIAS";                                                                                       // Generate.cs:129
        const Int32 KW_PROFILENum = 2;                                                                                                      // Generate.cs:121
        public const String KW_PROFILEStr = "KW_PROFILE";                                                                                   // Generate.cs:129
        const Int32 KW_EXTENSIONNum = 3;                                                                                                    // Generate.cs:121
        public const String KW_EXTENSIONStr = "KW_EXTENSION";                                                                               // Generate.cs:129
        const Int32 KW_INSTANCENum = 4;                                                                                                     // Generate.cs:121
        public const String KW_INSTANCEStr = "KW_INSTANCE";                                                                                 // Generate.cs:129
        const Int32 KW_INSTANCEOFNum = 5;                                                                                                   // Generate.cs:121
        public const String KW_INSTANCEOFStr = "KW_INSTANCEOF";                                                                             // Generate.cs:129
        const Int32 KW_INVARIANTNum = 6;                                                                                                    // Generate.cs:121
        public const String KW_INVARIANTStr = "KW_INVARIANT";                                                                               // Generate.cs:129
        const Int32 KW_VALUESETNum = 7;                                                                                                     // Generate.cs:121
        public const String KW_VALUESETStr = "KW_VALUESET";                                                                                 // Generate.cs:129
        const Int32 KW_CODESYSTEMNum = 8;                                                                                                   // Generate.cs:121
        public const String KW_CODESYSTEMStr = "KW_CODESYSTEM";                                                                             // Generate.cs:129
        const Int32 KW_RULESETNum = 9;                                                                                                      // Generate.cs:121
        public const String KW_RULESETStr = "KW_RULESET";                                                                                   // Generate.cs:129
        const Int32 KW_MAPPINGNum = 10;                                                                                                     // Generate.cs:121
        public const String KW_MAPPINGStr = "KW_MAPPING";                                                                                   // Generate.cs:129
        const Int32 KW_MIXINSNum = 11;                                                                                                      // Generate.cs:121
        public const String KW_MIXINSStr = "KW_MIXINS";                                                                                     // Generate.cs:129
        const Int32 KW_PARENTNum = 12;                                                                                                      // Generate.cs:121
        public const String KW_PARENTStr = "KW_PARENT";                                                                                     // Generate.cs:129
        const Int32 KW_IDNum = 13;                                                                                                          // Generate.cs:121
        public const String KW_IDStr = "KW_ID";                                                                                             // Generate.cs:129
        const Int32 KW_TITLENum = 14;                                                                                                       // Generate.cs:121
        public const String KW_TITLEStr = "KW_TITLE";                                                                                       // Generate.cs:129
        const Int32 KW_DESCRIPTIONNum = 15;                                                                                                 // Generate.cs:121
        public const String KW_DESCRIPTIONStr = "KW_DESCRIPTION";                                                                           // Generate.cs:129
        const Int32 KW_EXPRESSIONNum = 16;                                                                                                  // Generate.cs:121
        public const String KW_EXPRESSIONStr = "KW_EXPRESSION";                                                                             // Generate.cs:129
        const Int32 KW_XPATHNum = 17;                                                                                                       // Generate.cs:121
        public const String KW_XPATHStr = "KW_XPATH";                                                                                       // Generate.cs:129
        const Int32 KW_SEVERITYNum = 18;                                                                                                    // Generate.cs:121
        public const String KW_SEVERITYStr = "KW_SEVERITY";                                                                                 // Generate.cs:129
        const Int32 KW_USAGENum = 19;                                                                                                       // Generate.cs:121
        public const String KW_USAGEStr = "KW_USAGE";                                                                                       // Generate.cs:129
        const Int32 KW_SOURCENum = 20;                                                                                                      // Generate.cs:121
        public const String KW_SOURCEStr = "KW_SOURCE";                                                                                     // Generate.cs:129
        const Int32 KW_TARGETNum = 21;                                                                                                      // Generate.cs:121
        public const String KW_TARGETStr = "KW_TARGET";                                                                                     // Generate.cs:129
        const Int32 KW_MODNum = 22;                                                                                                         // Generate.cs:121
        public const String KW_MODStr = "KW_MOD";                                                                                           // Generate.cs:129
        const Int32 KW_MSNum = 23;                                                                                                          // Generate.cs:121
        public const String KW_MSStr = "KW_MS";                                                                                             // Generate.cs:129
        const Int32 KW_SUNum = 24;                                                                                                          // Generate.cs:121
        public const String KW_SUStr = "KW_SU";                                                                                             // Generate.cs:129
        const Int32 KW_TUNum = 25;                                                                                                          // Generate.cs:121
        public const String KW_TUStr = "KW_TU";                                                                                             // Generate.cs:129
        const Int32 KW_NORMATIVENum = 26;                                                                                                   // Generate.cs:121
        public const String KW_NORMATIVEStr = "KW_NORMATIVE";                                                                               // Generate.cs:129
        const Int32 KW_DRAFTNum = 27;                                                                                                       // Generate.cs:121
        public const String KW_DRAFTStr = "KW_DRAFT";                                                                                       // Generate.cs:129
        const Int32 KW_FROMNum = 28;                                                                                                        // Generate.cs:121
        public const String KW_FROMStr = "KW_FROM";                                                                                         // Generate.cs:129
        const Int32 KW_EXAMPLENum = 29;                                                                                                     // Generate.cs:121
        public const String KW_EXAMPLEStr = "KW_EXAMPLE";                                                                                   // Generate.cs:129
        const Int32 KW_PREFERREDNum = 30;                                                                                                   // Generate.cs:121
        public const String KW_PREFERREDStr = "KW_PREFERRED";                                                                               // Generate.cs:129
        const Int32 KW_EXTENSIBLENum = 31;                                                                                                  // Generate.cs:121
        public const String KW_EXTENSIBLEStr = "KW_EXTENSIBLE";                                                                             // Generate.cs:129
        const Int32 KW_REQUIREDNum = 32;                                                                                                    // Generate.cs:121
        public const String KW_REQUIREDStr = "KW_REQUIRED";                                                                                 // Generate.cs:129
        const Int32 KW_CONTAINSNum = 33;                                                                                                    // Generate.cs:121
        public const String KW_CONTAINSStr = "KW_CONTAINS";                                                                                 // Generate.cs:129
        const Int32 KW_NAMEDNum = 34;                                                                                                       // Generate.cs:121
        public const String KW_NAMEDStr = "KW_NAMED";                                                                                       // Generate.cs:129
        const Int32 KW_ANDNum = 35;                                                                                                         // Generate.cs:121
        public const String KW_ANDStr = "KW_AND";                                                                                           // Generate.cs:129
        const Int32 KW_ONLYNum = 36;                                                                                                        // Generate.cs:121
        public const String KW_ONLYStr = "KW_ONLY";                                                                                         // Generate.cs:129
        const Int32 KW_ORNum = 37;                                                                                                          // Generate.cs:121
        public const String KW_ORStr = "KW_OR";                                                                                             // Generate.cs:129
        const Int32 KW_OBEYSNum = 38;                                                                                                       // Generate.cs:121
        public const String KW_OBEYSStr = "KW_OBEYS";                                                                                       // Generate.cs:129
        const Int32 KW_TRUENum = 39;                                                                                                        // Generate.cs:121
        public const String KW_TRUEStr = "KW_TRUE";                                                                                         // Generate.cs:129
        const Int32 KW_FALSENum = 40;                                                                                                       // Generate.cs:121
        public const String KW_FALSEStr = "KW_FALSE";                                                                                       // Generate.cs:129
        const Int32 KW_EXCLUDENum = 41;                                                                                                     // Generate.cs:121
        public const String KW_EXCLUDEStr = "KW_EXCLUDE";                                                                                   // Generate.cs:129
        const Int32 KW_CODESNum = 42;                                                                                                       // Generate.cs:121
        public const String KW_CODESStr = "KW_CODES";                                                                                       // Generate.cs:129
        const Int32 KW_WHERENum = 43;                                                                                                       // Generate.cs:121
        public const String KW_WHEREStr = "KW_WHERE";                                                                                       // Generate.cs:129
        const Int32 KW_VSREFERENCENum = 44;                                                                                                 // Generate.cs:121
        public const String KW_VSREFERENCEStr = "KW_VSREFERENCE";                                                                           // Generate.cs:129
        const Int32 KW_SYSTEMNum = 45;                                                                                                      // Generate.cs:121
        public const String KW_SYSTEMStr = "KW_SYSTEM";                                                                                     // Generate.cs:129
        const Int32 KW_UNITSNum = 46;                                                                                                       // Generate.cs:121
        public const String KW_UNITSStr = "KW_UNITS";                                                                                       // Generate.cs:129
        const Int32 KW_EXACTLYNum = 47;                                                                                                     // Generate.cs:121
        public const String KW_EXACTLYStr = "KW_EXACTLY";                                                                                   // Generate.cs:129
        const Int32 KW_MACRONum = 48;                                                                                                       // Generate.cs:121
        public const String KW_MACROStr = "KW_MACRO";                                                                                       // Generate.cs:129
        const Int32 KW_MACRODEFNum = 49;                                                                                                    // Generate.cs:121
        public const String KW_MACRODEFStr = "KW_MACRODEF";                                                                                 // Generate.cs:129
        const Int32 EQUALNum = 50;                                                                                                          // Generate.cs:121
        public const String EQUALStr = "EQUAL";                                                                                             // Generate.cs:129
        const Int32 STARNum = 51;                                                                                                           // Generate.cs:121
        public const String STARStr = "STAR";                                                                                               // Generate.cs:129
        const Int32 COLONNum = 52;                                                                                                          // Generate.cs:121
        public const String COLONStr = "COLON";                                                                                             // Generate.cs:129
        const Int32 COMMANum = 53;                                                                                                          // Generate.cs:121
        public const String COMMAStr = "COMMA";                                                                                             // Generate.cs:129
        const Int32 ARROWNum = 54;                                                                                                          // Generate.cs:121
        public const String ARROWStr = "ARROW";                                                                                             // Generate.cs:129
        const Int32 STRINGNum = 55;                                                                                                         // Generate.cs:121
        public const String STRINGStr = "STRING";                                                                                           // Generate.cs:129
        const Int32 MULTILINE_STRINGNum = 56;                                                                                               // Generate.cs:121
        public const String MULTILINE_STRINGStr = "MULTILINE_STRING";                                                                       // Generate.cs:129
        const Int32 NUMBERNum = 57;                                                                                                         // Generate.cs:121
        public const String NUMBERStr = "NUMBER";                                                                                           // Generate.cs:129
        const Int32 UNITNum = 58;                                                                                                           // Generate.cs:121
        public const String UNITStr = "UNIT";                                                                                               // Generate.cs:129
        const Int32 CODENum = 59;                                                                                                           // Generate.cs:121
        public const String CODEStr = "CODE";                                                                                               // Generate.cs:129
        const Int32 CONCEPT_STRINGNum = 60;                                                                                                 // Generate.cs:121
        public const String CONCEPT_STRINGStr = "CONCEPT_STRING";                                                                           // Generate.cs:129
        const Int32 DATETIMENum = 61;                                                                                                       // Generate.cs:121
        public const String DATETIMEStr = "DATETIME";                                                                                       // Generate.cs:129
        const Int32 TIMENum = 62;                                                                                                           // Generate.cs:121
        public const String TIMEStr = "TIME";                                                                                               // Generate.cs:129
        const Int32 CARDNum = 63;                                                                                                           // Generate.cs:121
        public const String CARDStr = "CARD";                                                                                               // Generate.cs:129
        const Int32 REFERENCENum = 64;                                                                                                      // Generate.cs:121
        public const String REFERENCEStr = "REFERENCE";                                                                                     // Generate.cs:129
        const Int32 CARET_SEQUENCENum = 65;                                                                                                 // Generate.cs:121
        public const String CARET_SEQUENCEStr = "CARET_SEQUENCE";                                                                           // Generate.cs:129
        const Int32 REGEXNum = 66;                                                                                                          // Generate.cs:121
        public const String REGEXStr = "REGEX";                                                                                             // Generate.cs:129
        const Int32 COMMA_DELIMITED_CODESNum = 67;                                                                                          // Generate.cs:121
        public const String COMMA_DELIMITED_CODESStr = "COMMA_DELIMITED_CODES";                                                             // Generate.cs:129
        const Int32 COMMA_DELIMITED_SEQUENCESNum = 68;                                                                                      // Generate.cs:121
        public const String COMMA_DELIMITED_SEQUENCESStr = "COMMA_DELIMITED_SEQUENCES";                                                     // Generate.cs:129
        const Int32 SEQUENCENum = 69;                                                                                                       // Generate.cs:121
        public const String SEQUENCEStr = "SEQUENCE";                                                                                       // Generate.cs:129
        const Int32 WHITESPACENum = 70;                                                                                                     // Generate.cs:121
        public const String WHITESPACEStr = "WHITESPACE";                                                                                   // Generate.cs:129
        const Int32 BLOCK_COMMENTNum = 71;                                                                                                  // Generate.cs:121
        public const String BLOCK_COMMENTStr = "BLOCK_COMMENT";                                                                             // Generate.cs:129
        const Int32 LINE_COMMENTNum = 72;                                                                                                   // Generate.cs:121
        public const String LINE_COMMENTStr = "LINE_COMMENT";                                                                               // Generate.cs:129
        public static String GetTokenName(Int32 tokenIndex)                                                                                 // Generate.cs:94
        {                                                                                                                                   // Generate.cs:95
            switch (tokenIndex)                                                                                                             // Generate.cs:96
            {                                                                                                                               // Generate.cs:97
                case KW_ALIASNum: return "KW_ALIAS";                                                                                        // Generate.cs:125
                case KW_PROFILENum: return "KW_PROFILE";                                                                                    // Generate.cs:125
                case KW_EXTENSIONNum: return "KW_EXTENSION";                                                                                // Generate.cs:125
                case KW_INSTANCENum: return "KW_INSTANCE";                                                                                  // Generate.cs:125
                case KW_INSTANCEOFNum: return "KW_INSTANCEOF";                                                                              // Generate.cs:125
                case KW_INVARIANTNum: return "KW_INVARIANT";                                                                                // Generate.cs:125
                case KW_VALUESETNum: return "KW_VALUESET";                                                                                  // Generate.cs:125
                case KW_CODESYSTEMNum: return "KW_CODESYSTEM";                                                                              // Generate.cs:125
                case KW_RULESETNum: return "KW_RULESET";                                                                                    // Generate.cs:125
                case KW_MAPPINGNum: return "KW_MAPPING";                                                                                    // Generate.cs:125
                case KW_MIXINSNum: return "KW_MIXINS";                                                                                      // Generate.cs:125
                case KW_PARENTNum: return "KW_PARENT";                                                                                      // Generate.cs:125
                case KW_IDNum: return "KW_ID";                                                                                              // Generate.cs:125
                case KW_TITLENum: return "KW_TITLE";                                                                                        // Generate.cs:125
                case KW_DESCRIPTIONNum: return "KW_DESCRIPTION";                                                                            // Generate.cs:125
                case KW_EXPRESSIONNum: return "KW_EXPRESSION";                                                                              // Generate.cs:125
                case KW_XPATHNum: return "KW_XPATH";                                                                                        // Generate.cs:125
                case KW_SEVERITYNum: return "KW_SEVERITY";                                                                                  // Generate.cs:125
                case KW_USAGENum: return "KW_USAGE";                                                                                        // Generate.cs:125
                case KW_SOURCENum: return "KW_SOURCE";                                                                                      // Generate.cs:125
                case KW_TARGETNum: return "KW_TARGET";                                                                                      // Generate.cs:125
                case KW_MODNum: return "KW_MOD";                                                                                            // Generate.cs:125
                case KW_MSNum: return "KW_MS";                                                                                              // Generate.cs:125
                case KW_SUNum: return "KW_SU";                                                                                              // Generate.cs:125
                case KW_TUNum: return "KW_TU";                                                                                              // Generate.cs:125
                case KW_NORMATIVENum: return "KW_NORMATIVE";                                                                                // Generate.cs:125
                case KW_DRAFTNum: return "KW_DRAFT";                                                                                        // Generate.cs:125
                case KW_FROMNum: return "KW_FROM";                                                                                          // Generate.cs:125
                case KW_EXAMPLENum: return "KW_EXAMPLE";                                                                                    // Generate.cs:125
                case KW_PREFERREDNum: return "KW_PREFERRED";                                                                                // Generate.cs:125
                case KW_EXTENSIBLENum: return "KW_EXTENSIBLE";                                                                              // Generate.cs:125
                case KW_REQUIREDNum: return "KW_REQUIRED";                                                                                  // Generate.cs:125
                case KW_CONTAINSNum: return "KW_CONTAINS";                                                                                  // Generate.cs:125
                case KW_NAMEDNum: return "KW_NAMED";                                                                                        // Generate.cs:125
                case KW_ANDNum: return "KW_AND";                                                                                            // Generate.cs:125
                case KW_ONLYNum: return "KW_ONLY";                                                                                          // Generate.cs:125
                case KW_ORNum: return "KW_OR";                                                                                              // Generate.cs:125
                case KW_OBEYSNum: return "KW_OBEYS";                                                                                        // Generate.cs:125
                case KW_TRUENum: return "KW_TRUE";                                                                                          // Generate.cs:125
                case KW_FALSENum: return "KW_FALSE";                                                                                        // Generate.cs:125
                case KW_EXCLUDENum: return "KW_EXCLUDE";                                                                                    // Generate.cs:125
                case KW_CODESNum: return "KW_CODES";                                                                                        // Generate.cs:125
                case KW_WHERENum: return "KW_WHERE";                                                                                        // Generate.cs:125
                case KW_VSREFERENCENum: return "KW_VSREFERENCE";                                                                            // Generate.cs:125
                case KW_SYSTEMNum: return "KW_SYSTEM";                                                                                      // Generate.cs:125
                case KW_UNITSNum: return "KW_UNITS";                                                                                        // Generate.cs:125
                case KW_EXACTLYNum: return "KW_EXACTLY";                                                                                    // Generate.cs:125
                case KW_MACRONum: return "KW_MACRO";                                                                                        // Generate.cs:125
                case KW_MACRODEFNum: return "KW_MACRODEF";                                                                                  // Generate.cs:125
                case EQUALNum: return "EQUAL";                                                                                              // Generate.cs:125
                case STARNum: return "STAR";                                                                                                // Generate.cs:125
                case COLONNum: return "COLON";                                                                                              // Generate.cs:125
                case COMMANum: return "COMMA";                                                                                              // Generate.cs:125
                case ARROWNum: return "ARROW";                                                                                              // Generate.cs:125
                case STRINGNum: return "STRING";                                                                                            // Generate.cs:125
                case MULTILINE_STRINGNum: return "MULTILINE_STRING";                                                                        // Generate.cs:125
                case NUMBERNum: return "NUMBER";                                                                                            // Generate.cs:125
                case UNITNum: return "UNIT";                                                                                                // Generate.cs:125
                case CODENum: return "CODE";                                                                                                // Generate.cs:125
                case CONCEPT_STRINGNum: return "CONCEPT_STRING";                                                                            // Generate.cs:125
                case DATETIMENum: return "DATETIME";                                                                                        // Generate.cs:125
                case TIMENum: return "TIME";                                                                                                // Generate.cs:125
                case CARDNum: return "CARD";                                                                                                // Generate.cs:125
                case REFERENCENum: return "REFERENCE";                                                                                      // Generate.cs:125
                case CARET_SEQUENCENum: return "CARET_SEQUENCE";                                                                            // Generate.cs:125
                case REGEXNum: return "REGEX";                                                                                              // Generate.cs:125
                case COMMA_DELIMITED_CODESNum: return "COMMA_DELIMITED_CODES";                                                              // Generate.cs:125
                case COMMA_DELIMITED_SEQUENCESNum: return "COMMA_DELIMITED_SEQUENCES";                                                      // Generate.cs:125
                case SEQUENCENum: return "SEQUENCE";                                                                                        // Generate.cs:125
                case WHITESPACENum: return "WHITESPACE";                                                                                    // Generate.cs:125
                case BLOCK_COMMENTNum: return "BLOCK_COMMENT";                                                                              // Generate.cs:125
                case LINE_COMMENTNum: return "LINE_COMMENT";                                                                                // Generate.cs:125
                default: throw new Exception($"unknown token index {tokenIndex}");                                                          // Generate.cs:99
            }                                                                                                                               // Generate.cs:100
        }                                                                                                                                   // Generate.cs:101
        //- TokenNumbers
        #endregion

        String text;
        Int32 textIndex;
        readonly Stack<NodeRule> nodeStack = new Stack<NodeRule>();
        NodeRule Current => this.nodeStack.Peek();

        public FSHListener(String text, string fileName)
        {
            this.text = text;
            this.FileName = fileName;
            this.textIndex = 0;
            this.Head = new NodeRule("head");
            this.nodeStack.Push(this.Head);
        }

        NodeRule PushRule(String ruleName, Int32 lineNum, Int32 startIndex)
        {
            //Trace.WriteLine($"Enter {ruleName}");
            NodeRule retVal = new NodeRule(ruleName)
            {
                LineNum = lineNum,
                FileName = FileName
            };
            this.Current.ChildNodes.Add(retVal);
            this.nodeStack.Push(retVal);
            return retVal;
        }

        void PopRule(String ruleName, Int32 startIndex)
        {
            //Trace.WriteLine($"Exit {ruleName}");
            Debug.Assert(this.nodeStack.Peek().RuleName == ruleName);
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

            NodeComment node = new NodeComment()
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
            String tokenName = FSHListener.GetTokenName(node.Symbol.Type);
            NodeToken retVal = new NodeToken(
                GetTokenName(node.Symbol.Type),
                this.Consume(node.Symbol.StopIndex + 1)
                )
            {
                FileName = this.FileName,
                LineNum = node.Symbol.Line
            };
            this.Current.ChildNodes.Add(retVal);
            return retVal;
        }

        //+ VisitorMethods
        public override void EnterRatioPart(FSHParser.RatioPartContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(RatioPartStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitRatioPart(FSHParser.RatioPartContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("RatioPart", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterBool(FSHParser.BoolContext context)                                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(BoolStr, context.Start.Line, context.Start.StartIndex);                                                           // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitBool(FSHParser.BoolContext context)                                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Bool", context.Stop.StopIndex);                                                                                   // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterTargetType(FSHParser.TargetTypeContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(TargetTypeStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitTargetType(FSHParser.TargetTypeContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("TargetType", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterDoc(FSHParser.DocContext context)                                                                         // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(DocStr, context.Start.Line, context.Start.StartIndex);                                                            // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitDoc(FSHParser.DocContext context)                                                                          // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Doc", context.Stop.StopIndex);                                                                                    // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterEntity(FSHParser.EntityContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(EntityStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitEntity(FSHParser.EntityContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Entity", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterAlias(FSHParser.AliasContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(AliasStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitAlias(FSHParser.AliasContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Alias", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterProfile(FSHParser.ProfileContext context)                                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ProfileStr, context.Start.Line, context.Start.StartIndex);                                                        // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitProfile(FSHParser.ProfileContext context)                                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Profile", context.Stop.StopIndex);                                                                                // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterExtension(FSHParser.ExtensionContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ExtensionStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitExtension(FSHParser.ExtensionContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Extension", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterSdMetadata(FSHParser.SdMetadataContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(SdMetadataStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitSdMetadata(FSHParser.SdMetadataContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("SdMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterSdRule(FSHParser.SdRuleContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(SdRuleStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitSdRule(FSHParser.SdRuleContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("SdRule", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterInstance(FSHParser.InstanceContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(InstanceStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitInstance(FSHParser.InstanceContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Instance", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterInstanceMetadata(FSHParser.InstanceMetadataContext context)                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(InstanceMetadataStr, context.Start.Line, context.Start.StartIndex);                                               // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitInstanceMetadata(FSHParser.InstanceMetadataContext context)                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("InstanceMetadata", context.Stop.StopIndex);                                                                       // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterInvariant(FSHParser.InvariantContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(InvariantStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitInvariant(FSHParser.InvariantContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Invariant", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterInvariantMetadata(FSHParser.InvariantMetadataContext context)                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(InvariantMetadataStr, context.Start.Line, context.Start.StartIndex);                                              // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitInvariantMetadata(FSHParser.InvariantMetadataContext context)                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("InvariantMetadata", context.Stop.StopIndex);                                                                      // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterValueSet(FSHParser.ValueSetContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ValueSetStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitValueSet(FSHParser.ValueSetContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("ValueSet", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsMetadata(FSHParser.VsMetadataContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsMetadataStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsMetadata(FSHParser.VsMetadataContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCodeSystem(FSHParser.CodeSystemContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CodeSystemStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCodeSystem(FSHParser.CodeSystemContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("CodeSystem", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCsMetadata(FSHParser.CsMetadataContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CsMetadataStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCsMetadata(FSHParser.CsMetadataContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("CsMetadata", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterRuleSet(FSHParser.RuleSetContext context)                                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(RuleSetStr, context.Start.Line, context.Start.StartIndex);                                                        // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitRuleSet(FSHParser.RuleSetContext context)                                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("RuleSet", context.Stop.StopIndex);                                                                                // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMacroDef(FSHParser.MacroDefContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MacroDefStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMacroDef(FSHParser.MacroDefContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("MacroDef", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMacroDefMetadata(FSHParser.MacroDefMetadataContext context)                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MacroDefMetadataStr, context.Start.Line, context.Start.StartIndex);                                               // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMacroDefMetadata(FSHParser.MacroDefMetadataContext context)                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("MacroDefMetadata", context.Stop.StopIndex);                                                                       // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMapping(FSHParser.MappingContext context)                                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MappingStr, context.Start.Line, context.Start.StartIndex);                                                        // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMapping(FSHParser.MappingContext context)                                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Mapping", context.Stop.StopIndex);                                                                                // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMappingMetadata(FSHParser.MappingMetadataContext context)                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MappingMetadataStr, context.Start.Line, context.Start.StartIndex);                                                // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMappingMetadata(FSHParser.MappingMetadataContext context)                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("MappingMetadata", context.Stop.StopIndex);                                                                        // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterParent(FSHParser.ParentContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ParentStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitParent(FSHParser.ParentContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Parent", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterId(FSHParser.IdContext context)                                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(IdStr, context.Start.Line, context.Start.StartIndex);                                                             // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitId(FSHParser.IdContext context)                                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Id", context.Stop.StopIndex);                                                                                     // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterTitle(FSHParser.TitleContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(TitleStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitTitle(FSHParser.TitleContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Title", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterDescription(FSHParser.DescriptionContext context)                                                         // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(DescriptionStr, context.Start.Line, context.Start.StartIndex);                                                    // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitDescription(FSHParser.DescriptionContext context)                                                          // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Description", context.Stop.StopIndex);                                                                            // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterExpression(FSHParser.ExpressionContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ExpressionStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitExpression(FSHParser.ExpressionContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Expression", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterXpath(FSHParser.XpathContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(XpathStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitXpath(FSHParser.XpathContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Xpath", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterSeverity(FSHParser.SeverityContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(SeverityStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitSeverity(FSHParser.SeverityContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Severity", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterInstanceOf(FSHParser.InstanceOfContext context)                                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(InstanceOfStr, context.Start.Line, context.Start.StartIndex);                                                     // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitInstanceOf(FSHParser.InstanceOfContext context)                                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("InstanceOf", context.Stop.StopIndex);                                                                             // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterUsage(FSHParser.UsageContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(UsageStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitUsage(FSHParser.UsageContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Usage", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMixins(FSHParser.MixinsContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MixinsStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMixins(FSHParser.MixinsContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Mixins", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterSource(FSHParser.SourceContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(SourceStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitSource(FSHParser.SourceContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Source", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterTarget(FSHParser.TargetContext context)                                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(TargetStr, context.Start.Line, context.Start.StartIndex);                                                         // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitTarget(FSHParser.TargetContext context)                                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Target", context.Stop.StopIndex);                                                                                 // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCardRule(FSHParser.CardRuleContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CardRuleStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCardRule(FSHParser.CardRuleContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("CardRule", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterFlagRule(FSHParser.FlagRuleContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(FlagRuleStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitFlagRule(FSHParser.FlagRuleContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("FlagRule", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterValueSetRule(FSHParser.ValueSetRuleContext context)                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ValueSetRuleStr, context.Start.Line, context.Start.StartIndex);                                                   // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitValueSetRule(FSHParser.ValueSetRuleContext context)                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("ValueSetRule", context.Stop.StopIndex);                                                                           // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterFixedValueRule(FSHParser.FixedValueRuleContext context)                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(FixedValueRuleStr, context.Start.Line, context.Start.StartIndex);                                                 // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitFixedValueRule(FSHParser.FixedValueRuleContext context)                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("FixedValueRule", context.Stop.StopIndex);                                                                         // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterContainsRule(FSHParser.ContainsRuleContext context)                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ContainsRuleStr, context.Start.Line, context.Start.StartIndex);                                                   // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitContainsRule(FSHParser.ContainsRuleContext context)                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("ContainsRule", context.Stop.StopIndex);                                                                           // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterOnlyRule(FSHParser.OnlyRuleContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(OnlyRuleStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitOnlyRule(FSHParser.OnlyRuleContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("OnlyRule", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterObeysRule(FSHParser.ObeysRuleContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ObeysRuleStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitObeysRule(FSHParser.ObeysRuleContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("ObeysRule", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCaretValueRule(FSHParser.CaretValueRuleContext context)                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CaretValueRuleStr, context.Start.Line, context.Start.StartIndex);                                                 // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCaretValueRule(FSHParser.CaretValueRuleContext context)                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("CaretValueRule", context.Stop.StopIndex);                                                                         // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMappingRule(FSHParser.MappingRuleContext context)                                                         // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MappingRuleStr, context.Start.Line, context.Start.StartIndex);                                                    // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMappingRule(FSHParser.MappingRuleContext context)                                                          // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("MappingRule", context.Stop.StopIndex);                                                                            // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterMacroRule(FSHParser.MacroRuleContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(MacroRuleStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitMacroRule(FSHParser.MacroRuleContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("MacroRule", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsComponent(FSHParser.VsComponentContext context)                                                         // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsComponentStr, context.Start.Line, context.Start.StartIndex);                                                    // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsComponent(FSHParser.VsComponentContext context)                                                          // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsComponent", context.Stop.StopIndex);                                                                            // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsConceptComponent(FSHParser.VsConceptComponentContext context)                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsConceptComponentStr, context.Start.Line, context.Start.StartIndex);                                             // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsConceptComponent(FSHParser.VsConceptComponentContext context)                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsConceptComponent", context.Stop.StopIndex);                                                                     // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFilterComponent(FSHParser.VsFilterComponentContext context)                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFilterComponentStr, context.Start.Line, context.Start.StartIndex);                                              // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFilterComponent(FSHParser.VsFilterComponentContext context)                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFilterComponent", context.Stop.StopIndex);                                                                      // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsComponentFrom(FSHParser.VsComponentFromContext context)                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsComponentFromStr, context.Start.Line, context.Start.StartIndex);                                                // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsComponentFrom(FSHParser.VsComponentFromContext context)                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsComponentFrom", context.Stop.StopIndex);                                                                        // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFromSystem(FSHParser.VsFromSystemContext context)                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFromSystemStr, context.Start.Line, context.Start.StartIndex);                                                   // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFromSystem(FSHParser.VsFromSystemContext context)                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFromSystem", context.Stop.StopIndex);                                                                           // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFromValueset(FSHParser.VsFromValuesetContext context)                                                   // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFromValuesetStr, context.Start.Line, context.Start.StartIndex);                                                 // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFromValueset(FSHParser.VsFromValuesetContext context)                                                    // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFromValueset", context.Stop.StopIndex);                                                                         // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFilterList(FSHParser.VsFilterListContext context)                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFilterListStr, context.Start.Line, context.Start.StartIndex);                                                   // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFilterList(FSHParser.VsFilterListContext context)                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFilterList", context.Stop.StopIndex);                                                                           // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFilterDefinition(FSHParser.VsFilterDefinitionContext context)                                           // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFilterDefinitionStr, context.Start.Line, context.Start.StartIndex);                                             // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFilterDefinition(FSHParser.VsFilterDefinitionContext context)                                            // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFilterDefinition", context.Stop.StopIndex);                                                                     // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFilterOperator(FSHParser.VsFilterOperatorContext context)                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFilterOperatorStr, context.Start.Line, context.Start.StartIndex);                                               // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFilterOperator(FSHParser.VsFilterOperatorContext context)                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFilterOperator", context.Stop.StopIndex);                                                                       // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterVsFilterValue(FSHParser.VsFilterValueContext context)                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(VsFilterValueStr, context.Start.Line, context.Start.StartIndex);                                                  // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitVsFilterValue(FSHParser.VsFilterValueContext context)                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("VsFilterValue", context.Stop.StopIndex);                                                                          // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterPath(FSHParser.PathContext context)                                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(PathStr, context.Start.Line, context.Start.StartIndex);                                                           // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitPath(FSHParser.PathContext context)                                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Path", context.Stop.StopIndex);                                                                                   // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterPaths(FSHParser.PathsContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(PathsStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitPaths(FSHParser.PathsContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Paths", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCaretPath(FSHParser.CaretPathContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CaretPathStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCaretPath(FSHParser.CaretPathContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("CaretPath", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterFlag(FSHParser.FlagContext context)                                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(FlagStr, context.Start.Line, context.Start.StartIndex);                                                           // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitFlag(FSHParser.FlagContext context)                                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Flag", context.Stop.StopIndex);                                                                                   // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterStrength(FSHParser.StrengthContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(StrengthStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitStrength(FSHParser.StrengthContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Strength", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterValue(FSHParser.ValueContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ValueStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitValue(FSHParser.ValueContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Value", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterItem(FSHParser.ItemContext context)                                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ItemStr, context.Start.Line, context.Start.StartIndex);                                                           // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitItem(FSHParser.ItemContext context)                                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Item", context.Stop.StopIndex);                                                                                   // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterCode(FSHParser.CodeContext context)                                                                       // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(CodeStr, context.Start.Line, context.Start.StartIndex);                                                           // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitCode(FSHParser.CodeContext context)                                                                        // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Code", context.Stop.StopIndex);                                                                                   // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterConcept(FSHParser.ConceptContext context)                                                                 // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ConceptStr, context.Start.Line, context.Start.StartIndex);                                                        // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitConcept(FSHParser.ConceptContext context)                                                                  // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Concept", context.Stop.StopIndex);                                                                                // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterQuantity(FSHParser.QuantityContext context)                                                               // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(QuantityStr, context.Start.Line, context.Start.StartIndex);                                                       // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitQuantity(FSHParser.QuantityContext context)                                                                // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Quantity", context.Stop.StopIndex);                                                                               // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterRatio(FSHParser.RatioContext context)                                                                     // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(RatioStr, context.Start.Line, context.Start.StartIndex);                                                          // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitRatio(FSHParser.RatioContext context)                                                                      // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Ratio", context.Stop.StopIndex);                                                                                  // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
        public override void EnterReference(FSHParser.ReferenceContext context)                                                             // Generate.cs:60
        {                                                                                                                                   // Generate.cs:61
            this.PushRule(ReferenceStr, context.Start.Line, context.Start.StartIndex);                                                      // Generate.cs:62
        }                                                                                                                                   // Generate.cs:63
        public override void ExitReference(FSHParser.ReferenceContext context)                                                              // Generate.cs:73
        {                                                                                                                                   // Generate.cs:74
            this.PopRule("Reference", context.Stop.StopIndex);                                                                              // Generate.cs:75
        }                                                                                                                                   // Generate.cs:76
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
