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
    XMLFile(BCFProject& project, ListOfBCFObjects* parentList) : BCFObject(project, parentList) {}

    bool ReadFile(const std::string& folder);
    bool WriteFile(const std::string& folder);

protected:
    virtual const char* XMLFileName() = NULL;
    virtual const char* XSDName() = NULL;
    virtual const char* RootElemName() = NULL;
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) = NULL;    
    virtual void WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr);
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) = NULL;

};


/// <summary>
/// XML element content text
/// </summary>
class XMLText : public BCFObject
{
public:
    XMLText(BCFTopic&, ListOfBCFObjects* parentList);
    
    void Read(_xml::_element& elem, const std::string&);
    void Write(_xml_writer& writer, const std::string&, const char* tag);

    std::string& string() { return m_str; }

private:
    std::string m_str;
};

/// <summary>
/// XML writing macros
/// </summary>
/// 
#define REQUIRED(prop, condition)                                               \
    if (!(condition)) {                                                         \
        log().add(Log::Level::error, "Missed property or wrong value", #prop);  \
        throw std::exception();                                                 \
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

#define ATTR_GET(name)                          \
        if (attrName == #name) {                \
            m_##name.assign (attr->getValue()); \
        } else

#define ATTRS_END(onUnknownNames)           \
        if ((bool)onUnknownNames) { Project().log().add(Log::Level::warning, "XML parsing", "Unknown attribute %s in " __FUNCTION__, attrName.c_str()); assert(!"TODO?"); } } }


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
        { Project().log().add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str()); assert(!"TODO?"); } } }


/// <summary>
/// 
/// </summary>
template <class TReadable, class TContainer>
void AddToList(ListOf<TReadable>& list, TContainer& container, _xml::_element& elem, const std::string& folder)
{
    auto item = new TReadable(container, &list);
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
                }
            }
        }
    }
}
