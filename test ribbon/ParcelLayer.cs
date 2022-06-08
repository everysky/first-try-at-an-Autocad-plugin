using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace test_Ribbon
// this code came from project test_parcels
// It need some recycling (Not everything is needed anymore, but we can learn from it)
{
    internal class ParcelLayer
    {
        public ParcelLayer()
        {

        }

        public void Create()
        {
            var layerName = "Parcels";
            var document = Active.Document;
            var database = Active.Database;

            using (var transaction = database.TransactionManager.StartTransaction())
            {
                var layerTable = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForRead);
                LayerTableRecord layer;
                if (layerTable.Has(layerName) == false)
                {
                    layer = new LayerTableRecord
                    {
                        Name = layerName,
                        Color = Color.FromColorIndex(ColorMethod.ByAci, 161)
                    };

                    layerTable.UpgradeOpen();
                    layerTable.Add(layer);
                    transaction.AddNewlyCreatedDBObject(layer, true);

                }
                database.Clayer = layerTable[layerName];
                transaction.Commit();
            }
        }
    }
}