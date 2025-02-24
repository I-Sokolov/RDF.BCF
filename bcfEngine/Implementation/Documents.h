#pragma once
#include "XMLFile.h"
#include "GuidStr.h"

class Documents : public XMLFile
{
public:
    Documents(Project& project);

    const char* GetFilePath(const char* guid);
    const char* Add(const char* filePath);

    bool Validate(bool fix);

private:
    //XMLFile implementation
    virtual const char* XMLFileName() override { return "documents.xml"; }
    virtual const char* XSDName() override { return "documents.xsd"; }
    virtual const char* RootElemName() override { return "DocumentInfo"; }
    virtual void ReadRoot(_xml::_element& elem, const std::string& folder) override;
    virtual void AfterRead(const std::string& folder) override;
    virtual void WriteRootContent(_xml_writer& writer, const std::string& folder) override;

    virtual Topic* Topic_() override { return NULL; }
    virtual Comment* Comment_() override { return NULL; }

    void MarkUsedDocuments();

private:
    struct Doc : public BCFObject
    {
    public:
        Doc(Documents& documents, ListOf<Doc>* list, const char* filePath = NULL);

        const char* GetGuid() { return m_Guid.c_str(); }
        const char* GetFilePath(bool create);

        void Read(_xml::_element& elem, const std::string& folder);
        void Write(_xml_writer& writer, const std::string& folder, const char* tag);
        bool Validate(bool fix);

        bool Remove() { return RemoveImpl(); }

        virtual Topic* Topic_() override { return NULL; }
        virtual Comment* Comment_() override { return NULL; }

    public:
        bool used;

    private:
        void GetReadWritePath(std::string& path, bool createFolder);
        void PrepareToWrite(const std::string& folder);

    private:
        GuidStr     m_Guid;
        std::string m_Filename;

        std::string m_readFolder;
        std::string m_filePath;
    };

private:
    SetByGuid<Doc>     m_Documents;
};

