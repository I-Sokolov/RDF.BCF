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
    static void GetDirContent(const char* folderPath, DirList& elems);

    //remove file of directory
    static bool Remove(const char* path, Log& log);

    //
    static bool CreateTempDir(std::string& pathName, Log& log);

    //
    static bool CreateDir(const char* pathName, Log& log);
};

