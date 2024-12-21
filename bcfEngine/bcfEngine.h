

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
        _2_1 = 21,
        _3_0 = 30
    };

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
    RDFBCF_EXPORT BCFProject* bcfOpenProject(void);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfCloseProject(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT const char* bcfGetErrors(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT void bcfClearErrors(BCFProject* project);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfInitNew(BCFProject* project);

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
    RDFBCF_EXPORT const char* bcfGetEnumerationElement(BCFProject* project, BCFEnumeration enumeration, unsigned short index);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfAddEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT bool bcfRemoveEnumerationElement(BCFProject* project, BCFEnumeration enumeration, const char* element);

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

