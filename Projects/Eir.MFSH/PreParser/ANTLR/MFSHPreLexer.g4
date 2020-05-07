lexer grammar MFSHPreLexer;

COLON: ':' ;
EOL: '\n' ;
TICKTEXT: '`' ~[\n]* ;
TEXT: ~[ \t#\n:]+ ;

LB: '#' ;
WS: [ \t]+ ;

CR: '\r' -> skip ;