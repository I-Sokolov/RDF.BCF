

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
    RDFBCF_EXPORT bool bcfProjectDelete(BCFProject* project);

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
    RDFBCF_EXPORT bool bcfSetAuthor(BCFProject* project, const char* user, bool autoExtent);

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
    RDFBCF_EXPORT BCFTopic* bcfTopicAdd(BCFProject* project, const char* type, const char* title, const char* status, const char* guid);
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
    RDFBCF_EXPORT BCFFile* bcfFileIterate(BCFTopic* topic, BCFFile* prev);
    RDFBCF_EXPORT BCFFile* bcfFileAdd(BCFTopic* topic, const char* filePath, bool isExternal);
    RDFBCF_EXPORT bool bcfFileRemove(BCFFile* file);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool        bcfFileGetIsExternal                   (BCFFile* file);
    RDFBCF_EXPORT const char* bcfFileGetFilename                     (BCFFile* file);
    RDFBCF_EXPORT const char* bcfFileGetDate                         (BCFFile* file);
    RDFBCF_EXPORT const char* bcfFileGetReference                    (BCFFile* file);
    RDFBCF_EXPORT const char* bcfFileGetIfcProject                   (BCFFile* file);
    RDFBCF_EXPORT const char* bcfFileGetIfcSpatialStructureElement   (BCFFile* file);

    RDFBCF_EXPORT bool bcfFileSetIsExternal                   (BCFFile* file, bool        val);
    RDFBCF_EXPORT bool bcfFileSetFilename                     (BCFFile* file, const char* val);
    RDFBCF_EXPORT bool bcfFileSetDate                         (BCFFile* file, const char* val);
    RDFBCF_EXPORT bool bcfFileSetReference                    (BCFFile* file, const char* val);
    RDFBCF_EXPORT bool bcfFileSetIfcProject                   (BCFFile* file, const char* val);
    RDFBCF_EXPORT bool bcfFileSetIfcSpatialStructureElement   (BCFFile* file, const char* val);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFViewPoint* bcfViewPointIterate(BCFTopic* topic, BCFViewPoint* prev);
    RDFBCF_EXPORT BCFViewPoint* bcfViewPointAdd(BCFTopic* topic, const char* guid);
    RDFBCF_EXPORT bool bcfViewPointRemove(BCFViewPoint* viewPoint);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT const char* bcfViewPointGetGuid(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT const char* bcfViewPointGetSnapshot(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT bool        bcfViewPointGetDefaultVisibility(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT bool        bcfViewPointGetSpaceVisible(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT bool        bcfViewPointGetSpaceBoundariesVisible(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT bool        bcfViewPointGetOpeningsVisible(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT BCFCamera   bcfViewPointGetCameraType(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT bool        bcfViewPointGetCameraViewPoint(BCFViewPoint* viewPoint, BCFPoint* retPt);
    RDFBCF_EXPORT bool        bcfViewPointGetCameraDirection(BCFViewPoint* viewPoint, BCFPoint* retPt);
    RDFBCF_EXPORT bool        bcfViewPointGetCameraUpVector(BCFViewPoint* viewPoint, BCFPoint* retPt);
    RDFBCF_EXPORT double      bcfViewPointGetViewToWorldScale(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT double      bcfViewPointGetFieldOfView(BCFViewPoint* viewPoint);
    RDFBCF_EXPORT double      bcfViewPointGetAspectRatio(BCFViewPoint* viewPoint);

    RDFBCF_EXPORT bool        bcfViewPointSetSnapshot(BCFViewPoint* viewPoint, const char* filePath);
    RDFBCF_EXPORT bool        bcfViewPointSetDefaultVisibility(BCFViewPoint* viewPoint, bool val);
    RDFBCF_EXPORT bool        bcfViewPointSetSpaceVisible(BCFViewPoint* viewPoint, bool val);
    RDFBCF_EXPORT bool        bcfViewPointSetSpaceBoundariesVisible(BCFViewPoint* viewPoint, bool val);
    RDFBCF_EXPORT bool        bcfViewPointSetOpeningsVisible(BCFViewPoint* viewPoint, bool val);
    RDFBCF_EXPORT bool        bcfViewPointSetCameraType(BCFViewPoint* viewPoint, BCFCamera val);
    RDFBCF_EXPORT bool        bcfViewPointSetCameraViewPoint(BCFViewPoint* viewPoint, BCFPoint* pt);
    RDFBCF_EXPORT bool        bcfViewPointSetCameraDirection(BCFViewPoint* viewPoint, BCFPoint* pt);
    RDFBCF_EXPORT bool        bcfViewPointSetCameraUpVector(BCFViewPoint* viewPoint, BCFPoint* pt);
    RDFBCF_EXPORT bool        bcfViewPointSetViewToWorldScale(BCFViewPoint* viewPoint, double val);
    RDFBCF_EXPORT bool        bcfViewPointSetFieldOfView(BCFViewPoint* viewPoint, double val);
    RDFBCF_EXPORT bool        bcfViewPointSetAspectRatio(BCFViewPoint* viewPoint, double val);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFComment* bcfCommentIterate (BCFTopic* topic, BCFComment* prev);
    RDFBCF_EXPORT BCFComment* bcfCommentAdd  (BCFTopic* topic, const char* guid);
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

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFComponent* bcfViewPointSelectionAdd(BCFViewPoint* viewPoint, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfViewPointSelectionIterate(BCFViewPoint* viewPoint, BCFComponent* prev);
    RDFBCF_EXPORT bool bcfViewPointSelectionRemove(BCFViewPoint* viewPoint, BCFComponent* component);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFComponent* bcfViewPointExceptionsAdd(BCFViewPoint* viewPoint, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfViewPointExceptionIterate(BCFViewPoint* viewPoint, BCFComponent* prev);
    RDFBCF_EXPORT bool bcfViewPointExceptionRemove(BCFViewPoint* viewPoint, BCFComponent* component);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFColor* bcfViewPointColoringAdd();
    RDFBCF_EXPORT BCFColor* bcfViewPointColoringIterate(BCFColor* prev);
    RDFBCF_EXPORT bool bcfViewPointColoringRemove(BCFColor* color);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFComponent* bcfColorComponentAdd(BCFColor* color, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfColorComponentIterate(BCFColor* color, BCFComponent* prev);
    RDFBCF_EXPORT bool bcfColorComponetRemove(BCFColor* color, BCFComponent* component);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFColor* bcfViewPointColoringAdd();
    RDFBCF_EXPORT BCFColor* bcfViewPointColoringIterate(BCFColor* prev);
    RDFBCF_EXPORT bool bcfViewPointColoringRemove(BCFColor* coloring);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFLine* bcfViewPointLineAdd();
    RDFBCF_EXPORT BCFLine* bcfViewPointLineIterate(BCFLine* prev);
    RDFBCF_EXPORT bool bcfViewPointLineRemove(BCFLine* line);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFClippingPlane* bcfViewPointClippingPlaneAdd();
    RDFBCF_EXPORT BCFClippingPlane* bcfViewPointClippingPlaneIterate(BCFClippingPlane* prev);
    RDFBCF_EXPORT bool bcfViewPointClippingPlaneRemove(BCFClippingPlane* component);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFBitmap* bcfViewPointBitmapAdd();
    RDFBCF_EXPORT BCFBitmap* bcfViewPointBitmapIterate(BCFBitmap* prev);
    RDFBCF_EXPORT bool bcfViewPointBitmapRemove(BCFBitmap* line);


#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

