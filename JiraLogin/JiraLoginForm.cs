using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiraLogin
{
    public partial class JiraLoginForm : Form
    {
        public JiraLoginForm()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, EventArgs e)
        {

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(1, 0, 0, 0, 0);

                var byteArray = Encoding.ASCII.GetBytes(UsernameTextBox.Text + ":" + PasswordTextBox.Text);//eliohaddad98@outlook.com:UqQQDaQgyowQhkSlAwux7DEB
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string loginUrl = JiraUrlTextBox.Text + "/rest/auth/latest/session";//https://eliohaddad.atlassian.net

                //var response = client.PostAsync(sUri, content);
                var response = client.GetAsync(loginUrl).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var newclient = new HttpClient())
                    {
                        newclient.Timeout = new TimeSpan(1, 0, 0, 0, 0);
                        
                        var newByteArray = Encoding.ASCII.GetBytes(UsernameTextBox.Text + ":" + PasswordTextBox.Text);
                        newclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(newByteArray));
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                        string taskUrl = JiraUrlTextBox.Text + "/rest/api/latest/search";
                        var newResponse = client.GetAsync(taskUrl).Result;
                        string newResult = newResponse.Content.ReadAsStringAsync().Result;
                        UserTaskResponse userTaskResponse = JsonConvert.DeserializeObject<UserTaskResponse>(newResult);
                        this.Hide();
                        JiraUserTasks usertasks = new JiraUserTasks(userTaskResponse,UsernameTextBox.Text);
                        usertasks.BringToFront();
                        usertasks.AutoScroll = true;
                        usertasks.ShowDialog();
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }



            }
        }
    }
}
