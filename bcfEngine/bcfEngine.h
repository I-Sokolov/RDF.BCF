

#ifndef __RDF_LTF_BCF_ENGINE_H
#define __RDF_LTF_BCF_ENGINE_H


#ifndef RDFBCF_EXPORT
    #ifdef _WINDOWS
        #define RDFBCF_EXPORT __declspec(dllimport) 
    #else
        #define RDFBCF_EXPORT  
    #endif
#endif //RDFBCF_EXPORT

#ifdef __cplusplus
extern "C" {
#endif

    /// <summary>
    /// 
    /// </summary>
    struct BCFProject;

    /// <summary>
    /// 
    /// </summary>
    enum BCFVersion
    {
        BCFVerNotSupported = 0,
        BCFVer_2_1 = 21,
        BCFVer_3_0 = 30
    };

    /// <summary>
    /// 
    /// </summary>
    enum BCFEnumeration
    {
        BCFTopicTypes       = 1,
        BCFTopicStatuses    = 2,
        BCFPriorities       = 3,
        BCFTopicLabels      = 4,
        BCFUsers            = 5,
        BCFSnippetTypes     = 6,
        BCFStages           = 7
    };

    /// <summary>
    /// 
    /// </summary>
    typedef uint16_t BCFIndex;

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFProject* bcfCreateProject(const char* currentUser, bool autoExtent, const char* projectId);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT void bcfDeleteProject(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project, bool cleanLog);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfReadFile(BCFProject* project, const char* bcfFilePath);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfWriteFile(BCFProject* project, const char* bcfFilePath, BCFVersion version);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetProjectId(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetProjectName(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfSetProjectName(BCFProject* project, const char* name);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetEnumerationElement(BCFProject* project, BCFEnumeration enumeration, BCFIndex index);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfAddEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfRemoveEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT BCFIndex bcfGetTopicsCount(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetTopicGUID(BCFProject* project, BCFIndex topic);

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

