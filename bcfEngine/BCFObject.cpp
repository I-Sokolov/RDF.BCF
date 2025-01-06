#include "pch.h"
#include "BCFObject.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
std::string BCFObject::GetCurrentTime()
{
    auto now = std::chrono::system_clock::now();
    time_t now_time = std::chrono::system_clock::to_time_t(now);

    std::tm tm = *std::gmtime(&now_time);
    std::chrono::milliseconds ms = std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch()) % 1000;

    std::ostringstream oss;
    oss << std::put_time(&tm, "%FT%T.") << std::setfill('0') << std::setw(3) << ms.count() << std::put_time(&tm, "%z");

    return oss.str();
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

