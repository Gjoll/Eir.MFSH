parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   command* EOF;
command: fshLine | apply | end | include | macro | profile | use;

fshLine: FSHLINE anyString;

apply: APPLY NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;
include: INCLUDE anyString ;
macro: MACRO NAME OPAR (NAME (COMMA NAME)* )? CPAR redirect?;
profile: PROFILE NAME;
redirect: GT JSONARRAY OPAR singleString CPAR ;
use: USE anyString ;

anyString:  singleString | multiLineString ;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;
