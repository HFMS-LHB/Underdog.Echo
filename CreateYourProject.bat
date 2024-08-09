color 3
dotnet new -i Underdog.Main.Template

set /p OP=Please set your project name(for example:SimpleMain):

md .1YourProject

cd .1YourProject

dotnet new underdogmain -n %OP%

cd ../


echo "Create Successfully!!!! ^ please see the folder .1YourProject"

dotnet new uninstall Underdog.Main.Template


echo "Delete Template Successfully"

pause
