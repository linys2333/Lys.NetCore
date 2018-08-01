dotnet restore
rem dotnet publish -c Release -f netcoreapp2.1 -r ubuntu.16.04-x64 -o ./Publish
dotnet publish -c Release -f netcoreapp2.1 -r centos.7-x64 -o ./Publish
pause