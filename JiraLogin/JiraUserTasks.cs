using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraLogin
{
    public partial class JiraUserTasks : Form
    {
        public JiraUserTasks(UserTaskResponse response,string username)
        {
            InitializeComponent();
            fillUserTaskForm(response,username);
        }
        
        class OvalPictureBox : PictureBox
        {
            public OvalPictureBox()
            {
                this.BackColor = Color.DarkGray;
            }
            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                using (var gp = new GraphicsPath())
                {
                    gp.AddEllipse(new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                    this.Region = new Region(gp);
                }
            }
        }
        public void fillUserTaskForm(UserTaskResponse response,string username)
        {
            Boolean isFirstGroup = false;
            int groupSize = 0;
            FlowLayoutPanel panel1 = new FlowLayoutPanel();
            panel1.Location = new System.Drawing.Point(2,2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(3000, 3000);
            panel1.TabIndex = 0;
            panel1.AutoScroll = true;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            foreach (Issue issue in response.issues)
            {
                if (issue.fields["assignee"]["emailAddress"] == username)
                {
                    GroupBox groupBox1 = new System.Windows.Forms.GroupBox();
                    OvalPictureBox pictureBox1 = new OvalPictureBox();
                    Label label1 = new System.Windows.Forms.Label();
                    Label label2 = new System.Windows.Forms.Label();
                    groupBox1.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
                    //SuspendLayout();
                    panel1.SuspendLayout();

                    // 
                    // groupBox1
                    // 
                    if (isFirstGroup)
                    {
                        groupBox1.Location = new System.Drawing.Point(10,10);
                        groupSize = groupBox1.Location.Y;
                        isFirstGroup = true;

                    }
                    else
                    {
                        groupBox1.Location = new System.Drawing.Point(61, groupSize + 300);
                        groupSize = groupBox1.Location.Y;
                    }
                    groupBox1.Name = "groupBox";/*+ counter.ToString();*/
                    groupBox1.Size = new System.Drawing.Size(1200,300);
                    groupBox1.TabIndex = 0;
                    groupBox1.TabStop = false;
                    // 
                    // pictureBox1
                    // 
                    pictureBox1.ImageLocation = issue.fields["assignee"]["avatarUrls"]["16x16"];
                    pictureBox1.Location = new System.Drawing.Point(15, 20);
                    pictureBox1.Name = "pictureBox1";
                    pictureBox1.Size = new System.Drawing.Size(30,30);
                    pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                    pictureBox1.TabIndex = 0;
                    pictureBox1.TabStop = false;
                    // 
                    // label1
                    // 
                    label1.AutoSize = true;
                    label1.Location = new System.Drawing.Point(500, 20);
                    label1.Name = "label1";
                    label1.Size = new System.Drawing.Size(39, 17);
                    label1.TabIndex = 1;
                    label1.Text = issue.key;
                    // 
                    // label2
                    // 
                    label2.AutoSize = true;
                    label2.Location = new System.Drawing.Point(500, label1.Location.Y + 70);
                    label2.Name = "label2";
                    label2.Size = new System.Drawing.Size(46, 17);
                    label2.TabIndex = 2;
                    label2.Text = issue.fields["description"];
                    groupBox1.Controls.Add(label2);
                    groupBox1.Controls.Add(label1);
                    groupBox1.Controls.Add(pictureBox1);
                    panel1.Controls.Add(groupBox1);
                    panel1.SetAutoScrollMargin(20, 20);
                    ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
                    //groupBox1.Padding = new Padding(0);
                    this.Controls.Add(groupBox1);
                    //groupBox1.AutoSize = true;
                }
            }
            ClientSize = new System.Drawing.Size(1920,1080);
            Controls.Add(panel1);
            Name = "JiraUserTasks";
            //panel1.anchor = anchorstyles.top | anchorstyles.right | anchorstyles.bottom | anchorstyles.left;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            panel1.ResumeLayout(false);
            panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;
            panel1.PerformLayout();
            panel1.FlowDirection = FlowDirection.TopDown;
            this.AutoScroll = true;
            

        }


    }
}
