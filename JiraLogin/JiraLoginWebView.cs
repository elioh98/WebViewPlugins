using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraLogin
{
    public partial class JiraLoginWebView : Form
    {
        public JiraLoginWebView()
        {
            InitializeComponent();
            //InitializeAsync();


        }

        async void JiraLoginWebView_Load(object sender, EventArgs e)
        {
            
            //webView21.CoreWebView2InitializationCompleted += webView2_CoreWebView2InitializationCompleted;
            InitializeWebView2Async("");
            


        }
        private string getHtml()
        {
            string html = @"<html>
            <head>
            <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
            <script src=""https://code.jquery.com/jquery-3.3.1.slim.min.js"" integrity=""sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"" crossorigin=""anonymous""></script>
            <script src=""https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js"" integrity=""sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1"" crossorigin=""anonymous""></script>
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js"" integrity=""sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM"" crossorigin=""anonymous""></script>
            </head>
            <body>
            <form>
            <div class=""form-group"">
            <label for=""jiraurl"">Jira Url</label>
            <input type=""url"" class=""form-control"" id=""jiraurl""  placeholder=""Enter Jira URL"">
            </div>
            <div class=""form-group"">
            <label for=""jiraemail"">Email address</label>
            <input type=""email"" class=""form-control"" id=""jiraemail""  placeholder=""Enter email"">
            </div>
            <div class=""form-group"">
            <label for=""apikey"">Api Key</label>
            <input type=""password"" class=""form-control"" id=""apikey"" placeholder=""Enter Api Key"">
            </div>
            
            </form>
            
            </body>
            </html>";
            /* This was the button code removed : <button id = ""button"" type=""submit"" class=""btn btn-primary"" onclick=""myFunction()"">Submit</button>
             * <script>
            function myFunction() {
            let jiralogincreds = '{""jiraurl"" : document.getElementById('jiraurl').innerText ,""jiraemail"" : document.getElementById('jiraemail').innerText ,""apikey"" : document.getElementById('apikey').innerText }' ;
            window.chrome.webview.postMessage(jiralogincreds);};
            </script>*/
            return html;
        }


        async void WebView21_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {

            JiraLoginWebViewDTO jiraloginwebview = JsonConvert.DeserializeObject<JiraLoginWebViewDTO>(e.TryGetWebMessageAsString());
        }

        async void InitializeWebView2Async(string tempDir = "")
        {

            CoreWebView2Environment webView2Environment = null;


            string tempDir2 = tempDir;

            if (String.IsNullOrEmpty(tempDir2))
            {

                tempDir2 = Path.GetTempPath();
            }
            CoreWebView2EnvironmentOptions options = null;
            string html = getHtml();


            webView2Environment = await CoreWebView2Environment.CreateAsync(null, tempDir2, options);
            await webView21.EnsureCoreWebView2Async(webView2Environment);
            webView21.CoreWebView2.Settings.AreHostObjectsAllowed = true;
            webView21.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
            webView21.CoreWebView2.Settings.IsScriptEnabled = true;
            webView21.CoreWebView2.Settings.IsWebMessageEnabled = true;
            webView21.WebMessageReceived += WebView21_WebMessageReceived;
            webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
            //string x = await webView21.ExecuteScriptAsync(html);
            webView21.NavigateToString(html);


        }
        private void CallSecondForm(JiraLoginWebViewDTO webviewdto)
        {

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(1, 0, 0, 0, 0);

                var byteArray = Encoding.ASCII.GetBytes(webviewdto.jiraemail + ":" + webviewdto.apikey);//eliohaddad98@outlook.com:UqQQDaQgyowQhkSlAwux7DEB
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string loginUrl = webviewdto.jiraurl + "/rest/auth/latest/session";//https://eliohaddad.atlassian.net

                //var response = client.PostAsync(sUri, content);
                var response = client.GetAsync(loginUrl).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var newclient = new HttpClient())
                    {
                        newclient.Timeout = new TimeSpan(1, 0, 0, 0, 0);

                        var newByteArray = Encoding.ASCII.GetBytes(webviewdto.jiraemail + ":" + webviewdto.apikey);
                        newclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(newByteArray));
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                        string taskUrl = webviewdto.jiraurl + "/rest/api/latest/search";
                        var newResponse = client.GetAsync(taskUrl).Result;
                        string newResult = newResponse.Content.ReadAsStringAsync().Result;
                        UserTaskResponse userTaskResponse = JsonConvert.DeserializeObject<UserTaskResponse>(newResult);
                        this.Hide();
                        JiraLoginWebViewUserTasks usertasks = new JiraLoginWebViewUserTasks(userTaskResponse, webviewdto.jiraemail);
                        usertasks.BringToFront();
                        //usertasks.AutoScroll = true;
                        usertasks.Show();
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JiraLoginWebViewDTO webviewdto = new JiraLoginWebViewDTO();
            webviewdto.jiraurl = "https://eliohaddad.atlassian.net"; //webView21.ExecuteScriptAsync("document.getElementById('jiraurl').value;").Result;
            webviewdto.jiraemail = "eliohaddad98@outlook.com";//webView21.ExecuteScriptAsync("document.getElementById('jiraemail').value;").Result;
            webviewdto.apikey = "vK5CkxG3M28wYb8WjqoY5F11"; //webView21.ExecuteScriptAsync("document.getElementById('apikey').value;").Result;
            CallSecondForm(webviewdto);
        }
    }

}
