#pragma once

struct BCFProject;

#include "GUIDable.h"

class Topic : public GUIDable
{
public:
    Topic(BCFProject& project, const char* guid);

    bool Read(const std::string& bcfFolder) { return true; };
    bool Write(const std::string& bcfFolder) { return true; };

private:
    BCFProject& m_project;
};

