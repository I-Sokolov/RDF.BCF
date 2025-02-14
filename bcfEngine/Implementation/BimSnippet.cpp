#include "pch.h"
#include "BimSnippet.h"
#include "Project.h"
#include "Topic.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
BimSnippet::BimSnippet(Topic& topic, ListOfBCFObjects* parentList)
    : BCFObject(topic.Project_(), parentList)
    , m_topic(topic)
    , m_IsExternal("false")
{
}


/// <summary>
/// 
/// </summary>
void BimSnippet::Read(_xml::_element& elem, const std::string& folder)
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
bool BimSnippet::Validate(bool fix)
{
    if (fix) {
        if (m_SnippetType.empty()) {
            auto NOT_SET = "Not set";
            Project_().GetExtensions().AddElement(BCFSnippetTypes, NOT_SET);
            m_SnippetType.assign(NOT_SET);
        }
    }

    //
    bool valid = true;
    REQUIRED_PROP(SnippetType);
    REQUIRED_PROP(Reference);
    REQUIRED_PROP(ReferenceSchema);

    if (!valid && fix) {
        Remove();
        return true;
    }

    return valid;
}

/// <summary>
/// 
/// </summary>
void BimSnippet::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
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
bool BimSnippet::SetSnippetType(const char* val)
{
    UNNULL;

    if (Project_().GetExtensions_().CheckElement(BCFSnippetTypes, val)) {
        m_SnippetType.assign(val);
        return true;
    }

    return false;
}

/// <summary>
/// 
/// </summary>
bool BimSnippet::SetReference(const char* val)
{
    UNNULL;
    VALIDATE(Reference, FilePath);

    m_Reference.assign(val);

    return true;
}

/// <summary>
/// 
/// </summary>
bool BimSnippet::SetReferenceSchema(const char* val) 
{
    UNNULL;
    VALIDATE(ReferenceSchema, FilePath);

    m_ReferenceSchema.assign(val);

    return true;
}

/// <summary>
/// 
/// </summary>
BCFTopic& BimSnippet::GetTopic()
{ 
    return m_topic; 
}
