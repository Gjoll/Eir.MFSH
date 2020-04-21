parser grammar MFSHPreParser;

options { tokenVocab=MFSHPreLexer; }

text: (fsh | mfsh)* EOF;
fsh:
	WS? (TEXT (WS | TEXT | LB)*)? EOL
  |	WS? EOL
	;

mfsh: WS? LB (WS | TEXT | LB)* EOL
    ;

