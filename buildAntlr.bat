pushd %~dp0
pushd Projects\MFSH\Antlr
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4
popd
popd

pause
