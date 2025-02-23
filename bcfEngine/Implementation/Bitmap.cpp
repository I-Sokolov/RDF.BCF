
#include "pch.h"
#include "Bitmap.h"
#include "Project.h"
#include "XMLFile.h"
#include "ViewPoint.h"

/// <summary>
/// 
/// </summary>
Bitmap::Bitmap(ViewPoint& viewPoint, ListOfBCFObjects* parentList) 
    : BCFObject(viewPoint.Project_(), parentList)
    , m_viewPoint(viewPoint)
    , m_Location(viewPoint.Project_())
    , m_Normal(viewPoint.Project_())
    , m_Up(viewPoint.Project_())
{
}

/// <summary>
/// 
/// </summary>
void Bitmap::Read(_xml::_element& elem, const std::string& folder)
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
bool Bitmap::Validate(bool fix)
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
void Bitmap::Write(_xml_writer& writer, const std::string& folder, const char* tag)
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
BCFBitmapFormat Bitmap::GetFormat()
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
bool Bitmap::SetFormat(BCFBitmapFormat val)
{
    switch (val) {
        case BCFBitmapPNG:
            return SetPropertyString("png", m_Format);
            break;
        default:
            return SetPropertyString("jpg", m_Format);
            break;
    }
    Log_().add(Log::Level::error, "Invalid value", "Invalids BCF bitmap format value");
    return false;
}

/// <summary>
/// 
/// </summary>
bool Bitmap::SetReference(const char* val)
{
    VALIDATE(Reference, FilePath);

    return SetPropertyString(val, m_Reference);
}

/// <summary>
/// 
/// </summary>
void Bitmap::AfterRead(const std::string&)
{
    if (Project_().GetVersion() < BCFVer_3_0) {
        std::transform(m_Format.begin(), m_Format.end(), m_Format.begin(), [](unsigned char c) { return std::tolower(c); });
    }
}

/// <summary>
/// 
/// </summary>
BCFViewPoint& Bitmap::GetViewPoint()
{
    return m_viewPoint; 
}
