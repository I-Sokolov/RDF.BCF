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
    virtual Type Get##PropName() = 0;

#define BCF_PROPERTY_RW(Type, PropName)               \
    virtual Type Get##PropName()           = 0;    \
    virtual bool Set##PropName(Type value) = 0;

#define BCF_PROPERTY_PT(PropName)                     \
    virtual bool Get##PropName(BCFPoint& pt) = 0;  \
    virtual bool Set##PropName(BCFPoint* pt) = 0;

/// <summary>
/// Use static Create method to create prject, and Delete to dispose
/// All BCF obects lifetime until BCFProject::Delete
/// All strings are valid untile next call or BCFProject::Delete
/// </summary>
struct BCFProject
{
    long TypeCheck = BCFTypeCheck_Project;

    static RDFBCF_EXPORT BCFProject* Create(const char* projectId = NULL);

    virtual bool            Delete(void) = 0;
    virtual const char*     GetErrors(bool cleanLog = true) = 0;

    virtual bool            ReadFile(const char* bcfFilePath, bool autofix) = 0;
    virtual bool            WriteFile(const char* bcfFilePath, BCFVersion version) = 0;

    virtual bool            SetOptions(const char* user, bool autoExtent = true, bool validateIfcGuids = false) = 0;
    virtual BCFExtensions&  GetExtensions() = 0;

    virtual BCFTopic*       GetTopic(uint16_t ind) = 0;
    virtual BCFTopic*       AddTopic(const char* type, const char* title, const char* status, const char* guid = NULL) = 0;

    virtual bool            IsModified() = 0;

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

    virtual BCFBimFile* GetBimFile(uint16_t ind) = 0;
    virtual BCFBimFile* AddBimFile(const char* filePath, bool isExternal) = 0;

    virtual BCFComment* GetComment(uint16_t ind) = 0;
    virtual BCFComment* AddComment(const char* guid = NULL) = 0;

    virtual BCFViewPoint* GetViewPoint(uint16_t ind) = 0;
    virtual BCFViewPoint* AddViewPoint(const char* guid = NULL) = 0;

    virtual BCFDocumentReference* GetDocumentReference(uint16_t ind) = 0;
    virtual BCFDocumentReference* AddDocumentReference(const char* filePath, bool isExternal, const char* guid = NULL) = 0;

    virtual BCFBimSnippet* GetBimSnippet(bool forceCreate) = 0;

    virtual const char* GetReferenceLink(uint16_t ind) = 0;
    virtual bool AddReferenceLink(const char* value) = 0;
    virtual bool RemoveReferenceLink(const char* value) = 0;

    virtual const char* GetLabel(uint16_t ind) = 0;
    virtual bool AddLabel(const char* value) = 0;
    virtual bool RemoveLabel(const char* value) = 0;

    virtual BCFTopic* GetRelatedTopic(uint16_t ind) = 0;
    virtual bool AddRelatedTopic(BCFTopic* value) = 0;
    virtual bool RemoveRelatedTopic(BCFTopic* value) = 0;

    virtual BCFProject& GetProject() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFTopic& GetTopic() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFComponent* GetSelection(uint16_t ind) = 0;
    virtual BCFComponent* AddSelection(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = 0;

    virtual BCFComponent* GetException(uint16_t ind) = 0;
    virtual BCFComponent* AddException(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = 0;

    virtual BCFColoring* GetColoring(uint16_t ind) = 0;
    virtual BCFColoring* AddColoring(const char* color) = 0;

    virtual BCFBitmap* GetBitmap(uint16_t ind) = 0;
    virtual BCFBitmap* AddBitmap(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height) = 0;

    virtual BCFLine* GetLine(uint16_t ind) = 0;
    virtual BCFLine* AddLine(BCFPoint* start, BCFPoint* end) = 0;

    virtual BCFClippingPlane* GetClippingPlane(uint16_t ind) = 0;
    virtual BCFClippingPlane* AddClippingPlane(BCFPoint* start, BCFPoint* end) = 0;

    virtual BCFTopic& GetTopic() = 0;

    virtual bool Remove() = 0;
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

    virtual bool SetFilePath(const char* filePath, bool isExternal) = 0;

    virtual BCFTopic& GetTopic() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFViewPoint& GetViewPoint() = 0;
    virtual BCFColoring* GetColoring() = 0;

    virtual bool Remove() = 0;
};

/// <summary>
/// 
/// </summary>
struct BCFColoring
{
    long TypeCheck = BCFTypeCheck_Coloring;

    BCF_PROPERTY_RW(const char*, Color)

    virtual BCFComponent* GetComponent(uint16_t ind) = 0;
    virtual BCFComponent* AddComponent(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) = 0;

    virtual BCFViewPoint& GetViewPoint() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFTopic& GetTopic() = 0;

    virtual bool Remove() = 0;
};

/// <summary>
/// 
/// </summary>
struct BCFLine
{
    long TypeCheck = BCFTypeCheck_Line;

    BCF_PROPERTY_PT(StartPoint);
    BCF_PROPERTY_PT(EndPoint);

    virtual BCFViewPoint& GetViewPoint() = 0;

    virtual bool Remove() = 0;
};

/// <summary>
/// 
/// </summary>
struct BCFClippingPlane
{
    long TypeCheck = BCFTypeCheck_ClippingPlane;

    BCF_PROPERTY_PT(Location);
    BCF_PROPERTY_PT(Direction);

    virtual BCFViewPoint& GetViewPoint() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFViewPoint& GetViewPoint() = 0;

    virtual bool Remove() = 0;
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

    virtual BCFTopic& GetTopic() = 0;

    virtual bool Remove() = 0;
};

/// <summary>
/// 
/// </summary>
struct BCFExtensions
{
    long TypeCheck = BCFTypeCheck_Extensions;

    virtual const char* GetElement(BCFEnumeration enumeration, uint16_t ind) = 0;
    virtual bool        AddElement(BCFEnumeration enumeration, const char* value) = 0;
    virtual bool        RemoveElement(BCFEnumeration enumeration, const char* element) = 0;

    virtual BCFProject& GetProject() = 0;
};
