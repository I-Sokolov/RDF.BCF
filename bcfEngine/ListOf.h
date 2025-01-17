#pragma once

struct BCFProject;
struct BCFTopic;
struct BCFObject;
struct BCFComponent;
struct BCFViewPoint;
class XMLText;

/// <summary>
/// 
/// </summary>
class ListOfBCFObjects
{
public:
    bool Remove(BCFObject* item);
    void Add(BCFObject* item) { assert(item); if (item) m_items.push_back(item); }

protected:
    ListOfBCFObjects(BCFProject& project) : m_project(project) {}
    ~ListOfBCFObjects();

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
class ListOf : public ListOfBCFObjects
{
public:
    ListOf (BCFProject& project) : ListOfBCFObjects(project) {}

public:
    Item* GetNext(Item* prev)
    {
        auto next = __super::GetNext(prev);
        if (next) {
            assert(dynamic_cast<Item*>(next));
            return dynamic_cast<Item*>(next);
        }
        return NULL;
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
class SetByGuid : public ListOf<Item>
{
public:
    SetByGuid(BCFProject& project) : ListOf<Item>(project) {}

public:
    void Add(Item* item)
    {
        if (item && *item->GetGuid()) {

            if (auto found = FindByGuid(item->GetGuid())) {
                this->LogDuplicatedGuid(item->GetGuid());
                found->Remove();
            }

            __super::Add(item);
        }
        else {
            assert(false);
        }
    }

    Item* FindByGuid(const char* guid)
    {
        if (guid) {
            for (auto it = this->Items().begin(); it != this->Items().end(); it++) {
                if (0 == strcmp(guid, (*it)->GetGuid())) {
                    return *it;
                }
            }
        }
        return NULL;
    }
};

/// <summary>
/// 
/// </summary>
class ListOfBCFComponents : public ListOf<BCFComponent>
{
public:
    ListOfBCFComponents(BCFProject& project) : ListOf<BCFComponent>(project) {}

    BCFComponent* Add(BCFViewPoint& viewPoint, const char* ifcGuid, const char* authoringToolId, const char* originatingSystem);
};


/// <summary>
/// 
/// </summary>
class SetOfXMLText : public ListOf<XMLText>
{
public:
    SetOfXMLText(BCFTopic& topic);

    void Add(const char* val);
    XMLText* Find(const char* val);
    const char* GetNext(const char* prev);
    bool Remove(const char* val);

private:
    BCFTopic& m_topic;
};