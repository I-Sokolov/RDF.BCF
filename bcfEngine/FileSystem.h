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
    FileSystem (Log& log) : m_log(log) {}

public:
    void AddPath(std::string& path, const char* name, bool zippath = false);

    //get directory list
    void GetDirContent(const char* folderPath, DirList& elems);

    //remove file of directory
    bool Remove(const char* path);

    //
    bool CreateTempDir(std::string& pathName);

private:
    Log& m_log;
};

