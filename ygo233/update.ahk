#NoTrayIcon
#NoEnv
#Include <JSON_FromObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
#Include <JSON_ToObj> ;https://github.com/Jim-VxE/AHK-Lib-JSON_ToObj
#Include <aria2> ;https://autohotkey.com/boards/viewtopic.php?t=4506
;#Include <Z_Down> ;https://autohotkey.com/boards/viewtopic.php?t=3299
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
FileEncoding, UTF-8-RAW
OnExit, ExitSub

FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, packages.json
FileDelete, download.json
FileRemoveDir, downloads, 1

Process, Close, aria2c.exe
run, aria2c.exe --conf-path=aria2.conf,, hide, aria2cPID

Gui, Add, Text, x6 y6 w290 vstatus, 喵喵喵
Gui, -SysMenu

WinTitle=YGOPRO 233服 自动更新

FileRead, localconfig, config.json
localconfig:=JSON_ToObj(localconfig)

aria2.url := "http://localhost:" . localconfig.aria2_port . "/jsonrpc"
aria2.token := "ygo233"

;newconfig:=Z_Down(localconfig.update_url)
URLDownloadToFile, % localconfig.update_url, version.json
FileRead, newconfig, version.json
newconfig:=JSON_ToObj(newconfig)

if (localconfig.version!=newconfig.version)
{
	MsgBox, 36, % WinTitle, % "发现新版本，是否更新？`n`n发布日期：" newconfig.date "`n`n更新内容：" newconfig.txt
	IfMsgBox, Yes
	{
		gosub, StartUpdate
	}
	else
	{
		gosub, ExitSub
	}
}
else
{
	gosub, ExitSub
}

return

StartUpdate:
GuiControl,, status, 正在获取更新信息...
Gui, Show, w300 h25, % WinTitle

URLDownloadToFile, % newconfig.download_base . "update/data.7z", data.7z

RunWait, 7zg.exe x data.7z,, Hide

GuiControl,, status, 正在效验文件，约需2分钟，请稍候...

;RunWait, node.exe check.js,, Hide
RunWait, check.exe,, Hide

FileRead, download_json, download.json
download_json:=JSON_ToObj(download_json)

if (download_json.files_count>=200 || download_json.pack_size>=20*1024*1024) {
	Gui, +OwnDialogs
	MsgBox, 49, % WinTitle, 您需要更新的文件过多，请重新下载完整版！`n`n按确定打开YGOPRO 233服官网下载页面。
	IfMsgBox, OK
	{
		run, http://mercury233.me/ygosrv233/download.html?from=autoupdate
	}
	gosub, ExitSub
}

GuiControl,, status, 正在开始下载...

download_count:=0

for each, filename in download_json.packages_download
{
	url := newconfig.download_base . "update/" . filename
	ret := aria2.addUri( [url] , {dir: "downloads\packages"})
	download_count++
}

for each, filename in download_json.files_download
{
	SplitPath, filename,, filedir
	url := newconfig.download_base . "ygopro/" . StrReplace(filename, "\", "/")
	ret := aria2.addUri( [url] , {dir: "downloads\" . filedir})
	download_count++
}

gosub, UpdateStatus
return

UpdateStatus:
ret := aria2.getGlobalStat()
if (ret.result.numStoppedTotal<download_count)
{
	GuiControl,, status, % "正在下载(" . ret.result.numStoppedTotal . "/" . download_count .  ")... " . FormatByteSize(ret.result.downloadSpeed) . "/S"
	SetTimer, UpdateStatus, -333
}
else
{
	ret := aria2.shutdown()
	gosub, InstallUpdate
}
return

InstallUpdate:
GuiControl,, status, 正在安装更新...

Loop, Files, downloads\packages\*.7z
{
	RunWait, % "7zg.exe -aoa x """ . A_LoopFileLongPath . """", ..
}

for each, filename in download_json.files_download
{
	FileMove, downloads\%filename%, ..\%filename%, 1
}

GuiControl,, status, 更新完成！

localconfig.version:=newconfig.version
newlocalconfig:=JSON_FromObj(localconfig)
FileDelete, config.json
FileAppend, % newlocalconfig, *config.json

Sleep, 500
gosub, ExitSub

return

GuiClose:
ExitSub:
FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, packages.json
FileDelete, download.json
FileRemoveDir, downloads, 1
Process, Close, % aria2cPID
ExitApp
return



FormatByteSize(Bytes){ ; by HotKeyIt, http://ahkscript.org/boards/viewtopic.php?p=18338#p18338
  static size:="b,KB,MB,GB,TB,PB,EB,ZB,YB"
  Loop,Parse,size,`,
    If (bytes>999)
      bytes:=bytes/1024
    else {
      bytes:=Trim(SubStr(bytes,1,4),".") " " A_LoopField
      break
    }
  return bytes
}
