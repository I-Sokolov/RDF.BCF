#pragma once

#include "bcfEngine.h"
#include "Log.h"
#include "Version.h"
#include "ProjectInfo.h"
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

    const char* GetGUID() { return m_projectInfo.m_ProjectId.c_str(); }
    const char* GetName() { return m_projectInfo.m_Name.c_str(); }
    void SetName(const char* name) { m_projectInfo.m_Name = name; }

    Extensions& GetExtensions() { return m_extensions; }

private:
    bool InitializeEmpty();
    bool CheckInitialized();

private:
    Log         m_log;
    
    Version     m_version;
    ProjectInfo m_projectInfo;
    Extensions  m_extensions;

    std::string m_bcfFolder;
};

