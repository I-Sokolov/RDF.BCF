#pragma once

#include "bcfTypes.h"
#include "BCFObject.h"

struct XMLPoint : public BCFObject
{
public:
    XMLPoint(BCFObject& owner);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

    bool IsSet() { return !m_X.empty() && !m_Y.empty() && !m_Z.empty(); }

    bool GetPoint(BCFPoint& bcfpt);
    bool SetPoint(const BCFPoint* bcfpt);
    void SetPoint(double x, double y, double z);

    virtual Topic* Topic_() override { return m_owner.Topic_(); }
    virtual Comment* Comment_() override { return m_owner.Comment_(); }

private:
    BCFObject&  m_owner;
    std::string m_XYZ[3];

    std::string& m_X;
    std::string& m_Y;
    std::string& m_Z;
};

