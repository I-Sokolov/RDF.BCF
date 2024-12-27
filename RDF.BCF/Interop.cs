using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Interop
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

        public const UInt16 ERR_IND = UInt16.MaxValue;

        [DllImport(DLL, EntryPoint = "bcfProjectCreate")]
        public static extern IntPtr ProjectCreate(string? projectId = null);

        [DllImport(DLL, EntryPoint = "bcfProjectDelete")]
        public static extern void ProjectDelete(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfErrorsGet")]
        private static extern IntPtr ErrorsGet_(IntPtr project, [param: MarshalAs(UnmanagedType.U1)] bool cleanLog);

        public static string ErrorsGet(IntPtr project, bool cleanLog = true)
        {
            return PtrToString (ErrorsGet_(project, cleanLog));
        }

        [DllImport(DLL, EntryPoint = "bcfFileRead")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileRead(IntPtr project, string filePath);

        [DllImport(DLL, EntryPoint = "bcfFileWrite")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileWrite(IntPtr project, string filePath, Version version);

        [DllImport(DLL, EntryPoint = "bcfSetEditor")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool SetEditor(IntPtr project, string user, [param: MarshalAs(UnmanagedType.U1)] bool autoExtent);

        [DllImport(DLL, EntryPoint = "bcfProjectIdGet")]
        private static extern IntPtr ProjectIdGet_(IntPtr project);

        public static string ProjectIdGet(IntPtr project)
        {
            return PtrToString (ProjectIdGet_(project));
        }

        [DllImport(DLL, EntryPoint = "bcfProjectNameGet")]
        private static extern IntPtr ProjectNameGet_(IntPtr project);

        public static string ProjectNameGet(IntPtr project)
        {
            return PtrToString(ProjectNameGet_(project));
        }

        [DllImport(DLL, EntryPoint = "bcfProjectNameSet")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ProjectNameSet(IntPtr project, string name);

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementGet")]
        private static extern IntPtr EnumerationElementGet_(IntPtr project, BCFEnumeration extension, UInt16 index);

        public static string EnumerationElementGet(IntPtr project, BCFEnumeration enumeration, UInt16 index)
        {
            return PtrToString(EnumerationElementGet_(project, enumeration, index));
        }

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool EnumerationElementAdd(IntPtr project, BCFEnumeration enumeration, string element);

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool EnumerationElementRemove(IntPtr project, BCFEnumeration enumeration, string element);

        [DllImport(DLL, EntryPoint = "bcfTopicsCount")]
        public static extern UInt16 TopicsCount(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfTopicGuid")]
        private static extern IntPtr TopicGuid_(IntPtr project, UInt16 topic);

        public static string TopicGuid(IntPtr project, UInt16 topic)
        {
            var ptr = TopicGuid_(project, topic);
            return PtrToString(ptr);
        }

        [DllImport(DLL, EntryPoint = "bcfTopicCreate")]
        public static extern UInt16 TopicCreate(IntPtr project, string? guid);

        [DllImport(DLL, EntryPoint = "bcfTopicRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicRemove(IntPtr project, UInt16 topic);

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
