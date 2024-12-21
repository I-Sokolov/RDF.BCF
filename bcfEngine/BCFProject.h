#pragma once

#include "bcfEngine.h"
#include "Log.h"
#include "Extensions.h"

struct BCFProject
{
public:
    BCFProject();
    ~BCFProject();

public:
    const char* GetErrors();
    void        ClearErrors();

    bool InitNew();
    bool Read(const char* bcfFilePath);
    bool Write(const char* bcfFilePath, BCFVersion version);
    bool Close();

    Extensions& GetExtensions() { return m_extensions; }

private:
    bool InitializeEmpty();
    bool CheckInitialized();

private:
    Log         m_log;
    Extensions  m_extensions;

    std::string m_bcfFolder;
};

