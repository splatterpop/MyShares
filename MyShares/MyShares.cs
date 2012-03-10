#region Copyright (C) 2005-2008 Team MediaPortal

/* 
 *	Copyright (C) 2005-2008 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Collections.Generic;
using System.Text;
//using System.Drawing;
using System.IO;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
//using System.Drawing.Imaging;
//using MediaPortal.Player;
using System.Threading;
using System.Collections;
using MediaPortal;
//using WindowPlugins;
using System.Runtime.InteropServices;
using System.Xml;


namespace MyShares
{
    public class Plugin : IPlugin, ISetupForm
    {
        private class MediaParams
        {
            public long minSize;
            public long maxSize;
        }

        private Dictionary<string, MediaParams> Parameters = new Dictionary<string,MediaParams>();

        #region External Calls

        [DllImport("Netapi32.dll")]
        private static extern int NetApiBufferFree(IntPtr Buffer);
        
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetShareEnum(
             StringBuilder ServerName,
             int level,
             ref IntPtr bufPtr,
             uint prefmaxlen,
             ref int entriesread,
             ref int totalentries,
             ref int resume_handle
             );

        [DllImport("Netapi32.dll", CharSet = CharSet.Auto)]
        private static extern int NetServerEnum(
            string ServerNane, // must be null
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain, // null for login domain
            out int dwResumeHandle
            );

        #endregion
        
        #region External Structures

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHARE_INFO_1
            {
                public string shi1_netname;
                public uint shi1_type;
                public string shi1_remark;
                public SHARE_INFO_1(string sharename, uint sharetype, string remark)
                {
                    this.shi1_netname = sharename;
                    this.shi1_type = sharetype;
                    this.shi1_remark = remark;
                }
                public override string ToString()
                {
                    return shi1_netname;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct _SERVER_INFO_100
            {
                internal int sv100_platform_id;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string sv100_name;
            }

            #endregion

        private const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;
        private const int NERR_Success = 0;
        private enum NetError : uint
        {
            NERR_Success = 0,
            NERR_BASE = 2100,
            NERR_UnknownDevDir = (NERR_BASE + 16),
            NERR_DuplicateShare = (NERR_BASE + 18),
            NERR_BufTooSmall = (NERR_BASE + 23),
        }

        private enum SHARE_TYPE : uint
        {
            STYPE_DISKTREE = 0,
            STYPE_PRINTQ = 1,
            STYPE_DEVICE = 2,
            STYPE_IPC = 3,
            STYPE_SPECIAL = 0x80000000,
        }

        private struct NETWORK_SHARE
        {
            public string Server;
            public SHARE_INFO_1 shi;
        }

        private AutoResetEvent workerEvent = new AutoResetEvent(false);
        private static MyShares.Plugin instance;

        private bool stopWorkerThread = false;
        private MySharesConfig config = new MySharesConfig();
        //private bool verboseLog = false;
        //private List<MediaPortal.Util.Share> AddedShares = new List<MediaPortal.Util.Share>();
        private delegate void FooCallbackType(NETWORK_SHARE s);
        //long videominsize;
        //long videomaxsize;
        //long pictureminsize;
        //long picturemaxsize;
        //long musicminsize;
        //long musicmaxsize;
        private const string _mediaTypeMovies = "Movies";
        private const string _mediaTypePictures = "Pictures";
        private const string _mediaTypeMusic = "Music";
        String[] SharesToIgnore;
        int recursionDepth;
        int maxFailures;
        IList<MyShares.KnownShare> knownShares = new List<MyShares.KnownShare>();

        public Plugin()
        {
        }

        // Returns the name of the plugin which is shown in the plugin menu
        public string PluginName()
        {
            return "MyShares";
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description()
        {
            return "scans your network for accessible shares and adds them automatically to MP's virtual folders";
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

            MyShares.MySharesConfig dlg;
            dlg = new MyShares.MySharesConfig();
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
            return 5680;
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
            strButtonText = "";
            strButtonImage = "";
            strButtonImageFocus = "";
            strPictureImage = "";
            return false;
        }

        /// <summary>
        /// This method runs in a thread and calls the actual cropping methods
        ///  DynamicCrop and SingleCrop.
        /// </summary>
        private void Worker()
        {
            Log.Debug("worker says hello");
            while (true)
            {
                if (stopWorkerThread)
                {
                    stopWorkerThread = false;
                    return;
                }

                if (stopWorkerThread == false)
                {
                    Parameters.Clear();

                    Parameters.Add(_mediaTypeMovies, new MediaParams());
                    Parameters.Add(_mediaTypeMusic, new MediaParams());
                    Parameters.Add(_mediaTypePictures, new MediaParams());

                    Parameters[_mediaTypeMovies].minSize = Convert.ToInt32(config.VideoMinFileSizeMB.Value) * 1024 * 1024;
                    Parameters[_mediaTypeMovies].maxSize = Convert.ToInt32(config.VideoMaxFileSizeMB.Value) * 1024 * 1024;
                    Parameters[_mediaTypePictures].minSize = Convert.ToInt32(config.PictureMinFileSizeKB.Value) * 1024;
                    Parameters[_mediaTypePictures].maxSize = Convert.ToInt32(config.PictureMaxFileSizeKB.Value) * 1024;
                    Parameters[_mediaTypeMusic].minSize = Convert.ToInt32(config.AudioMinFileSizeMB.Value) * 1024 * 1024;
                    Parameters[_mediaTypeMusic].maxSize = Convert.ToInt32(config.AudioMaxFileSizeMB.Value) * 1024 * 1024;
                    SharesToIgnore = config.SharesToIgnore.Lines;
                    recursionDepth = Convert.ToInt32(config.FolderRecursionDepth.Value);
                    maxFailures = Convert.ToInt32(config.MaxNumFailures.Value);

                    ValidateKnownShares();
                    ScanForShares();
#if DEBUG
                    Thread.Sleep(System.Convert.ToInt32(1000 * 60));
#else
                    Thread.Sleep(System.Convert.ToInt32(config.SampleInterval.Value * 1000 * 60));
#endif
                }
            }
        }

        /// <summary>
        /// Loads settings from the configuration file, and sets up allowed modes.
        /// </summary>
        /// <returns>False if the MyShares has no allowed modes and true otherwise</returns>
        private bool LoadSettings()
        {
 //           using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
            {
                //bool enabled = reader.GetValueAsBool(MySharesConfig.myCropSectionName, MySharesConfig.enableAutoCropSetting, false);
                Log.Debug("MyShares: LoadSettings");

                return true;
            }
        }

        /// <summary>
        /// Implements PlugInBase.Start
        /// Sets up the MyShares
        /// and sets GUIGraphicsContext.MyShares
        /// to point to this object.
        /// </summary>
        public void Start()
        {
            Log.Info("MyShares: Start()");
            instance = this;

            // Load settings, if returns false,
            // none of MyShares modes are allowed
            // so we dont do anything
            //if (LoadSettings() == false) 
            //    return;

            LoadSettings();
            LoadKnownShares();

            // start the thread that will execute the actual cropping
            Thread t = new Thread(new ThreadStart(instance.Worker));
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Name = "MySharesThread";
            Log.Debug("MyShares: start worker thread");
            t.Start();
        }

        /// <summary>
        /// Implements PlugInBase.Stop
        /// Stops the worker thread, and removes the reference to this MyShares
        /// from GUIGraphicsContext
        /// </summary>
        public void Stop()
        {
            Log.Info("MyShares: Stop()");
            SaveKnownShares();
            stopWorkerThread = true;
        }

        private void ValidateKnownShares()
        {
            List<KnownShare> invalidShares = new List<KnownShare>();
            
            // scan all known shares
            foreach (KnownShare k in knownShares)
            {
                if (!Directory.Exists(k.path))
                {
                    MediaPortal.Util.VirtualDirectory d = MediaType.GetVirtualDirectory(k.mediatype);
                    invalidShares.Add(k);
                }
                else
                {
                    if (!HasMediaType(k.mediatype, k.path, recursionDepth, Parameters[k.mediatype].minSize, Parameters[k.mediatype].maxSize))
                        invalidShares.Add(k);
                }
            }

            foreach (KnownShare k in invalidShares)
            {
                k.numFailures++;
                if (k.numFailures > maxFailures)
                {
                    MediaPortal.Util.VirtualDirectory d = MediaType.GetVirtualDirectory(k.mediatype);
                    d.Remove(k.path);
                    knownShares.Remove(k);
                }
            }
        }

        /// <summary>
        /// Scans the network for shares containing media files
        /// </summary>
        private void ScanForShares()
        {
            ArrayList computers = EnumerateServers();
            List<NETWORK_SHARE> shares = EnumerateShares2(computers, ShareCallback);

/*            ProcessVideos(shares);
            ProcessPictures(shares);
            ProcessMusic(shares);*/
        }

        private void ShareCallback(NETWORK_SHARE sh)
        {
            MediaPortal.Util.VirtualDirectory vd = MediaPortal.Util.VirtualDirectories.Instance.Movies;
            ProcessShare(sh, MediaType.Movies, vd, Parameters[_mediaTypeMovies].minSize, Parameters[_mediaTypeMovies].maxSize);
            
            vd = MediaPortal.Util.VirtualDirectories.Instance.Pictures;
            ProcessShare(sh, MediaType.Pictures, vd, Parameters["Pictures"].minSize, Parameters[_mediaTypePictures].maxSize);
            
            vd = MediaPortal.Util.VirtualDirectories.Instance.Music;
            ProcessShare(sh, MediaType.Music, vd, Parameters[_mediaTypeMusic].minSize, Parameters[_mediaTypeMusic].maxSize);
        }

/*
        private void ProcessVideos(List<NETWORK_SHARE> shares)
        {
            MediaPortal.Util.VirtualDirectory vd = MediaPortal.Util.VirtualDirectories.Instance.Movies;
            long minsizemb = Convert.ToInt32(config.VideoMinFileSizeMB.Value) * 1024 * 1024;
            long maxsizemb = Convert.ToInt32(config.VideoMaxFileSizeMB.Value) * 1024 * 1024;
            ProcessShares(shares, vd, minsizemb, maxsizemb);
        }

        private void ProcessPictures(List<NETWORK_SHARE> shares)
        {
            MediaPortal.Util.VirtualDirectory vd = MediaPortal.Util.VirtualDirectories.Instance.Pictures;
            long minsizekb = Convert.ToInt32(config.PictureMinFileSizeKB.Value) * 1024;
            long maxsizekb = Convert.ToInt32(config.PictureMaxFileSizeKB.Value) * 1024;
            ProcessShares(shares, vd, minsizekb, maxsizekb);
        }

        private void ProcessMusic(List<NETWORK_SHARE> shares)
        {
            MediaPortal.Util.VirtualDirectory vd = MediaPortal.Util.VirtualDirectories.Instance.Music;
            long minsizekb = Convert.ToInt32(config.PictureMinFileSizeKB.Value) * 1024;
            long maxsizekb = Convert.ToInt32(config.PictureMaxFileSizeKB.Value) * 1024;
            ProcessShares(shares, vd, minsizekb, maxsizekb);
        }

        private void ProcessShares(List<NETWORK_SHARE> shares, MediaPortal.Util.VirtualDirectory vdir, long minsize, long maxsize)
        {
            foreach (NETWORK_SHARE s in shares)
            {
                if (s.shi.shi1_type == (uint)SHARE_TYPE.STYPE_DISKTREE)
                {
                    string uncpath = @"\\" + s.Server + @"\" + s.shi.shi1_netname;
                    if (HasMediaType(uncpath, vdir, 2, minsize, maxsize))
                    {
                        MediaPortal.Util.Share newshare = vdir.GetShare(uncpath);
                        bool createnewshare = false;
                        if (newshare == null) 
                            createnewshare = true;
                        if (newshare != null && createnewshare == false)
                        {
                            // GetShare treats incomplete paths equally
                            if (newshare.Path.ToLower() != uncpath.ToLower())
                                createnewshare = true;
                        }
                        if (createnewshare == true)
                        {
                            string sharename = s.Server + "-" + s.shi.shi1_netname;
                            Log.Debug("adding new share: " + sharename + " for " + uncpath);
                            newshare = new MediaPortal.Util.Share(sharename, uncpath);
                            vdir.Add(newshare);
                            AddedShares.Add(newshare);
                            knownShares.Add(new MyShares.KnownShare(vdir.ToString(), sharename, uncpath));

                            // todo: if active plugin is guivideofiles, refresh view to reflect changes
                            MediaPortal.GUI.Library.GUIWindow w = MediaPortal.GUI.Library.GUIWindowManager.GetWindow(MediaPortal.GUI.Library.GUIWindowManager.ActiveWindow);
                            MediaPortal.GUI.Video.GUIVideoFiles vf = (MediaPortal.GUI.Video.GUIVideoFiles)w;
                            if (vf != null)
                            {
                                // todo: only update if we are on the root folder level where the shares are listed
                            }
                            if (w != null)
                            {
                                    GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SHOW_DIRECTORY, w.GetID, MyShares.Plugin.instance.GetWindowId(), 0, 0, 0, null);
                                    MediaPortal.GUI.Library.GUIWindowManager.SendMessage(msg);
                            }
                        }
                    }
                }
            }

            // todo: verify existence of added shares for, remove if no longer existing
        }
*/

        private void ProcessShare(NETWORK_SHARE s, String mediatype, MediaPortal.Util.VirtualDirectory vdir, long minsize, long maxsize)
        {
            if (s.shi.shi1_type == (uint)SHARE_TYPE.STYPE_DISKTREE)
            {
                string uncpath = @"\\" + s.Server + @"\" + s.shi.shi1_netname;
                if (HasMediaType(mediatype, uncpath, recursionDepth, minsize, maxsize))
                {
                    string sharename = s.Server + ": " + s.shi.shi1_netname;
                    AddShare(mediatype, vdir, uncpath, sharename);
                }
            }
        }

        private void AddShare(String mediatype, MediaPortal.Util.VirtualDirectory vdir, string uncpath, string sharename)
        {
            bool createnewshare = false;
            // check whether theis unc path already exists as a share
            MediaPortal.Util.Share newshare = vdir.GetShare(uncpath);
            if (newshare == null)
                createnewshare = true;
            if (newshare != null && createnewshare == false)
            {
                // GetShare treats incomplete paths equally
                if (newshare.Path.ToLower() != uncpath.ToLower())
                    createnewshare = true;
            }
            if (createnewshare == true)
            {
                Log.Debug("adding new share: " + sharename + " for " + uncpath + ", type: " + mediatype);
                newshare = new MediaPortal.Util.Share(sharename, uncpath);
                newshare.RuntimeAdded = true;
                vdir.Add(newshare);
                //AddedShares.Add(newshare);
                knownShares.Add(new MyShares.KnownShare(mediatype, sharename, uncpath));

                // todo: if active plugin is guivideofiles, refresh view to reflect changes
                MediaPortal.GUI.Library.GUIWindow w = MediaPortal.GUI.Library.GUIWindowManager.GetWindow(MediaPortal.GUI.Library.GUIWindowManager.ActiveWindow);
                /*                            MediaPortal.GUI.Video.GUIVideoFiles vf = (MediaPortal.GUI.Video.GUIVideoFiles)w;
                                            if (vf != null)
                                            {
                                                // todo: only update if we are on the root folder level where the shares are listed
                                            }*/
                /*
                // refresh display of directories to reflect new shares
                if (w != null)
                {
                    GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SHOW_DIRECTORY, w.GetID, MyShares.Plugin.instance.GetWindowId(), 0, 0, 0, null);
                    MediaPortal.GUI.Library.GUIWindowManager.SendMessage(msg);
                }
                */
            }
        }

        private bool HasMediaType(string mediatype, string uncpath, int level, long minsize, long maxsize)
        {
            if (level <= 0) return false;

            try
            {
                // Log.Debug("retrieving information for folder " + uncpath);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(uncpath);

                if ((di.Attributes & FileAttributes.Hidden) > 0 && config.IgnoreHiddenFiles.Checked)
                    return false;
                if ((di.Attributes & FileAttributes.System) > 0 && config.IgnoreSystemFiles.Checked)
                    return false;
                
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Length < minsize && minsize > 0)
                        continue;
                    if (fi.Length > maxsize && maxsize > 0)
                        continue;
                    if ((fi.Attributes & FileAttributes.Hidden) > 0 && config.IgnoreHiddenFiles.Checked)
                        continue;
                    if ((fi.Attributes & FileAttributes.System) > 0 && config.IgnoreSystemFiles.Checked)
                        continue;

                    bool found = false;
                    if (mediatype == _mediaTypeMovies)
                    {
                        if (MediaPortal.Util.Utils.IsVideo(fi.Name))
                        {
                            found = true;
                        }
                    }
                    else if (mediatype == _mediaTypePictures)
                    {
                        if (MediaPortal.Util.Utils.IsPicture(fi.Name))
                        {
                            found = true;
                        }
                    }
                    else if (mediatype == _mediaTypeMusic)
                    {
                        if (MediaPortal.Util.Utils.IsAudio(fi.Name))
                        {
                            found = true;
                        }
                    }

                    if (found)
                    {
                        Log.Debug("identified " + uncpath + @"\" + fi.Name + " as media type " + mediatype);
                        return true;
                    }
                }

                foreach (DirectoryInfo subd in di.GetDirectories())
                {
                    bool has = HasMediaType(mediatype, uncpath + @"\" + subd.Name, level - 1, minsize, maxsize);
                    if (has) return true;
                }
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            return false;
        }

        private ArrayList EnumerateServers()
        {
            ArrayList networkComputers = new ArrayList();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));

            Log.Debug("scanning for servers");

            try
            {
                int ret = NetServerEnum(null, 100, ref buffer, 
                    MAX_PREFERRED_LENGTH,
                    out entriesRead,
                    out totalEntries, SV_TYPE_WORKSTATION | 
                    SV_TYPE_SERVER, null, out 
                    resHandle);

                if (ret == 0)
                {
                    for (int i = 0; i < totalEntries; i++)
                    {
                        tmpBuffer = new IntPtr((int)buffer + 
                                   (i * sizeofINFO));
                        _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)
                            Marshal.PtrToStructure(tmpBuffer, 
                                    typeof(_SERVER_INFO_100));

                        networkComputers.Add(svrInfo.sv100_name);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Problem with acessing network computers in NetworkBrowser " + ex.Message);
            }
            finally
            {
                NetApiBufferFree(buffer);
            }
            return networkComputers;
        }

        private List<NETWORK_SHARE> EnumerateShares(ArrayList computers)
        {
            Log.Debug("scanning for shares");

            List<NETWORK_SHARE> ShareInfos = new List<NETWORK_SHARE>();

            foreach (string ServerName in computers)
            {
                //string ServerName = info; // "Jedi-2";

                int entriesread = 0;
                int totalentries = 0;
                int resume_handle = 0;
                int nStructSize = Marshal.SizeOf(typeof(SHARE_INFO_1));
                IntPtr bufPtr = IntPtr.Zero;
                StringBuilder server = new StringBuilder(ServerName);
                int ret = NetShareEnum(server, 1, ref bufPtr, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle);
                if (ret == NERR_Success)
                {
                    IntPtr currentPtr = bufPtr;
                    for (int i = 0; i < entriesread; i++)
                    {
                        NETWORK_SHARE ns = new NETWORK_SHARE();
                        ns.Server = ServerName;
                        ns.shi = (SHARE_INFO_1)Marshal.PtrToStructure(currentPtr, typeof(SHARE_INFO_1));
                        ShareInfos.Add(ns);
                        currentPtr = new IntPtr(currentPtr.ToInt32() + nStructSize);
                    }
                    NetApiBufferFree(bufPtr);
                }
            }
            return ShareInfos;
        }

        private List<NETWORK_SHARE> EnumerateShares2(ArrayList computers, FooCallbackType fun)
        {
            List<NETWORK_SHARE> ShareInfos = new List<NETWORK_SHARE>();

            foreach (string ServerName in computers)
            {
                //string ServerName = info; // "Jedi-2";
                Log.Debug("processing server " + ServerName);
                int entriesread = 0;
                int totalentries = 0;
                int resume_handle = 0;
                int nStructSize = Marshal.SizeOf(typeof(SHARE_INFO_1));
                IntPtr bufPtr = IntPtr.Zero;
                StringBuilder server = new StringBuilder(ServerName);
                int ret = NetShareEnum(server, 1, ref bufPtr, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle);
                if (ret == NERR_Success)
                {
                    IntPtr currentPtr = bufPtr;
                    for (int i = 0; i < entriesread; i++)
                    {
                        NETWORK_SHARE ns = new NETWORK_SHARE();
                        ns.Server = ServerName;
                        ns.shi = (SHARE_INFO_1)Marshal.PtrToStructure(currentPtr, typeof(SHARE_INFO_1));
                        ShareInfos.Add(ns);
                        fun(ns);
                        currentPtr = new IntPtr(currentPtr.ToInt32() + nStructSize);
                    }
                    NetApiBufferFree(bufPtr);
                }
            }
            Log.Debug("MyShares: done processing servers");
            return ShareInfos;
        }

        public void LoadKnownShares()
        {
            try
            {
                System.Xml.XmlDocument doc = MyShares.MySharesConfig.ConfigFile();
                try
                {
                    XmlNode nKnownShares = doc.GetElementsByTagName(MyShares.MySharesConfig.knownSharesSectionName)[0];
                    foreach (XmlNode n in nKnownShares.ChildNodes)
                    {
                        String path = n.Attributes.GetNamedItem("path").Value;
                        String mediatype = n.Attributes.GetNamedItem("mediatype").Value;
                        String sharename = n.Attributes.GetNamedItem("sharename").Value;
                        
                        // todo: check chare for existence, only add if accessible
                        if (Directory.Exists(path))
                        {
                            //if (mediatype == MediaType.Movies)
                            //    AddShare(MediaType.Movies, MediaPortal.Util.VirtualDirectories.Instance.Movies, path, sharename);
                            //else if (mediatype == MediaType.Pictures)
                            //    AddShare(MediaType.Pictures, MediaPortal.Util.VirtualDirectories.Instance.Pictures, path, sharename);
                            //else if (mediatype == MediaType.Music)
                            //    AddShare(MediaType.Music, MediaPortal.Util.VirtualDirectories.Instance.Music, path, sharename);
                            AddShare(mediatype, MediaType.GetVirtualDirectory(mediatype), path, sharename);
                        }
                    }
                }
                catch (Exception e)
                {
                }

            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }
        }

        public void SaveKnownShares()
        {
            try
            {
                System.Xml.XmlDocument doc = MyShares.MySharesConfig.ConfigFile();
                try
                {
                    XmlNode nKnownShares = doc.GetElementsByTagName(MyShares.MySharesConfig.knownSharesSectionName)[0];
                    if (nKnownShares == null)
                    {
                        nKnownShares = doc.CreateElement(MyShares.MySharesConfig.knownSharesSectionName);
                        doc.DocumentElement.AppendChild(nKnownShares);
                    }
                    nKnownShares.RemoveAll();
                    int i = 1;
                    foreach (KnownShare k in knownShares)
                    {
                        XmlElement e = doc.CreateElement("Share_" + i.ToString());
                        e.SetAttribute("mediatype", k.mediatype);
                        e.SetAttribute("path", k.path);
                        e.SetAttribute("sharename", k.sharename);
                        nKnownShares.AppendChild(e);
                        i++;
                    }
                    doc.Save(MyShares.MySharesConfig.ConfigFileName());
                }
                catch (Exception e)
                {
                }

            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }
        }
    }
}
