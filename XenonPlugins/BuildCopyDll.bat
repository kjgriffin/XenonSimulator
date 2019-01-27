@ECHO ON
SET SourceDir="bin\Debug\netstandard2.0"
SET CopyDir="D:\\Main\\XenonSim\\Plugins"
SET Pattern="*.dll"
FOR %%A IN ("%SourceDir%\%Pattern%") DO ECHO F | XCOPY /Y /F "%%~A" "%CopyDir%\"
EXIT 0