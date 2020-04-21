parser grammar MFSHParser;

options { tokenVocab=MFSHLexer; }

document    :   command* EOF;
command: fshLine | include | use | macro | apply | end ;

fshLine: FSHLINE STRING;
include: INCLUDE STRING ;
use: USE STRING ;
macro: MACRO NAME ( OPAR (NAME (COMMA NAME)* )? CPAR );
apply: APPLY NAME OPAR ( string (COMMA string)*)? CPAR ;
end: END ;

string:  singleString | multiLineString ;
multiLineString:  MULTILINE_STRING ;
singleString: STRING ;
