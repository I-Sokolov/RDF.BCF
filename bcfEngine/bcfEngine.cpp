#include "pch.h"

#include "bcfEngine.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "BCFComment.h"
#include "BCFViewPoint.h"

/// <summary>
/// Macros to implement put/get attributes and iterate/remove objects
/// </summary>

typedef const char* tStr;
typedef int           tInt;
typedef BCFViewPoint* tViewPoint;

#define OBJ_GET_ATTR(VAL, OBJ, ATTR)                                                \
RDFBCF_EXPORT t##VAL bcf##OBJ##Get##ATTR (BCF##OBJ* obj)                            \
{                                                                                   \
    if (obj) {                                                                      \
       return obj->Get##ATTR ();                                                    \
    }                                                                               \
    return 0;                                                                       \
}

#define OBJ_SET_ATTR(VAL, OBJ, ATTR)                                                \
RDFBCF_EXPORT bool bcf  ##OBJ##Set##ATTR (BCF##OBJ* obj, t##VAL val)                \
{                                                                                   \
    if (obj) {                                                                      \
        return obj->Set##ATTR (val);                                                \
    }                                                                               \
    return false;                                                                   \
}

#define OBJ_ITERATE(OBJ, CONTAINER)                                                     \
RDFBCF_EXPORT BCF##OBJ* bcf##OBJ##Iterate(BCF##CONTAINER* container, BCF##OBJ* prev)    \
{                                                                                       \
    if (container) {                                                                    \
        return container->##OBJ##Iterate(prev);                                         \
    }                                                                                   \
    return NULL;                                                                        \
}                                                                                       \
                                                                                        \
RDFBCF_EXPORT bool bcf##OBJ##Remove(BCF##OBJ* obj)                                      \
{                                                                                       \
    if (obj) {                                                                          \
        return obj->Remove();                                                           \
    }                                                                                   \
    return false;                                                                       \
}


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
RDFBCF_EXPORT bool bcfSetAuthor(BCFProject* project, const char* user, bool autoExtent)
{
    if (project) {
        return project->SetAuthor(user, autoExtent);
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
OBJ_ITERATE(Topic, Project)
RDFBCF_EXPORT BCFTopic* bcfTopicCreate(BCFProject* project, const char* type, const char* title, const char* status, const char* guid)
{
    if (project) {
        return project->TopicCreate(type, title, status, guid);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
OBJ_GET_ATTR(Str, Topic, Guid)
OBJ_GET_ATTR(Str, Topic, ServerAssignedId)
OBJ_GET_ATTR(Str, Topic, TopicStatus)
OBJ_GET_ATTR(Str, Topic, TopicType)
OBJ_GET_ATTR(Str, Topic, Title)
OBJ_GET_ATTR(Str, Topic, Priority)
OBJ_GET_ATTR(Str, Topic, CreationDate)
OBJ_GET_ATTR(Str, Topic, CreationAuthor)
OBJ_GET_ATTR(Str, Topic, ModifiedDate)
OBJ_GET_ATTR(Str, Topic, ModifiedAuthor)
OBJ_GET_ATTR(Str, Topic, DueDate)
OBJ_GET_ATTR(Str, Topic, AssignedTo)
OBJ_GET_ATTR(Str, Topic, Description)
OBJ_GET_ATTR(Str, Topic, Stage)
OBJ_GET_ATTR(Int, Topic, Index)

OBJ_SET_ATTR(Str, Topic, ServerAssignedId)
OBJ_SET_ATTR(Str, Topic, TopicStatus)
OBJ_SET_ATTR(Str, Topic, TopicType)
OBJ_SET_ATTR(Str, Topic, Title)
OBJ_SET_ATTR(Str, Topic, Priority)
OBJ_SET_ATTR(Str, Topic, DueDate)
OBJ_SET_ATTR(Str, Topic, AssignedTo)
OBJ_SET_ATTR(Str, Topic, Description)
OBJ_SET_ATTR(Str, Topic, Stage)
OBJ_SET_ATTR(Int, Topic, Index)

/// <summary>
/// 
/// </summary>
OBJ_ITERATE(Comment, Topic)
RDFBCF_EXPORT BCFComment* bcfCommentCreate(BCFTopic* topic, const char* guid)
{
    if (topic) {
        return topic->CommentCreate(guid);
    }
    return 0;
}

/// <summary>
///
/// </summary>

OBJ_GET_ATTR(Str, Comment, Guid           )
OBJ_GET_ATTR(Str, Comment, Date           )
OBJ_GET_ATTR(Str, Comment, Author         )
OBJ_GET_ATTR(Str, Comment, ModifiedDate   )
OBJ_GET_ATTR(Str, Comment, ModifiedAuthor )
OBJ_GET_ATTR(Str, Comment, Text           )
OBJ_GET_ATTR(ViewPoint, Comment, ViewPoint)

OBJ_SET_ATTR(Str,       Comment, Text)
OBJ_SET_ATTR(ViewPoint, Comment, ViewPoint)

/// <summary>
/// 
/// </summary>
OBJ_ITERATE(ViewPoint, Topic)
RDFBCF_EXPORT BCFViewPoint* bcfViewPointCreate(BCFTopic* topic, const char* guid)
{
    if (topic) {
        return topic->ViewPointCreate(guid);
    }
    return 0;
}
