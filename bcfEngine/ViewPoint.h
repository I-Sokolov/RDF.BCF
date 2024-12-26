#pragma once

#include "XMLFile.h"
#include "GUIDable.h"

class ViewPoint : public GUIDable, public XMLFile
{
public:
    ViewPoint(BCFProject& project) : GUIDable(NULL), XMLFile(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return m_Viewpoint.c_str(); }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    std::string m_Viewpoint; //name.bcfv
    std::string m_Snapshot;  //name.jpg
};

