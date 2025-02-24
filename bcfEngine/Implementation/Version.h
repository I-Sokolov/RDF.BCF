#pragma once

#include "XMLFile.h"
#include "bcfTypes.h"


class Version : public XMLFile
{
public:
    Version(Project& project) : XMLFile(project, NULL), m_VersionId("3.0"){}

    BCFVersion Get();
    void Set(BCFVersion version);

    bool Validate(bool) { return true; }

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "bcf.version"; }
    virtual const char* XSDName() override { return "version.xsd"; }
    virtual const char* RootElemName() override { return "Version"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string&) override {}
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override {}

    virtual Topic* Topic_() override { return NULL; }
    virtual Comment* Comment_() override { return NULL; }

private:
    std::string m_VersionId;
};

