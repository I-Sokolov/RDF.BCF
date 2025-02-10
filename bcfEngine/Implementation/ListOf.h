#pragma once

struct Project;
struct Topic;
struct BCFObject;
struct Component;
struct ViewPoint;
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
    ListOfBCFObjects(Project& project) : m_project(project) {}
    ~ListOfBCFObjects();

    BCFObject* GetAt(uint16_t ind);

    void LogDuplicatedGuid(const char* guid);

private:
    typedef std::vector<BCFObject*> Items;
    typedef Items::iterator Iterator;

private:
    void Clean(Items& items);
    Iterator Find(BCFObject* item);

protected:
    Project& m_project;

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
    ListOf (Project& project) : ListOfBCFObjects(project) {}

public:
    Item* GetAt(uint16_t ind)
    {
        return dynamic_cast<Item*> (__super::GetAt(ind));
    }

    std::vector<Item*>& Items()
    {
        return (std::vector<Item*>&) m_items;
    }

    void AfterRead(const std::string& folder)
    {
        std::vector<Item*> items = Items();//list copy list is intentional, next processing may remove components
        for (auto item : items) {
            item->AfterRead(folder);
        }
    }

    bool Validate(bool fix)
    {
        bool valid = true;
        std::vector<Item*> items = Items();//list copy list is intentional, next processing may remove components
        for (auto item : items) {
            valid = item->Validate(fix) && valid;
        }
        return valid;
    }
};


/// <summary>
/// 
/// </summary>
template <class Item>
class SetByGuid : public ListOf<Item>
{
public:
    SetByGuid(Project& project) : ListOf<Item>(project) {}

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
class ListOfComponents : public ListOf<Component>
{
public:
    ListOfComponents(Project& project) : ListOf<Component>(project) {}

    Component* Add(ViewPoint& viewPoint, const char* ifcGuid, const char* authoringToolId, const char* originatingSystem);
};


/// <summary>
/// 
/// </summary>
class SetOfXMLText : public ListOf<XMLText>
{
public:
    SetOfXMLText(Topic& topic);

    void Add(const char* val);
    XMLText* Find(const char* val);
    const char* GetAt(uint16_t ind);
    bool Remove(const char* val);

private:
    Topic& m_topic;
};