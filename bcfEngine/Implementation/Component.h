#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"

struct ViewPoint;
struct Coloring;
struct Topic;

struct Component : public BCFObject, public BCFComponent
{
public:
    Component(ViewPoint& viewPoint, ListOfBCFObjects* parentList);
    Component(Coloring& coloring, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    //BCFComponent
    virtual const char* GetIfcGuid() override {return m_IfcGuid.c_str();}
    virtual const char* GetOriginatingSystem() override {return m_OriginatingSystem.c_str();}
    virtual const char* GetAuthoringToolId() override { return m_AuthoringToolId.c_str(); }

    virtual bool SetIfcGuid(const char* val) override { VALIDATE(IfcGuid, IfcGuid); return SetPropertyString (val, m_IfcGuid); }
    virtual bool SetOriginatingSystem(const char* val) override { return SetPropertyString(val, m_OriginatingSystem); }
    virtual bool SetAuthoringToolId(const char* val) override { return SetPropertyString(val, m_AuthoringToolId);}

    virtual BCFViewPoint& GetViewPoint() override;
    virtual BCFColoring* GetColoring() override;
    virtual bool Remove() override { return RemoveImpl(); }

    virtual Topic* Topic_() override;
    virtual Comment* Comment_() override { return NULL; }

private:
    bool ValidateIfcGuid();

private:
    ViewPoint*  m_pViewPoint;
    Coloring*   m_pColoring;

    std::string m_IfcGuid;
    std::string m_OriginatingSystem;
    std::string m_AuthoringToolId;
    
    //v2.0
    std::string m_Selected;
    std::string m_Visible;
    std::string m_Color;
};

