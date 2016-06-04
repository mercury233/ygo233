#NoTrayIcon
#NoEnv
#Include <aria2> ;https://autohotkey.com/boards/viewtopic.php?t=4506
#Include <JSON> ;https://github.com/cocobelgica/AutoHotkey-JSON
;#Include <Z_Down> ;https://autohotkey.com/boards/viewtopic.php?t=3299
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
SetControlDelay, -1
FileEncoding, UTF-8-RAW
OnExit, ExitSub

FileDelete, version.json
FileDelete, data.7z
FileDelete, files.json
FileDelete, packages.json
;FileDelete, download.json
FileRemoveDir, downloads, 1

Process, Close, aria2c.exe
run, aria2c.exe --conf-path=aria2.conf,, hide, aria2cPID

Gui, Add, Text, x6 y6 w290 vstatus, 喵喵喵
Gui, -SysMenu

WinTitle=YGOPRO 233服 自动更新

FileRead, localconfig, config.json
localconfig:=JSON.Load(localconfig)

aria2.url := "http://localhost:" . localconfig.aria2_port . "/jsonrpc"
aria2.token := "ygo233"

ToolTip, 正在检查更新...

URLDownloadToFile, % localconfig.update_url, version.json
FileRead, newconfig, version.json
newconfig:=JSON.Load(newconfig)

if (localconfig.version!=newconfig.version)
{
	ToolTip
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
	ToolTip, 没有发现新版本
	Sleep, 2000
	gosub, ExitSub
}

return

StartUpdate:
GuiControl,, status, 正在获取更新信息...
Gui, Show, w300 h25, % WinTitle

URLDownloadToFile, % newconfig.download_base . "update/data.7z", data.7z

RunWait, 7zg.exe x data.7z,, Hide

GuiControl,, status, 正在效验文件，请稍候...

files_download := {}
packages_download := []
pack_size := 0

FileRead, files_json, files.json
files_json:=JSON.load(files_json)

CheckFile(localconfig.ygopro_exe, files_json.ygopro_exe)

for i, file in files_json.files
{
	CheckFile(file.name, file.hash)
}

for folder, folder_files in files_json.folders
{
	for j, file in folder_files
	{
		CheckFile(folder . "\" . file.name, file.hash)
	}
}

FileRead, packages_json, packages.json
packages_json:=JSON.load(packages_json)

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
		for j, file in package.files
		{
			files_download.Delete(file)
		}
	}
}

if (files_download.Length()>=200 || pack_size>=20*1024*1024) {
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

for each, filename in packages_download
{
	url := newconfig.download_base . "update/" . filename
	ret := aria2.addUri( [url] , {dir: "downloads\packages"})
	download_count++
}

for each, filename in files_download
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

for each, filename in files_download
{
	FileMove, downloads\%filename%, ..\%filename%, 1
}

if (localconfig.ygopro_exe != "ygopro.exe")
{
	FileCopy, ..\ygopro.exe, % "..\" . localconfig.ygopro_exe, 1
}

GuiControl,, status, 更新完成！

localconfig.version:=newconfig.version
newlocalconfig:=JSON.Dump(localconfig)
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
;FileDelete, download.json
FileRemoveDir, downloads, 1
Process, Close, % aria2cPID
ExitApp
return

;--------------------------------------------
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

CheckFile(relPath, newHash)
{
	global
	GuiControl,, status, 正在效验文件%relPath%...
	fileHash := HashFile(localconfig.base_path . relPath)
	StringLeft, fileHash, fileHash, 8
	StringLower, fileHash, fileHash
	if (fileHash <> newHash)
	{
		files_download[relPath]:=relPath
	}
}

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