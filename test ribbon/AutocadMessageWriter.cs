namespace test_Ribbon /* this code came from project test_parcels */
{
    public class AutocadMessageWriter : IMessageWriter
    {
        public void WriteMessage(string message)
        {
            Active.Editor.WriteMessage($"\n{message}");
        }
    }
}
