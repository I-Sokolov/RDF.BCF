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
    virtual void GetRelativePathName(std::string& pathInBcfFolder) override;
    virtual void ReadRoot(_xml::_element& elem) override;

private:
    void Read_Header(_xml::_element& elem);
    void Read_Topic(_xml::_element& elem);

private:
    BCFProject& m_project;

    //Header
    std::vector<BIMFile>            m_lstFile;

    //Topic
    std::string                     m_ServerAssignedId;
    std::string                     m_TopicStatus;
    std::string                     m_TopicType;
    std::string                     m_Title;
    std::vector<XMLText>            m_lstReferenceLink;
    std::string                     m_Priority;
    std::string                     m_Index;
    std::vector<XMLText>            m_lstLabel;
    std::string                     m_CreationDate;
    std::string                     m_CreationAuthor;
    std::string                     m_ModifiedDate;
    std::string                     m_ModifiedAuthor;
    std::string                     m_DueDate;
    std::string                     m_AssignedTo;
    std::string                     m_Description;
    std::string                     m_Stage;
    std::vector<BimSnippet>         m_lstBimSnippet;
    std::vector<DocumentReference>  m_lstDocumentReference;
    std::vector<GuidReference>      m_lstRelatedTopic;
    std::vector<Comment>            m_lstComment;
    std::vector<ViewPoint>          m_lstViewPoint;
};

