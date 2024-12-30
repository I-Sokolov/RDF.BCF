#pragma once

struct BCFProject;
class Topic;

class DocumentReference
{
public:
    DocumentReference(Topic& topic) {}

    void Read(_xml::_element & elem, const std::string& folder) { assert(!"TODO"); }
};

