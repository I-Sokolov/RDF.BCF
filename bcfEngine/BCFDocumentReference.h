#pragma once

#include "BCFObject.h"
struct BCFTopic;

struct BCFDocumentReference : public BCFObject
{
public:
    BCFDocumentReference(BCFTopic& topic):BCFObject(topic.Project()) {}

    void Read(_xml::_element & elem, const std::string& folder) { assert(!"TODO"); }
};

