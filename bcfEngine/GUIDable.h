#pragma once

#include "bcfTypes.h"
#include "Log.h"

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

    Item* Get(BCFIndex index, Log& log)
    {
        if (index < this->size()) {
            return (*this)[index];
        }
        else {
            log.add(Log::Level::error, "Index is out of range", "Index %d is out of topics range [0..%d]", (int)index, (int)(this->size()));
            return NULL;
        }
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
