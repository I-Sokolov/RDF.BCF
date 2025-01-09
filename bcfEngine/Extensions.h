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
    virtual const char* XSDName() override { return "extensions.xsd"; }
    virtual const char* RootElemName() override { return "Extensions"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

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

    void WriteEnumeration(_xml_writer& writer, const std::string& tag, BCFEnumeration enumeration);
    void Write_TopicTypes(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "TopicType", BCFTopicTypes); }
    void Write_Priorities(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "Priority", BCFPriorities); }
    void Write_TopicStatuses(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "TopicStatus", BCFTopicStatuses); }
    void Write_TopicLabels(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "TopicLabel", BCFTopicLabels); }
    void Write_Users(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "User", BCFUsers); }
    void Write_SnippetTypes(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "SnippetType", BCFSnippetTypes); }
    void Write_Stages(_xml_writer& writer, const std::string& folder) { WriteEnumeration(writer, "Stage", BCFStages); }

private:
    std::vector<StringSet>  m_elements;
};

