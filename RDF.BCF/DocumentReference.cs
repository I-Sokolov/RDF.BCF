using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class DocumentReference
    {
        public string Guid { get { return Interop.DocumentReferenceGetGuid(m_handle); } }
        public string UrlPath { get { return Interop.DocumentReferenceGetUrlPath(m_handle); } set { Interop.DocumentReferenceSetUrlPath(m_handle, value); } }
        public string Description { get { return Interop.DocumentReferenceGetDescription(m_handle); } set { Interop.DocumentReferenceSetDescription(m_handle, value); } }

        public bool Remove() { return Interop.DocumentReferenceRemove(m_handle); }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal DocumentReference(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }
        #endregion IMPLEMENTATION
    }
}
