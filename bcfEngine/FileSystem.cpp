#include "pch.h"
#include "FileSystem.h"

#include <filesystem>
#include "Log.h"

#define OS_PATH_SEP  '\\'

/// <summary>
/// 
/// </summary>
void FileSystem::AddPath(std::string& path, const char* name, bool zippath)
{
    char sep = zippath ? '/' : OS_PATH_SEP;

    if (!path.empty() && path.back() != sep) {
        path.push_back(sep);
    }
    path.append(name);
}

/// <summary>
/// 
/// </summary>
void FileSystem::GetDirContent(const char* folderPath, DirList& elems)
{
    for (const auto& entry : std::filesystem::directory_iterator(folderPath)) {
        elems.push_back(DirElem());
        elems.back().name = entry.path().filename().string();
        elems.back().folder = entry.is_directory();
    }
}

/// <summary>
/// 
/// </summary>
bool FileSystem::Remove(const char* path)
{
    if (std::filesystem::exists(path)) {
        
        if (std::filesystem::is_directory(path)) {
            for (const auto& entry : std::filesystem::directory_iterator(path)) {
                Remove(entry.path().string().c_str());
            }
        }

        if (!std::filesystem::remove(path)) {
            m_log.error("Delete file error", "Can not delete %s", path);
            return false;
        }
    }

    return true;
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CreateTempDir(std::string& pathName)
{
    auto tmppath = std::filesystem::temp_directory_path();

    for (int ind = 0; ind < 1000; ind++) {
        char name[80];
        snprintf(name, 79, "RDF.BCF.%d", ind);
        
        auto tmpName(tmppath);
        tmpName.append(name);

        if (!std::filesystem::exists(tmpName)) {
            if (std::filesystem::create_directory(tmpName)) {
                pathName = tmpName.string();
                return true;
            }
            else {
                m_log.error("Create dir error", "Can't create folder %s", tmpName.string().c_str());
                return false;
            }
        }
    }    

    m_log.error("Create dir error", "Can't find free temporary name");
    return false;
}
