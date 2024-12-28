#pragma once

struct BCFProject;

/// <summary>
/// 
/// </summary>
struct BCFObject
{
public:
    BCFObject(BCFProject& project) : m_project(project) {}

    BCFProject& Project() { return m_project; }

protected:
    static std::string GetCurrentDate() { return GetCurrentTime(); }
    static std::string GetCurrentTime();

    bool SetIntVal(std::string& prop, int val);

protected:
    BCFProject& m_project;
};

/// <summary>
/// 
/// </summary>
struct BCFGuid : public BCFObject
{
public:
    BCFGuid(BCFProject& project, const char* guid);

    void CreateNew();

    bool empty() const { return value.empty(); }
    void assign(const std::string& s);

    const char* c_str() { return value.c_str(); }

private:
    std::string value;
};


/// <summary>
/// 
/// </summary>
struct GuidReference : public BCFObject
{
public:
    GuidReference(BCFProject& project) : BCFObject(project), m_Guid(project, NULL) {}

    void Read(_xml::_element& elem, const std::string&);

    const char* Guid() { return m_Guid.c_str(); }

private:
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
