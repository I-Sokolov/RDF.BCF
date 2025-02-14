
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
    struct BCFBimFile;
    struct BCFViewPoint;
    struct BCFComment;
    struct BCFDocumentReference;
    struct BCFComponent;
    struct BCFColoring;
    struct BCFLine;
    struct BCFClippingPlane;
    struct BCFBitmap;
    struct BCFBimSnippet;

    /// <summary>
    /// 
    /// </summary>
    enum BCFVersion
    {
        BCFVerNotSupported = 0,
        BCFVer_2_0 = 20,
        BCFVer_2_1 = 21,
        BCFVer_3_0 = 30
    };

    /// <summary>
    /// 
    /// </summary>
    enum BCFEnumeration
    {
        BCFUnknown          = 0,
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
        double xyz[3];
    };

    /// <summary>
    /// 
    /// </summary>
    enum BCFBitmapFormat
    {
        BCFBitmapPNG = 0,
        BCFBitmapJPG = 1
    };

#ifdef __cplusplus
} //extern "C"
#endif

#endif // __RDF_LTF_BCF_TYPES_H

