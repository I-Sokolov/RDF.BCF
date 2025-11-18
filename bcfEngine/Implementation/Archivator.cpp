#include "pch.h"
#include "Archivator.h"

#include <filesystem>
#include "../kubazip/zip.h"
#include "Log.h"
#include "FileSystem.h"


/// <summary>
/// 
/// </summary>
bool Archivator::Pack(const char* folder, const char* archivePath)
{
    struct zip_t* zip = zip_open(archivePath, ZIP_DEFAULT_COMPRESSION_LEVEL, 'w');
    if (zip == NULL) {
        m_log.add(Log::Level::error, "Write file error", "Can not open to write archive %s", archivePath);
        return false;
    }

    auto ok = AddFolder(folder, "", zip);

	zip_close(zip);

    return ok;
}


/// <summary>
/// 
/// </summary>
bool Archivator::AddFolder(const char* osPath, const char* zipPath, struct zip_t* zip)
{
    FileSystem::DirList elems;
    if (!FileSystem::GetDirContent(osPath, elems, m_log)) {
        return false;
    }

    for (auto& elem : elems) {

        std::string ospath(osPath);
        FileSystem::AddPath(ospath, elem.name.c_str());

        std::string zippath(zipPath);
        FileSystem::AddPath(zippath, elem.name.c_str(), true);

        if (elem.folder) {
            AddFolder(ospath.c_str(), zippath.c_str(), zip);
        }
        else {
            if (zip_entry_open(zip, zippath.c_str()) < 0) {
                m_log.add(Log::Level::error, "Zip error", "Fail zip open file %s", zippath.c_str());
                return false;
			}
            if (zip_entry_fwrite(zip, ospath.c_str()) < 0) {
                m_log.add(Log::Level::error, "Zip error", "Fail zip write file %s", ospath.c_str());
                return false;
			}
            zip_entry_close(zip);
        }
    }

    return true;
}


/// <summary>
/// 
/// </summary>
bool Archivator::Unpack(const char* archivePath, const char* folder)
{
    struct zip_t* zip = zip_open(archivePath, 0, 'r');
    if (zip == NULL) {
        m_log.add(Log::Level::error, "File read", "Failed to open archive %s", archivePath);
        return false;
    }

    bool ok = true;

    size_t i, n = zip_entries_total(zip);
    for (i = 0; i < n; ++i) {
        zip_entry_openbyindex(zip, i);
        {
            const char* name = zip_entry_name(zip);
            if (zip_entry_isdir(zip))
                continue;

            StringList folderNames;
            std::string fileName;
            SplitZipPath(name, folderNames, fileName);

            std::string path(folder);
            if (!CreateFolders(path, folderNames)) {
                ok = false;
                break;
            }

            if (!fileName.empty()) {
                FileSystem::AddPath(path, fileName.c_str());

                zip_entry_fread(zip, path.c_str());
                zip_entry_close(zip);
            }
        }
        zip_entry_close(zip);
    } // for (i = ...
    zip_close(zip);

    return ok;
}

/// <summary>
/// 
/// </summary>
void Archivator::SplitZipPath(const std::string& zipPath, StringList& folders, std::string& file)
{
    const char delimiter = '/';

    size_t start = 0;
    size_t end = zipPath.find(delimiter);

    while (end != std::string::npos) {
        folders.push_back(zipPath.substr(start, end - start));
        start = end + 1;
        end = zipPath.find(delimiter, start);
    }

    file = zipPath.substr(start);
}

/// <summary>
/// 
/// </summary>
bool Archivator::CreateFolders(std::string& path, StringList& folders)
{
    for (auto& folder : folders) {
        FileSystem::AddPath(path, folder.c_str());
        if (!FileSystem::CreateDir(path.c_str(), m_log)) {
            return false;
        }
    }
    return true;
}


