#pragma once

#include "XMLFile.h"

class Extensions : public XMLFile
{
public:
    Extensions(Log& log);

    const char* GetElement(BCFEnumeration enumeration, BCFIndex index);
    bool AddElement(BCFEnumeration enumeration, const char* element);
    bool RemoveElement(BCFEnumeration enumeration, const char* element);

private:
    virtual const char* FileName() override { return "extensions.xml"; }
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    StringSet* GetList(BCFEnumeration enumeration);

    void ReadEnumeration(_xml::_element& elem, BCFEnumeration enumeration);
    void Read_TopicTypes(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicTypes); }
    void Read_Priorities(_xml::_element& elem) { ReadEnumeration(elem, BCFPriorities); }
    void Read_TopicStatuses(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicStatuses); }
    void Read_TopicLabels(_xml::_element& elem) { ReadEnumeration(elem, BCFTopicLabels); }
    void Read_Users(_xml::_element& elem) { ReadEnumeration(elem, BCFUsers); }
    void Read_SnippetTypes(_xml::_element& elem) { ReadEnumeration(elem, BCFSnippetTypes); }
    void Read_Stages(_xml::_element& elem) { ReadEnumeration(elem, BCFStages); }

private:
    std::vector<StringSet>  m_elements;
};

