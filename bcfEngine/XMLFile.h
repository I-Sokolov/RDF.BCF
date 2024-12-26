#pragma once

#include "Log.h"

/// <summary>
/// base class for XML serialized data block
/// </summary>
class XMLFile
{
public:
    XMLFile(Log& log) :m_log(log) {}

    bool Read(const std::string& bcfFolder);
    bool Write(const std::string& bcfFolder);

protected:
    virtual void GetRelativePathName(std::string& pathInBcfFolder) = NULL;
    virtual void ReadRoot(_xml::_element& elem) = NULL;

protected:
    template <class TReadable> void ReadToList(std::vector<TReadable>& list, _xml::_element& elem, const char* childName);

protected:
    Log& m_log;
};


/// <summary>
/// XML element content text
/// </summary>
class XMLText : public std::string
{
public:
    XMLText(Log&){}
    void Read(_xml::_element& elem) { assign(elem.getContent()); }
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
        auto attrName = attr->getName();    \

#define ATTR_GET(name)                      \
        if (attrName == #name) {            \
            m_##name = attr->getValue();    \
        } else

#define ATTRS_END(onUnknownNames)           \
        if ((bool)onUnknownNames) { m_log.add(Log::Level::warning, "XML parsing", "Unknown attribute %s in " __FUNCTION__, attrName.c_str()); } } }


/// <summary>
/// 
/// </summary>
#define CHILDREN_START                      \
    for (auto child : elem.children()) {    \
        if (child) {                        \
            auto&  tag= child->getName();   \


#define CHILD_READ(name)                    \
            if (tag == #name) {             \
                Read_##name(*child);        \
            } else


#define CHILD_GET_CONTENT(name)                         \
            if (tag == #name) {                         \
                m_##name.assign(child->getContent());   \
            } else

#define CHILD_GET_LIST(name)                            \
            if (tag == #name) {                         \
                ReadToList(m_lst##name, *child, NULL);  \
            }                                           \
            else if(0==_stricmp(tag.c_str(),#name "s")){\
                ReadToList(m_lst##name, *child, #name); \
            } else


#define CHILDREN_END \
        { m_log.add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str()); } } }


/// <summary>
/// 
/// </summary>
template <class TReadable> void XMLFile::ReadToList(std::vector<TReadable>& list, _xml::_element& elem, const char* childName)
{
    if (!childName) {
        list.push_back(TReadable(m_log));
        list.back().Read(elem);
    }
    else {
        for (auto child : elem.children()) {
            if (child) {
                auto& tag = child->getName();
                if (tag == childName) {
                    list.push_back(TReadable(m_log));
                    list.back().Read(elem);
                }
                else {
                    m_log.add(Log::Level::error, "XML parsing", "Unknown child element <%s> in " __FUNCTION__, tag.c_str());
                }
            }
        }
    }
}
