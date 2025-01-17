#include "pch.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "Archivator.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFProject::BCFProject(const char* projectId)
    : m_version(*this)
    , m_projectInfo(*this, projectId)
    , m_extensions (*this)
    , m_autoExtentSchema(true)
    , m_topics(*this)
{
    BCFObject::gObjectCounter++;
}

/// <summary>
/// 
/// </summary>
BCFProject::~BCFProject()
{
    CleanWorkingFolders();
    BCFObject::gObjectCounter--;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Delete(void)
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
bool BCFProject::CleanWorkingFolders(bool keepLast)
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
bool BCFProject::Read(const char* bcfFilePath)
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
        
        ok = ok && ReadTopics(bcfFolder);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Write(const char* bcfFilePath, BCFVersion version)
{
    if (m_projectInfo.m_ProjectId.empty()) {
        log().add(Log::Level::warning, "Invalid value", "ProjectId must be set");
        m_projectInfo.m_ProjectId = GuidStr::New();
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
bool BCFProject::ReadTopics(const std::string& bcfFolder)
{
    FileSystem::DirList elems;
    bool ok = FileSystem::GetDirContent(bcfFolder.c_str(), elems, m_log);

    for (auto& elem : elems) {
        if (ok && elem.folder) {
            if (GuidStr::IsGUIDValid(elem.name.c_str(), NULL)) {
                auto topic = new BCFTopic(*this, &m_topics, elem.name.c_str());
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
bool BCFProject::WriteTopics(const std::string& bcfFolder)
{
    bool ok = true;

    for (auto topic : m_topics.Items()) {

        std::string topicFolder(bcfFolder);
        FileSystem::AddPath(topicFolder, topic->GetGuid());

        ok = ok && FileSystem::CreateDir(topicFolder.c_str(), log());

        ok = ok && topic->WriteFile(topicFolder);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
BCFTopic* BCFProject::TopicAdd(const char* type, const char* title, const char* status, const char* guid)
{
    auto topic = new BCFTopic(*this, &m_topics, guid ? guid : "");

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
BCFTopic* BCFProject::TopicIterate(BCFTopic* prev)
{ 
    return m_topics.GetNext(prev); 
}

/// <summary>
/// 
/// </summary>
BCFTopic* BCFProject::TopicByGuid(const char* guid)
{
    return m_topics.FindByGuid(guid);
}
