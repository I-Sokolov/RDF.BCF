#pragma once

#include "Log.h"
#include "bcfEngine.h"

class Extensions
{
public:
    Extensions(Log& log);

    bool Read(const std::string& bcfFolder);
    bool Write(const std::string& bcfFolder);

    const char* GetElement(BCFEnumeration enumeration, unsigned short index);
    bool AddElement(BCFEnumeration enumeration, const char* element);
    bool RemoveElement(BCFEnumeration enumeration, const char* element);

private:
    StringSet* GetList(BCFEnumeration enumeration);

    void ReadRoot(_xml::_element& elem);
    void ReadEnumeration(_xml::_element& elem, BCFEnumeration enumeration);
    void Read_TopicTypes(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicTypes); }
    void Read_Priorities(_xml::_element& elem) { ReadEnumeration(elem, BCFPriorities); }
    void Read_TopicStatuses(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicStatuses); }
    void Read_TopicLabels(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicLabels); }
    void Read_Users(_xml::_element& elem) { ReadEnumeration(elem, BCFUsers); }
    void Read_SnippetTypes(_xml::_element& elem) { ReadEnumeration(elem, BCFSnippetTypes); }
    void Read_Stages(_xml::_element& elem) { ReadEnumeration(elem, BCFStages); }

private:
    Log& m_log;
    std::vector<StringSet>  m_elements;
};

