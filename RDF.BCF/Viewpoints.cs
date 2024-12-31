using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class ViewPoints
    {
        /// <summary>
        /// List comments at the moment of call
        /// </summary>
        public IEnumerable<ViewPoint> Items { get { return m_viewpoints; } }

        /// <summary>
        /// Adds file to the topic by URL or local path.
        /// Can keep it as external or copy to BCF data
        /// </summary>
        public ViewPoint Add(Guid? guid = null) { return new ViewPoint(m_project, m_topic, guid); }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        private Project m_project;
        private Topic m_topic;
        private List<ViewPoint> m_viewpoints = new List<ViewPoint>();

        ///
        internal ViewPoints(Project project, Topic topic)
        {
            m_project = project;
            m_topic = topic;
        }

        #endregion IMPLEMENTATION
    }
}
