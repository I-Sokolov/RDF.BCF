using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Topics
    {
        /// <summary>
        /// Get topics in the BCF data, existing to the moment of call
        /// </summary>
        public IEnumerable<Topic> Items { get { return CreateList(); } }

        /// <summary>
        /// Creates new topic.
        /// Caller can assign GUID or it will generated automatically, GUID never changes after creation
        /// </summary>
        public Topic? TopicCreate (string type, string title, string status, string? guid = null) 
        { 
            var ntopic = Interop.TopicCreate(m_project.Handle, type, title, status, guid); 
            if (ntopic != Interop.ERR_IND)
            {
                return new Topic (m_project, ntopic);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TopicRemove (Topic topic)
        {
            return Interop.TopicRemove(m_project.Handle, topic.Handle);
        }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        private Project m_project;

        ///
        internal Topics(Project project) 
        {
            m_project = project;
        }

        private List<Topic> CreateList()
        {
            var list = new List<Topic> ();

            var N = Interop.TopicsCount(m_project.Handle);
            for (UInt16 i = 0; i < N; i++)
            {
                list.Add(new Topic(m_project, i));
            }

            return list;
        }

        #endregion IMPLEMENTATION
    }
}
