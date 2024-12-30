#include "pch.h"
#include "BIMFile.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFProject& BIMFile::Project()
{
    return m_topic.Project();
}

/// <summary>
/// 
/// </summary>
void BIMFile::Read(_xml::_element& elem, const std::string&)
{
    ATTRS_START
        ATTR_GET(IsExternal)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Reference)
    CHILDREN_END
}
