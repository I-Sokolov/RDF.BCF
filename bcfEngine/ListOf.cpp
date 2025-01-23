#include "pch.h"
#include "ListOf.h"
#include "BCFProject.h"
#include "BCFComponent.h"
#include "BCFTopic.h"

/// <summary>
/// 
/// </summary>
ListOfBCFObjects::~ListOfBCFObjects()
{
    Clean(m_items);
    Clean(m_removed);
}

/// <summary>
/// 
/// </summary>
BCFObject* ListOfBCFObjects::GetNext(BCFObject* prev)
{
    auto it = m_items.begin();

    if (prev) {
        it = Find(prev);
        if (it != m_items.end()) {
            it++;
        }
    }

    return (it == m_items.end()) ? NULL : *it;
}

/// <summary>
/// 
/// </summary>
bool ListOfBCFObjects::Remove(BCFObject* item)
{
    auto it = Find(item);

    if (it != m_items.end()) {
        m_removed.push_back(*it);
        m_items.erase(it);
        return true;
    }
    else {
        return false;
    }
}

/// <summary>
/// 
/// </summary>
void ListOfBCFObjects::Clean(Items& items)
{
    for (auto item : items) {
        delete item;
    }
    items.clear();
}

/// <summary>
/// 
/// </summary>
ListOfBCFObjects::Iterator ListOfBCFObjects::Find(BCFObject* item)
{
    for (auto it = m_items.begin(); it != m_items.end(); it++) {
        if (*it == item) {
            return it;
        }
    }
    m_project.log().add(Log::Level::error, "Item not found in the list");
    return m_items.end();
}

/// <summary>
/// 
/// </summary>
void ListOfBCFObjects::LogDuplicatedGuid(const char* guid)
{
    m_project.log().add(Log::Level::error, "Duplicated GUID");
}

/// <summary>
/// 
/// </summary>
SetOfXMLText::SetOfXMLText(BCFTopic& topic) 
    : ListOf<XMLText>(topic.Project()) 
    , m_topic(topic)
{
}


/// <summary>
/// 
/// </summary>
BCFComponent* ListOfBCFComponents::Add(BCFViewPoint& viewPoint, const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    auto comp = new BCFComponent(viewPoint, this);

    bool ok = true;
    if (ifcGuid) ok = ok && comp->SetIfcGuid(ifcGuid);
    if (authoringToolId) ok = ok && comp->SetAuthoringToolId(authoringToolId);
    if (originatingSystem) ok = ok && comp->SetOriginatingSystem(originatingSystem);

    if (ok) {
        __super::Add(comp);
        return comp;
    }
    else {
        delete comp;
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
void SetOfXMLText::Add(const char* val)
{
    if (val && *val) {
        if (!Find(val)) {
            auto txt = new XMLText(m_topic, this);
            txt->string().assign(val);
            __super::Add(txt);
        }
    }
}

/// <summary>
/// 
/// </summary>
XMLText* SetOfXMLText::Find(const char* val)
{
    if (val) {
        std::string sval(val);
        for (auto txt : Items()) {
            if (txt->string() == val) {
                return txt;
            }
        }
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
const char* SetOfXMLText::GetNext(const char* prev)
{
    XMLText* txtPrev = NULL;
    if (prev && *prev) {
        txtPrev = Find(prev);
        if (!txtPrev) {
            m_project.log().add(Log::Level::error, "Invalid element", "Text element %s is not in the list", prev);
            return NULL;
        }
    }

    auto txtNext = __super::GetNext(txtPrev);

    if (txtNext) {
        return txtNext->string().c_str();
    }
    else {
        return "";
    }
}

/// <summary>
/// 
/// </summary>
bool SetOfXMLText::Remove(const char* val)
{
    if (auto txt = Find(val)) {
        return txt->Remove();
    }
    return false;
}
