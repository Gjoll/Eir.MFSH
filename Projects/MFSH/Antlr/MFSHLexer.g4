lexer grammar MFSHLexer;

fragment SPACE: ('\t' | ' ');
MSTART: SPACE* '#'                  -> pushMode(MFSH) ;
LINE: SPACE* ~('#' | '\n') (~'\n')* '\n';
BLANKLINE: SPACE* '\n';
LASTLINE: SPACE+ -> skip;
CR: '\r' -> skip;

// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
Err: .;

mode MFSH;
MINCLUDE: 'include';
MMACRO: 'macro';
MENDDEF: 'end';
MAPPLY: 'apply';
MMODEEND: '\n' SPACE* ~'#'                    -> popMode;
MSTRING: '"' (~'"')* '"' ;
MOPAR: '(' ;
MCOMMA: ',' ;
MCPAR: ')' ;
MPNAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;
MWS: [\n \t#] -> skip;

MCR: '\r' -> skip;
// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
MErr: .;