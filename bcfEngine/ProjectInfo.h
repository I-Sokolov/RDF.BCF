#pragma once

#include "XMLFile.h"

class ProjectInfo : public XMLFile
{
public:
    ProjectInfo(Log& log) :XMLFile(log) {}

public:
    std::string m_ProjectId;
    std::string m_Name;

private:
    //XMLFile implementation
    virtual void GetRelativePathName(std::string& pathInBcfFolder) override { pathInBcfFolder.assign("project.bcfp"); }
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    void Read_Project(_xml::_element& elem);
};

