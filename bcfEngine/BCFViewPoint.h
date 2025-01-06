#pragma once

#include "bcfTypes.h"
#include "XMLFile.h"
#include "ListOf.h"
#include "Point.h"
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

    void Read(_xml::_element& elem, const std::string& folder);

public:
    const char* GetGuid() { return m_Guid.c_str(); }

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

private:
    BCFTopic&                   m_topic;

    GuidStr                     m_Guid;

    std::string                 m_Viewpoint; //name.bcfv
    std::string                 m_Snapshot;  //name.jpg

    ListOf<BCFComponent>           m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    ListOf<BCFComponent>           m_Exceptions;
    
    ListOf<BCFColor>               m_Coloring;

    BCFCamera                   m_cameraType;
    Point                       m_CameraViewPoint;
    Point                       m_CameraDirection;
    Point                       m_CameraUpVector;
    std::string                 m_ViewToWorldScale;
    std::string                 m_FieldOfView;
    std::string                 m_AspectRatio;

    ListOf<BCFLine>                m_Lines;
    ListOf<BCFClippingPlane>       m_ClippingPlanes;
    ListOf<BCFBitmap>              m_Bitmaps;
};

