#include "smStr.h"

char* wcharToChar(wchar_t* text)
{
	size_t bytesConverted;
	char NameBuffer[256];

	wcstombs_s(&bytesConverted, NameBuffer, text, 256);
	return NameBuffer;
}
wchar_t* charToWChar(const char* text)
{
	size_t bytesConverted;
	wchar_t NameBuffer[256];

	mbstowcs_s(&bytesConverted, NameBuffer, text, 256);
	return NameBuffer;
}
const wchar_t* getWChar(const char* c)
{
	size_t bytesConverted;
	const size_t cSize = strlen(c) + 1;
	wchar_t* wc = new wchar_t[cSize];
	mbstowcs_s(&bytesConverted, wc, cSize, c, cSize - 1);

	return wc;
}
__forceinline wchar_t locase_w(wchar_t c)
{
	if ((c >= 'A') && (c <= 'Z'))
		return c + 0x20;
	else
		return c;
}
int _strcmpi_w(const wchar_t* s1, const wchar_t* s2)
{
	wchar_t c1, c2;

	if (s1 == s2)
		return 0;

	if (s1 == 0)
		return -1;

	if (s2 == 0)
		return 1;

	do {
		c1 = locase_w(*s1);
		c2 = locase_w(*s2);
		s1++;
		s2++;
	} while ((c1 != 0) && (c1 == c2));

	return (int)(c1 - c2);
}

