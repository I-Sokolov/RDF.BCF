#pragma once

struct BCFProject;

class Topic
{
public:
    Topic(BCFProject& project, const char* guid);

    const char* GUID() { return m_guid.c_str(); }

private:
    BCFProject& m_project;

    std::string m_guid;
};

