﻿using System.Diagnostics;
using System.Reflection;

namespace KindleMate2 {
    internal partial class FrmAboutBox : Form {
        public FrmAboutBox() {
            InitializeComponent();
        }

        private static string FormatFileSize(long fileSize) {
            string[] sizes = ["B", "KB", "MB", "GB", "TB"];
            var order = 0;
            double size = fileSize;

            while (size >= 1024 && order < sizes.Length - 1) {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        private static string AssemblyVersion {
            get => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }

        private static string AssemblyProduct {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        private static string AssemblyTitle {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyTitleAttribute)attributes[0]).Title;
            }
        }

        private static string AssemblyCopyright {
            get {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private void LblPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("explorer.exe", lblPath.Text);
        }

        private void FrmAboutBox_Load(object sender, EventArgs e) {
            Text = "关于 " + AssemblyProduct;

            //lblProductName.Text = AssemblyTitle;
            lblVersion.Text = AssemblyVersion;
            lblCopyright.Text = AssemblyCopyright;

            var programsDirectory = AppDomain.CurrentDomain.BaseDirectory;
            lblPath.Text = programsDirectory;
            var filePath = Path.Combine(programsDirectory, "KM2.dat");
            var fileInfo = new FileInfo(filePath);
            var fileSize = fileInfo.Length;
            lblDatabase.Text = "KM2.dat (" + FormatFileSize(fileSize) + ")";
        }

        private void LblCleanDatabase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var programsDirectory = AppDomain.CurrentDomain.BaseDirectory;
            lblPath.Text = programsDirectory;
            var filePath = Path.Combine(programsDirectory, "KM2.dat");
            var fileInfo = new FileInfo(filePath);
            var originFileSize = fileInfo.Length;

            var staticData = new StaticData();
            staticData.VacuumDatabase();

            var newFileSize = fileInfo.Length;

            if (newFileSize < originFileSize) {
                MessageBox.Show("数据库已清理 " + FormatFileSize(originFileSize - newFileSize), "清理数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("数据库无需清理", "清理数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}