lexer grammar MFSHLexer;

APPLY: 'apply';
END: 'end';
FSHLINE: 'FshLine';
INCLUDE: 'include';
INCOMPATIBLE: 'incompatible';
JSONARRAY: 'jsonArray';
MACRO: 'macro';
ONCE: 'once';
PROFILE: 'profile';
TEXT: 'text';
USE: 'use';

STRING: '"' (~('"' | '\\' | '\r' | '\n') | '\\' ('"' | '\\'))* '"';
MULTILINE_STRING: '"""' .*? '"""' ;

OPAR: '(' ;
COMMA: ',' ;
CPAR: ')' ;
GT: '>' ;

NAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;

WS: [ \n\r\t]+ -> skip;
