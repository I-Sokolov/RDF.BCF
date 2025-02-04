namespace RDF.BCF
{
    public class Project : IDisposable
    {
        /// <summary>
        /// Creates new empty BCF data.
        /// </summary>
        public Project(string? projectId = null)
        {
            m_handle = BCF.Interop.ProjectCreate(projectId);
        }

        /// <summary>
        /// Get errors since last call of ClearErrors or since project creation
        /// </summary>
        public string GetErrors(bool cleanLog = true)
        {
            return BCF.Interop.GetErrors(m_handle, cleanLog);
        }

        /// <summary>
        /// Reads BCF data from given BCF XML file.
        /// Data can be modified after reading.
        /// </summary>
        public bool FileRead(string filePath, bool autofix)
        {
            return BCF.Interop.FileRead(m_handle, filePath, autofix);
        }

        /// <summary>
        /// Set author who will create or modify BCF data (not required if you only read)
        /// </summary>
        public bool FileWrite(string filePath, Interop.Version version = BCF.Interop.Version._3_0)
        {
            return BCF.Interop.FileWrite(m_handle, filePath, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">
        /// User must be set if you are going to create or modify topics.
        /// </param>
        /// <param name="autoExtent">
        /// Enabling the option will automatically add to extensions enumerations new user, 
        /// topic type, status etc. when you set the value which is not in enumeration yet.
        /// If the option is disable, it makes strict checking and assigning any unknown elements will rise RDF.BCF.Exception.
        /// </param>
        public bool SetAuthor(string user, bool autoExtent)
        {
            return BCF.Interop.SetAuthor(m_handle, user, autoExtent);
        }

        /// <summary>
        /// ProjectId is required property
        /// </summary>
        public string ProjectId { get { return BCF.Interop.ProjectIdGet(m_handle); } }

        /// <summary>
        /// Name is optional property
        /// </summary>
        public string Name { get { return BCF.Interop.ProjectNameGet(m_handle); } set { BCF.Interop.ProjectNameSet(m_handle, value); } }

        /// <summary>
        /// BCF is the list of topics
        /// </summary>
        public List<Topic> GetTopics()
        {
            var ret = new List<Topic>();
            IntPtr topicHandle = IntPtr.Zero;
            while ((topicHandle = RDF.BCF.Interop.TopicsIterate(m_handle, topicHandle)) != IntPtr.Zero)
            {
                ret.Add(new Topic(this, topicHandle));
            }
            return ret;
        }

        /// <summary>
        /// Creates new topic.
        /// Caller can assign GUID or it will generated automatically, GUID never changes after creation
        /// </summary>
        public Topic AddTopic(string type, string title, string status, string? guid = null)
        {
            IntPtr topicHandle = Interop.TopicAdd(m_handle, type, title, status, guid);
            if (topicHandle == IntPtr.Zero)
                throw new ApplicationException("Fail to create topic: " + Interop.GetErrors(Handle));
            return new Topic(this, topicHandle);
        }

        /// <summary> 
        /// Manage BCF schema extensions
        /// </summary>
        public Extensions Extensions { get {return new Extensions(this);} }


        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        private IntPtr m_handle = IntPtr.Zero;

        internal IntPtr Handle { get { return m_handle; } } 

        ~Project()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (m_handle != IntPtr.Zero)
            {
                var remainedErrors = RDF.BCF.Interop.GetErrors(m_handle);
                System.Diagnostics.Trace.Assert(remainedErrors.Length == 0);

                if (!RDF.BCF.Interop.ProjectDelete(m_handle))
                {
                    var errors = RDF.BCF.Interop.GetErrors(m_handle);
                    System.Diagnostics.Trace.Assert(false);
                }

                m_handle = IntPtr.Zero;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion INTERNAL
    }
}
