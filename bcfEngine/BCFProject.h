#pragma once

#include "bcfEngine.h"
#include "Log.h"
#include "Version.h"
#include "ProjectInfo.h"
#include "Extensions.h"
#include "Topic.h"

struct BCFProject
{
private:
    BCFProject(const BCFProject&) = delete;
    void operator=(const BCFProject&) = delete;

public:
    BCFProject(const char* projectId = NULL);
    ~BCFProject();

public:
    Log& log() { return m_log; }

    bool Read(const char* bcfFilePath);
    bool Write(const char* bcfFilePath, BCFVersion version);

    bool SetEditor(const char* user, bool autoExtentSchema) { m_editor = user; m_autoExtentSchema = autoExtentSchema; return true; }
    const char* GetEditor() { return m_editor.c_str(); }
    bool GetAutoExtentSchema() { return m_autoExtentSchema; }

    const char* ProjectId() { return m_projectInfo.m_ProjectId.c_str(); }
    const char* GetName() { return m_projectInfo.m_Name.c_str(); }
    void SetName(const char* name) { m_projectInfo.m_Name = name; }

    Extensions& GetExtensions() { return m_extensions; }

    BCFIndex TopicsCount() { return (BCFIndex) m_topics.size(); }
    Topic* GetTopic(BCFIndex index); //do not delete, valid until project destroyed or TopicRemove
    BCFIndex TopicCreate(const char* type, const char* title, const char* status, const char* guid = NULL);
    bool TopicRemove(BCFIndex index);

private:
    bool ReadTopics(const std::string& bcfFolder);
    bool WriteTopics(const std::string& bcfFolder);


private:
    Log         m_log;
    
    Version     m_version;
    ProjectInfo m_projectInfo;
    Extensions  m_extensions;

    std::string m_editor;
    bool        m_autoExtentSchema;

    GuidList<Topic>  m_topics;
};

