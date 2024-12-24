#include "pch.h"
#include "BCFProject.h"

#include "Archivator.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFProject::Topics::~Topics()
{
    for (auto t : *this) {
        delete t;
    }
    clear();
}

/// <summary>
/// 
/// </summary>
void BCFProject::Topics::push_back(Topic* topic)
{
    if (*topic->GUID()) {
        for (auto it = begin(); it != end(); it++) {
            if (0 == strcmp(topic->GUID(), (*it)->GUID())) {
                delete (*it);
                erase(it);
                break;
            }
        }
    }

    __super::push_back(topic);
}

/// <summary>
/// 
/// </summary>
BCFProject::BCFProject(const char* currentUser, bool autoExtent, const char* projectId)
    : m_version(m_log)
    , m_projectInfo(m_log)
    , m_extensions (m_log)
{
}

/// <summary>
/// 
/// </summary>
BCFProject::~BCFProject()
{
}

/// <summary>
/// 
/// </summary>
const char* BCFProject::GetErrors(bool cleanLog)
{
    return m_log.get(cleanLog);
}


/// <summary>
/// 
/// </summary>
bool BCFProject::Read(const char* bcfFilePath)
{
    std::string bcfFolder;
    bool ok = FileSystem::CreateTempDir(bcfFolder, m_log);

    if (ok) {
        
        Archivator ar(m_log);
        ok = ar.Unpack(bcfFilePath, bcfFolder.c_str());

        ok = ok && m_version.Read(bcfFolder);
        ok = ok && m_projectInfo.Read(bcfFolder);
        ok = ok && m_extensions.Read(bcfFolder);
        
        ok = ok && ReadTopics(bcfFolder);

        bool removed = FileSystem::Remove(bcfFolder.c_str(), m_log);
        ok = ok && removed;
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Write(const char* bcfFilePath, BCFVersion version)
{
    if (m_projectInfo.m_ProjectId.empty()) {
        m_projectInfo.m_ProjectId = GUIDable::CreateNewGUID();
    }

    m_version.Set(version);

    std::string bcfFolder;
    bool ok = FileSystem::CreateTempDir(bcfFolder, m_log);

    if (ok) {
        ok = ok && WriteTopics(bcfFolder);

        ok = ok && m_version.Write(bcfFolder);
        ok = ok && m_version.Write(bcfFolder);
        ok = ok && m_extensions.Write(bcfFolder);

        if (ok) {
            Archivator ar(m_log);
            ok = ar.Pack(bcfFolder.c_str(), bcfFilePath);
        }

        bool removed = FileSystem::Remove(bcfFolder.c_str(), m_log);
        ok = ok && removed;
    }

    return ok;
}

void push_back(Topic* topic);

/// <summary>
/// 
/// </summary>
bool BCFProject::ReadTopics(const std::string& bcfFolder)
{
    FileSystem::DirList elems;
    bool ok = FileSystem::GetDirContent(bcfFolder.c_str(), elems, m_log);

    for (auto& elem : elems) {
        if (ok && elem.folder) {
            auto topic = new Topic(*this, elem.name.c_str());
            m_topics.push_back(topic);
            ok = ok && topic->Read(bcfFolder);
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

    for (auto topic : m_topics) {
        ok = ok && topic->Write(bcfFolder);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
Topic* BCFProject::GetTopic(BCFIndex index)
{
    if (index < m_topics.size()) {
        return m_topics[index];
    }
    else {
        m_log.add(Log::Level::error, "Index is out of range", "Index %d is out of topics range [0..%d]", 0, (int)m_topics.size());
        return NULL;
    }
}
