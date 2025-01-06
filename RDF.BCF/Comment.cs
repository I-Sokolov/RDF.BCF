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
        /// Read-only persistent topic identifier
        /// </summary>
        public string Guid { get { return Interop.CommentGetGuid(m_handle); } }

        /// <summary>
        /// Comment attributes
        /// </summary>
        public string Text { get { return Interop.CommentGetText(m_handle); } set { Interop.CommentSetText(m_handle, value); } }
        public string Date { get { return Interop.CommentGetDate(m_handle); } }
        public string Author { get { return Interop.CommentGetAuthor(m_handle); } }
        public string ModifiedDate { get { return Interop.CommentGetModifiedDate(m_handle); } }
        public string ModifiedAuthor { get { return Interop.CommentGetModifiedAuthor(m_handle)  ; } }
        public ViewPoint? ViewPoint { get { return GetViewPoint(); } set { SetViewPoint(value); } }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove() { return Interop.CommentRemove(m_handle); }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal Comment(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        private ViewPoint? GetViewPoint()
        {
            var vpHandle = Interop.CommentGetViewPoint(m_handle);
            if (vpHandle != IntPtr.Zero)
            {
                return new ViewPoint(m_topic, vpHandle);
            }
            return null;
        }

        private void SetViewPoint(ViewPoint? vp)
        {
            var ok = Interop.CommentSetViewPoint(m_handle, vp==null? IntPtr.Zero : vp.Handle);
        }

        #endregion IMPLEMENTATION
    }
}
