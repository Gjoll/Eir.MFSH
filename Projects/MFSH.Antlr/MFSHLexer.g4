lexer grammar MFSHLexer;

MSTART: ('\t' | ' ')* '#'                  -> pushMode(MFSH) ;
LINE: ('\t' | ' ')* ~'#' (~'\n')* '\n';

CR: '\r' -> skip;
// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
Err: .;

mode MFSH;
MINCLUDE: 'include';
MDEFINE: 'define';
MENDDEF: 'enddef';
MAPPLY: 'apply';
MEND: '\n' ('\t' | ' ')* ~'#'                    -> popMode;
MSTRING: '"' (~'"')* '"' ;
MOPAR: '(' ;
MCOMMA: ',' ;
MCPAR: ')' ;
MPNAME: [A-Za-z][A-Za-z0-9]+ | '$' [A-Za-z][A-Za-z0-9]+ '$' | '%' [A-Za-z][A-Za-z0-9]+ '%' ;
MWS: [\r\n \t#] -> skip;

// Any character which does not match one of the above rules will appear in the token stream as
// an ErrorCharacter token. This ensures the lexer itself will never encounter a syntax error,
// so all error handling may be performed by the parser.
MErr: .;