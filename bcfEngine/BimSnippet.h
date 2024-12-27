#pragma once

struct BCFProject;


class BimSnippet
{
public:
    BimSnippet(BCFProject& project) : m_project(project) { }

    void Read(_xml::_element & elem) { assert(!"TODO"); }

private:
    BCFProject& m_project;
};

