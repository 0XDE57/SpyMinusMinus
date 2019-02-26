//docs.microsoft.com/en-us/windows/desktop/winmsg/using-hooks

// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>

#include <stdio.h>
#include <stdlib.h>
#include <iostream>
//TODO:
//does releasing a hook leave a dll instance loaded in the target?

static bool consoleAttached = false;

static const UINT WM_HOOKWNDPROC = RegisterWindowMessage(L"WM_HOOKWNDPROC");
static const UINT WM_ALLOCCONSOLE = RegisterWindowMessage(L"WM_ALLOCCONSOLE");

//#pragma data_seg(".shared")
HHOOK hhook;
HWND hwndListener;
//#pragma data_seg()

HINSTANCE dllInstance;

void CreateConsole() {
	if (!consoleAttached && AllocConsole()) {
		//AttachConsole(GetCurrentProcessId());
		freopen_s((FILE **)stdout, "CONOUT$", "w", stdout);
		consoleAttached = true;
	}
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {	
	switch (ul_reason_for_call)	{
		case DLL_PROCESS_ATTACH:
			dllInstance = hModule;

			CreateConsole();
			printf_s("DLL_PROCESS_ATTACH: %x \n", dllInstance);
			break;
		case DLL_THREAD_ATTACH:
			printf_s("DLL_THREAD_ATTACH\n");
			break;
		case DLL_THREAD_DETACH:
			printf_s("DLL_THREAD_DETACH\n");
			break;
		case DLL_PROCESS_DETACH:
			printf_s("DLL_PROCESS_DETACH\n");
			FreeConsole();
			break;
	}
	return TRUE;
}



LRESULT WINAPI HookWndProc(int nCode, WPARAM wParam, LPARAM lParam) {	

	if (nCode < 0)  // do not process message 
		return CallNextHookEx(NULL, nCode, wParam, lParam);


	CWPSTRUCT* cwp = (CWPSTRUCT *)lParam;
	//printf_s("n: %i, w:%u, l:i% \n", nCode, wParam, lParam);
	//if (consoleAttached) {
		printf_s("h:%i - m:%i - w:%i - l:%i\n", cwp->hwnd, cwp->message, cwp->wParam, cwp->lParam);
	//}

	//if (cwp->message == WM_ALLOCCONSOLE) { CreateConsole(); }
	switch (cwp->message) {
		case WM_SIZING:
			MessageBox(cwp->hwnd, L"test", L"123", MB_OK);
			break;
	}
	/*
	switch (nCode)
	{
	/*
	case WM_HOOKWNDPROC://static const != constant value?
		AllocConsole();
		AttachConsole(GetCurrentProcessId());
		freopen_s(nullptr, "CON", "w", stdout);
		break;
		*
	case WM_NCDESTROY: 
		//UnhookWindowsHookEx(hhook);
		//FreeConsole();
		//unload ? 
		break;
	default:
		//SendNotifyMessage(hwndListener, nCode, wParam, lParam); 
		//SendMessage = blocking
		//SendMessageTimout = blocking until timeout?
		//PostMessage
		break;
	}*/

	if (cwp->message == WM_HOOKWNDPROC) {
		MessageBox(cwp->hwnd, L"test", L"123", MB_OK);
	}

	
	return CallNextHookEx(NULL, nCode, wParam, lParam);
}


EXTERN_C __declspec(dllexport) int Hook(HWND hwndTarget, HWND hwndListener) {

	//SetWinEventHook?
	//CBT hook?

	//WH_GETMESSAGE?
	//WH_MSGFILTER?
	//WH_CALLWNDPROCRET?

	//Method: SetWindowsHookEx()
	SetLastError(0);
	hhook = SetWindowsHookEx(WH_CALLWNDPROC, HookWndProc, dllInstance, GetWindowThreadProcessId(hwndTarget, NULL));
	//hhook = SetWindowsHookEx(WH_CALLWNDPROC, HookWndProc, dllInstance, 0);
	//hhook = SetWindowsHookEx(WH_CALLWNDPROC, HookWndProc, dllInstance, GetCurrentThreadId());
	int error = GetLastError();
	if (error) {
		std::cout << "could not hook: " << error;
		return error;
	}

	//LRESULT test = SendMessage(hwndTarget, WM_ALLOCCONSOLE, 0, 0);

	LRESULT success = SendMessage(hwndTarget, WM_HOOKWNDPROC, WPARAM(hhook), LPARAM(hwndListener));
	return success;
	//CreateConsole();


	//std::cout << "target: " << hwndTarget << " - listener: " << hwndListener << std::endl;
	//printf_s("handle: %p", hwndTarget);
	//return 0;
}
