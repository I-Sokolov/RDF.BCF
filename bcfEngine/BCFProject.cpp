#include "pch.h"
#include "BCFProject.h"

#include "Archivator.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFProject::BCFProject()
    : m_extensions (m_log)
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
const char* BCFProject::GetErrors()
{
    return m_log.getMessages();
}

/// <summary>
/// 
/// </summary>
void BCFProject::ClearErrors()
{
    m_log.clear();
}

/// <summary>
/// 
/// </summary>
bool BCFProject::InitializeEmpty()
{
    return FileSystem::CreateTempDir(m_bcfFolder, m_log);
}

/// <summary>
/// 
/// </summary>
bool BCFProject::CheckInitialized()
{
    if (m_bcfFolder.empty()) {
        m_log.add(Log::Level::error, "Not initialized", "Init new or read BCF file before usage");
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
    if (!m_bcfFolder.empty()) {
        ok = FileSystem::Remove(m_bcfFolder.c_str(), m_log);
        m_bcfFolder.clear();
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
    bool ok = ar.Unpack(bcfFilePath, m_bcfFolder.c_str());

    if (ok) {
        ok = m_extensions.Read(m_bcfFolder);
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Write(const char* bcfFilePath, BCFVersion version)
{
    if (!CheckInitialized()) {
        return false;
    }

    bool ok = m_extensions.Write(m_bcfFolder);

    if (ok) {
        Archivator ar(m_log);
        ok = ar.Pack(R"(W:\DevArea\RDF\EBAPI\RDFGeomApi)", "test.zip");
    }

    return ok;
}
