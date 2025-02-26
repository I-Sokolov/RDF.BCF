#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "ListOf.h"

struct Coloring : public BCFObject, public BCFColoring
{
public:
    Coloring(ViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    //BCFColoring
    virtual const char* GetColor() override { return m_Color.c_str(); }
    virtual bool SetColor(const char* val) override { VALIDATE(Color, Color); return SetPropertyString(val, m_Color); }
    
    virtual BCFComponent* AddComponent(const char* ifcGuid = NULL, const char* authoringToolId = NULL, const char* originatingSystem = NULL) override;
    virtual BCFComponent* GetComponent(uint16_t ind) override;

    virtual BCFViewPoint& GetViewPoint() override;
    virtual bool Remove() override { return RemoveImpl(); }

    virtual Topic* Topic_() override;
    virtual Comment* Comment_() override { return NULL; }

private:
    bool IsColorValid(const char* str, const char* propName);

private:
    ViewPoint&              m_viewPoint;

    std::string             m_Color;
    ListOfComponents        m_Components;
};

