#pragma once

class Log;


class ViewPoint
{
public:
    ViewPoint(Log& log) : m_log(log) {}

    void Read(_xml::_element & elem) {}

private:
    Log& m_log;
};

