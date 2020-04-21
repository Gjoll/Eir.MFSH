lexer grammar MFSHPreLexer;

EOL: '\n' ;
TEXT: ~[ \t#\n]+ ;
LB: '#' ;
WS: [ \t]+ ;

CR: '\r' -> skip ;