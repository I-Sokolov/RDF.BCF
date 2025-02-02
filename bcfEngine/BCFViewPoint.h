#pragma once

#include "bcfTypes.h"
#include "XMLFile.h"
#include "ListOf.h"
#include "XMLPoint.h"
#include "GuidStr.h"

struct BCFColoring;
struct BCFLine;
struct BCFClippingPlane;
struct BCFBitmap;

struct BCFViewPoint : public XMLFile
{
public:
    BCFViewPoint(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

public:
    const char* GetGuid() { return m_Guid.c_str(); }

    const char* GetSnapshot() { return m_Snapshot.c_str(); }
    bool        GetDefaultVisibility() { return StrToBool(m_DefaultVisibility); }
    bool        GetSpaceVisible() { return StrToBool(m_SpacesVisible); }
    bool        GetSpaceBoundariesVisible() { return StrToBool(m_SpaceBoundariesVisible); }
    bool        GetOpeningsVisible() { return StrToBool(m_OpeningsVisible); }
    BCFCamera   GetCameraType() { return m_cameraType; }
    bool        GetCameraViewPoint(BCFPoint& pt) { return m_CameraViewPoint.GetPoint(pt); }
    bool        GetCameraDirection(BCFPoint& pt) { return m_CameraDirection.GetPoint(pt); }
    bool        GetCameraUpVector(BCFPoint& pt) { return m_CameraUpVector.GetPoint(pt); }
    double      GetViewToWorldScale() { return atof(m_ViewToWorldScale.c_str()); }
    double      GetFieldOfView() { return atof(m_FieldOfView.c_str()); }
    double      GetAspectRatio() { return atof(m_AspectRatio.c_str()); }

    bool        SetSnapshot(const char* val) { UNNULL; VALIDATE(Snapshot, FilePath); m_Snapshot.assign(val); return true; }
    bool        SetDefaultVisibility(bool val) { return BoolToStr(val, m_DefaultVisibility); }
    bool        SetSpaceVisible(bool val) { return BoolToStr(val, m_SpacesVisible); }
    bool        SetSpaceBoundariesVisible(bool val) { return BoolToStr(val, m_SpaceBoundariesVisible); }
    bool        SetOpeningsVisible(bool val) { return BoolToStr(val, m_OpeningsVisible); }
    bool        SetCameraType(BCFCamera val) { m_cameraType = val; return true; }
    bool        SetCameraViewPoint(BCFPoint* pt) { return m_CameraViewPoint.SetPoint(pt); }
    bool        SetCameraDirection(BCFPoint* pt) { return m_CameraDirection.SetPoint(pt); }
    bool        SetCameraUpVector(BCFPoint* pt) { return m_CameraUpVector.SetPoint(pt); }
    bool        SetViewToWorldScale(double val) { return RealToStr(val,m_ViewToWorldScale); }
    bool        SetFieldOfView(double val) { return RealToStr (val, m_FieldOfView); }
    bool        SetAspectRatio(double val) { return RealToStr (val, m_AspectRatio); }

    BCFComponent* SelectionAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL);
    BCFComponent* SelectionIterate(BCFComponent* prev);

    BCFComponent* ExceptionsAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL);
    BCFComponent* ExceptionsIterate(BCFComponent* prev);

    BCFBitmap* BitmapAdd(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height);
    BCFBitmap* BitmapIterate(BCFBitmap* prev);

    BCFColoring* ColoringAdd(const char* color);
    BCFColoring* ColoringIterate(BCFColoring* prev);

    BCFLine* LineAdd(BCFPoint* start, BCFPoint* end);
    BCFLine* LineIterate(BCFLine* prev);

    BCFClippingPlane* ClippingPlaneAdd(BCFPoint* location, BCFPoint* direction);
    BCFClippingPlane* ClippingPlaneIterate(BCFClippingPlane* prev);

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return m_Viewpoint.c_str(); }
    virtual const char* XSDName() override { return "visinfo.xsd"; }
    virtual const char* RootElemName() override { return "VisualizationInfo"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string& folder) override;
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

private:
    void Read_Components(_xml::_element& elem, const std::string& folder);
    void Read_Visibility(_xml::_element& elem, const std::string& folder);
    void Read_ViewSetupHints(_xml::_element& elem, const std::string& folder);
    void Read_PerspectiveCamera(_xml::_element& elem, const std::string& folder);
    void Read_OrthogonalCamera(_xml::_element& elem, const std::string& folder);

    void Write_ViewPoint(_xml_writer& writer, const std::string& folder);

    void Write_Components(_xml_writer& writer, const std::string& folder);
    void Write_Visibility(_xml_writer& writer, const std::string& folder);
    void Write_PerspectiveCamera(_xml_writer& writer, const std::string& folder);
    void Write_OrthogonalCamera(_xml_writer& writer, const std::string& folder);

private:
    BCFTopic&                   m_topic;

    GuidStr                     m_Guid;

    std::string                 m_Viewpoint; //name.bcfv
    std::string                 m_Snapshot;  //name.jpg
    std::string                 m_Index;

    ListOfBCFComponents         m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    ListOfBCFComponents         m_Exceptions;
    
    ListOf<BCFColoring>            m_Coloring;

    BCFCamera                   m_cameraType;
    XMLPoint                    m_CameraViewPoint;
    XMLPoint                    m_CameraDirection;
    XMLPoint                    m_CameraUpVector;
    std::string                 m_ViewToWorldScale;
    std::string                 m_FieldOfView;
    std::string                 m_AspectRatio;

    ListOf<BCFLine>             m_Lines;
    ListOf<BCFClippingPlane>    m_ClippingPlanes;
    ListOf<BCFBitmap>           m_Bitmaps;
};

