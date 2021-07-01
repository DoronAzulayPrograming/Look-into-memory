#pragma once
#include <stdlib.h>
#include <string.h>

char* wcharToChar(wchar_t* text);
wchar_t* charToWChar(const char* text);
const wchar_t* getWChar(const char* c);
__forceinline wchar_t locase_w(wchar_t c);
int _strcmpi_w(const wchar_t* s1, const wchar_t* s2);
