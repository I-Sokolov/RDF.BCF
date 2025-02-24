#include "pch.h"
#include "ViewPoint.h"
#include "Project.h"
#include "Topic.h"
#include "Coloring.h"
#include "Line.h"
#include "ClippingPlane.h"
#include "Bitmap.h"
#include "Component.h"
#include "FileSystem.h"
#include "Comment.h"

/// <summary>
/// 
/// </summary>
ViewPoint::ViewPoint(Topic& topic, ListOfBCFObjects* parentList, const char* guid)
    : XMLFile(topic.Project_(), parentList)
    , m_topic(topic)
    , m_Guid(topic.Project_(), guid)
    , m_cameraType(BCFCameraPerspective)
    , m_CameraViewPoint(*this)
    , m_CameraDirection(*this)
    , m_CameraUpVector(*this)
    , m_Selection(*this)
    , m_Exceptions(*this)
    , m_Coloring(*this)
    , m_Lines(*this)
    , m_ClippingPlanes(*this)
    , m_Bitmaps(*this)
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
bool ViewPoint::Validate(bool fix)
{
    if (fix) {
       
        if (!m_CameraViewPoint.IsSet())
            m_CameraViewPoint.SetPoint(0, 0, 0);
        if (!m_CameraDirection.IsSet())
            m_CameraDirection.SetPoint(1, 0, 0);
        if (!m_CameraUpVector.IsSet())
            m_CameraUpVector.SetPoint(0, 0, 1);
        if (GetAspectRatio() < 1e-8)
            SetAspectRatio(1);
        if (GetCameraType() == BCFCameraPerspective) {
            if (GetFieldOfView() < 1e-8 || GetFieldOfView() > 180 - 1e-8)
                SetFieldOfView(90);
        }
        else {
            if (GetViewToWorldScale() < 1e-8)
                SetViewToWorldScale(1);
        }
    }

    bool valid = true;

    valid = m_Selection.Validate(fix) && valid;
    valid = m_Exceptions.Validate(fix) && valid;
    valid = m_Coloring.Validate(fix) && valid;
    valid = m_Lines.Validate(fix) && valid;
    valid = m_ClippingPlanes.Validate(fix) && valid;
    valid = m_Bitmaps.Validate(fix) && valid;

    REQUIRED(CameraViewPoint, m_CameraViewPoint.IsSet());
    REQUIRED(CameraDirection, m_CameraDirection.IsSet());
    REQUIRED(CameraUpVector, m_CameraUpVector.IsSet());
    REQUIRED(AspectRatio, GetAspectRatio() > 0);

    REQUIRED(CameraViewPoint, m_CameraViewPoint.IsSet());
    REQUIRED(CameraDirection, m_CameraDirection.IsSet());
    REQUIRED(CameraUpVector, m_CameraUpVector.IsSet());
    REQUIRED(AspectRatio, GetAspectRatio() > 0);

    if (GetCameraType() == BCFCameraPerspective) {
        REQUIRED(FieldOfView, GetFieldOfView() > 0 && GetFieldOfView() < 180);
    }
    else {
        REQUIRED(ViewToWorldScale, GetViewToWorldScale() != 0);
    }

    return valid;
}

/// <summary>
/// 
/// </summary>
void ViewPoint::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
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
void ViewPoint::Write_ViewPoint(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Viewpoint);
    WRITE_CONTENT(Snapshot);
    WRITE_CONTENT(Index);
}

/// <summary>
/// 
/// </summary>
void ViewPoint::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    std::string vpGuid;

    ATTRS_START
        ATTR_GET_STR(Guid, vpGuid) //guid read from .bcfv file
    ATTRS_END(UnknownNames::Allowed)

    //if (!vpGuid.empty() && strcmp(vpGuid.c_str(), m_Guid.c_str())) {
    //    printf("Data inconsistent: %s\n", folder.c_str());
    //}

    CHILDREN_START
        CHILD_READ(Components)
        CHILD_READ(PerspectiveCamera)
        CHILD_READ(OrthogonalCamera)
        CHILD_GET_LIST(Lines, Line)
        CHILD_GET_LIST(ClippingPlanes, ClippingPlane)
        CHILD_GET_LIST_CONDITIONAL(Bitmaps, Bitmap, Project_().GetVersion() > BCFVer_2_0)
        CHILD_ADD_TO_LIST_CONDITIONAL(Bitmaps, Bitmaps, Project_().GetVersion() == BCFVer_2_0)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void ViewPoint::WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr)
{
    ATTR_ADD(Guid);
    __super::WriteRootElem(writer, folder, attr);
}

void ViewPoint::WriteRootContent(_xml_writer& writer, const std::string& folder)
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
void  ViewPoint::Read_Components(_xml::_element& elem, const std::string& folder)
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
void  ViewPoint::Write_Components(_xml_writer& writer, const std::string& folder)
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
void ViewPoint::Write_Visibility(_xml_writer& writer, const std::string& folder)
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
void ViewPoint::Write_PerspectiveCamera(_xml_writer& writer, const std::string& folder)
{
    WRITE_MEMBER(CameraViewPoint);
    WRITE_MEMBER(CameraDirection);
    WRITE_MEMBER(CameraUpVector);
    WRITE_CONTENT(FieldOfView);
    WRITE_CONTENT(AspectRatio);
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

/// <summary>
/// 
/// </summary>
void ViewPoint::Write_OrthogonalCamera(_xml_writer& writer, const std::string& folder)
{
    WRITE_MEMBER(CameraViewPoint);
    WRITE_MEMBER(CameraDirection);
    WRITE_MEMBER(CameraUpVector);
    WRITE_CONTENT(ViewToWorldScale);
    WRITE_CONTENT(AspectRatio);
}


/// <summary>
/// 
/// </summary>
BCFComponent* ViewPoint::AddSelection(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Selection.Add(*this, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* ViewPoint::GetSelection(uint16_t ind)
{
    return m_Selection.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFComponent* ViewPoint::AddException(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Exceptions.Add(*this, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* ViewPoint::GetException(uint16_t ind)
{
    return m_Exceptions.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFBitmap* ViewPoint::AddBitmap(const char* filePath, BCFBitmapFormat format, BCFPoint* location, BCFPoint* normal, BCFPoint* up, double height)
{
    auto bitmap = new Bitmap(*this, &m_Bitmaps);

    bool ok = true;

        ok = ok && bitmap->SetReference(filePath);
        ok = ok && bitmap->SetFormat(format);
        ok = ok && bitmap->SetLocation(location);
        ok = ok && bitmap->SetNormal(normal);
        ok = ok && bitmap->SetUp(up);
        ok = ok && bitmap->SetHeight(height);

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
BCFBitmap* ViewPoint::GetBitmap(uint16_t ind)
{
    return m_Bitmaps.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFColoring* ViewPoint::AddColoring(const char* color)
{
    auto obj = new Coloring(*this, &m_Coloring);

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
BCFColoring* ViewPoint::GetColoring(uint16_t ind)
{
    return m_Coloring.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFLine* ViewPoint::AddLine(BCFPoint* start, BCFPoint* end)
{
    auto obj = new Line(*this, &m_Lines);

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
BCFLine* ViewPoint::GetLine(uint16_t ind)
{
    return m_Lines.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFClippingPlane* ViewPoint::AddClippingPlane(BCFPoint* location, BCFPoint* direction)
{
    auto obj = new ClippingPlane(*this, &m_ClippingPlanes);

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
BCFClippingPlane* ViewPoint::GetClippingPlane(uint16_t ind)
{
    return m_ClippingPlanes.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
void ViewPoint::AfterRead(const std::string& folder)
{
    if (Project_().GetVersion() < BCFVer_3_0) {
        if (m_AspectRatio.empty()) {
            m_AspectRatio.assign("1");
        }
    }

    m_Bitmaps.AfterRead(folder);
    m_Selection.AfterRead(folder);
}

/// <summary>
/// 
/// </summary>
bool ViewPoint::Remove()
{
    //check references
    BCFComment* comment = NULL;
    uint16_t ind = 0;
    while (NULL != (comment = m_topic.GetComment(ind++))) {
        if (this == comment->GetViewPoint()) {
            Log_().add(Log::Level::error, "ViewPoint is used", "Can not delete used viewpoint %s", m_Guid.c_str());
            return false;
        }
    }

    //
    return RemoveImpl();
}

BCFTopic& ViewPoint::GetTopic()
{
    return m_topic;
}

bool ViewPoint::SetCameraType(BCFCamera val) 
{ 
    if (m_cameraType != val){ 
        m_cameraType = val; 
        return MarkModified(); 
    } 
    return true; 
}
