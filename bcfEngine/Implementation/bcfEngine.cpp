#include "pch.h"

#include "bcfEngine.h"
#include "bcfAPI.h"

///
#define TYPE_CHECK(ptr, type, errVal) {if (!TypeCheck ((char*)ptr, BCFTypeCheck_##type)) return errVal;}

static bool TypeCheck(char* ptr, long typeId)
{
    ptr += sizeof(char*); //skip vftable
    auto pl = (long*)ptr;
    if (*pl != typeId) {
        printf("Inavlid type in the RDF.BCF: %ld. Expected %ld!\n", *pl, typeId);
        assert(false);
        return false;
    }
    return true;
}


/// <summary>
/// Macros to implement put/get attributes and iterate/remove objects
/// </summary>

#define OBJ_GET_ATTR_R(VAL, OBJ, ATTR, errVal)                                      \
RDFBCF_EXPORT t##VAL bcf##OBJ##Get##ATTR (BCF##OBJ* obj)                            \
{                                                                                   \
    if (obj) {                                                                      \
       TYPE_CHECK(obj, OBJ, errVal)                                                 \
       return obj->Get##ATTR ();                                                    \
    }                                                                               \
    return errVal;                                                                  \
}

#define OBJ_GET_ATTR(VAL, OBJ, ATTR)    OBJ_GET_ATTR_R(VAL, OBJ, ATTR, 0)                                             

#define OBJ_GET_ATTR_PT(OBJ, ATTR)                                                  \
RDFBCF_EXPORT bool bcf##OBJ##Get##ATTR(BCF##OBJ* obj, BCFPoint* retPt)              \
{                                                                                   \
    if (obj && retPt) {                                                             \
        TYPE_CHECK(obj, OBJ, false)                                                 \
        return obj->Get##ATTR(*retPt);                                              \
    }                                                                               \
    return false;                                                                   \
}


#define OBJ_SET_ATTR(VAL, OBJ, ATTR)                                                \
RDFBCF_EXPORT bool bcf  ##OBJ##Set##ATTR (BCF##OBJ* obj, t##VAL val)                \
{                                                                                   \
    if (obj) {                                                                      \
        TYPE_CHECK(obj, OBJ, false)                                                 \
        return obj->Set##ATTR (val);                                                \
    }                                                                               \
    return false;                                                                   \
}

#define OBJ_ITERATE(OBJ, CONTAINER)                                                     \
RDFBCF_EXPORT BCF##OBJ* bcf##OBJ##GetAt(BCF##CONTAINER* container, uint16_t ind)        \
{                                                                                       \
    if (container) {                                                                    \
        TYPE_CHECK(container, CONTAINER, NULL)                                          \
        return container->Get##OBJ##(ind);                                              \
    }                                                                                   \
    return NULL;                                                                        \
}                                                                                       
   
#define OBJ_REMOVE(OBJ)                                                                 \
RDFBCF_EXPORT bool bcf##OBJ##Remove(BCF##OBJ* obj)                                      \
{                                                                                       \
    if (obj) {                                                                          \
        TYPE_CHECK(obj, OBJ, false)                                                     \
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
    return BCFProject::Create(projectId);
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfProjectDelete(BCFProject* project)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
        return project->Delete();
    }
    return true;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfProjectIsDirty(BCFProject* project)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
        return project->IsDirty();
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project, bool cleanLog)
{
    if (project) {
        TYPE_CHECK(project, Project, NULL);
        return project->GetErrors(cleanLog);
    }
    return "NULL ARGUMENT";
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfFileRead(BCFProject* project, const char* bcfFilePath, bool autofix)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
        return project->ReadFile(bcfFilePath, autofix);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfFileWrite(BCFProject* project, const char* bcfFilePath, BCFVersion version)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
        return project->WriteFile(bcfFilePath, version);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfSetOptions(BCFProject* project, const char* user, bool autoExtent, bool validateIfcGuids)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
        return project->SetOptions(user, autoExtent, validateIfcGuids);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfProjectIdGet(BCFProject* project)
{
    if (project) {
        TYPE_CHECK(project, Project, NULL);
        return project->GetProjectId();
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfProjectNameGet(BCFProject* project)
{
    if (project) {
        TYPE_CHECK(project, Project, NULL);
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
        TYPE_CHECK(project, Project, false);
        project->SetName(name);
        return true;
    }
    return false;
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT const char* bcfEnumerationElementGet(BCFProject* project, BCFEnumeration enumeration, uint16_t ind)
{
    if (project) {
        TYPE_CHECK(project, Project, NULL);
        return project->GetExtensions().GetElement(enumeration, ind);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfEnumerationElementAdd(BCFProject* project, BCFEnumeration enumeration, const char* element)
{
    if (project) {
        TYPE_CHECK(project, Project, false);
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
        TYPE_CHECK(project, Project, false);
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
        TYPE_CHECK(project, Project, NULL);
        return project->AddTopic(type, title, status, guid);
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
OBJ_ITERATE(BimFile, Topic)
OBJ_REMOVE(BimFile)
RDFBCF_EXPORT BCFBimFile* bcfBimFileAdd(BCFTopic* topic, const char* filePath, bool isExternal)
{
    if (topic) {
        TYPE_CHECK(topic, Topic, NULL);
        return topic->AddBimFile(filePath, isExternal);
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
OBJ_GET_ATTR(Bool,BimFile, IsExternal                   )
OBJ_GET_ATTR(Str, BimFile, Filename                     )
OBJ_GET_ATTR(Str, BimFile, Date                         )
OBJ_GET_ATTR(Str, BimFile, Reference                    )
OBJ_GET_ATTR(Str, BimFile, IfcProject                   )
OBJ_GET_ATTR(Str, BimFile, IfcSpatialStructureElement   )

OBJ_SET_ATTR(Bool,BimFile, IsExternal                   )
OBJ_SET_ATTR(Str, BimFile, Filename                     )
OBJ_SET_ATTR(Str, BimFile, Date                         )
OBJ_SET_ATTR(Str, BimFile, Reference                    )
OBJ_SET_ATTR(Str, BimFile, IfcProject                   )
OBJ_SET_ATTR(Str, BimFile, IfcSpatialStructureElement   )


/// <summary>
/// 
/// </summary>
OBJ_ITERATE(Comment, Topic)
OBJ_REMOVE(Comment)
RDFBCF_EXPORT BCFComment* bcfCommentAdd(BCFTopic* topic, const char* guid)
{
    if (topic) {
        TYPE_CHECK(topic, Topic, NULL);
        return topic->AddComment(guid);
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
        TYPE_CHECK(topic, Topic, NULL);
        return topic->AddDocumentReference(filePath, isExternal, guid);
    }
    return 0;
}

OBJ_GET_ATTR(Str, DocumentReference, Guid)
OBJ_GET_ATTR(Str, DocumentReference, FilePath)
OBJ_GET_ATTR(Str, DocumentReference, Description)

OBJ_SET_ATTR(Str, DocumentReference, Description)

RDFBCF_EXPORT bool bcfDocumentReferenceSetFilePath(BCFDocumentReference* documentReference, const char* filePath, bool isExternal)
{
    if (documentReference) {
        TYPE_CHECK(documentReference, DocumentReference, NULL);
        return documentReference->SetFilePath(filePath, isExternal);
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
        TYPE_CHECK(topic, Topic, NULL);
        return topic->AddViewPoint(guid);
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
        TYPE_CHECK(viewPoint, ViewPoint, NULL);
        return viewPoint->AddColoring(color);
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
        TYPE_CHECK(parent, Parent, NULL);                                                       \
        return parent->Add##List##(ifcGuid);                                                    \
    }                                                                                           \
    return NULL;                                                                                \
}                                                                                               \
RDFBCF_EXPORT BCFComponent* bcf##Parent##List##GetAt(BCF##Parent* parent, uint16_t ind)         \
{                                                                                               \
    if (parent) {                                                                               \
        TYPE_CHECK(parent, Parent, NULL);                                                       \
        return parent->Get##List##(ind);                                                        \
    }                                                                                           \
    return NULL;                                                                                \
}                                                                                               \

COMPONENT_LIST(ViewPoint, Selection)
COMPONENT_LIST(ViewPoint, Exception)
COMPONENT_LIST(Coloring, Component)

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT bool bcfViewComponentRemove(BCFComponent* component)
{
    if (component) {
        TYPE_CHECK(component, Component, false);
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
        TYPE_CHECK(topic, Topic, NULL);
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
        TYPE_CHECK(topic, Topic, false);                                                \
        return topic->Add##ListName##(val);                                             \
    }                                                                                   \
    return false;                                                                       \
}                                                                                       \
RDFBCF_EXPORT t##ElemType   bcf##ListName##GetAt(BCFTopic* topic, uint16_t ind)         \
{                                                                                       \
    if (topic) {                                                                        \
        TYPE_CHECK(topic, Topic, NULL);                                                 \
        return topic->Get##ListName##(ind);                                             \
    }                                                                                   \
    return NULL;                                                                        \
}                                                                                       \
RDFBCF_EXPORT bool          bcf##ListName##Remove(BCFTopic* topic, t##ElemType val)     \
{                                                                                       \
    if (topic) {                                                                        \
        TYPE_CHECK(topic, Topic, false);                                                \
        topic->Remove##ListName##(val);                                                 \
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
        TYPE_CHECK(viewPoint, ViewPoint, NULL);
        return viewPoint->AddBitmap(filePath, format, location, normal, up, height);
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
        TYPE_CHECK(viewPoint, ViewPoint, NULL);
        return viewPoint->AddLine(start, end);
    }
    return NULL;
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
        TYPE_CHECK(viewPoint, ViewPoint, NULL);
        return viewPoint->AddClippingPlane(location, direction);
    }
    return NULL;
}

OBJ_GET_ATTR_PT(ClippingPlane, Location);
OBJ_GET_ATTR_PT(ClippingPlane, Direction);

OBJ_SET_ATTR(Point, ClippingPlane, Location);
OBJ_SET_ATTR(Point, ClippingPlane, Direction);

