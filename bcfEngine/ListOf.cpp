#include "pch.h"
#include "ListOf.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
ListOfProjectObjects::~ListOfProjectObjects()
{
    Clean(m_items);
    Clean(m_removed);
}

/// <summary>
/// 
/// </summary>
BCFObject* ListOfProjectObjects::GetNext(BCFObject* prev)
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
bool ListOfProjectObjects::Remove(BCFObject* item)
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
void ListOfProjectObjects::Clean(Items& items)
{
    for (auto item : m_items) {
        delete item;
    }
}

/// <summary>
/// 
/// </summary>
ListOfProjectObjects::Iterator ListOfProjectObjects::Find(BCFObject* item)
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
void ListOfProjectObjects::LogDuplicatedGuid(const char* guid)
{
    m_project.log().add(Log::Level::error, "Duplicated GUID");
}



