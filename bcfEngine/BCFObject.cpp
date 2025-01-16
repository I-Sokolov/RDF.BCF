#include "pch.h"
#include "BCFObject.h"
#include "BCFProject.h"
#include "XMLPoint.h"
#include "FileSystem.h"

long BCFObject::gObjectCounter = 0;

/// <summary>
/// 
/// </summary>
std::string BCFObject::GetCurrentTime()
{
    auto now = std::chrono::system_clock::now();
    time_t now_time = std::chrono::system_clock::to_time_t(now);
    return TimeToStr(now_time);
}

/// <summary>
/// 
/// </summary>
std::string BCFObject::TimeToStr(time_t tm_)
{
    std::tm tm = *std::gmtime(&tm_);

    std::ostringstream oss;
    oss << std::put_time(&tm, "%FT%T") << std::setfill('0') << std::put_time(&tm, "%z");

    auto str = oss.str();

    str.insert(str.size() - 2, 1, ':');

    return str;
}

/// <summary>
/// 
/// </summary>
Log& BCFObject::log()
{
    return m_project.log();
}

/// <summary>
/// 
/// </summary>
bool BCFObject::IntToStr(int val, std::string& prop)
{
    char sz[80];
    sprintf(sz, "%d", val);
    prop.assign(sz);
    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFObject::RealToStr(double val, std::string& prop)
{
    char sz[80];
    sprintf(sz, "%g", val);
    prop.assign(sz);
    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFObject::GetPoint(const XMLPoint& xmlpt, BCFPoint& bcfpt)
{
    for (int i = 0; i < 3; i++) {
        bcfpt.xyz[i] = atof(xmlpt.XYZ[i].c_str());
    }

    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFObject::SetPoint(const BCFPoint* bcfpt, XMLPoint& xmlpt)
{
    if (bcfpt) {
        for (int i = 0; i < 3; i++) {
            RealToStr(bcfpt->xyz[i], xmlpt.XYZ[i]);
        }
        return true;
    }
    else {
        m_project.log().add(Log::Level::error, "NULL argument");
        return false;
    }
}

/// <summary>
/// 
/// </summary>
bool BCFObject::UpdateAuthor(std::string& author, std::string& date)
{
    const char* user = Project().GetAuthor();
    if (!*user) {
        Project().log().add(Log::Level::error, "Author is not set");
        return false;
    }

    if (!Project().GetExtensions().CheckElement(BCFUsers, user)) {
        return false;
    }

    author.assign(user);
    date.assign(GetCurrentDate());

    return true;
}

/// <summary>
/// 
/// </summary>
std::string BCFObject::AbsolutePath(const std::string& relativePath, const std::string& folder)
{
    if (!relativePath.empty()) {
        std::string filePath(folder);
        FileSystem::AddPath(filePath, relativePath.c_str());
        if (!FileSystem::Exists(filePath.c_str())) {
            log().add(Log::Level::error, "File read", "File does not exist: %s", filePath.c_str());
            throw std::exception("Failed to read viewpoint");
        }
        return filePath;
    }
    return "";
}


/// <summary>
/// 
/// </summary>
std::string BCFObject::CopyToRelative(const std::string& absolutePath, const std::string& folder, const char* relativePath)
{
    if (absolutePath.empty())
        return "";

    auto filename = FileSystem::GetFileName(absolutePath.c_str(), log());
    if (!filename.empty()) {

        std::string target(folder);
        if (relativePath && *relativePath) {
            FileSystem::AddPath(target, relativePath);
        }

        FileSystem::AddPath(target, filename.c_str());

        if (FileSystem::Exists(target.c_str())
            || FileSystem::CopyFile(absolutePath.c_str(), target.c_str(), log())
            ) {

            std::string ret;
            if (relativePath && *relativePath) {
                ret.assign(relativePath);
            }
            FileSystem::AddPath(ret, filename.c_str());

            return ret;
        }
    }
    throw std::exception("Failed to copy internal file to BCF package");
}

/// <summary>
/// 
/// </summary>
bool BCFObject::IsDateTimeValid (const char* str, const char* propName)
{
    std::regex dateReg(R"(^(\d{4}-\d{2}-\d{2})(T(\d{2}:\d{2}:\d{2})(\.\d+)?(Z|[\+\-]\d{2}:\d{2})?)?$)");

    if (std::regex_match(str, dateReg)) {
        return true;
    }

    log().add(Log::Level::error, "Invalid value", "'%s' is not correct ISO 8601 date-time and can not be used as %s", str, propName);
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFObject::IsIfcGuidValid(const char* str, const char* propName)
{
    std::regex pattern("[0-9A-Za-z_$]{22}");
   
    if (std::regex_match(str, pattern)) {
        return true;
    }

    log().add(Log::Level::error, "Invalid value", "'%s' is not correct IfcGuid and can not be used as %s", str, propName);
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFObject::IsFilePathValid(const char* str, const char* propName)
{
    if (IsURL(str)) {
        return true;
    }
    if (FileSystem::Exists(str)) {
        return true;
    }

    log().add(Log::Level::error, "Invalid value", "'%s' is not existing file and can not be used as %s", str, propName);
    return false;
}

//color "[0-9A-Fa-f]{6}([0-9A-Fa-f]{2})?"

/// <summary>
/// 
/// </summary>
bool BCFObject::IsURL(const char* path)
{
    if (!path) {
        return false;
    }

    std::string data(path, 6);
    std::transform(data.begin(), data.end(), data.begin(), [](unsigned char c) { return std::tolower(c); });

    return data == "https:" || data == "http:/" || data == "ftp://";
}

/// <summary>
/// 
/// </summary>
bool BCFObject::Remove()
{
    if (m_parentList) {
        return m_parentList->Remove(this);
    }
    else {
        assert(false);
        return true;
    }
}
