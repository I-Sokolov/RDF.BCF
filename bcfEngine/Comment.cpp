#include "pch.h"
#include "Comment.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void Comment::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Author)
        CHILD_GET_CONTENT(Comment)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
        CHILD_GET_LIST(Viewpoint)
    CHILDREN_END
}
