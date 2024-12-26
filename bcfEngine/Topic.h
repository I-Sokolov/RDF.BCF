#pragma once

struct BCFProject;

#include "GUIDable.h"
#include "XMLFile.h"
#include "BIMFile.h"
#include "Comment.h"
#include "ViewPoint.h"
#include "BimSnippet.h"
#include "DocumentReference.h"

class Topic : public GUIDable, public XMLFile
{
public:
    Topic(BCFProject& project, const char* guid);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "markup.bcf"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;

private:
    void Read_Header(_xml::_element& elem, const std::string& folder);
    void Read_Topic(_xml::_element& elem, const std::string& folder);

private:
    std::vector<BIMFile>            m_Files;
    std::string                     m_ServerAssignedId;
    std::string                     m_TopicStatus;
    std::string                     m_TopicType;
    std::string                     m_Title;
    std::vector<XMLText>            m_ReferenceLinks;
    std::string                     m_Priority;
    std::string                     m_Index;
    std::vector<XMLText>            m_Labels;
    std::string                     m_CreationDate;
    std::string                     m_CreationAuthor;
    std::string                     m_ModifiedDate;
    std::string                     m_ModifiedAuthor;
    std::string                     m_DueDate;
    std::string                     m_AssignedTo;
    std::string                     m_Description;
    std::string                     m_Stage;
    BimSnippet                      m_BimSnippet;
    std::vector<DocumentReference>  m_DocumentReferences;
    std::vector<GuidReference>      m_RelatedTopics;
    std::vector<Comment>            m_Comments;
    std::vector<ViewPoint>          m_Viewpoints;
};

