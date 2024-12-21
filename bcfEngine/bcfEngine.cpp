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
RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project)
{
    if (project) {
        return project->GetErrors();
    }
    else {
        return "NULL ARGUMENT";
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void bcfClearErrors(BCFProject* project)
{
    if (project) {
        project->ClearErrors();
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfCloseProject(BCFProject* project)
{
    if (project) {
        if (project->Close()) {
            delete project;
            return true;
        }
    }
    return false;
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfInitNew(BCFProject* project)
{
    if (project) {
        return project->InitNew();
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfReadFile(BCFProject* project, const char* bcfFilePath)
{
    bool res = false;
    if (project) {
        res = project->Read(bcfFilePath);
    }
    return res;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfWriteFile(BCFProject* project, const char* bcfFilePath, BCFVersion version)
{
    if (project) {
        return project->Write(bcfFilePath, version);
    }
    return false;
}

