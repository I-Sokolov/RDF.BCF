#pragma once

struct BCFProject;
struct BCFObject;
struct BCFComponent;
struct BCFViewPoint;

/// <summary>
/// 
/// </summary>
class ListOfProjectObjects
{
public:
    bool Remove(BCFObject* item);
    void Add(BCFObject* item) { assert(item); if (item) m_items.push_back(item); }

protected:
    ListOfProjectObjects(BCFProject& project) : m_project(project) {}
    ~ListOfProjectObjects();

    BCFObject* GetNext(BCFObject* prev);

    void LogDuplicatedGuid(const char* guid);

private:
    typedef std::list<BCFObject*> Items;
    typedef Items::iterator Iterator;

private:
    void Clean(Items& items);
    Iterator Find(BCFObject* item);

protected:
    BCFProject& m_project;

    Items       m_items;
    Items       m_removed;
};



/// <summary>
/// 
/// </summary>
template <class Item>
class ListOf : public ListOfProjectObjects
{
public:
    ListOf (BCFProject& project) : ListOfProjectObjects(project) {}

public:
    Item* GetNext(Item* prev)
    {
        auto next = __super::GetNext(prev);
        return dynamic_cast<Item*>(next);
    }

    std::list<Item*>& Items()
    {
        return (std::list<Item*>&) m_items;
    }
};


/// <summary>
/// 
/// </summary>
template <class Item>
class ListGuid : public ListOf<Item>
{
public:
    ListGuid(BCFProject& project) : ListOf<Item>(project) {}

public:
    void Add(Item* item) 
    {        
        if (item && *item->GetGuid()) {

            for (auto it = this->Items().begin(); it != this->Items().end(); it++) {
                if (0 == strcmp(item->GetGuid(), (*it)->GetGuid())) {
                    this->LogDuplicatedGuid(item->GetGuid());
                    this->Remove(item);
                    break;
                }
            }

            __super::Add(item);
        }
        else {
            assert(false);
        }
    }
};

/// <summary>
/// 
/// </summary>
class ListOfComponents : public ListOf<BCFComponent>
{
public:
    ListOfComponents(BCFProject& project) : ListOf<BCFComponent>(project) {}

    BCFComponent* Add(BCFViewPoint& viewPoint, const char* ifcGuid, const char* authoringToolId, const char* originatingSystem);
};
