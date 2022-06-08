using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;


namespace test_Ribbon /* this code came from project test_parcels */
{
    public static class Active
    {
        public static Document Document => Application.DocumentManager.MdiActiveDocument;
        public static Editor Editor => Document.Editor;
        public static Database Database => Document.Database;
        public static void UsingTransaction(Action<Transaction> action)
        {
            using (var transaction = Active.Database.TransactionManager.StartTransaction())
            {
                action(transaction);

                transaction.Commit();
            }
        }

    }
}
