#pragma once

#include "BCFObject.h"
#include "ListOf.h"

struct BCFColor : public BCFObject
{
public:
    BCFColor(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element & elem, const std::string& folder) { assert(!"TODO"); }
    void Write(_xml_writer& writer, const std::string& folder, const char* tag) { assert(!"TODO"); }

public:
    BCFComponent* ComponentAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL);
    BCFComponent* ComponentIterate(BCFComponent* prev);
    bool ComponentRemove(BCFComponent* component);

private:
    BCFViewPoint&              m_viewPoint;

    std::string                m_Color;
    ListOfBCFComponents        m_Components;
};

