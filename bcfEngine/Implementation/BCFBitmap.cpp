
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

        CHILD_GET_CONTENT_STR(Bitmap, m_Format) //v2.1

    CHILDREN_END

    m_Reference = AbsolutePath(m_Reference, folder);
}

/// <summary>
/// 
/// </summary>
bool BCFBitmap::Validate(bool fix)
{
    //
    bool valid = true;
    REQUIRED_PROP(Format);
    REQUIRED_PROP(Reference);
    REQUIRED(Location, m_Location.IsSet());
    REQUIRED(Normal, m_Normal.IsSet());
    REQUIRED(Up, m_Up.IsSet());
    REQUIRED_PROP(Height);

    if (fix && !valid) {
        Remove();
        return true;
    }
    return valid;
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

/// <summary>
/// 
/// </summary>
BCFBitmapFormat BCFBitmap::GetFormat()
{
    if (m_Format == "png") {
        return BCFBitmapPNG;
    }
    else if (m_Format == "jpg"){
        return BCFBitmapJPG;
    }
    else {
        assert(false);
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

/// <summary>
/// 
/// </summary>
void BCFBitmap::AfterRead(const std::string&)
{
    if (Project().GetVersion() < BCFVer_3_0) {
        std::transform(m_Format.begin(), m_Format.end(), m_Format.begin(), [](unsigned char c) { return std::tolower(c); });
    }
}
