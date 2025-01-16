using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class BimFile
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsExternal { get { return Interop.FileGetIsExternal(m_handle); } set { Interop.FileSetIsExternal(m_handle, value); } }
        public string Filename { get { return Interop.FileGetFilename(m_handle); } set { Interop.FileSetFilename(m_handle, value); } }
        public string Date { get { return Interop.FileGetDate(m_handle); } set { Interop.FileSetDate(m_handle, value); } }
        public string Reference { get { return Interop.FileGetReference(m_handle); } set { Interop.FileSetReference(m_handle, value); } }
        public string IfcProject { get { return Interop.FileGetIfcProject(m_handle); } set { Interop.FileSetIfcProject(m_handle, value); } }
        public string IfcSpatialStructureElement { get { return Interop.FileGetIfcSpatialStructureElement(m_handle); } set { Interop.FileSetIfcSpatialStructureElement(m_handle, value); } }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove() { return Interop.FileRemove(m_handle); }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal BimFile(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        #endregion IMPLEMENTATION
    }
}
