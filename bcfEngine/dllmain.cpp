// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "BCFObject.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        if (BCFObject::gObjectCounter) {
            printf("ERROR: BCF Object counter %d\n", BCFObject::gObjectCounter);
            assert(!"Object counter is not 0");
        }
        break;
    }
    return TRUE;
}

