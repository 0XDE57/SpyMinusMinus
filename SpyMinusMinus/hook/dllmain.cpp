// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <string>
#include <iostream>
#include "NamedPipeClient.h"

static bool attachConsole = false;
static bool isConsoleAttached = false;
static const UINT WM_HOOKWNDPROC = RegisterWindowMessage(L"WM_HOOKWNDPROC");

//#pragma data_seg(".shared")
HHOOK hhook;
HWND hwndListener;
//#pragma data_seg()

HINSTANCE dllInstance;


void CreateConsole() {
	if (!isConsoleAttached && AllocConsole()) {
		//AttachConsole(GetCurrentProcessId());
		freopen_s((FILE * *)stdout, "CONOUT$", "w", stdout);
		isConsoleAttached = true;
	}
}


BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {	
	switch (ul_reason_for_call)	{
		case DLL_PROCESS_ATTACH:
			dllInstance = hModule;
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			break;
	}
	return TRUE;
}

/** Hook to intercept messages, forwards to pipe. 
 * docs.microsoft.com/en-us/windows/desktop/winmsg/using-hooks 
 */
LRESULT WINAPI HookWndProc(int nCode, WPARAM wParam, LPARAM lParam) {	

	if (nCode < 0)
		return CallNextHookEx(NULL, nCode, wParam, lParam);


	CWPSTRUCT* cwp = (CWPSTRUCT *)lParam;

	
	if (isConsoleAttached) {
		printf_s("h:%i | m:%i | w:%i | l:%i\n", cwp->hwnd, cwp->message, cwp->wParam, cwp->lParam);		
	}
	/*
	if (hwndListener != nullptr) {
		//SendNotifyMessage(hwndListener, cwp->message, cwp->wParam, cwp->lParam);
		//SendMessage(hwndListener, cwp->message, cwp->wParam, cwp->lParam);
		//printf_s("h:%i | m:%i | w:%i | l:%i\n", cwp->hwnd, cwp->message, cwp->wParam, cwp->lParam);
		/*
		COPYDATASTRUCT data;
		data.dwData = (ULONG)hhook;
		data.cbData = sizeof(cwp);
		data.lpData = &cwp;
		*
		//SendMessage = blocking
		//SendMessage(hwndListener, WM_COPYDATA, 0, (LPARAM)&data);
	}*/

	//SendString(std::to_string((int)cwp->hwnd) + "," + std::to_string(cwp->message) + "," + std::to_string(cwp->wParam) + "," + std::to_string(cwp->lParam));
	SendCWPStruct(*cwp);
	


	switch (cwp->message) {
		case WM_NCDESTROY:
			ClosePipe();
			UnhookWindowsHookEx(hhook);
			//FreeConsole();
			//unload ? 	
			break;
		default:		
			if (cwp->message == WM_HOOKWNDPROC) {
				hwndListener = (HWND)cwp->lParam; 
				if (attachConsole) {
					CreateConsole();
					int pid = GetCurrentProcessId();
					printf_s("pid: %i | listener: 0x%p | hhook: 0x%p | dllInstance: 0x%p \n", pid, hwndListener, hhook, dllInstance);
				}
				ConnectPipeClient();				
			}
			break;
	}

	return CallNextHookEx(NULL, nCode, wParam, lParam);
}


EXTERN_C __declspec(dllexport) int Hook(HWND hwndTarget, HWND hwndListener) {

	//SetWinEventHook?
	//CBT hook?

	//WH_GETMESSAGE? called before a message reaches a window and can intercept and change the message to the window (used in the sample app
	//WH_MSGFILTER?
	//WH_CALLWNDPROCRET?
	//WH_DEBUG? called before any other hooktype gets called

	//Method: SetWindowsHookEx()
	SetLastError(0);
	hhook = SetWindowsHookEx(WH_CALLWNDPROC, HookWndProc, dllInstance, GetWindowThreadProcessId(hwndTarget, NULL));
	if (hhook == NULL) {
		int error = GetLastError();
		if (error) {
			std::cout << "could not hook: " << error << std::endl;
			return error;
		}
	}

	LRESULT success = SendMessage(hwndTarget, WM_HOOKWNDPROC, WPARAM(hhook), LPARAM(hwndListener));

	return (int)hhook;
}
