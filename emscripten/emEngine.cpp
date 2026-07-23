#include <stdio.h>
#include <string>
#include <set>
using namespace std;

#include <emscripten/bind.h>
using namespace emscripten;

#include "ifcengine.h"
#include "bcfAPI.h"

BCFProject* m_bcfProject = nullptr;

string loadBCF(string filePath)
{
	if (m_bcfProject != nullptr) {
		if (!m_bcfProject->Delete()) {
			//#todo log
			return "[BCFProject::Delete] Failed.";
		}
		m_bcfProject = nullptr;
	}

	m_bcfProject = BCFProject::Create();
	if (!m_bcfProject->ReadFile(filePath.c_str(), false))
	{
		//#todo log
		return "[BCFProject::ReadFile] Failed.";
	}

	//#todo log
	printf("Project Id: %s\n", m_bcfProject->GetProjectId());
	printf("Project Name: %s\n", m_bcfProject->GetName());

	m_bcfProject->SetOptions("BCF-WASM", false, false);//#tbd
	m_bcfProject->SetName(m_bcfProject->GetName());

	return "OK";
}

vector<string> getExtensions()
{
	vector<string> vecExtensions;

	if (m_bcfProject == nullptr) {
		//#todo log
		printf("Error: not initialized.");
		return vecExtensions;
	}

	auto& extensions = m_bcfProject->GetExtensions();

	uint16_t ind = 0;
	while (auto elem = extensions.GetElement(BCFTopicTypes, ind++)) {
		printf("BCFTopicTypes: %s\n", elem);//#todo log
	}

	ind = 0;
	while (auto elem = extensions.GetElement(BCFTopicStatuses, ind++)) {
		vecExtensions.push_back(elem);
	}

	ind = 0;
	while (auto elem = extensions.GetElement(BCFPriorities, ind++)) {
		vecExtensions.push_back(elem);
	}

	ind = 0;
	while (auto elem = extensions.GetElement(BCFUsers, ind++)) {
		vecExtensions.push_back(elem);
	}

	ind = 0;
	while (auto elem = extensions.GetElement(BCFSnippetTypes, ind++)) {
		vecExtensions.push_back(elem);
	}

	ind = 0;
	while (auto elem = extensions.GetElement(BCFStages, ind++)) {
		vecExtensions.push_back(elem);
	}

	return vecExtensions;
}

vector<string> getTopics()
{
	vector<string> vecTopics;

	if (m_bcfProject == nullptr) {
		//#todo log
		printf("Error: not initialized.");
		return vecTopics;
	}

	uint16_t ind = 0;
	while (auto pTopic = m_bcfProject->GetTopic(ind++)) {
		vecTopics.push_back(pTopic->GetTitle());
	}

	return vecTopics;
}

vector<string> getTopicBIMFiles(int topic)
{
	vector<string> vecBIMFiles;

	if (m_bcfProject == nullptr) {
		//#todo log
		printf("Error: not initialized.");
		return vecBIMFiles;
	}

	auto pTopic = m_bcfProject->GetTopic(topic);
	if (pTopic == nullptr) {
		//#todo log
		printf("Error: Topic not found.");
		return vecBIMFiles;
	}

	uint16_t ind = 0;
	while (auto pBIMFile = pTopic->GetBimFile(ind++)) {
		vecBIMFiles.push_back(pBIMFile->GetFilename());
	}

	return vecBIMFiles;
}

EMSCRIPTEN_BINDINGS(my_module) {
	register_vector<int_t>("VectorInt_t");
	register_vector<float>("VectorFloat");
	register_vector<string>("VectorStr_t");

	emscripten::function("loadBCF", &loadBCF);
	emscripten::function("getExtensions", &getExtensions);
	emscripten::function("getTopics", &getTopics);
	emscripten::function("getTopicBIMFiles", &getTopicBIMFiles);
}