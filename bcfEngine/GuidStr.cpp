#include "pch.h"
#include "GuidStr.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
GuidStr::GuidStr(BCFProject& project, const char* guid) 
    : m_project(project)
{
    if (guid) {
        if (*guid && IsGUIDValid(guid)) {
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
bool GuidStr::IsGUIDValid(const char* str)
{
    std::regex guidReg("[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}");

    if (std::regex_match(str, guidReg)) {
        return true;
    }

    m_project.log().add(Log::Level::error, "Invalid value", "'%s' is not correct IfcGuid", str);
    return false;
}

/// <summary>
/// 
/// </summary>
void GuidStr::CreateNew()
{
    assert(IsEmpty());

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
void GuidStr::assign(const std::string& s)
{ 
    if (value.empty()) {
        if (IsGUIDValid(s.c_str())) {
            value.assign(s);
        }
        else {
            CreateNew();
        }
    }
    else if (value!=s) {
        m_project.log().add(Log::Level::warning, "Inconsitent GUIDs",  "%s is aloso referenced as %s", value.c_str(), s.c_str());
    }
}

