#include "pch.h"
#include "ProjectInfo.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
ProjectInfo::ProjectInfo(Project& project, const char* projectId)
    : XMLFile(project, NULL)
    , m_ProjectId(projectId ? projectId : "")
{
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(Project)
        CHILD_READ_FUNC(ExtensionSchema, Project_().GetExtensions_().ReadExtensionSchema)
    CHILDREN_END;
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
    CHILDREN_END;
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::AfterRead(const std::string&)
{
    if (Project_().GetVersion() < BCFVer_3_0) {
        if (m_ProjectId.empty()) {
            m_ProjectId = GuidStr::New();
        }
    }
}

/// <summary>
/// 
/// </summary>
bool ProjectInfo::Validate(bool)
{
    return true;
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    if (m_ProjectId.empty()) {
        //GetLog().add(Log::Level::warning, "Missed value fixed", "ProjectId was not set and was generated");
        m_ProjectId = GuidStr::New();
    }

    Attributes attr;
    ATTR_ADD(ProjectId);

    WRITE_ELEM(Project);
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::Write_Project(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Name);
}

