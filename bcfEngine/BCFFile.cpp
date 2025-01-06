#include "pch.h"
#include "BCFFile.h"
#include "XMLFile.h"
#include "BCFProject.h"
#include "BCFTopic.h"

/// <summary>
/// 
/// </summary>
BCFFile::BCFFile(BCFTopic& topic) 
    : BCFObject(topic.Project())
    , m_topic(topic) 
{
}


/// <summary>
/// 
/// </summary>
void BCFFile::Read(_xml::_element& elem, const std::string&)
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
