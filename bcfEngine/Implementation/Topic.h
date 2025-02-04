#pragma once

#include "bcfAPI.h"
#include "bcfTypes.h"
#include "XMLFile.h"
#include "GuidStr.h"

struct File;
struct Comment;
struct GuidReference;
struct BimSnippet;
struct DocumentReference;

struct Topic : public XMLFile, public BCFTopic
{
public:
    Topic(Project& project, ListOfBCFObjects* parentList, const char* guid);
    ~Topic() {}

public:
    //BCFTopic
    virtual const char* GetGuid() override { return m_Guid.c_str(); }
    virtual const char* GetServerAssignedId() override { return m_ServerAssignedId.c_str(); }
    virtual const char* GetTopicStatus() override { return m_TopicStatus.c_str(); }
    virtual const char* GetTopicType() override { return m_TopicType.c_str(); }
    virtual const char* GetTitle() override { return m_Title.c_str(); }
    virtual const char* GetPriority() override { return m_Priority.c_str(); }
    virtual int GetIndex() override { return atoi(m_Index.c_str()); }
    virtual const char* GetCreationDate() override { return m_CreationDate.c_str(); }
    virtual const char* GetCreationAuthor() override { return m_CreationAuthor.c_str(); }
    virtual const char* GetModifiedDate() override { return m_ModifiedDate.c_str(); }
    virtual const char* GetModifiedAuthor() override { return m_ModifiedAuthor.c_str(); }
    virtual const char* GetDueDate() override { return m_DueDate.c_str(); }
    virtual const char* GetAssignedTo() override { return m_AssignedTo.c_str(); }
    virtual const char* GetDescription() override { return m_Description.c_str(); }
    virtual const char* GetStage() override { return m_Stage.c_str(); }

    virtual bool SetServerAssignedId(const char* val) override;
    virtual bool SetTopicStatus(const char* val) override;
    virtual bool SetTopicType(const char* val) override;
    virtual bool SetTitle(const char* val) override;
    virtual bool SetPriority(const char* val) override;
    virtual bool SetIndex(int val) override;
    virtual bool SetDueDate(const char* val) override;
    virtual bool SetAssignedTo(const char* val) override;
    virtual bool SetDescription(const char* val) override;
    virtual bool SetStage(const char* val) override;

    virtual BCFFile* FileAdd(const char* filePath, bool isExternal = true) override;
    virtual BCFFile* FileIterate(BCFFile* prev) override;

    virtual BCFViewPoint* ViewPointAdd(const char* guid = NULL) override;
    virtual BCFViewPoint* ViewPointIterate(BCFViewPoint* prev) override;

    virtual BCFComment* CommentAdd(const char* guid = NULL) override;
    virtual BCFComment* CommentIterate(BCFComment* prev) override;

    virtual BCFDocumentReference* DocumentReferenceAdd(const char* filePath, bool isExternal = true, const char* guid = NULL) override;
    virtual BCFDocumentReference* DocumentReferenceIterate(BCFDocumentReference* prev) override;

    virtual BCFBimSnippet* GetBimSnippet(bool forceCreate) override;

    virtual bool ReferenceLinkAdd(const char* val) override;
    virtual const char* ReferenceLinkIterate(const char* prev) override;
    virtual bool ReferenceLinkRemove(const char* val) override;

    virtual bool LabelAdd(const char* val) override;
    virtual const char* LabelIterate(const char* prev) override;
    virtual bool LabelRemove(const char* val) override;

    virtual bool RelatedTopicAdd(BCFTopic* topic) override;
    virtual BCFTopic* RelatedTopicIterate(BCFTopic* prev) override;
    virtual bool RelatedTopicRemove(BCFTopic* topic) override;

public:
    ViewPoint* ViewPointByGuid(const char* guid);

    bool UpdateAuthor();

    bool Validate(bool fix);

public:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "markup.bcf"; }
    virtual const char* XSDName() override { return "markup.xsd"; }
    virtual const char* RootElemName() override { return "Markup"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string& folder) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;
    virtual bool Remove() override;

private:
    void Read_Header(_xml::_element& elem, const std::string& folder);
    void Read_Topic(_xml::_element& elem, const std::string& folder);

    void Write_Header(_xml_writer& writer, const std::string& folder);
    void Write_Topic(_xml_writer& writer, const std::string& folder);

    Topic* GetNextRelatedTopic(const char* guid);

private:
    GuidStr                         m_Guid;
    ListOf<File>                    m_Files;
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
    ListOf<BimSnippet>              m_BimSnippets;
    SetByGuid<DocumentReference>    m_DocumentReferences;
    SetByGuid<GuidReference>        m_RelatedTopics;
    SetByGuid<Comment>              m_Comments;
    SetByGuid<ViewPoint>            m_Viewpoints;

private:
    bool                            m_bReadFromFile;
};

