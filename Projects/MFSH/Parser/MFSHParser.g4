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

mfshCmd: apply | description | end | if | elseIf | else | set |
         frag | incompatible | macro | parent | title | use | call;

apply: APPLY ONCE? NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;
description: DESCRIPTION COLON anyString ;
frag: FRAGMENT COLON ONCE? NAME ;
incompatible: INCOMPATIBLE NAME ;
macro: MACRO ONCE? NAME OPAR (NAME (COMMA NAME)* )? CPAR ? redirect?;
redirect: GT singleString ;
parent: PARENT COLON NAME ;
set: SET NAME EQ2 anyString ;
call: CALL path nameString*;
title: TITLE COLON anyString ;
path: nameString ( FS nameString)* ;
use: USE NAME ;

if: IF condition ;
elseIf: ELSE IF condition ;
else: ELSE ;

condition: conditionStrEq | conditionBoolIs | conditionBoolIsNot |
           conditionNumEq | conditionNumLt | conditionNumLe | 
           conditionNumGt | conditionNumGe ;
conditionBoolIs: NAME ;
conditionBoolIsNot: BANG NAME ;
conditionStrEq: anyString EQ anyString;
conditionNumEq: conditionValueNum EQ conditionValueNum;
conditionNumLt: conditionValueNum LT conditionValueNum ;
conditionNumLe: conditionValueNum LE conditionValueNum ;
conditionNumGt: conditionValueNum GT conditionValueNum ;
conditionNumGe: conditionValueNum GE conditionValueNum ;
conditionValueNum: NAME | NUMBER ;
conditionValueStr: anyString ;

anyString:  singleString | multiLineString;
nameString:  NAME | singleString | multiLineString;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;
