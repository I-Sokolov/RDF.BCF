#pragma once

struct BCFProject;

#include "GUIDable.h"
#include "XMLFile.h"
#include "BIMFile.h"
#include "Comment.h"
#include "ViewPoint.h"
#include "BimSnippet.h"
#include "DocumentReference.h"

class Topic : public XMLFile
{
public:
    Topic(BCFProject& project, const char* guid);

public:
    const char* Guid() { return m_Guid.c_str(); }

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

public:
    BimSnippet& GetBimSnippet() { return m_BimSnippet; }

    bool UpdateAuthor();

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "markup.bcf"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Header(_xml::_element& elem, const std::string& folder);
    void Read_Topic(_xml::_element& elem, const std::string& folder);

private:
    BCFGuid                         m_Guid;
    OwningList<BIMFile>             m_Files;
    std::string                     m_ServerAssignedId;
    std::string                     m_TopicStatus;
    std::string                     m_TopicType;
    std::string                     m_Title;
    OwningList<XMLText>             m_ReferenceLinks;
    std::string                     m_Priority;
    std::string                     m_Index;
    OwningList<XMLText>             m_Labels;
    std::string                     m_CreationDate;
    std::string                     m_CreationAuthor;
    std::string                     m_ModifiedDate;
    std::string                     m_ModifiedAuthor;
    std::string                     m_DueDate;
    std::string                     m_AssignedTo;
    std::string                     m_Description;
    std::string                     m_Stage;
    BimSnippet                      m_BimSnippet;
    GuidList<DocumentReference>     m_DocumentReferences;
    GuidList<GuidReference>         m_RelatedTopics;
    GuidList<Comment>               m_Comments;
    GuidList<ViewPoint>             m_Viewpoints;

private:
    bool                            m_bReadFromFile;
};

