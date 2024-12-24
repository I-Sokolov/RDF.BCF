#pragma once

class Log;

class BIMFile
{
public:
    BIMFile (Log& log) : m_log(log) {}

    void Read(_xml::_element& elem);

private:
    Log&        m_log;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
};

