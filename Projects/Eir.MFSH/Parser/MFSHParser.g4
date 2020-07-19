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

mfshCmd: apply | end | if | elseIf | else | frag | incompatible | macro | use ;

apply: APPLY NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;
incompatible: INCOMPATIBLE NAME ;
macro: MACRO SINGLE? ONCE? NAME OPAR (NAME (COMMA NAME)* )? CPAR ? redirect?;
redirect: GT singleString ;

frag: FRAG NAME COLON NAME anyString? ;

use: USE NAME ;

if: IF condition ;
elseIf: ELSE IF condition ;
else: ELSE ;

condition: conditionStrEq | conditionNumEq | conditionNumLt | conditionNumLe | conditionNumGt | conditionNumGe ;
conditionStrEq: anyString EQ anyString;
conditionNumEq: conditionValueNum EQ conditionValueNum;
conditionNumLt: conditionValueNum LT conditionValueNum ;
conditionNumLe: conditionValueNum LE conditionValueNum ;
conditionNumGt: conditionValueNum GT conditionValueNum ;
conditionNumGe: conditionValueNum GE conditionValueNum ;
conditionValueNum: NAME | NUMBER ;
conditionValueStr: anyString ;

anyString:  singleString | multiLineString;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;

