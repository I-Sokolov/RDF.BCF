#pragma once

#include "bcfTypes.h"

struct BCFExtensions;

// These constants are stored in BCF structures after virtual function tables 
// and can be used to runtime type checks
#define BCFTypeCheck_Project             102030 
#define BCFTypeCheck_Topic               102031
#define BCFTypeCheck_BimFile             102032
#define BCFTypeCheck_ViewPoint           102033
#define BCFTypeCheck_Comment             102034
#define BCFTypeCheck_DocumentReference   102036
#define BCFTypeCheck_Component           102037
#define BCFTypeCheck_Coloring            102038
#define BCFTypeCheck_Line                102039
#define BCFTypeCheck_ClippingPlane       102040
#define BCFTypeCheck_Bitmap              102041
#define BCFTypeCheck_BimSnippet          102042
#define BCFTypeCheck_Extensions          102043

// Properties declaration macros
//
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
    long TypeCheck = BCFTypeCheck_Project;

    static RDFBCF_EXPORT BCFProject* Create(const char* projectId = NULL);

    virtual bool            Delete(void) = NULL;
    virtual const char*     GetErrors(bool cleanLog = true) = NULL;

    virtual bool            ReadFile(const char* bcfFilePath, bool autofix) = NULL;
    virtual bool            WriteFile(const char* bcfFilePath, BCFVersion version) = NULL;

    virtual bool            SetOptions(const char* user, bool autoExtent = true, bool validateIfcGuids = false) = NULL;
    virtual BCFExtensions&  GetExtensions() = NULL;

    virtual BCFTopic*       GetTopic(uint16_t ind) = NULL;
    virtual BCFTopic*       AddTopic(const char* type, const char* title, const char* status, const char* guid = NULL) = NULL;

    virtual bool            IsModified() = NULL;

    BCF_PROPERTY_RO(const char*, ProjectId);
    BCF_PROPERTY_RW(const char*, Name);

};


/// <summary>
/// 
/// </summary>
struct BCFTopic
{
    long TypeCheck = BCFTypeCheck_Topic;

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
    BCF_PROPERTY_RW(const char*, IndexStr);

    virtual BCFBimFile* GetBimFile(uint16_t ind) = NULL;
    virtual BCFBimFile* AddBimFile(const char* filePath, bool isExternal) = NULL;

    virtual BCFComment* GetComment(uint16_t ind) = NULL;
    virtual BCFComment* AddComment(const char* guid = NULL) = NULL;

    virtual BCFViewPoint* GetViewPoint(uint16_t ind) = NULL;
    virtual BCFViewPoint* AddViewPoint(const char* guid = NULL) = NULL;

    virtual BCFDocumentReference* GetDocumentReference(uint16_t ind) = NULL;
    virtual BCFDocumentReference* AddDocumentReference(const char* filePath, bool isExternal, const char* guid = NULL) = NULL;

    virtual BCFBimSnippet* GetBimSnippet(bool forceCreate) = NULL;

    virtual const char* GetReferenceLink(uint16_t ind) = NULL;
    virtual bool AddReferenceLink(const char* value) = NULL;
    virtual bool RemoveReferenceLink(const char* value) = NULL;

    virtual const char* GetLabel(uint16_t ind) = NULL;
    virtual bool AddLabel(const char* value) = NULL;
    virtual bool RemoveLabel(const char* value) = NULL;

    virtual BCFTopic* GetRelatedTopic(uint16_t ind) = NULL;
    virtual bool AddRelatedTopic(BCFTopic* value) = NULL;
    virtual bool RemoveRelatedTopic(BCFTopic* value) = NULL;

    virtual BCFProject& GetProject() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFComment
{
    long TypeCheck = BCFTypeCheck_Comment;

    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RO(const char*, Date);
    BCF_PROPERTY_RO(const char*, Author);
    BCF_PROPERTY_RO(const char*, ModifiedDate);
    BCF_PROPERTY_RO(const char*, ModifiedAuthor);
    BCF_PROPERTY_RW(const char*, Text);
    BCF_PROPERTY_RW(BCFViewPoint*, ViewPoint);

    virtual BCFTopic& GetTopic() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFViewPoint
{
    long TypeCheck = BCFTypeCheck_ViewPoint;

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

    virtual BCFComponent* GetSelection(uint16_t ind) = NULL;
    virtual BCFComponent* AddSelection(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual BCFComponent* GetException(uint16_t ind) = NULL;
    virtual BCFComponent* AddException(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual BCFColoring* GetColoring(uint16_t ind) = NULL;
    virtual BCFColoring* AddColoring(const char* color) = NULL;

    virtual BCFBitmap* GetBitmap(uint16_t ind) = NULL;
    virtual BCFBitmap* AddBitmap(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height) = NULL;

    virtual BCFLine* GetLine(uint16_t ind) = NULL;
    virtual BCFLine* AddLine(BCFPoint* start, BCFPoint* end) = NULL;

    virtual BCFClippingPlane* GetClippingPlane(uint16_t ind) = NULL;
    virtual BCFClippingPlane* AddClippingPlane(BCFPoint* start, BCFPoint* end) = NULL;

    virtual BCFTopic& GetTopic() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFDocumentReference
{
    long TypeCheck = BCFTypeCheck_DocumentReference;

    BCF_PROPERTY_RO(const char*, Guid);
    BCF_PROPERTY_RO(const char*, DocumentGuid);
    BCF_PROPERTY_RO(const char*, FilePath);
    BCF_PROPERTY_RW(const char*, Description);

    virtual bool SetFilePath(const char* filePath, bool isExternal) = NULL;

    virtual BCFTopic& GetTopic() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFComponent
{
    long TypeCheck = BCFTypeCheck_Component;

    BCF_PROPERTY_RW(const char*, IfcGuid);
    BCF_PROPERTY_RW(const char*, OriginatingSystem);
    BCF_PROPERTY_RW(const char*, AuthoringToolId);

    virtual BCFViewPoint& GetViewPoint() = NULL;
    virtual BCFColoring* GetColoring() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFColoring
{
    long TypeCheck = BCFTypeCheck_Coloring;

    BCF_PROPERTY_RW(const char*, Color)

    virtual BCFComponent* GetComponent(uint16_t ind) = NULL;
    virtual BCFComponent* AddComponent(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = NULL;

    virtual BCFViewPoint& GetViewPoint() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFBimFile
{
    long TypeCheck = BCFTypeCheck_BimFile;

    BCF_PROPERTY_RW(bool, IsExternal);
    BCF_PROPERTY_RW(const char*, Filename);
    BCF_PROPERTY_RW(const char*, Date);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_RW(const char*, IfcProject);
    BCF_PROPERTY_RW(const char*, IfcSpatialStructureElement);

    virtual BCFTopic& GetTopic() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFLine
{
    long TypeCheck = BCFTypeCheck_Line;

    BCF_PROPERTY_PT(StartPoint);
    BCF_PROPERTY_PT(EndPoint);

    virtual BCFViewPoint& GetViewPoint() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFClippingPlane
{
    long TypeCheck = BCFTypeCheck_ClippingPlane;

    BCF_PROPERTY_PT(Location);
    BCF_PROPERTY_PT(Direction);

    virtual BCFViewPoint& GetViewPoint() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFBitmap
{
    long TypeCheck = BCFTypeCheck_Bitmap;

    BCF_PROPERTY_RW(BCFBitmapFormat, Format);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_PT(Location);
    BCF_PROPERTY_PT(Normal);
    BCF_PROPERTY_PT(Up);
    BCF_PROPERTY_RW(double, Height);

    virtual BCFViewPoint& GetViewPoint() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFBimSnippet
{
    long TypeCheck = BCFTypeCheck_BimSnippet;

    BCF_PROPERTY_RW(const char*, SnippetType);
    BCF_PROPERTY_RW(bool, IsExternal);
    BCF_PROPERTY_RW(const char*, Reference);
    BCF_PROPERTY_RW(const char*, ReferenceSchema);

    virtual BCFTopic& GetTopic() = NULL;

    virtual bool Remove() = NULL;
};

/// <summary>
/// 
/// </summary>
struct BCFExtensions
{
    long TypeCheck = BCFTypeCheck_Extensions;

    virtual const char* GetElement(BCFEnumeration enumeration, uint16_t ind) = NULL;
    virtual bool        AddElement(BCFEnumeration enumeration, const char* value) = NULL;
    virtual bool        RemoveElement(BCFEnumeration enumeration, const char* element) = NULL;

    virtual BCFProject& GetProject() = NULL;
};
