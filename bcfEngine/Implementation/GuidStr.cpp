#include "pch.h"
#include "GuidStr.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
GuidStr::GuidStr(Project& project, const char* guid) 
    : m_project(project)
{
    if (guid) {

        //older versions allow capital letters in GUID
        std::string lower;
        if (m_project.GetVersion() < BCFVer_3_0) {
            lower.assign(guid);
            std::transform(lower.begin(), lower.end(), lower.begin(), [](unsigned char c) { return std::tolower(c); });
            guid = lower.c_str();
        }

        if (*guid && IsGUIDValid(guid, &project.Log_())) {
            value.assign(guid);
        }
        else {
            AssignNew();
        }
    }
}

/// <summary>
/// 
/// </summary>
bool GuidStr::IsGUIDValid(const char* str, Log* log)
{
    std::regex guidReg("[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}");

    if (std::regex_match(str, guidReg)) {
        return true;
    }

    if (log) {
        log->add(Log::Level::error, "Invalid value", "'%s' is not correct IfcGuid", str);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
void GuidStr::AssignNew()
{
    assert(IsEmpty());
    assign(New());

}

/// <summary>
/// 
/// </summary>
std::string GuidStr::New()
{
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

    return oss.str();
}

/// <summary>
/// 
/// </summary>
void GuidStr::assign(const std::string& s)
{ 
    const char* str = s.c_str();

    //older versions allow capital letters in GUID
    std::string lower;
    if (m_project.GetVersion() < BCFVer_3_0) {
        lower.assign(str);
        std::transform(lower.begin(), lower.end(), lower.begin(), [](unsigned char c) { return std::tolower(c); });
        str = lower.c_str();
    }

    if (value.empty()) {
        if (IsGUIDValid(str, &m_project.Log_())) {
            value.assign(str);
        }
        else {
            AssignNew();
        }
    }
    else if (0!=strcmp(value.c_str(), str)) {
        m_project.Log_().add(Log::Level::warning, "Inconsistent GUIDs",  "%s is also referenced as %s", value.c_str(), str);
    }
}

