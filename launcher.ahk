#NoTrayIcon
#NoEnv
#SingleInstance Ignore
#Include <JSON> ;https://github.com/cocobelgica/AutoHotkey-JSON
SetWorkingDir, %A_ScriptDir%
SetBatchLines, -1
FileEncoding, UTF-8-RAW

If A_IsCompiled
Menu, Tray, Icon, %A_ScriptFullPath%
Else
Menu, Tray, Icon, ygo233\Tyr.ico

WinTitle=YGO233
Version:="0.0.4"


FileRead, localconfig, ygo233\config.json
localconfig:=JSON.Load(localconfig)

IfNotExist, % localconfig.ygopro_exe
{
	IfExist, ygopro_vs.exe
	{
		localconfig.ygopro_exe:="ygopro_vs.exe"
		gosub, SaveConfig
	}
	else IfExist, ygopro.exe
	{
		localconfig.ygopro_exe:="ygopro.exe"
		gosub, SaveConfig
	}
	else
	{
		MsgBox, 16, % WinTitle, 未找到YGOPRO主程序，请把本软件解压到YGOPRO文件夹中运行。
		ExitApp
	}
}

gui, add, pic, w200 h200 x10 y10 vIconYGO233 gClickIcon, % A_ScriptFullPath
gui, add, button, w160 h30 x10 y220 vButtonStart gRunYGOPRO, 启动游戏
gui, add, button, w30 h30 x180 y220 gGUI_Set_Show hwndIconSet, 
GuiButtonIcon(IconSet, A_ScriptFullPath, 2, "S20")
gui, show,, % WinTitle

if (localconfig.dont_auto_update <> "true")
{
	GuiControl, , ButtonStart, 正在检查更新...
	GuiControl, Disable, ButtonStart
	Run, ygo233\update.exe Background, ygo233
}
else if (localconfig.auto_run_ygopro == "true")
{
	gosub, RunYGOPRO
}

IDC_HAND := DllCall("LoadCursor", "UInt", NULL,"Int", 32649, "UInt")
OnMessage(0x200, "WM_MOUSEMOVE")
WM_MOUSEMOVE(wParam, lParam)
{
	global
	if (A_GuiControl == "IconYGO233")
	{
	  	DllCall("SetCursor", "UInt", IDC_HAND)
	}
}

OnMessage(0x2333, "CheckFinish")
CheckFinish()
{
	global
	GuiControl, , ButtonStart, 启动游戏
	GuiControl, Enable, ButtonStart
	if (localconfig.auto_run_ygopro == "true")
	{
		gosub, RunYGOPRO
	}
}

return

RunYGOPRO:
run, % localconfig.ygopro_exe
if (localconfig.auto_close_ygo233 == "true")
{
	ExitApp
}
return

ClickIcon:
run, http://mercury233.me/ygosrv233/?from=ygo233
return

GuiClose:
DllCall("DestroyCursor", "Uint", IDC_HAND)
ExitApp
return

SaveConfig:
newlocalconfig:=JSON.Dump(localconfig)
FileDelete, ygo233\config.json
FileAppend, % newlocalconfig, *ygo233\config.json
return

GUI_Set_Show:
IfWinExist, %WinTitle% 设置 ahk_class AutoHotkeyGUI
{
	WinActivate
	Return
}

Gui 2: +LabelGUI_Set_
Gui 2: Default
Gui, +Owner1
;Gui, 1:+Disabled

Gui, Add, GroupBox, x10 y10 w220 h100, 设置

Gui, Add, CheckBox, xp+10 yp+20 w200 h20 -Wrap vGUI_Set_Dont_Auto_Update gGUI_Set_Dont_Auto_Update_Click, 不自动检查更新
Gui, Add, CheckBox, xp+0 yp+25 w200 h20 -Wrap vGUI_Set_Auto_Run_YGOPRO gGUI_Set_Auto_Run_YGOPRO_Click, 自动启动游戏
Gui, Add, CheckBox, xp+0 yp+25 w200 h20 -Wrap vGUI_Set_Auto_Close_YGO233 gGUI_Set_Auto_Close_YGO233_Click, 启动游戏后关闭启动器

Gui, Add, GroupBox, x10 y120 w220 h265, 功能(更多功能会在以后的版本开放)

Gui, Add, Button, xp+10 yp+20 w95 h25 gGUI_Set_Enable_Pre, 启用先行卡
Gui, Add, Button, xp+105 yp+0 w95 h25 gGUI_Set_Remove_Pre, 删除先行卡
Gui, Add, Button, xp-105 yp+35 w200 h25 gGUI_Set_CheckUpdate, 立即自动更新
Gui, Add, Button, xp+0 yp+35 w200 h25 gGUI_Set_CheckUpdate_Full, 重新效验文件
Gui, Add, Button, xp+0 yp+35 w200 h25 gGUI_Set_OK Disabled, 设置双击打开卡组文件
Gui, Add, Button, xp+0 yp+35 w200 h25 gGUI_Set_OK Disabled, 清理过期的卡图和脚本
Gui, Add, Button, xp+0 yp+35 w95 h25 gGUI_Set_OK Disabled, 修复游戏字体
Gui, Add, Button, xp+105 yp+0 w95 h25 gGUI_Set_OK Disabled, 设置游戏背景
Gui, Add, Button, xp-105 yp+35 w95 h25 gGUI_About_Show, 关于YGO233
Gui, Add, Button, xp+105 yp+0 w95 h25 gGUI_Set_OK Disabled, 捐助

;Gui, Add, Button, x155 y370 w75 h25 gGUI_Set_OK ,确定
;Gui, Add, Button, x235 y370 w75 h25 gGUI_Set_Apply ,应用

if (localconfig.use_pre_release <> "false")
{
	GuiControl, Disable, 启用先行卡
}
else
{
	GuiControl, Disable, 删除先行卡
}
if (localconfig.dont_auto_update == "true")
{
	GuiControl, , GUI_Set_Dont_Auto_Update, 1
}
if (localconfig.auto_run_ygopro == "true")
{
	GuiControl, , GUI_Set_Auto_Run_YGOPRO, 1
	GuiControl, +Disabled, GUI_Set_Auto_Close_YGO233
}
if (localconfig.auto_close_ygo233 == "true")
{
	GuiControl, , GUI_Set_Auto_Close_YGO233, 1
}

Gui, Show,, %WinTitle% 设置

return

GUI_Set_OK:
GUI_Set_Apply:
GUI_Set_Cancel:
return

GUI_Set_Dont_Auto_Update_Click:
Gui, Submit, NoHide
if (GUI_Set_Dont_Auto_Update)
{
	localconfig.dont_auto_update:="true"
}
else
{
	localconfig.dont_auto_update:="false"
}
gosub, SaveConfig
return

GUI_Set_Auto_Run_YGOPRO_Click:
Gui, Submit, NoHide
if (GUI_Set_Auto_Run_YGOPRO)
{
	localconfig.auto_run_ygopro:="true"
	localconfig.auto_close_ygo233:="false"
	GuiControl, , GUI_Set_Auto_Close_YGO233, 0
	GuiControl, +Disabled, GUI_Set_Auto_Close_YGO233
}
else
{
	localconfig.auto_run_ygopro:="false"
	GuiControl, -Disabled, GUI_Set_Auto_Close_YGO233
}
gosub, SaveConfig
return

GUI_Set_Auto_Close_YGO233_Click:
Gui, Submit, NoHide
if (GUI_Set_Auto_Close_YGO233)
{
	localconfig.auto_close_ygo233:="true"
}
else
{
	localconfig.auto_close_ygo233:="false"
}
gosub, SaveConfig
return

GUI_Set_Enable_Pre:
Gui, +OwnDialogs
GuiControl, Disable, 启用先行卡
localconfig.use_pre_release:="true"
gosub, SaveConfig
MsgBox, 64, % WinTitle, 已启用先行卡，将立即为你下载。`n`n请在游戏时将端口由233改成23333，才能使用先行卡。
GuiControl, Enable, 删除先行卡
Run, ygo233\update.exe Pre, ygo233
return

GUI_Set_Remove_Pre:
Gui, +OwnDialogs
GuiControl, Disable, 删除先行卡
localconfig.use_pre_release:="false"
gosub, SaveConfig
FileDelete, expansions\pre-release.cdb
MsgBox, 64, % WinTitle, 已删除先行卡，你可能需要清理过期的卡图和脚本。
return

GUI_Set_CheckUpdate:
GuiControl, Disable, 立即自动更新
Run, ygo233\update.exe, ygo233
return

GUI_Set_CheckUpdate_Full:
GuiControl, Disable, 重新效验文件
Run, ygo233\update.exe Full, ygo233
return

GUI_Set_Close:
Gui, Destroy
;Gui, 1:-Disabled
Gui, 1:Default
Gui, 1:Show
return

GUI_About_Show:
IfWinExist, 关于 %WinTitle% ahk_class AutoHotkeyGUI
{
	WinActivate
	Return
}
Gui 3: +LabelGUI_About_
Gui 3: Default
Gui, +Owner2
;Gui, 2:+Disabled

Gui, Add, GroupBox, x10 y10 w300 h420, 关于 YGO233
Gui, Add, Link, xp+10 yp+20 w280 h12, <a href="http://mercury233.me/ygosrv233/?from=ygo233about">YGO233</a> %Version%
Gui, Add, Link, xp+0 yp+20 w280 h12, Copyright (C) 2016 <a href="http://mercury233.me/?from=ygo233about">mercury233</a>
Gui, Add, Link, xp+0 yp+20 w280 h12, YGO233以AGPL协议发布，<a href="https://github.com/mercury233/ygo233?from=ygo233about">源码</a>
Gui, Add, Text, xp+0 yp+30 w280 h12, YGOPRO
Gui, Add, Link, xp+0 yp+20 w280 h12, Copyright (C) 2016 <a href="https://github.com/Fluorohydride">Fluorohydride</a>
Gui, Add, Link, xp+0 yp+20 w280 h12, YGOPRO发布于<a href="http://ygocore.ys168.com/">ygocore.ys168.com</a>
Gui, Add, Text, xp+0 yp+40 w280 h12, 本软件中包含了Igor Pavlov开发的7-Zip的部分组件
Gui, Add, Link, xp+0 yp+20 w280 h12, Copyright (C) 1999-2016 Igor Pavlov
Gui, Add, Link, xp+0 yp+20 w280 h12, <a href="http://www.7-zip.org/">www.7-zip.org</a>
Gui, Add, Text, xp+0 yp+30 w280 h12, 本软件中包含了Tatsuhiro Tsujikawa开发的aria2
Gui, Add, Link, xp+0 yp+20 w280 h12, Copyright (C) 2016 Tatsuhiro Tsujikawa
Gui, Add, Link, xp+0 yp+20 w280 h12, <a href="https://aria2.github.io/">aria2.github.io</a>
Gui, Add, Text, xp+0 yp+40 w280 h30, YGO233相信，本软件对涉及“游戏王OCG”的`n图片、文字等内容的使用属于“合理使用”。
Gui, Font, S7
Gui, Add, Text, xp+0 yp+35 w280 h12, 遊戯王アーク・ファイブ オフィシャルカードゲーム
Gui, Add, Text, xp+0 yp+15 w280 h12, ©高橋和希　スタジオ・ダイス／集英社　企画・制作／KONAMI
Gui, Add, Text, xp+0 yp+15 w280 h12, ©高橋和希　スタジオ・ダイス／集英社・テレビ東京・NAS
Gui, Add, Text, xp+0 yp+15 w280 h12, ©高橋和希　スタジオ・ダイス／集英社
Gui, Font
Gui, Show,, 关于 %WinTitle%

return

GUI_About_Close:
Gui, Destroy
;Gui, 2:-Disabled
Gui, 2:Default
Gui, 2:Show
return


GuiButtonIcon(Handle, File, Index := 1, Options := "")
{
	RegExMatch(Options, "i)w\K\d+", W), (W="") ? W := 16 :
	RegExMatch(Options, "i)h\K\d+", H), (H="") ? H := 16 :
	RegExMatch(Options, "i)s\K\d+", S), S ? W := H := S :
	RegExMatch(Options, "i)l\K\d+", L), (L="") ? L := 0 :
	RegExMatch(Options, "i)t\K\d+", T), (T="") ? T := 0 :
	RegExMatch(Options, "i)r\K\d+", R), (R="") ? R := 0 :
	RegExMatch(Options, "i)b\K\d+", B), (B="") ? B := 0 :
	RegExMatch(Options, "i)a\K\d+", A), (A="") ? A := 4 :
	Psz := A_PtrSize = "" ? 4 : A_PtrSize, DW := "UInt", Ptr := A_PtrSize = "" ? DW : "Ptr"
	VarSetCapacity( button_il, 20 + Psz, 0 )
	NumPut( normal_il := DllCall( "ImageList_Create", DW, W, DW, H, DW, 0x21, DW, 1, DW, 1 ), button_il, 0, Ptr )	; Width & Height
	NumPut( L, button_il, 0 + Psz, DW )		; Left Margin
	NumPut( T, button_il, 4 + Psz, DW )		; Top Margin
	NumPut( R, button_il, 8 + Psz, DW )		; Right Margin
	NumPut( B, button_il, 12 + Psz, DW )	; Bottom Margin	
	NumPut( A, button_il, 16 + Psz, DW )	; Alignment
	SendMessage, BCM_SETIMAGELIST := 5634, 0, &button_il,, AHK_ID %Handle%
	return IL_Add( normal_il, File, Index )
}
