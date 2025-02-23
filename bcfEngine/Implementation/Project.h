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
    virtual bool SetOptions(const char* user, bool autoExtentSchema, bool validateIfcGuids) override { if (user) m_author.assign(user); m_autoExtentSchema = autoExtentSchema; m_validateIfcGuids = validateIfcGuids;  return true; }
    virtual const char* GetProjectId() override { return m_projectInfo.GetProjectId(); }
    virtual const char* GetName() override { return m_projectInfo.GetName(); }
    virtual bool SetName(const char* val) override { return m_projectInfo.SetName(val); }
    virtual BCFTopic* AddTopic(const char* type, const char* title, const char* status, const char* guid = NULL) override;
    virtual BCFTopic* GetTopic(uint16_t ind) override;
    virtual BCFExtensions& GetExtensions() override { return GetExtensions_(); }
    virtual const char* GetErrors(bool cleanLog) override { return Log_().get(cleanLog); }
    virtual bool IsDirty() override { return m_isDirty; }

public:
    const char* GetAuthor() { return m_author.c_str(); }

    BCFVersion GetVersion() { return m_version.Get(); }

    bool GetAutoExtentSchema() { return m_autoExtentSchema; }
    bool GetValidateIfcGuids() { return m_validateIfcGuids; }

    Extensions& GetExtensions_() { return m_extensions; }

    Documents& GetDocuments() { return m_documents; }

    Topic* TopicByGuid(const char* guid);

    void MarkDirty() { m_isDirty = true; }

public: //internal
    Log& Log_() { return m_log; }

private:
    bool Validate(bool fix);
    bool ReadTopics(const std::string& bcfFolder);
    bool WriteTopics(const std::string& bcfFolder);
    bool CleanWorkingFolders(bool keepLast = false);

private:
    Log         m_log;
    StringList  m_workingFolders;
    bool        m_isDirty;

    Version     m_version;
    ProjectInfo m_projectInfo;
    Extensions  m_extensions;
    Documents   m_documents;

    std::string m_author;
    bool        m_autoExtentSchema;
    bool        m_validateIfcGuids;

    SetByGuid<Topic>  m_topics;

};

