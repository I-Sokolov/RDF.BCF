#include "pch.h"
#include "Topic.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
Topic::Topic(BCFProject& project, const char* guid)
    : GUIDable(guid)
    , m_project(project)
{

}