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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MediaPortal.Util;
//using DShowNET;
//using DShowNET.Helper;
//using DirectShowLib;
using System.Diagnostics;
using MediaPortal.GUI.Library;
using MediaPortal.Configuration;
using System.Xml;
using System.Text;

//#pragma warning disable 108
namespace MyShares
{
    public class MySharesConfig : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;
        public MediaPortal.UserInterface.Controls.MPButton okButton;
        public MediaPortal.UserInterface.Controls.MPButton defaultsButton;
        private TabControl tabControl1;
        private TabPage General;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown FolderRecursionDepth;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel3;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown SampleInterval;
        public MediaPortal.UserInterface.Controls.MPLabel autoSampleIntervalLabel;
        private TabPage Videos;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown VideoMaxFileSizeMB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel2;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown VideoMinFileSizeMB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel1;
        private TabPage Pictures;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown PictureMaxFileSizeKB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel4;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown PictureMinFileSizeKB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel5;
        private TabPage Music;
        public CheckBox IgnoreHiddenFiles;
        private Label label1;
        public TextBox SharesToIgnore;
        public CheckBox IgnoreSystemFiles;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown AudioMaxFileSizeMB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel6;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown AudioMinFileSizeMB;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel7;
        public MediaPortal.UserInterface.Controls.MPNumericUpDown MaxNumFailures;
        public MediaPortal.UserInterface.Controls.MPLabel mpLabel8;
        public MediaPortal.UserInterface.Controls.MPButton cancelButton;

        public MySharesConfig()
            : base()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            LoadSettings();
//            OnChange(null, null);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new MediaPortal.UserInterface.Controls.MPButton();
            this.defaultsButton = new MediaPortal.UserInterface.Controls.MPButton();
            this.cancelButton = new MediaPortal.UserInterface.Controls.MPButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.SharesToIgnore = new System.Windows.Forms.TextBox();
            this.IgnoreSystemFiles = new System.Windows.Forms.CheckBox();
            this.IgnoreHiddenFiles = new System.Windows.Forms.CheckBox();
            this.FolderRecursionDepth = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel3 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.SampleInterval = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.autoSampleIntervalLabel = new MediaPortal.UserInterface.Controls.MPLabel();
            this.Videos = new System.Windows.Forms.TabPage();
            this.VideoMaxFileSizeMB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.VideoMinFileSizeMB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.Pictures = new System.Windows.Forms.TabPage();
            this.PictureMaxFileSizeKB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel4 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.PictureMinFileSizeKB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel5 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.Music = new System.Windows.Forms.TabPage();
            this.AudioMaxFileSizeMB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel6 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.AudioMinFileSizeMB = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel7 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.MaxNumFailures = new MediaPortal.UserInterface.Controls.MPNumericUpDown();
            this.mpLabel8 = new MediaPortal.UserInterface.Controls.MPLabel();
            this.tabControl1.SuspendLayout();
            this.General.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FolderRecursionDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleInterval)).BeginInit();
            this.Videos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VideoMaxFileSizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoMinFileSizeMB)).BeginInit();
            this.Pictures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureMaxFileSizeKB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureMinFileSizeKB)).BeginInit();
            this.Music.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AudioMaxFileSizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioMinFileSizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxNumFailures)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(443, 277);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(87, 27);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnOk);
            // 
            // defaultsButton
            // 
            this.defaultsButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultsButton.Location = new System.Drawing.Point(351, 277);
            this.defaultsButton.Name = "defaultsButton";
            this.defaultsButton.Size = new System.Drawing.Size(87, 27);
            this.defaultsButton.TabIndex = 6;
            this.defaultsButton.Text = "Defaults";
            this.defaultsButton.UseVisualStyleBackColor = true;
            this.defaultsButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnDefaults);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(535, 277);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 27);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.General);
            this.tabControl1.Controls.Add(this.Videos);
            this.tabControl1.Controls.Add(this.Pictures);
            this.tabControl1.Controls.Add(this.Music);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(610, 259);
            this.tabControl1.TabIndex = 21;
            // 
            // General
            // 
            this.General.Controls.Add(this.MaxNumFailures);
            this.General.Controls.Add(this.mpLabel8);
            this.General.Controls.Add(this.label1);
            this.General.Controls.Add(this.SharesToIgnore);
            this.General.Controls.Add(this.IgnoreSystemFiles);
            this.General.Controls.Add(this.IgnoreHiddenFiles);
            this.General.Controls.Add(this.FolderRecursionDepth);
            this.General.Controls.Add(this.mpLabel3);
            this.General.Controls.Add(this.SampleInterval);
            this.General.Controls.Add(this.autoSampleIntervalLabel);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Padding = new System.Windows.Forms.Padding(3);
            this.General.Size = new System.Drawing.Size(602, 233);
            this.General.TabIndex = 0;
            this.General.Text = "General";
            this.General.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(262, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Shares to ignore:";
            // 
            // SharesToIgnore
            // 
            this.SharesToIgnore.AcceptsReturn = true;
            this.SharesToIgnore.Location = new System.Drawing.Point(262, 27);
            this.SharesToIgnore.Multiline = true;
            this.SharesToIgnore.Name = "SharesToIgnore";
            this.SharesToIgnore.Size = new System.Drawing.Size(323, 187);
            this.SharesToIgnore.TabIndex = 19;
            // 
            // IgnoreSystemFiles
            // 
            this.IgnoreSystemFiles.AutoSize = true;
            this.IgnoreSystemFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IgnoreSystemFiles.Location = new System.Drawing.Point(50, 115);
            this.IgnoreSystemFiles.Name = "IgnoreSystemFiles";
            this.IgnoreSystemFiles.Size = new System.Drawing.Size(117, 17);
            this.IgnoreSystemFiles.TabIndex = 18;
            this.IgnoreSystemFiles.Text = "Ignore system files";
            this.IgnoreSystemFiles.UseVisualStyleBackColor = true;
            // 
            // IgnoreHiddenFiles
            // 
            this.IgnoreHiddenFiles.AutoSize = true;
            this.IgnoreHiddenFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IgnoreHiddenFiles.Location = new System.Drawing.Point(52, 92);
            this.IgnoreHiddenFiles.Name = "IgnoreHiddenFiles";
            this.IgnoreHiddenFiles.Size = new System.Drawing.Size(115, 17);
            this.IgnoreHiddenFiles.TabIndex = 17;
            this.IgnoreHiddenFiles.Text = "Ignore hidden files";
            this.IgnoreHiddenFiles.UseVisualStyleBackColor = true;
            // 
            // FolderRecursionDepth
            // 
            this.FolderRecursionDepth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderRecursionDepth.Location = new System.Drawing.Point(153, 38);
            this.FolderRecursionDepth.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FolderRecursionDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FolderRecursionDepth.Name = "FolderRecursionDepth";
            this.FolderRecursionDepth.Size = new System.Drawing.Size(61, 21);
            this.FolderRecursionDepth.TabIndex = 16;
            this.FolderRecursionDepth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // mpLabel3
            // 
            this.mpLabel3.AutoSize = true;
            this.mpLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel3.Location = new System.Drawing.Point(10, 40);
            this.mpLabel3.Name = "mpLabel3";
            this.mpLabel3.Size = new System.Drawing.Size(115, 13);
            this.mpLabel3.TabIndex = 15;
            this.mpLabel3.Text = "Folder recursion depth";
            // 
            // SampleInterval
            // 
            this.SampleInterval.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SampleInterval.Location = new System.Drawing.Point(153, 11);
            this.SampleInterval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.SampleInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SampleInterval.Name = "SampleInterval";
            this.SampleInterval.Size = new System.Drawing.Size(61, 21);
            this.SampleInterval.TabIndex = 14;
            this.SampleInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // autoSampleIntervalLabel
            // 
            this.autoSampleIntervalLabel.AutoSize = true;
            this.autoSampleIntervalLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoSampleIntervalLabel.Location = new System.Drawing.Point(10, 13);
            this.autoSampleIntervalLabel.Name = "autoSampleIntervalLabel";
            this.autoSampleIntervalLabel.Size = new System.Drawing.Size(129, 13);
            this.autoSampleIntervalLabel.TabIndex = 13;
            this.autoSampleIntervalLabel.Text = "Rescan interval (minutes)";
            // 
            // Videos
            // 
            this.Videos.Controls.Add(this.VideoMaxFileSizeMB);
            this.Videos.Controls.Add(this.mpLabel2);
            this.Videos.Controls.Add(this.VideoMinFileSizeMB);
            this.Videos.Controls.Add(this.mpLabel1);
            this.Videos.Location = new System.Drawing.Point(4, 22);
            this.Videos.Name = "Videos";
            this.Videos.Padding = new System.Windows.Forms.Padding(3);
            this.Videos.Size = new System.Drawing.Size(602, 233);
            this.Videos.TabIndex = 1;
            this.Videos.Text = "Videos";
            this.Videos.UseVisualStyleBackColor = true;
            // 
            // VideoMaxFileSizeMB
            // 
            this.VideoMaxFileSizeMB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VideoMaxFileSizeMB.Location = new System.Drawing.Point(153, 38);
            this.VideoMaxFileSizeMB.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.VideoMaxFileSizeMB.Name = "VideoMaxFileSizeMB";
            this.VideoMaxFileSizeMB.Size = new System.Drawing.Size(61, 21);
            this.VideoMaxFileSizeMB.TabIndex = 24;
            // 
            // mpLabel2
            // 
            this.mpLabel2.AutoSize = true;
            this.mpLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel2.Location = new System.Drawing.Point(10, 40);
            this.mpLabel2.Name = "mpLabel2";
            this.mpLabel2.Size = new System.Drawing.Size(119, 13);
            this.mpLabel2.TabIndex = 23;
            this.mpLabel2.Text = "Video max file size (MB)";
            // 
            // VideoMinFileSizeMB
            // 
            this.VideoMinFileSizeMB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VideoMinFileSizeMB.Location = new System.Drawing.Point(153, 11);
            this.VideoMinFileSizeMB.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.VideoMinFileSizeMB.Name = "VideoMinFileSizeMB";
            this.VideoMinFileSizeMB.Size = new System.Drawing.Size(61, 21);
            this.VideoMinFileSizeMB.TabIndex = 22;
            // 
            // mpLabel1
            // 
            this.mpLabel1.AutoSize = true;
            this.mpLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel1.Location = new System.Drawing.Point(10, 13);
            this.mpLabel1.Name = "mpLabel1";
            this.mpLabel1.Size = new System.Drawing.Size(115, 13);
            this.mpLabel1.TabIndex = 21;
            this.mpLabel1.Text = "Video min file size (MB)";
            // 
            // Pictures
            // 
            this.Pictures.Controls.Add(this.PictureMaxFileSizeKB);
            this.Pictures.Controls.Add(this.mpLabel4);
            this.Pictures.Controls.Add(this.PictureMinFileSizeKB);
            this.Pictures.Controls.Add(this.mpLabel5);
            this.Pictures.Location = new System.Drawing.Point(4, 22);
            this.Pictures.Name = "Pictures";
            this.Pictures.Size = new System.Drawing.Size(602, 233);
            this.Pictures.TabIndex = 2;
            this.Pictures.Text = "Pictures";
            this.Pictures.UseVisualStyleBackColor = true;
            // 
            // PictureMaxFileSizeKB
            // 
            this.PictureMaxFileSizeKB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PictureMaxFileSizeKB.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.PictureMaxFileSizeKB.Location = new System.Drawing.Point(153, 38);
            this.PictureMaxFileSizeKB.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PictureMaxFileSizeKB.Name = "PictureMaxFileSizeKB";
            this.PictureMaxFileSizeKB.Size = new System.Drawing.Size(61, 21);
            this.PictureMaxFileSizeKB.TabIndex = 28;
            // 
            // mpLabel4
            // 
            this.mpLabel4.AutoSize = true;
            this.mpLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel4.Location = new System.Drawing.Point(10, 40);
            this.mpLabel4.Name = "mpLabel4";
            this.mpLabel4.Size = new System.Drawing.Size(123, 13);
            this.mpLabel4.TabIndex = 27;
            this.mpLabel4.Text = "Picture max file size (kB)";
            // 
            // PictureMinFileSizeKB
            // 
            this.PictureMinFileSizeKB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PictureMinFileSizeKB.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.PictureMinFileSizeKB.Location = new System.Drawing.Point(153, 11);
            this.PictureMinFileSizeKB.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PictureMinFileSizeKB.Name = "PictureMinFileSizeKB";
            this.PictureMinFileSizeKB.Size = new System.Drawing.Size(61, 21);
            this.PictureMinFileSizeKB.TabIndex = 26;
            // 
            // mpLabel5
            // 
            this.mpLabel5.AutoSize = true;
            this.mpLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel5.Location = new System.Drawing.Point(10, 13);
            this.mpLabel5.Name = "mpLabel5";
            this.mpLabel5.Size = new System.Drawing.Size(119, 13);
            this.mpLabel5.TabIndex = 25;
            this.mpLabel5.Text = "Picture min file size (kB)";
            // 
            // Music
            // 
            this.Music.Controls.Add(this.AudioMaxFileSizeMB);
            this.Music.Controls.Add(this.mpLabel6);
            this.Music.Controls.Add(this.AudioMinFileSizeMB);
            this.Music.Controls.Add(this.mpLabel7);
            this.Music.Location = new System.Drawing.Point(4, 22);
            this.Music.Name = "Music";
            this.Music.Size = new System.Drawing.Size(602, 233);
            this.Music.TabIndex = 3;
            this.Music.Text = "Music";
            this.Music.UseVisualStyleBackColor = true;
            // 
            // AudioMaxFileSizeMB
            // 
            this.AudioMaxFileSizeMB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AudioMaxFileSizeMB.Location = new System.Drawing.Point(153, 38);
            this.AudioMaxFileSizeMB.Name = "AudioMaxFileSizeMB";
            this.AudioMaxFileSizeMB.Size = new System.Drawing.Size(61, 21);
            this.AudioMaxFileSizeMB.TabIndex = 32;
            // 
            // mpLabel6
            // 
            this.mpLabel6.AutoSize = true;
            this.mpLabel6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel6.Location = new System.Drawing.Point(10, 40);
            this.mpLabel6.Name = "mpLabel6";
            this.mpLabel6.Size = new System.Drawing.Size(120, 13);
            this.mpLabel6.TabIndex = 31;
            this.mpLabel6.Text = "Audio max file size (MB)";
            // 
            // AudioMinFileSizeMB
            // 
            this.AudioMinFileSizeMB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AudioMinFileSizeMB.Location = new System.Drawing.Point(153, 11);
            this.AudioMinFileSizeMB.Name = "AudioMinFileSizeMB";
            this.AudioMinFileSizeMB.Size = new System.Drawing.Size(61, 21);
            this.AudioMinFileSizeMB.TabIndex = 30;
            // 
            // mpLabel7
            // 
            this.mpLabel7.AutoSize = true;
            this.mpLabel7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel7.Location = new System.Drawing.Point(10, 13);
            this.mpLabel7.Name = "mpLabel7";
            this.mpLabel7.Size = new System.Drawing.Size(116, 13);
            this.mpLabel7.TabIndex = 29;
            this.mpLabel7.Text = "Audio min file size (MB)";
            // 
            // MaxNumFailures
            // 
            this.MaxNumFailures.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxNumFailures.Location = new System.Drawing.Point(153, 65);
            this.MaxNumFailures.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MaxNumFailures.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxNumFailures.Name = "MaxNumFailures";
            this.MaxNumFailures.Size = new System.Drawing.Size(61, 21);
            this.MaxNumFailures.TabIndex = 22;
            this.MaxNumFailures.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // mpLabel8
            // 
            this.mpLabel8.AutoSize = true;
            this.mpLabel8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mpLabel8.Location = new System.Drawing.Point(10, 67);
            this.mpLabel8.Name = "mpLabel8";
            this.mpLabel8.Size = new System.Drawing.Size(121, 13);
            this.mpLabel8.TabIndex = 21;
            this.mpLabel8.Text = "max. number of failures";
            // 
            // MySharesConfig
            // 
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(635, 527);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.defaultsButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MySharesConfig";
            this.Load += new System.EventHandler(this.MySharesConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.General.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FolderRecursionDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SampleInterval)).EndInit();
            this.Videos.ResumeLayout(false);
            this.Videos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VideoMaxFileSizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoMinFileSizeMB)).EndInit();
            this.Pictures.ResumeLayout(false);
            this.Pictures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureMaxFileSizeKB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureMinFileSizeKB)).EndInit();
            this.Music.ResumeLayout(false);
            this.Music.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AudioMaxFileSizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioMinFileSizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxNumFailures)).EndInit();
            this.ResumeLayout(false);

        }

        void OnCancel(object sender, EventArgs e)
        {
            this.Close();
        }

        void OnHelp(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("IExplore", "http://wiki.team-mediaportal.com/MediaPortalSetup_Plugins/MyShares");
            }
            catch (Exception ex)
            {
                Log.Warn(ex.ToString());
            }
        }
        #endregion

        public void OnOk(object o, MouseEventArgs args)
        {
            SaveSettings();
            this.Close();
        }

        public void OnDefaults(object o, MouseEventArgs args)
        {
            this.SampleInterval.Value = 15;
        }

        // if any settings change check if some options such be hidden
        public void OnChange(object o, EventArgs args)
        {

        }


        public static string mySharesSectionName = "profile";
        public static string settingsSectionName = "Settings";
        public static string knownSharesSectionName = "KnownShares";

        public static String ConfigFileName()
        {
            return Config.GetFile(Config.Dir.Config, "MyShares.xml");
            //return Config.Dir.Config + @"\MyShares.xml";
        }

        public static System.Xml.XmlDocument ConfigFile()
        { 
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            String fp = ConfigFileName();
            try
            {
              doc.Load(fp);
            }
            catch (Exception ex)
            {
              Log.Debug("error loading configuration file {0}: {1}", fp, ex.Message);
              WriteEmptyConfigurationFile(fp);
              doc.Load(fp);
            }
            return doc;
        }

        public static void WriteEmptyConfigurationFile(String _fp)
        {
            XmlTextWriter xtw = new XmlTextWriter(_fp, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement(mySharesSectionName);
            xtw.WriteStartElement(knownSharesSectionName);
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.Close();
        }

        public void LoadSettings()
        {
            try
            {
                using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(ConfigFileName()))
                {
                    // Global parameters
                    this.SampleInterval.Value = reader.GetValueAsInt(settingsSectionName, this.SampleInterval.Name, 15);
                    this.FolderRecursionDepth.Value = reader.GetValueAsInt(settingsSectionName, this.FolderRecursionDepth.Name, 2);
                    this.MaxNumFailures.Value = reader.GetValueAsInt(settingsSectionName, this.MaxNumFailures.Name, 2);
                    this.IgnoreHiddenFiles.Checked = reader.GetValueAsBool(settingsSectionName, this.IgnoreHiddenFiles.Name, true);
                    this.IgnoreSystemFiles.Checked = reader.GetValueAsBool(settingsSectionName, this.IgnoreSystemFiles.Name, true);
                    this.SharesToIgnore.Lines = reader.GetValueAsString(settingsSectionName, this.SharesToIgnore.Name, "").Split('|');

                    // Video parameters
                    this.VideoMinFileSizeMB.Value = reader.GetValueAsInt(settingsSectionName, this.VideoMinFileSizeMB.Name, 0);
                    this.VideoMaxFileSizeMB.Value = reader.GetValueAsInt(settingsSectionName, this.VideoMaxFileSizeMB.Name, 0);

                    // Picture parameters
                    this.PictureMinFileSizeKB.Value = reader.GetValueAsInt(settingsSectionName, this.PictureMinFileSizeKB.Name, 0);
                    this.PictureMaxFileSizeKB.Value = reader.GetValueAsInt(settingsSectionName, this.PictureMaxFileSizeKB.Name, 0);

                    // Music parameters
                    this.AudioMinFileSizeMB.Value = reader.GetValueAsInt(settingsSectionName, this.AudioMinFileSizeMB.Name, 0);
                    this.AudioMaxFileSizeMB.Value = reader.GetValueAsInt(settingsSectionName, this.AudioMaxFileSizeMB.Name, 0);

                }
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }
        }

        public void SaveSettings()
        {
            using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(ConfigFileName()))
            {
                xmlwriter.SetValue(settingsSectionName, SampleInterval.Name, SampleInterval.Value);
                xmlwriter.SetValue(settingsSectionName, FolderRecursionDepth.Name, FolderRecursionDepth.Value);
                xmlwriter.SetValue(settingsSectionName, MaxNumFailures.Name, MaxNumFailures.Value);
                xmlwriter.SetValue(settingsSectionName, IgnoreHiddenFiles.Name, IgnoreHiddenFiles.Checked.ToString());
                xmlwriter.SetValue(settingsSectionName, IgnoreSystemFiles.Name, IgnoreSystemFiles.Checked.ToString());

                string s = "";
                foreach (string l in SharesToIgnore.Lines)
                    s = s + l + "|";
                s.TrimEnd('|');
                xmlwriter.SetValue(settingsSectionName, SharesToIgnore.Name, s);

                xmlwriter.SetValue(settingsSectionName, VideoMinFileSizeMB.Name, VideoMinFileSizeMB.Value);
                xmlwriter.SetValue(settingsSectionName, VideoMaxFileSizeMB.Name, VideoMaxFileSizeMB.Value);

                xmlwriter.SetValue(settingsSectionName, PictureMinFileSizeKB.Name, PictureMinFileSizeKB.Value);
                xmlwriter.SetValue(settingsSectionName, PictureMaxFileSizeKB.Name, PictureMaxFileSizeKB.Value);

                xmlwriter.SetValue(settingsSectionName, AudioMinFileSizeMB.Name, AudioMinFileSizeMB.Value);
                xmlwriter.SetValue(settingsSectionName, AudioMaxFileSizeMB.Name, AudioMaxFileSizeMB.Value);

            }
        }

        private void MySharesConfig_Load(object sender, EventArgs e)
        {

        }

        private void HLine(Bitmap b, int x1, int y, int x2, Color c)
        {
            for (int x = x1; x < x2; x++)
            {
                b.SetPixel(x, y, c);
            }
        }

        private void VLine(Bitmap b, int x, int y1, int y2, Color c)
        {
            for (int y = y1; y < y2; y++)
            {
                b.SetPixel(x, y, c);
            }
        }
    }

    public class KnownShare
    {
        public KnownShare(String _mediatype, String _sharename, String _path)
        {
            mediatype = _mediatype;
            sharename = _sharename;
            path = _path;
            numFailures = 0;
        }
        public String sharename;
        public String path;
        public String mediatype;
        public int numFailures;
    }

    public class MediaType
    {
        public static  string Movies = "Movies";
        public static  string Pictures = "Pictures";
        public static  string Music = "Music";

        public static MediaPortal.Util.VirtualDirectory GetVirtualDirectory(String mediatype)
        {
            if (mediatype == MediaType.Movies)
                return MediaPortal.Util.VirtualDirectories.Instance.Movies;
            else if (mediatype == MediaType.Pictures)
                return MediaPortal.Util.VirtualDirectories.Instance.Pictures;
            else if (mediatype == MediaType.Music)
                return MediaPortal.Util.VirtualDirectories.Instance.Music;

            throw new System.InvalidOperationException();
        }
    }
}

