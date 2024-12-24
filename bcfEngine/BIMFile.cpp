#include "pch.h"
#include "BIMFile.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
void BIMFile::Read(_xml::_element& elem)
{
    GET_ATTR(IsExternal)
    END_ATTR(UnknownNames::NotAllowed)

    GET_CHILD_CONTENT(Filename)
    NEXT_CHILD_CONTENT(Date)
    NEXT_CHILD_CONTENT(Reference)
    END_CHILDREN
}
