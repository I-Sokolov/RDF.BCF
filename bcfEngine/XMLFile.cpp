#include "pch.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "XMLFile.h"
#include "FileSystem.h"
#include "SmokeTest.h"

/// <summary>
/// 
/// </summary>
bool XMLFile::ReadFile(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, XMLFileName());

    if (!FileSystem::Exists(path.c_str())) {
        return true; 
    }

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root, bcfFolder);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_project.log().add(Log::Level::error, "Read file error", "Failed to read %s file. %s", path.c_str(), ex.what());
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool XMLFile::WriteFile(const std::string& bcfFolder)
{
    bool ok = false;

    const char* xmlname = XMLFileName();
    assert(xmlname && *xmlname);

    std::string xmlpath(bcfFolder);
    FileSystem::AddPath(xmlpath, xmlname);

    try {
        _xml_writer xmlWriter(xmlpath.c_str());

        xmlWriter.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlWriter.writeComment("RDF BCF engine build " __DATE__ ". http://rdf.bg.");

        Attributes attr;
        attr.Add("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        attr.Add("xsi:schemaLocation", XSDName());

        WriteRootElem(xmlWriter, bcfFolder, attr);

        ok = true;
    }
    catch (std::exception& ex) {
        m_project.log().add(Log::Level::error, "Write file error", "Failed to write %s file. %s", xmlpath.c_str(), ex.what());
    }

#ifdef SMOKE_TEST
    if (ok) {
        SmokeTest_ValidateXSD(XSDName(), xmlpath.c_str());
    }
#endif

    return ok;
}

/// <summary>
/// 
/// </summary>
void XMLFile::WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr)
{
    const char* rootName = RootElemName();
    
    writer.writeStartTag(rootName, attr);
    writer.indent()++;

    WriteRootContent(writer, folder);

    writer.indent()--;
    writer.writeEndTag(rootName);
}


/// <summary>
/// 
/// </summary>
XMLText::XMLText(BCFTopic& topic, ListOfBCFObjects* parentList)
    : BCFObject(topic.Project(), parentList)
{
}

/// <summary>
/// 
/// </summary>
void XMLText::Read(_xml::_element& elem, const std::string&) 
{ 
    m_str.assign(elem.getContent()); 
}

/// <summary>
/// 
/// </summary>
void XMLText::Write(_xml_writer& writer, const std::string&, const char* tag)
{ 
    writer.writeTag(tag, m_str); 
}
