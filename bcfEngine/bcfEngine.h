

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
    RDFBCF_EXPORT BCFIndex bcfTopicsCount(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfTopicGuid(BCFProject* project, BCFIndex topic);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFIndex bcfTopicCreate(BCFProject* project, const char* type, const char* title, const char* status, const char* guid);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfTopicRemove(BCFProject* project, BCFIndex topic);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfTopicGetServerAssignedId(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetTopicStatus(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetTopicType(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetTitle(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetPriority(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetCreationDate(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetCreationAuthor(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetModifiedDate(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetModifiedAuthor(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetDueDate(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetAssignedTo(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetDescription(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT const char* bcfTopicGetStage(BCFProject* project, BCFIndex topic);
    RDFBCF_EXPORT int         bcfTopicGetIndex(BCFProject* project, BCFIndex topic);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool SetServerAssignedId(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetTopicType(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetTitle(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetPriority(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetDueDate(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetAssignedTo(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetDescription(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetStage(BCFProject* project, BCFIndex topic, const char* val);
    RDFBCF_EXPORT bool SetIndex(BCFProject* project, BCFIndex topic, int val);


#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

