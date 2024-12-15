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
    FileSystem fs(m_log);
    return fs.CreateTempDir(m_workingFolder);
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
        FileSystem fs(m_log);
        ok = fs.Remove(m_workingFolder.c_str());
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
    return true; // ar.Unpack(bcfFilePath, R"(C:\Users\igor\Downloads\Test)");
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
