#include "pch.h"
#include "bcfTypes.h"
#include "Extensions.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
Extensions::Extensions(BCFProject& project)
    :XMLFile(project, NULL)
{
    m_elements.resize(7);
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
const char* Extensions::GetElement(BCFEnumeration enumeration, BCFIndex index)
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
        m_project.log().add(Log::Level::error, "Extension schema", "Index %d is out of extensions types range [0..%d]", (int)ind, (int)m_elements.size());
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(TopicTypes)
        CHILD_READ(TopicStatuses)
        CHILD_READ(Priorities)
        CHILD_READ(TopicLabels)
        CHILD_READ(Users)
        CHILD_READ(SnippetTypes)
        CHILD_READ(Stages)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Extensions::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    Attributes attr;

        WRITE_ELEM(TopicTypes);
        WRITE_ELEM(TopicStatuses);
        WRITE_ELEM(Priorities);
        WRITE_ELEM(TopicLabels);
        WRITE_ELEM(Users);
        WRITE_ELEM(SnippetTypes);
        WRITE_ELEM(Stages);
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadEnumeration(_xml::_element& elem, const std::string& /*folder*/, BCFEnumeration enumeration)
{
    auto list = GetList(enumeration);
    if (!list) {
        assert(false);
        throw std::exception("Invalid BCF enumeration in " __FUNCTION__);
    }

    for (auto child : elem.children()) {
        if (child) {
            auto& content = child->getContent();
            list->insert(content);
        }
    }
}

/// <summary>
/// 
/// </summary>
void Extensions::WriteEnumeration(_xml_writer& writer, const std::string& tag, BCFEnumeration enumeration)
{
    auto list = GetList(enumeration);
    if (!list) {
        assert(false);
        throw std::exception("Invalid BCF enumeration in " __FUNCTION__);
    }

    for (auto& elem : *list) {
        if (!elem.empty()) {
            writer.writeTag(tag, elem);
        }
    }

}

/// <summary>
/// 
/// </summary>
bool Extensions::CheckElement(BCFEnumeration enumeration, const char* element)
{
    auto list = GetList(enumeration);
    if (!list) {
        return false;
    }

    if (list->find(element) != list->end()) {
        return true;
    }

    if (Project().GetAutoExtentSchema()) {
        list->insert(element);
        return true;
    }

    m_project.log().add(Log::Level::error, "Extension schema", "%s is not in enumeration", element);
    return false;
}
