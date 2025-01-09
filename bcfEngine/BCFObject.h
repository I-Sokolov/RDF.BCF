#pragma once

class Log;
struct BCFProject;
struct XMLPoint;
struct BCFPoint;

/// <summary>
/// 
/// </summary>
struct BCFObject
{
public:
    static long gObjectCounter;

public:
    BCFObject(BCFProject& project) : m_project(project) { gObjectCounter++; }
    virtual ~BCFObject() { gObjectCounter--; }
   
public:
    BCFProject& Project() { return m_project; }
    Log& log();

protected:
    bool IntToStr(int val, std::string& prop);

    bool RealToStr(double val, std::string& prop);

    bool StrToBool(const std::string& prop) { return 0 == _stricmp(prop.c_str(), "true"); }
    bool BoolToStr(bool val, std::string& prop) { prop.assign(val ? "true" : "false"); return true; }

    bool GetPoint(const XMLPoint& xmlpt, BCFPoint& bcfpt);
    bool SetPoint(const BCFPoint* bcfpt, XMLPoint& xmlpt);

    std::string AbsolutePath(const std::string& relativePath, const std::string& folder);
    std::string CopyToRelative(const std::string& absolutePath, const std::string& folder, const char* relativePath);

    bool UpdateAuthor(std::string& author, std::string& date);

private:
    static std::string GetCurrentDate() { return GetCurrentTime(); }
    static std::string GetCurrentTime();

protected:
    BCFProject& m_project;
};

