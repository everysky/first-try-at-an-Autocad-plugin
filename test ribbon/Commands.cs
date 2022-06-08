
using System;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Ribbon;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;

namespace test_Ribbon
{
    public class Commands
    {
        [CommandMethod("TEST_RIBBON", CommandFlags.Transparent)]
        public void NoplanRibbon()
        {
            // We get and store the existing Autocad ribbon
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                // we search for a spécific tab in the ribbon
                RibbonTab rtab = ribbon.FindTab("TEST_NOPLAN");
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = "TEST-NOPLAN";
                rtab.Id = "TEST_NOPLAN-ID_1";
                // Add the Tab 
                ribbon.Tabs.Add(rtab);
                addContentTo(rtab);
            }
        }

        [CommandMethod("HELLO", CommandFlags.Transparent)]
        public void HelloWorld()
        {
            var document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            var editor = document.Editor;
            editor.WriteMessage("\nHello World!");
        }

        static void addContentTo(RibbonTab rtab)
        {
            rtab.Panels.Add(AddOnePanel());
        }

        static RibbonPanel AddOnePanel()
        {
            RibbonPanelSource rps = new RibbonPanelSource
            {
                Title = "Test One",
                Name = "Test_1"
            };
            RibbonPanel rp = new RibbonPanel
            {
                Source = rps
            };

            ///////////////////////////////////////////////////////////////
            //////////I don't understand everything. need to investigate 
            ///////////////////////////////////////////////////////////////
            // Create a Command Item that the Dialog Launcher can use,
            // for this test it is just a place holder.
            RibbonButton rci = new RibbonButton();
            rci.Name = "placeholder";

            // assign the Command Item to the DialgLauncher which auto-enables
            // the little button at the lower right of a Panel
            rps.DialogLauncher = rci;
            ///////////////////////////////////////////////////////////////

            // some documentation for later
            RibbonButton rb = new RibbonButton();
            rb.Name = "Noplan";
            rb.ShowText = true;
            rb.Text = rb.Name;
            rb.CommandHandler = new RibbonButtonCommandHandler();
            rb.CommandParameter = "HELLO ";

            RibbonButton rb2 = new RibbonButton();
            //rb2.Id = "ID_BUTTON_2";
            rb2.Name = "Get a comic";
            rb2.Text = rb2.Name;
            rb2.ShowText = true;
            rb2.CommandHandler = new RibbonButtonCommandHandler();
            rb2.CommandParameter = "PS_InvokeAComic ";

            //Add the Button to the Tab
            rps.Items.Add(rb);
            rps.Items.Add(rb2);
            return rp;
        }

        [CommandMethod("PS_InvokeAComic")]
        public async Task InvokeAComicAsync()
        {
            try
            {
                ComicGetter cmd = new ComicGetter();

                ComicSummarizer summarizer = new ComicSummarizer();
                AutocadMessageWriter writter = new AutocadMessageWriter();

                var comic = await cmd.LoadComic();

                comic.Write(summarizer, writter);
            }
            catch(Autodesk.AutoCAD.Runtime.Exception Ex)
            {
                Application.ShowAlertDialog("The following exception was caught:\n" +
                                     Ex.Message);
            }
        }
    }
}