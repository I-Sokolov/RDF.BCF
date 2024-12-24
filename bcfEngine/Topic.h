#pragma once

struct BCFProject;

#include "GUIDable.h"
#include "XMLFile.h"
#include "BIMFile.h"

class Topic : public GUIDable, public XMLFile
{
public:
    Topic(BCFProject& project, const char* guid);

private:
    //XMLFile implementation
    virtual void GetRelativePathName(std::string& pathInBcfFolder) override;
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    void Read_Header(_xml::_element& elem);
    void Read_Files(_xml::_element& elem);
    void Read_File(_xml::_element& elem);
    void Read_Topic(_xml::_element& elem);

private:
    BCFProject& m_project;

    std::vector<BIMFile> m_bimFiles;
};

