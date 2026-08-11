#pragma once

#ifdef _WINDOWS
#include <atlbase.h>
using namespace ATL;
#else
#define _atoi64(s) strtoull(s, nullptr, 10)
#endif // _WINDOWS

#include <locale>
#include <codecvt>
#include <string>
#include <vector>
#include <algorithm> 
#include <functional>
#include <cwchar>
#include <uchar.h>
using namespace std;

#include <assert.h>

#define CHR2STR(c) string(1, c)

// ************************************************************************************************
class _string
{

public: //  Methods
		
	static inline void ltrim(string& strInput, function<int(int)> fnTrim = [](int c) { return isblank((unsigned char)c); })
	{
		strInput.erase(strInput.begin(), find_if(strInput.begin(), strInput.end(), not1(fnTrim)));
	}

	static inline void ltrim(string& strInput, char cToTrim)
	{
		function<int(int)> fnTrim = [cToTrim](int c) { return c == cToTrim; };

		strInput.erase(strInput.begin(), find_if(strInput.begin(), strInput.end(), not1(fnTrim)));
	}

	static inline void rtrim(string& strInput, function<int(int)> fnTrim = [](int c) { return isblank((unsigned char)c); })
	{
		strInput.erase(find_if(strInput.rbegin(), strInput.rend(), not1(fnTrim)).base(), strInput.end());
	}

	static inline void rtrim(string& strInput, char cToTrim)
	{
		function<int(int)> fnTrim = [cToTrim](int c) { return c == cToTrim; };

		strInput.erase(find_if(strInput.rbegin(), strInput.rend(), not1(fnTrim)).base(), strInput.end());
	}

	static inline void trim(string& strInput)
	{
		rtrim(strInput);
		ltrim(strInput);
	}

	static inline void trim(vector<string>& vecInput)
	{
		for (auto& strInput : vecInput)
		{
			rtrim(strInput);
			ltrim(strInput);
		}		
	}

	static inline void trim(string& strInput, char cToTrim)
	{
		function<int(int)> fnTrim = [cToTrim](int c) { return c == cToTrim; };

		rtrim(strInput, fnTrim);
		ltrim(strInput, fnTrim);
	}

	static inline string ltrim_copy(string strInput)
	{
		ltrim(strInput);

		return strInput;
	}

	static inline string rtrim_copy(string strInput)
	{
		rtrim(strInput);

		return strInput;
	}

	static inline string trim_copy(string strInput)
	{
		trim(strInput);

		return strInput;
	}

	static inline void toUpper(string& strInput)
	{
		transform(strInput.begin(), strInput.end(), strInput.begin(),
			[](unsigned char c) { return (unsigned char)toupper(c); });
	}

	static inline void toLower(string& strInput)
	{
		transform(strInput.begin(), strInput.end(), strInput.begin(),
			[](unsigned char c) { return (unsigned char)tolower(c); });
	}

	static inline bool equal(const string& strInput1, const string& strInput2, bool bIgnoreCase)
	{
		if (bIgnoreCase)
		{
			string strInput1Copy = strInput1;
			toUpper(strInput1Copy);

			string strInput2Copy = strInput2;
			toUpper(strInput2Copy);

			return strInput1Copy == strInput2Copy;
		}

		return strInput1 == strInput2;
	}

	static inline bool contains(const string& strInput, const string& strValue, bool bIgnoreCase = true)
	{
		if (bIgnoreCase)
		{
			string strInputCopy = strInput;
			toUpper(strInputCopy);

			string strValueCopy = strValue;
			toUpper(strValueCopy);

			return strInputCopy.find(strValueCopy) != string::npos;
		}

		return strInput.find(strValue) != string::npos;
	}

	static inline bool startsWith(const string& strInput, const string& strValue, bool bIgnoreCase= true)
	{
		if (bIgnoreCase)
		{
			string strInputCopy = strInput;
			toUpper(strInputCopy);			

			string strValueCopy = strValue;
			toUpper(strValueCopy);

			return strInputCopy.find(strValueCopy) == 0;
		}		

		return strInput.find(strValue) == 0;
	}

	static inline bool endsWith(const string& strInput, const string& strValue, bool bIgnoreCase = true)
	{
		if (strValue.size() > strInput.size())
		{
			return false;
		}

		if (bIgnoreCase)
		{
			string strInputCopy = strInput;
			toUpper(strInputCopy);

			string strValueCopy = strValue;
			toUpper(strValueCopy);

			return strInputCopy.rfind(strValueCopy) == (strInputCopy.size() - strValueCopy.size());
		}

		return strInput.rfind(strValue) == (strInput.size() - strValue.size());
	}

	static inline bool startsEndsWith(const string& strInput, const string& strStartValue, const string& strEndValue, bool bIgnoreCase)
	{
		if ((strStartValue.size() > strInput.size()) || 
			(strEndValue.size() > strInput.size()))
		{
			return false;
		}

		if (bIgnoreCase)
		{
			string strInputCopy = strInput;
			toUpper(strInputCopy);

			string strStartValueCopy = strStartValue;
			toUpper(strStartValueCopy);

			if (strInputCopy.find(strStartValueCopy) != 0)
			{
				return false;
			}

			string strEndValueCopy = strEndValue;
			toUpper(strEndValueCopy);

			return(strInputCopy.rfind(strEndValueCopy) == (strInputCopy.size() - strEndValueCopy.size()));
		}

		return (strInput.find(strStartValue) == 0) &&
			(strInput.rfind(strEndValue) == (strInput.size() - strEndValue.size()));
	}

	static inline void tokenize(const string& strInput, const string& strDelimter, vector<string>& vecTokens, bool bFirstOnly = false)
	{
		vecTokens.clear();

		string strInputCopy = strInput;
		trim(strInputCopy);

		size_t iPos = 0;		
		while ((iPos = strInputCopy.find(strDelimter)) != string::npos)
		{
			string strToken = strInputCopy.substr(0, iPos);
			vecTokens.push_back(strToken);

			strInputCopy.erase(0, iPos + strDelimter.length());

			if (bFirstOnly)
			{
				break;
			}
		}

		vecTokens.push_back(strInputCopy);
	}

	static inline void tokenizeN(const string& strInput, const string& strDelimter, vector<string>& vecTokens, int iTokensCount)
	{
		assert(iTokensCount >= 1);

		vecTokens.clear();

		string strInputCopy = strInput;
		trim(strInputCopy);

		size_t iPos = 0;
		while ((iPos = strInputCopy.find(strDelimter)) != string::npos)
		{
			string strToken = strInputCopy.substr(0, iPos);
			vecTokens.push_back(strToken);

			strInputCopy.erase(0, iPos + strDelimter.length());

			if ((int)vecTokens.size() == iTokensCount)
			{
				break;
			}
		}

		if (!strInputCopy.empty())
		{
			if ((int)vecTokens.size() == iTokensCount)
			{
				vecTokens[vecTokens.size() - 1] += strDelimter;
				vecTokens[vecTokens.size() - 1] += strInputCopy;
			}
			else
			{
				vecTokens.push_back(strInputCopy);
			}
		}
	}

	static inline void split(const string& strInput, const string& strDelimter, vector<string>& vecTokens, bool bRemoveEmpty = true)
	{
		tokenize(strInput, strDelimter, vecTokens);

		if (bRemoveEmpty)
		{
			trim(vecTokens);

			auto isEmptyOrBlank = [](const string& s) {
				return s.find_first_not_of(" \t") == string::npos;
			};

			vecTokens.erase(remove_if(vecTokens.begin(), vecTokens.end(), isEmptyOrBlank), vecTokens.end());
		}		
	}

	static inline void replace(string& strInput, const string& strOldValue, const string& strNewValue)
	{
		size_t iIndex = 0;
		while (true) 
		{
			iIndex = strInput.find(strOldValue, iIndex);
			if (iIndex == string::npos)
			{
				break;
			}

			strInput.replace(iIndex, strOldValue.size(), strNewValue);

			iIndex += strNewValue.size();
		} // while (true)
	}

	template<typename... Arguments> 
	static inline string format(const char* szInput, Arguments... args)
	{
		char szBuffer[1024];

#ifdef _WINDOWS
		sprintf_s(szBuffer, 1024, szInput, args...);
#else
		sprintf(szBuffer, szInput, args...);
#endif		
		return szBuffer;
	}

	template<typename... Arguments>
	static inline string sformat(const string& strInput, Arguments... args)
	{
		char szBuffer[1024];

#ifdef _WINDOWS
		sprintf_s(szBuffer, 1024, strInput.c_str(), args...);
#else
		sprintf(szBuffer, strInput.c_str(), args...);
#endif
		return szBuffer;
	}

	static inline bool isInteger(const string& strInput)
	{
		return !strInput.empty() && 
			find_if(strInput.begin(),
			strInput.end(), 
			[](unsigned char c) { return !isdigit(c); }) == strInput.end();
	}

	static inline bool isXInteger(const string& strInput)
	{
		return !strInput.empty() &&
			find_if(strInput.begin(),
				strInput.end(),
				[](unsigned char c) { return !isxdigit(c); }) == strInput.end();
	}

	// Escapes characters that are not allowed as-is in XML text/attribute values.
	static inline string escapeXml(const string& strInput)
	{
		string strOutput;
		strOutput.reserve(strInput.size());

		for (char ch : strInput)
		{
			switch (ch)
			{
			case '&':  strOutput += "&amp;";  break;
			case '<':  strOutput += "&lt;";   break;
			case '>':  strOutput += "&gt;";   break;
			case '"':  strOutput += "&quot;"; break;
			case '\'': strOutput += "&apos;"; break;
			default:   strOutput += ch;       break;
			}
		}

		return strOutput;
	}

	// Decodes XML character/entity references (&amp; &lt; &gt; &quot; &apos; &#NN; &#xHH;) back to plain text.
	static inline string unescapeXml(const string& strInput)
	{
		if (strInput.find('&') == string::npos)
		{
			return strInput;
		}

		string strOutput;
		strOutput.reserve(strInput.size());

		size_t iPos = 0;
		while (iPos < strInput.size())
		{
			char ch = strInput[iPos];
			if (ch == '&')
			{
				size_t iSemicolon = strInput.find(';', iPos + 1);
				if (iSemicolon != string::npos)
				{
					string strEntity = strInput.substr(iPos + 1, iSemicolon - iPos - 1);

					if (strEntity == "amp")
					{
						strOutput += '&';
						iPos = iSemicolon + 1;
						continue;
					}
					else if (strEntity == "lt")
					{
						strOutput += '<';
						iPos = iSemicolon + 1;
						continue;
					}
					else if (strEntity == "gt")
					{
						strOutput += '>';
						iPos = iSemicolon + 1;
						continue;
					}
					else if (strEntity == "quot")
					{
						strOutput += '"';
						iPos = iSemicolon + 1;
						continue;
					}
					else if (strEntity == "apos")
					{
						strOutput += '\'';
						iPos = iSemicolon + 1;
						continue;
					}
					else if (!strEntity.empty() && (strEntity[0] == '#'))
					{
						try
						{
							unsigned long iCode = 0;
							if ((strEntity.size() > 1) && ((strEntity[1] == 'x') || (strEntity[1] == 'X')))
							{
								iCode = stoul(strEntity.substr(2), nullptr, 16);
							}
							else
							{
								iCode = stoul(strEntity.substr(1), nullptr, 10);
							}

							// Encode code point as UTF-8
							if (iCode <= 0x7F)
							{
								strOutput += (char)iCode;
							}
							else if (iCode <= 0x7FF)
							{
								strOutput += (char)(0xC0 | (iCode >> 6));
								strOutput += (char)(0x80 | (iCode & 0x3F));
							}
							else if (iCode <= 0xFFFF)
							{
								strOutput += (char)(0xE0 | (iCode >> 12));
								strOutput += (char)(0x80 | ((iCode >> 6) & 0x3F));
								strOutput += (char)(0x80 | (iCode & 0x3F));
							}
							else
							{
								strOutput += (char)(0xF0 | (iCode >> 18));
								strOutput += (char)(0x80 | ((iCode >> 12) & 0x3F));
								strOutput += (char)(0x80 | ((iCode >> 6) & 0x3F));
								strOutput += (char)(0x80 | (iCode & 0x3F));
							}

							iPos = iSemicolon + 1;
							continue;
						}
						catch (...)
						{
							// Not a valid numeric reference - fall through and copy '&' as-is
						}
					}
				} // if (iSemicolon != string::npos)
			} // if (ch == '&')

			strOutput += ch;
			iPos++;
		} // while (iPos < strInput.size())

		return strOutput;
	}
};

// ************************************************************************************************
static wstring utf8_to_wstring(const char* szInput)
{
	assert(szInput != nullptr);

	return wstring_convert<codecvt_utf8_utf16<wchar_t>>().from_bytes(szInput);
}

// ************************************************************************************************
static string wstring_to_utf8(const wchar_t* szInput)
{
	assert(szInput != nullptr);

	return wstring_convert<codecvt_utf8_utf16<wchar_t>>().to_bytes(szInput);
}

// ************************************************************************************************
typedef string u8string;

static u8string To_UTF8(const u16string& s)
{
	wstring_convert<codecvt_utf8_utf16<char16_t>, char16_t> conv;
	return conv.to_bytes(s);
}

static u8string To_UTF8(const u32string& s)
{
	wstring_convert<codecvt_utf8<char32_t>, char32_t> conv;
	return conv.to_bytes(s);
}

static u16string To_UTF16(const u8string& s)
{
	wstring_convert<codecvt_utf8_utf16<char16_t>, char16_t> conv;
	return conv.from_bytes(s);
}

static u16string To_UTF16(const u32string& s)
{
	wstring_convert<codecvt_utf16<char32_t>, char32_t> conv;
	string bytes = conv.to_bytes(s);
	return u16string(reinterpret_cast<const char16_t*>(bytes.c_str()), bytes.length() / sizeof(char16_t));
}

static u32string To_UTF32(const u8string& s)
{
	wstring_convert<codecvt_utf8<char32_t>, char32_t> conv;
	return conv.from_bytes(s);
}

static u32string To_UTF32(const u16string& s)
{
	const char16_t* pData = s.c_str();
	wstring_convert<codecvt_utf16<char32_t>, char32_t> conv;
	return conv.from_bytes(reinterpret_cast<const char*>(pData), reinterpret_cast<const char*>(pData + s.length()));
}

// ************************************************************************************************
#ifndef _WINDOWS
#define LPCSTR const char*
#define LPCWSTR const wchar_t*

// ************************************************************************************************
class CW2A
{

private: // Members

	string m_strOutput;

public: // Methods

	CW2A(const wchar_t* szInput)
		: m_strOutput("")
	{
		m_strOutput = wstring_to_utf8(szInput);
	}

	operator LPCSTR()
	{
		return m_strOutput.c_str();
	}
};

// ************************************************************************************************
class CA2W
{

private: // Members

	wstring m_strOutput;

public: // Methods

	CA2W(const char* szInput)
		: m_strOutput(L"")
	{
		m_strOutput = utf8_to_wstring(szInput);
	}

	operator LPCWSTR()
	{
		return m_strOutput.c_str();
	}
};
#endif // _WINDOWS