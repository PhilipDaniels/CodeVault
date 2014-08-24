using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FSWatcher {
    public partial class Form1 : Form {
        private const string m_Dir = @"C:\Temp";
        private FileSystemWatcher m_Watcher;

        public delegate void InvokeDelegate();

        public Form1() {
            InitializeComponent();
            m_Watcher = new FileSystemWatcher(m_Dir);
            m_Watcher.Changed += new FileSystemEventHandler(OnChange);
            m_Watcher.Created += new FileSystemEventHandler(OnChange);
            m_Watcher.Deleted += new FileSystemEventHandler(OnChange);
            m_Watcher.Renamed += new RenamedEventHandler(OnRename);
            //m_Watcher.Renamed += new FileSystemEventHandler(OnChange);
            m_Watcher.EnableRaisingEvents = true;
        }

        void OnRename(object sender, RenamedEventArgs e) {
            Console.WriteLine("{0} was renamed to {1}", e.OldName, e.Name);
        }

        void OnChange(object sender, FileSystemEventArgs e) {
            /*
             * This naive code will blow up because the FSW will be invoking this
             * code from another thread, which means we can't update the UI.
            string[] files = Directory.GetFiles(m_Dir);
            lstFiles.BeginUpdate();
            lstFiles.Items.Clear();
            foreach (string file in files)
                lstFiles.Items.Add(file);
            lstFiles.EndUpdate();
             */

            // This will run it on the GUI's thread.
            if (InvokeRequired) {
                BeginInvoke(new InvokeDelegate(OnChangeInThread));
            }
        }

        private void OnChangeInThread() {
            string[] files = Directory.GetFiles(m_Dir);
            lstFiles.BeginUpdate();
            lstFiles.Items.Clear();
            foreach (string file in files)
                lstFiles.Items.Add(file);
            lstFiles.EndUpdate();
        }
    }
}