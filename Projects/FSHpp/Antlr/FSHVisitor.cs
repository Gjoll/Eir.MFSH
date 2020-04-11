//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from FSH.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace FSHpp {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="FSHParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IFSHVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.doc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDoc([NotNull] FSHParser.DocContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.entity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEntity([NotNull] FSHParser.EntityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.alias"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAlias([NotNull] FSHParser.AliasContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.profile"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProfile([NotNull] FSHParser.ProfileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.extension"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExtension([NotNull] FSHParser.ExtensionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.sdMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSdMetadata([NotNull] FSHParser.SdMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.sdRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSdRule([NotNull] FSHParser.SdRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.instance"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInstance([NotNull] FSHParser.InstanceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.instanceMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInstanceMetadata([NotNull] FSHParser.InstanceMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.invariant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInvariant([NotNull] FSHParser.InvariantContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.invariantMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInvariantMetadata([NotNull] FSHParser.InvariantMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.valueSet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValueSet([NotNull] FSHParser.ValueSetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsMetadata([NotNull] FSHParser.VsMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.codeSystem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCodeSystem([NotNull] FSHParser.CodeSystemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.csMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCsMetadata([NotNull] FSHParser.CsMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.ruleSet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRuleSet([NotNull] FSHParser.RuleSetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.macroDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMacroDef([NotNull] FSHParser.MacroDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.mapping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMapping([NotNull] FSHParser.MappingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.mappingMetadata"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMappingMetadata([NotNull] FSHParser.MappingMetadataContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.parent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParent([NotNull] FSHParser.ParentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.id"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitId([NotNull] FSHParser.IdContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.title"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTitle([NotNull] FSHParser.TitleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.description"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDescription([NotNull] FSHParser.DescriptionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] FSHParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.xpath"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitXpath([NotNull] FSHParser.XpathContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.severity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSeverity([NotNull] FSHParser.SeverityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.instanceOf"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInstanceOf([NotNull] FSHParser.InstanceOfContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.usage"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUsage([NotNull] FSHParser.UsageContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.mixins"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMixins([NotNull] FSHParser.MixinsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.source"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSource([NotNull] FSHParser.SourceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.target"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTarget([NotNull] FSHParser.TargetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.cardRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCardRule([NotNull] FSHParser.CardRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.flagRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFlagRule([NotNull] FSHParser.FlagRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.valueSetRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValueSetRule([NotNull] FSHParser.ValueSetRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.fixedValueRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFixedValueRule([NotNull] FSHParser.FixedValueRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.containsRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitContainsRule([NotNull] FSHParser.ContainsRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.onlyRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOnlyRule([NotNull] FSHParser.OnlyRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.obeysRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObeysRule([NotNull] FSHParser.ObeysRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.caretValueRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCaretValueRule([NotNull] FSHParser.CaretValueRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.mappingRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMappingRule([NotNull] FSHParser.MappingRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.macroRule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMacroRule([NotNull] FSHParser.MacroRuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsComponent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsComponent([NotNull] FSHParser.VsComponentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsConceptComponent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsConceptComponent([NotNull] FSHParser.VsConceptComponentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFilterComponent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFilterComponent([NotNull] FSHParser.VsFilterComponentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsComponentFrom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsComponentFrom([NotNull] FSHParser.VsComponentFromContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFromSystem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFromSystem([NotNull] FSHParser.VsFromSystemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFromValueset"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFromValueset([NotNull] FSHParser.VsFromValuesetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFilterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFilterList([NotNull] FSHParser.VsFilterListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFilterDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFilterDefinition([NotNull] FSHParser.VsFilterDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFilterOperator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFilterOperator([NotNull] FSHParser.VsFilterOperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.vsFilterValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVsFilterValue([NotNull] FSHParser.VsFilterValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.path"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPath([NotNull] FSHParser.PathContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.paths"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPaths([NotNull] FSHParser.PathsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.caretPath"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCaretPath([NotNull] FSHParser.CaretPathContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.flag"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFlag([NotNull] FSHParser.FlagContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.strength"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStrength([NotNull] FSHParser.StrengthContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] FSHParser.ValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.item"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitItem([NotNull] FSHParser.ItemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.code"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCode([NotNull] FSHParser.CodeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.concept"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConcept([NotNull] FSHParser.ConceptContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.quantity"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitQuantity([NotNull] FSHParser.QuantityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.ratio"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRatio([NotNull] FSHParser.RatioContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.reference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReference([NotNull] FSHParser.ReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.ratioPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRatioPart([NotNull] FSHParser.RatioPartContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.bool"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBool([NotNull] FSHParser.BoolContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FSHParser.targetType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTargetType([NotNull] FSHParser.TargetTypeContext context);
}
} // namespace FSHpp
