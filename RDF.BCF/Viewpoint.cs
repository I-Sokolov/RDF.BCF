using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class ViewPoint
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Camera 
        {
            Perspective,
            Orthogonal
        }

        /// <summary>
        /// 
        /// </summary>
        public struct Point  {  public double x; public double y; public double z; }

        /// <summary>
        /// Read-only persistent viewpoint identifier
        /// </summary>
        public Guid Guid { get { return m_guid; } }

        /// <summary>
        /// When true, all components should be visible unless listed in the exceptions
        /// When false all components should be invisible unless listed in the exceptions
        /// Defaults to false
        /// </summary>
        public bool DefaultVisibility { get; set; }

        /// <summary>
        /// A list of IfcGuids of components to hide when DefaultVisibility=true or to show when DefaultVisibility=false
        /// </summary>
        public IEnumerable<string> Exceptions { get; set; }

        /// <summary>
        /// Same as DefaultVisibility but restricted to spaces only
        /// </summary>
        public bool SpacesVisible { get; set; }

        /// <summary>
        /// Same as DefaultVisibility but restricted to space boundaries only
        /// </summary>
        public bool SpaceBoundariesVisible {  get; set; }

        /// <summary>
        /// Same as DefaultVisibility but restricted to openings only
        /// </summary>
        public bool OpeningsVisible { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Camera CameraType { get; set; }

        /// <summary>
        /// Camera location
        /// </summary>
        public Point CameraViewPoint {  get; set; }

        /// <summary>
        /// Camera direction
        /// </summary>
        public Point CameraDirection { get; set; }

        /// <summary>
        /// Camera up vector
        /// </summary>
        public Point CameraUpVector {  get; set; }

        /// <summary>
        /// For perspective camera: FieldOfView - The entire vertical field of view angle of the camera, expressed in degrees. Valid range 0 to 180 exclusive.
        /// For orthogonal camera: ViewToWorldScale - Vertical scaling from view to world  
        /// </summary>
        public double CameraRange { get; set; }

        /// <summary>
        /// Proportional relationship between the width and the height of the view(w/h). Assume 1.0 when reading previous BCF versions.
        /// </summary>
        public double AspectRatio { get; set; }

        /// <summary>
        /// Parameter for sorting
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Adds snapshot for the viewpoint.
        /// png or jpeg file path.
        /// The file will be copied to BCF data.
        /// Passing null will remove snapshot
        /// </summary>
        public void AddSnapshot(string? filePath) { }

        /// <summary>
        /// File path to viewpoint snapshot (png or jpeg)
        /// </summary>
        public string? Snapshot { get; }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Project m_project;
        Topic m_topic;
        Guid m_guid;

        internal ViewPoint(Project project, Topic topic, Guid? guid = null)
        {
            m_project = project;
            m_topic = topic;

            if (guid == null)
            {
                m_guid = Guid.NewGuid();
            }
            else
            {
                m_guid = guid.Value;
            }

            Exceptions = new List<string>();
        }

        #endregion IMPLEMENTATION
    }
}
