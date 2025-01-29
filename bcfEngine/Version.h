#pragma once

#include "XMLFile.h"
#include "bcfTypes.h"


class Version : public XMLFile
{
public:
    Version(BCFProject& project) : XMLFile(project, NULL), m_VersionId("3.0"){}

    BCFVersion Get();
    void Set(BCFVersion version);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "bcf.version"; }
    virtual const char* XSDName() override { return "version.xsd"; }
    virtual const char* RootElemName() override { return "Version"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void UpgradeReadVersion() override {}
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override {}
private:
    std::string m_VersionId;
};

