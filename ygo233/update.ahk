;-----------------------------------------------------------
; Updater for ygo233
; LICENSE: AGPL
; https://github.com/mercury233/ygo233
;-----------------------------------------------------------

;-----------------------------------------------------------
; 基本设置
;-----------------------------------------------------------
#NoTrayIcon
#NoEnv
#SingleInstance Ignore
#Include <aria2> ;https://autohotkey.com/boards/viewtopic.php?t=4506
#Include <JSON> ;https://github.com/cocobelgica/AutoHotkey-JSON
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
SetControlDelay, -1
FileEncoding, UTF-8-RAW
;OnExit, ExitSub

if 0 >= 1
{
	param1 = %1%
	if (param1=="Background")
	{
		BackgroundCheckMode:=1
	}
	if (param1=="Pre")
	{
		OnlyPreMode:=1
	}
	if (param1=="Full")
	{
		FullCheckMode:=1
	}
}
ErrorCount:=0
Version:="0.0.4"

;-----------------------------------------------------------
; 初始化
;-----------------------------------------------------------
; 删除临时文件
FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, pre.json
FileDelete, packages.json
FileDelete, ygo233.json
FileDelete, update.bak
FileRemoveDir, downloads, 1

; 建立GUI
Gui, Add, Text, x6 y6 w290 vstatus, 喵喵喵
Gui, -SysMenu

WinTitle=YGO233 自动更新

; 读取本地配置文件
FileRead, localconfig, config.json
localconfig:=JSON.Load(localconfig)

aria2.url := "http://localhost:" . localconfig.aria2_port . "/jsonrpc"
aria2.token := "ygo233"

DebugLog( "aria2.url: " . aria2.url )

;-----------------------------------------------------------
; 开始检查更新
;-----------------------------------------------------------

if (BackgroundCheckMode)
{
	;ToolTip, 正在检查更新...
}
else
{
	GuiControl,, status, 正在检查更新...
	Gui, Show, w300 h25, % WinTitle
}

; 从配置文件里定义的更新URL读取版本信息
URLDownloadToFile, % localconfig.update_url, version.json
if (ErrorLevel)
{
	Gui, +OwnDialogs
	MsgBox, 16, % WinTitle, 检查更新失败！
	gosub, ExitSub
}
FileRead, newconfig, version.json
newconfig:=JSON.Load(newconfig)

DebugLog( "newconfig.version: " . newconfig.version )

if (localconfig.ygo233_version!=newconfig.ygo233_version)
{
	ToolTip
	Process, Close, 启动游戏.exe
	GuiControl,, status, 正在更新YGOPRO 233服 客户端...
	Gui, Show, w300 h25, % WinTitle
	URLDownloadToFile, % newconfig.download_base . "update/ygo233_update.7z", ygo233_update.7z
	if (ErrorLevel)
	{
		Gui, +OwnDialogs
		MsgBox, 16, % WinTitle, 自动更新失败！
		gosub, ExitSub
	}
	FileMove, update.exe, update.bak, 1
	RunWait, 7zg.exe -aoa x ygo233\ygo233_update.7z, ..
	FileDelete, ygo233_update.7z
	localconfig.ygo233_version:=newconfig.ygo233_version
	newlocalconfig:=JSON.Dump(localconfig)
	FileDelete, config.json
	FileAppend, % newlocalconfig, *config.json
	Run, ..\启动游戏.exe
	gosub, ExitSub
}

if (localconfig.version!=newconfig.version && BackgroundCheckMode)
{
	;ToolTip
	MsgBox, 36, % WinTitle, % "发现新版本，是否更新？`n`n发布日期：" newconfig.date "`n`n更新内容：`n" newconfig.txt
	IfMsgBox, Yes
	{
		gosub, StartUpdate
	}
	else
	{
		gosub, ExitSub
	}
}
else if (localconfig.use_pre_release=="true" && localconfig.pre_version!=newconfig.pre_version && BackgroundCheckMode)
{
	;ToolTip
	MsgBox, 36, % WinTitle, % "发现先行卡新版本，是否更新？`n`n发布日期：" newconfig.pre_date "`n`n更新内容：`n" newconfig.pre_txt
	IfMsgBox, Yes
	{
		gosub, StartUpdate
	}
	else
	{
		gosub, ExitSub
	}
}
else if (BackgroundCheckMode)
{
	gosub, ExitSub
}
else
{
	gosub, StartUpdate
}

return

;-----------------------------------------------------------
; 开始更新
;-----------------------------------------------------------
StartUpdate:
GuiControl,, status, 正在获取更新信息...
Gui, Show, w300 h25, % WinTitle

; 下载文件列表和更新包列表并解压，获得files.json和packages.json
URLDownloadToFile, % newconfig.download_base . "update/data.7z", data.7z
if (ErrorLevel)
{
	Gui, +OwnDialogs
	MsgBox, 16, % WinTitle, 获取更新信息失败！
	gosub, ExitSub
}
RunWait, 7zg.exe -aoa x data.7z,, Hide

; 对比MD5效验文件
GuiControl,, status, 正在效验文件，请稍候...

files_download := {} ; 需要下载的文件列表
packages_download := [] ; 需要下载的更新列表
pack_size := 0
file_count := 0

if (!OnlyPreMode)
{
	FileRead, files_json, files.json
	files_json:=JSON.Load(files_json)

	if (CheckFile(localconfig.ygopro_exe, 0, files_json.ygopro_exe))
	{
		files_download["ygopro.exe"]:="ygopro.exe"
	}

	for i, file in files_json.files
	{
		if (CheckFile(file.name, file.size, file.hash))
		{
			files_download[file.name]:=file.name
		}
	}

	for folder, folder_files in files_json.folders
	{
		if (!FullCheckMode && folder == "pics\thumbnail")
		{
			continue
		}
		for j, file in folder_files
		{
			filename := folder . "\" . file.name
			if (CheckFile(filename, file.size, file.hash))
			{
				files_download[filename]:=filename
				if (!FullCheckMode && folder == "pics")
				{
					filename := "pics\thumbnail" . "\" . file.name
					files_download[filename]:=filename
				}
			}
		}
	}
}

if (localconfig.use_pre_release=="true")
{
	FileRead, pre_json, pre.json
	pre_json:=JSON.Load(pre_json)

	for i, file in pre_json.files
	{
		if (CheckFile(file.name, file.size, file.hash))
		{
			files_download[file.name]:=file.name
		}
	}

	for folder, folder_files in pre_json.folders
	{
		for j, file in folder_files
		{
			filename := folder . "\" . file.name
			if (CheckFile(filename, file.size, file.hash))
			{
				files_download[filename]:=filename
			}
		}
	}
}

; 检查每个更新包里存在多少个需要下载的文件
; 如果需要下载的文件数量大于三分之二，或者大于100，记录这个更新包为可用
; 并把这个更新包里的所有文件从需要下载文件列表里移除
; 得出需要下载更新包列表，和剩余的需要下载文件列表
FileRead, packages_json, packages.json
packages_json:=JSON.Load(packages_json)

for i, package in packages_json.packages
{
	count:=0
	for j, file in package.files
	{
		if (files_download.HasKey(file))
		{
			count++
		}
	}
	if ((count>=package.filecount*0.66 || count>=100))
	{
		packages_download.push(package.filename)
		pack_size += package.filesize
		file_count++
		for j, file in package.files
		{
			files_download.Delete(file)
		}
		
		DebugLog( "use pack: " . package.filename )
	}
}

; 开始下载

for each, file in files_download
{
	file_count++
	DebugLog( "download file: " . file )
}

if (file_count==0)
{
	GuiControl,, status, 已是最新版本！
	gosub, SaveConfig
	Sleep, 500
	gosub, ExitSub
}

if (file_count>=200 || pack_size>=20*1024*1024) {
	Gui, +OwnDialogs
	MsgBox, 49, % WinTitle, 您需要更新的文件过多，请重新下载完整版！`n`n按确定打开YGOPRO 233服官网下载页面。
	IfMsgBox, OK
	{
		run, http://mercury233.me/ygosrv233/download.html?from=autoupdate
	}
	gosub, ExitSub
}

GuiControl,, status, 正在开始下载...

; 关闭残留的aria2c，并重新运行
Process, Close, aria2c.exe
Run, aria2c.exe --conf-path=aria2.conf,, Hide UseErrorLevel, aria2cPID
if (A_OSVersion=="WIN_XP")
{
	Sleep, 1000
	Process, Exist, aria2c.exe
	if (ErrorLevel<>aria2cPID)
	{
		MsgBox, 17, % WinTitle, 您的XP操作系统早已在两年前被淘汰了，无法正常使用新特性！`n`n按确定关闭程序，或者按取消尝试使用旧版。
		IfMsgBox, OK
		{
			run, http://windows.microsoft.com/zh-CN/windows/end-support-help
			gosub, ExitSub
		}
		else
		{
			run, http://windows.microsoft.com/zh-CN/windows/end-support-help
			GuiControl,, status, 正在下载旧版aria2...
			URLDownloadToFile, % newconfig.download_base . "xp/aria2c.7z", aria2c.7z
			if (ErrorLevel)
			{
				Gui, +OwnDialogs
				MsgBox, 16, % WinTitle, 下载aria2失败！
				gosub, ExitSub
			}
			RunWait, 7zg.exe -aoa x aria2c.7z
			FileDelete, aria2c.7z
			Run, aria2c.exe --conf-path=aria2.conf,, Hide UseErrorLevel, aria2cPID
			GuiControl,, status, 正在开始下载...
		}
	}
}
if (ErrorLevel=="ERROR")
{
	MsgBox, 16, % WinTitle, aria2下载引擎启动失败！
	DebugLog( "fail to run aria2c!" )
	gosub, ExitSub
}

download_count:=0

for each, filename in packages_download
{
	url := newconfig.download_base . "update/" . filename
	ret := aria2.addUri( [url] , {dir: "downloads\packages"})
	DebugLog( "aria2 addUri: " . url . " " . JSON.Dump(ret) )
	download_count++
}

for each, filename in files_download
{
	SplitPath, filename,, filedir
	url := newconfig.download_base . "ygopro/" . StrReplace(filename, "\", "/")
	ret := aria2.addUri( [url] , {dir: "downloads\" . filedir})
	DebugLog( "aria2 addUri: " . url . " " . JSON.Dump(ret) )
	download_count++
}

gosub, UpdateStatus
return

;-----------------------------------------------------------
; 更新进度
;-----------------------------------------------------------
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
	DebugLog( "aria2 shutdown: " . JSON.Dump(ret) )
	gosub, InstallUpdate
}
return

;-----------------------------------------------------------
; 安装更新
;-----------------------------------------------------------
InstallUpdate:
GuiControl,, status, 正在安装更新...

Process, Exist, % localconfig.ygopro_exe
PID1:=ErrorLevel
Process, Exist, ygopro.exe
PID2:=ErrorLevel

if (PID1 || PID2)
{
	MsgBox, 52, % WinTitle, 检查到YGOPRO正在运行。`n安装更新前请关闭YGOPRO。`n`n是否自动关闭？
	IfMsgBox, Yes
	{
		GroupAdd, YGOPRO, ahk_exe ygopro.exe
		GroupAdd, YGOPRO, % "ahk_exe" . localconfig.ygopro_exe
		WinClose, ahk_group YGOPRO
	}
}

Loop, Files, downloads\packages\*.7z
{
	GuiControl,, status, 正在安装更新%A_LoopFileName%...
	RunWait, % "7zg.exe -aoa x """ . A_LoopFileLongPath . """", .., UseErrorLevel
	DebugLog( "extract: " . ErrorLevel . " " . A_LoopFileLongPath )
	if (ErrorLevel<>0)
	{
		MsgBox, 48, % WinTitle, %A_LoopFileName%解压失败，请重新尝试更新！
		ErrorCount++
	}
}

for each, filename in files_download
{
	GuiControl,, status, 正在更新%filename%...
	FileMove, downloads\%filename%, ..\%filename%, 1
	DebugLog( "filemove: " . ErrorLevel . " " . filename )
	if (ErrorLevel<>0)
	{
		ErrorCount++
	}
}

if (localconfig.ygopro_exe != "ygopro.exe")
{
	FileCopy, ..\ygopro.exe, % "..\" . localconfig.ygopro_exe, 1
	DebugLog( "copy ygopro.exe: " . ErrorLevel . " " . localconfig.ygopro_exe )
	if (ErrorLevel<>0)
	{
		ErrorCount++
	}
}

if (ErrorCount)
{
	GuiControl,, status, 更新过程中出现问题，请重新尝试更新！
	
	Sleep, 2000
	gosub, ExitSub
}
else
{
	GuiControl,, status, 更新完成！
	gosub, SaveConfig
	Sleep, 500
	gosub, ExitSub
}
return

;-----------------------------------------------------------
; 保存配置
;-----------------------------------------------------------
SaveConfig:
if (!OnlyPreMode)
{
	localconfig.version:=newconfig.version
}
if (localconfig.use_pre_release=="true")
{
	localconfig.pre_version:=newconfig.pre_version
}
newlocalconfig:=JSON.Dump(localconfig)
FileDelete, config.json
FileAppend, % newlocalconfig, *config.json
return

;-----------------------------------------------------------
; 退出
;-----------------------------------------------------------
GuiClose:
ExitSub:
FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, pre.json
FileDelete, packages.json
FileDelete, ygo233.json
FileRemoveDir, downloads, 1
Process, Close, % aria2cPID
WinGet, sn, ID, ahk_exe 启动游戏.exe
if (sn)
{
	PostMessage, 0x2333,,,, ahk_id %sn%
}
DebugLog( "exit" )
ExitApp
return

;-----------------------------------------------------------
; 函数
;-----------------------------------------------------------

; 记录日志
DebugLog(txt)
{
	global
	StringReplace, txt, txt, `r, , All
	StringReplace, txt, txt, `n, , All
	txt := "[" . A_Now . "] update.ahk v" . Version . " : " . txt . "`n"
	FileAppend, % txt, update.log
}

; 检查某个文件的MD5
CheckFile(relPath, newSize, newHash)
{
	global
	;GuiControl,, status, 正在效验文件%relPath%...
	if (!FullCheckMode && newSize)
	{
		GuiControl,, status, 正在快速效验文件%relPath%...
		FileGetSize, fileSize, % localconfig.base_path . relPath
		if (fileSize == newSize)
		{
			return 0
		}
	}
	else
	{
		GuiControl,, status, 正在效验文件%relPath%...
		fileHash := HashFile(localconfig.base_path . relPath)
		StringLeft, fileHash, fileHash, 8
		StringLower, fileHash, fileHash
		if (fileHash == newHash)
		{
			return 0
		}
	}
	;DebugLog( "file diff: " . relPath )
	return 1
}

; 将字节转换为KB/MB等
FormatByteSize(Bytes){ ; by HotKeyIt, http://ahkscript.org/boards/viewtopic.php?p=18338#p18338
  static size:="B,KB,MB,GB,TB,PB,EB,ZB,YB"
  Loop,Parse,size,`,
    If (bytes>999)
      bytes:=bytes/1024
    else {
      bytes:=Trim(SubStr(bytes,1,4),".") " " A_LoopField
      break
    }
  return bytes
}

; MD5
HashFile(filePath, hashType=2) ; By Deo, http://www.autohotkey.com/forum/viewtopic.php?t=71133
{
   PROV_RSA_AES := 24
   CRYPT_VERIFYCONTEXT := 0xF0000000
   BUFF_SIZE := 1024 * 1024 ; 1 MB
   HP_HASHVAL := 0x0002
   HP_HASHSIZE := 0x0004
   
   HASH_ALG := (hashType = 1 OR hashType = "MD2") ? (CALG_MD2 := 32769) : HASH_ALG
   HASH_ALG := (hashType = 2 OR hashType = "MD5") ? (CALG_MD5 := 32771) : HASH_ALG
   HASH_ALG := (hashType = 3 OR hashType = "SHA") ? (CALG_SHA := 32772) : HASH_ALG
   HASH_ALG := (hashType = 4 OR hashType = "SHA256") ? (CALG_SHA_256 := 32780) : HASH_ALG   ;Vista+ only
   HASH_ALG := (hashType = 5 OR hashType = "SHA384") ? (CALG_SHA_384 := 32781) : HASH_ALG   ;Vista+ only
   HASH_ALG := (hashType = 6 OR hashType = "SHA512") ? (CALG_SHA_512 := 32782) : HASH_ALG   ;Vista+ only
   
   f := FileOpen(filePath,"r","CP0")
   if !IsObject(f)
      return 0
   if !hModule := DllCall( "GetModuleHandleW", "str", "Advapi32.dll", "Ptr" )
      hModule := DllCall( "LoadLibraryW", "str", "Advapi32.dll", "Ptr" )
   if !dllCall("Advapi32\CryptAcquireContextW"
            ,"Ptr*",hCryptProv
            ,"Uint",0
            ,"Uint",0
            ,"Uint",PROV_RSA_AES
            ,"UInt",CRYPT_VERIFYCONTEXT )
      Gosub, HashTypeFreeHandles
   
   if !dllCall("Advapi32\CryptCreateHash"
            ,"Ptr",hCryptProv
            ,"Uint",HASH_ALG
            ,"Uint",0
            ,"Uint",0
            ,"Ptr*",hHash )
      Gosub, HashTypeFreeHandles
   
   VarSetCapacity(read_buf,BUFF_SIZE,0)
   
   hCryptHashData := DllCall("GetProcAddress", "Ptr", hModule, "AStr", "CryptHashData", "Ptr")
   While (cbCount := f.RawRead(read_buf, BUFF_SIZE))
   {
      if (cbCount = 0)
         break
      
      if !dllCall(hCryptHashData
               ,"Ptr",hHash
               ,"Ptr",&read_buf
               ,"Uint",cbCount
               ,"Uint",0 )
         Gosub, HashTypeFreeHandles
   }
   
   if !dllCall("Advapi32\CryptGetHashParam"
            ,"Ptr",hHash
            ,"Uint",HP_HASHSIZE
            ,"Uint*",HashLen
            ,"Uint*",HashLenSize := 4
            ,"UInt",0 ) 
      Gosub, HashTypeFreeHandles
      
   VarSetCapacity(pbHash,HashLen,0)
   if !dllCall("Advapi32\CryptGetHashParam"
            ,"Ptr",hHash
            ,"Uint",HP_HASHVAL
            ,"Ptr",&pbHash
            ,"Uint*",HashLen
            ,"UInt",0 )
      Gosub, HashTypeFreeHandles   
   
   SetFormat,integer,Hex
   loop,%HashLen%
   {
      num := numget(pbHash,A_index-1,"UChar")
      hashval .= substr((num >> 4),0) . substr((num & 0xf),0)
   }
   SetFormat,integer,D
      
HashTypeFreeHandles:
   f.Close()
   DllCall("FreeLibrary", "Ptr", hModule)
   dllCall("Advapi32\CryptDestroyHash","Ptr",hHash)
   dllCall("Advapi32\CryptReleaseContext","Ptr",hCryptProv,"UInt",0)
   return hashval
}