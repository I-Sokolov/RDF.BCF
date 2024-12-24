#include "pch.h"
#include "ProjectInfo.h"


/// <summary>
/// 
/// </summary>
void ProjectInfo::ReadRoot(_xml::_element& elem)
{
    GET_CHILD(Project)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::Read_Project(_xml::_element& elem)
{
    GET_ATTR(ProjectId)
    END_ATTR(UnknownNames::NotAllowed)

    GET_CHILD(Name)
    END_CHILDREN
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::Read_Name(_xml::_element& elem)
{
    m_Name = elem.getContent();
}
