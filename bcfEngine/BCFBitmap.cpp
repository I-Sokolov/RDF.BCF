
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
    REQUIRED_PROP(Format);
    REQUIRED_PROP(Reference);
    REQUIRED(Location, m_Location.IsSet());
    REQUIRED(Normal, m_Normal.IsSet());
    REQUIRED(Up, m_Up.IsSet());
    REQUIRED_PROP(Height);

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

/// <summary>
/// 
/// </summary>
BCFBitmapFormat BCFBitmap::GetFormat()
{
    std::string format(m_Format);
    std::transform(format.begin(), format.end(), format.begin(), [](unsigned char c) { return std::tolower(c); });

    if (m_Format == "png") {
        return BCFBitmapPNG;
    }
    else {
        return BCFBitmapJPG;
    }
}

/// <summary>
/// 
/// </summary>
bool BCFBitmap::SetFormat(BCFBitmapFormat val)
{
    switch (val) {
        case BCFBitmapPNG:
            m_Format.assign("png");
            break;
        default:
            m_Format.assign("jpg");
            break;
    }
    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFBitmap::SetReference(const char* val)
{
    UNNULL;
    VALIDATE(Reference, FilePath);

    m_Reference.assign(val);

    return true;
}
