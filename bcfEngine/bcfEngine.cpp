#include "pch.h"

#ifdef _WINDOWS
#define RDFBCF_EXPORT __declspec(dllexport) 
#else
#define RDFBCF_EXPORT 
#endif

#include "bcfEngine.h"
#include "BCFProject.h"
#include "Topic.h"

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFProject* bcfCreateProject(const char* currentUser, bool autoExtent, const char* projectId)
{
    return new BCFProject(currentUser, autoExtent, projectId);
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void bcfDeleteProject(BCFProject* project)
{
    if (project) {
        assert(!*project->GetErrors());
        delete project;
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project, bool cleanLog)
{
    if (project) {
        return project->GetErrors(cleanLog);
    }
    return "NULL ARGUMENT";
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfReadFile(BCFProject* project, const char* bcfFilePath)
{
    if (project) {
        return project->Read(bcfFilePath);
    }
    return false;
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


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetProjectId(BCFProject* project)
{
    if (project) {
        return project->GetGUID();
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetProjectName(BCFProject* project)
{
    if (project) {
        return project->GetName();
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfSetProjectName(BCFProject* project, const char* name)
{
    if (project) {
        project->SetName(name);
        return true;
    }
    return false;
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetEnumerationElement(BCFProject* project, BCFEnumeration enumeration, BCFIndex index)
{
    if (project) {
        return project->GetExtensions().GetElement(enumeration, index);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfAddEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element)
{
    if (project) {
        return project->GetExtensions().AddElement(enumeration, element);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfRemoveEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element)
{
    if (project) {
        return project->GetExtensions().RemoveElement(enumeration, element);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFIndex bcfGetTopicsCount(BCFProject* project)
{
    if (project) {
        return project->GetTopicsCount();
    }
    return 0;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetTopicGUID(BCFProject* project, BCFIndex topic)
{
    if (project) {
        if (auto pt = project->GetTopic(topic)) {
            return pt->GUID();
        }
    }
    return NULL;
}
