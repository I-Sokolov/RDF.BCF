#include "pch.h"
#include "XMLPoint.h"
#include "XMLFile.h"
#include "BCFProject.h"

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

