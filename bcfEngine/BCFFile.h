#pragma once

#include "BCFObject.h"
struct BCFTopic;

struct BCFFile : public BCFObject
{
public:
    BCFFile(BCFTopic& topic, ListOfBCFObjects* parentList);

public:
    bool        GetIsExternal                   () { return StrToBool (m_IsExternal); }
    const char* GetFilename                     () { return m_Filename.c_str(); }
    const char* GetDate                         () { return m_Date.c_str(); }
    const char* GetReference                    () { return m_Reference.c_str(); }
    const char* GetIfcProject                   () { return m_IfcProject.c_str(); }
    const char* GetIfcSpatialStructureElement   () { return m_IfcSpatialStructureElement.c_str(); }

    bool SetIsExternal                   (bool        val) { return BoolToStr(val, m_IsExternal);}
    bool SetFilename                     (const char* val) { UNNULL; m_Filename.assign(val); return true;}
    bool SetDate                         (const char* val) { UNNULL; VALIDATE(Date, DateTime); m_Date.assign(val); return true;}
    bool SetReference                    (const char* val);
    bool SetIfcProject                   (const char* val) { UNNULL; VALIDATE(IfcProject, IfcGuid); m_IfcProject.assign(val); return true;}
    bool SetIfcSpatialStructureElement   (const char* val) { UNNULL; VALIDATE(m_IfcSpatialStructureElement, IfcGuid); m_IfcSpatialStructureElement.assign(val); return true;}

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

private:
    void Write_File(_xml_writer& writer, const std::string& folder);

private:
    void UpdateFileInfo();

private:
    BCFTopic& m_topic;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
    std::string m_IfcProject;
    std::string m_IfcSpatialStructureElement;
};

