#include "pch.h"
#include "GUIDable.h"
#include "XMLFile.h"
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
bool BCFObject::SetIntVal(std::string& prop, int val)
{
    char sz[80];
    sprintf(sz, "%d", val);
    prop.assign(sz);
    return true;
}


/// <summary>
/// 
/// </summary>
BCFGuid::BCFGuid(BCFProject& project, const char* guid) 
    : BCFObject(project)
{
    if (guid) {
        if (*guid) {
            value.assign(guid);
        }
        else {
            CreateNew();
        }
    }
}

/// <summary>
/// 
/// </summary>
void BCFGuid::CreateNew()
{
    assert(empty());

    std::ostringstream oss;
    for (int i = 0; i < 4; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 6; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }

    assign(oss.str());
}

/// <summary>
/// 
/// </summary>
void BCFGuid::assign(const std::string& s)
{ 
    if (value.empty()) {
        value.assign(s);
    }
    else if (value!=s) {
        m_project.log().add(Log::Level::warning, "Inconsitent GUIDs",  "%s is aloso referenced as %s", value.c_str(), s.c_str());
    }
}


/// <summary>
/// 
/// </summary>
void GuidReference::Read(_xml::_element& elem, const std::string& /*folder*/)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)
}
