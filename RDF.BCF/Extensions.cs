using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Extensions
    {
        /// <summary>
        /// Get elements of enumeration of given type
        /// </summary>
        public IEnumerable<string> GetEnumeration(Native.BCFEnumeration enumeration)
        {
            var list = new List<string>();
            UInt16 index = 0;
            string? elem = null;
            while (null!=(elem = Native.GetEnumerationElement(m_project.BCFProject, enumeration, index++))){
                list.Add(elem);
            }
            return list;
        }

        /// <summary>
        /// Modify enumeration of given type
        /// </summary>
        public bool AddEnumerationElement (Native.BCFEnumeration enumeration, string elem)
        {
            return Native.AddEnumerationElement(m_project.BCFProject, enumeration, elem);
        }

        /// <summary>
        /// Modify elements of enumeration of given type
        /// </summary>
        public bool RemoveEnumerationElement (Native.BCFEnumeration enumeration, string elem)
        {
            return Native.RemoveEnumerationElement(m_project.BCFProject, enumeration, elem);
        }


        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        private Project m_project;

        internal Extensions(Project project)
        {
            m_project = project;
        }

        #endregion IMPLEMENTATION
    }
}
