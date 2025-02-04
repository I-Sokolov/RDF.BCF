#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"

struct Topic;

struct File : public BCFObject, public BCFFile
{
public:
    File(Topic& topic, ListOfBCFObjects* parentList);

public:
    //BCFFile
    virtual bool        GetIsExternal                   () override { return StrToBool (m_IsExternal); }
    virtual const char* GetFilename                     () override { return m_Filename.c_str(); }
    virtual const char* GetDate                         () override { return m_Date.c_str(); }
    virtual const char* GetReference                    () override { return m_Reference.c_str(); }
    virtual const char* GetIfcProject                   () override { return m_IfcProject.c_str(); }
    virtual const char* GetIfcSpatialStructureElement   () override { return m_IfcSpatialStructureElement.c_str(); }

    virtual bool SetIsExternal                   (bool        val) override { return BoolToStr(val, m_IsExternal);}
    virtual bool SetFilename                     (const char* val) override { UNNULL; m_Filename.assign(val); return true;}
    virtual bool SetDate                         (const char* val) override { UNNULL; VALIDATE(Date, DateTime); m_Date.assign(val); return true;}
    virtual bool SetReference                    (const char* val) override;
    virtual bool SetIfcProject                   (const char* val) override { UNNULL; VALIDATE(IfcProject, IfcGuid); m_IfcProject.assign(val); return true;}
    virtual bool SetIfcSpatialStructureElement   (const char* val) override { UNNULL; VALIDATE(m_IfcSpatialStructureElement, IfcGuid); m_IfcSpatialStructureElement.assign(val); return true;}

    virtual bool Remove() override { return RemoveImpl(); }

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

private:
    void Write_File(_xml_writer& writer, const std::string& folder);

private:
    void UpdateFileInfo();

private:
    Topic&      m_topic;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
    std::string m_IfcProject;
    std::string m_IfcSpatialStructureElement;
};

