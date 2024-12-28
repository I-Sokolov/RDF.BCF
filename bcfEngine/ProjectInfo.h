#pragma once

#include "XMLFile.h"
#include "GUIDable.h"

class ProjectInfo : public XMLFile
{
public:
    ProjectInfo(BCFProject& project, const char* projectId);

public:
    BCFGuid     m_ProjectId;
    std::string m_Name;

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "project.bcfp"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Project(_xml::_element& elem, const std::string& folder);
};

