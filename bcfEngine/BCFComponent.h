#pragma once

#include "BCFObject.h"
struct BCFViewPoint;
struct BCFColoring;

struct BCFComponent : public BCFObject
{
public:
    BCFComponent(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);
    BCFComponent(BCFColoring& coloring, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    const char* GetIfcGuid() {return m_IfcGuid.c_str();}
    const char* GetOriginatingSystem() {return m_OriginatingSystem.c_str();}
    const char* GetAuthoringToolId() { return m_AuthoringToolId.c_str(); }

    bool SetIfcGuid(const char* val) { UNNULL; VALIDATE(IfcGuid, IfcGuid); m_IfcGuid.assign(val); return true; }
    bool SetOriginatingSystem(const char* val) { UNNULL; m_OriginatingSystem.assign(val); return true; }
    bool SetAuthoringToolId(const char* val) { UNNULL; m_AuthoringToolId.assign(val); return true; }

private:
    BCFViewPoint* m_pViewPoint;

    std::string m_IfcGuid;
    std::string m_OriginatingSystem;
    std::string m_AuthoringToolId;
    
    //v2.0
    std::string m_Selected;
    std::string m_Visible;
    std::string m_Color;
};

