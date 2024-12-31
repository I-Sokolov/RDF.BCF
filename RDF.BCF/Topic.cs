using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Topic
    {
        /// <summary>
        /// Read-only persistent topic identifier
        /// </summary>
        public string Guid { get { return Interop.TopicGetGuid(m_project.Handle, m_handle); } }

        /// <summary>
        /// Topic attributes
        /// </summary>
        public string ServerAssignedId { get { return Interop.TopicGetServerAssignedId(m_project.Handle, m_handle); } set { Interop.TopicSetServerAssignedId(m_project.Handle, m_handle, value); } }
        public string TopicStatus { get { return Interop.TopicGetTopicStatus(m_project.Handle, m_handle); } set { Interop.TopicSetTopicStatus(m_project.Handle, m_handle, value); } }
        public string TopicType { get { return Interop.TopicGetTopicType(m_project.Handle, m_handle); } set { Interop.TopicSetTopicType(m_project.Handle, m_handle, value); } }
        public string Title { get { return Interop.TopicGetTitle(m_project.Handle, m_handle); } set { Interop.TopicSetTitle(m_project.Handle, m_handle, value); } }
        public string Priority { get { return Interop.TopicGetPriority(m_project.Handle, m_handle); } set { Interop.TopicSetPriority(m_project.Handle, m_handle, value); } }
        public string CreationDate { get { return Interop.TopicGetCreationDate(m_project.Handle, m_handle); } }
        public string CreationAuthor { get { return Interop.TopicGetCreationAuthor(m_project.Handle, m_handle); } }
        public string ModifiedDate { get { return Interop.TopicGetModifiedDate(m_project.Handle, m_handle); } }
        public string ModifiedAuthor { get { return Interop.TopicGetModifiedAuthor(m_project.Handle, m_handle); } }
        public string DueDate { get { return Interop.TopicGetDueDate(m_project.Handle, m_handle); } set { Interop.TopicSetDueDate(m_project.Handle, m_handle, value); } }
        public string AssignedTo { get { return Interop.TopicGetAssignedTo(m_project.Handle, m_handle); } set { Interop.TopicSetAssignedTo(m_project.Handle, m_handle, value); } }
        public string Description { get { return Interop.TopicGetDescription(m_project.Handle, m_handle); } set { Interop.TopicSetDescription(m_project.Handle, m_handle, value); } }
        public string Stage { get { return Interop.TopicGetStage(m_project.Handle, m_handle); } set { Interop.TopicSetStage(m_project.Handle, m_handle, value); } }
        public int Index { get { return Interop.TopicGetIndex(m_project.Handle, m_handle); } set { Interop.TopicSetIndex(m_project.Handle, m_handle, value); } }

        /// <summary>
        /// BIM files, associated with the topic
        /// </summary>
        //public BIMFiles BIMFiles { get { return m_topicFiles; } }

        /// <summary>
        /// The topic comments
        /// </summary>
        public Comments Comments { get { return m_comments; } }

        //public Viewpoints Viewpoints { get { return m_viewpoints; } }

        public UInt16 Handle { get { return m_handle; }  }

        public Project Project { get { return m_project; } }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        UInt16 m_handle;
        Comments m_comments;
        
        internal Topic(Project project, UInt16 handle) 
        {
            m_project = project;
            m_handle = handle;

            m_comments = new Comments(this);
        }

        #endregion IMPLEMENTATION
    }
}
