
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
#else
    typedef unsigned char bool;
#endif

    /// <summary>
    /// 
    /// </summary>
    struct BCFProject;
    struct BCFTopic;
    struct BCFViewPoint;
    struct BCFComment;
    struct BCFFile;
    struct BCFDocumentReference;
    struct BCFComponent;
    struct BCFColor;
    struct BCFLine;
    struct BCFClippingPlane;
    struct BCFBitmap;

    /// <summary>
    /// 
    /// </summary>
    typedef uint16_t BCFIndex;

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
    enum BCFCamera
    {
        BCFCameraPerspective = 0,
        BCFCameraOrthogonal = 1
    };

    /// <summary>
    /// 
    /// </summary>
    struct BCFPoint
    {
        double x, y, z;
    };

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_TYPES_H

