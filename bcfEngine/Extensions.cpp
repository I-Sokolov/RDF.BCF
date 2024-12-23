#include "pch.h"
#include "Extensions.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
#define FILE_NAME "extensions.xml"


/// <summary>
/// 
/// </summary>
#define GET_ATTR(name)                      \
    for (auto attr : elem.attributes()) {   \
    if (attr) {                             \
        auto attrName = attr->getName();    \
        if (attrName == #name) {            \
            m_##name = attr->getValue();    \
        }                                   

#define NEXT_ATTR(name)                     \
            else if (attrName == #name) {   \
            m_##name = attr->getValue();    \
        }

#define END_ATTR        \
            else { ctx.LogMsg(MsgLevel::Warning, "Unknown attribute '%s'", attrName.c_str()); } } }


/// <summary>
/// 
/// </summary>
#define GET_CHILD(name)                     \
    for (auto child : elem.children()) {    \
        if (child) {                        \
            auto&  tag= child->getName();   \
            if (tag == #name) {             \
                Read_##name(*child);        \
            }

#define NEXT_CHILD(name)                    \
            else if (tag == #name) {        \
                Read_##name(*child);        \
            }

#define GET_CHILD_MEMBER(name)              \
    for (auto child : elem.children()) {    \
        if (child) {                        \
            auto&  tag= child->getName();   \
            if (tag == #name) {             \
                m_##name.Read(*child, ctx); \
            }

#define NEXT_CHILD_MEMBER(name)             \
            else if (tag == #name) {        \
                m_##name.Read(*child, ctx); \
            }

#define END_CHILDREN \
            else { m_log.add(Log::Level::warning, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str()); } } }


/// <summary>
/// 
/// </summary>
Extensions::Extensions(Log& log)
    :m_log(log)
{
    m_elements.resize(7);
}


/// <summary>
/// 
/// </summary>
bool Extensions::Read(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path (bcfFolder);
    FileSystem::AddPath(path, FILE_NAME);

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Read file error", "Failed to read %s file : % s", FILE_NAME, ex.what());
    }


    return ok;
}

/// <summary>
/// 
/// </summary>
bool Extensions::Write(const std::string& bcfFolder)
{
    //assert(!"todo");
    return false;
}


/// <summary>
/// 
/// </summary>
bool Extensions::AddElement(BCFEnumeration enumeration, const char* element)
{
    NULL_CHECK(element);

    auto list = GetList(enumeration);
    if (!list) {
        return false;
    }

    list->insert(element);
    return true;
}


/// <summary>
/// 
/// </summary>
const char* Extensions::GetElement(BCFEnumeration enumeration, unsigned short index)
{
    auto list = GetList(enumeration);
    if (list) {
        for (auto& elem : *list) {
            if (index == 0) {
                return elem.c_str();
            }
            index--;
        }
    }
    return NULL;
}


/// <summary>
/// 
/// </summary>
bool Extensions::RemoveElement(BCFEnumeration enumeration, const char* element)
{
    NULL_CHECK(element);

    auto list = GetList(enumeration);
    if (!list) {
        return false;
    }

    list->erase(element);
    return true;
}


/// <summary>
/// 
/// </summary>
StringSet* Extensions::GetList(BCFEnumeration enumeration)
{
    size_t ind = enumeration - 1;
    
    if (ind < m_elements.size()) {
        return &m_elements[ind];
    }
    else {
        m_log.add(Log::Level::error, "Index is out of range", "Index %d is out of extensions types range [0..%d]", (int)ind, (int)m_elements.size());
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadRoot(_xml::_element& elem)
{
    GET_CHILD(TopicTypes)
    NEXT_CHILD(TopicStatuses)
    NEXT_CHILD(Priorities)
    NEXT_CHILD(TopicLabels)
    NEXT_CHILD(Users)
    NEXT_CHILD(SnippetTypes)
    NEXT_CHILD(Stages)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadEnumeration(_xml::_element& elem, BCFEnumeration enumeration)
{
    auto list = GetList(enumeration);
    if (!list) {
        assert(false);
        throw std::exception(FILE_NAME " parsing error");
    }

    for (auto child : elem.children()) {
        if (child) {
            auto& content = child->getContent();
            list->insert(content);
        }
    }
}
