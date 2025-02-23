#include "pch.h"
#include "bcfTypes.h"
#include "Extensions.h"
#include "Project.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
Extensions::Extensions(Project& project)
    : XMLFile(project, NULL)
    , m_currentList (NULL)
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
    MARK_DIRTY;

    return true;
}


/// <summary>
/// 
/// </summary>
const char* Extensions::GetElement(BCFEnumeration enumeration, uint16_t ind)
{
    auto list = GetList(enumeration);
    if (list) {
        for (auto& elem : *list) {
            if (ind == 0) {
                return elem.c_str();
            }
            ind--;
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
    MARK_DIRTY;

    return true;
}


/// <summary>
/// 
/// </summary>
StringSet* Extensions::GetList(BCFEnumeration enumeration)
{
    size_t ind = enumeration - 1;
    
    if (ind>=0 && ind < m_elements.size()) {
        return &m_elements[ind];
    }
    else {
        m_project.Log_().add(Log::Level::error, "Extension schema", "Index %d is out of extensions types range [0..%d]", (int)ind, (int)m_elements.size());
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

    if (Project_().GetAutoExtentSchema()) {
        list->insert(element);
        return true;
    }

    m_project.Log_().add(Log::Level::error, "Extension schema", "%s is not in enumeration", element);
    return false;
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadExtensionSchema(_xml::_element& extensionSchemaElem, const std::string& folder)
{
    auto& fileName = extensionSchemaElem.getContent();

    if (!fileName.empty()) {
        std::string path(folder);
        FileSystem::AddPath(path, fileName.c_str());

        if (FileSystem::Exists(path.c_str())) {
            try {
                _xml::_document doc(nullptr);
                doc.load(path.c_str());

                if (auto pelem = doc.getRoot()) {
                    auto& elem = *pelem;
                    CHILDREN_START
                        CHILD_READ_FUNC(redefine, ReadExtensionSchema_redefine)
                        CHILDREN_END
                }
            }
            catch (std::exception& ex) {
                m_project.Log_().add(Log::Level::error, "Read file error", "Failed to read %s file. %s", path.c_str(), ex.what());
                throw;
            }
        }
        else {
            m_project.Log_().add(Log::Level::warning, "File not exists", "File is referenced but not exists: %s", path.c_str());
        }
    }
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadExtensionSchema_redefine(_xml::_element& elem, const std::string& folder) //v2.1
{
    CHILDREN_START
        CHILD_READ_FUNC(simpleType, ReadExtensionSchema_simpleType)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
BCFEnumeration Extensions::FromName(const std::string& enumName)
{
    if (enumName == "Priority") return BCFPriorities;
    if (enumName == "SnippetType") return BCFSnippetTypes;
    if (enumName == "Stage") return BCFStages;
    if (enumName == "TopicLabel") return BCFTopicLabels;
    if (enumName == "TopicStatus") return BCFTopicStatuses;
    if (enumName == "TopicType") return BCFTopicTypes;
    if (enumName == "UserIdType") return BCFUsers;
    return BCFUnknown;
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadExtensionSchema_simpleType(_xml::_element& elem, const std::string& folder) //v2.1
{
    std::string enumName;

    ATTRS_START
        ATTR_GET_STR(name, enumName)
    ATTRS_END(UnknownNames::NotAllowed);

    auto enumeration = FromName(enumName);

    m_currentList = GetList(enumeration);

    CHILDREN_START
        CHILD_READ_FUNC(restriction, ReadExtensionSchema_restriction)
    CHILDREN_END

    m_currentList = NULL;
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadExtensionSchema_restriction(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ_FUNC(enumeration, ReadExtensionSchema_enumeration)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Extensions::ReadExtensionSchema_enumeration(_xml::_element& elem, const std::string& folder)
{
    std::string value;

    ATTRS_START
        ATTR_GET_STR(value, value)
    ATTRS_END(UnknownNames::NotAllowed);

    if (!m_currentList) {
        assert(false);
        throw std::exception("Invalid BCF enumeration in " __FUNCTION__);
    }

    m_currentList->insert(value);
}

/// <summary>
/// 
/// </summary>
BCFProject& Extensions::GetProject()
{
    return m_project;
}
