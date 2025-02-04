#pragma once

class BCFProject;

/// <summary>
/// 
/// </summary>
struct BCFGuid
{
public:
    BCFGuid(BCFProject& project, const char* guid);

    void CreateNew();

    bool empty() const { return value.empty(); }
    void assign(const std::string& s);

    const char* c_str() { return value.c_str(); }

private:
    BCFProject& m_project;
    std::string value;
};


/// <summary>
/// 
/// </summary>
struct GuidReference : public BCFObject
{
public:
    GuidReference(BCFTopic& topic);

    void Read(_xml::_element& elem, const std::string&);

    const char* GetGuid() { return m_Guid.c_str(); }
    void SetGuid(const char* guid) { if (guid && *guid) m_Guid.assign(guid); else m_Guid.clear(); }

private:
    std::string    m_Guid;
};

/// <summary>
/// 
/// </summary>
template <class Item>
struct OwningList 
{
public:
    ~OwningList()
    {
        Clean(m_items);
        Clean(m_removed);
    }

    Item* 

    Item* GetNext(Item* prev, Log& log)
    {
        auto it = m_items.begin();

        if (prev) {
            it = GetNexIterator(prev);
        }

        return (it == m_items.end()) ? NULL : *it;
    }

    bool Remove(BCFIndex index, Log& log)
    {
        auto N = this->size();

        if (index < N) {

            if ((*this)[index])
                delete (*this)[index];

            for (auto i = index; i < N-1; i++) {
                (*this)[i] = (*this)[i + 1];
            }
            this->resize(N - 1);

            return true;
        }
        else {
            log.add(Log::Level::error, "Index is out of range", "Index %d is out of topics range [0..%d]", (int)index, (int)(this->size()));
            return false;
        }
    }

private:
    typedef std::list<Item*> Items;
    typedef Items::iterator Iterator;

private:
    void Clean(Items& items)
    {
        for (auto item : m_items) {
            delete item;
        }
    }

    Iterator GetNexIterator(Item* item)
    {        
        for (auto it = m_items.begin(); it != m_items.end(); it++) {
            if (*it == item) {
                return ++it;
            }
        }
        log.add(Log::Level::error, "Item is not from list", "Item is not found in the list");
        return m_items.end();
    }

private:
    Items m_items;
    Items m_removed;
};


/// <summary>
/// 
/// </summary>
template <class Item>
struct GuidList : public OwningList<Item>
{
    void push_back(Item* item)
    {
        if (*item->GetGuid()) {

            for (auto it = this->begin(); it != this->end(); it++) {
                if (0 == strcmp(item->GetGuid(), (*it)->GetGuid())) {
                    //LogDuplicatedGuid(item->Guid());
                    delete (*it);
                    this->erase(it);
                    break;
                }
            }

            __super::push_back(item);
        }
        else {
            assert(!"Guid must be set before adding to list");
        }
    }

private:
    //void LogDuplicatedGuid(BCFGuid& guid);

};
