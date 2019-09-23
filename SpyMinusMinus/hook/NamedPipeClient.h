#pragma once
class NamedPipeClient {};

#include <string>

void ConnectPipeClient();
void ClosePipe();
void SendString(std::string message);
void SendCWPStruct(CWPSTRUCT cwp);
