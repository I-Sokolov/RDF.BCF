#pragma once

struct BCFProject;

/// <summary>
/// 
/// </summary>
struct BCFGuid : private std::string
{
public:
    BCFGuid(BCFProject& project, const char* guid) : m_project(project) { if (guid) assign(guid); }

    void CreateNew();

    bool empty() const { return __super::empty(); }
    void assign(const std::string& s);

    const char* c_str() { return __super::c_str(); }

private:
    BCFProject& m_project;
};


/// <summary>
/// 
/// </summary>
struct GuidReference
{
public:
    GuidReference(BCFProject& project) : m_project(project), m_Guid(project, NULL) {}

    void Read(_xml::_element& elem, const std::string&);

    const char* Guid() { return m_Guid.c_str(); }

private:
    BCFProject& m_project;

    BCFGuid     m_Guid;
};

/// <summary>
/// 
/// </summary>
template <class Item>
struct OwningList : public std::vector<Item*>
{
    ~OwningList()
    {
        for (auto item : *this) {
            delete item;
        }
    }
};


/// <summary>
/// 
/// </summary>
template <class Item>
struct GuidList : public OwningList<Item>
{
    void push_back(Item* item)
    {
        if (*item->Guid()) {

            for (auto it = this->begin(); it != this->end(); it++) {
                if (0 == strcmp(item->Guid(), (*it)->Guid())) {
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
};
