namespace RDF.BCF
{
    public class Project : IDisposable
    {
        /// <summary>
        /// Creates new empty BCF data
        /// </summary>
        public Project()
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            m_bcfProject = Native.OpenProject();
            m_topics = new Topics(this);
            m_extensions = new Extensions(this);
        }

        /// <summary>
        /// Enabling the option will automatically add to extensions enumerations new user, 
        /// topic type, status etc. when you set the value which is not in enumeration yet.
        /// If the option is disable, it makes strict checking and assigning any unknown elements will rise RDF.BCF.Exception.
        /// </summary>
        public bool AutoExtentSchema { get; set; }

        /// <summary>
        /// User must be set if you are going to create or modify topic.
        /// </summary>
        public string? CurrentUser { get; set; }

        /// <summary>
        /// Populates BCF data with some preset data
        /// </summary>
        public bool InitNew()
        {
            return Native.InitNew(m_bcfProject);
        }

        /// <summary>
        /// Reads BCF data from given BCF XML file.
        /// Data can be modified after reading.
        /// </summary>
        public bool ReadFile(string filePath)
        {
            return Native.ReadFile(m_bcfProject, filePath);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteFile(string filePath, Native.Version version = Native.Version._3_0)
        {
            return Native.WriteFile(m_bcfProject, filePath, version);
        }

        /// <summary>
        /// ProjectId is required property
        /// </summary>
        public string ProjectId { get { return ""; } set { } }

        /// <summary>
        /// Name is optional property
        /// </summary>
        public string? Name { get { return null; } set { } }

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
        private IntPtr m_bcfProject = IntPtr.Zero;
        private Extensions m_extensions;
        private Topics m_topics;

        ~Project()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (m_bcfProject != IntPtr.Zero)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                RDF.BCF.Native.CloseProject(m_bcfProject);
                m_bcfProject = IntPtr.Zero;
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
