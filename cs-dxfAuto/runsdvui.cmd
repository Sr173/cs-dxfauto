cd /d "C:\Users\admin\source\repos\cs-dxfAuto\cs-dxfAuto" &msbuild "cs-dxfAuto.csproj" /t:sdvViewer /p:configuration="Release" /p:platform=Any CPU
exit %errorlevel% 