using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraLogin
{
    public partial class JiraLoginWebViewUserTasks : Form
    {
        public JiraLoginWebViewUserTasks(UserTaskResponse response, string username)
        {
            InitializeComponent();
            InitializeWebView2Async(response, username, "");
        }
        async void InitializeWebView2Async(UserTaskResponse response, string username, string tempDir = "")
        {

            CoreWebView2Environment webView2Environment = null;


            string tempDir2 = tempDir;

            if (String.IsNullOrEmpty(tempDir2))
            {

                tempDir2 = Path.GetTempPath();
            }
            CoreWebView2EnvironmentOptions options = null;
            string html = getHtml(response, username);


            webView2Environment = await CoreWebView2Environment.CreateAsync(null, tempDir2, options);
            await webView21.EnsureCoreWebView2Async(webView2Environment);

            //string x = await webView21.ExecuteScriptAsync(html);
            webView21.NavigateToString(html);

        }
        async void JiraLoginWebViewUserTasks_Load(object sender, EventArgs e)
        {

            //webView21.CoreWebView2InitializationCompleted += webView2_CoreWebView2InitializationCompleted;


        }
        private string getHtml(UserTaskResponse response, string username)
        {
            string html = @"<html>
            <head>
            <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
            <script src=""https://code.jquery.com/jquery-3.3.1.slim.min.js"" integrity=""sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"" crossorigin=""anonymous""></script>
            <script src=""https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js"" integrity=""sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1"" crossorigin=""anonymous""></script>
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js"" integrity=""sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM"" crossorigin=""anonymous""></script>
            </head>
            <body>
            <style>
.how-section1{
    margin-top:-15%;
    padding: 10%;
}
.how-section1 h4{
    color: #ffa500;
    font-weight: bold;
    font-size: 30px;
}
.how-section1 .subheading{
    color: #3931af;
    font-size: 20px;
}
.how-section1 .row
{
    margin-top: 10%;
}
.how-img 
{
    text-align: center;
}
.how-img img{
    width: 40%;
} </style>
<div class=""how - section1"">";
            foreach (Issue issue in response.issues)
            {
                if (issue.fields["assignee"]["emailAddress"] == username)
                {
                    html += @"<div class=""row"" style="" outline: 0.01em solid black;  ""><div class=""col-md-6 how-img"">
                            <img src=";
                    html += issue.fields["assignee"]["avatarUrls"]["16x16"];
                    html += @"class=""rounded-circle img-fluid"" alt=""""/>
                        </div>
                        <div class=""col-md-6"">
                            <h4>";
                    html+= issue.key;
                    html += @"</h4>
                                      
                        <p class=""text-muted"">";
                        html+= issue.fields["description"];

                    html+=@"</p>
                        </div>
                    </div>




";


                }
                html += "<br><br>";
            }
            html += @"</ body > </ html > </div>";


            return html;

        }
    }
    }
