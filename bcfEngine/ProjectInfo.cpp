#include "pch.h"
#include "ProjectInfo.h"


/// <summary>
/// 
/// </summary>
void ProjectInfo::ReadRoot(_xml::_element& elem)
{
    CHILDREN_START
        CHILD_READ(Project)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::Read_Project(_xml::_element& elem)
{
    ATTRS_START
        ATTR_GET(ProjectId)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Name)
    CHILDREN_END
}

