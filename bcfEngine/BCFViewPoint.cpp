#include "pch.h"
#include "BCFViewPoint.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "BCFColoring.h"
#include "BCFLine.h"
#include "BCFClippingPlane.h"
#include "BCFBitmap.h"
#include "BCFComponent.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFViewPoint::BCFViewPoint(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid)
    : XMLFile(topic.Project(), parentList)
    , m_topic(topic)
    , m_Guid(topic.Project(), guid)
    , m_GuidBCFV(topic.Project(), guid)
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
        CHILD_GET_CONTENT(Index)
    CHILDREN_END

    if (!ReadFile(folder)) {
        throw std::exception("Failed to read viewpoint");
    }

    //
    m_Snapshot = AbsolutePath(m_Snapshot, folder);
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    m_Snapshot = CopyToRelative(m_Snapshot, folder, NULL);
    
    m_Viewpoint = m_Guid.c_str();
    assert(!m_Viewpoint.empty());
    m_Viewpoint.append(".bcfv");

    //
    if (!WriteFile(folder)) {
        throw std::exception();
    }
    Attributes attr;
    ATTR_ADD(Guid);

    WRITE_ELEM(ViewPoint);

    //
    m_Snapshot = AbsolutePath(m_Snapshot, folder);
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::Write_ViewPoint(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Viewpoint);
    WRITE_CONTENT(Snapshot);
    WRITE_CONTENT(Index);
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET_STR(Guid, m_GuidBCFV) //guid read from .bcfv file
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
void BCFViewPoint::WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr)
{
    ATTR_ADD(Guid);
    __super::WriteRootElem(writer, folder, attr);
}

void BCFViewPoint::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    Attributes attr;

    WRITE_ELEM(Components);
    if (m_cameraType == BCFCameraPerspective) {
        WRITE_ELEM(PerspectiveCamera);
    }
    else {
        WRITE_ELEM(OrthogonalCamera);
    }
    WRITE_LIST(Line);
    WRITE_LIST(ClippingPlane);
    WRITE_LIST(Bitmap);
}

/// <summary>
/// 
/// </summary>
void  BCFViewPoint::Read_Components(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Selection, Component)
        CHILD_GET_LIST(Coloring, Color)
        CHILD_READ(Visibility)

        CHILD_READ(ViewSetupHints) //v2.1

    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void  BCFViewPoint::Write_Components(_xml_writer& writer, const std::string& folder)
{
    WRITE_LIST_EX(Selection, Component);

    Attributes attr;
    ATTR_ADD(DefaultVisibility);
    WRITE_ELEM(Visibility);

    WRITE_LIST_EX(Coloring, Color);
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
        CHILD_GET_LIST(Exceptions, Component)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::Write_Visibility(_xml_writer& writer, const std::string& folder)
{
    Attributes attr;
    ATTR_ADD(SpacesVisible);
    ATTR_ADD(SpaceBoundariesVisible);
    ATTR_ADD(OpeningsVisible);
    writer.writeTag("ViewSetupHints", attr, "");

    WRITE_LIST_EX(Exceptions, Component);
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
void BCFViewPoint::Write_PerspectiveCamera(_xml_writer& writer, const std::string& folder)
{
    REQUIRED(CameraViewPoint, m_CameraViewPoint.IsSet());
    REQUIRED(CameraDirection, m_CameraDirection.IsSet());
    REQUIRED(CameraUpVector, m_CameraUpVector.IsSet());
    REQUIRED(FieldOfView, GetFieldOfView() > 0 && GetFieldOfView() < 180);
    REQUIRED(AspectRatio, GetAspectRatio() > 0);

    WRITE_MEMBER(CameraViewPoint);
    WRITE_MEMBER(CameraDirection);
    WRITE_MEMBER(CameraUpVector);
    WRITE_CONTENT(FieldOfView);
    WRITE_CONTENT(AspectRatio);
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

/// <summary>
/// 
/// </summary>
void BCFViewPoint::Write_OrthogonalCamera(_xml_writer& writer, const std::string& folder)
{
    REQUIRED(CameraViewPoint, m_CameraViewPoint.IsSet());
    REQUIRED(CameraDirection, m_CameraDirection.IsSet());
    REQUIRED(CameraUpVector, m_CameraUpVector.IsSet());
    REQUIRED(ViewToWorldScale, GetViewToWorldScale() != 0);
    REQUIRED(AspectRatio, GetAspectRatio() > 0);

    WRITE_MEMBER(CameraViewPoint);
    WRITE_MEMBER(CameraDirection);
    WRITE_MEMBER(CameraUpVector);
    WRITE_CONTENT(ViewToWorldScale);
    WRITE_CONTENT(AspectRatio);
}


/// <summary>
/// 
/// </summary>
BCFComponent* BCFViewPoint::SelectionAdd(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Selection.Add(*this, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFViewPoint::SelectionIterate(BCFComponent* prev)
{
    return m_Selection.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFViewPoint::ExceptionsAdd(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Exceptions.Add(*this, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFViewPoint::ExceptionsIterate(BCFComponent* prev)
{
    return m_Exceptions.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFBitmap* BCFViewPoint::BitmapAdd(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height)
{
    auto bitmap = new BCFBitmap(*this, &m_Bitmaps);

    bool ok = true;

        ok = ok && bitmap->SetReference(filePath);
        ok = ok && bitmap->SetFormat(format);
        ok = ok && bitmap->SetLocation(location);
        ok = ok && bitmap->SetNormal(normal);
        ok = ok && bitmap->SetUp(up);
        ok = ok && bitmap->SetHeight(height);
        ok = ok && m_topic.UpdateAuthor();

    if (!ok) {
        delete bitmap;
        bitmap = NULL;
    }

    if (bitmap) {
        m_Bitmaps.Add(bitmap);
        return bitmap;
    }
    else {
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFBitmap* BCFViewPoint::BitmapIterate(BCFBitmap* prev)
{
    return m_Bitmaps.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFColoring* BCFViewPoint::ColoringAdd(const char* color)
{
    auto obj = new BCFColoring(*this, &m_Coloring);

    bool ok = true;

    ok = ok && obj->SetColor(color);

    if (ok) {
        m_Coloring.Add(obj);
        return obj;
    }
    else {
        delete obj;
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFColoring* BCFViewPoint::ColoringIterate(BCFColoring* prev)
{
    return m_Coloring.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFLine* BCFViewPoint::LineAdd(BCFPoint* start, BCFPoint* end)
{
    auto obj = new BCFLine(*this, &m_Lines);

    bool ok = true;

    ok = ok && start && obj->SetStartPoint(start);
    ok = ok && end && obj->SetEndPoint(end);

    if (ok) {
        m_Lines.Add(obj);
        return obj;
    }
    else {
        delete obj;
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFLine* BCFViewPoint::LineIterate(BCFLine* prev)
{
    return m_Lines.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFClippingPlane* BCFViewPoint::ClippingPlaneAdd(BCFPoint* location, BCFPoint* direction)
{
    auto obj = new BCFClippingPlane(*this, &m_ClippingPlanes);

    bool ok = true;

    ok = ok && location && obj->SetLocation(location);
    ok = ok && direction && obj->SetDirection(direction);

    if (ok) {
        m_ClippingPlanes.Add(obj);
        return obj;
    }
    else {
        delete obj;
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFClippingPlane* BCFViewPoint::ClippingPlaneIterate(BCFClippingPlane* prev)
{
    return m_ClippingPlanes.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
void BCFViewPoint::UpgradeReadVersion()
{
    if (Project().GetVersion() < BCFVer_3_0) {
        if (m_AspectRatio.empty()) {
            m_AspectRatio.assign("1");
        }
    }

    m_Bitmaps.UpgradeReadVersion();
}
