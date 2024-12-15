using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    internal class Native
    {
        private const string DDFBCFDLL = @"W:\DevArea\RDF\BCF\output\bcfEngine.dll";

        [DllImport(DDFBCFDLL, EntryPoint = "rdfbcf_OpenProject")]
        public static extern IntPtr OpenProject();


        [DllImport(DDFBCFDLL, EntryPoint = "rdfbcf_CloseProject")]
        public static extern void CloseProject(IntPtr project);
    }
}
