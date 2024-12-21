using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Native
    {
        private const string DDFBCFDLL = @"..\..\bcfEngine.dll";

        /// <summary>
        /// BCF file version
        /// </summary>
        public enum Version
        {
            _2_1 = 21,
            _3_0 = 30
        }

        [DllImport(DDFBCFDLL, EntryPoint = "bcfOpenProject")]
        public static extern IntPtr OpenProject();

        [DllImport(DDFBCFDLL, EntryPoint = "bcfCloseProject")]
        public static extern bool CloseProject(IntPtr project);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfGetErrors")]
        public static extern IntPtr GetErrors_(IntPtr project);

        public static string GetErrors(IntPtr project)
        {
            var ptr = GetErrors_(project);
            var str = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
            return (str!=null) ? str : "";
        }

        [DllImport(DDFBCFDLL, EntryPoint = "bcfClearErrors")]
        public static extern void ClearErrors(IntPtr project);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfInitNew")]
        public static extern bool InitNew(IntPtr project);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfReadFile")]
        public static extern bool ReadFile(IntPtr project, string filePath);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfWriteFile")]
        public static extern bool WriteFile(IntPtr project, string filePath, Version version);
    }
}
