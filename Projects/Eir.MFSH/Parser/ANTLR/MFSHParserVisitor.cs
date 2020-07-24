//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MFSHParser.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Eir.MFSH.Parser {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="MFSHParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IMFSHParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.document"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDocument([NotNull] MFSHParser.DocumentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommand([NotNull] MFSHParser.CommandContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.textA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextA([NotNull] MFSHParser.TextAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.textB"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextB([NotNull] MFSHParser.TextBContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.textC"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextC([NotNull] MFSHParser.TextCContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.textD"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextD([NotNull] MFSHParser.TextDContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.tickText"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTickText([NotNull] MFSHParser.TickTextContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.mfshExit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMfshExit([NotNull] MFSHParser.MfshExitContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.mfshCmds"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMfshCmds([NotNull] MFSHParser.MfshCmdsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.mfshCmd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMfshCmd([NotNull] MFSHParser.MfshCmdContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.apply"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitApply([NotNull] MFSHParser.ApplyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.end"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnd([NotNull] MFSHParser.EndContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.incompatible"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIncompatible([NotNull] MFSHParser.IncompatibleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.macro"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMacro([NotNull] MFSHParser.MacroContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.redirect"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRedirect([NotNull] MFSHParser.RedirectContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.frag"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFrag([NotNull] MFSHParser.FragContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.parent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParent([NotNull] MFSHParser.ParentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.description"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDescription([NotNull] MFSHParser.DescriptionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.title"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTitle([NotNull] MFSHParser.TitleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.use"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUse([NotNull] MFSHParser.UseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.if"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIf([NotNull] MFSHParser.IfContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.elseIf"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseIf([NotNull] MFSHParser.ElseIfContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.else"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElse([NotNull] MFSHParser.ElseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondition([NotNull] MFSHParser.ConditionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionStrEq"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionStrEq([NotNull] MFSHParser.ConditionStrEqContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionNumEq"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionNumEq([NotNull] MFSHParser.ConditionNumEqContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionNumLt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionNumLt([NotNull] MFSHParser.ConditionNumLtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionNumLe"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionNumLe([NotNull] MFSHParser.ConditionNumLeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionNumGt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionNumGt([NotNull] MFSHParser.ConditionNumGtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionNumGe"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionNumGe([NotNull] MFSHParser.ConditionNumGeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionValueNum"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionValueNum([NotNull] MFSHParser.ConditionValueNumContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.conditionValueStr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionValueStr([NotNull] MFSHParser.ConditionValueStrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.anyString"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAnyString([NotNull] MFSHParser.AnyStringContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.multiLineString"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiLineString([NotNull] MFSHParser.MultiLineStringContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MFSHParser.singleString"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSingleString([NotNull] MFSHParser.SingleStringContext context);
}
} // namespace Eir.MFSH.Parser
