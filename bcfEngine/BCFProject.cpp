#include "pch.h"
#include "BCFProject.h"

#include "Archivator.h"
#include "FileSystem.h"

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
    for (auto t : m_topics) {
        delete t;
    }
    m_topics.clear();
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
    m_version.Set(version);

    std::string bcfFolder;
    bool ok = FileSystem::CreateTempDir(bcfFolder, m_log);

    if (ok) {
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
