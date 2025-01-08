#pragma once

#include "bcfTypes.h"
#include "XMLFile.h"
#include "ListOf.h"
#include "XMLPoint.h"
#include "GuidStr.h"

struct BCFComponent;
struct BCFColor;
struct BCFLine;
struct BCFClippingPlane;
struct BCFBitmap;

struct BCFViewPoint : public XMLFile
{
public:
    BCFViewPoint(BCFTopic& topic, const char* guid = NULL);

public:
    const char* GetGuid() { return m_Guid.c_str(); }

    const char* GetSnapshot() { return m_snapshotPath.c_str(); }
    bool        GetDefaultVisibility() { return StrToBool(m_DefaultVisibility); }
    bool        GetSpaceVisible() { return StrToBool(m_SpacesVisible); }
    bool        GetSpaceBoundariesVisible() { return StrToBool(m_SpaceBoundariesVisible); }
    bool        GetOpeningsVisible() { return StrToBool(m_OpeningsVisible); }
    BCFCamera   GetCameraType() { return m_cameraType; }
    bool        GetCameraViewPoint(BCFPoint& pt) { return GetPoint(m_CameraViewPoint, pt); }
    bool        GetCameraDirection(BCFPoint& pt) { return GetPoint(m_CameraDirection, pt); }
    bool        GetCameraUpVector(BCFPoint& pt) { return GetPoint(m_CameraUpVector, pt); }
    double      GetViewToWorldScale() { return atof(m_ViewToWorldScale.c_str()); }
    double      GetFieldOfView() { return atof(m_FieldOfView.c_str()); }
    double      GetAspectRatio() { return atof(m_AspectRatio.c_str()); }

    bool        SetSnapshot(const char* pathFile) { m_snapshotPath.assign(pathFile); return true; }
    bool        SetDefaultVisibility(bool val) { return BoolToStr(val, m_DefaultVisibility); }
    bool        SetSpaceVisible(bool val) { return BoolToStr(val, m_SpacesVisible); }
    bool        SetSpaceBoundariesVisible(bool val) { return BoolToStr(val, m_SpaceBoundariesVisible); }
    bool        SetOpeningsVisible(bool val) { return BoolToStr(val, m_OpeningsVisible); }
    bool        SetCameraType(BCFCamera val) { m_cameraType = val; return true; }
    bool        SetCameraViewPoint(BCFPoint* pt) { return SetPoint(pt, m_CameraViewPoint); }
    bool        SetCameraDirection(BCFPoint* pt) { return SetPoint(pt, m_CameraDirection); }
    bool        SetCameraUpVector(BCFPoint* pt) { return SetPoint(pt, m_CameraUpVector); }
    bool        SetViewToWorldScale(double val) { return RealToStr(val,m_ViewToWorldScale); }
    bool        SetFieldOfView(double val) { return RealToStr (val, m_FieldOfView); }
    bool        SetAspectRatio(double val) { return RealToStr (val, m_AspectRatio); }

    BCFComponent* SelectionAdd();
    BCFComponent* SelectionIterate(BCFComponent* prev);
    bool SelectionRemove(BCFComponent* component);

    BCFComponent* ExceptionsAdd();
    BCFComponent* ExceptionIterate(BCFComponent* prev);
    bool ExceptionRemove(BCFComponent* component);

    BCFColor* ColoringAdd();
    BCFColor* ColoringIterate(BCFColor* prev);
    bool ColoringRemove(BCFColor* coloring);

    BCFLine* LineAdd();
    BCFLine* LineIterate(BCFLine* prev);
    bool LineRemove(BCFLine* line);

    BCFClippingPlane* ClippingPlaneAdd();
    BCFClippingPlane* ClippingPlaneIterate(BCFClippingPlane* prev);
    bool ClippingPlaneRemove(BCFClippingPlane* component);

    BCFBitmap* BitmapAdd();
    BCFBitmap* BitmapIterate(BCFBitmap* prev);
    bool BitmapRemove(BCFBitmap* line);

    bool Remove();

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return m_Viewpoint.c_str(); }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Components(_xml::_element& elem, const std::string& folder);
    void Read_Visibility(_xml::_element& elem, const std::string& folder);
    void Read_ViewSetupHints(_xml::_element& elem, const std::string& folder);
    void Read_PerspectiveCamera(_xml::_element& elem, const std::string& folder);
    void Read_OrthogonalCamera(_xml::_element& elem, const std::string& folder);

    void Write_ViewPoint(_xml_writer& writer, const std::string& folder);

private:
    BCFTopic&                   m_topic;
    std::string                 m_snapshotPath;

    GuidStr                     m_Guid;

    std::string                 m_Viewpoint; //name.bcfv
    std::string                 m_Snapshot;  //name.jpg

    ListOf<BCFComponent>        m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    ListOf<BCFComponent>        m_Exceptions;
    
    ListOf<BCFColor>            m_Coloring;

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

