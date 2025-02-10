#include "pch.h"
#include "Component.h"
#include "ViewPoint.h"
#include "Coloring.h"
#include "Project.h"
#include "Topic.h"
#include "File.h"

/// <summary>
/// 
/// </summary>
Component::Component(ViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.GetProject(), parentList)
    , m_pViewPoint(&viewPoint)
    , m_pColoring(NULL)
    , m_Visible("true")
{
}

/// <summary>
/// 
/// </summary>
Component::Component(Coloring& coloring, ListOfBCFObjects* parentList)
    : BCFObject(coloring.GetProject(), parentList)
    , m_pViewPoint(NULL)
    , m_pColoring(&coloring)
    , m_Visible("true")
{
}

/// <summary>
/// 
/// </summary>
Topic& Component::GetTopic()
{
    if (m_pViewPoint) {
        return m_pViewPoint->GetTopic();
    }
    else if (m_pColoring) {
        return m_pColoring->GetTopic();
    }
    else {
        assert(false);
        return *(new Topic(GetProject(), NULL, NULL));
    }
}

/// <summary>
/// 
/// </summary>
void Component::Read(_xml::_element& elem, const std::string&)
{
    ATTRS_START
        ATTR_GET(IfcGuid)
        //v2.0
        ATTR_GET(Selected)
        ATTR_GET(Visible)
        ATTR_GET(Color)
    ATTRS_END(UnknownNames::NotAllowed)


    CHILDREN_START
        CHILD_GET_CONTENT(OriginatingSystem)
        CHILD_GET_CONTENT(AuthoringToolId)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Component::AfterRead(const std::string&)
{
    if (GetProject().GetVersion() < BCFVer_2_1) {
        assert(m_pViewPoint);

        if (!StrToBool(m_Visible) && m_pViewPoint) {
            m_pViewPoint->ExceptionsAdd(m_IfcGuid.c_str(), m_AuthoringToolId.c_str(), m_OriginatingSystem.c_str());
        }

        if (!m_Color.empty() && m_pViewPoint) {
            BCFColoring* coloring = NULL;            
            uint16_t ind = 0;
            while ((coloring = m_pViewPoint->ColoringGetAt(ind++)) != NULL) {
                if (0 == strcmp(coloring->GetColor(), m_Color.c_str())) {
                    break;
                }
            }

            if (!coloring) {
                coloring = m_pViewPoint->ColoringAdd(m_Color.c_str());
            }

            coloring->ComponentAdd(m_IfcGuid.c_str(), m_AuthoringToolId.c_str(), m_OriginatingSystem.c_str());
        }

        if (!StrToBool(m_Selected)) {
            Remove();
        }
    }
}

/// <summary>
/// 
/// </summary>
bool Component::Validate(bool fix)
{
    bool valid = true;

    if (m_AuthoringToolId.empty()) {
        REQUIRED_PROP(IfcGuid);
    }

    if (!ValidateIfcGuid()) {
        valid = false;
    }

    if (!valid && fix) {
        Remove();
        return true;
    }

    return valid;
}

/// <summary>
/// 
/// </summary>
bool Component::ValidateIfcGuid()
{
    if (m_IfcGuid.empty()) 
        return true;

    if (!GetProject().GetValidateIfcGuids())
        return true;

    auto& topic = GetTopic();
    BCFFile* bcffile = NULL;
    uint16_t ind = 0;
    while (NULL != (bcffile = topic.FileGetAt(ind++))) {
        
        auto file = dynamic_cast<File*>(bcffile);
        assert(file);
        if (file) {

            if (file->HasComponent(m_IfcGuid)) {
                return true;
            }
        }
    }

    Log().add(Log::Level::error, "Invalud IfcGUID", "Invalid IFC GlobalId: %s", m_IfcGuid.c_str());
    return false;
}

/// <summary>
/// 
/// </summary>
void Component::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{ 
    XMLFile::Attributes attr;
    ATTR_ADD(IfcGuid);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_CONTENT(OriginatingSystem);
    WRITE_CONTENT(AuthoringToolId);
}

