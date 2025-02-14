using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RDF.BCF.Interop;

namespace RDF.BCF
{
    public class ViewPoint
    {
        /// <summary>
        /// 
        /// </summary>
        public string Guid { get { return Interop.ViewPointGetGuild(m_handle); } }

        /// <summary>
        /// Absolute file path to the snapshot (jpeg or png). See Files Management in documentation.
        /// </summary>
        public string Snapshot { get { return Interop.ViewPointGetSnapshot(m_handle); } set { Interop.ViewPointSetSnapshot(m_handle, value); } }

        /// <summary>
        /// Defaults to false
        /// When true, all components should be visible unless listed in the exceptions
        /// When false all components should be invisible unless listed in the exceptions
        /// </summary>
        public bool DefaultVisibility { get { return Interop.ViewPointGetDefaultVisibility(m_handle); } set { Interop.ViewPointSetDefaultVisibility(m_handle, value); } }

        /// <summary>
        /// same as DefaultVisibility but restricted to spaces only
        /// </summary>
        public bool SpaceVisible { get { return Interop.ViewPointGetSpaceVisible(m_handle); } set { Interop.ViewPointSetSpaceVisible(m_handle, value); } }

        /// <summary>
        /// same as DefaultVisibility but restricted to space boundaries only
        /// </summary>
        public bool SpaceBoundariesVisible { get { return Interop.ViewPointGetSpaceBoundariesVisible(m_handle); } set { Interop.ViewPointSetSpaceBoundariesVisible(m_handle, value); } }

        /// <summary>
        /// same as DefaultVisibility but restricted to openings only
        /// </summary>
        public bool OpeningsVisible { get { return Interop.ViewPointGetOpeningsVisible(m_handle); } set { Interop.ViewPointSetOpeningsVisible(m_handle, value); } }

        /// <summary>
        /// The visualization information file must specify exactly one of either an orthogonal or a perspective camera.
        /// </summary>
        public Interop.BCFCamera CameraType { get { return Interop.ViewPointGetCameraType(m_handle); } set { Interop.ViewPointSetCameraType(m_handle, value); } }

        /// <summary>
        /// For Orthogonal Camera. Vertical scaling from view to world.
        /// </summary>
        public double ViewToWorldScale { get { return Interop.ViewPointGetViewToWorldScale(m_handle); } set { Interop.ViewPointSetViewToWorldScale(m_handle, value); } }

        /// <summary>
        /// For Perspective Camera. The entire vertical field of view angle of the camera, expressed in degrees. Valid range 0 to 180 exclusive.
        /// </summary>
        public double FieldOfView { get { return Interop.ViewPointGetFieldOfView(m_handle); } set { Interop.ViewPointSetFieldOfView(m_handle, value); } }

        /// <summary>
        /// Proportional relationship between the width and the height of the view (w/h). 
        /// </summary>
        public double AspectRatio { get { return Interop.ViewPointGetAspectRatio(m_handle); } set { Interop.ViewPointSetAspectRatio(m_handle, value); } }

        /// <summary>
        /// Camera location
        /// </summary>
        public Interop.BCFPoint GetCameraViewPoint() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraViewPoint(m_handle, out point)) throw new ApplicationException(Project.GetErrors()); return point; }

        /// <summary>
        /// Camera location
        /// </summary>
        public bool SetCameraViewPoint(Interop.BCFPoint value) { return Interop.ViewPointSetCameraViewPoint(m_handle, value); }

        /// <summary>
        /// Camera direction
        /// </summary>
        public Interop.BCFPoint GetCameraDirection() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraDirection(m_handle, out point)) throw new ApplicationException(Project.GetErrors()); return point; }

        /// <summary>
        /// Camera direction
        /// </summary>
        public bool SetCameraDirection(Interop.BCFPoint value) { return Interop.ViewPointSetCameraDirection(m_handle, value); }

        /// <summary>
        /// Camera up vector
        /// </summary>
        public Interop.BCFPoint GetCameraUpVector() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraUpVector(m_handle, out point)) throw new ApplicationException(Project.GetErrors()); return point; }

        /// <summary>
        /// Camera up vector
        /// </summary>
        public bool SetCameraUpVector(Interop.BCFPoint value) { return Interop.ViewPointSetCameraUpVector(m_handle, value); }

        /// <summary>
        /// List components of interest
        /// </summary>
        public List<Component> GetSelection()
        {
            var ret = new List<Component>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.ViewPointSelectionGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Component(Project, handle));
            }
            return ret;
        }

        /// <summary>
        ///  
        /// </summary>
        public Component AddSelection(string? ifcGuid = null)
        {
            IntPtr handle = Interop.ViewPointSelectionAdd(m_handle, ifcGuid);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add selection: " + Interop.GetErrors(Project.Handle));
            return new Component(Project, handle);
        }

        /// <summary>
        /// List of exceptions from DefaultVisibility
        /// </summary>
        public List<Component> GetExceptions()
        {
            var ret = new List<Component>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.ViewPointExceptionGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Component(Project, handle));
            }
            return ret;
        }

        /// <summary>
        /// Add exception from default visibility 
        /// </summary>
        public Component AddException(string? ifcGuid = null)
        {
            IntPtr handle = Interop.ViewPointExceptionAdd(m_handle, ifcGuid);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add exception: " + Interop.GetErrors(Project.Handle));
            return new Component(Project, handle);
        }

        /// <summary>
        /// List of bitmaps can be used to add more information, for example, text in the visualization.
        /// </summary>
        public List<Bitmap> GetBitmaps()
        {
            var ret = new List<Bitmap>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.BitmapGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Bitmap(this, handle));
            }
            return ret;
        }

        /// <summary>
        /// Bitmap can be used to add more information, for example, text in the visualization.
        /// </summary>
        public Bitmap AddBitmap (string filePath, BCFBitmapFormat format, BCFPoint location, BCFPoint normal, BCFPoint up, double height)
        {
            var handle = Interop.BitmapAdd(m_handle, filePath, format, location, normal, up, height);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add bitmap: " + Interop.GetErrors(Project.Handle));
            return new Bitmap(this, handle);
        }

        /// <summary>
        /// The Coloring element allows specifying the color of components. 
        /// For each color a list of components to be displayed with the that color should be provided.
        /// </summary>
        public List<Coloring> GetColorings()
        {
            var ret = new List<Coloring>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.ColoringGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Coloring(this, handle));
            }
            return ret;
        }

        /// <summary>
        /// The Coloring element allows specifying the color of components. 
        /// For each color a list of components to be displayed with the that color should be provided.
        /// </summary>
        public Coloring AddColoring(string? color)
        {
            var handle = Interop.ColoringAdd(m_handle, color);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add color: " + Interop.GetErrors(Project.Handle));
            return new Coloring(this, handle);
        }

        /// <summary>
        /// Lines can be used to add markup in 3D.
        /// </summary>
        public List<Line> GetLines()
        {
            var ret = new List<Line>();

            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.LineGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new Line(this, handle));
            }

            return ret;
        }

        /// <summary>
        /// Lines can be used to add markup in 3D.
        /// </summary>
        public Line AddLine(BCFPoint start, BCFPoint end)
        {
            var handle = Interop.LineAdd(m_handle, start, end);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add line: " + Interop.GetErrors(Project.Handle));
            return new Line(this, handle);
        }

        /// <summary>
        /// ClippingPlanes can be used to define a subsection of a building model that is related to the topic. 
        /// </summary>
        public List<ClippingPlane> GetClippingPlanes()
        {
            var ret = new List<ClippingPlane>();
            IntPtr handle = IntPtr.Zero;
            UInt16 ind = 0;
            while ((handle = Interop.ClippingPlaneGetAt(m_handle, ind++)) != IntPtr.Zero)
            {
                ret.Add(new ClippingPlane(this, handle));
            }
            return ret;
        }

        /// <summary>
        /// ClippingPlanes can be used to define a subsection of a building model that is related to the topic. 
        /// </summary>
        public ClippingPlane AddClippingPlane(BCFPoint location, BCFPoint direction)
        {
            var handle = Interop.ClippingPlaneAdd(m_handle, location, direction);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add clipping plane: " + Interop.GetErrors(Project.Handle));
            return new ClippingPlane(this, handle);
        }

        /// <summary>
        /// Remove object from BCF package. See Memory Management in documentation.
        /// </summary>
        public bool Remove()
        {
            return Interop.ViewPointRemove(m_handle);
        }

        #region INTERNAL
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal IntPtr Handle { get { return m_handle; } }
        internal Project Project { get { return m_topic.Project; } }

        internal ViewPoint(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        #endregion INTERNAL
    }
}
