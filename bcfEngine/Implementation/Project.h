#pragma once

#include "bcfAPI.h"
#include "Log.h"
#include "Version.h"
#include "ProjectInfo.h"
#include "Extensions.h"
#include "Documents.h"

struct Topic;

struct Project : public BCFProject
{
public:
    static long gProjectCounter;

public:
    Project(const char* projectId = NULL);
private:
    ~Project();

public:
    //BCFProject
    virtual bool Delete(void) override; 
    virtual bool ReadFile(const char* bcfFilePath, bool autofix) override;
    virtual bool WriteFile(const char* bcfFilePath, BCFVersion version) override;
    virtual bool SetAuthor(const char* user, bool autoExtentSchema) override { m_author = user; m_autoExtentSchema = autoExtentSchema; return true; }
    virtual const char* GetProjectId() override { return m_projectInfo.m_ProjectId.c_str(); }
    virtual const char* GetName() override { return m_projectInfo.m_Name.c_str(); }
    virtual bool SetName(const char* name) override { m_projectInfo.m_Name = name; return true; }
    virtual BCFTopic* TopicAdd(const char* type, const char* title, const char* status, const char* guid = NULL) override;
    virtual BCFTopic* TopicIterate(BCFTopic* prev) override;
    virtual BCFExtensions& GetExtensions() override { return GetExtensionsImpl(); }
    virtual const char* GetErrors(bool cleanLog) override { return GetLog().get(cleanLog); }

public:
    const char* GetAuthor() { return m_author.c_str(); }

    BCFVersion GetVersion() { return m_version.Get(); }

    bool GetAutoExtentSchema() { return m_autoExtentSchema; }

    Extensions& GetExtensionsImpl() { return m_extensions; }

    Documents& GetDocuments() { return m_documents; }

    Topic* TopicByGuid(const char* guid);

public: //internal
    Log& GetLog() { return m_log; }

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

    SetByGuid<Topic>  m_topics;

    StringList  m_workingFolders;
};

