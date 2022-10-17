pushd %~dp0

pushd Projects\Eir.MFSH\Parser
java -jar ..\..\..\Lib\antlr-4.11.1-complete.jar -o .\Antlr -package Eir.MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\Lib\antlr-4.11.1-complete.jar -o .\Antlr -package Eir.MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4
popd

popd

pause
