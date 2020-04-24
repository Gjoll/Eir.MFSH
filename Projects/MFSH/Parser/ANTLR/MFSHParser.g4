parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   command* EOF;
command: fshLine | include | use | macro | apply | end ;

fshLine: FSHLINE anyString;
include: INCLUDE anyString ;
use: USE anyString ;
apply: APPLY NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
macro: MACRO NAME OPAR (NAME (COMMA NAME)* )? CPAR redirect?;
redirect: GT JSONARRAY OPAR singleString CPAR ;
end: END ;

anyString:  singleString | multiLineString ;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;
