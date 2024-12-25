#pragma once

class Log;


class DocumentReference
{
public:
    DocumentReference(Log& log) : m_log(log) {}

    void Read(_xml::_element & elem) {}

private:
    Log& m_log;
};

