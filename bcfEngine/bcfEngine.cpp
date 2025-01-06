#include "pch.h"

#include "bcfEngine.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "BCFComment.h"

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
RDFBCF_EXPORT BCFTopic* bcfTopicIterate(BCFProject* project, BCFTopic* prev)
{
    if (project) {
        return project->TopicIterate(prev);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
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
RDFBCF_EXPORT bool bcfTopicRemove(BCFTopic* topic)
{
    if (topic) {
        return topic->Remove();
    }
    return false;
}

/// <summary>
/// 
/// </summary>

#define TOPIC_GET_ATTR(ATTR)                                                        \
RDFBCF_EXPORT const char* bcfTopicGet##ATTR (BCFTopic* topic)                       \
{                                                                                   \
    if (topic) {                                                                    \
       return topic->Get##ATTR ();                                                  \
    }                                                                               \
    return NULL;                                                                    \
}

TOPIC_GET_ATTR(Guid)
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

RDFBCF_EXPORT int         bcfTopicGetIndex(BCFTopic* topic)
{
    if (topic) {
        return topic->GetIndex();
    }
    return 0;
}


/// <summary>
/// 
/// </summary>
#define TOPIC_SET_ATTR(ATTR)                                                        \
RDFBCF_EXPORT bool bcfTopicSet##ATTR (BCFTopic* topic, const char* val)             \
{                                                                                   \
    if (topic) {                                                                    \
        return topic->Set##ATTR (val);                                              \
    }                                                                               \
    return false;                                                                   \
}

TOPIC_SET_ATTR(ServerAssignedId)
TOPIC_SET_ATTR(TopicStatus)
TOPIC_SET_ATTR(TopicType)
TOPIC_SET_ATTR(Title)
TOPIC_SET_ATTR(Priority)
TOPIC_SET_ATTR(DueDate)
TOPIC_SET_ATTR(AssignedTo)
TOPIC_SET_ATTR(Description)
TOPIC_SET_ATTR(Stage)

RDFBCF_EXPORT bool bcfTopicSetIndex(BCFTopic* topic, int val)
{
    if (topic) {
        return topic->SetIndex(val);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFComment* bcfCommentIterate(BCFTopic* topic, BCFComment* prev)
{
    if (topic) {
        return topic->CommentIterate(prev);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
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
RDFBCF_EXPORT bool bcfCommentRemove(BCFComment* comment)
{
    if (comment) {
        return comment->Remove();
    }
    return false;
}

/// <summary>
///
/// </summary>
#define COMMENT_GET_ATTR(ATTR)                                                      \
RDFBCF_EXPORT const char* bcfCommentGet##ATTR (BCFComment*  comment)                \
{                                                                                   \
    if (comment) {                                                                  \
        return comment->Get##ATTR();                                                \
    }                                                                               \
    return NULL;                                                                    \
}


COMMENT_GET_ATTR(Guid           )
COMMENT_GET_ATTR(Date           )
COMMENT_GET_ATTR(Author         )
COMMENT_GET_ATTR(ModifiedDate   )
COMMENT_GET_ATTR(ModifiedAuthor )
COMMENT_GET_ATTR(Text           )

/// <summary>
///
/// </summary>
RDFBCF_EXPORT BCFViewPoint* bcfCommentGetViewPoint(BCFComment* comment)
{
    if (comment) {
        return comment->GetViewPoint();
    }
    return NULL;
}

/// <summary>
///
/// </summary>
RDFBCF_EXPORT bool bcfCommentSetText(BCFComment* comment, const char* text)
{
    if (comment) {
        return comment->SetText(text);
    }
    return false;
}

RDFBCF_EXPORT bool bcfCommentSetViewPoint(BCFComment* comment, BCFViewPoint* viewPoint)
{
    if (comment) {
        return comment->SetViewPoint(viewPoint);
    }
    return false;
}
