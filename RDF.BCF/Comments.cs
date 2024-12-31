using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Comments
    {
        /// <summary>
        /// List comments at the moment of call
        /// </summary>
        public Comment[] Items { get { return CreateList(); } }

        /// <summary>
        /// Adds file to the topic by URL or local path.
        /// Can keep it as external or copy to BCF data
        /// </summary>
        public Comment? CreateComment(string? guid = null)
        {
            var ncomment = Interop.CommentCreate(m_topic.Project.Handle, m_topic.Handle, guid);
            if (ncomment != Interop.ERR_IND)
            {
                return new Comment(m_topic, ncomment);
            }
            return null;
        }

        public bool RemoveComment(Comment comment)
        {
            return Interop.CommentRemove(m_topic.Project.Handle, m_topic.Handle, comment.Handle);
        }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        private Topic m_topic;

        ///
        internal Comments(Topic topic)
        {
            m_topic = topic;
        }

        private Comment[] CreateList()
        {
            var N = Interop.CommentsCount(m_topic.Project.Handle, m_topic.Handle);
         
            var list = new Comment[N];

            for (UInt16 i = 0; i < N; i++)
            {
                list[i] = new Comment(m_topic, i);
            }

            return list;
        }

        #endregion IMPLEMENTATION
    }
}
