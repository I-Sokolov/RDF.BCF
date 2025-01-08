#include "pch.h"
#include "BCFFile.h"
#include "XMLFile.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFFile::BCFFile(BCFTopic& topic) 
    : BCFObject(topic.Project())
    , m_topic(topic)
    , m_IsExternal("true")
{
}

/// <summary>
/// 
/// </summary>
void BCFFile::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(IsExternal)
        ATTR_GET(IfcProject)
        ATTR_GET(IfcSpatialStructureElement)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Reference)
    CHILDREN_END

    if (!m_Reference.empty() && !GetIsExternal()) {
        std::string filePath(folder);
        FileSystem::AddPath(filePath, m_Reference.c_str());
        std::swap(m_Reference, filePath);
    }
}

/// <summary>
/// 
/// </summary>
void BCFFile::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    if (!m_Reference.empty() && !GetIsExternal()) {
        assert(!"copy file");
    }

    XMLFile::Attributes attr;
    ATTR_ADD(IsExternal);
    ATTR_ADD(IfcProject);
    ATTR_ADD(IfcSpatialStructureElement);

    WRITE_ELEM(File);
}

/// <summary>
/// 
/// </summary>
void BCFFile::Write_File(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Filename);
    WRITE_CONTENT(Date);
    WRITE_CONTENT(Reference);
}

/// <summary>
/// 
/// </summary>
bool BCFFile::Remove(void)
{
    return m_topic.FileRemove(this);
}