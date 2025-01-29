#pragma once

#include "bcfTypes.h"
#include "XMLFile.h"
#include "GuidStr.h"

struct GuidReference;

struct BCFTopic : public XMLFile
{
public:
    BCFTopic(BCFProject& project, ListOfBCFObjects* parentList, const char* guid);
    ~BCFTopic() {}

public:
    const char* GetGuid() { return m_Guid.c_str(); }
    const char* GetServerAssignedId() { return m_ServerAssignedId.c_str(); }
    const char* GetTopicStatus() { return m_TopicStatus.c_str(); }
    const char* GetTopicType() { return m_TopicType.c_str(); }
    const char* GetTitle() { return m_Title.c_str(); }
    const char* GetPriority() { return m_Priority.c_str(); }
    int GetIndex() { return atoi(m_Index.c_str()); }
    const char* GetCreationDate() { return m_CreationDate.c_str(); }
    const char* GetCreationAuthor() { return m_CreationAuthor.c_str(); }
    const char* GetModifiedDate() { return m_ModifiedDate.c_str(); }
    const char* GetModifiedAuthor() { return m_ModifiedAuthor.c_str(); }
    const char* GetDueDate() { return m_DueDate.c_str(); }
    const char* GetAssignedTo() { return m_AssignedTo.c_str(); }
    const char* GetDescription() { return m_Description.c_str(); }
    const char* GetStage() { return m_Stage.c_str(); }

    bool SetServerAssignedId(const char* val);
    bool SetTopicStatus(const char* val);
    bool SetTopicType(const char* val);
    bool SetTitle(const char* val);
    bool SetPriority(const char* val);
    bool SetIndex(int val);
    bool SetDueDate(const char* val);
    bool SetAssignedTo(const char* val);
    bool SetDescription(const char* val);
    bool SetStage(const char* val);

    BCFFile* FileAdd(const char* filePath, bool isExternal = true);
    BCFFile* FileIterate(BCFFile* prev);

    BCFViewPoint* ViewPointAdd(const char* guid = NULL);
    BCFViewPoint* ViewPointIterate(BCFViewPoint* prev);
    BCFViewPoint* ViewPointByGuid(const char* guid);

    BCFComment* CommentAdd(const char* guid = NULL);
    BCFComment* CommentIterate(BCFComment* prev);

    BCFDocumentReference* DocumentReferenceAdd(const char* filePath, bool isExternal = true, const char* guid = NULL);
    BCFDocumentReference* DocumentReferenceIterate(BCFDocumentReference* prev);

    BCFBimSnippet* GetBimSnippet(bool forceCreate);

    bool ReferenceLinkAdd(const char* val);
    const char* ReferenceLinkIterate(const char* prev);
    bool ReferenceLinkRemove(const char* val);

    bool LabelAdd(const char* val);
    const char* LabelIterate(const char* prev);
    bool LabelRemove(const char* val);

    bool RelatedTopicAdd(BCFTopic* topic);
    BCFTopic* RelatedTopicIterate(BCFTopic* prev);
    bool RelatedTopicRemove(BCFTopic* topic);

    bool UpdateAuthor();

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "markup.bcf"; }
    virtual const char* XSDName() override { return "markup.xsd"; }
    virtual const char* RootElemName() override { return "Markup"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void UpgradeReadVersion() override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

private:
    void Read_Header(_xml::_element& elem, const std::string& folder);
    void Read_Topic(_xml::_element& elem, const std::string& folder);

    void Write_Header(_xml_writer& writer, const std::string& folder);
    void Write_Topic(_xml_writer& writer, const std::string& folder);

    BCFTopic* GetNextRelatedTopic(const char* guid);

private:
    GuidStr                         m_Guid;
    ListOf<BCFFile>                 m_Files;
    std::string                     m_ServerAssignedId;
    std::string                     m_TopicStatus;
    std::string                     m_TopicType;
    std::string                     m_Title;
    SetOfXMLText                    m_ReferenceLinks;
    std::string                     m_Priority;
    std::string                     m_Index;
    SetOfXMLText                    m_Labels;
    std::string                     m_CreationDate;
    std::string                     m_CreationAuthor;
    std::string                     m_ModifiedDate;
    std::string                     m_ModifiedAuthor;
    std::string                     m_DueDate;
    std::string                     m_AssignedTo;
    std::string                     m_Description;
    std::string                     m_Stage;
    ListOf<BCFBimSnippet>           m_BimSnippets;
    SetByGuid<BCFDocumentReference> m_DocumentReferences;
    SetByGuid<GuidReference>        m_RelatedTopics;
    SetByGuid<BCFComment>           m_Comments;
    SetByGuid<BCFViewPoint>         m_Viewpoints;

private:
    bool                            m_bReadFromFile;
};

