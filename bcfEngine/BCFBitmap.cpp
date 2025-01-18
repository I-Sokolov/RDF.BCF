
#include "pch.h"
#include "BCFBitmap.h"
#include "BCFProject.h"
#include "XMLFile.h"
#include "BCFViewPoint.h"

/// <summary>
/// 
/// </summary>
BCFBitmap::BCFBitmap(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList) 
    : BCFObject(viewPoint.Project(), parentList) 
    , m_Location(viewPoint.Project())
    , m_Normal(viewPoint.Project())
    , m_Up(viewPoint.Project())
{
}

/// <summary>
/// 
/// </summary>
void BCFBitmap::Read(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_CONTENT(Format)
        CHILD_GET_CONTENT(Reference)
        CHILD_READ_MEMBER(Location)
        CHILD_READ_MEMBER(Normal)
        CHILD_READ_MEMBER(Up)
        CHILD_GET_CONTENT(Height)
    CHILDREN_END

    m_Reference = AbsolutePath(m_Reference, folder);
}

/// <summary>
/// 
/// </summary>
void BCFBitmap::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
    XMLFile::Attributes attr;
    XMLFile::ElemTag _(writer, tag, attr);

    m_Reference = CopyToRelative(m_Reference, folder, NULL);

    WRITE_CONTENT(Format);
    WRITE_CONTENT(Reference);
    WRITE_MEMBER(Location);
    WRITE_MEMBER(Normal);
    WRITE_MEMBER(Up);
    WRITE_CONTENT(Height);

    m_Reference = AbsolutePath(m_Reference, folder);
}

