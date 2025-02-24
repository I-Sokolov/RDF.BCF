#pragma once

#include "pch.h"
#include "Coloring.h"
#include "ViewPoint.h"
#include "Component.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
Coloring::Coloring(ViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project_(), parentList) 
    , m_viewPoint(viewPoint)
    , m_Components(viewPoint)
{ 
}

/// <summary>
/// 
/// </summary>
Topic* Coloring::Topic_()
{
    return m_viewPoint.Topic_();
}


/// <summary>
/// 
/// </summary>
void Coloring::Read(_xml::_element& elem, const std::string& folder) 
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
bool Coloring::Validate(bool fix)
{
    if (fix) {
        if (m_Color.empty()) {
            m_Color.assign("FF0000");
        }
    }

    bool valid = true;

    valid = m_Components.Validate(fix) && valid;

    REQUIRED_PROP(Color);
    REQUIRED(Components, !m_Components.Items().empty());

    if (!valid && fix) {
        Remove();
        return true;
    }

    return valid;
}


/// <summary>
/// 
/// </summary>
void Coloring::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{ 
    XMLFile::Attributes attr;
    ATTR_ADD(Color);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_LIST_EX(Components, Component);
}

/// <summary>
/// 
/// </summary>
BCFComponent* Coloring::AddComponent(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Components.Add(m_viewPoint, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* Coloring::GetComponent(uint16_t ind)
{
    return m_Components.GetAt(ind);
}


/// <summary>
/// 
/// </summary>
bool Coloring::IsColorValid(const char* str, const char* propName)
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

    Log_().add(Log::Level::error, "Invalid value", "Color value must be 6 or 8 hex digits, value '%s' is incorrect for %s", str, propName);
    return false;
}

/// <summary>
/// 
/// </summary>
BCFViewPoint& Coloring::GetViewPoint()
{ 
    return m_viewPoint; 
}
