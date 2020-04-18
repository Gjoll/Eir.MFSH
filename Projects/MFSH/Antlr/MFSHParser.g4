parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | macro)* EOF;
fsh: LINE | BLANKLINE;

macro: mModeStart mCommand+ (mModeEnd | EOF);
mModeStart: MSTART;
mModeEnd: MMODEEND;

mCommand: mInclude | mDefine | mApply | mEndDef ;
mInclude: MINCLUDE MSTRING ;
mMacro: MMACRO MPNAME ( MOPAR (MPNAME (MCOMMA MPNAME)* )? MCPAR );
mApply: MAPPLY MPNAME MOPAR ( MSTRING (MCOMMA MSTRING)*)? MCPAR ;
mEndDef: MENDDEF ;
