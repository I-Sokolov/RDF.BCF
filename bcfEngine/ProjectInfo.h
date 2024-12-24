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
    virtual const char* FileName() override { return "project.bcfp"; }
    virtual void ReadRoot(_xml::_element& elem) override;

    void Read_Project(_xml::_element& elem);
    void Read_Name(_xml::_element& elem);
};

