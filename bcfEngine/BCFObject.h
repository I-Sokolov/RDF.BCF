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
    BCFObject(BCFProject& project) : m_project(project) {}
    virtual ~BCFObject() {}
   
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

    bool UpdateAuthor(std::string& author, std::string& date);

private:
    static std::string GetCurrentDate() { return GetCurrentTime(); }
    static std::string GetCurrentTime();

protected:
    BCFProject& m_project;
};

