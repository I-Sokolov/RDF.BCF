// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#ifdef _WINDOWS
#define RDFBCF_EXPORT __declspec(dllexport)
#else
#define RDFBCF_EXPORT 
#endif

// add headers that you want to pre-compile here
#include "framework.h"

#include <assert.h>

#include <list>
#include <string>
#include <vector>
#include <set>
#include <regex>

#include "XMLparser/_xml.h"
#include "XMLparser/_xml_writer.h"

typedef std::list<std::string> StringList;
typedef std::set<std::string> StringSet;

#define NULL_CHECK(x) if (!x) { m_project.GetLog().add(Log::Level::error, "NULL argument", "NULL argument"); return false; }

#endif //PCH_H
