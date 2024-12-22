#pragma once

#include "Log.h"
#include "bcfEngine.h"

class Extensions
{
public:
    Extensions(Log& log);

    bool Read(const std::string& bcfFolder);
    bool Write(const std::string& bcfFolder);

    const char* GetElement(BCFEnumeration enumeration, unsigned short index);
    bool AddElement(BCFEnumeration enumeration, const char* element);
    bool RemoveElement(BCFEnumeration enumeration, const char* element);

private:
    StringSet* GetList(BCFEnumeration enumeration);

private:
    Log&                    m_log;
    std::vector<StringSet>  m_elements;
};

