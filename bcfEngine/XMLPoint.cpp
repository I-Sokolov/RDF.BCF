#include "pch.h"
#include "XMLPoint.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void XMLPoint::Read(_xml::_element& elem, const std::string&)
{
    CHILDREN_START
        CHILD_GET_CONTENT(X)
        CHILD_GET_CONTENT(Y)
        CHILD_GET_CONTENT(Z)
    CHILDREN_END
}