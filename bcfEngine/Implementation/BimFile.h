#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"

struct Topic;

struct BimFile : public BCFObject, public BCFBimFile
{
public:
    BimFile(Topic& topic, ListOfBCFObjects* parentList);
    ~BimFile();

public:
    //BCFFile
    virtual bool        GetIsExternal                   () override { return GetPropertyBool (m_IsExternal); }
    virtual const char* GetFilename                     () override { return m_Filename.c_str(); }
    virtual const char* GetDate                         () override { return m_Date.c_str(); }
    virtual const char* GetReference                    () override { return m_Reference.c_str(); }
    virtual const char* GetIfcProject                   () override { return m_IfcProject.c_str(); }
    virtual const char* GetIfcSpatialStructureElement   () override { return m_IfcSpatialStructureElement.c_str(); }

    virtual bool SetIsExternal                   (bool        val) override { return SetPropertyBool(val, m_IsExternal);}
    virtual bool SetFilename                     (const char* val) override { return SetPropertyString(val, m_Filename);}
    virtual bool SetDate                         (const char* val) override { VALIDATE(Date, DateTime); return SetPropertyString(val, m_Date);}
    virtual bool SetReference                    (const char* val) override;
    virtual bool SetIfcProject                   (const char* val) override { VALIDATE(IfcProject, IfcGuid); return SetPropertyString(val, m_IfcProject);}
    virtual bool SetIfcSpatialStructureElement   (const char* val) override { VALIDATE(m_IfcSpatialStructureElement, IfcGuid); return SetPropertyString(val, m_IfcSpatialStructureElement);}

    virtual BCFTopic& GetTopic() override;
    virtual bool Remove() override { return RemoveImpl(); }

    virtual Topic* Topic_() override { return &m_topic; }
    virtual Comment* Comment_() override { return NULL; }

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);
    bool HasComponent(const std::string& ifcGuid);

private:
    typedef std::unordered_set<std::string> StrSet;

private:
    void ClearContent();
    void Write_File(_xml_writer& writer, const std::string& folder);

private:
    bool UpdateFileInfo();
    bool IsIFC();
    bool UpdateFileInfoIFC();

private:
    Topic&      m_topic;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
    std::string m_IfcProject;
    std::string m_IfcSpatialStructureElement;

    StrSet*     m_pIfcGuids;
};

