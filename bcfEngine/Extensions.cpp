#include "pch.h"
#include "Extensions.h"

Extensions::Extensions(Log& log)
    :m_log(log)
{
    m_elements.resize(7);
}


bool Extensions::Read(const std::string& bcfFolder)
{
    assert(!"todo");
    return false;
}

bool Extensions::Write(const std::string& bcfFolder)
{
    assert(!"todo");
    return false;
}

bool Extensions::AddElement(BCFEnumeration enumeration, const char* element)
{
    NULL_CHECK(element);

    auto list = GetList(enumeration);
    if (!list) {
        return false;
    }

    list->insert(element);
    return true;
}

const char* Extensions::GetElement(BCFEnumeration enumeration, unsigned short index)
{
    auto list = GetList(enumeration);
    if (list) {
        for (auto& elem : *list) {
            if (index == 0) {
                return elem.c_str();
            }
            index--;
        }
    }
    return NULL;
}

bool Extensions::RemoveElement(BCFEnumeration enumeration, const char* element)
{
    NULL_CHECK(element);

    auto list = GetList(enumeration);
    if (!list) {
        return false;
    }

    list->erase(element);
    return true;
}

StringSet* Extensions::GetList(BCFEnumeration enumeration)
{
    size_t ind = enumeration - 1;
    
    if (ind < m_elements.size()) {
        return &m_elements[ind];
    }
    else {
        m_log.error("Index is out of range", "Index %d is out of extensions types range [0..%d]", (int)ind, (int)m_elements.size());
        return NULL;
    }
}

