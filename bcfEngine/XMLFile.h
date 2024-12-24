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
    virtual const char* FileName() = NULL;
    virtual void ReadRoot(_xml::_element& elem) = NULL;

protected:
    Log& m_log;
};


/// <summary>
/// XML reading macros
/// </summary>
enum class UnknownNames : bool
{
    Allowed = false,
    NotAllowed = true
};

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

#define END_ATTR(onUnknownNames)        \
            else if ((bool)onUnknownNames) { m_log.add(Log::Level::warning, "XML parsing", "Unknown attribute %s in " __FUNCTION__, attrName.c_str()); } } }


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
