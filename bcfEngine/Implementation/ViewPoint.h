#pragma once

#include "bcfAPI.h"
#include "bcfTypes.h"
#include "XMLFile.h"
#include "ListOf.h"
#include "XMLPoint.h"
#include "GuidStr.h"

struct Coloring;
struct Line;
struct ClippingPlane;
struct Bitmap;

struct ViewPoint : public XMLFile, public BCFViewPoint
{
public:
    ViewPoint(Topic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

public:
    //BCFViewPoint
    virtual const char* GetGuid() override { return m_Guid.c_str(); }
    virtual const char* GetSnapshot() override { return m_Snapshot.c_str(); }
    virtual bool        GetDefaultVisibility() override { return GetPropertyBool(m_DefaultVisibility); }
    virtual bool        GetSpaceVisible() override { return GetPropertyBool(m_SpacesVisible); }
    virtual bool        GetSpaceBoundariesVisible() override { return GetPropertyBool(m_SpaceBoundariesVisible); }
    virtual bool        GetOpeningsVisible() override { return GetPropertyBool(m_OpeningsVisible); }
    virtual BCFCamera   GetCameraType() override { return m_cameraType; }
    virtual bool        GetCameraViewPoint(BCFPoint& pt) override { return m_CameraViewPoint.GetPoint(pt); }
    virtual bool        GetCameraDirection(BCFPoint& pt) override { return m_CameraDirection.GetPoint(pt); }
    virtual bool        GetCameraUpVector(BCFPoint& pt) override { return m_CameraUpVector.GetPoint(pt); }
    virtual double      GetViewToWorldScale() override { return atof(m_ViewToWorldScale.c_str()); }
    virtual double      GetFieldOfView() override { return atof(m_FieldOfView.c_str()); }
    virtual double      GetAspectRatio() override { return atof(m_AspectRatio.c_str()); }

    virtual bool        SetSnapshot(const char* val) override { VALIDATE(Snapshot, FilePath); return SetPropertyString(val, m_Snapshot); }
    virtual bool        SetDefaultVisibility(bool val) override { return SetPropertyBool(val, m_DefaultVisibility); }
    virtual bool        SetSpaceVisible(bool val) override { return SetPropertyBool(val, m_SpacesVisible); }
    virtual bool        SetSpaceBoundariesVisible(bool val) override { return SetPropertyBool(val, m_SpaceBoundariesVisible); }
    virtual bool        SetOpeningsVisible(bool val) override { return SetPropertyBool(val, m_OpeningsVisible); }
    virtual bool        SetCameraType(BCFCamera val);
    virtual bool        SetCameraViewPoint(BCFPoint* pt) override { return m_CameraViewPoint.SetPoint(pt); }
    virtual bool        SetCameraDirection(BCFPoint* pt) override { return m_CameraDirection.SetPoint(pt); }
    virtual bool        SetCameraUpVector(BCFPoint* pt) override { return m_CameraUpVector.SetPoint(pt); }
    virtual bool        SetViewToWorldScale(double val) override { return SetPropertyReal(val,m_ViewToWorldScale); }
    virtual bool        SetFieldOfView(double val) override { return SetPropertyReal (val, m_FieldOfView); }
    virtual bool        SetAspectRatio(double val) override { return SetPropertyReal (val, m_AspectRatio); }

    virtual BCFComponent* AddSelection(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) override;
    virtual BCFComponent* GetSelection(uint16_t ind) override;

    virtual BCFComponent* AddException(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) override;
    virtual BCFComponent* GetException(uint16_t ind) override;

    virtual BCFBitmap* AddBitmap(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height) override;
    virtual BCFBitmap* GetBitmap(uint16_t ind) override;

    virtual BCFColoring* AddColoring(const char* color) override;
    virtual BCFColoring* GetColoring(uint16_t ind) override;

    virtual BCFLine* AddLine(BCFPoint* start, BCFPoint* end) override;
    virtual BCFLine* GetLine(uint16_t ind) override;

    virtual BCFClippingPlane* AddClippingPlane(BCFPoint* location, BCFPoint* direction) override;
    virtual BCFClippingPlane* GetClippingPlane(uint16_t ind) override;

    virtual BCFTopic& GetTopic() override;

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

    virtual Topic* Topic_() override { return &m_topic; }
    virtual Comment* Comment_() override { return NULL; }

public:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return m_Viewpoint.c_str(); }
    virtual const char* XSDName() override { return "visinfo.xsd"; }
    virtual const char* RootElemName() override { return "VisualizationInfo"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string& folder) override;
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;
    virtual bool Remove();

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
    Topic&                      m_topic;

    GuidStr                     m_Guid;

    std::string                 m_Viewpoint; //name.bcfv
    std::string                 m_Snapshot;  //name.jpg
    std::string                 m_Index;

    ListOfComponents            m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    ListOfComponents            m_Exceptions;
    
    ListOf<Coloring>            m_Coloring;

    BCFCamera                   m_cameraType;
    XMLPoint                    m_CameraViewPoint;
    XMLPoint                    m_CameraDirection;
    XMLPoint                    m_CameraUpVector;
    std::string                 m_ViewToWorldScale;
    std::string                 m_FieldOfView;
    std::string                 m_AspectRatio;

    ListOf<Line>             m_Lines;
    ListOf<ClippingPlane>    m_ClippingPlanes;
    ListOf<Bitmap>           m_Bitmaps;
};

