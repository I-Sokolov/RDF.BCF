#pragma once

#include "_errors.h"
#include "_reader.h"

#include <fstream>
#include <locale>
#include <codecvt>
using namespace std;

// ************************************************************************************************
class _xml_writer
{

private: // Members

	wofstream* m_pOutputStream;
	int m_iIndent; // TAB-s count

public: // Methods

	_xml_writer(const char* szOutputFile)
		: m_pOutputStream(nullptr)
		, m_iIndent(0)
	{
		VERIFY_POINTER(szOutputFile);

		std::ofstream outputStream;
		outputStream.open(szOutputFile, std::ios::out | std::ios::binary | std::ios::trunc);

		unsigned char BOM[3] = { 0xEF, 0xBB, 0xBF };
		outputStream.write((char*)BOM, sizeof(BOM));
		outputStream.close();

		m_pOutputStream = new wofstream(szOutputFile, std::ios::out | std::ios::app);

		// UTF-8 locale
		std::locale loc(std::locale(), new std::codecvt_utf8<char>);
		m_pOutputStream->imbue(loc);
	}

	virtual ~_xml_writer()
	{
		delete m_pOutputStream;
	}

	void write(const string& strText)
	{
		*getOutputStream() << strText.c_str();
	}

	void writeComment(const string& strText)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strText);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<!--" << strText.c_str() << "-->";
	}

	void writeStartTag(const string& strTag)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<" << strTag.c_str();
		*getOutputStream() << ">";
	}

	void writeStartTag(const string& strTag, const vector<pair<string, string>>& vecAttributes)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<" << strTag.c_str();
		for (auto prAttribute : vecAttributes)
		{
			*getOutputStream() << " " << prAttribute.first.c_str() << "=\"" << prAttribute.second.c_str() << "\"";
		}
		*getOutputStream() << ">";
	}

	void writeEndTag(const string& strTag, bool bNewLine = true)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		if (bNewLine)
		{
			*getOutputStream() << "\n";
			writeIndent();
		}

		*getOutputStream() << "</" << strTag.c_str() << ">";
	}

	void writeTag(const string& strTag, const string& strValue)
	{
		writeStartTag(strTag);
		write(strValue);
		writeEndTag(strTag, false);
	}

	void writeTag(const string& strTag, const vector<pair<string, string>>& vecAttributes, const string& strValue)
	{
		writeStartTag(strTag, vecAttributes);
		write(strValue);
		writeEndTag(strTag, false);
	}

	void writeIndent()
	{
		for (int iTab = 0; iTab < m_iIndent; iTab++)
		{
			*m_pOutputStream << TAB;
		}
	}

public: // Properties

	wofstream* getOutputStream() const { return m_pOutputStream; }
	int& indent() { return m_iIndent; }
};