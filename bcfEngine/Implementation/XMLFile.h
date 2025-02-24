#pragma once

#include "bcfTypes.h"
#include "BCFObject.h"
#include "ListOf.h"
#include "Log.h"


/// <summary>
/// base class for XML serialized data block
/// </summary>
class XMLFile : public BCFObject
{
public:
    struct Attributes : public vector<pair<string, string>>
    {
        void Add(const char* name, const char* value) { if (value && *value) push_back(Attributes::value_type(name, value)); }
    };

    struct ElemTag
    {
        ElemTag(_xml_writer& writer, const char* tag, Attributes& attr) : m_writer(writer), m_tag(tag) { writer.writeStartTag(tag, attr); writer.indent()++; }
        ~ElemTag() { m_writer.indent()--; m_writer.writeEndTag(m_tag); }
    private:
        _xml_writer& m_writer;
        const char* m_tag;
    };

public:
    XMLFile(Project& project, ListOfBCFObjects* parentList) : BCFObject(project, parentList) {}

    bool ReadFile(const std::string& folder);
    bool WriteFile(const std::string& folder);

protected:
    virtual const char* XMLFileName() = NULL;
    virtual const char* XSDName() = NULL;
    virtual const char* RootElemName() = NULL;
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) = NULL;    
    virtual void AfterRead(const std::string& folder) = NULL;
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr);
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) = NULL;

};


/// <summary>
/// XML element content text
/// </summary>
class XMLText : public BCFObject
{
public:
    XMLText(Topic&, ListOfBCFObjects* parentList);
    
    void Read(_xml::_element& elem, const std::string&);
    void Write(_xml_writer& writer, const std::string&, const char* tag);
    bool Validate(bool fix);

    bool Remove() { return RemoveImpl(); }

    std::string& string() { return m_str; }

    virtual Topic* Topic_() override { return &m_topic; }
    virtual Comment* Comment_() override { return NULL; }

private:
    Topic&      m_topic;

    std::string m_str;
};

/// <summary>
/// XML writing macros
/// </summary>
/// 
#define REQUIRED(prop, condition)                                                   \
    if (!(condition)) {                                                             \
        Log_().add(Log::Level::error, "Missed property or wrong value", #prop);   \
        valid = false;                                                              \
    }

#define REQUIRED_PROP(prop) REQUIRED(prop, !m_##prop.empty())



#define ATTR_ADD(name) attr.Add(#name,m_##name.c_str())

#define WRITE_CONTENT(name)   if (!m_##name.empty()) writer.writeTag(#name, m_##name)

#define WRITE_MEMBER(name)    m_##name.Write(writer, folder, #name)

#define WRITE_ELEM(name)                        \
    writer.writeStartTag(#name, attr);          \
    writer.indent()++;                          \
                                                \
    Write_##name(writer, folder);               \
                                                \
    writer.indent()--;                          \
    writer.writeEndTag(#name)

#define WRITE_LIST_EX(list, elem)               \
    if (!m_##list.Items().empty()) {            \
        writer.writeStartTag(#list);            \
        writer.indent()++;                      \
                                                \
        for (auto item : m_##list.Items()) {    \
            item->Write(writer, folder, #elem); \
        }                                       \
                                                \
        writer.indent()--;                      \
        writer.writeEndTag(#list);              \
    }

#define WRITE_LIST(elem) WRITE_LIST_EX(##elem##s, elem)

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

#define ATTR_GET_STR(name, str)                \
        if (attrName == #name) {               \
            str.assign (attr->getValue());     \
        } else

#define ATTR_GET(name) ATTR_GET_STR(name, m_##name)

#define ATTRS_END(onUnknownNames)           \
        if ((bool)onUnknownNames) { Project_().Log_().add(Log::Level::warning, "XML parsing", "Unknown attribute '%s' in " __FUNCTION__, attrName.c_str()); assert(!"TODO?"); } } }


static inline bool IsEmpty(_xml::_element* elem)
{
    if (!elem) {
        return true;
    }

    //auto& tag = elem->getName();
    auto& attr = elem->attributes();
    auto& child = elem->children();
    auto& text = elem->getContent();

    if (attr.empty() && child.empty() && text.empty())
        return true;

    return false;
}

/// <summary>
/// 
/// </summary>
#define CHILDREN_START                      \
    for (auto child : elem.children()) {    \
        if (!IsEmpty(child)) {              \
            auto&  tag= child->getName();   

#define CHILD_GET_CONTENT_STR(name, str)             \
            if (tag == #name) {                      \
                str.assign(child->getContent());     \
            } else

#define CHILD_GET_CONTENT(name) CHILD_GET_CONTENT_STR(name, m_##name)

#define CHILD_READ_FUNC(name, func)                     \
            if (tag == #name) {                         \
                ##func(*child, folder);                 \
            } else

#define CHILD_READ(name)        CHILD_READ_FUNC(name, Read_##name)

#define CHILD_READ_MEMBER(name) CHILD_READ_FUNC(name, m_##name.Read)

#define CHILD_GET_LIST_CONDITIONAL(listName, elemName, condition)                                   \
            if ((tag == #elemName) && (condition)) {                                                \
                ReadList(m_##listName, *this, *child, folder, NULL, m_project.Log_());            \
            }                                                                                       \
            else if((tag == #listName) && (condition)) {                                            \
                ReadList(m_##listName, *this, *child, folder, #elemName, m_project.Log_());       \
            } else

#define CHILD_GET_LIST(listName, elemName)       CHILD_GET_LIST_CONDITIONAL(listName, elemName, true)

#define CHILD_ADD_TO_LIST_CONDITIONAL(listName, elemName, condition)                                \
            if((tag == #elemName) && (condition)) {                                                 \
                AddToList(m_##listName, *this, *child, folder);                                     \
            } else

#define CHILD_ADD_TO_LIST(listName, elemName)   CHILD_ADD_TO_LIST_CONDITIONAL(listName, elemName, true)

#define CHILD_UNEXPECTED(tag) { Project_().Log_().add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str()); assert(!"TODO?"); }

#define CHILDREN_END CHILD_UNEXPECTED(tag) } }


/// <summary>
/// 
/// </summary>
template <class TReadable, class TContainer>
void AddToList(ListOf<TReadable>& list, TContainer& container, _xml::_element& elem, const std::string& folder)
{
    auto item = new TReadable(container, &list);
    item->Read(elem, folder);
    list.Add(item);
}

/// <summary>
/// 
/// </summary>
template <class TReadable, class TContainer> 
void ReadList(ListOf<TReadable>& list, TContainer& container, _xml::_element& elem, const std::string& folder, const char* childName, Log& log)
{
    if (!childName) {
        auto item = new TReadable(container, &list);
        item->Read(elem, folder);
        list.Add(item);
    }
    else {
        for (auto child : elem.children()) {
            if (child) {
                auto& tag = child->getName();
                if (tag == childName) {
                    auto item = new TReadable(container, &list);
                    item->Read(*child, folder);
                    list.Add(item);
                }
                else {
                    log.add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str());
                    assert(false);
                }
            }
        }
    }
}
