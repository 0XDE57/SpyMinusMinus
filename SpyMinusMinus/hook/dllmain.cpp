//docs.microsoft.com/en-us/windows/desktop/winmsg/using-hooks

// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>

#include <stdio.h>
#include <stdlib.h>

//TODO:
//does releasing a hook leave a dll instance loaded in the target?



static HHOOK hhook;

static const UINT WM_HOOKWNDPROC = RegisterWindowMessage(L"WM_HOOKWNDPROC");

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:

		//MessageBox(nullptr, L"test", L"test", MB_OK);
		
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		//FreeConsole();
		break;
	}
	return TRUE;
}



LRESULT WINAPI SubclassWndProc(int nCode, WPARAM wParam, LPARAM lParam)
{


	if (nCode < 0)  // do not process message 
		return CallNextHookEx(hhook, nCode, wParam, lParam);


	

	printf("test");
	printf("%s", nCode);

	switch (nCode)
	{
	/*
	case WM_HOOKWNDPROC://static const != constant value?
		AllocConsole();
		AttachConsole(GetCurrentProcessId());
		freopen_s(nullptr, "CON", "w", stdout);
		break;
		*/
	case WM_NCDESTROY: 
		UnhookWindowsHookEx(hhook);
		FreeConsole();
		//unload ? 
		break;
	default:
		//SendNotifyMessage(hwndListener, nCode, wParam, lParam); 
		//SendMessage = blocking
		//SendMessageTimout = blocking until timeout?
		//PostMessage
		break;
	}

	
	

	return CallNextHookEx(nullptr/*hhook is ignored.*/, nCode, wParam, lParam);
}

EXTERN_C __declspec(dllexport) int Sum(int a, int b)
{
	//test 
	return a + b;
}

EXTERN_C __declspec(dllexport) int Hook(HWND hwndTarget, HWND hwndListener)
{

	//SetWinEventHook?

	//WH_GETMESSAGE?
	//WH_MSGFILTER?
	//WH_CALLWNDPROCRET?

	//Method: SetWindowsHookEx()
	hhook = SetWindowsHookEx(WH_CALLWNDPROC, SubclassWndProc, (HINSTANCE)NULL, GetCurrentThreadId());
	//SendMessage(WM_HOOKWNDPROC)




	printf("HOOK!");
	return 0;
}
