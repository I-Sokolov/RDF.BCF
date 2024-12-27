
#ifndef __RDF_LTF_BCF_TYPES_H
#define __RDF_LTF_BCF_TYPES_H


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

    enum BCFCamera
    {
        BCFCameraPerspective = 0,
        BCFCameraOrthogonal = 1
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

    struct BCFTopicAttributes
    {

    };

    /// <summary>
    /// 
    /// </summary>
    typedef uint16_t BCFIndex;

    /// <summary>
    /// 
    /// </summary>
    #define BCFIndex_ERROR ((uint16_t)-1)


#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_TYPES_H

