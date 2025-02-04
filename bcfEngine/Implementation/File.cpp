#include "pch.h"
#include "File.h"
#include "XMLFile.h"
#include "Project.h"
#include "Topic.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
File::File(Topic& topic, ListOfBCFObjects* parentList)
    : BCFObject(topic.GetProject(), parentList)
    , m_topic(topic)
    , m_IsExternal("true")
{
}

/// <summary>
/// 
/// </summary>
void File::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(IsExternal)
        ATTR_GET(IfcProject)
        ATTR_GET(IfcSpatialStructureElement)

        ATTR_GET_STR(isExternal, m_IsExternal) //v2.1
    
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Reference)
    CHILDREN_END

    //
    if (!GetIsExternal()) {
        m_Reference = AbsolutePath(m_Reference, folder);
    }
}

/// <summary>
/// 
/// </summary>
bool File::Validate(bool)
{
    return true;
}

/// <summary>
/// 
/// </summary>
void File::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    if (!GetIsExternal()) {
        m_Reference = CopyToRelative(m_Reference, folder, "..");
    }

    XMLFile::Attributes attr;
    ATTR_ADD(IsExternal);
    ATTR_ADD(IfcProject);
    ATTR_ADD(IfcSpatialStructureElement);

    WRITE_ELEM(File);

    if (!GetIsExternal()) {
        m_Reference = AbsolutePath(m_Reference, folder);
    }
}

/// <summary>
/// 
/// </summary>
void File::Write_File(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Filename);
    WRITE_CONTENT(Date);
    WRITE_CONTENT(Reference);
}

/// <summary>
/// 
/// </summary>
bool File::SetReference(const char* val)
{ 
    UNNULL;
    VALIDATE(Reference, FilePath);

    m_Reference.assign(val);
    if (!m_Reference.empty()) {
        UpdateFileInfo();
    }
    return true; 
}

/// <summary>
/// 
/// </summary>
void File::UpdateFileInfo()
{
    if (!m_Reference.empty()) {

        m_Filename = FileSystem::GetFileName(m_Reference.c_str(), GetLog());

        if (FileSystem::Exists(m_Reference.c_str())) {
            auto tm = FileSystem::GetFileModificationTime(m_Reference.c_str(), GetLog());
            if (tm) {
                m_Date = TimeToStr(tm);
            }
        }
    }
}
