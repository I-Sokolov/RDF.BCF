#pragma once

struct GUIDable
{
public:
    static std::string CreateNewGUID();

public:
    const char* GUID() { return m_guid.c_str(); }

protected:
    GUIDable(const char* guid);

private:
    std::string m_guid;
};

