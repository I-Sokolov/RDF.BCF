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
        /// Is the IFC file external or within the bcfzip. (Default = true).
        /// </summary>
        public bool IsExternal { get { return Interop.BimFileGetIsExternal(m_handle); } set { Interop.BimFileSetIsExternal(m_handle, value); } }

        /// <summary>
        /// The BIM file related to this topic. For IFC files this is the first item in the FILE_NAME entry in the IFC file's header.
        /// </summary>
        public string Filename { get { return Interop.BimFileGetFilename(m_handle); } set { Interop.BimFileSetFilename(m_handle, value); } }

        /// <summary>
        /// Date of the BIM file. For IFC files this is the second entry of the FILE_NAME entry in the IFC file's header. 
        /// When the timestamp given in the header does not provide timezone, it is interpreted as UTC.
        /// </summary>
        public string Date { get { return Interop.BimFileGetDate(m_handle); } set { Interop.BimFileSetDate(m_handle, value); } }

        /// <summary>
        /// Absolute file path (file system or URI). See Files Management in documentation.
        /// </summary>
        public string Reference { get { return Interop.BimFileGetReference(m_handle); } set { Interop.BimFileSetReference(m_handle, value); } }

        /// <summary>
        /// IfcGuid Reference to the project to which this topic is related in the IFC file.
        /// </summary>
        public string IfcProject { get { return Interop.BimFileGetIfcProject(m_handle); } set { Interop.BimFileSetIfcProject(m_handle, value); } }

        /// <summary>
        /// IfcGuid Reference to the spatial structure element, e.g. IfcBuildingStorey, to which this topic is related.
        /// </summary>
        public string IfcSpatialStructureElement { get { return Interop.BimFileGetIfcSpatialStructureElement(m_handle); } set { Interop.BimFileSetIfcSpatialStructureElement(m_handle, value); } }

        /// <summary>
        /// Remove object from BCF package. See Memory Management in documentation.
        /// </summary>
        public bool Remove() { return Interop.BimFileRemove(m_handle); }

        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal BimFile(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        #endregion INTERNAL
    }
}
