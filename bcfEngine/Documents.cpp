#include "pch.h"
#include "Documents.h"
#include "GuidStr.h"
#include "FileSystem.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
Documents::Documents(BCFProject& project)
    : XMLFile (project, NULL)
    , m_Documents(project)
{
}

/// <summary>
/// 
/// </summary>
void Documents::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Documents, Document)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Documents::Doc::Read(_xml::_element& elem, const std::string& folder)
{
    m_readFolder.assign(folder);

    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed);

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Documents::AfterRead(const std::string&)
{
}

/// <summary>
/// 
/// </summary>
bool Documents::Validate(bool fix)
{
    return m_Documents.Validate(fix);
}

/// <summary>
/// 
/// </summary>
void Documents::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    WRITE_LIST(Document);
}

/// <summary>
/// 
/// </summary>
void Documents::Doc::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
    PrepareToWrite(folder);

    Attributes attr;
    ATTR_ADD(Guid);
    
    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_CONTENT(Filename);
}

/// <summary>
/// 
/// </summary>
const char* Documents::GetFilePath(const char* guid)
{
    auto doc = m_Documents.FindByGuid(guid);
    
    if (doc) {
        return doc->GetFilePath(true);
    }
    else {
        log().add(Log::Level::error, "Invalid document guid");
        return "";
    }
}

/// <summary>
/// 
/// </summary>
Documents::Doc::Doc(Documents& documents, ListOf<Doc>* list, const char* filePath)
    : BCFObject(documents.Project(), list)
    , m_Guid(documents.Project(), filePath ? "" : NULL)
{
    if (filePath && *filePath) {
        m_filePath = filePath;
        m_Filename = FileSystem::GetFileName(filePath, log());
    }
}


/// <summary>
/// 
/// </summary>
void Documents::Doc::GetReadWritePath(std::string& path, bool createFolder)
{
    path.assign(m_readFolder);
    FileSystem::AddPath(path, "Documents");

    if (createFolder) {
        if (!FileSystem::CreateDir(path.c_str(), log())) {
            throw std::exception();
        }
    }

    FileSystem::AddPath(path, m_Guid.c_str());
}

/// <summary>
/// 
/// </summary>
const char* Documents::Doc::GetFilePath(bool create)
{
    if (create && m_filePath.empty()) {
        assert(!m_readFolder.empty());

        std::string src_path;
        GetReadWritePath(src_path, false);

        std::string dst_path(m_readFolder);

        FileSystem::AddPath(dst_path, "Documents_");
        if (!FileSystem::CreateDir(dst_path.c_str(), log())) {
            return "";
        }

        FileSystem::AddPath(dst_path, m_Guid.c_str());
        if (!FileSystem::CreateDir(dst_path.c_str(), log())) {
            return "";
        }

        FileSystem::AddPath(dst_path, m_Filename.c_str());

        if (!FileSystem::MoveFile(src_path.c_str(), dst_path.c_str(), log())) {
            return "";
        }

        std::swap(m_filePath, dst_path);
    }

    return m_filePath.c_str();
}

/// <summary>
/// 
/// </summary>
void Documents::Doc::PrepareToWrite(const std::string& folder)
{
    std::string src_name(m_filePath);
    if (src_name.empty()) {
        GetReadWritePath(src_name, false);
    }

    m_filePath.clear();
    m_readFolder.assign(folder);

    std::string dst_name;
    GetReadWritePath(dst_name, true);

    if (!FileSystem::CopyFile(src_name.c_str(), dst_name.c_str(), log())) {
        throw std::exception();
    }
}

/// <summary>
/// 
/// </summary>
bool Documents::Doc::Validate(bool)
{
    return true;
}


/// <summary>
/// 
/// </summary>
const char* Documents::Add(const char* filePath)
{
    if (!filePath || !FileSystem::Exists(filePath)) {
        log().add(Log::Level::error, "File does not exist", "%s", filePath ? filePath : "<null>");
        return "";
    }

    //search existing
    for (auto& doc : m_Documents.Items()) {
        auto docFilePath = doc->GetFilePath(false);
        if (docFilePath && *docFilePath) {
            if (0 == strcmp(filePath, docFilePath)) {
                return doc->GetGuid();
            }
        }
    }

    //add new
    auto doc = new Doc(*this, &m_Documents, filePath);
    m_Documents.Add(doc);

    return doc->GetGuid();
}

