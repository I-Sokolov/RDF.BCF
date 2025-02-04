#include "pch.h"
#include "Archivator.h"

#include <filesystem>
#include <zip.h>
#include "Log.h"
#include "FileSystem.h"


/// <summary>
/// 
/// </summary>
bool Archivator::Pack(const char* folder, const char* archivePath)
{
    zip_t* zip = zip_open(archivePath, ZIP_CREATE | ZIP_TRUNCATE, nullptr);
    if (!zip) {
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
bool Archivator::AddFolder(const char* osPath, const char* zipPath, struct zip* zip)
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
            zip_source_t* source = zip_source_file(zip, ospath.c_str(), 0, 0);
            if (source) {
                if (0 > zip_file_add(zip, zippath.c_str(), source, ZIP_FL_ENC_GUESS)) {
                    m_log.add(Log::Level::error, "Zip error", "Fail zip add file %s", ospath.c_str());
                    return false;
                }
            }
            else {
                m_log.add(Log::Level::error, "Zip error", "Fail zip source file %s", ospath.c_str());
                return false;
            }
        }
    }

    return true;
}


/// <summary>
/// 
/// </summary>
bool Archivator::Unpack(const char* archivePath, const char* folder)
{
    struct zip* archive = zip_open(archivePath, 0, NULL);
    if (archive == NULL) {
        m_log.add(Log::Level::error, "File read",  "Failed to open archive %s", archivePath);
        return false;
    }

    bool ok = true;

    int numFiles = zip_get_num_files(archive);

    for (int i = 0; i < numFiles && ok; ++i) {
        struct zip_file* file = zip_fopen_index(archive, i, 0);
        if (!file)
            continue;

        struct zip_stat fileStat;
        zip_stat_init(&fileStat);
        zip_stat_index(archive, i, 0, &fileStat);

        StringList folderNames;
        std::string fileName;
        SplitZipPath(fileStat.name, folderNames, fileName);

        std::string path(folder);
        if (!CreateFolders(path, folderNames)) {
            ok = false;
            break;
        }

        if (!fileName.empty()) {
            FileSystem::AddPath(path, fileName.c_str());

            FILE* outFile = fopen(path.c_str(), "wb");
            if (!outFile) {
                m_log.add(Log::Level::error, "File write error", "Can not open to write archive %s", path.c_str());
                ok = false;
                break;
            }

            if (fileStat.size > 0) {
                std::vector<char> buffer(fileStat.size);
                zip_fread(file, &buffer[0], fileStat.size);

                fwrite(&buffer[0], 1, fileStat.size, outFile);
            }

            fclose(outFile);
        }
        
        zip_fclose(file);
    }

    zip_close(archive);

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


