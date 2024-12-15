#include "pch.h"
#include "Archivator.h"

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
        m_log.error("Write file error", "Can not open to write archive %s", archivePath);
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
    FileSystem fs(m_log);

    FileSystem::DirList elems;
    fs.GetDirContent(osPath, elems);

    for (auto& elem : elems) {

        std::string ospath(osPath);
        fs.AddPath(ospath, elem.name.c_str());

        std::string zippath(zipPath);
        fs.AddPath(zippath, elem.name.c_str(), true);

        if (elem.folder) {
            AddFolder(ospath.c_str(), zippath.c_str(), zip);
        }
        else {
            zip_source_t* source = zip_source_file(zip, ospath.c_str(), 0, 0);
            if (source) {
                if (0 > zip_file_add(zip, zippath.c_str(), source, ZIP_FL_ENC_GUESS)) {
                    m_log.error("Zip error", "Fail zip add file %s", ospath.c_str());
                    return false;
                }
            }
            else {
                m_log.error("Zip error", "Fail zip source file %s", ospath.c_str());
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
#if 0
    struct zip* archive = zip_open(archivePath, 0, NULL);
    if (archive == NULL) {
        m_log.error ("File read error",  "Failed to open archive %s", archivePath);
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
            m_log.error("File write error", "Can not open to write archive %s", filePath.string().c_str());
            return false;
        }

        std::vector<char> buffer(fileStat.size);
        zip_fread(file, &buffer[0], fileStat.size);

        fwrite(&buffer[0], 1, fileStat.size, outFile);

        fclose(outFile);
        zip_fclose(file);
    }

    zip_close(archive);
#endif
    return true;
}

