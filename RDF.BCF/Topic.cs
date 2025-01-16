using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Topic
    {
        /// <summary>
        /// Read-only persistent topic identifier
        /// </summary>
        public string Guid { get { return Interop.TopicGetGuid(m_handle); } }

        /// <summary>
        /// Topic attributes
        /// </summary>
        public string ServerAssignedId { get { return Interop.TopicGetServerAssignedId(m_handle); } set { Interop.TopicSetServerAssignedId(m_handle, value); } }
        public string TopicStatus { get { return Interop.TopicGetTopicStatus(m_handle); } set { Interop.TopicSetTopicStatus(m_handle, value); } }
        public string TopicType { get { return Interop.TopicGetTopicType(m_handle); } set { Interop.TopicSetTopicType(m_handle, value); } }
        public string Title { get { return Interop.TopicGetTitle(m_handle); } set { Interop.TopicSetTitle(m_handle, value); } }
        public string Priority { get { return Interop.TopicGetPriority(m_handle); } set { Interop.TopicSetPriority(m_handle, value); } }
        public string CreationDate { get { return Interop.TopicGetCreationDate(m_handle); } }
        public string CreationAuthor { get { return Interop.TopicGetCreationAuthor(m_handle); } }
        public string ModifiedDate { get { return Interop.TopicGetModifiedDate(m_handle); } }
        public string ModifiedAuthor { get { return Interop.TopicGetModifiedAuthor(m_handle); } }
        public string DueDate { get { return Interop.TopicGetDueDate(m_handle); } set { Interop.TopicSetDueDate(m_handle, value); } }
        public string AssignedTo { get { return Interop.TopicGetAssignedTo(m_handle); } set { Interop.TopicSetAssignedTo(m_handle, value); } }
        public string Description { get { return Interop.TopicGetDescription(m_handle); } set { Interop.TopicSetDescription(m_handle, value); } }
        public string Stage { get { return Interop.TopicGetStage(m_handle); } set { Interop.TopicSetStage(m_handle, value); } }
        public int Index { get { return Interop.TopicGetIndex(m_handle); } set { Interop.TopicSetIndex(m_handle, value); } }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove()
        {
            return Interop.TopicRemove(m_handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<BimFile> Files { get { return GetFiles(); } }

        public BimFile AddFile(string? filePath, bool isExternal = true)
        {
            IntPtr fileHandle = Interop.FileAdd(m_handle, filePath, isExternal);
            if (fileHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to add file: " + Interop.ErrorsGet(m_project.Handle));
            return new BimFile(this, fileHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ViewPoint> ViewPoints { get { return GetViewPoints(); } }

        public ViewPoint AddViewPoint(string? guid = null)
        {
            IntPtr vpHandle = Interop.ViewPointAdd(m_handle, guid);
            if (vpHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create view point: " + Interop.ErrorsGet(m_project.Handle));
            return new ViewPoint(this, vpHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<DocumentReference> GetDocumentReferences()
        {
            var ret = new List<DocumentReference>();
            IntPtr handle = IntPtr.Zero;
            while ((handle = Interop.DocumentReferenceIterate(m_handle, handle)) != IntPtr.Zero)
            {
                ret.Add(new DocumentReference(this, handle));
            }
            return ret;
        }

        public DocumentReference AddDocumentRefernce(string urlPath, string? guid = null)
        {
            IntPtr vpHandle = Interop.DocumentReferenceAdd(m_handle, urlPath, guid);
            if (vpHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create document reference: " + Interop.ErrorsGet(m_project.Handle));
            return new DocumentReference(this, vpHandle);
        }

        /// <summary>
        /// The topic comments
        /// </summary>
        public List<Comment> Comments { get { return GetComments(); } }

        /// <summary>
        /// 
        /// </summary>
        public Comment AddComment(string? guid = null)
        {
            IntPtr commentHandle = Interop.CommentAdd(m_handle, guid);
            if (commentHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create comment: " + Interop.ErrorsGet(m_project.Handle));
            return new Comment(this, commentHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        public BimSnippet? GetBimSnippet(bool forceCreate)
        {
            IntPtr handle = Interop.TopicGetBimSnippet(m_handle, forceCreate);
            if (handle == IntPtr.Zero)
                return null;
            else
                return new BimSnippet(this, handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public Project Project { get { return m_project; } }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        IntPtr m_handle;
        
        internal Topic(Project project, IntPtr handle) 
        {
            m_project = project;
            m_handle = handle;
        }

        private List<BimFile> GetFiles()
        {
            var ret = new List<BimFile>();

            IntPtr handle = IntPtr.Zero;

            while ((handle = Interop.FileIterate(m_handle, handle)) != IntPtr.Zero)
            {
                ret.Add(new BimFile(this, handle));
            }

            return ret;
        }

        private List<Comment> GetComments()
        {
            var ret = new List<Comment>();

            IntPtr commentHandle = IntPtr.Zero;

            while ((commentHandle = Interop.CommentIterate(m_handle, commentHandle)) != IntPtr.Zero)
            {
                ret.Add(new Comment(this, commentHandle));
            }

            return ret;
        }

        private List<ViewPoint> GetViewPoints()
        {
            var ret = new List<ViewPoint>();

            IntPtr viewPoint = IntPtr.Zero;

            while ((viewPoint = Interop.ViewPointIterate(m_handle, viewPoint)) != IntPtr.Zero)
            {
                ret.Add(new ViewPoint(this, viewPoint));
            }

            return ret;
        }

        #endregion IMPLEMENTATION
    }
}
