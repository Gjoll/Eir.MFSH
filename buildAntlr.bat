pushd %~dp0

pushd Projects\Eir.MFSH\Parser2
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package Eir.MFSH.Parser2 -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package Eir.MFSH.Parser2 -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4
popd

popd

pause
