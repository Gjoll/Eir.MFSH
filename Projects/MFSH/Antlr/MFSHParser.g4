parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | mCommands)* EOF;
fsh: LINE | BLANKLINE | LASTLINE | BLANKLASTLINE;

mCommands: mModeStart mCommand+ (mModeEnd | EOF);
mModeStart: MSTART;
mModeEnd: MMODEEND;

mCommand: mInclude | mUse | mMacro | mApply | mEnd ;
mInclude: MINCLUDE MSTRING ;
mUse: MUSE MSTRING ;
mMacro: MMACRO MPNAME ( MOPAR (MPNAME (MCOMMA MPNAME)* )? MCPAR );
mApply: MAPPLY MPNAME MOPAR ( mString (MCOMMA mString)*)? MCPAR ;
mEnd: MEND ;

mString: mSingleString | mlString ;
mSingleString: MSTRING ;
mlString: MSTARTMLSTRING mlCont? (mlText mlCont?)* MMLENDSTRING ;
mlCont: MLCONT ;
mlText: MLTEXT ;
