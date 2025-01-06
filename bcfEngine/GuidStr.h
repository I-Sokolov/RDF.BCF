#pragma once

struct BCFProject;

/// <summary>
/// 
/// </summary>
struct GuidStr
{
public:
    GuidStr(BCFProject& project, const char* guid);

    void CreateNew();

    bool empty() const { return value.empty(); }
    void assign(const std::string& s);

    const char* c_str() { return value.c_str(); }

private:
    BCFProject& m_project;
    std::string value;
};


