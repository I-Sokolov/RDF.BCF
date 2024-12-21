#pragma once

class Log
{
public:
    void error(const char* code, const char* detailsFormat, ...);
    const char* getMessages();
    void clear();

private:
    struct Message
    {
        std::string code;
        std::string details;
    };
    std::list<Message>   m_messages;
    std::string          m_buffer;
};

