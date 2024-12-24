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


        [DllImport(DLL, EntryPoint = "bcfCreateProject")]
        public static extern IntPtr CreateProject(string? currentUser = null, [param:MarshalAs(UnmanagedType.U1)] bool autoExtent = false, string? projectId = null);

        [DllImport(DLL, EntryPoint = "bcfCloseProject")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool DeleteProject(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfGetErrors")]
        private static extern IntPtr GetErrors_(IntPtr project);

        public static string GetErrors(IntPtr project)
        {
            return PtrToString (GetErrors_(project));
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

        [DllImport(DLL, EntryPoint = "bcfGetProjectId")]
        private static extern IntPtr GetProjectId_(IntPtr project);

        public static string GetProjectId(IntPtr project)
        {
            return PtrToString (GetProjectId_(project));
        }

        [DllImport(DLL, EntryPoint = "bcfGetProjectName")]
        private static extern IntPtr GetProjectName_(IntPtr project);

        public static string GetProjectName(IntPtr project)
        {
            return PtrToString(GetProjectName_(project));
        }

        [DllImport(DLL, EntryPoint = "bcfSetProjectName")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool SetProjectName(IntPtr project, string name);

        [DllImport(DLL, EntryPoint = "bcfGetEnumerationElement")]
        private static extern IntPtr GetEnumerationElement_(IntPtr project, BCFEnumeration extension, UInt16 index);

        public static string GetEnumerationElement(IntPtr project, BCFEnumeration enumeration, UInt16 index)
        {
            return PtrToString(GetEnumerationElement_(project, enumeration, index));
        }

        [DllImport(DLL, EntryPoint = "bcfAddEnumerationElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool AddEnumerationElement(IntPtr project, BCFEnumeration enumeration, string element);

        [DllImport(DLL, EntryPoint = "bcfRemoveEnumerationElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RemoveEnumerationElement(IntPtr project, BCFEnumeration enumeration, string element);

        /// <summary>
        /// 
        /// </summary>
        private static string PtrToString(IntPtr ptr)
        {
            var str = Marshal.PtrToStringUTF8(ptr);
            if (str == null)
                str = "";
            return str;
        }
    }
}
