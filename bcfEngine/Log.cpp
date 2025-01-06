#include "pch.h"
#include "Log.h"

/// <summary>
/// 
/// </summary>
Log::~Log()
{
    //assert(m_messages.empty());
}

/// <summary>
/// 
/// </summary>
void Log::add(Level level, const char* code, const char* detailsFormat, ...)
{
    m_messages.push_back(Message());
    auto& message = m_messages.back();

    message.level = level;
    message.code.assign(code);

    if (detailsFormat) {
        char details[1024];
        va_list args;
        va_start(args, detailsFormat);
        vsprintf_s(details, detailsFormat, args);
        va_end(args);
        message.details.assign(details);
    }

#ifdef DEBUG
    printf("Add to log %s\n", message.ToString().c_str());
#endif // DEBUG
}

std::string Log::Message::ToString()
{
    std::string str;

    switch (level) {
        case Level::error: str.append("ERROR: "); break;
        case Level::warning: str.append("warning: "); break;
        default: assert(false); str.append("UNKNOWN");
    }

    str.append(code);
    str.push_back(':');
    str.push_back(' ');

    str.append(details);

    return str;
}


/// <summary>
/// 
/// </summary>
const char* Log::get(bool clean)
{
    m_buffer.clear();

    for (auto m : m_messages) {
        m_buffer.append(m.ToString());
        m_buffer.push_back('\n');
        m_buffer.push_back('\n');
    }

    if (clean) {
        m_messages.clear();
    }

    return m_buffer.c_str();
}

