parser grammar MFSHParser;
options { tokenVocab=MFSHLexer; }

document:
    command* EOF
    ;

command: text | mfshExit | mfshCmds;
text: TEXT ;
mfshExit: MFSHExit;

mfshCmds: MFSH mfshCmd* ;

mfshCmd: apply | end | incompatible | macro | redirect ;

apply: APPLY ONCE? NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;
incompatible: INCOMPATIBLE NAME ;
macro: MACRO NAME OPAR (NAME (COMMA NAME)* )? CPAR redirect?;
redirect: GT singleString ;

anyString:  singleString | multiLineString;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;

