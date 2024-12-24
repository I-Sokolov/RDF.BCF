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
RDFBCF_EXPORT BCFProject* bcfProjectCreate(const char* currentUser, bool autoExtent, const char* projectId)
{
    return new BCFProject(currentUser, autoExtent, projectId);
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void bcfProjectDelete(BCFProject* project)
{
    if (project) {
        assert(!*project->ErrorsGet());
        delete project;
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfErrorsGet(BCFProject* project, bool cleanLog)
{
    if (project) {
        return project->ErrorsGet(cleanLog);
    }
    return "NULL ARGUMENT";
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfFileRead(BCFProject* project, const char* bcfFilePath)
{
    if (project) {
        return project->Read(bcfFilePath);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfFileWrite(BCFProject* project, const char* bcfFilePath, BCFVersion version)
{
    if (project) {
        return project->Write(bcfFilePath, version);
    }
    return false;
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfProjectIdGet(BCFProject* project)
{
    if (project) {
        return project->GetGUID();
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfProjectNameGet(BCFProject* project)
{
    if (project) {
        return project->GetName();
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfProjectNameSet(BCFProject* project, const char* name)
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
RDFBCF_EXPORT const char* bcfEnumerationElementGet(BCFProject* project, BCFEnumeration enumeration, BCFIndex index)
{
    if (project) {
        return project->GetExtensions().GetElement(enumeration, index);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfEnumerationElementAdd(BCFProject* project, BCFEnumeration enumeration, const char* element)
{
    if (project) {
        return project->GetExtensions().AddElement(enumeration, element);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfEnumerationElementRemove(BCFProject* project, BCFEnumeration enumeration, const char* element)
{
    if (project) {
        return project->GetExtensions().RemoveElement(enumeration, element);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFIndex bcfTopicsCount(BCFProject* project)
{
    if (project) {
        return project->TopicsCount();
    }
    return 0;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfTopicGuid(BCFProject* project, BCFIndex topic)
{
    if (project) {
        if (auto pt = project->GetTopic(topic)) {
            return pt->GUID();
        }
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFIndex bcfTopicCreate(BCFProject* project, const char* guid)
{
    if (project) {
        return project->TopicCreate(guid);
    }
    return BCFIndex_ERROR;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfTopicRemove(BCFProject* project, BCFIndex topic)
{
    if (project) {
        return project->TopicRemove(topic);
    }
    return false;
}
