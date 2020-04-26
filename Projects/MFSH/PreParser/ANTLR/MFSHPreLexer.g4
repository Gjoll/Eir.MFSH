lexer grammar MFSHPreLexer;

PROFILE: 'Profile' WS? COLON;

COLON: ':' ;
EOL: '\n' ;
TEXT: ~[ \t#\n:]+ ;
LB: '#' ;
WS: [ \t]+ ;

CR: '\r' -> skip ;