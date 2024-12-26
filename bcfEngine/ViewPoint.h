#pragma once

#include "XMLFile.h"
#include "GUIDable.h"
#include "Point.h"
#include "Line.h"
#include "ClippingPlane.h"
#include "Bitmap.h"
#include "Component.h"
#include "Color.h"

class ViewPoint : public GUIDable, public XMLFile
{
public:
    ViewPoint(BCFProject& project);

    void Read(_xml::_element& elem, const std::string& folder);

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
    std::string m_Viewpoint; //name.bcfv
    std::string m_Snapshot;  //name.jpg

    std::vector<Component>      m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    std::vector<Component>      m_Exceptions;
    
    std::vector<Color>          m_Coloring;

    BCFCamera                   m_cameraType;
    Point                       m_CameraViewPoint;
    Point                       m_CameraDirection;
    Point                       m_CameraUpVector;
    std::string                 m_ViewToWorldScale;
    std::string                 m_FieldOfView;
    std::string                 m_AspectRatio;

    std::vector<Line>           m_Lines;
    std::vector<ClippingPlane>  m_ClippingPlanes;
    std::vector<Bitmap>         m_Bitmaps;
};

