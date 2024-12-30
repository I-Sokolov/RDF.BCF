#pragma once

#include "XMLFile.h"
#include "GUIDable.h"
#include "Point.h"
#include "Line.h"
#include "ClippingPlane.h"
#include "Bitmap.h"
#include "Component.h"
#include "Color.h"

class ViewPoint : public XMLFile
{
public:
    ViewPoint(Topic& topic, const char* guid = NULL);

    void Read(_xml::_element& elem, const std::string& folder);

public:
    BCFIndex GetIndex();

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
    Topic&      m_topic;

    BCFGuid     m_Guid;

    std::string m_Viewpoint; //name.bcfv
    std::string m_Snapshot;  //name.jpg

    OwningList<Component>       m_Selection;
    
    std::string                 m_DefaultVisibility;
    std::string                 m_SpacesVisible;
    std::string                 m_SpaceBoundariesVisible;
    std::string                 m_OpeningsVisible;
    OwningList<Component>       m_Exceptions;
    
    OwningList<Color>           m_Coloring;

    BCFCamera                   m_cameraType;
    Point                       m_CameraViewPoint;
    Point                       m_CameraDirection;
    Point                       m_CameraUpVector;
    std::string                 m_ViewToWorldScale;
    std::string                 m_FieldOfView;
    std::string                 m_AspectRatio;

    OwningList<Line>            m_Lines;
    OwningList<ClippingPlane>   m_ClippingPlanes;
    OwningList<Bitmap>          m_Bitmaps;
};

