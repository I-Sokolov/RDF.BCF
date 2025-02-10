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
        /// Guid of the topic, in lowercase.
        /// </summary>
        public string Guid { get { return Interop.TopicGetGuid(m_handle); } }

        /// <summary>
        /// A server controlled, user friendly and project-unique issue identifier. Clients provided values will be disregarded by the server.
        /// </summary>
        public string ServerAssignedId { get { return Interop.TopicGetServerAssignedId(m_handle); } set { Interop.TopicSetServerAssignedId(m_handle, value); } }

        /// <summary>
        /// Type of the topic (Predefined list in extensions).
        /// </summary>
        public string TopicStatus { get { return Interop.TopicGetTopicStatus(m_handle); } set { Interop.TopicSetTopicStatus(m_handle, value); } }

        /// <summary>
        /// Type of the topic (Predefined list in extensions_.
        /// </summary>
        public string TopicType { get { return Interop.TopicGetTopicType(m_handle); } set { Interop.TopicSetTopicType(m_handle, value); } }

        /// <summary>
        /// Title of the topic.
        /// </summary>
        public string Title { get { return Interop.TopicGetTitle(m_handle); } set { Interop.TopicSetTitle(m_handle, value); } }

        /// <summary>
        /// Topic priority (Predefined list in extensions).
        /// </summary>
        public string Priority { get { return Interop.TopicGetPriority(m_handle); } set { Interop.TopicSetPriority(m_handle, value); } }

        /// <summary>
        /// Date until when the topics issue needs to be resolved.
        /// </summary>
        public string DueDate { get { return Interop.TopicGetDueDate(m_handle); } set { Interop.TopicSetDueDate(m_handle, value); } }

        /// <summary>
        /// The user to whom this topic is assigned to. Recommended to be in email format (Predefined list in extensions).
        /// </summary>
        public string AssignedTo { get { return Interop.TopicGetAssignedTo(m_handle); } set { Interop.TopicSetAssignedTo(m_handle, value); } }

        /// <summary>
        /// Description of the topic.
        /// </summary>
        public string Description { get { return Interop.TopicGetDescription(m_handle); } set { Interop.TopicSetDescription(m_handle, value); } }

        /// <summary>
        /// Stage this topic is part of (Predefined list in extensions).
        /// </summary>
        public string Stage { get { return Interop.TopicGetStage(m_handle); } set { Interop.TopicSetStage(m_handle, value); } }

        /// <summary>
        /// Date when the topic was created.
        /// </summary>
        public string CreationDate { get { return Interop.TopicGetCreationDate(m_handle); } }

        /// <summary>
        /// User who created the topic.
        /// </summary>
        public string CreationAuthor { get { return Interop.TopicGetCreationAuthor(m_handle); } }

        /// <summary>
        /// Date when the topic was last modified. Exists only when Topic has been modified after creation.
        /// </summary>
        public string ModifiedDate { get { return Interop.TopicGetModifiedDate(m_handle); } }

        /// <summary>
        /// User who modified the topic. Exists only when Topic has been modified after creation.
        /// </summary>
        public string ModifiedAuthor { get { return Interop.TopicGetModifiedAuthor(m_handle); } }

        /// <summary>
        /// Number to maintain the order of the topics. This property is deprecated and will be removed in a future release.
        /// </summary>
        public int Index { get { return Interop.TopicGetIndex(m_handle); } set { Interop.TopicSetIndex(m_handle, value); } }

        /// <summary>
        /// List of references to the topic, for example, a work request management system or an URI to a model.
        /// </summary>
        public List<string> GetReferenceLinks()
        {
            var list = new List<string>();
            string elem = "";
            UInt16 ind = 0;
            while (!string.IsNullOrEmpty(elem = Interop.ReferenceLinkGetAt(m_handle, ind++)))
            {
                list.Add(elem);
            }
            return list;
        }

        /// <summary>
        /// List of references to the topic, for example, a work request management system or an URI to a model.
        /// </summary>
        public void SetReferenceLinks(IEnumerable<string> list)
        {
            string elem = "";
            while (!string.IsNullOrEmpty(elem = Interop.ReferenceLinkGetAt(m_handle, 0)))
            {
                Interop.ReferenceLinkRemove(m_handle, elem);
            }
            foreach (var e in list)
            {
                Interop.ReferenceLinkAdd(m_handle, e);
            }
        }

        /// <summary>
        /// Tags for grouping Topics (Predefined list in extensions).
        /// </summary>
        public List<string> GetLabels()
        {
            var list = new List<string>();
            string elem = "";
            UInt16 ind = 0;
            while (!string.IsNullOrEmpty (elem = Interop.LabelGetAt(m_handle, ind++)))
            {
                list.Add(elem);
            }
            return list;
        }

        /// <summary>
        /// Tags for grouping Topics (Predefined list in extensions).
        /// </summary>
        public void SetLabels(IEnumerable<string> list)
        {
            string elem = "";
            while (!string.IsNullOrEmpty (elem = Interop.LabelGetAt(m_handle, 0)))
            {
                Interop.LabelRemove(m_handle, elem);
            }
            foreach (var e in list)
            {
                Interop.LabelAdd(m_handle, e);
            }
        }

        /// <summary>
        /// List of referenced topics
        /// </summary>
        public List<Topic> GetRelatedTopics()
        {
            var list = new List<Topic>();
            IntPtr elem = IntPtr.Zero;
            UInt16 ind = 0;
            while (IntPtr.Zero != (elem = Interop.RelatedTopicGetAt(m_handle, ind++)))
            {
                list.Add(new Topic(m_project, elem));
            }
            return list;
        }

        /// <summary>
        /// List of referenced topics
        /// </summary>
        public void SetRelatedTopics(IEnumerable<Topic> list)
        {
            IntPtr elem = IntPtr.Zero;
            while (IntPtr.Zero != (elem = Interop.RelatedTopicGetAt(m_handle, 0)))
            {
                Interop.RelatedTopicRemove(m_handle, elem);
            }
            foreach (var e in list)
            {
                Interop.RelatedTopicAdd(m_handle, e.m_handle);
            }
        }

        /// <summary>
        /// Remove object from BCF package. See Memory Management in documentation.
        /// </summary>
        public bool Remove()
        {
            return Interop.TopicRemove(m_handle);
        }

        /// <summary>
        /// BIM files relevant to this topic.
        /// </summary>
        public List<BimFile> GetFiles()
        {
            var ret = new List<BimFile>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.FileGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new BimFile(this, handle));
            }
            return ret;
        }

        /// <summary>
        /// Add BIM (preferable IFC) file relevant to this topic.
        /// </summary>
        public BimFile AddFile(string? filePath, bool isExternal = true)
        {
            IntPtr fileHandle = Interop.FileAdd(m_handle, filePath, isExternal);
            if (fileHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to add file: " + Interop.GetErrors(m_project.Handle));
            return new BimFile(this, fileHandle);
        }

        /// <summary>
        /// viewpoints related to one or more comments.
        /// </summary>
        public List<ViewPoint> GetViewPoints()
        {
            var ret = new List<ViewPoint>();
            IntPtr viewPoint = IntPtr.Zero;
            UInt16 ind = 0;
            while ((viewPoint = Interop.ViewPoitGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new ViewPoint(this, viewPoint));
            }
            return ret;
        }

        /// <summary>
        /// add viewpoint related to one or more comments.
        /// </summary>
        public ViewPoint AddViewPoint(string? guid = null)
        {
            IntPtr vpHandle = Interop.ViewPointAdd(m_handle, guid);
            if (vpHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create view point: " + Interop.GetErrors(m_project.Handle));
            return new ViewPoint(this, vpHandle);
        }

        /// <summary>
        /// Documents associated with the topics
        /// </summary>
        public List<DocumentReference> GetDocumentReferences()
        {
            var ret = new List<DocumentReference>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.DocumentReferenceGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new DocumentReference(this, handle));
            }
            return ret;
        }

        /// <summary>
        /// Add documents associated with the topics
        /// </summary>
        public DocumentReference AddDocumentRefernce(string filePath, bool isExternal = true, string? guid = null)
        {
            IntPtr vpHandle = Interop.DocumentReferenceAdd(m_handle, filePath, isExternal, guid);
            if (vpHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create document reference: " + Interop.GetErrors(m_project.Handle));
            return new DocumentReference(this, vpHandle);
        }

        /// <summary>
        /// Comments related to the topic.
        /// </summary>
        public List<Comment> GetComments()
        {
            var ret = new List<Comment>();
            IntPtr commentHandle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((commentHandle = Interop.CommentGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Comment(this, commentHandle));
            }
            return ret;
        }

        /// <summary>
        /// Add comments related to the topic.
        /// </summary>
        public Comment AddComment(string? guid = null)
        {
            IntPtr commentHandle = Interop.CommentAdd(m_handle, guid);
            if (commentHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create comment: " + Interop.GetErrors(m_project.Handle));
            return new Comment(this, commentHandle);
        }

        /// <summary>
        /// BimSnippet is an additional file containing information related to one or multiple topics. 
        /// For example, it can be an IFC file containing provisions for voids.
        /// </summary>
        public BimSnippet? GetBimSnippet(bool forceCreate)
        {
            IntPtr handle = Interop.TopicGetBimSnippet(m_handle, forceCreate);
            if (handle == IntPtr.Zero)
                return null;
            else
                return new BimSnippet(this, handle);
        }

        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        IntPtr m_handle;

        internal Project Project { get { return m_project; } }

        internal Topic(Project project, IntPtr handle) 
        {
            m_project = project;
            m_handle = handle;
        }

        #endregion INTERNAL
    }
}
