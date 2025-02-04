#pragma once

class Log
{
private:
    Log(const Log&) = delete;
    void operator=(const Log&) = delete;

public:
    enum class Level { error, warning };

public:
    Log() {}
    ~Log();

    void add(Level level, const char* code, const char* detailsFormat = NULL, ...);

    const char* get(bool clean);

private:
    struct Message
    {
        std::string ToString();

        Level       level;
        std::string code;
        std::string details;
    };

private:
    std::list<Message>   m_messages;
    std::string          m_buffer;
};

