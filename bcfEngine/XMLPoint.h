#pragma once

#include "bcfTypes.h"
#include "BCFObject.h"

struct XMLPoint : public BCFObject
{
public:
    XMLPoint(BCFProject& project);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

    BCFProject& Project() { return m_project; }

    bool IsSet() { return !m_X.empty() && !m_Y.empty() && !m_Z.empty(); }

    bool GetPoint(BCFPoint& bcfpt);
    bool SetPoint(const BCFPoint* bcfpt);
    void SetPoint(double x, double y, double z);

private:
    std::string XYZ[3];

    std::string& m_X;
    std::string& m_Y;
    std::string& m_Z;
};

