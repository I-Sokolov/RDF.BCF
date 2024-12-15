#pragma once

#include "bcfEngine.h"
#include "Log.h"

struct BCFProject
{
public:
    BCFProject();
    ~BCFProject();

public:
    bool InitNew();
    bool Read(const char* bcfFilePath);
    bool Write(const char* bcfFilePath, BCFVersion version);
    bool Close();

private:
    bool InitializeEmpty();
    bool CheckInitialized();

private:
    Log         m_log;
    std::string m_workingFolder;
};

