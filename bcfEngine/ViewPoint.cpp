#include "pch.h"
#include "bcfEngine.h"
#include "ViewPoint.h"
#include "BCFProject.h"


/// <summary>
/// 
/// </summary>
ViewPoint::ViewPoint(BCFProject& project)
    : GUIDable(NULL)
    , XMLFile(project)
    , m_cameraType(BCFCameraPerspective)
    , m_CameraViewPoint(project)
    , m_CameraDirection(project)
    , m_CameraUpVector(project)
{

}

/// <summary>
/// 
/// </summary>
void ViewPoint::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Viewpoint)
        CHILD_GET_CONTENT(Snapshot)
    CHILDREN_END

    if (!ReadFile(folder)) {
        throw std::exception();
    }
}

/// <summary>
/// 
/// </summary>
void ViewPoint::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::Allowed)

    CHILDREN_START
        CHILD_READ(Components)
        CHILD_READ(PerspectiveCamera)
        CHILD_READ(OrthogonalCamera)
        CHILD_GET_LIST(Lines, Line)
        CHILD_GET_LIST(ClippingPlanes, ClippingPlane)
        CHILD_GET_LIST(Bitmaps, Bitmap)
    CHILDREN_END
}


/// <summary>
/// 
/// </summary>
void  ViewPoint::Read_Components(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Selection, Component)
        CHILD_READ(Visibility)
        CHILD_GET_LIST(Coloring, Color)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  ViewPoint::Read_Visibility(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(DefaultVisibility)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_READ(ViewSetupHints)
        CHILD_GET_LIST(Exceptions, Component)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  ViewPoint::Read_ViewSetupHints(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(SpacesVisible)
        ATTR_GET(SpaceBoundariesVisible)
        ATTR_GET(OpeningsVisible)
    ATTRS_END(UnknownNames::NotAllowed)
}

/// <summary>
/// 
/// </summary>
void  ViewPoint::Read_PerspectiveCamera(_xml::_element& elem, const std::string& folder)
{
    m_cameraType = BCFCameraPerspective;

    CHILDREN_START
        CHILD_READ_MEMBER(CameraViewPoint)
        CHILD_READ_MEMBER(CameraDirection)
        CHILD_READ_MEMBER(CameraUpVector)
        CHILD_GET_CONTENT(FieldOfView)
        CHILD_GET_CONTENT(AspectRatio)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  ViewPoint::Read_OrthogonalCamera(_xml::_element& elem, const std::string& folder)
{
    m_cameraType = BCFCameraOrthogonal;

    CHILDREN_START
        CHILD_READ_MEMBER(CameraViewPoint) 
        CHILD_READ_MEMBER(CameraDirection) 
        CHILD_READ_MEMBER(CameraUpVector)
        CHILD_GET_CONTENT(ViewToWorldScale)
        CHILD_GET_CONTENT(AspectRatio)
    CHILDREN_END
}


