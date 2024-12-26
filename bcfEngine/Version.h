#pragma once

#include "XMLFile.h"
#include "bcfEngine.h"


class Version : public XMLFile
{
public:
    Version(BCFProject& project) : XMLFile(project) {}

    BCFVersion Get();
    void Set(BCFVersion version);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "bcf.version"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    std::string m_VersionId;
};

