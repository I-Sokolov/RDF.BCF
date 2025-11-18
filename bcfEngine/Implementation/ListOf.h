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
    bool Add(BCFObject* item);

    Project& Project_();

protected:
    ListOfBCFObjects(BCFObject& container) : m_container(container) {}
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
    BCFObject&  m_container;

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
    ListOf (BCFObject& container) : ListOfBCFObjects(container) {}

public:
    Item* GetAt(uint16_t ind)
    {
        return dynamic_cast<Item*> (ListOfBCFObjects::GetAt(ind));
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
    SetByGuid(BCFObject& container) : ListOf<Item>(container) {}

public:
    bool Add(Item* item)
    {
        if (item && *item->GetGuid()) {

            if (auto found = FindByGuid(item->GetGuid())) {
                this->LogDuplicatedGuid(item->GetGuid());
                found->Remove();
            }

            return ListOf<Item>::Add(item);
        }
        else {
            assert(false);
            return false;
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
    ListOfComponents(BCFObject& container) : ListOf<Component>(container) {}

    Component* Add(ViewPoint& viewPoint, const char* ifcGuid, const char* authoringToolId, const char* originatingSystem);
};


/// <summary>
/// 
/// </summary>
class SetOfXMLText : public ListOf<XMLText>
{
public:
    SetOfXMLText(Topic& topic);

    bool Add(const char* val);
    XMLText* Find(const char* val);
    const char* GetAt(uint16_t ind);
    bool Remove(const char* val);

private:
    Topic& m_topic;
};