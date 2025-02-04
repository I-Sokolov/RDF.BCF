#pragma once

struct BCFProject;
class Log;

/// <summary>
/// 
/// </summary>
struct GuidStr
{
public:
    static bool IsGUIDValid(const char* str, Log* log);
    static std::string New();

public:
    GuidStr(BCFProject& project, const char* guid);

    void AssignNew();

    bool IsEmpty() const { return value.empty(); }
    void assign(const std::string& s);

    const char* c_str() { return value.c_str(); }

private:
    BCFProject& m_project;
    std::string value;
};


