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
        public string Text { get { return Interop.CommentGetText(m_handle); } set { Interop.CommentSetText(m_handle, value); } }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal Comment(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        #endregion IMPLEMENTATION
    }
}
