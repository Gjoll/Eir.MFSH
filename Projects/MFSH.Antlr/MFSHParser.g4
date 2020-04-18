parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | macro)* EOF;
fsh: LINE ;

macro: mStart mCommand+ (mEnd | EOF);
mStart: MSTART;
mEnd: MEND;

mCommand: mInclude | mDefine | mCall | mEndDef ;
mInclude: MINCLUDE MSTRING ;
mDefine: MDEFINE MPNAME ( MOPAR MPNAME (MCOMMA MPNAME)* MCPAR )?;
mCall: MCALL MPNAME MOPAR ( MSTRING (MCOMMA MSTRING)*)? MCPAR ;
mEndDef: MENDDEF ;
