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
bool FileSystem::GetDirContent(const char* folderPath, DirList& elems, Log& log)
{
    try {
        for (const auto& entry : std::filesystem::directory_iterator(folderPath)) {
            elems.push_back(DirElem());
            elems.back().name = entry.path().filename().string();
            elems.back().folder = entry.is_directory();
        }
        return true;
    }
    catch (std::exception& expt) {
        log.add(Log::Level::error, "Directory list", "Failed to list directory %s: %s", folderPath, expt.what());
        return false;
    }
    catch (...) {
        log.add(Log::Level::error, "Directory list", "Failed to list directory %s: unknown error", folderPath);
        return false;
    }
}

/// <summary>
/// 
/// </summary>
std::string FileSystem::GetFileName(const char* path, Log& log)
{
    try {
        return filesystem::path(path).filename().string();
    }
    catch (std::exception& expt) {
        log.add(Log::Level::error, "File system", "Can not get filename of '%s': %s", path, expt.what());
        return "";
    }
}

/// <summary>
/// 
/// </summary>
bool FileSystem::Exists(const char* path)
{
    return std::filesystem::exists(path);
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CopyFile(const char* src, const char* dst, Log& log)
{
    try {

        if (!std::filesystem::copy_file(src, dst)) {
            log.add(Log::Level::error, "File copy", "Can not copy file '%s' to '%s'", src, dst);
            return false;
        }

        return true;
    }
    catch (std::exception& expt) {
        log.add(Log::Level::error, "File copy", "Can not copy '%s' to '%s': %s", src, dst, expt.what());
        return false;
    }
}

/// <summary>
/// 
/// </summary>
bool FileSystem::Remove(const char* path, Log& log)
{
    try {
        if (std::filesystem::exists(path)) {

            if (std::filesystem::is_directory(path)) {
                for (const auto& entry : std::filesystem::directory_iterator(path)) {
                    if (!Remove(entry.path().string().c_str(), log)) {
                        return false;
                    }
                }
            }

            if (!std::filesystem::remove(path)) {
                log.add(Log::Level::error, "File delete", "Can not delete %s", path);
                return false;
            }
        }

        return true;
    }
    catch (std::exception& expt) {
        log.add(Log::Level::error, "File delete", "Can not delete %s: %s", path, expt.what());
        return false;
    }
    catch (...) {
        log.add(Log::Level::error, "File delete", "Can not delete %s: unknown error", path);
        return false;
    }
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CreateTempDir(std::string& pathName, Log& log)
{
    try {
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
    catch (std::exception& expt) {
        log.add(Log::Level::error, "File write", "Can not create a temporary folder: %s", expt.what());
        return false;
    }
    catch (...) {
        log.add(Log::Level::error, "File write", "Can not create a temporary folder: unknown error");
        return false;
    }
}

/// <summary>
/// 
/// </summary>
bool FileSystem::CreateDir(const char* pathName, Log& log)
{
    try {
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
    catch (std::exception& expt) {
        log.add(Log::Level::error, "File write", "Can not create folder %s: %s", pathName, expt.what());
        return false;
    }
    catch (...) {
        log.add(Log::Level::error, "File write", "Can not create folder %s: unknown error", pathName);
        return false;
    }
}

/// <summary>
/// 
/// </summary>
time_t FileSystem::GetFileModificationTime(const char* path, Log& log)
{
    if (filesystem::exists(path)) {
        struct stat result;
        if (stat(path, &result) == 0) {
            return result.st_mtime;
        }
    }
    log.add(Log::Level::error, "File read", "Can not get modification time for file '%s'", path);
    return 0;
}
