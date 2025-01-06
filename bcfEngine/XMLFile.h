#pragma once

#include "BCFObject.h"
#include "ListOf.h"
#include "Log.h"


/// <summary>
/// base class for XML serialized data block
/// </summary>
class XMLFile : public BCFObject
{
public:
    XMLFile(BCFProject& project) : BCFObject(project) {}

    bool ReadFile(const std::string& folder);
    bool WriteFile(const std::string& folder);

protected:
    virtual const char* XMLFileName() = NULL;
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) = NULL;

};


/// <summary>
/// XML element content text
/// </summary>
class XMLText : public BCFObject
{
public:
    XMLText(BCFTopic&);
    
    void Read(_xml::_element& elem, const std::string&);

private:
    std::string m_str;
};

/// <summary>
/// XML reading macros
/// </summary>
enum class UnknownNames : bool
{
    Allowed = false,
    NotAllowed = true
};

#define ATTRS_START                         \
    for (auto attr : elem.attributes()) {   \
    if (attr) {                             \
        auto& attrName = attr->getName();   \

#define ATTR_GET(name)                          \
        if (attrName == #name) {                \
            m_##name.assign (attr->getValue()); \
        } else

#define ATTRS_END(onUnknownNames)           \
        if ((bool)onUnknownNames) { Project().log().add(Log::Level::warning, "XML parsing", "Unknown attribute %s in " __FUNCTION__, attrName.c_str()); } } }


/// <summary>
/// 
/// </summary>
#define CHILDREN_START                      \
    for (auto child : elem.children()) {    \
        if (child) {                        \
            auto&  tag= child->getName();   \

#define CHILD_GET_CONTENT(name)                         \
            if (tag == #name) {                         \
                m_##name.assign(child->getContent());   \
            } else

#define CHILD_READ(name)                                \
            if (tag == #name) {                         \
                Read_##name(*child, folder);            \
            } else


#define CHILD_READ_MEMBER(name)                         \
            if (tag == #name) {                        \
                m_##name.Read(*child, folder);         \
            } else

#define CHILD_GET_LIST(listName, elemName)                                                          \
            if (tag == #elemName) {                                                                 \
                ReadList(m_##listName, *this, *child, folder, NULL, m_project.log());               \
            }                                                                                       \
            else if(tag == #listName) {                                                             \
                ReadList(m_##listName, *this, *child, folder, #elemName, m_project.log());          \
            } else

#define CHILD_ADD_TO_LIST(listName, elemName)                                                       \
            if(tag == #elemName) {                                                                  \
                AddToList(m_##listName, *this, *child, folder);                                     \
            } else

#define CHILDREN_END \
        { Project().log().add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str()); } } }


/// <summary>
/// 
/// </summary>
template <class TReadable, class TContainer>
void AddToList(ListOf<TReadable>& list, TContainer& container, _xml::_element& elem, const std::string& folder)
{
    auto item = new TReadable(container);
    item->Read(elem, folder);
    list.push_back(item);
}

/// <summary>
/// 
/// </summary>
template <class TReadable, class TContainer> 
void ReadList(ListOf<TReadable>& list, TContainer& container, _xml::_element& elem, const std::string& folder, const char* childName, Log& log)
{
    if (!childName) {
        auto item = new TReadable(container);
        item->Read(elem, folder);
        list.Add(item);
    }
    else {
        for (auto child : elem.children()) {
            if (child) {
                auto& tag = child->getName();
                if (tag == childName) {
                    auto item = new TReadable(container);
                    item->Read(*child, folder);
                    list.Add(item);
                }
                else {
                    log.add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str());
                }
            }
        }
    }
}
