

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
    RDFBCF_EXPORT BCFProject* rdfbcf_OpenProject(void);

    /// <summary>
    /// 
    /// </summary>
    RDFBCF_EXPORT void rdfbcf_CloseProject(BCFProject* project);


#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_ENGINE_H

