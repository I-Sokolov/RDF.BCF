#pragma once

#include "bcfAPI.h"
#include "XMLFile.h"

class Extensions : public XMLFile, public BCFExtensions
{
public:
    Extensions(Project& project);

public:
    //BCF API
    virtual const char* GetElement(BCFEnumeration enumeration, uint16_t ind) override;
    virtual bool AddElement(BCFEnumeration enumeration, const char* element) override;
    virtual bool RemoveElement(BCFEnumeration enumeration, const char* element) override;

public:
    bool CheckElement(BCFEnumeration enumeration, const char* element);
    void ReadExtensionSchema(_xml::_element& elem, const std::string& folder); //v2.1
    bool Validate(bool) { return true; }

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "extensions.xml"; }
    virtual const char* XSDName() override { return "extensions.xsd"; }
    virtual const char* RootElemName() override { return "Extensions"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string&) override  {}
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

private:
    StringSet* GetList(BCFEnumeration enumeration);
    BCFEnumeration FromName(const std::string& enumName);

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

    //v2.1
    void ReadExtensionSchema_redefine(_xml::_element& elem, const std::string& folder);
    void ReadExtensionSchema_simpleType(_xml::_element& elem, const std::string& folder);
    void ReadExtensionSchema_restriction(_xml::_element& elem, const std::string& folder);
    void ReadExtensionSchema_enumeration(_xml::_element& elem, const std::string& folder);

private:
    std::vector<StringSet>  m_elements;

    StringSet*              m_currentList;
};

