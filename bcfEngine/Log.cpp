#include "pch.h"
#include "Log.h"

void Log::error(const char* code, const char* descr, ...) 
{ 
    printf("%s\n", code); 
}
