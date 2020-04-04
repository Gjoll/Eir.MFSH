pushd Projects\FSHpp\Antlr
java -jar ..\..\..\Lib\antlr-4.8-complete.jar -o . -package FSHpp -visitor -listener -Dlanguage=CSharp FSH.g4
popd

pause
