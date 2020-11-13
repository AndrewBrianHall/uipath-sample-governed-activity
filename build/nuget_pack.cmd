CALL set_envs.cmd
SET _configuration=Debug
IF /I "%1" == "-release" SET _configuration=Release

SET _build_dir=%_src_root%\bin\%_configuration%

msbuild %_src_root%\%_PROJECT_NAME%.csproj -p:Configuration=%_configuration%
nuget pack %_src_root% -OutputDirectory %_build_dir% -p Configuration=%_configuration%