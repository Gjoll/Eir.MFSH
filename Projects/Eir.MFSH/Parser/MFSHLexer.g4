lexer grammar MFSHLexer;

MFSH: [ \t]* '#' -> pushMode(MFSHMode);
TEXT: [ \t]* (~'#' ~'\n'* ('\n' | EOF)) | ('\n' | EOF);

CR: '\r' -> skip ;


mode MFSHMode;

APPLY: 'apply';
END: 'end';
INCOMPATIBLE: 'incompatible';
MACRO: 'macro';
ONCE: 'once';

STRING: '"' (~('"' | '\\' | '\r' | '\n') | '\\' . )* '"';
MULTILINE_STRING: '"""' .*? '"""' ;

OPAR: '(' ;
COMMA: ',' ;
CPAR: ')' ;
GT: '>' ;

NAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;

MFSHCont: '\n' [ \t]* '#'			-> skip;
MFSHExit: '\n'						-> popMode;

MFSHCR: '\r' -> skip ;
MFSH_SPACE: [ \t]+ -> skip;
