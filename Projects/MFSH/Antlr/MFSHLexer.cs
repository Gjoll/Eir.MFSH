//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MFSHLexer.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace MFSH {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class MFSHLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		MSTART=1, FSHLINE=2, BLANKLINE=3, CR=4, Err=5, MINCLUDE=6, MUSE=7, MMACRO=8, 
		MEND=9, MAPPLY=10, MMODECONT=11, MSTRING=12, MSTARTMLSTRING=13, MFSHLINE=14, 
		MBLANKLINE=15, MOPAR=16, MCOMMA=17, MCPAR=18, MPNAME=19, MWS=20, MErr=21, 
		MLCONT=22, MMLENDSTRING=23, MLTEXT=24;
	public const int
		MFSH=1, MLSTRING=2;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "MFSH", "MLSTRING"
	};

	public static readonly string[] ruleNames = {
		"SPACE", "LINE", "MSTART", "FSHLINE", "BLANKLINE", "CR", "Err", "MINCLUDE", 
		"MUSE", "MMACRO", "MEND", "MAPPLY", "MMODECONT", "MSTRING", "MSTARTMLSTRING", 
		"MFSHLINE", "MBLANKLINE", "MOPAR", "MCOMMA", "MCPAR", "MPNAME", "MWS", 
		"MErr", "MLCONT", "MMLENDSTRING", "MLTEXT"
	};


	public MFSHLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public MFSHLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, "'\r'", null, "'include'", "'use'", "'macro'", 
		"'end'", "'apply'", null, null, null, null, null, "'('", "','", "')'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "MSTART", "FSHLINE", "BLANKLINE", "CR", "Err", "MINCLUDE", "MUSE", 
		"MMACRO", "MEND", "MAPPLY", "MMODECONT", "MSTRING", "MSTARTMLSTRING", 
		"MFSHLINE", "MBLANKLINE", "MOPAR", "MCOMMA", "MCPAR", "MPNAME", "MWS", 
		"MErr", "MLCONT", "MMLENDSTRING", "MLTEXT"
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

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static MFSHLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\x1A', '\xE2', '\b', '\x1', '\b', '\x1', '\b', '\x1', 
		'\x4', '\x2', '\t', '\x2', '\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', 
		'\x4', '\x4', '\x5', '\t', '\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', 
		'\t', '\a', '\x4', '\b', '\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', 
		'\t', '\n', '\x4', '\v', '\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', 
		'\t', '\r', '\x4', '\xE', '\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', 
		'\x10', '\t', '\x10', '\x4', '\x11', '\t', '\x11', '\x4', '\x12', '\t', 
		'\x12', '\x4', '\x13', '\t', '\x13', '\x4', '\x14', '\t', '\x14', '\x4', 
		'\x15', '\t', '\x15', '\x4', '\x16', '\t', '\x16', '\x4', '\x17', '\t', 
		'\x17', '\x4', '\x18', '\t', '\x18', '\x4', '\x19', '\t', '\x19', '\x4', 
		'\x1A', '\t', '\x1A', '\x4', '\x1B', '\t', '\x1B', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x3', '\x3', '\x3', '\a', '\x3', '>', '\n', '\x3', '\f', 
		'\x3', '\xE', '\x3', '\x41', '\v', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\a', '\x3', '\x45', '\n', '\x3', '\f', '\x3', '\xE', '\x3', 'H', '\v', 
		'\x3', '\x3', '\x4', '\x3', '\x4', '\a', '\x4', 'L', '\n', '\x4', '\f', 
		'\x4', '\xE', '\x4', 'O', '\v', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', 
		'\x6', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\b', 
		'\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', 
		'\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', 
		'\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', '\f', 
		'\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', 
		'\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\a', '\xE', '}', '\n', 
		'\xE', '\f', '\xE', '\xE', '\xE', '\x80', '\v', '\xE', '\x3', '\xE', '\x3', 
		'\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\a', '\xF', 
		'\x88', '\n', '\xF', '\f', '\xF', '\xE', '\xF', '\x8B', '\v', '\xF', '\x3', 
		'\xF', '\x3', '\xF', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', 
		'\x11', '\x3', '\x11', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', 
		'\x12', '\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', 
		'\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x6', '\x16', '\xA5', 
		'\n', '\x16', '\r', '\x16', '\xE', '\x16', '\xA6', '\x3', '\x16', '\x3', 
		'\x16', '\x3', '\x16', '\x6', '\x16', '\xAC', '\n', '\x16', '\r', '\x16', 
		'\xE', '\x16', '\xAD', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x16', '\x6', '\x16', '\xB4', '\n', '\x16', '\r', '\x16', '\xE', '\x16', 
		'\xB5', '\x3', '\x16', '\x5', '\x16', '\xB9', '\n', '\x16', '\x3', '\x17', 
		'\x3', '\x17', '\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', 
		'\x3', '\x19', '\x3', '\x19', '\a', '\x19', '\xC3', '\n', '\x19', '\f', 
		'\x19', '\xE', '\x19', '\xC6', '\v', '\x19', '\x3', '\x19', '\x3', '\x19', 
		'\x3', '\x1A', '\x3', '\x1A', '\a', '\x1A', '\xCC', '\n', '\x1A', '\f', 
		'\x1A', '\xE', '\x1A', '\xCF', '\v', '\x1A', '\x3', '\x1A', '\x3', '\x1A', 
		'\a', '\x1A', '\xD3', '\n', '\x1A', '\f', '\x1A', '\xE', '\x1A', '\xD6', 
		'\v', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', 
		'\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x6', '\x1B', '\xDF', '\n', 
		'\x1B', '\r', '\x1B', '\xE', '\x1B', '\xE0', '\x2', '\x2', '\x1C', '\x5', 
		'\x2', '\a', '\x2', '\t', '\x3', '\v', '\x4', '\r', '\x5', '\xF', '\x6', 
		'\x11', '\a', '\x13', '\b', '\x15', '\t', '\x17', '\n', '\x19', '\v', 
		'\x1B', '\f', '\x1D', '\r', '\x1F', '\xE', '!', '\xF', '#', '\x10', '%', 
		'\x11', '\'', '\x12', ')', '\x13', '+', '\x14', '-', '\x15', '/', '\x16', 
		'\x31', '\x17', '\x33', '\x18', '\x35', '\x19', '\x37', '\x1A', '\x5', 
		'\x2', '\x3', '\x4', '\t', '\x4', '\x2', '\v', '\v', '\"', '\"', '\x3', 
		'\x2', '%', '%', '\x3', '\x2', '\f', '\f', '\x3', '\x2', '$', '$', '\x4', 
		'\x2', '\x43', '\\', '\x63', '|', '\x5', '\x2', '\x32', ';', '\x43', '\\', 
		'\x63', '|', '\x5', '\x2', '\v', '\v', '\xF', '\xF', '\"', '\"', '\x2', 
		'\xEB', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'\x13', '\x3', '\x2', '\x2', '\x2', '\x3', '\x15', '\x3', '\x2', '\x2', 
		'\x2', '\x3', '\x17', '\x3', '\x2', '\x2', '\x2', '\x3', '\x19', '\x3', 
		'\x2', '\x2', '\x2', '\x3', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'\x1D', '\x3', '\x2', '\x2', '\x2', '\x3', '\x1F', '\x3', '\x2', '\x2', 
		'\x2', '\x3', '!', '\x3', '\x2', '\x2', '\x2', '\x3', '#', '\x3', '\x2', 
		'\x2', '\x2', '\x3', '%', '\x3', '\x2', '\x2', '\x2', '\x3', '\'', '\x3', 
		'\x2', '\x2', '\x2', '\x3', ')', '\x3', '\x2', '\x2', '\x2', '\x3', '+', 
		'\x3', '\x2', '\x2', '\x2', '\x3', '-', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'/', '\x3', '\x2', '\x2', '\x2', '\x3', '\x31', '\x3', '\x2', '\x2', '\x2', 
		'\x4', '\x33', '\x3', '\x2', '\x2', '\x2', '\x4', '\x35', '\x3', '\x2', 
		'\x2', '\x2', '\x4', '\x37', '\x3', '\x2', '\x2', '\x2', '\x5', '\x39', 
		'\x3', '\x2', '\x2', '\x2', '\a', ';', '\x3', '\x2', '\x2', '\x2', '\t', 
		'I', '\x3', '\x2', '\x2', '\x2', '\v', 'T', '\x3', '\x2', '\x2', '\x2', 
		'\r', 'V', '\x3', '\x2', '\x2', '\x2', '\xF', 'X', '\x3', '\x2', '\x2', 
		'\x2', '\x11', '\\', '\x3', '\x2', '\x2', '\x2', '\x13', '^', '\x3', '\x2', 
		'\x2', '\x2', '\x15', '\x66', '\x3', '\x2', '\x2', '\x2', '\x17', 'j', 
		'\x3', '\x2', '\x2', '\x2', '\x19', 'p', '\x3', '\x2', '\x2', '\x2', '\x1B', 
		't', '\x3', '\x2', '\x2', '\x2', '\x1D', 'z', '\x3', '\x2', '\x2', '\x2', 
		'\x1F', '\x85', '\x3', '\x2', '\x2', '\x2', '!', '\x8E', '\x3', '\x2', 
		'\x2', '\x2', '#', '\x94', '\x3', '\x2', '\x2', '\x2', '%', '\x98', '\x3', 
		'\x2', '\x2', '\x2', '\'', '\x9C', '\x3', '\x2', '\x2', '\x2', ')', '\x9E', 
		'\x3', '\x2', '\x2', '\x2', '+', '\xA0', '\x3', '\x2', '\x2', '\x2', '-', 
		'\xB8', '\x3', '\x2', '\x2', '\x2', '/', '\xBA', '\x3', '\x2', '\x2', 
		'\x2', '\x31', '\xBE', '\x3', '\x2', '\x2', '\x2', '\x33', '\xC0', '\x3', 
		'\x2', '\x2', '\x2', '\x35', '\xC9', '\x3', '\x2', '\x2', '\x2', '\x37', 
		'\xDE', '\x3', '\x2', '\x2', '\x2', '\x39', ':', '\t', '\x2', '\x2', '\x2', 
		':', '\x6', '\x3', '\x2', '\x2', '\x2', ';', '?', '\a', '\f', '\x2', '\x2', 
		'<', '>', '\x5', '\x5', '\x2', '\x2', '=', '<', '\x3', '\x2', '\x2', '\x2', 
		'>', '\x41', '\x3', '\x2', '\x2', '\x2', '?', '=', '\x3', '\x2', '\x2', 
		'\x2', '?', '@', '\x3', '\x2', '\x2', '\x2', '@', '\x42', '\x3', '\x2', 
		'\x2', '\x2', '\x41', '?', '\x3', '\x2', '\x2', '\x2', '\x42', '\x46', 
		'\n', '\x3', '\x2', '\x2', '\x43', '\x45', '\n', '\x4', '\x2', '\x2', 
		'\x44', '\x43', '\x3', '\x2', '\x2', '\x2', '\x45', 'H', '\x3', '\x2', 
		'\x2', '\x2', '\x46', '\x44', '\x3', '\x2', '\x2', '\x2', '\x46', 'G', 
		'\x3', '\x2', '\x2', '\x2', 'G', '\b', '\x3', '\x2', '\x2', '\x2', 'H', 
		'\x46', '\x3', '\x2', '\x2', '\x2', 'I', 'M', '\a', '\f', '\x2', '\x2', 
		'J', 'L', '\x5', '\x5', '\x2', '\x2', 'K', 'J', '\x3', '\x2', '\x2', '\x2', 
		'L', 'O', '\x3', '\x2', '\x2', '\x2', 'M', 'K', '\x3', '\x2', '\x2', '\x2', 
		'M', 'N', '\x3', '\x2', '\x2', '\x2', 'N', 'P', '\x3', '\x2', '\x2', '\x2', 
		'O', 'M', '\x3', '\x2', '\x2', '\x2', 'P', 'Q', '\a', '%', '\x2', '\x2', 
		'Q', 'R', '\x3', '\x2', '\x2', '\x2', 'R', 'S', '\b', '\x4', '\x2', '\x2', 
		'S', '\n', '\x3', '\x2', '\x2', '\x2', 'T', 'U', '\x5', '\a', '\x3', '\x2', 
		'U', '\f', '\x3', '\x2', '\x2', '\x2', 'V', 'W', '\a', '\f', '\x2', '\x2', 
		'W', '\xE', '\x3', '\x2', '\x2', '\x2', 'X', 'Y', '\a', '\xF', '\x2', 
		'\x2', 'Y', 'Z', '\x3', '\x2', '\x2', '\x2', 'Z', '[', '\b', '\a', '\x3', 
		'\x2', '[', '\x10', '\x3', '\x2', '\x2', '\x2', '\\', ']', '\v', '\x2', 
		'\x2', '\x2', ']', '\x12', '\x3', '\x2', '\x2', '\x2', '^', '_', '\a', 
		'k', '\x2', '\x2', '_', '`', '\a', 'p', '\x2', '\x2', '`', '\x61', '\a', 
		'\x65', '\x2', '\x2', '\x61', '\x62', '\a', 'n', '\x2', '\x2', '\x62', 
		'\x63', '\a', 'w', '\x2', '\x2', '\x63', '\x64', '\a', '\x66', '\x2', 
		'\x2', '\x64', '\x65', '\a', 'g', '\x2', '\x2', '\x65', '\x14', '\x3', 
		'\x2', '\x2', '\x2', '\x66', 'g', '\a', 'w', '\x2', '\x2', 'g', 'h', '\a', 
		'u', '\x2', '\x2', 'h', 'i', '\a', 'g', '\x2', '\x2', 'i', '\x16', '\x3', 
		'\x2', '\x2', '\x2', 'j', 'k', '\a', 'o', '\x2', '\x2', 'k', 'l', '\a', 
		'\x63', '\x2', '\x2', 'l', 'm', '\a', '\x65', '\x2', '\x2', 'm', 'n', 
		'\a', 't', '\x2', '\x2', 'n', 'o', '\a', 'q', '\x2', '\x2', 'o', '\x18', 
		'\x3', '\x2', '\x2', '\x2', 'p', 'q', '\a', 'g', '\x2', '\x2', 'q', 'r', 
		'\a', 'p', '\x2', '\x2', 'r', 's', '\a', '\x66', '\x2', '\x2', 's', '\x1A', 
		'\x3', '\x2', '\x2', '\x2', 't', 'u', '\a', '\x63', '\x2', '\x2', 'u', 
		'v', '\a', 'r', '\x2', '\x2', 'v', 'w', '\a', 'r', '\x2', '\x2', 'w', 
		'x', '\a', 'n', '\x2', '\x2', 'x', 'y', '\a', '{', '\x2', '\x2', 'y', 
		'\x1C', '\x3', '\x2', '\x2', '\x2', 'z', '~', '\a', '\f', '\x2', '\x2', 
		'{', '}', '\x5', '\x5', '\x2', '\x2', '|', '{', '\x3', '\x2', '\x2', '\x2', 
		'}', '\x80', '\x3', '\x2', '\x2', '\x2', '~', '|', '\x3', '\x2', '\x2', 
		'\x2', '~', '\x7F', '\x3', '\x2', '\x2', '\x2', '\x7F', '\x81', '\x3', 
		'\x2', '\x2', '\x2', '\x80', '~', '\x3', '\x2', '\x2', '\x2', '\x81', 
		'\x82', '\a', '%', '\x2', '\x2', '\x82', '\x83', '\x3', '\x2', '\x2', 
		'\x2', '\x83', '\x84', '\b', '\xE', '\x3', '\x2', '\x84', '\x1E', '\x3', 
		'\x2', '\x2', '\x2', '\x85', '\x89', '\a', '$', '\x2', '\x2', '\x86', 
		'\x88', '\n', '\x5', '\x2', '\x2', '\x87', '\x86', '\x3', '\x2', '\x2', 
		'\x2', '\x88', '\x8B', '\x3', '\x2', '\x2', '\x2', '\x89', '\x87', '\x3', 
		'\x2', '\x2', '\x2', '\x89', '\x8A', '\x3', '\x2', '\x2', '\x2', '\x8A', 
		'\x8C', '\x3', '\x2', '\x2', '\x2', '\x8B', '\x89', '\x3', '\x2', '\x2', 
		'\x2', '\x8C', '\x8D', '\a', '$', '\x2', '\x2', '\x8D', ' ', '\x3', '\x2', 
		'\x2', '\x2', '\x8E', '\x8F', '\a', '$', '\x2', '\x2', '\x8F', '\x90', 
		'\a', '$', '\x2', '\x2', '\x90', '\x91', '\a', '$', '\x2', '\x2', '\x91', 
		'\x92', '\x3', '\x2', '\x2', '\x2', '\x92', '\x93', '\b', '\x10', '\x4', 
		'\x2', '\x93', '\"', '\x3', '\x2', '\x2', '\x2', '\x94', '\x95', '\x5', 
		'\a', '\x3', '\x2', '\x95', '\x96', '\x3', '\x2', '\x2', '\x2', '\x96', 
		'\x97', '\b', '\x11', '\x5', '\x2', '\x97', '$', '\x3', '\x2', '\x2', 
		'\x2', '\x98', '\x99', '\a', '\f', '\x2', '\x2', '\x99', '\x9A', '\x3', 
		'\x2', '\x2', '\x2', '\x9A', '\x9B', '\b', '\x12', '\x5', '\x2', '\x9B', 
		'&', '\x3', '\x2', '\x2', '\x2', '\x9C', '\x9D', '\a', '*', '\x2', '\x2', 
		'\x9D', '(', '\x3', '\x2', '\x2', '\x2', '\x9E', '\x9F', '\a', '.', '\x2', 
		'\x2', '\x9F', '*', '\x3', '\x2', '\x2', '\x2', '\xA0', '\xA1', '\a', 
		'+', '\x2', '\x2', '\xA1', ',', '\x3', '\x2', '\x2', '\x2', '\xA2', '\xA4', 
		'\t', '\x6', '\x2', '\x2', '\xA3', '\xA5', '\t', '\a', '\x2', '\x2', '\xA4', 
		'\xA3', '\x3', '\x2', '\x2', '\x2', '\xA5', '\xA6', '\x3', '\x2', '\x2', 
		'\x2', '\xA6', '\xA4', '\x3', '\x2', '\x2', '\x2', '\xA6', '\xA7', '\x3', 
		'\x2', '\x2', '\x2', '\xA7', '\xB9', '\x3', '\x2', '\x2', '\x2', '\xA8', 
		'\xA9', '\a', '&', '\x2', '\x2', '\xA9', '\xAB', '\t', '\x6', '\x2', '\x2', 
		'\xAA', '\xAC', '\t', '\a', '\x2', '\x2', '\xAB', '\xAA', '\x3', '\x2', 
		'\x2', '\x2', '\xAC', '\xAD', '\x3', '\x2', '\x2', '\x2', '\xAD', '\xAB', 
		'\x3', '\x2', '\x2', '\x2', '\xAD', '\xAE', '\x3', '\x2', '\x2', '\x2', 
		'\xAE', '\xAF', '\x3', '\x2', '\x2', '\x2', '\xAF', '\xB9', '\a', '&', 
		'\x2', '\x2', '\xB0', '\xB1', '\a', '\'', '\x2', '\x2', '\xB1', '\xB3', 
		'\t', '\x6', '\x2', '\x2', '\xB2', '\xB4', '\t', '\a', '\x2', '\x2', '\xB3', 
		'\xB2', '\x3', '\x2', '\x2', '\x2', '\xB4', '\xB5', '\x3', '\x2', '\x2', 
		'\x2', '\xB5', '\xB3', '\x3', '\x2', '\x2', '\x2', '\xB5', '\xB6', '\x3', 
		'\x2', '\x2', '\x2', '\xB6', '\xB7', '\x3', '\x2', '\x2', '\x2', '\xB7', 
		'\xB9', '\a', '\'', '\x2', '\x2', '\xB8', '\xA2', '\x3', '\x2', '\x2', 
		'\x2', '\xB8', '\xA8', '\x3', '\x2', '\x2', '\x2', '\xB8', '\xB0', '\x3', 
		'\x2', '\x2', '\x2', '\xB9', '.', '\x3', '\x2', '\x2', '\x2', '\xBA', 
		'\xBB', '\t', '\b', '\x2', '\x2', '\xBB', '\xBC', '\x3', '\x2', '\x2', 
		'\x2', '\xBC', '\xBD', '\b', '\x17', '\x3', '\x2', '\xBD', '\x30', '\x3', 
		'\x2', '\x2', '\x2', '\xBE', '\xBF', '\v', '\x2', '\x2', '\x2', '\xBF', 
		'\x32', '\x3', '\x2', '\x2', '\x2', '\xC0', '\xC4', '\a', '\f', '\x2', 
		'\x2', '\xC1', '\xC3', '\x5', '\x5', '\x2', '\x2', '\xC2', '\xC1', '\x3', 
		'\x2', '\x2', '\x2', '\xC3', '\xC6', '\x3', '\x2', '\x2', '\x2', '\xC4', 
		'\xC2', '\x3', '\x2', '\x2', '\x2', '\xC4', '\xC5', '\x3', '\x2', '\x2', 
		'\x2', '\xC5', '\xC7', '\x3', '\x2', '\x2', '\x2', '\xC6', '\xC4', '\x3', 
		'\x2', '\x2', '\x2', '\xC7', '\xC8', '\a', '%', '\x2', '\x2', '\xC8', 
		'\x34', '\x3', '\x2', '\x2', '\x2', '\xC9', '\xCD', '\a', '\f', '\x2', 
		'\x2', '\xCA', '\xCC', '\x5', '\x5', '\x2', '\x2', '\xCB', '\xCA', '\x3', 
		'\x2', '\x2', '\x2', '\xCC', '\xCF', '\x3', '\x2', '\x2', '\x2', '\xCD', 
		'\xCB', '\x3', '\x2', '\x2', '\x2', '\xCD', '\xCE', '\x3', '\x2', '\x2', 
		'\x2', '\xCE', '\xD0', '\x3', '\x2', '\x2', '\x2', '\xCF', '\xCD', '\x3', 
		'\x2', '\x2', '\x2', '\xD0', '\xD4', '\a', '%', '\x2', '\x2', '\xD1', 
		'\xD3', '\x5', '\x5', '\x2', '\x2', '\xD2', '\xD1', '\x3', '\x2', '\x2', 
		'\x2', '\xD3', '\xD6', '\x3', '\x2', '\x2', '\x2', '\xD4', '\xD2', '\x3', 
		'\x2', '\x2', '\x2', '\xD4', '\xD5', '\x3', '\x2', '\x2', '\x2', '\xD5', 
		'\xD7', '\x3', '\x2', '\x2', '\x2', '\xD6', '\xD4', '\x3', '\x2', '\x2', 
		'\x2', '\xD7', '\xD8', '\a', '$', '\x2', '\x2', '\xD8', '\xD9', '\a', 
		'$', '\x2', '\x2', '\xD9', '\xDA', '\a', '$', '\x2', '\x2', '\xDA', '\xDB', 
		'\x3', '\x2', '\x2', '\x2', '\xDB', '\xDC', '\b', '\x1A', '\x5', '\x2', 
		'\xDC', '\x36', '\x3', '\x2', '\x2', '\x2', '\xDD', '\xDF', '\n', '\x4', 
		'\x2', '\x2', '\xDE', '\xDD', '\x3', '\x2', '\x2', '\x2', '\xDF', '\xE0', 
		'\x3', '\x2', '\x2', '\x2', '\xE0', '\xDE', '\x3', '\x2', '\x2', '\x2', 
		'\xE0', '\xE1', '\x3', '\x2', '\x2', '\x2', '\xE1', '\x38', '\x3', '\x2', 
		'\x2', '\x2', '\x12', '\x2', '\x3', '\x4', '?', '\x46', 'M', '~', '\x89', 
		'\xA6', '\xAD', '\xB5', '\xB8', '\xC4', '\xCD', '\xD4', '\xE0', '\x6', 
		'\a', '\x3', '\x2', '\b', '\x2', '\x2', '\a', '\x4', '\x2', '\x6', '\x2', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace MFSH
