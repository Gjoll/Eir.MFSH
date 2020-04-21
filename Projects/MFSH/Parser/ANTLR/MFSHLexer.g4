lexer grammar MFSHLexer;

FSHLINE: 'FshLine';
INCLUDE: 'include';
USE: 'use';
MACRO: 'macro';
END: 'end';
APPLY: 'apply';

STRING: '"' (~[\\"] | '\\"' | '\\\\')* '"' ;
MULTILINE_STRING: '"""' .*? '"""' ;

OPAR: '(' ;
COMMA: ',' ;
CPAR: ')' ;
NAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;

WS: [ \n\r\t]+ -> skip;
