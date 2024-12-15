#pragma once

class Log;

class Archivator
{
public:
    Archivator(Log& log) : m_log(log) {}

public:
    bool Pack(const char* folder, const char* archivePath);
    bool Unpack(const char* archivePath, const char* folder);

private:
    bool AddFolder(const char* osPath, const char* zipPath, struct zip* zip);

private:
    Log& m_log;
};

