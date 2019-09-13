//docs.microsoft.com/en-us/windows/desktop/winmsg/using-hooks

// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <string>
#include <iostream>

//TODO:
//does releasing a hook leave a dll instance loaded in the target?

static bool consoleAttached = false;

static const UINT WM_HOOKWNDPROC = RegisterWindowMessage(L"WM_HOOKWNDPROC");

//#pragma data_seg(".shared")
#define BUFFSIZE 512
HHOOK hhook;
HWND hwndListener;

LPCWSTR lpszPipename = TEXT("\\\\.\\pipe\\testpipe");
HANDLE hPipe;
 
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

			//CreateConsole();
			//printf_s("DLL_PROCESS_ATTACH: %x \n", hModule);
			break;
		case DLL_THREAD_ATTACH:
			//printf_s("DLL_THREAD_ATTACH: %x \n", hModule);
			break;
		case DLL_THREAD_DETACH:
			//printf_s("DLL_THREAD_DETACH: %x \n", hModule);
			break;
		case DLL_PROCESS_DETACH:
			//printf_s("DLL_PROCESS_DETACH: %x \n", hModule);
			//FreeConsole();
			break;
	}
	return TRUE;
}



LRESULT WINAPI HookWndProc(int nCode, WPARAM wParam, LPARAM lParam) {	

	if (nCode < 0)
		return CallNextHookEx(NULL, nCode, wParam, lParam);


	CWPSTRUCT* cwp = (CWPSTRUCT *)lParam;

	
	if (consoleAttached) {
		printf_s("h:%i | m:%i | w:%i | l:%i\n", cwp->hwnd, cwp->message, cwp->wParam, cwp->lParam);		
	}
	
	if (hwndListener != nullptr) {
		//SendNotifyMessage(hwndListener, cwp->message, cwp->wParam, cwp->lParam);
		//SendMessage(hwndListener, cwp->message, cwp->wParam, cwp->lParam);
		//printf_s("h:%i | m:%i | w:%i | l:%i\n", cwp->hwnd, cwp->message, cwp->wParam, cwp->lParam);
		/*
		COPYDATASTRUCT data;
		data.dwData = (ULONG)hhook;
		data.cbData = sizeof(cwp);
		data.lpData = &cwp;
		*/
		//SendMessage = blocking
		//SendMessage(hwndListener, WM_COPYDATA, 0, (LPARAM)&data);
		//use named pipe instead? //docs.microsoft.com/en-us/windows/desktop/ipc/named-pipe-client	
		
		
		

		if (hPipe != INVALID_HANDLE_VALUE) {
			std::string str = "Test: " + std::to_string(cwp->message);

			DWORD cbWritten;		
			BOOL success = WriteFile(
				hPipe,		// handle to pipe 
				str.c_str(),		// buffer to write from 
				str.size(),	// number of bytes to write, include the NULL
				&cbWritten,	// number of bytes written 
				NULL);		// not overlapped I/O 

			if (!success) {
				std::cout << "WriteFile to pipe failed: " << GetLastError() << std::endl;
			}
		}
		
	}
	
	switch (cwp->message) {
		case WM_NCDESTROY:
			UnhookWindowsHookEx(hhook);
			//FreeConsole();
			//unload ? 
			CloseHandle(hPipe);
			break;
		default:
			//SendNotifyMessage(hwndListener, nCode, wParam, lParam); 
			//SendMessage = blocking
			//SendMessageTimout = blocking until timeout?
			//PostMessage

			
			if (cwp->message == WM_HOOKWNDPROC) {
				hwndListener = (HWND)cwp->lParam; 
				//hwndListender = FindWindow("SpyMinusMinus")?
				//CreateConsole();
				//printf_s("listener: 0x%x - hhook: 0x%x\n", hwndListener, hhook);
				hPipe = CreateFileW(
					lpszPipename, 
					GENERIC_READ | GENERIC_WRITE,
					0,              // no sharing 
					NULL,           // default security attributes
					OPEN_EXISTING,  // opens existing pipe 
					0,              // default attributes 
					NULL);          // no template file 

				//DWORD mode = PIPE_READMODE_MESSAGE;
				//SetNamedPipeHandleState(hPipe, &mode, nullptr, nullptr);

				if (hPipe != INVALID_HANDLE_VALUE)
					break;


				int lastError = GetLastError();
				if (lastError != ERROR_PIPE_BUSY) {
					std::cout << "Could not open pipe: " << lastError << std::endl;
					break;
				}

				// pipe instances are busy, wait 
				if (!WaitNamedPipe(lpszPipename, 20000)) {
					std::cout << "Could not open pipe: 20 second wait timed out." << std::endl;
					break;
				}

				std::cout << "Pipe Connected!" << std::endl;
			}
			break;
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
	if (hhook == NULL) {
		int error = GetLastError();
		if (error) {
			printf_s("could not hook %x", error);
			return error;
		}
	}

	LRESULT success = SendMessage(hwndTarget, WM_HOOKWNDPROC, WPARAM(hhook), LPARAM(hwndListener));

	return (int)hhook;
}
