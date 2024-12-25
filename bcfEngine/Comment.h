#pragma once

class Log;


class Comment
{
public:
    Comment(Log& log) : m_log(log) {}

    void Read(_xml::_element& elem) {};

private:
    Log& m_log;
};

