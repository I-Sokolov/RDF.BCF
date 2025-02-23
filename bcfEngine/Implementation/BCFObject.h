#pragma once

class  Log;
struct Project;
struct XMLPoint;
struct BCFPoint;
class  ListOfBCFObjects;

/// <summary>
/// 
/// </summary>
struct BCFObject
{
public:
    static std::set<BCFObject*> gAllocatedObjects;

public:
    BCFObject(Project& project, ListOfBCFObjects* parentList);
    virtual ~BCFObject();
   
public:
    Project& Project_() { return m_project; }
    Log& Log_();

protected:
    bool SetPropertyString(const char* val, std::string& prop);

    bool SetPropertyInt(int val, std::string& prop);

    bool SetPropertyReal(double val, std::string& prop);

    bool GetPropertyBool(const std::string& prop) { return 0 == _stricmp(prop.c_str(), "true"); }
    bool SetPropertyBool(bool val, std::string& prop);

    std::string AbsolutePath(const std::string& relativePath, const std::string& folder);
    std::string CopyToRelative(const std::string& absolutePath, const std::string& folder, const char* relativePath);

    bool UpdateAuthor(std::string& author, std::string& date);

    static std::string TimeToStr(time_t tm);

    bool IsDateTimeValid(const char* str, const char* propName);
    bool IsIfcGuidValid(const char* str, const char* propName);
    bool IsFilePathValid(const char* str, const char* propName);

    static bool IsURL(const char* path);

    static std::string GetCurrentDate() { return GetCurrentTime(); }

    bool RemoveImpl();

private:
    static std::string GetCurrentTime();

protected:
    Project&            m_project;
    ListOfBCFObjects*   m_parentList;
};

#define VALIDATE(Prop,Predicate)    {if (val && *val && !Is##Predicate##Valid(val, #Prop)) return false;}
#define MARK_DIRTY                  Project_().MarkDirty()