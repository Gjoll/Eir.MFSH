parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | macro)* EOF;
fsh: LINE ;

macro: mStart mCommand+ (mEnd | EOF);
mStart: MSTART;
mEnd: MEND;

mCommand: mInclude | mDefine | mApply | mEndDef ;
mInclude: MINCLUDE MSTRING ;
mDefine: MDEFINE MPNAME ( MOPAR (MPNAME (MCOMMA MPNAME)* )? MCPAR );
mApply: MAPPLY MPNAME MOPAR ( MSTRING (MCOMMA MSTRING)*)? MCPAR ;
mEndDef: MENDDEF ;
