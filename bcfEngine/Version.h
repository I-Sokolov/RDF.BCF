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
    //XMLFile implementation
    virtual void GetRelativePathName(std::string& pathInBcfFolder) override { pathInBcfFolder.assign("bcf.version"); }
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    std::string m_VersionId;
};

