//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.EditorInput;
//using System;

namespace test_Ribbon
// this code came from project test_parcels
// It need some recycling (Not everything is needed anymore, but we can learn from it)
{
    //public class ParcelCounter
    //{
    //    internal ParcelSummary Count()
    //    {
    //        PromptSelectionResult result = SelectParcels();

    //        var summary = new ParcelSummary();
    //        if (result.Status == PromptStatus.OK)
    //        {
    //            Active.UsingTransaction(tr =>
    //            {
    //                foreach (var objectId in result.Value.GetObjectIds())
    //                {
    //                    var polyline = (Polyline)tr.GetObject(objectId, OpenMode.ForRead);
    //                    if (polyline.Closed)
    //                    {
    //                        summary.Count++;
    //                        summary.Area += polyline.Area;
    //                    }
    //                }
    //            });
    //        }
    //        return summary;
    //    }

    //    private PromptSelectionResult SelectParcels()
    //    {
    //        var options = new PromptSelectionOptions();
    //        options.MessageForAdding = "Add parcels";
    //        options.MessageForRemoval = "Remove parcels";

    //        var filter = new SelectionFilter(new TypedValue[]
    //        {
    //            new TypedValue((int)DxfCode.Start, "LWPOLYLINE"),
    //            new TypedValue((int)DxfCode.LayerName, "Parcels")
    //        });
    //        return Active.Editor.GetSelection(options, filter);
    //    }
    //}
}