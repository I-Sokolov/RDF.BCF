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
        public IEnumerable<string> GetEnumeration(Interop.BCFEnumeration enumeration)
        {
            var list = new List<string>();
            UInt16 index = 0;
            string elem;
            while (""!=(elem = Interop.EnumerationElementGet(m_project.Handle, enumeration, index++))){
                list.Add(elem);
            }
            return list;
        }

        /// <summary>
        /// Modify enumeration of given type
        /// </summary>
        public bool EnumerationElementAdd (Interop.BCFEnumeration enumeration, string elem)
        {
            return Interop.EnumerationElementAdd(m_project.Handle, enumeration, elem);
        }

        /// <summary>
        /// Modify elements of enumeration of given type
        /// </summary>
        public bool EnumerationElementRemove (Interop.BCFEnumeration enumeration, string elem)
        {
            return Interop.EnumerationElementRemove(m_project.Handle, enumeration, elem);
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
