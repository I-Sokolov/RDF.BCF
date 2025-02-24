#pragma once

#include "XMLFile.h"
#include "GuidStr.h"

class ProjectInfo : public XMLFile
{
public:
    ProjectInfo(Project& project, const char* projectId);

    bool Validate(bool fix);

public:
    const char* GetProjectId() { return m_ProjectId.c_str(); }
    const char* GetName() { return m_Name.c_str(); }

    bool SetName(const char* val) { return SetPropertyString(val, m_Name); }

private:
    std::string m_ProjectId;
    std::string m_Name;

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "project.bcfp"; }
    virtual const char* XSDName() override { return "project.xsd"; }
    virtual const char* RootElemName() override { return "ProjectInfo"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string& folder) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

    virtual Topic* Topic_() override { return NULL; }
    virtual Comment* Comment_() override { return NULL; }

private:
    void Read_Project(_xml::_element& elem, const std::string& folder);
    void Write_Project(_xml_writer& writer, const std::string& folder);
};

