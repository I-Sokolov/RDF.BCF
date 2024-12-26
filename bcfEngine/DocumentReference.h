#pragma once

struct BCFProject;


class DocumentReference
{
public:
    DocumentReference(BCFProject& project) : m_project(project) {}

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    BCFProject& m_project;
};

