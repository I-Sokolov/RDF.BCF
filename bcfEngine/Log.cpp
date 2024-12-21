#include "pch.h"
#include "Log.h"


/// <summary>
/// 
/// </summary>
void Log::error(const char* code, const char* detailsFormat, ...)
{
    char details[1024];
    va_list args;
    va_start(args, detailsFormat);
    vsprintf_s(details, detailsFormat, args);
    va_end(args);

    m_messages.push_front(Message());

    m_messages.front().code.assign(code);
    m_messages.front().details.assign(details);

#ifdef DEBUG
    prntf("\nERROR %s\n%s\n\n", code, details);

#endif // DEBUG
}


/// <summary>
/// 
/// </summary>
const char* Log::getMessages()
{
    m_buffer.clear();

    for (auto m : m_messages) {
        m_buffer.append(m.code);
        m_buffer.push_back('\n');
        m_buffer.append(m.details);
        m_buffer.push_back('\n');
        m_buffer.push_back('\n');
    }

    return m_buffer.c_str();
}

/// <summary>
/// 
/// </summary>
void Log::clear()
{
    m_messages.clear();
}
