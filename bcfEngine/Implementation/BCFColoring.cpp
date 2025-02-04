#pragma once

#include "pch.h"
#include "BCFColoring.h"
#include "BCFViewPoint.h"
#include "BCFComponent.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFColoring::BCFColoring(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project(), parentList) 
    , m_viewPoint(viewPoint)
    , m_Components(viewPoint.Project())
{ 
}


/// <summary>
/// 
/// </summary>
void BCFColoring::Read(_xml::_element& elem, const std::string& folder) 
{ 
    ATTRS_START
        ATTR_GET(Color)
    ATTRS_END(UnknownNames::NotAllowed)
    
    CHILDREN_START
        CHILD_GET_LIST(Components, Component)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool BCFColoring::Validate(bool fix)
{
    if (fix) {
        if (m_Color.empty()) {
            m_Color.assign("FF0000");
        }
    }

    bool valid = true;
    
    REQUIRED_PROP(Color);
    REQUIRED(Components, !m_Components.Items().empty());

    valid = m_Components.Validate(fix) && valid;

    if (!valid && fix) {
        Remove();
        return true;
    }

    return valid;
}


/// <summary>
/// 
/// </summary>
void BCFColoring::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{ 
    XMLFile::Attributes attr;
    ATTR_ADD(Color);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_LIST_EX(Components, Component);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFColoring::ComponentAdd(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Components.Add(m_viewPoint, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFColoring::ComponentIterate(BCFComponent* prev)
{
    return m_Components.GetNext(prev);
}


/// <summary>
/// 
/// </summary>
bool BCFColoring::IsColorValid(const char* str, const char* propName)
{
    auto len = strlen(str);

    if (len == 0 || len == 6 || len == 8) {
    
        bool hex = true;
        for (auto p = str; *p && hex; p++) {
            hex = isxdigit(*p);
        }

        if (hex) {
            return true;
        }
    }

    log().add(Log::Level::error, "Invalid value", "Color value must be 6 or 8 hex digits, value '%s' is incorrect for %s", str, propName);
    return false;
}
