#include "pch.h"
#include "BimFile.h"
#include "XMLFile.h"
#include "Project.h"
#include "Topic.h"
#include "FileSystem.h"
#ifndef __EMSCRIPTEN__
#include "ifcengine.h"
#else
#include "../include/ifcengine.h"
#endif

/// <summary>
/// 
/// </summary>
BimFile::BimFile(Topic& topic, ListOfBCFObjects* parentList)
    : BCFObject(topic.Project_(), parentList)
    , m_topic(topic)
    , m_IsExternal("true")
    , m_pIfcGuids(NULL)
{
}

/// <summary>
/// 
/// </summary>
BimFile::~BimFile()
{
    ClearContent();
}

/// <summary>
/// 
/// </summary>
void BimFile::ClearContent()
{
    if (m_pIfcGuids) {
        delete m_pIfcGuids;
    }
    m_pIfcGuids = NULL;
}

/// <summary>
/// 
/// </summary>
void BimFile::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(IsExternal)
        ATTR_GET(IfcProject)
        ATTR_GET(IfcSpatialStructureElement)

        ATTR_GET_STR(isExternal, m_IsExternal) //v2.1
    
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Filename)
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Reference)
    CHILDREN_END

    //
    if (!GetIsExternal()) {
        m_Reference = AbsolutePath(m_Reference, folder);
    }
}

/// <summary>
/// 
/// </summary>
bool BimFile::Validate(bool)
{
    return true;
}

/// <summary>
/// 
/// </summary>
void BimFile::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    if (!GetIsExternal()) {
        m_Reference = CopyToRelative(m_Reference, folder, "..");
    }

    XMLFile::Attributes attr;
    ATTR_ADD(IsExternal);
    ATTR_ADD(IfcProject);
    ATTR_ADD(IfcSpatialStructureElement);

    WRITE_ELEM(File);

    if (!GetIsExternal()) {
        m_Reference = AbsolutePath(m_Reference, folder);
    }
}

/// <summary>
/// 
/// </summary>
void BimFile::Write_File(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Filename);
    WRITE_CONTENT(Date);
    WRITE_CONTENT(Reference);
}

/// <summary>
/// 
/// </summary>
bool BimFile::SetReference(const char* val)
{ 
    VALIDATE(Reference, FilePath);

    if (0!=strcmp(val, m_Reference.c_str())) {

        ClearContent();

        SetPropertyString(val, m_Reference);

        if (!m_Reference.empty()) {
            return UpdateFileInfo();
        }
    }

    return true; 
}

/// <summary>
/// 
/// </summary>
bool BimFile::UpdateFileInfo()
{
    if (!m_Reference.empty()) {

        m_Filename = FileSystem::GetFileName(m_Reference.c_str(), Log_());

        if (FileSystem::Exists(m_Reference.c_str())) {

            if (IsIFC()) {
                return UpdateFileInfoIFC();
            }
            else {
                auto tm = FileSystem::GetFileModificationTime(m_Reference.c_str(), Log_());
                if (tm) {
                    m_Date = TimeToStr(tm);
                }
            }
        }
    }

    return true;
}

/// <summary>
/// 
/// </summary>
bool BimFile::IsIFC()
{
    auto len = m_Reference.length();
    if (len > 4) {
        auto ext = m_Reference.c_str() + len - 4;
#ifdef _WINDOWS
        if (0 == _stricmp(".ifc", ext)) {
#else
		if (0 == strcasecmp(".ifc", ext)) {
#endif
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BimFile::UpdateFileInfoIFC()
{
    auto model = sdaiOpenModelBN(0, m_Reference.c_str(), NULL);
    if (model) {

        engiSetStringEncoding(model, enum_string_encoding::UTF8);

        const char* value = NULL;
        GetSPFFHeaderItem(model, 3, 0, sdaiSTRING, &value);
        if (value && *value){
            m_Date.assign(value);
        }

        value = NULL;
        GetSPFFHeaderItem(model, 2, 0, sdaiSTRING, &value);
        if (value && *value){
            m_Filename.assign(value);
        }

        sdaiCloseModel(model);
    }
    return true;
}

/// <summary>
/// 
/// </summary>
bool BimFile::HasComponent(const std::string& ifcGuid)
{
    if (!IsIFC()) {
        return false;
    }

    if (!m_pIfcGuids) {
        m_pIfcGuids = new StrSet();

        auto model = sdaiOpenModelBN(0, m_Reference.c_str(), "");
        if (model) {

            auto root = sdaiGetEntity(model, "IfcRoot");
            auto attr = sdaiGetAttrDefinition(root, "GlobalId");

            auto extent = xxxxGetEntityAndSubTypesExtentBN(model, "IfcRoot");
            auto it = sdaiCreateIterator(extent);
            while (sdaiNext(it)) {

                auto inst = sdaiGetAggrByIterator(it);

                const char* guid = NULL;
                if (sdaiGetAttr(inst, attr, sdaiSTRING, &guid) && guid && *guid) {

                    m_pIfcGuids->insert(guid);
                }
            }
            sdaiDeleteIterator(it);
            sdaiCloseModel(model);
        }
    }

    return m_pIfcGuids->find(ifcGuid) != m_pIfcGuids->end();
}

BCFTopic& BimFile::GetTopic()
{
    return m_topic;
}
