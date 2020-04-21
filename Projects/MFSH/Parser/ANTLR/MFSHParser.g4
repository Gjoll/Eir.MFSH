parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   command* EOF;
command: fshLine | include | use | macro | apply | end ;

fshLine: FSHLINE anyString;
include: INCLUDE anyString ;
use: USE anyString ;
macro: MACRO NAME ( OPAR (NAME (COMMA NAME)* )? CPAR );
apply: APPLY NAME OPAR ( anyString (COMMA anyString)*)? CPAR ;
end: END ;

anyString:  singleString | multiLineString ;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;
