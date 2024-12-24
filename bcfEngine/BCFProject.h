#pragma once

#include "bcfEngine.h"
#include "Log.h"
#include "Version.h"
#include "ProjectInfo.h"
#include "Extensions.h"
#include "Topic.h"

struct BCFProject
{
public:
    BCFProject(const char* currentUser, bool autoExtent, const char* projectId);
    ~BCFProject();

public:
    const char* ErrorsGet(bool cleanLog = true);

    bool Read(const char* bcfFilePath);
    bool Write(const char* bcfFilePath, BCFVersion version);

    const char* GetGUID() { return m_projectInfo.m_ProjectId.c_str(); }
    const char* GetName() { return m_projectInfo.m_Name.c_str(); }
    void SetName(const char* name) { m_projectInfo.m_Name = name; }

    Extensions& GetExtensions() { return m_extensions; }

    BCFIndex TopicsCount() { return (BCFIndex) m_topics.size(); }
    Topic* GetTopic(BCFIndex index);
    BCFIndex TopicCreate(const char* guid);
    bool TopicRemove(BCFIndex index);

private:
    bool ReadTopics(const std::string& bcfFolder);
    bool WriteTopics(const std::string& bcfFolder);

private:
    struct Topics : std::vector<Topic*>
    {
        ~Topics();
        void push_back(Topic* topic);
    };

private:
    Log         m_log;
    
    Version     m_version;
    ProjectInfo m_projectInfo;
    Extensions  m_extensions;

    Topics      m_topics;
};

