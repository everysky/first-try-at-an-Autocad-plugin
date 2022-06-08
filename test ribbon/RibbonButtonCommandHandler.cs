using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using System;

namespace test_Ribbon
{
    // This entire class comes from there:
    // https://github.com/PavelKrKrastev/AutoCadConvertToPdfPlugIn
    public class RibbonButtonCommandHandler : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            RibbonCommandItem CommandItem = parameter as RibbonCommandItem;
            Document DwgDocument = Application.DocumentManager.MdiActiveDocument;
            DwgDocument.SendStringToExecute((string)CommandItem.CommandParameter, true, false, true);
        }
    }
}
