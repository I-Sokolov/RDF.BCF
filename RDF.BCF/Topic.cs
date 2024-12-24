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
        public string Guid { get { return Interop.TopicGuid(m_project.Handle, m_handle); } }

        /// <summary>
        /// Title of the topic.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type of the topic(Predefined list in “extensions.xml”)
        /// </summary>
        public string TopicType { get; set; }

        /// <summary>
        /// Status of the topic(Predefined list in “extensions.xml”)
        /// </summary>
        public string TopicStatus { get; set; }

        /// <summary>
        /// Description of the topic.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Date when the topic was created.
        /// ISO 8601 compatible YYYY-MM-DDThh:mm:ss format with optional time zone indicators.
        /// </summary>
        public string CreationDate { get { return "2016-04-28T16:31:12.270+02:00 todo"; } }

        /// <summary>
        /// User who created the topic.
        /// </summary>
        public string CreationAuthor { get { return "CreationAuthor todo"; } }

        /// <summary>
        /// Date when the topic was last modified.  only when Topic has been modified after creation.
        /// ISO 8601 compatible YYYY-MM-DDThh:mm:ss format with optional time zone indicators.
        /// </summary>
        public string? ModifiedDate { get { return null; } }

        /// <summary>
        /// User who modified the topic. Exists only when Topic has been modified after creation.
        /// </summary>
        public string? ModifiedAuthor { get { return null; } }

        /// <summary>
        /// BIM files, associated with the topic
        /// </summary>
        //public BIMFiles BIMFiles { get { return m_topicFiles; } }

        /// <summary>
        /// The topic comments
        /// </summary>
        //public Comments Comments { get { return m_comments; } }

        //public Viewpoints Viewpoints { get { return m_viewpoints; } }

        public UInt16 Handle { get { return m_handle; }  }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        UInt16 m_handle;
        
        internal Topic(Project project, UInt16 handle) 
        {
            m_project = project;
            m_handle = handle;
        }

        #endregion IMPLEMENTATION
    }
}
