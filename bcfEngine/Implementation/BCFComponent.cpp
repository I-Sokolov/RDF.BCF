#include "pch.h"
#include "BCFComponent.h"
#include "BCFViewPoint.h"
#include "BCFColoring.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFComponent::BCFComponent(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project(), parentList)
    , m_pViewPoint(&viewPoint)
    , m_Visible("true")
{
}

/// <summary>
/// 
/// </summary>
BCFComponent::BCFComponent(BCFColoring& coloring, ListOfBCFObjects* parentList)
    : BCFObject(coloring.Project(), parentList)
    , m_pViewPoint(NULL)
    , m_Visible("true")
{
}

/// <summary>
/// 
/// </summary>
void BCFComponent::Read(_xml::_element& elem, const std::string&)
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
void BCFComponent::AfterRead(const std::string&)
{
    if (Project().GetVersion() < BCFVer_2_1) {
        assert(m_pViewPoint);

        if (!StrToBool(m_Visible) && m_pViewPoint) {
            m_pViewPoint->ExceptionsAdd(m_IfcGuid.c_str(), m_AuthoringToolId.c_str(), m_OriginatingSystem.c_str());
        }

        if (!m_Color.empty() && m_pViewPoint) {
            BCFColoring* coloring = NULL;
            
            while ((coloring = m_pViewPoint->ColoringIterate(coloring)) != NULL) {
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
bool BCFComponent::Validate(bool fix)
{
    bool valid = true;

    if (m_AuthoringToolId.empty()) {
        REQUIRED_PROP(IfcGuid);
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
void BCFComponent::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{ 
    XMLFile::Attributes attr;
    ATTR_ADD(IfcGuid);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_CONTENT(OriginatingSystem);
    WRITE_CONTENT(AuthoringToolId);
}

