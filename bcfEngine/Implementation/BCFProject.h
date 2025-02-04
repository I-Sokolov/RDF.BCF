#pragma once

#include "bcfTypes.h"
#include "Log.h"
#include "Version.h"
#include "ProjectInfo.h"
#include "Extensions.h"
#include "Documents.h"

struct BCFTopic;

struct BCFProject
{
public:
    static long gProjectCounter;

private:
    BCFProject(const BCFProject&) = delete;
    void operator=(const BCFProject&) = delete;

public:
    BCFProject(const char* projectId = NULL);
    bool Delete(void); //if failed you can still use it to request errors

private:
    ~BCFProject();

public:
    bool Read(const char* bcfFilePath, bool autofix);
    bool Write(const char* bcfFilePath, BCFVersion version);

    BCFVersion GetVersion() { return m_version.Get(); }

    bool SetAuthor(const char* user, bool autoExtentSchema) { m_author = user; m_autoExtentSchema = autoExtentSchema; return true; }
    const char* GetAuthor() { return m_author.c_str(); }
    bool GetAutoExtentSchema() { return m_autoExtentSchema; }

    const char* ProjectId() { return m_projectInfo.m_ProjectId.c_str(); }
    const char* GetName() { return m_projectInfo.m_Name.c_str(); }
    void SetName(const char* name) { m_projectInfo.m_Name = name; }

    Extensions& GetExtensions() { return m_extensions; }
    Documents& GetDocuments() { return m_documents; }

    BCFTopic* TopicAdd(const char* type, const char* title, const char* status, const char* guid = NULL);
    BCFTopic* TopicIterate(BCFTopic* prev);
    BCFTopic* TopicByGuid(const char* guid);

public: //internal
    Log& log() { return m_log; }

private:
    bool Validate(bool fix);
    bool ReadTopics(const std::string& bcfFolder);
    bool WriteTopics(const std::string& bcfFolder);
    bool CleanWorkingFolders(bool keepLast = false);

private:
    Log         m_log;
    
    Version     m_version;
    ProjectInfo m_projectInfo;
    Extensions  m_extensions;
    Documents   m_documents;

    std::string m_author;
    bool        m_autoExtentSchema;

    SetByGuid<BCFTopic>  m_topics;

    StringList  m_workingFolders;
};

