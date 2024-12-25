#pragma once

class Log;

/// <summary>
/// 
/// </summary>
struct GUIDable
{
public:
    static std::string CreateNewGUID();

public:
    const char* Guid() { return m_Guid.c_str(); }

protected:
    GUIDable(const char* guid);

protected:
    std::string m_Guid;
};


/// <summary>
/// 
/// </summary>
struct GuidReference : public GUIDable
{
public:
    GuidReference(Log& log) : GUIDable(NULL), m_log(log) {}

    void Read(_xml::_element& elem);

private:
    Log& m_log;
};