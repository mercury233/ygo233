#NoTrayIcon
#Include <JSON_FromObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
#Include <JSON_ToObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
SetControlDelay, -1
FileEncoding, UTF-8-RAW

FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, packages.json
FileDelete, download.json

WinTitle=YGOPRO 233服 自动更新

Gui, Add, Text, x6 y6 w290 vstatus, 喵喵喵
Gui, -SysMenu

FileRead, localconfig, config.json
localconfig:=JSON_ToObj(localconfig)

URLDownloadToFile, % localconfig.update_url, version.json
FileRead, newconfig, version.json
newconfig:=JSON_ToObj(newconfig)

if (localconfig.version!=newconfig.version)
{
	MsgBox, 36, % WinTitle, % "发现新版本，是否更新？`n`n发布日期：" newconfig.date "`n`n更新内容：" newconfig.txt
	IfMsgBox, Yes
	{
		gosub, DoUpdate
	}
	else
	{
		gosub, Exit
	}
}

return

DoUpdate:
GuiControl,, status, 正在获取更新信息...
Gui, Show, w300 h25, % WinTitle

URLDownloadToFile, % newconfig.download_base . "update/data.7z", data.7z

RunWait, 7zg.exe x data.7z,, Hide

GuiControl,, status, 正在效验文件，约需2分钟，请稍候...

RunWait, node.exe check.js,, Hide

FileRead, download_json, download.json
download_json:=JSON_ToObj(download_json)

if (download_json.files_count>=200 || download_json.pack_size>=20*1024*1024) {
	MsgBox, 49, % WinTitle, 您需要更新的文件过多，请重新下载完整版！`n`n按确定打开官网下载页面。
	IfMsgBox, OK
	{
		run, http://mercury233.me/ygosrv233/download.html?from=autoupdate
	}
	gosub, Exit
}

return


GuiClose:
Exit:
FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, packages.json
FileDelete, download.json
ExitApp
return
