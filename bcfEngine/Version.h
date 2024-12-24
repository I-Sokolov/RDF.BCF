#pragma once

#include "XMLFile.h"
#include "bcfEngine.h"


class Version : public XMLFile
{
public:
    Version(Log& log) : XMLFile(log) {}

    BCFVersion Get();
    void Set(BCFVersion version);

private:
    virtual const char* FileName() override { return "bcf.version"; }
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    std::string m_VersionId;
};

