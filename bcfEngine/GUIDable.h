#pragma once

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

