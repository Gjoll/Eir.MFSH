lexer grammar MFSHLexer;

APPLY: 'apply';
END: 'end';
FSHLINE: 'FshLine';
INCLUDE: 'include';
JSONARRAY: 'jsonArray';
MACRO: 'macro';
USE: 'use';

STRING: '"' (~[\\"] | '\\"' | '\\\\')* '"' ;
MULTILINE_STRING: '"""' .*? '"""' ;

OPAR: '(' ;
COMMA: ',' ;
CPAR: ')' ;
GT: '>' ;

NAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;

WS: [ \n\r\t]+ -> skip;
