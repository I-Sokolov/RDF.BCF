#include "pch.h"
#include "BIMFile.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
void BIMFile::Read(_xml::_element& elem)
{
    ATTRS_START
        ATTR_GET(IsExternal)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Reference)
    CHILDREN_END
}
