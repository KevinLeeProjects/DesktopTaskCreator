using System.Collections.Generic;
using System.Runtime.InteropServices;


using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WallpaperImageInput
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 

        //UI needed to submit task information
        private TextBox taskNameTextBox;
        private TextBox informationTextBox;
        private Button submitButton;
        private Button newTask;

        private List<TaskInfo> tasks = new List<TaskInfo>();

        //Windows stuff so program has access to change wallpaper
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";

            // Create and initialize controls
            this.taskNameTextBox = new TextBox();
            this.taskNameTextBox.PlaceholderText = "Task name";
            this.taskNameTextBox.Location = new System.Drawing.Point(20, 20);
            this.taskNameTextBox.Size = new System.Drawing.Size(200, 20);

            this.informationTextBox = new TextBox();
            this.informationTextBox.PlaceholderText = "Information";
            this.informationTextBox.Location = new System.Drawing.Point(20, 60);
            this.informationTextBox.Size = new System.Drawing.Size(200, 100);
            this.informationTextBox.Multiline = true;

            this.newTask = new Button();
            this.newTask.Text = "New Task";
            this.newTask.Location = new System.Drawing.Point(20, 180);
            this.newTask.Click += new EventHandler(this.NewTask_Click);

            this.submitButton = new Button();
            this.submitButton.Text = "Submit";
            this.submitButton.Location = new System.Drawing.Point(100, 180);
            this.submitButton.Click += new EventHandler(this.SubmitButton_Click);

            // Add controls to the form
            this.Controls.Add(this.taskNameTextBox);
            this.Controls.Add(this.informationTextBox);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.newTask);
        }

        private void NewTask_Click(object sender, EventArgs e)
        {
            string taskName = taskNameTextBox.Text;
            string information = informationTextBox.Text;

            TaskInfo task = new TaskInfo
            {
                TaskName = taskName,
                Information = information
            };

            tasks.Add(task);

            taskNameTextBox.Clear();
            informationTextBox.Clear();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            string taskName = taskNameTextBox.Text;
            string information = informationTextBox.Text;

            TaskInfo task = new TaskInfo
            {
                TaskName = taskName,
                Information = information
            };

            tasks.Add(task);

            string wallpaperFilePath = GenerateWallpaperImage(tasks);
            SetWallpaper(wallpaperFilePath);

            taskNameTextBox.Clear();
            informationTextBox.Clear();
        }

        public class TaskInfo
        {
            public string TaskName { get; set; }
            public string Information { get; set; }
            // Add more properties as needed
        }

        private void SetWallpaper(string filePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        private string GenerateWallpaperImage(List<TaskInfo> tasks)
        {
            // Create a Bitmap to draw the information
            Bitmap bitmap = new Bitmap(800, 600); // Set your desired image dimensions
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Clear the background
                graphics.Clear(Color.White);

                int startingPoint = 60;

                // Draw the project information
                using (Font font = new Font("Arial", 14))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    for (int i = 0; i < tasks.Count(); i++)
                    {
                        string projectName = tasks[i].TaskName;
                        string description = tasks[i].Information; 
                        //graphics.DrawString("Project Name: " + projectName, font, brush, new PointF(20, 20));
                        graphics.DrawString(projectName + ": " + description, font, brush, new PointF(20, startingPoint));
                        startingPoint += 20;
                    }
                }
            }

            // Save the image to a file
            string imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ProjectWallpaper.png");
            bitmap.Save(imagePath, ImageFormat.Png);

            return imagePath;
        }

        #endregion
    }
}