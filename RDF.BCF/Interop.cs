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
        /// 
        /// </summary>

        private static string PtrToString(IntPtr ptr)
        {
            var str = Marshal.PtrToStringUTF8(ptr);
            if (str == null)
                str = "";
            return str;
        }

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
        public static extern IntPtr ProjectCreate([param: MarshalAs(UnmanagedType.LPUTF8Str)] string? projectId = null);

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
        public static extern bool FileRead(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

        [DllImport(DLL, EntryPoint = "bcfFileWrite")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileWrite(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath, Version version);

        [DllImport(DLL, EntryPoint = "bcfSetAuthor")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool SetAuthor(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string user, [param: MarshalAs(UnmanagedType.U1)] bool autoExtent);

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
        public static extern bool ProjectNameSet(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementGet")]
        private static extern IntPtr EnumerationElementGet_(IntPtr project, BCFEnumeration extension, UInt16 index);

        public static string EnumerationElementGet(IntPtr project, BCFEnumeration enumeration, UInt16 index)
        {
            return PtrToString(EnumerationElementGet_(project, enumeration, index));
        }

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool EnumerationElementAdd(IntPtr project, BCFEnumeration enumeration, [param:MarshalAs(UnmanagedType.LPUTF8Str)] string element);

        [DllImport(DLL, EntryPoint = "bcfEnumerationElementRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool EnumerationElementRemove(IntPtr project, BCFEnumeration enumeration, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string element);

        [DllImport(DLL, EntryPoint = "bcfTopicIterate")]
        public static extern IntPtr TopicsIterate(IntPtr project, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfTopicGetGuid")]
        private static extern IntPtr TopicGetGuid_(IntPtr topic);

        public static string TopicGetGuid(IntPtr topic)
        {
            var ptr = TopicGetGuid_(topic);
            return PtrToString(ptr);
        }

        [DllImport(DLL, EntryPoint = "bcfTopicCreate")]
        public static extern IntPtr TopicCreate(
            IntPtr project, 
            [param: MarshalAs(UnmanagedType.LPUTF8Str)] string type, 
            [param: MarshalAs(UnmanagedType.LPUTF8Str)] string title, 
            [param: MarshalAs(UnmanagedType.LPUTF8Str)] string status,
            [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? guid
            );

        [DllImport(DLL, EntryPoint = "bcfTopicRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicRemove(IntPtr topic);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL, EntryPoint = "bcfTopicGetServerAssignedId")]
        private static extern IntPtr TopicGetServerAssignedId_(IntPtr topic);        
        public static string TopicGetServerAssignedId (IntPtr topic) { return PtrToString (TopicGetServerAssignedId_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTopicStatus")]
        private static extern IntPtr TopicGetTopicStatus_(IntPtr topic);
        public static string TopicGetTopicStatus(IntPtr topic) { return PtrToString(TopicGetTopicStatus_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTopicType")]
        private static extern IntPtr TopicGetTopicType_(IntPtr topic);
        public static string TopicGetTopicType(IntPtr topic) { return PtrToString(TopicGetTopicType_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetTitle")]
        private static extern IntPtr TopicGetTitle_(IntPtr topic);
        public static string TopicGetTitle(IntPtr topic) { return PtrToString(TopicGetTitle_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetPriority")]
        private static extern IntPtr TopicGetPriority_(IntPtr topic);
        public static string TopicGetPriority(IntPtr topic) { return PtrToString(TopicGetPriority_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetCreationDate")]
        private static extern IntPtr TopicGetCreationDate_(IntPtr topic);
        public static string TopicGetCreationDate(IntPtr topic) { return PtrToString(TopicGetCreationDate_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetCreationAuthor")]
        private static extern IntPtr TopicGetCreationAuthor_(IntPtr topic);
        public static string TopicGetCreationAuthor(IntPtr topic) { return PtrToString(TopicGetCreationAuthor_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetModifiedDate")]
        private static extern IntPtr TopicGetModifiedDate_(IntPtr topic);
        public static string TopicGetModifiedDate(IntPtr topic) { return PtrToString(TopicGetModifiedDate_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetModifiedAuthor")]
        private static extern IntPtr TopicGetModifiedAuthor_(IntPtr topic);
        public static string TopicGetModifiedAuthor(IntPtr topic) { return PtrToString(TopicGetModifiedAuthor_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetDueDate")]
        private static extern IntPtr TopicGetDueDate_(IntPtr topic);
        public static string TopicGetDueDate(IntPtr topic) { return PtrToString(TopicGetDueDate_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetAssignedTo")]
        private static extern IntPtr TopicGetAssignedTo_(IntPtr topic);
        public static string TopicGetAssignedTo(IntPtr topic) { return PtrToString(TopicGetAssignedTo_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetDescription")]
        private static extern IntPtr TopicGetDescription_(IntPtr topic);
        public static string TopicGetDescription(IntPtr topic) { return PtrToString(TopicGetDescription_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetStage")]
        private static extern IntPtr TopicGetStage_(IntPtr topic);
        public static string TopicGetStage(IntPtr topic) { return PtrToString(TopicGetStage_(topic)); }

        [DllImport(DLL, EntryPoint = "bcfTopicGetIndex")]
        public static extern int TopicGetIndex(IntPtr topic);

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfTopicSetServerAssignedId")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetServerAssignedId(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTopicType")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTopicType(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTitle")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTitle(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetTopicStatus")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetTopicStatus(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetPriority")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetPriority(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetDueDate")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetDueDate(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetAssignedTo")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetAssignedTo(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetDescription")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetDescription(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetStage")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetStage(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfTopicSetIndex")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool TopicSetIndex(IntPtr topic, int val);

        [DllImport(DLL, EntryPoint = "bcfCommentIterate")]
        public static extern IntPtr CommentIterate(IntPtr topic, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfCommentCreate")]
        public static extern IntPtr CommentCreate(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? guid = null);

        [DllImport(DLL, EntryPoint = "bcfCommentRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CommentRemove(IntPtr comment);

        [DllImport(DLL, EntryPoint = "bcfCommentGetGuid")]
        private static extern IntPtr CommentGetGuid_(IntPtr comment);
        public static string CommentGetGuid(IntPtr project, UInt16 topic, UInt16 comment) { return PtrToString(CommentGetGuid_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetDate")]
        private static extern IntPtr CommentGetDate_(IntPtr comment);
        public static string CommentGetDate(IntPtr comment) { return PtrToString(CommentGetDate_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetAuthor")]
        private static extern IntPtr CommentGetAuthor_(IntPtr comment);
        public static string CommentGetAuthor(IntPtr comment) { return PtrToString(CommentGetAuthor_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetModifiedDate")]
        private static extern IntPtr CommentGetModifiedDate_(IntPtr comment);
        public static string CommentGetModifiedDate(IntPtr comment) { return PtrToString(CommentGetModifiedDate_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetModifiedAuthor")]
        private static extern IntPtr CommentGetModifiedAuthor_(IntPtr comment);
        public static string CommentGetModifiedAuthor(IntPtr comment) { return PtrToString(CommentGetModifiedAuthor_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetText")]
        private static extern IntPtr CommentGetText_(IntPtr comment);
        public static string CommentGetText(IntPtr comment) { return PtrToString(CommentGetText_(comment)); }

        [DllImport(DLL, EntryPoint = "bcfCommentGetViewPoint")]
        public static extern IntPtr CommentGetViewPoint(IntPtr comment);

        [DllImport(DLL, EntryPoint = "bcfCommentSetText")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CommentSetText(IntPtr comment, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(DLL, EntryPoint = "bcfCommentSetViewPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CommentSetViewPoint(IntPtr comment, IntPtr viewPoint);
    }
}
