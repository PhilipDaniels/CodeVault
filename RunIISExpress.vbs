' Put this into your Send To folder to serve any folder
' via IIS Express.

' Init randomization
Randomize
 
' Set random port number
Dim port
port = Int(Rnd() * 8974) + 1025
 
' Launch IIS Express / Browser
Set WshShell = CreateObject("WScript.Shell")
WshShell.Run """%programfiles%\iis express\iisexpress"" /path:""" & WScript.Arguments.Item(0) & """ /port:" & CStr(port) & " /systray:true", 0, False
WshShell.Run "http://localhost:" & CStr(port), 9, False
Set WshShell = Nothing