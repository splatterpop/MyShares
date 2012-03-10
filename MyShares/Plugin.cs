using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MediaPortal.GUI.Library;
using MediaPortal.Dialogs;
using MediaPortal.Util;


namespace MyCropper
{
    public class Plugin : GUIWindow, ISetupForm
    {
        private AutoResetEvent workerEvent = new AutoResetEvent(false);

        public Plugin()
        {
            _finder = null;
            iLastSearchIndex = -1;
            iLastListIndex = -1;
        }

        #region ISetupForm Members

        // Returns the name of the plugin which is shown in the plugin menu
        public string PluginName()
        {
          return "TvBits";
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description()
        {
          return "TvBits plugin";
        }

        // Returns the author of the plugin which is shown in the plugin menu
        public string Author()
        {
          return "splatterpop";
        }

        // show the setup dialog
        public void ShowPlugin()
        {
//          MessageBox.Show("Nothing to configure, this is just an example");

            TvBits.Setup dlg;
            dlg = new TvBits.Setup();
            dlg.ShowDialog();
        }

        // Indicates whether plugin can be enabled/disabled
        public bool CanEnable()
        {
          return true;
        }

        // Get Windows-ID
        public int GetWindowId()
        {
          // WindowID of windowplugin belonging to this setup
          // enter your own unique code
          return 5678;
        }

        // Indicates if plugin is enabled by default;
        public bool DefaultEnabled()
        {
          return true;
        }

        // indicates if a plugin has it's own setup screen
        public bool HasSetup()
        {
          return true;
        }

        /// <summary>
        /// If the plugin should have it's own button on the main menu of MediaPortal then it
        /// should return true to this method, otherwise if it should not be on home
        /// it should return false
        /// </summary>
        /// <param name="strButtonText">text the button should have</param>
        /// <param name="strButtonImage">image for the button, or empty for default</param>
        /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
        /// <param name="strPictureImage">subpicture for the button or empty for none</param>
        /// <returns>true : plugin needs it's own button on home
        /// false : plugin does not need it's own button on home</returns>

        public bool GetHome(out string strButtonText, out string strButtonImage,
          out string strButtonImageFocus, out string strPictureImage)
        {
          strButtonText = PluginName();
          strButtonImage = String.Empty;
          strButtonImageFocus = String.Empty;
          strPictureImage = String.Empty;
          return true;
        }

        // With GetID it will be an window-plugin / otherwise a process-plugin
        // Enter the id number here again
        public override int GetID
        {
          get
          {
            return 5678;
          }

          set
          {
          }
        }

    #endregion

        public override bool Init()
        {
            bool result;

            if (System.IO.File.Exists(GUIGraphicsContext.Skin + @"\MyCropper.xml") == false)
            {
                String s = Resource1.tvbits_xml;
                System.IO.BinaryWriter w = new System.IO.BinaryWriter(System.IO.File.Open(GUIGraphicsContext.Skin + @"\TvBits.xml", System.IO.FileMode.CreateNew));
                w.Write(s.ToCharArray());
                w.Close();
            }

            result = Load(GUIGraphicsContext.Skin + @"\MyCropper.xml");

            if (result == true)
            {
                Thread t = new Thread(new ThreadStart(this.Worker));
                t.IsBackground = true;
                t.Priority = ThreadPriority.Lowest;
                t.Name = "TvBits";
                t.Start();
            }
            
            return result;
        }

        private void Worker()
        {
            DateTime dtNextScan = DateTime.Now; // tsDelay = new TimeSpan(_finder.RepeatSearch, 0, 0);
            while (true)
            {
                while (_finder == null)
                {
                    try
                    {
                        Thread.Sleep(5000);
                        _finder = new Finder();
                        _finder.NewResults += new Finder.NewResultsCallback(_finder_NewResults);
                        Log.Info("Start searching programs database");
                    }
                    catch (Exception e)
                    {
                        Log.Warn("caught excption: " + e.Message);
                        Log.Info(e.StackTrace);
                        _finder = null;
                        Thread.Sleep(500);
                    }
                }
                try
                {
                    if (DateTime.Now >= dtNextScan)
                    {
                        _finder.FindPrograms();
                        dtNextScan += new TimeSpan(_finder.RepeatSearch, 0, 0);
                        Log.Info("Done searching programs database, next scan will be at " + dtNextScan.ToString());
                    }
                    else
                        UpdateIsRunning();
                }
                catch (Exception e)
                {
                    Log.Warn("caught excption: " + e.Message);
                    Log.Info(e.StackTrace);
                    _finder = null;
                }
                Thread.Sleep(15000);
            }
        }
    }

}
