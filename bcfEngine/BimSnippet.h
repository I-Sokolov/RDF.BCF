#pragma once

class Log;


class BimSnippet
{
public:
    BimSnippet(Log& log) : m_log(log) {}

    void Read(_xml::_element & elem) {}

private:
    Log& m_log;
};

