#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"

struct ViewPoint;
struct Coloring;

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

    virtual bool SetIfcGuid(const char* val) override { UNNULL; VALIDATE(IfcGuid, IfcGuid); m_IfcGuid.assign(val); return true; }
    virtual bool SetOriginatingSystem(const char* val) override { UNNULL; m_OriginatingSystem.assign(val); return true; }
    virtual bool SetAuthoringToolId(const char* val) override { UNNULL; m_AuthoringToolId.assign(val); return true; }

    virtual bool Remove() override { return RemoveImpl(); }

private:
    ViewPoint*  m_pViewPoint;

    std::string m_IfcGuid;
    std::string m_OriginatingSystem;
    std::string m_AuthoringToolId;
    
    //v2.0
    std::string m_Selected;
    std::string m_Visible;
    std::string m_Color;
};

