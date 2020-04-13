pushd %~dp0
pushd Projects\FSHer.Antlr
java -jar ..\..\Lib\antlr-4.8-complete.jar -o . -package FSHer -visitor -listener -Dlanguage=CSharp FSH.g4
popd
popd

pause
