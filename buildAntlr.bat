pushd %~dp0

pushd Projects\Eir.MFSH\Parser2
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.Parser2 -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.Parser2 -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4
popd

pushd Projects\Eir.MFSH\PreParser
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.PreParser -no-listener -visitor -Dlanguage=CSharp MFSHPreLexer.g4
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.PreParser -no-listener -visitor -Dlanguage=CSharp MFSHPreParser.g4
popd

pushd Projects\Eir.MFSH\Parser
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHLexer.g4
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o .\Antlr -package MFSH.Parser -no-listener -visitor -Dlanguage=CSharp MFSHParser.g4

popd
popd

pause
