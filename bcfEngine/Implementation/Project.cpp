#include "pch.h"
#include "Project.h"
#include "Topic.h"
#include "Archivator.h"
#include "FileSystem.h"

long Project::gProjectCounter = 0;

/// <summary>
/// 
/// </summary>
BCFProject* BCFProject::Create(const char* projectId)
{
    return new Project(projectId);
}

/// <summary>
/// 
/// </summary>
Project::Project(const char* projectId)
    : m_version(*this)
    , m_projectInfo(*this, projectId)
    , m_extensions (*this)
    , m_documents(*this)
    , m_autoExtentSchema(true)
    , m_topics(*this)
{
    gProjectCounter++;
}

/// <summary>
/// 
/// </summary>
Project::~Project()
{
    CleanWorkingFolders();
    gProjectCounter--;
}

/// <summary>
/// 
/// </summary>
bool Project::Delete(void)
{
    if (CleanWorkingFolders()) {
        delete this;
        return true;
    }
    else {
        return false;
    }
}

/// <summary>
/// 
/// </summary>
bool Project::CleanWorkingFolders(bool keepLast)
{
    StringList keep;
    if (keepLast && m_workingFolders.size()) {
        keep.push_back(m_workingFolders.back());
        m_workingFolders.pop_back();
    }

    bool ok = true;
    for (auto& folder : m_workingFolders) {
        bool removed = FileSystem::Remove(folder.c_str(), m_log);
        if (!removed) {
            keep.push_front(folder);
            ok = false;
        }
    }

    m_workingFolders.clear();
    std::swap(m_workingFolders, keep);

    return ok;
}

/// <summary>
/// 
/// </summary>
bool Project::ReadFile(const char* bcfFilePath, bool autofix)
{
    std::string bcfFolder;
    bool ok = FileSystem::CreateTempDir(bcfFolder, m_log);

    if (ok) {
        m_workingFolders.push_back(bcfFolder);

        Archivator ar(m_log);
        ok = ar.Unpack(bcfFilePath, bcfFolder.c_str());

        ok = ok && m_version.ReadFile(bcfFolder);
        ok = ok && m_projectInfo.ReadFile(bcfFolder);
        ok = ok && m_extensions.ReadFile(bcfFolder);
        ok = ok && m_documents.ReadFile(bcfFolder);
        
        ok = ok && ReadTopics(bcfFolder);
    }

    if (ok && autofix) {
        Validate(true);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool Project::Validate(bool fix)
{
    bool valid = true;
    valid = m_version.Validate(fix) && valid;
    valid = m_projectInfo.Validate(fix) && valid;
    valid = m_extensions.Validate(fix) && valid;
    valid = m_documents.Validate(fix) && valid;
    valid = m_topics.Validate(fix) && valid;
    return valid;
}

/// <summary>
/// 
/// </summary>
bool Project::WriteFile(const char* bcfFilePath, BCFVersion version)
{
    if (!Validate(false)) {
        return false;
    }

    m_version.Set(version);

    std::string bcfFolder;
    bool ok = FileSystem::CreateTempDir(bcfFolder, m_log);

    if (ok) {
        m_workingFolders.push_back(bcfFolder);

        ok = ok && WriteTopics(bcfFolder);

        ok = ok && m_version.WriteFile(bcfFolder);
        ok = ok && m_projectInfo.WriteFile(bcfFolder);
        ok = ok && m_extensions.WriteFile(bcfFolder);
        ok = ok && m_documents.WriteFile(bcfFolder);

        if (ok) {
            Archivator ar(m_log);
            ok = ar.Pack(bcfFolder.c_str(), bcfFilePath);
        }
    }

    //on successfull write we can free old working folders
    if (ok) {
        ok = CleanWorkingFolders(true);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool Project::ReadTopics(const std::string& bcfFolder)
{
    FileSystem::DirList elems;
    bool ok = FileSystem::GetDirContent(bcfFolder.c_str(), elems, m_log);

    for (auto& elem : elems) {
        if (ok && elem.folder) {
            if (GuidStr::IsGUIDValid(elem.name.c_str(), NULL)) {
                auto topic = new Topic(*this, &m_topics, elem.name.c_str());
                m_topics.Add(topic); 

                std::string topicFolder(bcfFolder);
                FileSystem::AddPath(topicFolder, elem.name.c_str());
                ok = ok && topic->ReadFile(topicFolder);
            }
        }
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool Project::WriteTopics(const std::string& bcfFolder)
{
    bool ok = true;

    for (auto topic : m_topics.Items()) {

        std::string topicFolder(bcfFolder);
        FileSystem::AddPath(topicFolder, topic->GetGuid());

        ok = ok && FileSystem::CreateDir(topicFolder.c_str(), GetLog());

        ok = ok && topic->WriteFile(topicFolder);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
BCFTopic* Project::TopicAdd(const char* type, const char* title, const char* status, const char* guid)
{
    auto topic = new Topic(*this, &m_topics, guid ? guid : "");

    if (topic) {

        bool ok = topic->SetTopicType(type);
        ok = ok && topic->SetTitle(title);
        ok = ok && topic->SetTopicStatus(status);

        if (!ok) {
            delete topic;
            topic = NULL;
        }
    }

    if (topic) {
        m_topics.Add(topic);
        return topic;
    }
    else {
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFTopic* Project::TopicIterate(BCFTopic* prev)
{ 
    return m_topics.GetNext((Topic*)prev); 
}

/// <summary>
/// 
/// </summary>
Topic* Project::TopicByGuid(const char* guid)
{
    return m_topics.FindByGuid(guid);
}
