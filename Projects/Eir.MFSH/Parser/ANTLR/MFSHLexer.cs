//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.11.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MFSHLexer.g4 by ANTLR 4.11.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Eir.MFSH.Parser {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.11.1")]
[System.CLSCompliant(false)]
public partial class MFSHLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		MFSH=1, TEXTA=2, TEXTB=3, TEXTC=4, TEXTD=5, TICKTEXT=6, CR=7, APPLY=8, 
		CALL=9, END=10, IF=11, ELSE=12, FRAGMENT=13, INCOMPATIBLE=14, MACRO=15, 
		ONCE=16, SET=17, USE=18, PARENT=19, TITLE=20, DESCRIPTION=21, STRING=22, 
		MULTILINE_STRING=23, OPAR=24, COMMA=25, CPAR=26, COLON=27, BANG=28, FS=29, 
		GT=30, LT=31, GE=32, LE=33, EQ=34, EQ2=35, NAME=36, NUMBER=37, CMNT=38, 
		MFSHCont=39, MFSHExit=40, MFSHCR=41, MFSH_SPACE=42;
	public const int
		MFSHMode=1;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "MFSHMode"
	};

	public static readonly string[] ruleNames = {
		"MFSH", "TEXTA", "TEXTB", "TEXTC", "TEXTD", "TICKTEXT", "CR", "APPLY", 
		"CALL", "END", "IF", "ELSE", "FRAGMENT", "INCOMPATIBLE", "MACRO", "ONCE", 
		"SET", "USE", "PARENT", "TITLE", "DESCRIPTION", "STRING", "MULTILINE_STRING", 
		"OPAR", "COMMA", "CPAR", "COLON", "BANG", "FS", "GT", "LT", "GE", "LE", 
		"EQ", "EQ2", "NAMECHARS", "NAME", "NUMBER", "CMNT", "MFSHCont", "MFSHExit", 
		"MFSHCR", "MFSH_SPACE"
	};


	public MFSHLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public MFSHLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, null, "'apply'", "'call'", "'end'", 
		"'if'", "'else'", "'Fragment'", "'incompatible'", "'macro'", "'once'", 
		"'set'", "'use'", "'Parent'", "'Title'", "'Description'", null, null, 
		"'('", "','", "')'", "':'", "'!'", "'/'", "'>'", "'<'", "'>='", "'<='", 
		"'=='", "'='", null, null, null, null, "'\\n'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "MFSH", "TEXTA", "TEXTB", "TEXTC", "TEXTD", "TICKTEXT", "CR", "APPLY", 
		"CALL", "END", "IF", "ELSE", "FRAGMENT", "INCOMPATIBLE", "MACRO", "ONCE", 
		"SET", "USE", "PARENT", "TITLE", "DESCRIPTION", "STRING", "MULTILINE_STRING", 
		"OPAR", "COMMA", "CPAR", "COLON", "BANG", "FS", "GT", "LT", "GE", "LE", 
		"EQ", "EQ2", "NAME", "NUMBER", "CMNT", "MFSHCont", "MFSHExit", "MFSHCR", 
		"MFSH_SPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "MFSHLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static MFSHLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,42,372,6,-1,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,
		6,7,6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,
		2,14,7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,
		2,21,7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,
		2,28,7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,
		2,35,7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,
		2,42,7,42,1,0,5,0,90,8,0,10,0,12,0,93,9,0,1,0,1,0,1,0,1,0,1,1,5,1,100,
		8,1,10,1,12,1,103,9,1,1,1,4,1,106,8,1,11,1,12,1,107,1,1,5,1,111,8,1,10,
		1,12,1,114,9,1,1,1,1,1,1,2,5,2,119,8,2,10,2,12,2,122,9,2,1,2,4,2,125,8,
		2,11,2,12,2,126,1,2,5,2,130,8,2,10,2,12,2,133,9,2,1,2,1,2,1,3,5,3,138,
		8,3,10,3,12,3,141,9,3,1,3,1,3,1,4,5,4,146,8,4,10,4,12,4,149,9,4,1,4,1,
		4,1,5,5,5,154,8,5,10,5,12,5,157,9,5,1,5,1,5,5,5,161,8,5,10,5,12,5,164,
		9,5,1,5,3,5,167,8,5,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,
		8,1,8,1,8,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,
		1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,
		1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,14,1,15,
		1,15,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,18,1,18,
		1,18,1,18,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,
		1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,21,1,21,1,21,1,21,5,21,
		266,8,21,10,21,12,21,269,9,21,1,21,1,21,1,22,1,22,1,22,1,22,1,22,5,22,
		278,8,22,10,22,12,22,281,9,22,1,22,1,22,1,22,1,22,1,23,1,23,1,24,1,24,
		1,25,1,25,1,26,1,26,1,27,1,27,1,28,1,28,1,29,1,29,1,30,1,30,1,31,1,31,
		1,31,1,32,1,32,1,32,1,33,1,33,1,33,1,34,1,34,1,35,1,35,4,35,316,8,35,11,
		35,12,35,317,1,36,1,36,1,36,1,36,1,36,1,36,1,36,1,36,1,36,3,36,329,8,36,
		1,37,4,37,332,8,37,11,37,12,37,333,1,38,1,38,1,38,1,38,5,38,340,8,38,10,
		38,12,38,343,9,38,1,38,1,38,1,39,1,39,5,39,349,8,39,10,39,12,39,352,9,
		39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,41,1,41,1,41,1,41,1,42,4,
		42,367,8,42,11,42,12,42,368,1,42,1,42,1,279,0,43,2,1,4,2,6,3,8,4,10,5,
		12,6,14,7,16,8,18,9,20,10,22,11,24,12,26,13,28,14,30,15,32,16,34,17,36,
		18,38,19,40,20,42,21,44,22,46,23,48,24,50,25,52,26,54,27,56,28,58,29,60,
		30,62,31,64,32,66,33,68,34,70,35,72,0,74,36,76,37,78,38,80,39,82,40,84,
		41,86,42,2,0,1,10,3,0,9,10,13,13,32,32,2,0,9,9,32,32,4,0,9,10,32,32,35,
		35,96,96,1,0,10,10,1,1,10,10,4,0,10,10,13,13,34,34,92,92,2,0,65,90,97,
		122,4,0,45,46,48,57,65,90,97,122,2,0,46,46,48,57,2,0,10,10,13,13,390,0,
		2,1,0,0,0,0,4,1,0,0,0,0,6,1,0,0,0,0,8,1,0,0,0,0,10,1,0,0,0,0,12,1,0,0,
		0,0,14,1,0,0,0,1,16,1,0,0,0,1,18,1,0,0,0,1,20,1,0,0,0,1,22,1,0,0,0,1,24,
		1,0,0,0,1,26,1,0,0,0,1,28,1,0,0,0,1,30,1,0,0,0,1,32,1,0,0,0,1,34,1,0,0,
		0,1,36,1,0,0,0,1,38,1,0,0,0,1,40,1,0,0,0,1,42,1,0,0,0,1,44,1,0,0,0,1,46,
		1,0,0,0,1,48,1,0,0,0,1,50,1,0,0,0,1,52,1,0,0,0,1,54,1,0,0,0,1,56,1,0,0,
		0,1,58,1,0,0,0,1,60,1,0,0,0,1,62,1,0,0,0,1,64,1,0,0,0,1,66,1,0,0,0,1,68,
		1,0,0,0,1,70,1,0,0,0,1,74,1,0,0,0,1,76,1,0,0,0,1,78,1,0,0,0,1,80,1,0,0,
		0,1,82,1,0,0,0,1,84,1,0,0,0,1,86,1,0,0,0,2,91,1,0,0,0,4,101,1,0,0,0,6,
		120,1,0,0,0,8,139,1,0,0,0,10,147,1,0,0,0,12,155,1,0,0,0,14,168,1,0,0,0,
		16,172,1,0,0,0,18,178,1,0,0,0,20,183,1,0,0,0,22,187,1,0,0,0,24,190,1,0,
		0,0,26,195,1,0,0,0,28,204,1,0,0,0,30,217,1,0,0,0,32,223,1,0,0,0,34,228,
		1,0,0,0,36,232,1,0,0,0,38,236,1,0,0,0,40,243,1,0,0,0,42,249,1,0,0,0,44,
		261,1,0,0,0,46,272,1,0,0,0,48,286,1,0,0,0,50,288,1,0,0,0,52,290,1,0,0,
		0,54,292,1,0,0,0,56,294,1,0,0,0,58,296,1,0,0,0,60,298,1,0,0,0,62,300,1,
		0,0,0,64,302,1,0,0,0,66,305,1,0,0,0,68,308,1,0,0,0,70,311,1,0,0,0,72,313,
		1,0,0,0,74,328,1,0,0,0,76,331,1,0,0,0,78,335,1,0,0,0,80,346,1,0,0,0,82,
		357,1,0,0,0,84,361,1,0,0,0,86,366,1,0,0,0,88,90,7,0,0,0,89,88,1,0,0,0,
		90,93,1,0,0,0,91,89,1,0,0,0,91,92,1,0,0,0,92,94,1,0,0,0,93,91,1,0,0,0,
		94,95,5,35,0,0,95,96,1,0,0,0,96,97,6,0,0,0,97,3,1,0,0,0,98,100,7,1,0,0,
		99,98,1,0,0,0,100,103,1,0,0,0,101,99,1,0,0,0,101,102,1,0,0,0,102,105,1,
		0,0,0,103,101,1,0,0,0,104,106,8,2,0,0,105,104,1,0,0,0,106,107,1,0,0,0,
		107,105,1,0,0,0,107,108,1,0,0,0,108,112,1,0,0,0,109,111,8,3,0,0,110,109,
		1,0,0,0,111,114,1,0,0,0,112,110,1,0,0,0,112,113,1,0,0,0,113,115,1,0,0,
		0,114,112,1,0,0,0,115,116,5,10,0,0,116,5,1,0,0,0,117,119,7,1,0,0,118,117,
		1,0,0,0,119,122,1,0,0,0,120,118,1,0,0,0,120,121,1,0,0,0,121,124,1,0,0,
		0,122,120,1,0,0,0,123,125,8,2,0,0,124,123,1,0,0,0,125,126,1,0,0,0,126,
		124,1,0,0,0,126,127,1,0,0,0,127,131,1,0,0,0,128,130,8,3,0,0,129,128,1,
		0,0,0,130,133,1,0,0,0,131,129,1,0,0,0,131,132,1,0,0,0,132,134,1,0,0,0,
		133,131,1,0,0,0,134,135,5,0,0,1,135,7,1,0,0,0,136,138,7,1,0,0,137,136,
		1,0,0,0,138,141,1,0,0,0,139,137,1,0,0,0,139,140,1,0,0,0,140,142,1,0,0,
		0,141,139,1,0,0,0,142,143,5,10,0,0,143,9,1,0,0,0,144,146,7,1,0,0,145,144,
		1,0,0,0,146,149,1,0,0,0,147,145,1,0,0,0,147,148,1,0,0,0,148,150,1,0,0,
		0,149,147,1,0,0,0,150,151,5,0,0,1,151,11,1,0,0,0,152,154,7,1,0,0,153,152,
		1,0,0,0,154,157,1,0,0,0,155,153,1,0,0,0,155,156,1,0,0,0,156,158,1,0,0,
		0,157,155,1,0,0,0,158,162,5,96,0,0,159,161,8,3,0,0,160,159,1,0,0,0,161,
		164,1,0,0,0,162,160,1,0,0,0,162,163,1,0,0,0,163,166,1,0,0,0,164,162,1,
		0,0,0,165,167,7,4,0,0,166,165,1,0,0,0,167,13,1,0,0,0,168,169,5,13,0,0,
		169,170,1,0,0,0,170,171,6,6,1,0,171,15,1,0,0,0,172,173,5,97,0,0,173,174,
		5,112,0,0,174,175,5,112,0,0,175,176,5,108,0,0,176,177,5,121,0,0,177,17,
		1,0,0,0,178,179,5,99,0,0,179,180,5,97,0,0,180,181,5,108,0,0,181,182,5,
		108,0,0,182,19,1,0,0,0,183,184,5,101,0,0,184,185,5,110,0,0,185,186,5,100,
		0,0,186,21,1,0,0,0,187,188,5,105,0,0,188,189,5,102,0,0,189,23,1,0,0,0,
		190,191,5,101,0,0,191,192,5,108,0,0,192,193,5,115,0,0,193,194,5,101,0,
		0,194,25,1,0,0,0,195,196,5,70,0,0,196,197,5,114,0,0,197,198,5,97,0,0,198,
		199,5,103,0,0,199,200,5,109,0,0,200,201,5,101,0,0,201,202,5,110,0,0,202,
		203,5,116,0,0,203,27,1,0,0,0,204,205,5,105,0,0,205,206,5,110,0,0,206,207,
		5,99,0,0,207,208,5,111,0,0,208,209,5,109,0,0,209,210,5,112,0,0,210,211,
		5,97,0,0,211,212,5,116,0,0,212,213,5,105,0,0,213,214,5,98,0,0,214,215,
		5,108,0,0,215,216,5,101,0,0,216,29,1,0,0,0,217,218,5,109,0,0,218,219,5,
		97,0,0,219,220,5,99,0,0,220,221,5,114,0,0,221,222,5,111,0,0,222,31,1,0,
		0,0,223,224,5,111,0,0,224,225,5,110,0,0,225,226,5,99,0,0,226,227,5,101,
		0,0,227,33,1,0,0,0,228,229,5,115,0,0,229,230,5,101,0,0,230,231,5,116,0,
		0,231,35,1,0,0,0,232,233,5,117,0,0,233,234,5,115,0,0,234,235,5,101,0,0,
		235,37,1,0,0,0,236,237,5,80,0,0,237,238,5,97,0,0,238,239,5,114,0,0,239,
		240,5,101,0,0,240,241,5,110,0,0,241,242,5,116,0,0,242,39,1,0,0,0,243,244,
		5,84,0,0,244,245,5,105,0,0,245,246,5,116,0,0,246,247,5,108,0,0,247,248,
		5,101,0,0,248,41,1,0,0,0,249,250,5,68,0,0,250,251,5,101,0,0,251,252,5,
		115,0,0,252,253,5,99,0,0,253,254,5,114,0,0,254,255,5,105,0,0,255,256,5,
		112,0,0,256,257,5,116,0,0,257,258,5,105,0,0,258,259,5,111,0,0,259,260,
		5,110,0,0,260,43,1,0,0,0,261,267,5,34,0,0,262,266,8,5,0,0,263,264,5,92,
		0,0,264,266,9,0,0,0,265,262,1,0,0,0,265,263,1,0,0,0,266,269,1,0,0,0,267,
		265,1,0,0,0,267,268,1,0,0,0,268,270,1,0,0,0,269,267,1,0,0,0,270,271,5,
		34,0,0,271,45,1,0,0,0,272,273,5,34,0,0,273,274,5,34,0,0,274,275,5,34,0,
		0,275,279,1,0,0,0,276,278,9,0,0,0,277,276,1,0,0,0,278,281,1,0,0,0,279,
		280,1,0,0,0,279,277,1,0,0,0,280,282,1,0,0,0,281,279,1,0,0,0,282,283,5,
		34,0,0,283,284,5,34,0,0,284,285,5,34,0,0,285,47,1,0,0,0,286,287,5,40,0,
		0,287,49,1,0,0,0,288,289,5,44,0,0,289,51,1,0,0,0,290,291,5,41,0,0,291,
		53,1,0,0,0,292,293,5,58,0,0,293,55,1,0,0,0,294,295,5,33,0,0,295,57,1,0,
		0,0,296,297,5,47,0,0,297,59,1,0,0,0,298,299,5,62,0,0,299,61,1,0,0,0,300,
		301,5,60,0,0,301,63,1,0,0,0,302,303,5,62,0,0,303,304,5,61,0,0,304,65,1,
		0,0,0,305,306,5,60,0,0,306,307,5,61,0,0,307,67,1,0,0,0,308,309,5,61,0,
		0,309,310,5,61,0,0,310,69,1,0,0,0,311,312,5,61,0,0,312,71,1,0,0,0,313,
		315,7,6,0,0,314,316,7,7,0,0,315,314,1,0,0,0,316,317,1,0,0,0,317,315,1,
		0,0,0,317,318,1,0,0,0,318,73,1,0,0,0,319,329,3,72,35,0,320,321,5,36,0,
		0,321,322,3,72,35,0,322,323,5,36,0,0,323,329,1,0,0,0,324,325,5,37,0,0,
		325,326,3,72,35,0,326,327,5,37,0,0,327,329,1,0,0,0,328,319,1,0,0,0,328,
		320,1,0,0,0,328,324,1,0,0,0,329,75,1,0,0,0,330,332,7,8,0,0,331,330,1,0,
		0,0,332,333,1,0,0,0,333,331,1,0,0,0,333,334,1,0,0,0,334,77,1,0,0,0,335,
		336,5,47,0,0,336,337,5,47,0,0,337,341,1,0,0,0,338,340,8,9,0,0,339,338,
		1,0,0,0,340,343,1,0,0,0,341,339,1,0,0,0,341,342,1,0,0,0,342,344,1,0,0,
		0,343,341,1,0,0,0,344,345,6,38,1,0,345,79,1,0,0,0,346,350,5,10,0,0,347,
		349,7,1,0,0,348,347,1,0,0,0,349,352,1,0,0,0,350,348,1,0,0,0,350,351,1,
		0,0,0,351,353,1,0,0,0,352,350,1,0,0,0,353,354,5,35,0,0,354,355,1,0,0,0,
		355,356,6,39,1,0,356,81,1,0,0,0,357,358,5,10,0,0,358,359,1,0,0,0,359,360,
		6,40,2,0,360,83,1,0,0,0,361,362,5,13,0,0,362,363,1,0,0,0,363,364,6,41,
		1,0,364,85,1,0,0,0,365,367,7,1,0,0,366,365,1,0,0,0,367,368,1,0,0,0,368,
		366,1,0,0,0,368,369,1,0,0,0,369,370,1,0,0,0,370,371,6,42,1,0,371,87,1,
		0,0,0,23,0,1,91,101,107,112,120,126,131,139,147,155,162,166,265,267,279,
		317,328,333,341,350,368,3,5,1,0,6,0,0,4,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace Eir.MFSH.Parser
