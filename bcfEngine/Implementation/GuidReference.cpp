#include "pch.h"
#include "GuidReference.h"
#include "Topic.h"
#include "Project.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
GuidReference::GuidReference(Topic& topic, ListOfBCFObjects* parentList) 
    : BCFObject(topic.GetProject(), parentList)
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

/// <summary>
/// 
/// </summary>
bool GuidReference::Validate(bool)
{
    return true;
}

/// <summary>
/// 
/// </summary>
void GuidReference::Write(_xml_writer& writer, const std::string&, const char* tag)
{
    if (!m_Guid.empty()) {
        XMLFile::Attributes attr;
        ATTR_ADD(Guid);

        writer.writeTag(tag, attr, "");
    }
}
