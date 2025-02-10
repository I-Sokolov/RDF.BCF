using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Coloring
    {
        /// <summary>
        /// The color is given in ARGB format. Colors are represented as 6 or 8 hexadecimal digits. If 8 digits are present, the first two represent the alpha (transparency) channel.
        /// </summary>
        public string Color { get { return Interop.ColoringGetColor(m_handle); } set { Interop.ColoringSetColor(m_handle, value); } }

        /// <summary>
        /// Add component to be shown with the coloring 
        /// </summary>
        public Component AddComponet(string? ifcGuid = null)
        {
            IntPtr handle = Interop.ColoringComponentAdd(m_handle, ifcGuid);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add coloring component: " + Interop.GetErrors(Project.Handle));
            return new Component(Project, handle);
        }

        /// <summary>
        /// Get components to be shown with the coloring 
        /// </summary>
        public List<Component> GetComponents()
        {
            var ret = new List<Component>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.ColoringComponentGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Component(Project, handle));
            }
            return ret;
        }

        /// <summary>
        /// Remove object from BCF package. See Memory Management in documentation.
        /// </summary>
        public bool Remove () { return Interop.ColoringRemove(m_handle); }

        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        ViewPoint m_viewPoint;
        IntPtr m_handle;

        internal Project Project { get { return m_viewPoint.Project; } }

        internal IntPtr Handle { get { return m_handle; } }

        internal Coloring(ViewPoint viewPoint, IntPtr handle)
        {
            m_viewPoint = viewPoint;
            m_handle = handle;
        }

        #endregion INTERNAL    
    }
}
