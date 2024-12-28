#include "pch.h"
#include "ProjectInfo.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
ProjectInfo::ProjectInfo(BCFProject& project, const char* projectId)
    : XMLFile(project) 
    , m_ProjectId(project, projectId)
{
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(Project)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::Read_Project(_xml::_element& elem, const std::string& /*folder*/)
{
    ATTRS_START
        ATTR_GET(ProjectId)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Name)
    CHILDREN_END
}

