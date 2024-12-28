#include "pch.h"

#include "bcfEngine.h"
#include "BCFProject.h"
#include "Topic.h"

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFProject* bcfProjectCreate(const char* projectId)
{
    return new BCFProject(projectId);
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void bcfProjectDelete(BCFProject* project)
{
    if (project) {
        delete project;
    }
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfErrorsGet(BCFProject* project, bool cleanLog)
{
    if (project) {
        return project->log().get(cleanLog);
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
RDFBCF_EXPORT bool bcfSetEditor(BCFProject* project, const char* user, bool autoExtent)
{
    if (project) {
        return project->SetEditor(user, autoExtent);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfProjectIdGet(BCFProject* project)
{
    if (project) {
        return project->ProjectId();
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
            return pt->Guid();
        }
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFIndex bcfTopicCreate(BCFProject* project, const char* type, const char* title, const char* status, const char* guid)
{
    if (project) {
        return project->TopicCreate(type, title, status, guid);
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

/// <summary>
/// 
/// </summary>

#define TOPIC_GET_ATTR(ATTR)                                                        \
RDFBCF_EXPORT const char* bcfTopicGet##ATTR (BCFProject* project, BCFIndex topic)   \
{                                                                                   \
    if (project) {                                                                  \
        if (auto t = project->GetTopic(topic)) {                                    \
            return t->Get##ATTR ();                                                 \
        }                                                                           \
    }                                                                               \
    return NULL;                                                                    \
}

TOPIC_GET_ATTR(ServerAssignedId)
TOPIC_GET_ATTR(TopicStatus)
TOPIC_GET_ATTR(TopicType)
TOPIC_GET_ATTR(Title)
TOPIC_GET_ATTR(Priority)
TOPIC_GET_ATTR(CreationDate)
TOPIC_GET_ATTR(CreationAuthor)
TOPIC_GET_ATTR(ModifiedDate)
TOPIC_GET_ATTR(ModifiedAuthor)
TOPIC_GET_ATTR(DueDate)
TOPIC_GET_ATTR(AssignedTo)
TOPIC_GET_ATTR(Description)
TOPIC_GET_ATTR(Stage)

RDFBCF_EXPORT int         bcfTopicGetIndex(BCFProject* project, BCFIndex topic)
{
    if (project) {
        if (auto t = project->GetTopic(topic)) {
            return t->GetIndex();
        }
    }
    return 0;
}


/// <summary>
/// 
/// </summary>
#define TOPIC_SET_ATTR(ATTR)                                                        \
RDFBCF_EXPORT bool bcfTopicSet##ATTR (BCFProject* project, BCFIndex topic, const char* val)\
{                                                                                   \
    if (project) {                                                                  \
        if (auto t = project->GetTopic(topic)) {                                    \
            return t->Set##ATTR (val);                                              \
        }                                                                           \
    }                                                                               \
    return false;                                                                   \
}

TOPIC_SET_ATTR(ServerAssignedId)
TOPIC_SET_ATTR(TopicType)
TOPIC_SET_ATTR(Title)
TOPIC_SET_ATTR(Priority)
TOPIC_SET_ATTR(DueDate)
TOPIC_SET_ATTR(AssignedTo)
TOPIC_SET_ATTR(Description)
TOPIC_SET_ATTR(Stage)

RDFBCF_EXPORT bool SetIndex(BCFProject* project, BCFIndex topic, int val)
{
    if (project) {
        if (auto t = project->GetTopic(topic)) {
            return t->SetIndex(val);
        }
    }
    return false;
}
