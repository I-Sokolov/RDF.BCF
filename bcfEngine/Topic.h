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
};

