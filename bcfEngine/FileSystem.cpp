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
bool FileSystem::Remove(const char* path, Log& log)
{
    if (std::filesystem::exists(path)) {
        
        if (std::filesystem::is_directory(path)) {
            for (const auto& entry : std::filesystem::directory_iterator(path)) {
                Remove(entry.path().string().c_str(), log);
            }
        }

        if (!std::filesystem::remove(path)) {
            log.add(Log::Level::error, "File delete", "Can not delete %s", path);
            return false;
        }
    }

    return true;
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CreateTempDir(std::string& pathName, Log& log)
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
                log.add(Log::Level::error, "File write", "Can't create folder %s", tmpName.string().c_str());
                return false;
            }
        }
    }    

    log.add(Log::Level::error, "File write", "Can't find free temporary name");
    return false;
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CreateDir(const char* pathName, Log& log)
{
    if (std::filesystem::exists(pathName)) {
        if (!std::filesystem::is_directory(pathName)) {
            log.add(Log::Level::error, "File write", "Path exist, expected folder but it is a file: %s", pathName);
            return false;
        }
    }
    else if (!std::filesystem::create_directory(pathName)) {
        log.add(Log::Level::error, "File write", "Can't create folder %s", pathName);
        return false;
    }
    return true;
}