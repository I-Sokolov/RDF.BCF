#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "GuidReference.h"
#include "GuidStr.h"

struct ViewPoint;

struct Comment : public BCFObject, public BCFComment
{
public:
    Comment(Topic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

public:
    //BCFComment
    virtual const char* GetGuid() override { return m_Guid.c_str(); }
    virtual const char* GetDate() override { return m_Date.c_str(); }
    virtual const char* GetAuthor() override { return m_Author.c_str(); }
    virtual const char* GetModifiedDate() override { return m_ModifiedDate.c_str(); }
    virtual const char* GetModifiedAuthor() override { return m_ModifiedAuthor.c_str(); }
    virtual const char* GetText() override { return m_Comment.c_str(); }
    virtual BCFViewPoint*  GetViewPoint();

    virtual bool SetText(const char* value) override;
    virtual bool SetViewPoint(BCFViewPoint* viewPoint) override;

    virtual bool Remove() override { return RemoveImpl(); }

private:
    bool UpdateAuthor();

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

private:
    void Write_Comment(_xml_writer& writer, const std::string& folder);

private:
    Topic&                  m_topic;
    bool                       m_readFromFile;

private:
    GuidStr                    m_Guid;
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    GuidReference              m_Viewpoint;
};

