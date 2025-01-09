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

    //get directory list
    static bool GetDirContent(const char* folderPath, DirList& elems, Log& log);

    //check file or directory exists
    static bool Exists(const char* path);

    //remove file of directory
    static bool Remove(const char* path, Log& log);

    //returns final path 
    static std::string CopyFile(const char* path, const char* targetDir, Log& log);

    //
    static bool CreateTempDir(std::string& pathName, Log& log);

    //
    static bool CreateDir(const char* pathName, Log& log);
};

