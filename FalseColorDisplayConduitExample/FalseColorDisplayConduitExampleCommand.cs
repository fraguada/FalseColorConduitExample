using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace FalseColorDisplayConduitExample
{
    public class FalseColorDisplayConduitExampleCommand : Command
    {
        public FalseColorDisplayConduitExampleCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static FalseColorDisplayConduitExampleCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "FalseColorDisplayConduitExampleCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            var visible = FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.Enabled;

            string prompt = (visible)
              ? "FalseColorDisplay is visible"
              : "FalseColorDisplay is hidden";

            var go = new GetOption();
            go.SetCommandPrompt(prompt);

            var hide_index = go.AddOption("Hide");
            var show_index = go.AddOption("Show");
            var toggle_index = go.AddOption("Toggle");

            go.Get();
            if (go.CommandResult() != Result.Success)
                return go.CommandResult();

            var option = go.Option();
            if (null == option)
                return Result.Failure;

            var index = option.Index;

            if (index == hide_index)
            {
                if (visible)
                {
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.Enabled = false;
                    doc.Views.Redraw();
                }
            }
            else if (index == show_index)
            {
                if (!visible)
                {
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.SetObjects();
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.Enabled = true;
                    doc.Views.Redraw();
                }
                    
            }
            else if (index == toggle_index)
            {
                if (visible)
                {
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.Enabled = false;
                    doc.Views.Redraw();
                }
                else
                {
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.SetObjects();
                    FalseColorDisplayConduitExamplePlugIn.Instance.FalseColorDisplay.Enabled = true;
                    doc.Views.Redraw();
                }
            }


            return Result.Success;
        }
    }
}
