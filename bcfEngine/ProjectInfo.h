#pragma once

#include "XMLFile.h"

class ProjectInfo : public XMLFile
{
public:
    ProjectInfo(BCFProject& project) :XMLFile(project) {}

public:
    std::string m_ProjectId;
    std::string m_Name;

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "project.bcfp"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Project(_xml::_element& elem, const std::string& folder);
};

