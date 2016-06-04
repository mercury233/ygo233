#NoTrayIcon
#NoEnv
#Include <JSON_FromObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
#Include <JSON_ToObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
#Include <aria2> ;https://autohotkey.com/boards/viewtopic.php?t=4506
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
FileEncoding, UTF-8-RAW

WinTitle=YGOPRO 233服

FileRead, localconfig, ygo233\config.json
localconfig:=JSON_ToObj(localconfig)

IfNotExist, % localconfig.ygopro_exe
{
	IfExist, ygopro_vs.exe
	{
		localconfig.ygopro_exe:="ygopro_vs.exe"
		newlocalconfig:=JSON_FromObj(localconfig)
		FileDelete, ygo233\config.json
		FileAppend, % newlocalconfig, *ygo233\config.json
	}
	else
	{
		MsgBox, 16, % WinTitle, 未找到YGOPRO主程序。
		ExitApp
	}
}

gui, add, button, w200 h30 gRunYGOPRO, 启动游戏
gui, show,, % WinTitle

run, ygo233\update.exe, ygo233

return

RunYGOPRO:
run, % localconfig.ygopro_exe

GuiClose:
ExitApp
return

