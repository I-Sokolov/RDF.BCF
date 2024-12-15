#include "pch.h"

#include "BCFProject.h"

#include "Archivator.h"

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

}

/// <summary>
/// 
/// </summary>
bool BCFProject::InitNew()
{
    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Read(const char* bcfFilePath)
{
    Archivator ar(m_log);
    return true; // ar.Unpack(bcfFilePath, R"(C:\Users\igor\Downloads\Test)");
}

/// <summary>
/// 
/// </summary>
bool BCFProject::Write(const char* bcfFilePath, BCFVersion version)
{
    Archivator ar(m_log);
    return ar.Pack(R"(W:\DevArea\RDF\EBAPI\RDFGeomApi)", "test.zip");
}
