#pragma once

#include "BCFObject.h"
#include "ListOf.h"

struct BCFColoring : public BCFObject
{
public:
    BCFColoring(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    const char* GetColor() { return m_Color.c_str(); }
    bool SetColor(const char* val) { UNNULL; VALIDATE(Color, Color); m_Color.assign(val); return true; }

    BCFComponent* ComponentAdd(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL);
    BCFComponent* ComponentIterate(BCFComponent* prev);

private:
    bool IsColorValid(const char* str, const char* propName);

private:
    BCFViewPoint&              m_viewPoint;

    std::string                m_Color;
    ListOfBCFComponents        m_Components;
};

