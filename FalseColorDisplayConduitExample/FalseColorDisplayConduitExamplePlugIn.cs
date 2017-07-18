using System;
using Rhino.Display;
using Rhino.DocObjects;

namespace FalseColorDisplayConduitExample
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class FalseColorDisplayConduitExamplePlugIn : Rhino.PlugIns.PlugIn
    {
        ///<summary>Gets the only instance of the FalseColorDisplayConduitExamplePlugIn plug-in.</summary>
        public static FalseColorDisplayConduitExamplePlugIn Instance { get; private set; }

        /// <summary>
        /// The display conduit for false color display.
        /// </summary>
        public FalseColorDisplayConduit FalseColorDisplay { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        public FalseColorDisplayConduitExamplePlugIn()
        {
            Instance = this;

            FalseColorDisplay = new FalseColorDisplayConduit();

            //Listen to events
            Rhino.RhinoDoc.AddRhinoObject += OnAddRhinoObject;
            Rhino.RhinoDoc.DeleteRhinoObject += OnDeleteRhinoObject;
            Rhino.RhinoDoc.ReplaceRhinoObject += OnReplaceRhinoObject;

        }


        private void OnReplaceRhinoObject(object sender, RhinoReplaceObjectEventArgs e)
        {
            if (e.NewRhinoObject.ObjectType != ObjectType.Brep) return;

            RhinoObjectActivity();
        }

        private void OnDeleteRhinoObject(object sender, RhinoObjectEventArgs e)
        {
            if (e.TheObject.ObjectType != ObjectType.Brep) return;

            RhinoObjectActivity();
        }

        private void OnAddRhinoObject(object sender, RhinoObjectEventArgs e)
        {
            if (e.TheObject.ObjectType != ObjectType.Brep) return;

            RhinoObjectActivity();
        }

        /// <summary>
        /// Called to update the display conduit when something happens to a Rhino Object
        /// </summary>
        private void RhinoObjectActivity()
        {
            if (!FalseColorDisplay.Enabled) return;

            FalseColorDisplay.SetObjects();

        }



        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.


    }
}