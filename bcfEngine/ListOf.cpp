#include "pch.h"
#include "ListOf.h"
#include "BCFProject.h"
#include "BCFComponent.h"

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

