using System.Runtime.InteropServices;

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

        [DllImport(DLL, EntryPoint = "bcfProjectIsDirty")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ProjectIsDirty(IntPtr project);

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

        [DllImport(DLL, EntryPoint = "bcfSetOptions")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool SetOptions(IntPtr project, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string user, [param: MarshalAs(UnmanagedType.U1)] bool autoExtent = true, [param: MarshalAs(UnmanagedType.U1)] bool validateIfcGuids = false);

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

        [DllImport(DLL, EntryPoint = "bcfTopicGetAt")]
        public static extern IntPtr TopicsGetAt(IntPtr project, UInt16 ind);

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


        [DllImport(DLL, EntryPoint = "bcfBimFileGetAt")]
        public static extern IntPtr BimFileGetAt(IntPtr topic, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfBimFileAdd")]
        public static extern IntPtr BimFileAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? filePath, [param: MarshalAs(UnmanagedType.U1)] bool isExternal = true);

        [DllImport(DLL, EntryPoint = "bcfBimFileRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileRemove(IntPtr file);

        [DllImport(DLL, EntryPoint = "bcfBimFileGetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileGetIsExternal(IntPtr file);

        [DllImport(DLL, EntryPoint = "bcfBimFileGetFilename")]
        private static extern IntPtr _BimFileGetFilename(IntPtr file);
        public static string BimFileGetFilename(IntPtr file) { return PtrToString(_BimFileGetFilename(file)); }

        [DllImport(DLL, EntryPoint = "bcfBimFileGetDate")]
        private static extern IntPtr _BimFileGetDate(IntPtr file);
        public static string BimFileGetDate(IntPtr file) { return PtrToString(_BimFileGetDate(file)); }

        [DllImport(DLL, EntryPoint = "bcfBimFileGetReference")]
        private static extern IntPtr _BimFileGetReference(IntPtr file);
        public static string BimFileGetReference(IntPtr file) { return PtrToString(_BimFileGetReference(file)); }

        [DllImport(DLL, EntryPoint = "bcfBimFileGetIfcProject")]
        private static extern IntPtr _BimFileGetIfcProject(IntPtr file);
        public static string BimFileGetIfcProject(IntPtr file) { return PtrToString(_BimFileGetIfcProject(file)); }

        [DllImport(DLL, EntryPoint = "bcfBimFileGetIfcSpatialStructureElement")]
        private static extern IntPtr _BimFileGetIfcSpatialStructureElement(IntPtr file); 
        public static string BimFileGetIfcSpatialStructureElement(IntPtr file) { return PtrToString(_BimFileGetIfcSpatialStructureElement(file)); }

        [DllImport(DLL, EntryPoint = "bcfBimFileSetIsExternal")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetIsExternal(IntPtr file, bool val);

        [DllImport(DLL, EntryPoint = "bcfBimFileSetFilename")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetFilename(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimFileSetDate")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetDate(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimFileSetReference")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetReference(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimFileSetIfcProject")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetIfcProject(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfBimFileSetIfcSpatialStructureElement")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool BimFileSetIfcSpatialStructureElement(IntPtr file, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfCommentGetAt")]
        public static extern IntPtr CommentGetAt(IntPtr topic, UInt16 ind);

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

        [DllImport(DLL, EntryPoint = "bcfViewPointGetAt")]
        public static extern IntPtr ViewPoitGetAt(IntPtr topic, UInt16 ind);

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

        [DllImport(DLL, EntryPoint = "bcfViewPointSelectionGetAt")]
        public static extern IntPtr ViewPointSelectionGetAt(IntPtr viewPoint, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfViewPointSelectionAdd")]
        public static extern IntPtr ViewPointSelectionAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? ifcGuid = null);

        [DllImport(DLL, EntryPoint = "bcfViewComponentRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ViewPointComponentRemove(IntPtr component);

        [DllImport(DLL, EntryPoint = "bcfViewPointExceptionGetAt")]
        public static extern IntPtr ViewPointExceptionGetAt(IntPtr viewPoint, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfViewPointExceptionAdd")]
        public static extern IntPtr ViewPointExceptionAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string? ifcGuid = null);

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

        [DllImport(DLL, EntryPoint = "bcfDocumentReferenceGetAt")]
        public static extern IntPtr DocumentReferenceGetAt(IntPtr topic, UInt16 ind);

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

        [DllImport(DLL, EntryPoint = "bcfReferenceLinkGetAt")]
        private static extern IntPtr _ReferenceLinkGetAt(IntPtr topic, UInt16 ind);

        public static string ReferenceLinkGetAt(IntPtr topic, UInt16 ind) { return PtrToString(_ReferenceLinkGetAt(topic, ind)); }

        [DllImport(DLL, EntryPoint = "bcfReferenceLinkRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool ReferenceLinkRemove(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfLabelAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LabelAdd(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfLabelGetAt")]
        private static extern IntPtr _LabelGetAt(IntPtr topic, UInt16 ind);
        public static string LabelGetAt(IntPtr topic, UInt16 ind) { return PtrToString(_LabelGetAt(topic, ind)); }

        [DllImport(DLL, EntryPoint = "bcfLabelRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool LabelRemove(IntPtr topic, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string val);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicAdd")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RelatedTopicAdd(IntPtr topic, IntPtr related);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicGetAt")]
        public static extern IntPtr RelatedTopicGetAt(IntPtr topic, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfRelatedTopicRemove")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool RelatedTopicRemove(IntPtr topic, IntPtr related);

        [DllImport(DLL, EntryPoint = "bcfBitmapAdd")]
        public static extern IntPtr BitmapAdd(IntPtr viewPoint, [param: MarshalAs(UnmanagedType.LPUTF8Str)] string filePath, BCFBitmapFormat format, BCFPoint location, BCFPoint normal, BCFPoint up, double height);

        [DllImport(DLL, EntryPoint = "bcfBitmapGetAt")]
        public static extern IntPtr BitmapGetAt(IntPtr viewPoint, UInt16 ind);

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

        [DllImport(DLL, EntryPoint = "bcfColoringGetAt")]
        public static extern IntPtr ColoringGetAt(IntPtr viewPoint, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfColoringRemove")]
        public static extern bool ColoringRemove(IntPtr color);

        [DllImport(DLL, EntryPoint = "bcfColoringGetColor")]
        private static extern IntPtr _ColoringGetColor(IntPtr coloring);
        public static string ColoringGetColor(IntPtr coloring) { return PtrToString(_ColoringGetColor(coloring)); }

        [DllImport(DLL, EntryPoint = "bcfColoringSetColor")]
        public static extern bool ColoringSetColor(IntPtr coloring, string color);

        [DllImport(DLL, EntryPoint = "bcfColoringComponentAdd")] 
        public static extern IntPtr ColoringComponentAdd(IntPtr coloring, string? ifcGuid);

        [DllImport(DLL, EntryPoint = "bcfColoringComponentGetAt")] 
        public static extern IntPtr ColoringComponentGetAt(IntPtr coloring, UInt16 ind);

        [DllImport(DLL, EntryPoint = "bcfLineAdd")]
        public static extern IntPtr LineAdd(IntPtr viewPoint, BCFPoint start, BCFPoint end);

        [DllImport(DLL, EntryPoint = "bcfLineGetAt")]
        public static extern IntPtr LineGetAt(IntPtr viewPoint, UInt16 ind);

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

        [DllImport(DLL, EntryPoint = "bcfClippingPlaneGetAt")]
        public static extern IntPtr ClippingPlaneGetAt(IntPtr viewPoint, UInt16 ind);

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
