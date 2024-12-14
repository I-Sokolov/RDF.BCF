#include "pch.h"

#include <zip.h>

#include "Log.h"

#include "Archivator.h"


/// <summary>
/// 
/// </summary>
bool Archivator::Pack(const char* folder, const char* archivePath)
{
    zip_t* zip = zip_open(archivePath, ZIP_CREATE | ZIP_TRUNCATE, nullptr);
    if (!zip) {
        m_log.error("File write fail", "Can not open to write archive %s", archivePath);
        return false;
    }

    auto ok = AddFolder(folder, "", zip);
    
    zip_close(zip);
    
    return ok;
}


/// <summary>
/// 
/// </summary>
bool Archivator::AddFolder(const std::filesystem::path& folderPath, std::string inzipPath, struct zip* zip)
{
    for (const auto& entry : std::filesystem::directory_iterator(folderPath)) {

        std::string entryPath(inzipPath);
        entryPath.push_back('/');
        entryPath.append(entry.path().filename().string());

        if (entry.is_directory()) {
           
            AddFolder(entry.path(), entryPath, zip);
        }
        else {
            auto filePath = entry.path().string().c_str();
            zip_source_t* source = zip_source_file(zip, filePath, 0, 0);
            if (source) {
                if (0 > zip_file_add(zip, entryPath.c_str(), source, ZIP_FL_ENC_GUESS)) {
                    m_log.error("Fail add to zip", "Fail zip add file %s", filePath);
                    return false;
                }
            }
            else {
                m_log.error("Fail zip file", "Fail zip source file %s", filePath);
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
        m_log.error ("File read fail",  "Failed to open archive %s", archivePath);
        return false;
    }

    int numFiles = zip_get_num_files(archive);

    for (int i = 0; i < numFiles; ++i) {
        struct zip_file* file = zip_fopen_index(archive, i, 0);
        if (!file)
            continue;

        struct zip_stat fileStat;
        zip_stat_init(&fileStat);
        zip_stat_index(archive, i, 0, &fileStat);

        std::filesystem::path filePath (folder);
        filePath.append (fileStat.name);

        FILE* outFile = fopen(filePath.string().c_str(), "wb");
        if (!outFile) {
            m_log.error("File write fail", "Can not open to write archive %s", filePath.string().c_str());
            return false;
        }

        std::vector<char> buffer(fileStat.size);
        zip_fread(file, &buffer[0], fileStat.size);

        fwrite(&buffer[0], 1, fileStat.size, outFile);

        fclose(outFile);
        zip_fclose(file);
    }

    zip_close(archive);

    return true;
}

