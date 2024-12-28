#pragma once

#include "XMLFile.h"

class Extensions : public XMLFile
{
public:
    Extensions(BCFProject& project);

    const char* GetElement(BCFEnumeration enumeration, BCFIndex index);
    bool AddElement(BCFEnumeration enumeration, const char* element);
    bool RemoveElement(BCFEnumeration enumeration, const char* element);

public:
    bool CheckElement(BCFEnumeration enumeration, const char* element);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "extensions.xml"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    StringSet* GetList(BCFEnumeration enumeration);

    void ReadEnumeration(_xml::_element& elem, const std::string& folder, BCFEnumeration enumeration);
    void Read_TopicTypes(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFTopicTypes); }
    void Read_Priorities(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFPriorities); }
    void Read_TopicStatuses(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFTopicStatuses); }
    void Read_TopicLabels(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFTopicLabels); }
    void Read_Users(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFUsers); }
    void Read_SnippetTypes(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFSnippetTypes); }
    void Read_Stages(_xml::_element& elem, const std::string& folder) { ReadEnumeration(elem, folder, BCFStages); }

private:
    std::vector<StringSet>  m_elements;
};

