#pragma once

#include "XMLFile.h"
#include "GuidStr.h"

class ProjectInfo : public XMLFile
{
public:
    ProjectInfo(BCFProject& project, const char* projectId);

public:
    std::string m_ProjectId;
    std::string m_Name;

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "project.bcfp"; }
    virtual const char* XSDName() override { return "project.xsd"; }
    virtual const char* RootElemName() override { return "ProjectInfo"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void UpgradeReadVersion(const std::string& folder) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

private:
    void Read_Project(_xml::_element& elem, const std::string& folder);
    void Write_Project(_xml_writer& writer, const std::string& folder);
};

