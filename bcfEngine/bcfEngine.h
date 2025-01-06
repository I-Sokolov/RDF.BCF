

#ifndef __RDF_LTF_BCF_ENGINE_H
#define __RDF_LTF_BCF_ENGINE_H


#include "bcfTypes.h"

#ifdef __cplusplus
extern "C" {
#endif

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFProject* bcfProjectCreate(const char* projectId = NULL);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT void bcfProjectDelete(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfErrorsGet(BCFProject* project, bool cleanLog);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfFileRead(BCFProject* project, const char* bcfFilePath);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfFileWrite(BCFProject* project, const char* bcfFilePath, BCFVersion version);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfSetEditor(BCFProject* project, const char* user, bool autoExtent);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfProjectIdGet(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfProjectNameGet(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfProjectNameSet(BCFProject* project, const char* name);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfEnumerationElementGet(BCFProject* project, BCFEnumeration enumeration, BCFIndex index);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfEnumerationElementAdd(BCFProject* project, BCFEnumeration enumeration, const char* element);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfEnumerationElementRemove(BCFProject* project, BCFEnumeration enumeration, const char* element);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFTopic* bcfTopicIterate(BCFProject* project, BCFTopic* prev);
    RDFBCF_EXPORT BCFTopic* bcfTopicCreate(BCFProject* project, const char* type, const char* title, const char* status, const char* guid);
    RDFBCF_EXPORT bool bcfTopicRemove(BCFTopic* topic);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfTopicGetGuid               (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetServerAssignedId   (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetTopicStatus        (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetTopicType          (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetTitle              (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetPriority           (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetCreationDate       (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetCreationAuthor     (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetModifiedDate       (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetModifiedAuthor     (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetDueDate            (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetAssignedTo         (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetDescription        (BCFTopic* topic);
    RDFBCF_EXPORT const char* bcfTopicGetStage              (BCFTopic* topic);
    RDFBCF_EXPORT int         bcfTopicGetIndex              (BCFTopic* topic);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfTopicSetServerAssignedId          (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetTopicStatus               (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetTopicType                 (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetTitle                     (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetPriority                  (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetDueDate                   (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetAssignedTo                (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetDescription               (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetStage                     (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT bool bcfTopicSetIndex                     (BCFTopic* topic, int val);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFComment* bcfCommentIterate (BCFTopic* topic, BCFComment* prev);
    RDFBCF_EXPORT BCFComment* bcfCommentCreate  (BCFTopic* topic, const char* guid);
    RDFBCF_EXPORT bool bcfCommentRemove         (BCFComment* comment);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT const char*   bcfCommentGetGuid             (BCFComment* comment);
    RDFBCF_EXPORT const char*   bcfCommentGetDate             (BCFComment* comment);
    RDFBCF_EXPORT const char*   bcfCommentGetAuthor           (BCFComment* comment);
    RDFBCF_EXPORT const char*   bcfCommentGetModifiedDate     (BCFComment* comment);
    RDFBCF_EXPORT const char*   bcfCommentGetModifiedAuthor   (BCFComment* comment);
    RDFBCF_EXPORT const char*   bcfCommentGetText             (BCFComment* comment);
    RDFBCF_EXPORT BCFViewPoint* bcfCommentGetViewPoint        (BCFComment* comment);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool bcfCommentSetText        (BCFComment* comment, const char* text);
    RDFBCF_EXPORT bool bcfCommentSetViewPoint   (BCFComment* comment, BCFViewPoint* viewPoint);

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

