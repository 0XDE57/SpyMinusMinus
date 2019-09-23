#include "stdafx.h"
#include <string>
#include <iostream>
#include "NamedPipeClient.h"

LPCWSTR lpszPipename = TEXT("\\\\.\\pipe\\spyminuspipe");
HANDLE hPipe;

void ConnectPipeClient() {
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
		return;


	int lastError = GetLastError();
	if (lastError != ERROR_PIPE_BUSY) {
		std::cout << "Could not open pipe: " << lastError << std::endl;
		return;
	}

	// pipe instances are busy, wait 
	if (!WaitNamedPipe(lpszPipename, 20000)) {
		std::cout << "Could not open pipe: 20 second wait timed out." << std::endl;
		return;
	}

	std::cout << "Pipe Connected!" << std::endl;
}


void ClosePipe() {
	CloseHandle(hPipe);
}


void SendString(std::string message) {
	if (hPipe == INVALID_HANDLE_VALUE) {
		return;
	}

	DWORD cbWritten;
	BOOL success = WriteFile(
		hPipe,				// handle to pipe 
		message.c_str(),	// buffer to write from 
		message.size(),		// number of bytes to write, include the NULL
		&cbWritten,			// number of bytes written 
		NULL);				// not overlapped I/O 

	if (!success) {
		std::cout << "WriteFile to pipe failed: " << GetLastError() << std::endl;
	}
}


void SendCWPStruct(CWPSTRUCT cwp) {
	if (hPipe == INVALID_HANDLE_VALUE) {
		return;
	}

	char buffer[sizeof(cwp)];
	memcpy(&buffer, &cwp, sizeof(buffer));
	/*
	std::cout << "buffer:" << buffer << " size:" << sizeof(buffer) << " &" << &buffer << std::endl;
	for (int i = 0; i < sizeof(buffer); i++) {
		std::cout << std::to_string(i) << ": " << std::hex << i << " / " << std::to_string(buffer[i]) << std::endl;
	}*/

	DWORD cbWritten;
	BOOL success = WriteFile(
		hPipe,			// handle to pipe 
		reinterpret_cast<void*>(&buffer),	// buffer to write from 
		sizeof(buffer),	// number of bytes to write, include the NULL
		&cbWritten,		// number of bytes written 
		NULL);			// not overlapped I/O 

	if (!success) {
		std::cout << "WriteFile to pipe failed: " << GetLastError() << std::endl;
	}
}

