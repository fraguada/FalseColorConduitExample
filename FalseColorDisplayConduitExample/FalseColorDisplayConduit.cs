using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FalseColorDisplayConduitExample
{
    public class FalseColorDisplayConduit: DisplayConduit
    {
        public static FalseColorDisplayConduit Instance { get; private set; }
        List<Mesh> Meshes { get; set; }

        public FalseColorDisplayConduit()
        {
            Instance = this;
        }

        /// <summary>
        /// Gets any Brep from the Rhino Model, extracts the Analysis Mesh, and sets Vertex Colors.
        /// </summary>
        public void SetObjects()
        {

            Meshes = new List<Mesh>();

            var objects = Rhino.RhinoDoc.ActiveDoc.Objects.FindByObjectType(ObjectType.Brep);

            foreach (var obj in objects)
            {
                var brep = (obj as BrepObject);
                brep.CreateMeshes(MeshType.Analysis, MeshingParameters.Smooth, false);
                var mesh = new Mesh();
                foreach (Mesh m in brep.GetMeshes(MeshType.Analysis))
                    mesh.Append(m);

                mesh.Normals.ComputeNormals();
                mesh.Compact();

                Meshes.Add(mesh);
            }

            AddColors();

        }

        /// <summary>
        /// Sets vertex colors for the meshes.
        /// </summary>
        void AddColors()
        {
            var bbMax = Rhino.RhinoDoc.ActiveDoc.Objects.BoundingBox.Max.Z;
            var bbMin = Rhino.RhinoDoc.ActiveDoc.Objects.BoundingBox.Min.Z;

            foreach (var m in Meshes)
            {
                m.VertexColors.Clear();
                for (int i = 0; i < m.Vertices.Count; i++)
                {
                    var v = m.Vertices[i];
                    var val = Remap(v.Z, bbMin, bbMax, 0, 1);

                    var color = Color.FromArgb((int)(255*val), (int)(255*(1-val)), 0);

                    m.VertexColors.Add(color);
                    
                }
            }
        }

        public static double Remap(double value, double originStart, double originEnd, double targetStart, double targetEnd)
        {
            return ((value - originStart) / (originEnd - originStart) * (targetEnd - targetStart) + targetStart);
        }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            Rhino.Geometry.BoundingBox bbox = Rhino.Geometry.BoundingBox.Unset;
            if (null != Meshes)
            {
                foreach (var obj in Meshes)
                    bbox.Union(obj.GetBoundingBox(false));
                e.IncludeBoundingBox(bbox);
            }
        }

        protected override void CalculateBoundingBoxZoomExtents(CalculateBoundingBoxEventArgs e)
        {
            Rhino.Geometry.BoundingBox bbox = Rhino.Geometry.BoundingBox.Unset;
            if (null != Meshes)
            {
                foreach (var obj in Meshes)
                    bbox.Union(obj.GetBoundingBox(false));
                e.IncludeBoundingBox(bbox);
            }
        }

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            base.PostDrawObjects(e);
            if(null != Meshes)
                foreach (var m in Meshes)
                    e.Display.DrawMeshFalseColors(m);
        }

    }
}
