

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
    RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project, bool cleanLog);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfFileRead(BCFProject* project, const char* bcfFilePath, bool autofix);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfFileWrite(BCFProject* project, const char* bcfFilePath, BCFVersion version);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfSetOptions(BCFProject* project, const char* user, bool autoExtent, bool validateIfcGuids);

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
    RDFBCF_EXPORT const char* bcfEnumerationElementGet(BCFProject* project, BCFEnumeration enumeration, uint16_t ind);

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
    RDFBCF_EXPORT BCFTopic* bcfTopicGetAt(BCFProject* project, uint16_t ind);
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
    RDFBCF_EXPORT BCFBimFile* bcfBimFileGetAt(BCFTopic* topic, uint16_t ind);
    RDFBCF_EXPORT BCFBimFile* bcfBimFileAdd(BCFTopic* topic, const char* filePath, bool isExternal);
    RDFBCF_EXPORT bool bcfBimFileRemove(BCFBimFile* file);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool        bcfBimFileGetIsExternal                   (BCFBimFile* file);
    RDFBCF_EXPORT const char* bcfBimFileGetFilename                     (BCFBimFile* file);
    RDFBCF_EXPORT const char* bcfBimFileGetDate                         (BCFBimFile* file);
    RDFBCF_EXPORT const char* bcfBimFileGetReference                    (BCFBimFile* file);
    RDFBCF_EXPORT const char* bcfBimFileGetIfcProject                   (BCFBimFile* file);
    RDFBCF_EXPORT const char* bcfBimFileGetIfcSpatialStructureElement   (BCFBimFile* file);

    RDFBCF_EXPORT bool bcfBimFileSetIsExternal                   (BCFBimFile* file, bool        val);
    RDFBCF_EXPORT bool bcfBimFileSetFilename                     (BCFBimFile* file, const char* val);
    RDFBCF_EXPORT bool bcfBimFileSetDate                         (BCFBimFile* file, const char* val);
    RDFBCF_EXPORT bool bcfBimFileSetReference                    (BCFBimFile* file, const char* val);
    RDFBCF_EXPORT bool bcfBimFileSetIfcProject                   (BCFBimFile* file, const char* val);
    RDFBCF_EXPORT bool bcfBimFileSetIfcSpatialStructureElement   (BCFBimFile* file, const char* val);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFViewPoint* bcfViewPointGetAt(BCFTopic* topic, uint16_t ind);
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
    RDFBCF_EXPORT BCFComment* bcfCommentGetAt (BCFTopic* topic, uint16_t ind);
    RDFBCF_EXPORT BCFComment* bcfCommentAdd     (BCFTopic* topic, const char* guid);
    RDFBCF_EXPORT bool        bcfCommentRemove  (BCFComment* comment);

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
    RDFBCF_EXPORT BCFDocumentReference* bcfDocumentReferenceGetAt(BCFTopic* topic, uint16_t ind);
    RDFBCF_EXPORT BCFDocumentReference* bcfDocumentReferenceAdd(BCFTopic* topic, const char* filePath, bool isExternal, const char* guid);
    RDFBCF_EXPORT bool                  bcfDocumentReferenceRemove(BCFDocumentReference* comment);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT const char* bcfDocumentReferenceGetGuid       (BCFDocumentReference* documentReferece);
    RDFBCF_EXPORT const char* bcfDocumentReferenceGetFilePath   (BCFDocumentReference* documentReferece);
    RDFBCF_EXPORT const char* bcfDocumentReferenceGetDescription(BCFDocumentReference* documentReferece);

    RDFBCF_EXPORT bool bcfDocumentReferenceSetFilePath          (BCFDocumentReference* documentReferece, const char* filePath, bool isExternal);
    RDFBCF_EXPORT bool bcfDocumentReferenceSetDescription       (BCFDocumentReference* documentReferece, const char* value);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFComponent* bcfViewPointSelectionAdd(BCFViewPoint* viewPoint, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfViewPointSelectionGetAt(BCFViewPoint* viewPoint, uint16_t ind);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFComponent* bcfViewPointExceptionAdd(BCFViewPoint* viewPoint, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfViewPointExceptionGetAt(BCFViewPoint* viewPoint, uint16_t ind);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfViewComponentRemove(BCFComponent* component);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT const char* bcfComponentGetIfcGuid(BCFComponent* component);
    RDFBCF_EXPORT const char* bcfComponentGetOriginatingSystem(BCFComponent* component);
    RDFBCF_EXPORT const char* bcfComponentGetAuthoringToolId(BCFComponent* component);

    RDFBCF_EXPORT bool bcfComponentSetIfcGuid (BCFComponent* component, const char* value);
    RDFBCF_EXPORT bool bcfComponentSetOriginatingSystem(BCFComponent* component, const char* value);
    RDFBCF_EXPORT bool bcfComponentSetAuthoringToolId(BCFComponent* component, const char* value);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFColoring* bcfColoringAdd(BCFViewPoint* viewPoint, const char* color);
    RDFBCF_EXPORT BCFColoring* bcfColoringGetAt(BCFViewPoint* viewPoint, uint16_t ind);
    RDFBCF_EXPORT bool bcfColoringRemove(BCFColoring* color);

    RDFBCF_EXPORT const char* bcfColoringGetColor(BCFColoring* coloring);
    RDFBCF_EXPORT bool bcfColoringSetColor(BCFColoring* coloring, const char* color);

    RDFBCF_EXPORT BCFComponent* bcfColoringComponentAdd(BCFColoring* coloring, const char* ifcGuid);
    RDFBCF_EXPORT BCFComponent* bcfColoringComponentGetAt(BCFColoring* coloring, uint16_t ind);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFLine* bcfLineAdd(BCFViewPoint* viewPoint, BCFPoint* start, BCFPoint* end);
    RDFBCF_EXPORT BCFLine* bcfLineGetAt(BCFViewPoint* viewPoint, uint16_t ind);
    RDFBCF_EXPORT bool bcfLineRemove(BCFLine* line);

    RDFBCF_EXPORT bool bcfLineGetStartPoint  (BCFLine* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool bcfLineGetEndPoint    (BCFLine* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool bcfLineSetStartPoint  (BCFLine* bitmap, BCFPoint* pt);
    RDFBCF_EXPORT bool bcfLineSetEndPoint    (BCFLine* bitmap, BCFPoint* pt);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFClippingPlane* bcfClippingPlaneAdd(BCFViewPoint* viewPoint, BCFPoint* location, BCFPoint* direction);
    RDFBCF_EXPORT BCFClippingPlane* bcfClippingPlaneGetAt(BCFViewPoint* viewPoint, uint16_t ind);
    RDFBCF_EXPORT bool bcfClippingPlaneRemove(BCFClippingPlane* clippingPlane);

    RDFBCF_EXPORT bool bcfClippingPlaneGetLocation  (BCFClippingPlane* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool bcfClippingPlaneGetDirection (BCFClippingPlane* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool bcfClippingPlaneSetLocation  (BCFClippingPlane* bitmap, BCFPoint* pt);
    RDFBCF_EXPORT bool bcfClippingPlaneSetDirection (BCFClippingPlane* bitmap, BCFPoint* pt);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFBitmap* bcfBitmapAdd(BCFViewPoint* viewPoint, const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height);
    RDFBCF_EXPORT BCFBitmap* bcfBitmapGetAt(BCFViewPoint* viewPoint, uint16_t ind);
    RDFBCF_EXPORT bool bcfBitmapRemove(BCFBitmap* bitmap);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFBitmapFormat   bcfBitmapGetFormat      (BCFBitmap* bitmap);
    RDFBCF_EXPORT const char*       bcfBitmapGetReference   (BCFBitmap* bitmap);
    RDFBCF_EXPORT bool              bcfBitmapGetLocation    (BCFBitmap* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool              bcfBitmapGetNormal      (BCFBitmap* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT bool              bcfBitmapGetUp          (BCFBitmap* bitmap, BCFPoint* retPt);
    RDFBCF_EXPORT double            bcfBitmapGetHeight      (BCFBitmap* bitmap);

    RDFBCF_EXPORT bool bcfBitmapSetFormat       (BCFBitmap* bitmap, BCFBitmapFormat val);
    RDFBCF_EXPORT bool bcfBitmapSetReference    (BCFBitmap* bitmap, const char* val);
    RDFBCF_EXPORT bool bcfBitmapSetLocation     (BCFBitmap* bitmap, BCFPoint* pt);
    RDFBCF_EXPORT bool bcfBitmapSetNormal       (BCFBitmap* bitmap, BCFPoint* pt);
    RDFBCF_EXPORT bool bcfBitmapSetUp           (BCFBitmap* bitmap, BCFPoint* pt);
    RDFBCF_EXPORT bool bcfBitmapSetHeight       (BCFBitmap* bitmap, double val);


    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT BCFBimSnippet* bcfTopicGetBimSnippet(BCFTopic* topic, bool forceCreate);
    RDFBCF_EXPORT bool bcfBimSnippetRemove(BCFBimSnippet* snippet);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT const char* bcfBimSnippetGetSnippetType (BCFBimSnippet* snippet);
    RDFBCF_EXPORT bool        bcfBimSnippetGetIsExternal (BCFBimSnippet* snippet);
    RDFBCF_EXPORT const char* bcfBimSnippetGetReference (BCFBimSnippet* snippet);
    RDFBCF_EXPORT const char* bcfBimSnippetGetReferenceSchema(BCFBimSnippet* snippet);

    RDFBCF_EXPORT bool bcfBimSnippetSetSnippetType(BCFBimSnippet* snippet, const char* val);
    RDFBCF_EXPORT bool bcfBimSnippetSetIsExternal(BCFBimSnippet* snippet, bool val);
    RDFBCF_EXPORT bool bcfBimSnippetSetReference(BCFBimSnippet* snippet, const char* val);
    RDFBCF_EXPORT bool bcfBimSnippetSetReferenceSchema(BCFBimSnippet* snippet, const char* val);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool          bcfReferenceLinkAdd    (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT const char*   bcfReferenceLinkGetAt  (BCFTopic* topic, uint16_t ind);
    RDFBCF_EXPORT bool          bcfReferenceLinkRemove (BCFTopic* topic, const char* val);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool          bcfLabelAdd    (BCFTopic* topic, const char* val);
    RDFBCF_EXPORT const char*   bcfLabelGetAt  (BCFTopic* topic, uint16_t ind);
    RDFBCF_EXPORT bool          bcfLabelRemove (BCFTopic* topic, const char* val);

    /// <summary>
    ///
    /// </summary>
    RDFBCF_EXPORT bool          bcfRelatedTopicAdd     (BCFTopic* topic,BCFTopic* related);
    RDFBCF_EXPORT BCFTopic*     bcfRelatedTopicGetAt   (BCFTopic* topic,uint16_t ind);
    RDFBCF_EXPORT bool          bcfRelatedTopicRemove  (BCFTopic* topic,BCFTopic* related);

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

