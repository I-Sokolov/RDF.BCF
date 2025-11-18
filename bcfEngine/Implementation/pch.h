// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#ifdef _USRDLL
#define RDFBCF_EXPORT __declspec(dllexport)
#else
#define RDFBCF_EXPORT 
#endif

// add headers that you want to pre-compile here
#ifndef __EMSCRIPTEN__
#include "framework.h"
#endif

#include <assert.h>

#include <list>
#include <string>
#include <vector>
#include <set>
#include <regex>
#include <unordered_set>

#include "../XMLparser/_xml.h"
#include "../XMLparser/_xml_writer.h"

typedef std::list<std::string> StringList;
typedef std::set<std::string> StringSet;

#define NULL_CHECK(x) if (!x) { m_project.Log_().add(Log::Level::error, "NULL argument", "NULL argument"); return false; }

#define EPS 1e-8

#endif //PCH_H
