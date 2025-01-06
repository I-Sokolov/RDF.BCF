#pragma once

#include "bcfTypes.h"

class Log;

/// <summary>
/// 
/// </summary>
class BCFObject
{
public:
    BCFObject(BCFProject& project) : m_project(project) {}
    virtual ~BCFObject() {}
   
public:
    BCFProject& Project() { return m_project; }
    Log& log();

protected:
    bool IntToStr(int val, std::string& prop);
    bool UpdateAuthor(std::string& author, std::string& date);

private:
    static std::string GetCurrentDate() { return GetCurrentTime(); }
    static std::string GetCurrentTime();

protected:
    BCFProject& m_project;
};

