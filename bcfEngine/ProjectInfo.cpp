#include "pch.h"
#include "ProjectInfo.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
ProjectInfo::ProjectInfo(BCFProject& project, const char* projectId)
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
        CHILD_READ_FUNC(ExtensionSchema, Project().GetExtensions().ReadExtensionSchema)
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
void ProjectInfo::UpgradeReadVersion()
{
    if (Project().GetVersion() < BCFVer_3_0) {
        if (m_ProjectId.empty()) {
            m_ProjectId = GuidStr::New();
        }
    }
}

/// <summary>
/// 
/// </summary>
void ProjectInfo::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    if (m_ProjectId.empty()) {
        log().add(Log::Level::warning, "Invalid value", "ProjectId must be set");
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
