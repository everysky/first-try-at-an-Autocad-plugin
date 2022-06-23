
using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Windows;
using Autodesk.AutoCAD.DatabaseServices;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace test_Ribbon
{
    public class Commands
    {
        public static int MaxComicNumber { get; set; }

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
                rtab = new RibbonTab
                {
                    Title = "TEST-NOPLAN",
                    Id = "TEST_NOPLAN-ID_1"
                };
                // Add the Tab 
                ribbon.Tabs.Add(rtab);
                AddContentTo(rtab);
            }
        }

        [CommandMethod("HELLO", CommandFlags.Transparent)]
        public void HelloWorld()
        {
            var document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            var editor = document.Editor;
            editor.WriteMessage("\nHello World!");
        }

        static void AddContentTo(RibbonTab rtab)
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
            //////////I don't understand everything in here. I need to investigate 
            ///////////////////////////////////////////////////////////////
            // Create a Command Item that the Dialog Launcher can use,
            // for this test it is just a place holder.
            RibbonButton rci = new RibbonButton
            {
                Name = "placeholder"
            };

            // assign the Command Item to the DialgLauncher which auto-enables
            // the little button at the lower right of a Panel
            rps.DialogLauncher = rci;

            // some documentation for later
            RibbonButton rb = new RibbonButton()
            {
                Name = "Noplan",
                ShowText = true,
                Text = "Noplan",
                CommandHandler = new RibbonButtonCommandHandler(),
                CommandParameter = "HELLO ",
            };

            RibbonButton rb2 = new RibbonButton()
            {
                //Id = "ID_BUTTON_2",
                Name = "Get a comic",
                Text = "Get a comic",
                ShowText = true,
                CommandHandler = new RibbonButtonCommandHandler(),
                CommandParameter = "XKCDWS "
            };
            ///////////////////////////////////////////////////////////////

            //Add the Button to the Tab
            rps.Items.Add(rb);
            rps.Items.Add(rb2);
            return rp;
        }

        //[CommandMethod("PS_InvokeAComic")]
        //public async Task InvokeAComicAsync()
        //{
        //    try
        //    {
        //        ComicGetter cmd = new ComicGetter();

        //        ComicSummarizer summarizer = new ComicSummarizer();
        //        AutocadMessageWriter writter = new AutocadMessageWriter();

        //        var comic = await cmd.LoadComic();

        //        comic.Write(summarizer, writter);
        //    }
        //    catch(Autodesk.AutoCAD.Runtime.Exception Ex)
        //    {
        //        Application.ShowAlertDialog("The following exception was caught:\n" +
        //                             Ex.Message);
        //        Console.WriteLine("console: The following exception was caught:\n" +
        //                             Ex.Message);
        //    }
        //}

        [CommandMethod("XKCDWS")]
        public static void XkcdWebService()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            Editor ed = doc.Editor;

            // Prompt for the comic's number
            PromptIntegerOptions pio = new PromptIntegerOptions("\nEnter the comic's number")
            {
                LowerLimit = 0,
                UpperLimit = MaxComicNumber > 0 ? MaxComicNumber : 0,
                DefaultValue = 0,
                UseDefaultValue = true
            };

            PromptIntegerResult pir = ed.GetInteger(pio);

            if (pir.Status != PromptStatus.OK)
                return;

            int comicNumber = pir.Value;

            Transaction tr = doc.TransactionManager.StartTransaction();

            using (tr)
            {
                Stopwatch sw = Stopwatch.StartNew();

                dynamic res = Commands.XkcdComicWs(comicNumber);

                sw.Stop();



                ed.WriteMessage(

                   "\nWeb service call took {0} seconds.",

                   sw.Elapsed.TotalSeconds

                 );

                if (res == null)
                {
                    throw new ArgumentNullException("res");
                }
                Dictionary<string, string> newRes = res;

                ed.WriteMessage("\nThis is your comic, enjoy!");

                if (newRes.ContainsKey("safe_title"))
                {
                    string message = string.Format("\nTitle: \"{0}\"", newRes["safe_title"]);
                    ed.WriteMessage(message);
                }

                if (newRes.ContainsKey("alt"))
                {
                    string message = string.Format("\nAlternative title: \"{0}\"", newRes["alt"]);
                    ed.WriteMessage(message);
                }

                if (newRes.ContainsKey("num"))
                {
                    int num = Int32.Parse(newRes["num"]);
                    if (MaxComicNumber < num) { MaxComicNumber = num; };
                }

                // Go through our "dynamic" list, accessing each property
                // dynamically

                foreach (KeyValuePair<string, string> kvp in res)
                {
                    string message = string.Format("{0} = {1}", kvp.Key, kvp.Value);
                    ed.WriteMessage("\n" + message);
                    Debug.WriteLine("\n" + message);
                }

                tr.Commit();

                string countMessage = string.Format("\n{0} keys in the dictionary.", res.Count);
                ed.WriteMessage(countMessage);
                Debug.WriteLine(countMessage);
            }
        }

        private static dynamic XkcdComicWs(int comicNumber = 0)
        {
            string json = null;
            string url = (
                "https://xkcd.com"
                + (comicNumber == 0 ? "" : $"/{comicNumber}")
                + "/info.0.json"
                );

            Debug.WriteLine(
                "\nThe URL is: "
                + url
                );

            // Call our web-service synchronously (this isn't ideal, as
            // it blocks the UI thread)

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            // Get the response

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream

                StreamReader reader =
                  new StreamReader(response.GetResponseStream());

                // Extract our JSON results

                json = reader.ReadToEnd();

                //Debug.WriteLine(
                //    "\n+++++++++++++++++++++++++++"
                //    + "\nHey, this is the JSON:"
                //    + "\n" + json
                //);
            }

            if (!String.IsNullOrEmpty(json))
            {
                // Use our dynamic JSON converter to populate/return
                // our list of results

                var serializer = new JavaScriptSerializer();

                Dictionary<string, string> obj = serializer.Deserialize<Dictionary<string, string>>(json);
                //Debug.WriteLine("yoooooo");
                //Debug.WriteLine(obj["month"]);
                return obj;
            }
            return null;
        }
    }
}