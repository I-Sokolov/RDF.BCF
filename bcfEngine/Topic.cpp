#include "pch.h"
#include "Topic.h"
#include "BCFProject.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
Topic::Topic(BCFProject& project, const char* guid)
    : GUIDable(guid)
    , XMLFile(m_project.log())
    , m_project(project)
{

}

/// <summary>
/// 
/// </summary>
void Topic::GetRelativePathName(std::string& pathInBcfFolder)
{ 
    pathInBcfFolder.assign(GUID());
    FileSystem::AddPath (pathInBcfFolder, "markup.bcf"); 
}

/// <summary>
/// 
/// </summary>
void Topic::ReadRoot(_xml::_element& elem)
{
    GET_CHILD(Header)
    NEXT_CHILD(Topic)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Header(_xml::_element& elem)
{
    GET_CHILD(Files)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Files(_xml::_element& elem)
{
    GET_CHILD(File)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void Topic::Read_File(_xml::_element& elem)
{
    m_bimFiles.push_back(BIMFile(m_log));
    m_bimFiles.back().Read(elem);
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Topic(_xml::_element& elem)
{

}
