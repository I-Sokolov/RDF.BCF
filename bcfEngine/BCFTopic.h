#pragma once

#include "XMLFile.h"
#include "GuidStr.h"

#include "BimSnippet.h"
struct GuidReference;

struct BCFTopic : public XMLFile
{
public:
    BCFTopic(BCFProject& project, const char* guid);
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

    bool Remove(void);

    BCFViewPoint* ViewPointCreate(const char* guid = NULL);
    BCFViewPoint* ViewPointIterate(BCFViewPoint* prev);
    BCFViewPoint* ViewPointByGuid(const char* guid);
    bool          ViewPointRemove(BCFViewPoint* viewPoint);

    BCFComment* CommentCreate(const char* guid = NULL);
    BCFComment* CommentIterate(BCFComment* prev);
    bool        CommentRemove(BCFComment* comment);

public:
    BimSnippet& GetBimSnippet() { return m_BimSnippet; }

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "markup.bcf"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Header(_xml::_element& elem, const std::string& folder);
    void Read_Topic(_xml::_element& elem, const std::string& folder);

    bool UpdateAuthor();

private:
    GuidStr                         m_Guid;
    ListOf<BCFFile>                 m_Files;
    std::string                     m_ServerAssignedId;
    std::string                     m_TopicStatus;
    std::string                     m_TopicType;
    std::string                     m_Title;
    ListOf<XMLText>                 m_ReferenceLinks;
    std::string                     m_Priority;
    std::string                     m_Index;
    ListOf<XMLText>                 m_Labels;
    std::string                     m_CreationDate;
    std::string                     m_CreationAuthor;
    std::string                     m_ModifiedDate;
    std::string                     m_ModifiedAuthor;
    std::string                     m_DueDate;
    std::string                     m_AssignedTo;
    std::string                     m_Description;
    std::string                     m_Stage;
    BimSnippet                      m_BimSnippet;
    ListGuid<BCFDocumentReference>     m_DocumentReferences;
    ListGuid<GuidReference>         m_RelatedTopics;
    ListGuid<BCFComment>               m_Comments;
    ListGuid<BCFViewPoint>             m_Viewpoints;

private:
    bool                            m_bReadFromFile;
};
