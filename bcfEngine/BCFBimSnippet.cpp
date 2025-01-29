#include "pch.h"
#include "BCFBimSnippet.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
BCFBimSnippet::BCFBimSnippet(BCFTopic& topic, ListOfBCFObjects* parentList)
    : BCFObject(topic.Project(), parentList)
    , m_IsExternal("false")
{
}


/// <summary>
/// 
/// </summary>
void BCFBimSnippet::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(SnippetType)
        ATTR_GET(IsExternal)

        ATTR_GET_STR(isExternal, m_IsExternal) //v2.1
    
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Reference)
        CHILD_GET_CONTENT(ReferenceSchema)
    CHILDREN_END

     //
     if (!GetIsExternal()) {
         m_Reference = AbsolutePath(m_Reference, folder);
     }
}

/// <summary>
/// 
/// </summary>
void BCFBimSnippet::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
    //
    REQUIRED_PROP(SnippetType);
    REQUIRED_PROP(Reference);
    REQUIRED_PROP(ReferenceSchema);

    //
    if (!GetIsExternal()) {
        m_Reference = CopyToRelative(m_Reference, folder, NULL);
    }

    XMLFile::Attributes attr;
    ATTR_ADD(SnippetType);
    ATTR_ADD(IsExternal);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_CONTENT(Reference);
    WRITE_CONTENT(ReferenceSchema);

    //
    if (!GetIsExternal()) {
        m_Reference = AbsolutePath(m_Reference, folder);
    }
}

/// <summary>
/// 
/// </summary>
bool BCFBimSnippet::SetSnippetType(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFSnippetTypes, val)) {
        m_SnippetType.assign(val);
        return true;
    }

    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFBimSnippet::SetReference(const char* val)
{
    UNNULL;
    VALIDATE(Reference, FilePath);

    m_Reference.assign(val);

    return true;
}

/// <summary>
/// 
/// </summary>
bool BCFBimSnippet::SetReferenceSchema(const char* val) 
{
    UNNULL;
    VALIDATE(ReferenceSchema, FilePath);

    m_ReferenceSchema.assign(val);

    return true;
}
