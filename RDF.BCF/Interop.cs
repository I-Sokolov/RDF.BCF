using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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

        /// <summary>
        /// 
        /// </summary>
        public enum BCFCamera
        {
            Perspective = 0,
            Orthogonal = 1
        };

        public struct BCFPoint
        {
            public double X, Y, Z;

            public BCFPoint(double x = 0, double y = 0, double z = 0) { X = x; Y = y; Z = z; }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum BCFBitmapFormat
        {
            BCFBitmapPNG = 0,
            BCFBitmapJPG = 1
        };

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfProjectCreate")]
        public static extern IntPtr ProjectCreate([param: MarshalAs(UnmanagedType.LPUTF8Str)] string? projectId = null);

        [DllImport(DLL, EntryPoint = "bcfProjectDelete")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ProjectDelete(IntPtr project);

        [DllImport(DLL, EntryPoint = "bcfGetErrors")]
        private static extern IntPtr GetErrors_(IntPtr project, [param: MarshalAs(UnmanagedType.U1)] bool cleanLog);

        public static string GetErrors(IntPtr project, bool cleanLog = true)
        {
            return PtrToString(GetErrors_(project, cleanLog));
        }

        [DllImport(DLL, EntryPoint = "bcfFileRead")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileRead(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath, [param: MarshalAs(UnmanagedType.U1)] bool autofix);

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
            return PtrToString(ProjectIdGet_(project));
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
        public static extern bool EnumerationElementAdd(IntPtr project, BCFEnumeration enumeration, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string element);

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

        [DllImport(DLL, EntryPoint = "bcfTopicAdd")]
        public static extern IntPtr TopicAdd(
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
        public static string TopicGetServerAssignedId(IntPtr topic) { return PtrToString(TopicGetServerAssignedId_(topic)); }

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


        [DllImport(DLL, EntryPoint = "bcfFileIterate")]
        public static extern IntPtr FileIterate(IntPtr topic, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfFileAdd")]
        public static extern IntPtr FileAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? filePath, [param: MarshalAs(UnmanagedType.U1)] bool isExternal = true);

        [DllImport(DLL, EntryPoint = "bcfFileRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileRemove(IntPtr file);

        [DllImport(DLL, EntryPoint = "bcfFileGetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileGetIsExternal(IntPtr file);
        [DllImport(DLL, EntryPoint = "bcfFileGetFilename")]
        private static extern IntPtr _FileGetFilename(IntPtr file);
        public static string FileGetFilename(IntPtr file) { return PtrToString(_FileGetFilename(file)); }

        [DllImport(DLL, EntryPoint = "bcfFileGetDate")]
        private static extern IntPtr _FileGetDate(IntPtr file);
        public static string FileGetDate(IntPtr file) { return PtrToString(_FileGetDate(file)); }

        [DllImport(DLL, EntryPoint = "bcfFileGetReference")]
        private static extern IntPtr _FileGetReference(IntPtr file);
        public static string FileGetReference(IntPtr file) { return PtrToString(_FileGetReference(file)); }

        [DllImport(DLL, EntryPoint = "bcfFileGetIfcProject")]
        private static extern IntPtr _FileGetIfcProject(IntPtr file);
        public static string FileGetIfcProject(IntPtr file) { return PtrToString(_FileGetIfcProject(file)); }

        [DllImport(DLL, EntryPoint = "bcfFileGetIfcSpatialStructureElement")]
        private static extern IntPtr _FileGetIfcSpatialStructureElement(IntPtr file); public static string FileGetIfcSpatialStructureElement(IntPtr file) { return PtrToString(_FileGetIfcSpatialStructureElement(file)); }

        [DllImport(DLL, EntryPoint = "bcfFileSetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetIsExternal(IntPtr file, bool val);

        [DllImport(DLL, EntryPoint = "bcfFileSetFilename")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetFilename(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);
        [DllImport(DLL, EntryPoint = "bcfFileSetDate")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetDate(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfFileSetReference")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetReference(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfFileSetIfcProject")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetIfcProject(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfFileSetIfcSpatialStructureElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FileSetIfcSpatialStructureElement(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfCommentIterate")]
        public static extern IntPtr CommentIterate(IntPtr topic, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfCommentAdd")]
        public static extern IntPtr CommentAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? guid = null);

        [DllImport(DLL, EntryPoint = "bcfCommentRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CommentRemove(IntPtr comment);

        [DllImport(DLL, EntryPoint = "bcfCommentGetGuid")]
        private static extern IntPtr CommentGetGuid_(IntPtr comment);
        public static string CommentGetGuid(IntPtr comment) { return PtrToString(CommentGetGuid_(comment)); }

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

        [DllImport(DLL, EntryPoint = "bcfViewPointIterate")]
        public static extern IntPtr ViewPointIterate(IntPtr topic, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfViewPointAdd")]
        public static extern IntPtr ViewPointAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? guid = null);

        [DllImport(DLL, EntryPoint = "bcfViewPointRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointRemove(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetGuid")]
        private static extern IntPtr ViewPointGetGuid_(IntPtr viewPoint);

        public static string ViewPointGetGuild(IntPtr viewPoint) { return PtrToString(ViewPointGetGuid_(viewPoint)); }

        [DllImport(DLL, EntryPoint = "bcfViewPointGetSnapshot")]
        private static extern IntPtr ViewPointGetSnapshot_(IntPtr viewPoint);

        public static string ViewPointGetSnapshot(IntPtr viewPoint) { return PtrToString(ViewPointGetSnapshot_(viewPoint)); }

        [DllImport(DLL, EntryPoint = "bcfViewPointGetDefaultVisibility")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetDefaultVisibility(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetSpaceVisible")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetSpaceVisible(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetSpaceBoundariesVisible")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetSpaceBoundariesVisible(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetOpeningsVisible")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetOpeningsVisible(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetCameraType")]
        public static extern BCFCamera ViewPointGetCameraType(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetCameraViewPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetCameraViewPoint(IntPtr viewPoint, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetCameraDirection")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetCameraDirection(IntPtr viewPoint, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetCameraUpVector")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointGetCameraUpVector(IntPtr viewPoint, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetViewToWorldScale")]
        public static extern double ViewPointGetViewToWorldScale(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetFieldOfView")]
        public static extern double ViewPointGetFieldOfView(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointGetAspectRatio")]
        public static extern double ViewPointGetAspectRatio(IntPtr viewPoint);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetSnapshot")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetSnapshot(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetDefaultVisibility")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetDefaultVisibility(IntPtr viewPoint, bool val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetSpaceVisible")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetSpaceVisible(IntPtr viewPoint, bool val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetSpaceBoundariesVisible")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetSpaceBoundariesVisible(IntPtr viewPoint, bool val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetOpeningsVisible")]
        [return: MarshalAs(UnmanagedType.U1)] public static extern bool ViewPointSetOpeningsVisible(IntPtr viewPoint, bool val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetCameraType")]
        [return: MarshalAs(UnmanagedType.U1)] public static extern bool ViewPointSetCameraType(IntPtr viewPoint, BCFCamera val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetCameraViewPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetCameraViewPoint(IntPtr viewPoint, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetCameraDirection")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetCameraDirection(IntPtr viewPoint, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetCameraUpVector")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetCameraUpVector(IntPtr viewPoint, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetViewToWorldScale")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetViewToWorldScale(IntPtr viewPoint, double val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetFieldOfView")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetFieldOfView(IntPtr viewPoint, double val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSetAspectRatio")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointSetAspectRatio(IntPtr viewPoint, double val);

        [DllImport(DLL, EntryPoint = "bcfViewPointSelectionIterate")]
        public static extern IntPtr ViewPointSelectionIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfViewPointSelectionAdd")]
        public static extern IntPtr ViewPointSelectionAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? ifcGuid = null);

        [DllImport(DLL, EntryPoint = "bcfViewComponentRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointComponentRemove(IntPtr component);

        [DllImport(DLL, EntryPoint = "bcfViewPointExceptionsIterate")]
        public static extern IntPtr ViewPointExceptionsIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfViewPointExceptionsAdd")]
        public static extern IntPtr ViewPointExceptionsAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? ifcGuid = null);

        [DllImport(DLL, EntryPoint = "bcfComponentGetIfcGuid")]
        private static extern IntPtr ComponentGetIfcGuid_(IntPtr component);
        public static string ComponentGetIfcGuid(IntPtr component) { return PtrToString(ComponentGetIfcGuid_(component)); }

        [DllImport(DLL, EntryPoint = "bcfComponentGetOriginatingSystem")]
        private static extern IntPtr ComponentGetOriginatingSystem_(IntPtr component);
        public static string ComponentGetOriginatingSystem(IntPtr component) { return PtrToString(ComponentGetOriginatingSystem_(component)); }

        [DllImport(DLL, EntryPoint = "bcfComponentGetAuthoringToolId")]
        private static extern IntPtr ComponentGetAuthoringToolId_(IntPtr component);
        public static string ComponentGetAuthoringToolId(IntPtr component) { return PtrToString(ComponentGetAuthoringToolId_(component)); }

        [DllImport(DLL, EntryPoint = "bcfComponentSetIfcGuid")]
        public static extern bool ComponentSetIfcGuid(IntPtr component, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(DLL, EntryPoint = "bcfComponentSetOriginatingSystem")]
        public static extern bool ComponentSetOriginatingSystem(IntPtr component, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(DLL, EntryPoint = "bcfComponentSetAuthoringToolId")]
        public static extern bool ComponentSetAuthoringToolId(IntPtr component, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceIterate")]
        public static extern IntPtr DocumentReferenceIterate(IntPtr topic, IntPtr prev);
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceAdd")]
        public static extern IntPtr DocumentReferenceAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath, [param: MarshalAs(UnmanagedType.U1)] bool isExternal, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? guid = null);
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceRemove")]
        public static extern bool DocumentReferenceRemove(IntPtr comment);

        /// <summary>
        ///
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceGetGuid")]
        private static extern IntPtr _DocumentReferenceGetGuid(IntPtr documentReferece); public static string DocumentReferenceGetGuid(IntPtr documentReference) { return PtrToString(_DocumentReferenceGetGuid(documentReference)); }
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceGetFilePath")]
        private static extern IntPtr _DocumentReferenceGetUrlPath(IntPtr documentReferece); public static string DocumentReferenceGetFilePath(IntPtr documentReference) { return PtrToString(_DocumentReferenceGetUrlPath(documentReference)); }
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceGetDescription")]
        private static extern IntPtr _DocumentReferenceGetDescription(IntPtr documentReferece); public static string DocumentReferenceGetDescription(IntPtr documentReference) { return PtrToString(_DocumentReferenceGetDescription(documentReference)); }

        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceSetFilePath")]
        public static extern bool DocumentReferenceSetFilePath(IntPtr documentReferece, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value, [param: MarshalAs(UnmanagedType.U1)] bool isExternal);
        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceSetDescription")]
        public static extern bool DocumentReferenceSetDescription(IntPtr documentReferece, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(DLL, EntryPoint = "bcfTopicGetBimSnippet")]
        public static extern IntPtr TopicGetBimSnippet(IntPtr topic, [param: MarshalAs(UnmanagedType.U1)] bool forceCreate);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetRemove(IntPtr snippet);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetGetSnippetType")]
        private static extern IntPtr _BimSnippetGetSnippetType(IntPtr snippet);
        public static string BimSnippetGetSnippetType(IntPtr snippet) { return PtrToString(_BimSnippetGetSnippetType(snippet)); }

        [DllImport(DLL, EntryPoint = "bcfBimSnippetGetReference")]
        private static extern IntPtr _BimSnippetGetReference(IntPtr snippet);
        public static string BimSnippetGetReference(IntPtr snippet) { return PtrToString(_BimSnippetGetReference(snippet)); }

        [DllImport(DLL, EntryPoint = "bcfBimSnippetGetReferenceSchema")]
        private static extern IntPtr _BimSnippetGetReferenceSchema(IntPtr snippet);
        public static string BimSnippetGetReferenceSchema(IntPtr snippet) { return PtrToString(_BimSnippetGetReferenceSchema(snippet)); }

        [DllImport(DLL, EntryPoint = "bcfBimSnippetGetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetGetIsExternal(IntPtr snippet);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetSetReference")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetSetReference(IntPtr snippet, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetSetReferenceSchema")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetSetReferenceSchema(IntPtr snippet, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetSetSnippetType")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetSetSnippetType(IntPtr snippet, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimSnippetSetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimSnippetSetIsExternal(IntPtr snippet, [param: MarshalAs(UnmanagedType.U1)] bool val);

        [DllImport(DLL, EntryPoint = "bcfReferenceLinkAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ReferenceLinkAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfReferenceLinkIterate")]
        private static extern IntPtr _ReferenceLinkIterate(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? prev);
        public static string ReferenceLinkIterate(IntPtr topic, string? prev) { return PtrToString(_ReferenceLinkIterate(topic, prev)); }

        [DllImport(DLL, EntryPoint = "bcfReferenceLinkRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ReferenceLinkRemove(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfLabelAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LabelAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfLabelIterate")]
        private static extern IntPtr _LabelIterate(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? prev);
        public static string LabelIterate(IntPtr topic, string? prev) { return PtrToString(_LabelIterate(topic, prev)); }

        [DllImport(DLL, EntryPoint = "bcfLabelRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LabelRemove(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RelatedTopicAdd(IntPtr topic, IntPtr related);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicIterate")]
        public static extern IntPtr RelatedTopicIterate(IntPtr topic, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RelatedTopicRemove(IntPtr topic, IntPtr related);

        [DllImport(DLL, EntryPoint = "bcfBitmapAdd")]
        public static extern IntPtr BitmapAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath, BCFBitmapFormat format, BCFPoint location, BCFPoint normal, BCFPoint up, double height);

        [DllImport(DLL, EntryPoint = "bcfBitmapIterate")]
        public static extern IntPtr BitmapIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfBitmapRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapRemove(IntPtr bitmap);

        /// <summary>
        ///
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfBitmapGetFormat")]
        public static extern BCFBitmapFormat BitmapGetFormat(IntPtr bitmap);

        [DllImport(DLL, EntryPoint = "bcfBitmapGetReference")]
        private static extern IntPtr _BitmapGetReference(IntPtr bitmap);
        public static string BitmapGetReference(IntPtr bitmap) { return PtrToString(_BitmapGetReference(bitmap)); }

        [DllImport(DLL, EntryPoint = "bcfBitmapGetLocation")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapGetLocation(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfBitmapGetNormal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapGetNormal(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfBitmapGetUp")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapGetUp(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfBitmapGetHeight")]
        public static extern double BitmapGetHeight(IntPtr bitmap);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetFormat")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetFormat(IntPtr bitmap, BCFBitmapFormat val);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetReference")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetReference(IntPtr bitmap, [MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetLocation")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetLocation(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetNormal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetNormal(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetUp")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetUp(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfBitmapSetHeight")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BitmapSetHeight(IntPtr bitmap, double val);

        /// <summary>
        ///
        /// </summary>
        [DllImport(DLL, EntryPoint = "bcfColoringAdd")]
        public static extern IntPtr ColoringAdd(IntPtr viewPoint, string? color);

        [DllImport(DLL, EntryPoint = "bcfColoringIterate")]
        public static extern IntPtr ColoringIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfColoringRemove")]
        public static extern bool ColoringRemove(IntPtr color);

        [DllImport(DLL, EntryPoint = "bcfColoringGetColor")]
        private static extern IntPtr _ColoringGetColor(IntPtr coloring);
        public static string ColoringGetColor(IntPtr coloring) { return PtrToString(_ColoringGetColor(coloring)); }

        [DllImport(DLL, EntryPoint = "bcfColoringSetColor")]
        public static extern bool ColoringSetColor(IntPtr coloring, string color);

        [DllImport(DLL, EntryPoint = "bcfColoringComponentAdd")] 
        public static extern IntPtr ColoringComponentAdd(IntPtr coloring, string? ifcGuid);

        [DllImport(DLL, EntryPoint = "bcfColoringComponentIterate")] 
        public static extern IntPtr ColoringComponentIterate(IntPtr coloring, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfLineAdd")]
        public static extern IntPtr LineAdd(IntPtr viewPoint, BCFPoint start, BCFPoint end);

        [DllImport(DLL, EntryPoint = "bcfLineIterate")]
        public static extern IntPtr LineIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfLineRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LineRemove(IntPtr line);

        [DllImport(DLL, EntryPoint = "bcfLineGetStartPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LineGetStartPoint(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfLineGetEndPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LineGetEndPoint(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfLineSetStartPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LineSetStartPoint(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfLineSetEndPoint")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LineSetEndPoint(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneAdd")]
        public static extern IntPtr ClippingPlaneAdd(IntPtr viewPoint, BCFPoint location, BCFPoint direction);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneIterate")]
        public static extern IntPtr ClippingPlaneIterate(IntPtr viewPoint, IntPtr prev);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ClippingPlaneRemove(IntPtr clippingPlane);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneGetLocation")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ClippingPlaneGetLocation(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneGetDirection")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ClippingPlaneGetDirection(IntPtr bitmap, out BCFPoint retPt);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneSetLocation")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ClippingPlaneSetLocation(IntPtr bitmap, BCFPoint pt);

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneSetDirection")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ClippingPlaneSetDirection(IntPtr bitmap, BCFPoint pt);
    }
}
