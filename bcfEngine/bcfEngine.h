

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

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

