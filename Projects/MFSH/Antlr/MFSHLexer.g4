lexer grammar MFSHLexer;

fragment SPACE: ('\t' | ' ');
fragment LINE: '\n' SPACE* ~'#' ~'\n'*;
MSTART: '\n' SPACE* '#'                  -> pushMode(MFSH) ;
FSHLINE: LINE;
BLANKLINE: '\n';
CR: '\r' -> skip;

// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
Err: .;

mode MFSH;
MINCLUDE: 'include';
MUSE: 'use';
MMACRO: 'macro';
MEND: 'end';
MAPPLY: 'apply';
MMODECONT: '\n' SPACE* '#'						-> skip;
MSTRING: '"' (~'"')* '"' ;
MSTARTMLSTRING: '"' '"' '"' 					-> pushMode(MLSTRING);
MFSHLINE: LINE									-> popMode;
MBLANKLINE: '\n'								-> popMode;

MOPAR: '(' ;
MCOMMA: ',' ;
MCPAR: ')' ;
MPNAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;
MWS: [ \t\r] -> skip;

// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
MErr: .;

mode MLSTRING;

MLCONT: '\n' SPACE* '#';
MMLENDSTRING: '\n' SPACE* '#' SPACE* '"' '"' '"'				-> popMode;
MLTEXT: ~('\n')+ ;
