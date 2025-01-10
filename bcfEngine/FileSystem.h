#pragma once

class Log;

class FileSystem
{
public:
    struct DirElem
    {
        std::string name;   //name with extension without path
        bool        folder;
    };

    typedef std::list<DirElem> DirList;

public:
    static void AddPath(std::string& path, const char* name, bool zippath = false);

    static std::string GetFileName(const char* path, Log& log);

    //get directory list
    static bool GetDirContent(const char* folderPath, DirList& elems, Log& log);

    //check file or directory exists
    static bool Exists(const char* path);

    //
    static time_t GetFileModificationTime(const char* path, Log& log);

    //remove file of directory
    static bool Remove(const char* path, Log& log);

    // 
    static bool CopyFile(const char* src, const char* dst, Log& log);

    //
    static bool CreateTempDir(std::string& pathName, Log& log);

    //
    static bool CreateDir(const char* pathName, Log& log);
};

