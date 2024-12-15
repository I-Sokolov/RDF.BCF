#include "pch.h"

#include "BCFProject.h"

#ifdef _WINDOWS
    #define RDFBCF_EXPORT __declspec(dllexport) 
#else
    #define RDFBCF_EXPORT 
#endif

#include "RDFBCF.h"


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT BCFProject* rdfbcf_OpenProject(void)
{
    MessageBox(0, "created", "created", 0);
    return new BCFProject();
}


/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void rdfbcf_CloseProject(BCFProject* project)
{
    MessageBox(0, "deleted", "deleted", 0);
    if (project) {
        delete project;
    }
}
