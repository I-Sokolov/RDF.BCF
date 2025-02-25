#include "pch.h"
#include "bcfTypes.h"
#include "Topic.h"
#include "Project.h"
#include "ViewPoint.h"
#include "Comment.h"
#include "BimFile.h"
#include "DocumentReference.h"
#include "BimSnippet.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
Topic::Topic(Project& project, ListOfBCFObjects* parentList, const char* guid)
    : XMLFile(project, parentList)
    , m_Guid(project, guid)
    , m_BimSnippets(*this)
    , m_bReadFromFile (false)
    , m_Files(*this)
    , m_ReferenceLinks(*this)
    , m_Labels(*this)
    , m_DocumentReferences(*this)
    , m_RelatedTopics(*this)
    , m_Comments(*this)
    , m_Viewpoints(*this)
{
}


/// <summary>
/// 
/// </summary>
void Topic::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(Header)
        CHILD_READ(Topic)

        CHILD_GET_LIST(Comments, Comment) //v2.1
        CHILD_ADD_TO_LIST(Viewpoints, Viewpoints)

    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::AfterRead(const std::string& folder)
{
    m_Comments.AfterRead(folder);
    m_DocumentReferences.AfterRead(folder);
}

/// <summary>
/// 
/// </summary>
bool Topic::Validate(bool fix)
{
    if (fix) {
        auto NOT_SET = "Not set";

        if (m_Guid.IsEmpty()) {
            m_Guid.AssignNew();
        }

        if (m_TopicType.empty()) {
            GetProject().GetExtensions().AddElement(BCFTopicTypes, NOT_SET);
            m_TopicType.assign(NOT_SET);
        }

        if (m_TopicStatus.empty()) {
            GetProject().GetExtensions().AddElement(BCFTopicStatuses, NOT_SET);
            m_TopicStatus.assign(NOT_SET);
        }

        if (m_CreationAuthor.empty()) {
            GetProject().GetExtensions().AddElement(BCFUsers, NOT_SET);
            m_CreationAuthor.assign(NOT_SET);
        }

        if (m_CreationDate.empty()) {
            m_CreationDate = GetCurrentDate();
        }

        if (m_Title.empty()) {
            m_Title.assign(NOT_SET);
        }
    }

    //
    bool valid = true;

    REQUIRED_PROP(TopicType);
    REQUIRED_PROP(TopicStatus);
    REQUIRED_PROP(Title);

    valid = m_Files.Validate(fix) && valid;
    valid = m_ReferenceLinks.Validate(fix) && valid;
    valid = m_Labels.Validate(fix) && valid;
    valid = m_BimSnippets.Validate(fix) && valid;
    valid = m_DocumentReferences.Validate(fix) && valid;
    valid = m_RelatedTopics.Validate(fix) && valid;
    valid = m_Comments.Validate(fix) && valid;
    valid = m_Viewpoints.Validate(fix) && valid;

    return valid;
}

/// <summary>
/// 
/// </summary>
void Topic::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    Attributes attr;

    WRITE_ELEM(Header);

    ATTR_ADD(Guid);
    ATTR_ADD(ServerAssignedId);
    ATTR_ADD(TopicStatus);
    ATTR_ADD(TopicType);
    WRITE_ELEM(Topic);
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Header(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Files, File)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::Write_Header(_xml_writer& writer, const std::string& folder)
{
    WRITE_LIST(File)
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Topic(_xml::_element& elem, const std::string& folder)
{
    m_bReadFromFile = true;
    std::string topicGuid;

    ATTRS_START
        ATTR_GET_STR(Guid, topicGuid)
        ATTR_GET(ServerAssignedId)
        ATTR_GET(TopicStatus)
        ATTR_GET(TopicType)
    ATTRS_END(UnknownNames::NotAllowed)

    //if (!topicGuid.empty() && strcmp(topicGuid.c_str(), m_Guid.c_str())) {
    //    printf("Data inconsistent: %s\n", folder.c_str());
    //}

    CHILDREN_START
        CHILD_GET_CONTENT(Title)
        CHILD_GET_LIST(ReferenceLinks, ReferenceLink)
        CHILD_GET_CONTENT(Priority)
        CHILD_GET_CONTENT(Index)
        CHILD_GET_LIST(Labels, Label)
        CHILD_GET_CONTENT(CreationDate)
        CHILD_GET_CONTENT(CreationAuthor)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
        CHILD_GET_CONTENT(DueDate)
        CHILD_GET_CONTENT(AssignedTo)
        CHILD_GET_CONTENT(Description)
        CHILD_GET_CONTENT(Stage)
        CHILD_GET_LIST_CONDITIONAL(DocumentReferences, DocumentReference, Project_().GetVersion() > BCFVer_2_0)
        CHILD_ADD_TO_LIST_CONDITIONAL(DocumentReferences, DocumentReferences, Project_().GetVersion() == BCFVer_2_0)
        CHILD_GET_LIST(RelatedTopics, RelatedTopic)
        CHILD_GET_LIST(Comments, Comment)
        CHILD_GET_LIST(Viewpoints, ViewPoint)
        CHILD_GET_LIST(BimSnippets, BimSnippet)
    CHILDREN_END
}

void Topic::Write_Topic(_xml_writer& writer, const std::string& folder)
{
    WRITE_LIST(ReferenceLink);
    WRITE_CONTENT(Title);
    WRITE_CONTENT(Priority);
    WRITE_CONTENT(Index);
    WRITE_LIST(Label);
    WRITE_CONTENT(CreationDate);
    WRITE_CONTENT(CreationAuthor);
    WRITE_CONTENT(ModifiedDate);
    WRITE_CONTENT(ModifiedAuthor);
    WRITE_CONTENT(DueDate);
    WRITE_CONTENT(AssignedTo);
    WRITE_CONTENT(Stage);
    WRITE_CONTENT(Description);
    if (!m_BimSnippets.Items().empty()) {
        m_BimSnippets.Items().front()->Write(writer, folder, "BimSnippet");
    }
    WRITE_LIST(DocumentReference);
    WRITE_LIST(RelatedTopic);
    WRITE_LIST(Comment);
    WRITE_LIST(Viewpoint);

}

/// <summary>
/// 
/// </summary>
bool Topic::SetServerAssignedId(const char* val)
{
    return SetPropertyString(val, m_ServerAssignedId);
}

/// <summary>
/// 
/// </summary>
bool Topic::SetTopicStatus(const char* val)
{
    if (Project_().GetExtensions_().CheckElement(BCFTopicStatuses, val)) {
        return SetPropertyString(val, m_TopicStatus);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetTopicType(const char* val)
{
    if (Project_().GetExtensions_().CheckElement(BCFTopicTypes, val)) {
        return SetPropertyString(val, m_TopicType);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetTitle(const char* val)
{
    return SetPropertyString(val, m_Title);
}

/// <summary>
/// 
/// </summary>
bool Topic::SetPriority(const char* val)
{
    if (Project_().GetExtensions_().CheckElement(BCFPriorities, val)) {
        return SetPropertyString(val, m_Priority);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetIndex(int val)
{
    return SetPropertyInt(val, m_Index);
}

bool Topic::SetIndexStr(const char* val)
{
    return SetPropertyString(val, m_Index);
}

/// <summary>
/// 
/// </summary>
bool Topic::SetDueDate(const char* val)
{
    VALIDATE(DueDate, DateTime);

    return SetPropertyString(val, m_DueDate);
}

/// <summary>
/// 
/// </summary>
bool Topic::SetAssignedTo(const char* val)
{
    if (Project_().GetExtensions_().CheckElement(BCFUsers, val)) {
        return SetPropertyString(val, m_AssignedTo);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetDescription(const char* val)
{
    return SetPropertyString(val, m_Description);
}

/// <summary>
/// 
/// </summary>
bool Topic::SetStage(const char* val)
{
    if (Project_().GetExtensions_().CheckElement(BCFStages, val)) {
        return SetPropertyString(val, m_Stage);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetEditorAndDate()
{
    return __super::SetEditorAndDate(m_bReadFromFile ? m_ModifiedAuthor : m_CreationAuthor, m_bReadFromFile ? m_ModifiedDate : m_CreationDate);
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* Topic::AddViewPoint(const char* guid)
{
    auto viewPoint = new ViewPoint(*this, &m_Viewpoints, guid ? guid : "");

    if (viewPoint) {
        m_Viewpoints.Add(viewPoint);
        return viewPoint;
    }

    return NULL;
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* Topic::GetViewPoint(uint16_t ind)
{
    return m_Viewpoints.GetAt(ind);
}


/// <summary>
/// 
/// </summary>
BCFBimFile* Topic::AddBimFile(const char* filePath, bool isExternal)
{
    auto file = new BimFile(*this, &m_Files);

    bool ok = true;

    if (file && filePath && *filePath) {
        ok = ok && file->SetIsExternal(isExternal);
        ok = ok && file->SetReference(filePath);
    }

    if (!ok) {
        delete file;
        file = NULL;
    }

    if (file) {
        m_Files.Add(file);
        return file;
    }
    else {
        return NULL;
    }

}

/// <summary>
/// 
/// </summary>
BCFBimFile* Topic::GetBimFile(uint16_t ind)
{
    return m_Files.GetAt(ind);
}


/// <summary>
/// 
/// </summary>
ViewPoint* Topic::ViewPointByGuid(const char* guid)
{
    return m_Viewpoints.FindByGuid(guid);
}

/// <summary>
/// 
/// </summary>
BCFComment* Topic::AddComment(const char* guid)
{
    auto comment = new Comment(*this, &m_Comments, guid ? guid : "");//"" forces generate guid

    if (comment) {
        m_Comments.Add(comment);
        return comment;
    }

    return NULL;
}

/// <summary>
/// 
/// </summary>
BCFComment* Topic::GetComment(uint16_t ind)
{
    return m_Comments.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFDocumentReference* Topic::AddDocumentReference(const char* path, bool isExternal, const char* guid)
{
    auto ref = new DocumentReference(*this, &m_DocumentReferences, guid ? guid : "");

    bool ok = ref->SetFilePath(path, isExternal);

    if (!ok){
        delete ref;
        ref = NULL;
    }

    if (ref) {
        m_DocumentReferences.Add(ref);
        return ref;
    }
    else {
        return NULL;
    }

}

/// <summary>
/// 
/// </summary>
BCFDocumentReference* Topic::GetDocumentReference(uint16_t ind)
{
    return m_DocumentReferences.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
BCFBimSnippet* Topic::GetBimSnippet(bool forceCreate)
{
    if (forceCreate && m_BimSnippets.Items().empty()) {
        auto snippet = new BimSnippet(*this, &m_BimSnippets);
        m_BimSnippets.Add(snippet);
    }

    if (m_BimSnippets.Items().empty()) {
        return NULL;
    }
    else {
        return m_BimSnippets.Items().front();
    }
}

/// <summary>
/// 
/// </summary>
bool Topic::AddReferenceLink(const char* val)
{
    if (val && *val) {
        return m_ReferenceLinks.Add(val);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
const char* Topic::GetReferenceLink(uint16_t ind)
{
    return m_ReferenceLinks.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
bool Topic::RemoveReferenceLink(const char* val)
{
    return m_ReferenceLinks.Remove(val);
}

/// <summary>
/// 
/// </summary>
bool Topic::AddLabel(const char* val)
{
    if (val && *val) {
        if (Project_().GetExtensions_().CheckElement(BCFTopicLabels, val)) {
            return m_Labels.Add(val);
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
const char* Topic::GetLabel(uint16_t ind)
{
    return m_Labels.GetAt(ind);
}

/// <summary>
/// 
/// </summary>
bool Topic::RemoveLabel(const char* val)
{
    return m_Labels.Remove(val);
}

/// <summary>
/// 
/// </summary>
bool Topic::AddRelatedTopic(BCFTopic* topic)
{
    if (topic) {
        auto guid = topic->GetGuid();
        if (guid) {
            if (!m_RelatedTopics.FindByGuid(guid)) {
                auto ref = new GuidReference(*this, &m_RelatedTopics);
                ref->SetGuid(guid);
                return m_RelatedTopics.Add(ref);
            }
        }
    }

    return false;
}


/// <summary>
/// 
/// </summary>
BCFTopic* Topic::GetRelatedTopic(uint16_t ind)
{
    auto guidRef = m_RelatedTopics.GetAt(ind);
    if (guidRef)
        return Project_().TopicByGuid(guidRef->GetGuid());
    else
        return NULL;
}

/// <summary>
/// 
/// </summary>
bool Topic::RemoveRelatedTopic(BCFTopic* topic)
{
    if (topic) {
        auto guid = topic->GetGuid();
        auto ref = m_RelatedTopics.FindByGuid(guid);
        if (ref) {
            return ref->Remove();
        }
        return true;
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::Remove()
{
    //remove references
    BCFTopic* topic = NULL;
    uint16_t ind = 0;
    while (NULL != (topic = Project_().GetTopic(ind++))) {

        if (topic != this) {

            BCFTopic* related = NULL;
            uint16_t ind2 = 0;
            while (NULL != (related = topic->GetRelatedTopic(ind2++))) {

                if (related == this) {
                    topic->RemoveRelatedTopic(this);
                    break;
                }
            }
        }
    }

    //
    return RemoveImpl();
}

/// <summary>
/// 
/// </summary>
BCFProject& Topic::GetProject()
{ 
    return Project_(); 
}

