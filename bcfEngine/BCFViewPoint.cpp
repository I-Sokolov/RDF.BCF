#include "pch.h"
#include "BCFViewPoint.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "BCFColor.h"
#include "BCFLine.h"
#include "BCFClippingPlane.h"
#include "BCFBitmap.h"
#include "BCFComponent.h"

/// <summary>
/// 
/// </summary>
BCFViewPoint::BCFViewPoint(BCFTopic& topic, const char* guid)
    : XMLFile(topic.Project())
    , m_topic(topic)
    , m_Guid(topic.Project(), guid)
    , m_cameraType(BCFCameraPerspective)
    , m_CameraViewPoint(topic.Project())
    , m_CameraDirection(topic.Project())
    , m_CameraUpVector(topic.Project())
    , m_Selection(topic.Project())
    , m_Exceptions(topic.Project())
    , m_Coloring(topic.Project())
    , m_Lines(topic.Project())
    , m_ClippingPlanes(topic.Project())
    , m_Bitmaps(topic.Project())
{
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::Read(_xml::_element& elem, const std::string& folder)
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
void BCFViewPoint::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::Allowed)

    CHILDREN_START
        CHILD_READ(Components)
        CHILD_READ(PerspectiveCamera)
        CHILD_READ(OrthogonalCamera)
        CHILD_GET_LIST(Lines, BCFLine)
        CHILD_GET_LIST(ClippingPlanes, BCFClippingPlane)
        CHILD_GET_LIST(Bitmaps, BCFBitmap)
    CHILDREN_END
}


/// <summary>
/// 
/// </summary>
void  BCFViewPoint::Read_Components(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Selection, BCFComponent)
        CHILD_READ(Visibility)
        CHILD_GET_LIST(Coloring, BCFColor)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  BCFViewPoint::Read_Visibility(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(DefaultVisibility)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_READ(ViewSetupHints)
        CHILD_GET_LIST(Exceptions, BCFComponent)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  BCFViewPoint::Read_ViewSetupHints(_xml::_element& elem, const std::string& folder)
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
void  BCFViewPoint::Read_PerspectiveCamera(_xml::_element& elem, const std::string& folder)
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
void  BCFViewPoint::Read_OrthogonalCamera(_xml::_element& elem, const std::string& folder)
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

#if 0
BCFIndex BCFViewPoint::GetIndex()
{
    BCFIndex N = m_topic.ViewPointsCount();
    for (BCFIndex i = 0; i < N; i++) {
        auto vp = m_topic.ViewPointGet(i);
        if (vp == this) {
            return i;
        }
    }

    log().add(Log::Level::error, "Out of range", "ViewPoint %s is not from topic %s", GetGuid(), m_topic.GetGuid());
    return BCFIndex_ERROR;
}
#endif
