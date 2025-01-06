#include "pch.h"
#include "GuidReference.h"
#include "BCFTopic.h"
#include "BCFProject.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
GuidReference::GuidReference(BCFTopic& topic) 
    : BCFObject(topic.Project())
{
}


/// <summary>
/// 
/// </summary>
void GuidReference::Read(_xml::_element& elem, const std::string& /*folder*/)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)
}
