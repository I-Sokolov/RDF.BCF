using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Comment
    {
        /// <summary>
        /// The comment text, must not be empty if provided
        /// </summary>
        public string Text 
        { 
            get { return Interop.CommentGetComment(m_topic.Project.Handle, m_topic.Handle, Handle); } 
            set { Interop.CommentSetComment(m_topic.Project.Handle, m_topic.Handle, Handle, value); } 
        }

        public UInt16 Handle { get { return m_handle; } }

        public Topic Topic { get { return m_topic; } }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        UInt16 m_handle;

        internal Comment(Topic topic, UInt16 handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        #endregion IMPLEMENTATION
    }
}
