#pragma once

#include "pch.h"
#include "BCFColor.h"
#include "BCFViewPoint.h"
#include "BCFComponent.h"

/// <summary>
/// 
/// </summary>
BCFColor::BCFColor(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project(), parentList) 
    , m_viewPoint(viewPoint)
    , m_Components(viewPoint.Project())
{ 
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFColor::ComponentAdd(const char* ifcGuid, const char* authoringToolId, const char* originatingSystem)
{
    return m_Components.Add(m_viewPoint, ifcGuid, authoringToolId, originatingSystem);
}

/// <summary>
/// 
/// </summary>
BCFComponent* BCFColor::ComponentIterate(BCFComponent* prev)
{
    return m_Components.GetNext(prev);
}


