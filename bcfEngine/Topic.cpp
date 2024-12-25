#include "pch.h"
#include "Topic.h"
#include "BCFProject.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
Topic::Topic(BCFProject& project, const char* guid)
    : GUIDable(guid)
    , XMLFile(project.log())
    , m_project(project)
{

}

/// <summary>
/// 
/// </summary>
void Topic::GetRelativePathName(std::string& pathInBcfFolder)
{ 
    pathInBcfFolder.assign(Guid());
    FileSystem::AddPath (pathInBcfFolder, "markup.bcf"); 
}

/// <summary>
/// 
/// </summary>
void Topic::ReadRoot(_xml::_element& elem)
{
    CHILDREN_START
        CHILD_READ(Header)
        CHILD_READ(Topic)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Header(_xml::_element& elem)
{
    CHILDREN_START
        CHILD_GET_LIST(File)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Topic(_xml::_element& elem)
{
    ATTRS_START
        ATTR_GET(Guid)
        ATTR_GET(ServerAssignedId)
        ATTR_GET(TopicStatus)
        ATTR_GET(TopicType)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Title)
        CHILD_GET_LIST(ReferenceLink)
        CHILD_GET_LIST(Label)
        CHILD_GET_CONTENT(CreationDate)
        CHILD_GET_CONTENT(CreationAuthor)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(AssignedTo)
        CHILD_GET_LIST(DocumentReference)
    CHILDREN_END
}
