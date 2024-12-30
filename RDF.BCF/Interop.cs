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

        [DllImport(DLL, EntryPoint = "bcfTopicGetGuid")]
        private static extern IntPtr TopicGetGuid_(IntPtr project, UInt16 topic);

        public static string TopicGetGuid(IntPtr project, UInt16 topic)
        {
            var ptr = TopicGetGuid_(project, topic);
            return PtrToString(ptr);
        }

        [DllImport(DLL, EntryPoint = "bcfTopicCreate")]
        public static extern UInt16 TopicCreate(IntPtr project, string type, string title, string status, string? guid);

        [DllImport(DLL, EntryPoint = "bcfTopicRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicRemove(IntPtr project, UInt16 topic);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL, EntryPoint = "bcfTopicGetServerAssignedId")]
        private static extern IntPtr TopicGetServerAssignedId_(IntPtr project, UInt16 topic);        
        public static string TopicGetServerAssignedId (IntPtr project, UInt16 topic) { return PtrToString (TopicGetServerAssignedId_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTopicStatus")]
        private static extern IntPtr TopicGetTopicStatus_(IntPtr project, UInt16 topic);
        public static string TopicGetTopicStatus(IntPtr project, UInt16 topic) { return PtrToString(TopicGetTopicStatus_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTopicType")]
        private static extern IntPtr TopicGetTopicType_(IntPtr project, UInt16 topic);
        public static string TopicGetTopicType(IntPtr project, UInt16 topic) { return PtrToString(TopicGetTopicType_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTitle")]
        private static extern IntPtr TopicGetTitle_(IntPtr project, UInt16 topic);
        public static string TopicGetTitle(IntPtr project, UInt16 topic) { return PtrToString(TopicGetTitle_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetPriority")]
        private static extern IntPtr TopicGetPriority_(IntPtr project, UInt16 topic);
        public static string TopicGetPriority(IntPtr project, UInt16 topic) { return PtrToString(TopicGetPriority_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetCreationDate")]
        private static extern IntPtr TopicGetCreationDate_(IntPtr project, UInt16 topic);
        public static string TopicGetCreationDate(IntPtr project, UInt16 topic) { return PtrToString(TopicGetCreationDate_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetCreationAuthor")]
        private static extern IntPtr TopicGetCreationAuthor_(IntPtr project, UInt16 topic);
        public static string TopicGetCreationAuthor(IntPtr project, UInt16 topic) { return PtrToString(TopicGetCreationAuthor_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetModifiedDate")]
        private static extern IntPtr TopicGetModifiedDate_(IntPtr project, UInt16 topic);
        public static string TopicGetModifiedDate(IntPtr project, UInt16 topic) { return PtrToString(TopicGetModifiedDate_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetModifiedAuthor")]
        private static extern IntPtr TopicGetModifiedAuthor_(IntPtr project, UInt16 topic);
        public static string TopicGetModifiedAuthor(IntPtr project, UInt16 topic) { return PtrToString(TopicGetModifiedAuthor_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetDueDate")]
        private static extern IntPtr TopicGetDueDate_(IntPtr project, UInt16 topic);
        public static string TopicGetDueDate(IntPtr project, UInt16 topic) { return PtrToString(TopicGetDueDate_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetAssignedTo")]
        private static extern IntPtr TopicGetAssignedTo_(IntPtr project, UInt16 topic);
        public static string TopicGetAssignedTo(IntPtr project, UInt16 topic) { return PtrToString(TopicGetAssignedTo_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetDescription")]
        private static extern IntPtr TopicGetDescription_(IntPtr project, UInt16 topic);
        public static string TopicGetDescription(IntPtr project, UInt16 topic) { return PtrToString(TopicGetDescription_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetStage")]
        private static extern IntPtr TopicGetStage_(IntPtr project, UInt16 topic);
        public static string TopicGetStage(IntPtr project, UInt16 topic) { return PtrToString(TopicGetStage_(project, topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetIndex")]
        public static extern int TopicGetIndex(IntPtr project, UInt16 topic);

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfTopicSetServerAssignedId")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetServerAssignedId(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTopicType")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTopicType(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTitle")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTitle(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTopicStatus")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTopicStatus(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetPriority")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetPriority(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetDueDate")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetDueDate(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetAssignedTo")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetAssignedTo(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetDescription")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetDescription(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetStage")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetStage(IntPtr project, UInt16 topic, string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetIndex")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetIndex(IntPtr project, UInt16 topic, int val);


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
