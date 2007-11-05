@echo off

IF EXIST Interop.SHDocVw.dll GOTO step2
tlbimp %SystemRoot%\System32\shdocvw.dll /out:Interop.SHDocVw.dll

:step2
IF EXIST AxInterop.SHDocVw.dll GOTO step3
aximp  %SystemRoot%\System32\shdocvw.dll /rcw:Interop.SHDocVw.dll /out:AxInterop.SHDocVw.dll /source

:step3
