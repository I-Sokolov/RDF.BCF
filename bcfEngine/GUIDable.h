#pragma once

struct BCFProject;

/// <summary>
/// 
/// </summary>
struct GUIDable
{
public:
    static std::string CreateNewGUID();

public:
    const char* Guid() { return m_Guid.c_str(); }

protected:
    GUIDable(const char* guid);

protected:
    std::string m_Guid;
};


/// <summary>
/// 
/// </summary>
struct GuidReference : public GUIDable
{
public:
    GuidReference(BCFProject& project) : GUIDable(NULL), m_project(project) {}

    void Read(_xml::_element& elem, const std::string&);

private:
    BCFProject& m_project;
};