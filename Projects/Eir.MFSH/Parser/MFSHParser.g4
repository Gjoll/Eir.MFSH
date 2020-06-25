parser grammar MFSHParser;
options { tokenVocab=MFSHLexer; }

document:
    command* EOF
    ;

command: textA | textB | textC | textD | tickText | mfshExit | mfshCmds;
textA: TEXTA ;
textB: TEXTB ;
textC: TEXTC ;
textD: TEXTD ;
tickText: TICKTEXT ;
mfshExit: MFSHExit;

mfshCmds: MFSH mfshCmd* ;

mfshCmd: apply | end | if | incompatible | macro | use ;

apply: APPLY NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;
incompatible: INCOMPATIBLE NAME ;
macro: MACRO SINGLE? ONCE? NAME OPAR (NAME (COMMA NAME)* )? CPAR redirect?;
redirect: GT singleString ;
use: USE NAME ;

if: IF condition elseIf* else? ;
elseIf: ELSE if condition ;
else: ELSE condition ;
condition: conditionEq | conditionLt | conditionLe | conditionGt | conditionGe ;

conditionEq: NAME EQ conditionValue ;
conditionLt: NAME LT conditionValue ;
conditionLe: NAME LE conditionValue ;
conditionGt: NAME GT conditionValue ;
conditionGe: NAME GE conditionValue ;
conditionValue: NUMBER | NAME | anyString ;

anyString:  singleString | multiLineString;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;

