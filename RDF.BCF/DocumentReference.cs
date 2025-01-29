using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class DocumentReference
    {
        /// <summary>
        /// Guid attribute for identifying the reference uniquely.
        /// </summary>
        public string Guid { get { return Interop.DocumentReferenceGetGuid(m_handle); } }

        /// <summary>
        /// Url of an external document.
        /// </summary>
        public string FilePath { get { return Interop.DocumentReferenceGetFilePath(m_handle); } }

        public bool SetFilePath(string filePath, bool isExternal = true) { return Interop.DocumentReferenceSetFilePath(m_handle, filePath, isExternal); }
        /// <summary>
        /// Human readable description of the document reference
        /// </summary>
        public string Description { get { return Interop.DocumentReferenceGetDescription(m_handle); } set { Interop.DocumentReferenceSetDescription(m_handle, value); } }

        /// <summary>
        /// Remove the reference from the BCF package. See Memory Management in documentation
        /// </summary>
        /// <returns></returns>
        public bool Remove() { return Interop.DocumentReferenceRemove(m_handle); }

        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal DocumentReference(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }
        #endregion INTERNAL
    }
}
