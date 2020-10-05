CALL set_envs.cmd
CALL nuget_pack.cmd

msbuild %_src_root%\%_PROJECT_NAME%.csproj -p:Configuration=Debug

@echo on
IF /I "%1" == "-dll" (
    copy /Y %_build_dir%\%_PROJECT_NAME%.dll "%USERPROFILE%\.nuget\packages\%_PROJECT_NAME%\1.0.0\lib\net461"
)
copy /Y %_build_dir%\%_PROJECT_NAME%.1.0.0.nupkg "C:\NuGet"