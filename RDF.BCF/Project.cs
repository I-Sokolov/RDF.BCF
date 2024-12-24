namespace RDF.BCF
{
    public class Project : IDisposable
    {
        /// <summary>
        /// Creates new empty BCF data
        /// </summary>
        /// <param name="currentUser">
        /// User must be set if you are going to create or modify topics.
        /// </param>
        /// <param name="autoExtent">
        /// Enabling the option will automatically add to extensions enumerations new user, 
        /// topic type, status etc. when you set the value which is not in enumeration yet.
        /// If the option is disable, it makes strict checking and assigning any unknown elements will rise RDF.BCF.Exception.
        /// </param>
        public Project(string? currentUser = null, bool autoExtent = false, string? projectId = null)
        {
            m_handle = BCF.Native.CreateProject(currentUser, autoExtent, projectId);
            m_topics = new Topics(this);
            m_extensions = new Extensions(this);
        }

        /// <summary>
        /// Native handler of BCF project to use in Native.* calls
        /// </summary>
        public IntPtr Handle { get { return m_handle; } }

        /// <summary>
        /// Get errors since last call of ClearErrors or since project creation
        /// </summary>
        public string GetErrors(bool cleanLog = true)
        {
            return BCF.Native.GetErrors(m_handle, cleanLog);
        }

        /// <summary>
        /// Reads BCF data from given BCF XML file.
        /// Data can be modified after reading.
        /// </summary>
        public bool ReadFile(string filePath)
        {
            return BCF.Native.ReadFile(m_handle, filePath);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteFile(string filePath, Native.Version version = BCF.Native.Version._3_0)
        {
            return BCF.Native.WriteFile(m_handle, filePath, version);
        }

        /// <summary>
        /// ProjectId is required property
        /// </summary>
        public string ProjectId { get { return BCF.Native.GetProjectId(m_handle); } }

        /// <summary>
        /// Name is optional property
        /// </summary>
        public string Name { get { return BCF.Native.GetProjectName(m_handle); } set { BCF.Native.SetProjectName(m_handle, value); } }

        /// <summary>
        /// BCF data are mainly list of topics, enumerate or modify topics with the property
        /// </summary>
        public Topics Topics { get { return m_topics; } }

        /// <summary> 
        /// Manage BCF schema extensions
        /// </summary>
        public Extensions Extensions { get {return m_extensions;} }


        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        private IntPtr m_handle = IntPtr.Zero;
        private Extensions m_extensions;
        private Topics m_topics;

        ~Project()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (m_handle != IntPtr.Zero)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                var remainedErrors = RDF.BCF.Native.GetErrors(m_handle);
                System.Diagnostics.Trace.Assert(remainedErrors.Length == 0);

                RDF.BCF.Native.DeleteProject(m_handle);

                m_handle = IntPtr.Zero;
            }
        }


        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IMPLEMENTATION
    }
}
