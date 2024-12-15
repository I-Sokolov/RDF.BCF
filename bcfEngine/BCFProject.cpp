#include "pch.h"
#include "BCFProject.h"

#include "Archivator.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFProject::BCFProject()
{
}

/// <summary>
/// 
/// </summary>
BCFProject::~BCFProject()
{
    Close();
}

/// <summary>
/// 
/// </summary>
bool BCFProject::InitializeEmpty()
{
    return FileSystem::CreateTempDir(m_workingFolder, m_log);
}

/// <summary>
/// 
/// </summary>
bool BCFProject::CheckInitialized()
{
    if (m_workingFolder.empty()) {
        m_log.error("Not initialized", "Init new or read BCF file before usage");
        return false;
    }
    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Close()
{
    bool ok = true;
    if (!m_workingFolder.empty()) {
        ok = FileSystem::Remove(m_workingFolder.c_str(), m_log);
        m_workingFolder.clear();
    }
    return ok;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::InitNew()
{
    return InitializeEmpty();
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Read(const char* bcfFilePath)
{
    if (!InitializeEmpty()) {
        return false;
    }

    Archivator ar(m_log);
    return ar.Unpack("test.zip", m_workingFolder.c_str());
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Write(const char* bcfFilePath, BCFVersion version)
{
    if (!CheckInitialized()) {
        return false;
    }

    Archivator ar(m_log);
    return ar.Pack(R"(W:\DevArea\RDF\EBAPI\RDFGeomApi)", "test.zip");
}
