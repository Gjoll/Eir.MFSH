lexer grammar MFSHLexer;

fragment SPACE: ('\t' | ' ');
fragment EOLN: ('\n' | EOF);
MSTART: SPACE* '#'                  -> pushMode(MFSH) ;
LINE: SPACE* ~('#' | '\n' | ' ' | '\t') (~EOLN)* EOLN;
BLANKLINE: SPACE* EOLN ;
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
MMODEEND: '\n' SPACE* ~'#'                    -> popMode;
MMODECONT: '\n' SPACE* '#'                    -> skip;
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