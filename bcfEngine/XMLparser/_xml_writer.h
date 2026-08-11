#pragma once

#include "_errors.h"
#include "_reader.h"
#include "_string.h"

#include <fstream>
#include <locale>
#include <codecvt>
using namespace std;

// ************************************************************************************************
class _xml_writer
{

private: // Members

	ofstream*	m_pOutputStream;
	int			m_iIndent; // TAB-s count

public: // Methods

	_xml_writer(const char* szOutputFile)
		: m_pOutputStream(nullptr)
		, m_iIndent(0)
	{
		VERIFY_POINTER(szOutputFile);

		m_pOutputStream = new ofstream(szOutputFile, std::ios::out | std::ios::binary | std::ios::trunc);

		// Write UTF-8 BOM
		unsigned char BOM[3] = { 0xEF, 0xBB, 0xBF };
		m_pOutputStream->write((char*)BOM, sizeof(BOM));

		// UTF-8 locale
		try {
			// Try to use UTF-8 locale if available
			std::locale utf8_locale("en_US.UTF-8");
			m_pOutputStream->imbue(utf8_locale);
		}
		catch (...) {
			// Fallback to classic locale
			// For Emscripten, classic locale should work with UTF-8 if we use ofstream
			m_pOutputStream->imbue(std::locale::classic());
		}
	}

	virtual ~_xml_writer()
	{
		if (m_pOutputStream) {
			m_pOutputStream->close();
			delete m_pOutputStream;
		}
	}

	void write(const string& strText)
	{
		*getOutputStream() << strText;
	}

	// Writes text as escaped XML content (use write() for raw markup like the XML declaration).
	void writeText(const string& strText)
	{
		*getOutputStream() << _string::escapeXml(strText);
	}

	void writeComment(const string& strText)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strText);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<!--" << strText << "-->";
	}

	void writeStartTag(const string& strTag)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<" << strTag << ">";
	}

	void writeStartTag(const string& strTag, const vector<pair<string, string>>& vecAttributes)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<" << strTag;
		for (const auto& prAttribute : vecAttributes)
		{
			*getOutputStream() << " " << prAttribute.first << "=\"" << _string::escapeXml(prAttribute.second) << "\"";
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

		*getOutputStream() << "</" << strTag << ">";
	}

	void writeTag(const string& strTag, const string& strValue)
	{
		writeStartTag(strTag);
		writeText(strValue);
		writeEndTag(strTag, false);
	}

	void writeTag(const string& strTag, const vector<pair<string, string>>& vecAttributes, const string& strValue)
	{
		writeStartTag(strTag, vecAttributes);
		writeText(strValue);
		writeEndTag(strTag, false);
	}

	void writeTag(const string& strTag, const vector<pair<string, string>>& vecAttributes)
	{
		VERIFY_STLOBJ_IS_NOT_EMPTY(strTag);

		*getOutputStream() << "\n";
		writeIndent();
		*getOutputStream() << "<" << strTag;
		for (const auto& prAttribute : vecAttributes)
		{
			*getOutputStream() << " " << prAttribute.first << "=\"" << _string::escapeXml(prAttribute.second) << "\"";
		}
		*getOutputStream() << " />";
	}

	void writeIndent()
	{
		for (int iTab = 0; iTab < m_iIndent; iTab++)
		{
			*m_pOutputStream << TAB;
		}
	}

public: // Properties

	ofstream* getOutputStream() const { return m_pOutputStream; }
	int& indent() { return m_iIndent; }
};