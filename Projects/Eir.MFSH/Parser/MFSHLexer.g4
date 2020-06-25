lexer grammar MFSHLexer;

MFSH: [ \t]* '#' -> pushMode(MFSHMode);
TEXTA: [ \t]* ~[ \t#\n`]+  ~'\n'* '\n';
TEXTB: [ \t]* ~[ \t#\n`]+  ~'\n'* EOF;
TEXTC: [ \t]* '\n';
TEXTD: [ \t]* EOF;
TICKTEXT: [ \t]* '`' ~'\n'* ('\n' | EOF);

CR: '\r' -> skip ;


mode MFSHMode;

APPLY: 'apply';
END: 'end';
IF: 'if' ;
ELSE: 'else' ;
INCOMPATIBLE: 'incompatible';
MACRO: 'macro';
ONCE: 'once';
SINGLE: 'single';
USE: 'use';

STRING: '"' (~('"' | '\\' | '\r' | '\n') | '\\' . )* '"';
MULTILINE_STRING: '"""' .*? '"""' ;

OPAR: '(' ;
COMMA: ',' ;
CPAR: ')' ;
GT: '>' ;
LT: '<' ;
GE: '>=' ;
LE: '<=' ;
EQ: '==' ;

NAME: [A-Za-z][A-Za-z0-9.]+ | '$' [A-Za-z][A-Za-z0-9.]+ '$' | '%' [A-Za-z][A-Za-z0-9.]+ '%' ;
NUMBER: [0-9.]+;

MFSHCont: '\n' [ \t]* '#'			-> skip;
MFSHExit: '\n'						-> popMode;

MFSHCR: '\r' -> skip ;
MFSH_SPACE: [ \t]+ -> skip;
