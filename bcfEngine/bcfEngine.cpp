#include "pch.h"

#include "bcfEngine.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "BCFComment.h"
#include "BCFViewPoint.h"
#include "BCFFile.h"
#include "BCFComponent.h"
#include "BCFColoring.h"
#include "BCFDocumentReference.h"
#include "BCFBimSnippet.h"
#include "BCFBitmap.h"
#include "BCFLine.h"
#include "BCFClippingPlane.h"

/// <summary>
/// Macros to implement put/get attributes and iterate/remove objects
/// </summary>

#define OBJ_GET_ATTR_R(VAL, OBJ, ATTR, errVal)                                      \
RDFBCF_EXPORT t##VAL bcf##OBJ##Get##ATTR (BCF##OBJ* obj)                            \
{                                                                                   \
    if (obj) {                                                                      \
       return obj->Get##ATTR ();                                                    \
    }                                                                               \
    return errVal;                                                                  \
}

#define OBJ_GET_ATTR(VAL, OBJ, ATTR)    OBJ_GET_ATTR_R(VAL, OBJ, ATTR, 0)                                             

#define OBJ_GET_ATTR_PT(OBJ, ATTR)                                                  \
RDFBCF_EXPORT bool bcf##OBJ##Get##ATTR(BCF##OBJ* obj, BCFPoint* retPt)              \
{                                                                                   \
    if (obj && retPt) {                                                             \
        return obj->Get##ATTR(*retPt);                                              \
    }                                                                               \
    return false;                                                                   \
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
}                                                                                       
   
#define OBJ_REMOVE(OBJ)                                                                 \
RDFBCF_EXPORT bool bcf##OBJ##Remove(BCF##OBJ* obj)                                      \
{                                                                                       \
    if (obj) {                                                                          \
        return obj->Remove();                                                           \
    }                                                                                   \
    return false;                                                                       \
}

//to use in macros
typedef const char*   tStr;
typedef int           tInt;
typedef double        tReal;
typedef bool          tBool;
typedef BCFViewPoint* tViewPoint;
typedef BCFCamera     tCamera;
typedef BCFPoint*     tPoint;
typedef BCFTopic*     tTopic;
typedef BCFBitmapFormat tBitmapFormat;

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
RDFBCF_EXPORT bool bcfProjectDelete(BCFProject* project)
{
    if (project) {
        return project->Delete();
    }
    return true;
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
RDFBCF_EXPORT bool bcfFileRead(BCFProject* project, const char* bcfFilePath, bool autofix)
{
    if (project) {
        return project->Read(bcfFilePath, autofix);
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
OBJ_REMOVE(Topic)
RDFBCF_EXPORT BCFTopic* bcfTopicAdd(BCFProject* project, const char* type, const char* title, const char* status, const char* guid)
{
    if (project) {
        return project->TopicAdd(type, title, status, guid);
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
OBJ_ITERATE(File, Topic)
OBJ_REMOVE(File)
RDFBCF_EXPORT BCFFile* bcfFileAdd(BCFTopic* topic, const char* filePath, bool isExternal)
{
    if (topic) {
        return topic->FileAdd(filePath, isExternal);
    }
    return 0;
}

/// <summary>
/// 
/// </summary>
OBJ_GET_ATTR(Bool,File, IsExternal                   )
OBJ_GET_ATTR(Str, File, Filename                     )
OBJ_GET_ATTR(Str, File, Date                         )
OBJ_GET_ATTR(Str, File, Reference                    )
OBJ_GET_ATTR(Str, File, IfcProject                   )
OBJ_GET_ATTR(Str, File, IfcSpatialStructureElement   )

OBJ_SET_ATTR(Bool,File, IsExternal                   )
OBJ_SET_ATTR(Str, File, Filename                     )
OBJ_SET_ATTR(Str, File, Date                         )
OBJ_SET_ATTR(Str, File, Reference                    )
OBJ_SET_ATTR(Str, File, IfcProject                   )
OBJ_SET_ATTR(Str, File, IfcSpatialStructureElement   )


/// <summary>
/// 
/// </summary>
OBJ_ITERATE(Comment, Topic)
OBJ_REMOVE(Comment)
RDFBCF_EXPORT BCFComment* bcfCommentAdd(BCFTopic* topic, const char* guid)
{
    if (topic) {
        return topic->CommentAdd(guid);
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
OBJ_ITERATE(DocumentReference, Topic)
OBJ_REMOVE(DocumentReference)
RDFBCF_EXPORT BCFDocumentReference* bcfDocumentReferenceAdd(BCFTopic* topic, const char* filePath, bool isExternal, const char* guid)
{
    if (topic) {
        return topic->DocumentReferenceAdd(filePath, isExternal, guid);
    }
    return 0;
}

OBJ_GET_ATTR(Str, DocumentReference, Guid)
OBJ_GET_ATTR(Str, DocumentReference, FilePath)
OBJ_GET_ATTR(Str, DocumentReference, Description)

OBJ_SET_ATTR(Str, DocumentReference, Description)

RDFBCF_EXPORT bool bcfDocumentReferenceSetFilePath(BCFDocumentReference* documentReferece, const char* filePath, bool isExternal)
{
    if (documentReferece) {
        return documentReferece->SetFilePath(filePath, isExternal);
    }
    return false;
}


/// <summary>
/// 
/// </summary>
OBJ_ITERATE(ViewPoint, Topic)
OBJ_REMOVE(ViewPoint)
RDFBCF_EXPORT BCFViewPoint* bcfViewPointAdd(BCFTopic* topic, const char* guid)
{
    if (topic) {
        return topic->ViewPointAdd(guid);
    }
    return 0;
}

/// <summary>
///
/// </summary>
OBJ_GET_ATTR (Str,  ViewPoint, Guid)
OBJ_GET_ATTR (Str,  ViewPoint, Snapshot)
OBJ_GET_ATTR (Bool, ViewPoint, DefaultVisibility)
OBJ_GET_ATTR (Bool, ViewPoint, SpaceVisible)
OBJ_GET_ATTR (Bool, ViewPoint, SpaceBoundariesVisible)
OBJ_GET_ATTR (Bool, ViewPoint, OpeningsVisible)
OBJ_GET_ATTR (Real, ViewPoint, ViewToWorldScale)
OBJ_GET_ATTR (Real, ViewPoint, FieldOfView)
OBJ_GET_ATTR (Real, ViewPoint, AspectRatio)
OBJ_GET_ATTR_R(Camera, ViewPoint, CameraType, BCFCameraOrthogonal)
OBJ_GET_ATTR_PT(ViewPoint, CameraViewPoint)
OBJ_GET_ATTR_PT(ViewPoint, CameraDirection)
OBJ_GET_ATTR_PT(ViewPoint, CameraUpVector)

/// <summary>
///
/// </summary>
OBJ_SET_ATTR(Str,    ViewPoint, Snapshot)
OBJ_SET_ATTR(Bool,   ViewPoint, DefaultVisibility)
OBJ_SET_ATTR(Bool,   ViewPoint, SpaceVisible)
OBJ_SET_ATTR(Bool,   ViewPoint, SpaceBoundariesVisible)
OBJ_SET_ATTR(Bool,   ViewPoint, OpeningsVisible)
OBJ_SET_ATTR(Camera, ViewPoint, CameraType)
OBJ_SET_ATTR(Point,  ViewPoint, CameraViewPoint)
OBJ_SET_ATTR(Point,  ViewPoint, CameraDirection)
OBJ_SET_ATTR(Point,  ViewPoint, CameraUpVector)
OBJ_SET_ATTR(Real,   ViewPoint, ViewToWorldScale)
OBJ_SET_ATTR(Real,   ViewPoint, FieldOfView)
OBJ_SET_ATTR(Real,   ViewPoint, AspectRatio)

/// <summary>
///
/// </summary>
OBJ_ITERATE(Coloring, ViewPoint)
OBJ_REMOVE(Coloring)
RDFBCF_EXPORT BCFColoring* bcfColoringAdd(BCFViewPoint* viewPoint, const char* color)
{
    if (viewPoint) {
        return viewPoint->ColoringAdd(color);
    }
    return NULL;
}

OBJ_GET_ATTR(Str, Coloring, Color)

OBJ_SET_ATTR(Str, Coloring, Color)

/// <summary>
/// 
/// </summary>
#define COMPONENT_LIST(Parent, List)                                                            \
RDFBCF_EXPORT BCFComponent* bcf##Parent##List##Add(BCF##Parent* parent, const char* ifcGuid)    \
{                                                                                               \
    if (parent) {                                                                               \
        return parent->##List##Add(ifcGuid);                                                    \
    }                                                                                           \
    return NULL;                                                                                \
}                                                                                               \
RDFBCF_EXPORT BCFComponent* bcf##Parent##List##Iterate(BCF##Parent* parent, BCFComponent* prev) \
{                                                                                               \
    if (parent) {                                                                               \
        return parent->##List##Iterate(prev);                                                   \
    }                                                                                           \
    return NULL;                                                                                \
}                                                                                               \

COMPONENT_LIST(ViewPoint, Selection)
COMPONENT_LIST(ViewPoint, Exceptions)
COMPONENT_LIST(Coloring, Component)

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfViewComponentRemove(BCFComponent* component)
{
    if (component) {
        return component->Remove();
    }
    return false;
}

/// <summary>
/// 
/// </summary>

OBJ_GET_ATTR(Str, Component, IfcGuid)
OBJ_GET_ATTR(Str, Component, OriginatingSystem)
OBJ_GET_ATTR(Str, Component, AuthoringToolId)

OBJ_SET_ATTR(Str, Component, IfcGuid)
OBJ_SET_ATTR(Str, Component, OriginatingSystem)
OBJ_SET_ATTR(Str, Component, AuthoringToolId)

/// <summary>
/// 
/// </summary>
OBJ_REMOVE(BimSnippet)
RDFBCF_EXPORT BCFBimSnippet* bcfTopicGetBimSnippet(BCFTopic* topic, bool forceCreate)
{
    if (topic) {
        return topic->GetBimSnippet(forceCreate);
    }
    return NULL;
}

OBJ_GET_ATTR(Str, BimSnippet, SnippetType)
OBJ_GET_ATTR(Bool,BimSnippet, IsExternal)
OBJ_GET_ATTR(Str, BimSnippet, Reference)
OBJ_GET_ATTR(Str, BimSnippet, ReferenceSchema)

OBJ_SET_ATTR(Str, BimSnippet, SnippetType)
OBJ_SET_ATTR(Bool,BimSnippet, IsExternal)
OBJ_SET_ATTR(Str, BimSnippet, Reference)
OBJ_SET_ATTR(Str, BimSnippet, ReferenceSchema)

/// <summary>
/// 
/// </summary>
#define TOPIC_LIST(ListName, ElemType)                                                  \
RDFBCF_EXPORT bool          bcf##ListName##Add   (BCFTopic* topic, t##ElemType val)     \
{                                                                                       \
    if (topic) {                                                                        \
        return topic->##ListName##Add(val);                                             \
    }                                                                                   \
    return false;                                                                       \
}                                                                                       \
RDFBCF_EXPORT t##ElemType   bcf##ListName##Iterate(BCFTopic* topic, t##ElemType prev)   \
{                                                                                       \
    if (topic) {                                                                        \
        return topic->##ListName##Iterate(prev);                                        \
    }                                                                                   \
    return NULL;                                                                        \
}                                                                                       \
RDFBCF_EXPORT bool          bcf##ListName##Remove(BCFTopic* topic, t##ElemType val)     \
{                                                                                       \
    if (topic) {                                                                        \
        topic->##ListName##Remove(val);                                                 \
    }                                                                                   \
    return false;                                                                       \
}

TOPIC_LIST(ReferenceLink, Str)
TOPIC_LIST(Label, Str)
TOPIC_LIST(RelatedTopic, Topic)

/// <summary>
/// 
/// </summary>
OBJ_REMOVE(Bitmap)
OBJ_ITERATE(Bitmap, ViewPoint)
RDFBCF_EXPORT BCFBitmap* bcfBitmapAdd(BCFViewPoint* viewPoint, const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height)
{
    if (viewPoint) {
        return viewPoint->BitmapAdd(filePath, format, location, normal, up, height);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
OBJ_GET_ATTR_R(BitmapFormat, Bitmap, Format, BCFBitmapPNG);
OBJ_GET_ATTR(Str, Bitmap, Reference);
OBJ_GET_ATTR_PT(Bitmap, Location);
OBJ_GET_ATTR_PT(Bitmap, Normal);
OBJ_GET_ATTR_PT(Bitmap, Up);
OBJ_GET_ATTR(Real, Bitmap, Height);

OBJ_SET_ATTR(BitmapFormat, Bitmap, Format);
OBJ_SET_ATTR(Str, Bitmap, Reference);
OBJ_SET_ATTR(Point, Bitmap, Location);
OBJ_SET_ATTR(Point, Bitmap, Normal);
OBJ_SET_ATTR(Point, Bitmap, Up);
OBJ_SET_ATTR(Real, Bitmap, Height);

/// <summary>
/// 
/// </summary>
OBJ_REMOVE(Line)
OBJ_ITERATE(Line, ViewPoint)
RDFBCF_EXPORT BCFLine* bcfLineAdd(BCFViewPoint* viewPoint, BCFPoint* start, BCFPoint* end)
{
    if (viewPoint) {
        return viewPoint->LineAdd(start, end);
    }
    else {
        return NULL;
    }
}

OBJ_GET_ATTR_PT(Line, StartPoint);
OBJ_GET_ATTR_PT(Line, EndPoint);

OBJ_SET_ATTR(Point, Line, StartPoint);
OBJ_SET_ATTR(Point, Line, EndPoint);

/// <summary>
/// 
/// </summary>
OBJ_REMOVE(ClippingPlane)
OBJ_ITERATE(ClippingPlane, ViewPoint)
RDFBCF_EXPORT BCFClippingPlane* bcfClippingPlaneAdd(BCFViewPoint* viewPoint, BCFPoint* location, BCFPoint* direction)
{
    if (viewPoint) {
        return viewPoint->ClippingPlaneAdd(location, direction);
    }
    else {
        return NULL;
    }
}

OBJ_GET_ATTR_PT(ClippingPlane, Location);
OBJ_GET_ATTR_PT(ClippingPlane, Direction);

OBJ_SET_ATTR(Point, ClippingPlane, Location);
OBJ_SET_ATTR(Point, ClippingPlane, Direction);

