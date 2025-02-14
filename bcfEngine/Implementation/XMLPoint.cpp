#include "pch.h"
#include "XMLPoint.h"
#include "XMLFile.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
XMLPoint::XMLPoint(Project& project) 
    : BCFObject(project, NULL)
    , m_X(XYZ[0]), m_Y(XYZ[1]), m_Z(XYZ[2]) {}

/// <summary>
/// 
/// </summary>
void XMLPoint::Read(_xml::_element& elem, const std::string&)
{
    CHILDREN_START
        CHILD_GET_CONTENT(X)
        CHILD_GET_CONTENT(Y)
        CHILD_GET_CONTENT(Z)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void XMLPoint::Write(_xml_writer& writer, const std::string& folder, const char* tag) 
{
    writer.writeStartTag(tag);
    writer.indent()++;

    WRITE_CONTENT(X);
    WRITE_CONTENT(Y);
    WRITE_CONTENT(Z);

    writer.indent()--;
    writer.writeEndTag(tag);
}

/// <summary>
/// 
/// </summary>
bool XMLPoint::GetPoint(BCFPoint& bcfpt)
{
    for (int i = 0; i < 3; i++) {
        bcfpt.xyz[i] = atof(XYZ[i].c_str());
    }

    return true;
}

/// <summary>
/// 
/// </summary>
bool XMLPoint::SetPoint(const BCFPoint* bcfpt)
{
    if (bcfpt) {
        for (int i = 0; i < 3; i++) {
            RealToStr(bcfpt->xyz[i], XYZ[i]);
        }
        return true;
    }
    else {
        m_project.Log_().add(Log::Level::error, "NULL argument");
        return false;
    }
}

/// <summary>
/// 
/// </summary>
void XMLPoint::SetPoint(double x, double y, double z)
{
    RealToStr(x, m_X);
    RealToStr(y, m_Y);
    RealToStr(z, m_Z);
}
