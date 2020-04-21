pushd %~dp0
pushd Projects\MFSH\PreParser\Antlr
java -jar ..\..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH.PreParser -no-listener -visitor -Dlanguage=CSharp MFSHPreLexer.g4
java -jar ..\..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH.PreParser -no-listener -visitor -Dlanguage=CSharp MFSHPreParser.g4
popd

pushd Projects\MFSH\Parser\Antlr
java -jar ..\..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\..\Lib\antlr-4.8-complete.jar -o . -package MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4

popd
popd

pause
