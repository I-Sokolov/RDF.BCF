#pragma once

#include "Log.h"
#include "bcfEngine.h"

class Version
{
public:
    Version(Log& log) :m_log(log) {}

    bool Read(const std::string& bcfFolder);
    bool Write(const std::string& bcfFolder);

    BCFVersion Get();
    void Set(BCFVersion version);

private:
    void ReadRoot(_xml::_element& elem);

private:
    Log&        m_log;
    std::string m_VersionId;
};

