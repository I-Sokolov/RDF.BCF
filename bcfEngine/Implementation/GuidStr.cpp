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
        if (*guid && IsGUIDValid(guid, &project.GetLog())) {
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
    if (value.empty()) {
        if (IsGUIDValid(s.c_str(), &m_project.GetLog())) {
            value.assign(s);
        }
        else {
            AssignNew();
        }
    }
    else if (value!=s) {
        m_project.GetLog().add(Log::Level::warning, "Inconsistent GUIDs",  "%s is also referenced as %s", value.c_str(), s.c_str());
    }
}

