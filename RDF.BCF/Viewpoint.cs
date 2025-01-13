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
        public string Guid { get { return Interop.ViewPointGetGuild(m_handle); } }
        public bool DefaultVisibility { get { return Interop.ViewPointGetDefaultVisibility(m_handle); } set { Interop.ViewPointSetDefaultVisibility(m_handle, value); } }
        public bool SpaceVisible { get { return Interop.ViewPointGetSpaceVisible(m_handle); } set { Interop.ViewPointSetSpaceVisible(m_handle, value); } }
        public bool SpaceBoundariesVisible { get { return Interop.ViewPointGetSpaceBoundariesVisible(m_handle); } set { Interop.ViewPointSetSpaceBoundariesVisible(m_handle, value); } }
        public bool OpeningsVisible { get { return Interop.ViewPointGetOpeningsVisible(m_handle); } set { Interop.ViewPointSetOpeningsVisible(m_handle, value); } }
        public Interop.BCFCamera CameraType { get { return Interop.ViewPointGetCameraType(m_handle); } set { Interop.ViewPointSetCameraType(m_handle, value); } }
        public double ViewToWorldScale { get { return Interop.ViewPointGetViewToWorldScale(m_handle); } set { Interop.ViewPointSetViewToWorldScale(m_handle, value); } }
        public double FieldOfView { get { return Interop.ViewPointGetFieldOfView(m_handle); } set { Interop.ViewPointSetFieldOfView(m_handle, value); } }
        public double AspectRatio { get { return Interop.ViewPointGetAspectRatio(m_handle); } set { Interop.ViewPointSetAspectRatio(m_handle, value); } }
        public string Snapshot { get { return Interop.ViewPointGetSnapshot(m_handle); } set { Interop.ViewPointSetSnapshot(m_handle, value); } }
        public Interop.BCFPoint GetCameraViewPoint() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraViewPoint(m_handle, out point)) throw new ApplicationException(Project.ErrorsGet()); return point; }
        public bool SetCameraViewPoint(Interop.BCFPoint value) { return Interop.ViewPointSetCameraViewPoint(m_handle, value); }        
        public Interop.BCFPoint GetCameraDirection() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraDirection(m_handle, out point)) throw new ApplicationException(Project.ErrorsGet()); return point; } 
        public bool SetCameraDirection (Interop.BCFPoint value) { return Interop.ViewPointSetCameraDirection(m_handle, value); }        
        public Interop.BCFPoint GetCameraUpVector() { Interop.BCFPoint point; if (!Interop.ViewPointGetCameraUpVector(m_handle, out point)) throw new ApplicationException(Project.ErrorsGet()); return point; } 
        public bool SetCameraUpVector (Interop.BCFPoint value) { return Interop.ViewPointSetCameraUpVector(m_handle, value); }

        public List<Component> Selection { get { return GetSelection(); } }
        public Component AddSelection(string? ifcGuid = null)
        {
            IntPtr handle = Interop.ViewPointSelectionAdd(m_handle, ifcGuid);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add selection: " + Interop.ErrorsGet(Project.Handle));
            return new Component(Project, handle);
        }
        public bool RemoveSelection(Component component)
        {
            return Interop.ViewPointSelectionRemove(m_handle, component.Handle);
        }

        public List<Component> Exceptions { get { return GetExceptions(); } }
        public Component AddException(string? ifcGuid = null)
        {
            IntPtr handle = Interop.ViewPointExceptionsAdd(m_handle, ifcGuid);
            if (handle == IntPtr.Zero)
                throw new ApplicationException("Fail to add exception: " + Interop.ErrorsGet(Project.Handle));
            return new Component(Project, handle);
        }
        public bool RemoveException(Component component)
        {
            return Interop.ViewPointExceptionsRemove(m_handle, component.Handle);
        }

        /// <summary>
        /// 
        /// </summary>
        public Project Project { get { return m_topic.Project; } }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove()
        {
            return Interop.ViewPointRemove(m_handle);
        }

        #region IMPLEMENTATION
        ///////////////////////////////////////////////////////////////////////////////////////////
        Topic m_topic;
        IntPtr m_handle;

        internal IntPtr Handle { get { return m_handle; } }

        internal ViewPoint(Topic topic, IntPtr handle)
        {
            m_topic = topic;
            m_handle = handle;
        }

        public List<Component> GetSelection()
        {
            var ret = new List<Component>();

            IntPtr handle = IntPtr.Zero;
            while ((handle = Interop.ViewPointSelectionIterate(m_handle, handle)) != IntPtr.Zero)
            {
                ret.Add(new Component(Project, handle));
            }

            return ret;
        }

        public List<Component> GetExceptions()
        {
            var ret = new List<Component>();

            IntPtr handle = IntPtr.Zero;
            while ((handle = Interop.ViewPointExceptionsIterate(m_handle, handle)) != IntPtr.Zero)
            {
                ret.Add(new Component(Project, handle));
            }

            return ret;
        }

        #endregion IMPLEMENTATION
    }
}
