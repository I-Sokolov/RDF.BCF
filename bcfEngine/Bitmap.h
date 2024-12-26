#pragma once

struct BCFProject;


class Bitmap
{
public:
    Bitmap(BCFProject& project) : m_project(project) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    BCFProject& m_project;
};

