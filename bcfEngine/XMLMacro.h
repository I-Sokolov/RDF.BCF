#pragma once

enum class UnknownNames : bool
{
    Allowed = false,
    NotAllowed = true
};

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

#define END_ATTR(allNamesKnown)        \
            else if ((bool)allNamesKnown) { m_log.add(Log::Level::warning, "XML parsing", "Unknown attribute %s in " __FUNCTION__, attrName.c_str()); } } }


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