#pragma once

#include "bcfTypes.h"

struct BCFExtensions;

#define BCF_PROPERTY_RO(Type, PropName)               \
    virtual Type Get##PropName() = NULL;

#define BCF_PROPERTY_RW(Type, PropName)               \
    virtual Type Get##PropName()           = NULL;    \
    virtual bool Set##PropName(Type value) = NULL;

#define BCF_PROPERTY_PT(PropName)                     \
    virtual bool Get##PropName(BCFPoint& pt) = NULL;  \
    virtual bool Set##PropName(BCFPoint* pt) = NULL;

/// <summary>
/// Use static Create method to create prject, and Delete to dispose
/// All BCF obects lifetime until BCFProject::Delete
/// All strings are valid untile next call or BCFProject::Delete
/// </summary>
struct BCFProject
{
    static RDFBCF_EXPORT BCFProject* Create(const char* projectId = NULL);

    virtual bool            Delete(void) = NULL;
    virtual const char*     GetErrors(bool cleanLog = true) = NULL;

    virtual bool            ReadFile(const char* bcfFilePath, bool autofix) = NULL;
    virtual bool            WriteFile(const char* bcfFilePath, BCFVersion version) = NULL;

    virtual bool            SetAuthor(const char* user, bool autoExtent) = NULL;
    virtual BCFExtensions&  GetExtensions() = NULL;

    BCF_PROPERTY_RO(const char*, ProjectId);
    BCF_PROPERTY_RW(const char*, Name);

    virtual BCFTopic* TopicIterate(BCFTopic* prev) = NULL;
    virtual BCFTopic* TopicAdd(const char* type, const char* title, const char* status, const char* guid = NULL) = NULL;
};


/// <summary>
/// 
/// </summary>
struct BCFTopic
{
    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RW(const char*, ServerAssignedId);
    BCF_PROPERTY_RW(const char*, TopicStatus);
    BCF_PROPERTY_RW(const char*, TopicType);
    BCF_PROPERTY_RW(const char*, Title);
    BCF_PROPERTY_RW(const char*, Priority);
    BCF_PROPERTY_RO(const char*, CreationDate);
    BCF_PROPERTY_RO(const char*, CreationAuthor);
    BCF_PROPERTY_RO(const char*, ModifiedDate);
    BCF_PROPERTY_RO(const char*, ModifiedAuthor);
    BCF_PROPERTY_RW(const char*, DueDate);
    BCF_PROPERTY_RW(const char*, AssignedTo);
    BCF_PROPERTY_RW(const char*, Description);
    BCF_PROPERTY_RW(const char*, Stage);
    BCF_PROPERTY_RW(int,         Index);

    virtual BCFFile* FileIterate(BCFFile* prev) = NULL;
    virtual BCFFile* FileAdd(const char* filePath, bool isExternal) = NULL;

    virtual BCFComment* CommentIterate(BCFComment* prev) = NULL;
    virtual BCFComment* CommentAdd(const char* guid = NULL) = NULL;

    virtual BCFViewPoint* ViewPointIterate(BCFViewPoint* prev) = NULL;
    virtual BCFViewPoint* ViewPointAdd(const char* guid = NULL) = NULL;

    virtual BCFDocumentReference* DocumentReferenceIterate(BCFDocumentReference* prev) = NULL;
    virtual BCFDocumentReference* DocumentReferenceAdd(const char* filePath, bool isExternal, const char* guid = NULL) = NULL;

    virtual BCFBimSnippet* GetBimSnippet(bool forceCreate) = NULL;

    virtual const char* ReferenceLinkIterate(const char* prev) = NULL;
    virtual bool ReferenceLinkAdd(const char* value) = NULL;
    virtual bool ReferenceLinkRemove(const char* value) = NULL;

    virtual const char* LabelIterate(const char* prev) = NULL;
    virtual bool LabelAdd(const char* value) = NULL;
    virtual bool LabelRemove(const char* value) = NULL;

    virtual BCFTopic* RelatedTopicIterate(BCFTopic* prev) = NULL;
    virtual bool RelatedTopicAdd(BCFTopic* value) = NULL;
    virtual bool RelatedTopicRemove(BCFTopic* value) = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFComment
{
    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RO(const char*, Date);
    BCF_PROPERTY_RO(const char*, Author);
    BCF_PROPERTY_RO(const char*, ModifiedDate);
    BCF_PROPERTY_RO(const char*, ModifiedAuthor);
    BCF_PROPERTY_RW(const char*, Text);
    BCF_PROPERTY_RW(BCFViewPoint*, ViewPoint);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFViewPoint
{
    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RW(const char*, Snapshot);
    BCF_PROPERTY_RW(bool, DefaultVisibility);
    BCF_PROPERTY_RW(bool, SpaceVisible);
    BCF_PROPERTY_RW(bool, SpaceBoundariesVisible);
    BCF_PROPERTY_RW(bool, OpeningsVisible);
    BCF_PROPERTY_RW(BCFCamera, CameraType);
    BCF_PROPERTY_RW(double, ViewToWorldScale);
    BCF_PROPERTY_RW(double, FieldOfView);
    BCF_PROPERTY_RW(double, AspectRatio);
    BCF_PROPERTY_PT(CameraViewPoint);
    BCF_PROPERTY_PT(CameraDirection);
    BCF_PROPERTY_PT(CameraUpVector);

    virtual BCFComponent* SelectionIterate(BCFComponent* prev) = NULL;
    virtual BCFComponent* SelectionAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual BCFComponent* ExceptionsIterate(BCFComponent* prev) = NULL;
    virtual BCFComponent* ExceptionsAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual BCFColoring* ColoringIterate(BCFColoring* prev) = NULL;
    virtual BCFColoring* ColoringAdd(const char* color) = NULL;

    virtual BCFBitmap* BitmapIterate(BCFBitmap* prev) = NULL;
    virtual BCFBitmap* BitmapAdd(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height) = NULL;

    virtual BCFLine* LineIterate(BCFLine* prev) = NULL;
    virtual BCFLine* LineAdd(BCFPoint* start, BCFPoint* end) = NULL;

    virtual BCFClippingPlane* ClippingPlaneIterate(BCFClippingPlane* prev) = NULL;
    virtual BCFClippingPlane* ClippingPlaneAdd(BCFPoint* start, BCFPoint* end) = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFDocumentReference
{
    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RO(const char*, DocumentGuid);
    BCF_PROPERTY_RO(const char*, FilePath);
    BCF_PROPERTY_RW(const char*, Description);

    virtual bool SetFilePath(const char* filePath, bool isExternal) = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFComponent
{
    BCF_PROPERTY_RW(const char*, IfcGuid);
    BCF_PROPERTY_RW(const char*, OriginatingSystem);
    BCF_PROPERTY_RW(const char*, AuthoringToolId);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFColoring
{
    BCF_PROPERTY_RW(const char*, Color)

    virtual BCFComponent* ComponentIterate(BCFComponent* prev) = NULL;
    virtual BCFComponent* ComponentAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFFile
{
    BCF_PROPERTY_RW(bool, IsExternal);
    BCF_PROPERTY_RW(const char*, Filename);
    BCF_PROPERTY_RW(const char*, Date);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_RW(const char*, IfcProject);
    BCF_PROPERTY_RW(const char*, IfcSpatialStructureElement);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFLine
{
    BCF_PROPERTY_PT(StartPoint);
    BCF_PROPERTY_PT(EndPoint);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFClippingPlane
{
    BCF_PROPERTY_PT(Location);
    BCF_PROPERTY_PT(Direction);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFBitmap
{
    BCF_PROPERTY_RW(BCFBitmapFormat, Format);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_PT(Location);
    BCF_PROPERTY_PT(Normal);
    BCF_PROPERTY_PT(Up);
    BCF_PROPERTY_RW(double, Height);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFBimSnippet
{
    BCF_PROPERTY_RW(const char*, SnippetType);
    BCF_PROPERTY_RW(bool, IsExternal);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_RW(const char*, ReferenceSchema);

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFExtensions
{
    virtual const char* GetElement(BCFEnumeration enumeration, int index) = NULL;
    virtual bool        AddElement(BCFEnumeration enumeration, const char* value) = NULL;
    virtual bool        RemoveElement(BCFEnumeration enumeration, const char* element) = NULL;
};
