parser grammar MFSHPreParser;
options { tokenVocab=MFSHPreLexer; }

text: (data | tickData | fshCmd | mfsh )* EOF;

fshCmd:
	WS? TEXT WS? COLON (WS | TEXT | LB | COLON)+ EOL
	;

data:
    WS? EOL
 |  WS? TEXT EOL
 |	WS? TEXT WS? ~COLON (WS | TEXT | LB | COLON)* EOL
	;

tickData: WS?  TICKTEXT EOL;
mfsh: WS? LB (WS | TEXT | LB)* EOL;

