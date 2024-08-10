color 3
dotnet new -i Underdog.Echo.Template

set /p OP=Please set your project name(for example:Simple.Echo):

md .1YourProject

cd .1YourProject

dotnet new underdogecho -n %OP%

cd ../


echo "Create Successfully!!!! ^ please see the folder .1YourProject"

dotnet new uninstall Underdog.Echo.Template


echo "Delete Template Successfully"

pause
