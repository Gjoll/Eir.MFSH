parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | mCommands)* EOF;
fsh: LINE | BLANKLINE | LASTLINE ;

mCommands: mModeStart mCommand+ (mModeEnd | EOF);
mModeStart: MSTART;
mModeEnd: MMODEEND;

mCommand: mInclude | mUse | mMacro | mApply | mEnd ;
mInclude: MINCLUDE MSTRING ;
mUse: MUSE MSTRING ;
mMacro: MMACRO MPNAME ( MOPAR (MPNAME (MCOMMA MPNAME)* )? MCPAR );
mApply: MAPPLY MPNAME MOPAR ( MSTRING (MCOMMA MSTRING)*)? MCPAR ;
mEnd: MEND ;
