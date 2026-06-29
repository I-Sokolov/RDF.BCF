#pragma once

#ifdef SMOKE_TEST

extern "C" RDFBCF_EXPORT void SmokeTest_DataSet(const char* folder);

extern void SmokeTest_ValidateXSD(const char* xsdName, const char* xmlFilePath, BCFVersion version);

#endif //SMOKE_TEST