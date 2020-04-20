parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   (fsh | mCommands)* EOF;
fsh: FSHLINE | MFSHLINE | MBLANKLINE | BLANKLINE;

mCommands: mModeStart mCommand+ ;
mModeStart: MSTART;

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
