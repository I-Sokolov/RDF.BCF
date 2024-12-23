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
        private const string DLL = "bcfEngine.dll";

        /// <summary>
        /// BCF file version
        /// </summary>
        public enum Version
        {
            _2_1 = 21,
            _3_0 = 30
        }

        /// <summary>
        /// Types of BCF extension enumerations 
        /// </summary>
        public enum BCFEnumeration
        {
            TopicTypes = 1,
            TopicStatuses = 2,
            Priorities = 3,
            TopicLabels = 4,
            Users = 5,
            SnippetTypes = 6,
            Stages = 7
        };


        [DllImport(DLL, EntryPoint = "bcfOpenProject")]
        public static extern IntPtr OpenProject();

        [DllImport(DLL, EntryPoint = "bcfCloseProject")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CloseProject(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfGetErrors")]
        public static extern IntPtr GetErrors_(IntPtr project);

        public static string GetErrors(IntPtr project)
        {
            var ptr = GetErrors_(project);
            var str = Marshal.PtrToStringAnsi(ptr);
            return (str!=null) ? str : "";
        }

        [DllImport(DLL, EntryPoint = "bcfClearErrors")]
        public static extern void ClearErrors(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfInitNew")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool InitNew(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfReadFile")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ReadFile(IntPtr project, string filePath);

        [DllImport(DLL, EntryPoint = "bcfWriteFile")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool WriteFile(IntPtr project, string filePath, Version version);

        [DllImport(DLL, EntryPoint = "bcfGetEnumerationElement")]
        public static extern IntPtr GetEnumerationElement_(IntPtr project, BCFEnumeration extension, UInt16 index);

        public static string? GetEnumerationElement(IntPtr project, BCFEnumeration enumeration, UInt16 index)
        {
            var ptr = GetEnumerationElement_(project, enumeration, index);
            return Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport(DLL, EntryPoint = "bcfAddEnumerationElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool AddEnumerationElement(IntPtr project, BCFEnumeration enumeration, string element);

        [DllImport(DLL, EntryPoint = "bcfRemoveEnumerationElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RemoveEnumerationElement(IntPtr project, BCFEnumeration enumeration, string element);
    }
}
