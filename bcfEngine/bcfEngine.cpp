#include "pch.h"

#ifdef _WINDOWS
#define RDFBCF_EXPORT __declspec(dllexport) 
#else
#define RDFBCF_EXPORT 
#endif

#include "bcfEngine.h"

#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFProject* bcfOpenProject(void)
{
    return new BCFProject();
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void bcfCloseProject(BCFProject* project)
{
    if (project) {
        delete project;
    }
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfInitNew(BCFProject* project)
{
    if (project) {
        return project->InitNew();
    }
    else {
        return false;
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfReadFile(BCFProject* project, const char* bcfFilePath)
{
    if (project) {
        return project->Read(bcfFilePath);
    }
    else {
        return false;
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfWriteFile(BCFProject* project, const char* bcfFilePath, BCFVersion version)
{
    if (project) {
        return project->Write(bcfFilePath, version);
    }
    else {
        return false;
    }
}

