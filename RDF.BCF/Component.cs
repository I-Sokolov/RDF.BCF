using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Component
    {
        /// <summary>
        /// 
        /// </summary>
        public string IfcGuid { get { return Interop.ComponentGetIfcGuid(m_handle); } set { Interop.ComponentSetIfcGuid(m_handle, value); } }
        public string OriginatingSystem { get { return Interop.ComponentGetOriginatingSystem(m_handle); } set { Interop.ComponentSetOriginatingSystem(m_handle, value); } }
        public string AuthoringToolId { get { return Interop.ComponentGetAuthoringToolId(m_handle); } set { Interop.ComponentSetAuthoringToolId(m_handle, value); } }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove() { return Interop.ViewPointComponentRemove(m_handle); }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        IntPtr m_handle;

        internal IntPtr Handle { get { return m_handle; } }

        internal Component(Project project, IntPtr handle)
        {
            m_project = project;
            m_handle = handle;
        }

        #endregion IMPLEMENTATION    
    }
}
