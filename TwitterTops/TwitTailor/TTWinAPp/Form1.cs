using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.IO;
using TTWinAPp.Model;

namespace TTWinAPp
{
    public partial class Form1 : Form
    {
        int max_love_id;
        int max_joke_id;
        int max_facts_id;
        public Form1()
        {
            InitializeComponent();
        }

        private void createAuthToken()
        {
            try
            {
                var oAuthConsumerKey = "LKWgT2LyALDIH0QGax2ARp5Yt";
                var oAuthConsumerSecret = "9RDsrlIoJANz1G5xHr0HMWqV4MCHvyhNazCFd33WGnsVno7ptj";
                var oAuthUrl = ConfigurationManager.AppSettings["TwitAuthAPI"].ToString();
                if (string.IsNullOrEmpty(oAuthUrl))
                    throw new Exception("Twitter AUTH API Url is not found or invalid");

                // Do the Authenticate
                var authHeaderFormat = "Basic {0}";

                var authHeader = string.Format(authHeaderFormat,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                    Uri.EscapeDataString((oAuthConsumerSecret)))
                ));

                var postBody = "grant_type=client_credentials";

                HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
                authRequest.Headers.Add("Authorization", authHeader);
                authRequest.Method = "POST";
                authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (Stream stream = authRequest.GetRequestStream())
                {
                    byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }

                authRequest.Headers.Add("Accept-Encoding", "gzip");

                WebResponse authResponse = authRequest.GetResponse();
                // deserialize into an object
                using (authResponse)
                {
                    using (var reader = new StreamReader(authResponse.GetResponseStream()))
                    {
                        //JavaScriptSerializer js = new JavaScriptSerializer();
                        //var objectText = reader.ReadToEnd();
                        //twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //createAuthToken();           
           
        }

        private int getActiveTabIndex()
        {
            if (tabControl1.SelectedTab.Text.ToLower().Contains("love"))
            {
                return 0;
            }
            else if (tabControl1.SelectedTab.Text.ToLower().Contains("jokes"))
            {
                return 1;
            }
            else if (tabControl1.SelectedTab.Text.ToLower().Contains("facts"))
            {
                return 2;
            }
            else
                return -1;
        }

        TRDBEntities context = new TRDBEntities();
        private void button1_Click(object sender, EventArgs e)
        {
            btnSelectAlll.Enabled = true;
            if (getActiveTabIndex() == 0)
                fetchLoveTweets();
            else if (getActiveTabIndex() == 1)
                fetchJokeTweets();
            else if (getActiveTabIndex() == 2)
                fetchFactsTweets();
        }

        private void fetchLoveTweets()
        {
            max_love_id = Properties.Settings.Default.MAX_LOVE_ID;            
            lblStatus.Text = "fetching Love Tweets from DB";
            dgv1.Rows.Clear();
            var q1 = (from c in context.tblCatLoves
                      where c.MediaUrl.Length > 0 && c.Id > max_love_id
                      select c).OrderBy(c => c.Id).Take(100);
            var users = q1.ToList();

            DataTable dtEmp = new DataTable();
            int i = 0;
            foreach (var rows in users)
            {
                if (i == (users.Count - 1))
                    max_love_id = rows.Id;
                DataGridViewRow row = (DataGridViewRow)dgv1.Rows[i].Clone();
                row.Cells[3].Value = rows.text;
                row.Cells[1].Value = "Delete - " + rows.Id;
                Image mediaImage = GetImageFromUrl(rows.MediaUrl);
                if (mediaImage != null)
                    row.Cells[2].Value = mediaImage;
                if (dgv1.Columns[2] is DataGridViewImageColumn)
                {
                    ((DataGridViewImageColumn)dgv1.Columns[2]).ImageLayout = DataGridViewImageCellLayout.Normal;

                }
                row.Cells[4].Value = rows.screenname;
                row.Cells[5].Value = rows.created_at;
                dgv1.Rows.Add(row);
                lblStatus.Text += ".";
                i++;
            }
            lblStatus.Text = "fetching Love Tweets from DB completed...";
        }

        private void fetchJokeTweets()
        {
            max_joke_id = Properties.Settings.Default.MAX_JOKES_ID;
            lblStatus.Text = "fetching Joke Tweets from DB";
            dgv2.Rows.Clear();
            var q1 = (from c in context.tblCatJokes
                      where c.MediaUrl.Length > 0 && c.Id > max_joke_id
                      select c).OrderBy(c => c.Id).Take(100);
            var users = q1.ToList();
            int i = 0;
            foreach (var rows in users)
            {
                if (i == (users.Count - 1))
                    max_joke_id = rows.Id;
                DataGridViewRow row = (DataGridViewRow)dgv2.Rows[i].Clone();
                row.Cells[3].Value = rows.text;
                row.Cells[1].Value = "Delete - " + rows.Id;
                Image mediaImage = GetImageFromUrl(rows.MediaUrl);
                if (mediaImage != null)
                    row.Cells[2].Value = mediaImage;
                if (dgv1.Columns[2] is DataGridViewImageColumn)
                {
                    ((DataGridViewImageColumn)dgv1.Columns[2]).ImageLayout = DataGridViewImageCellLayout.Stretch;

                }
                row.Cells[4].Value = rows.screenname;
                row.Cells[5].Value = rows.created_at;
                dgv2.Rows.Add(row);
                lblStatus.Text += ".";
                i++;
            }
            lblStatus.Text = "fetching Jokes Tweets from DB completed...";
        }

        private void fetchFactsTweets()
        {
            max_facts_id = Properties.Settings.Default.MAX_FACTS_ID;
            lblStatus.Text = "fetching Facts Tweets from DB";
            dgv3.Rows.Clear();
            var q1 = (from c in context.tblCatFacts
                      where c.MediaUrl.Length > 0 && c.Id > max_facts_id
                      select c).OrderBy(c => c.Id).Take(100);
            var users = q1.ToList();

            int i = 0;
            foreach (var rows in users)
            {
                if (i == (users.Count - 1))
                    max_facts_id = rows.Id;
                DataGridViewRow row = (DataGridViewRow)dgv3.Rows[i].Clone();
                row.Cells[3].Value = rows.text;
                row.Cells[1].Value = "Delete - " + rows.Id;
                Image mediaImage = GetImageFromUrl(rows.MediaUrl);
                if (mediaImage != null)
                    row.Cells[2].Value = mediaImage;
                row.Cells[4].Value = rows.screenname;
                row.Cells[5].Value = rows.created_at;
                dgv3.Rows.Add(row);
                lblStatus.Text += ".";
                i++;
            }
            lblStatus.Text = "fetching Facts Tweets from DB completed...";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // assume your DataGridViewImageColumn name is Column1, you can change this as your column name
            if (dgv1.Columns[e.ColumnIndex].Name == "col2")
            {
                //e.Value = GetImageFromUrl("");
            }
        }

        // below method will download the image from given url
        public static Image GetImageFromUrl(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                // if you have proxy server, you may need to set proxy details like below 
                //httpWebRequest.Proxy = new WebProxy("proxyserver",port){ Credentials = new NetworkCredential(){ UserName ="uname", Password = "pw"}};

                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream stream = httpWebReponse.GetResponseStream())
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (getActiveTabIndex() == 0)
            {
                for (int i = 0; i < dgv1.RowCount; i++)
                {
                    dgv1.Rows[i].Cells[0].Value = true;
                }
                if (dgv1.Rows.Count > 0)
                    btnMakeModerated.Enabled = true;
                else
                    btnMakeModerated.Enabled = false;
            }
            else if (getActiveTabIndex() == 1)
            {
                for (int i = 0; i < dgv2.RowCount; i++)
                {
                    dgv2.Rows[i].Cells[0].Value = true;
                }
                if (dgv2.Rows.Count > 0)
                    btnMakeModerated.Enabled = true;
                else
                    btnMakeModerated.Enabled = false;
            }
            else if (getActiveTabIndex() == 2)
            {
                for (int i = 0; i < dgv3.RowCount; i++)
                {
                    dgv3.Rows[i].Cells[0].Value = true;
                }
                if (dgv3.Rows.Count > 0)
                    btnMakeModerated.Enabled = true;
                else
                    btnMakeModerated.Enabled = false;
            }
        }

        private void btnMakeModerated_Click(object sender, EventArgs e)
        {
            btnMakeModerated.Enabled = false;
            btnSelectAlll.Enabled = false;
            if (getActiveTabIndex() == 0)
            {
                dgv1.Rows.Clear();
                Properties.Settings.Default.MAX_LOVE_ID = max_love_id;
            }
            else if (getActiveTabIndex() == 1)
            {
                dgv2.Rows.Clear();
                Properties.Settings.Default.MAX_JOKES_ID = max_joke_id;
            }
            else if (getActiveTabIndex() == 2)
            {
                dgv3.Rows.Clear();
                Properties.Settings.Default.MAX_FACTS_ID = max_facts_id;
            }
            Properties.Settings.Default.Save();
        }
        TRDBEntities entitiesContext = new TRDBEntities();
        tblCatLove loveEntity;
        tblCatJoke jokeEntity;
        tblCatFact factEntity;
        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //loveEntity = new tblCatLove();
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                lblStatus.Text = "Deleting tweet in progress";
                //TODO - Button Clicked - Execute Code Here
                //senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                int id = Convert.ToInt32(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Delete - ", ""));
                loveEntity = entitiesContext.tblCatLoves.Where(m => m.Id == id).FirstOrDefault();
                if (loveEntity != null)
                {
                    loveEntity.MediaUrl = null;
                    loveEntity.MediaType = null;
                    loveEntity.Display_Url = null;
                    loveEntity.expanded_url = null;
                    loveEntity.Url = null;
                    entitiesContext.Entry(loveEntity).State = EntityState.Modified;
                }
                lblStatus.Text = "Deleting tweet completed. Id : " + id;
                entitiesContext.SaveChanges();
            }
        }

        private void dgv3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //loveEntity = new tblCatLove();
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                lblStatus.Text = "Deleting tweet in progress";
                //TODO - Button Clicked - Execute Code Here
                //senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                int id = Convert.ToInt32(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Delete - ", ""));
                factEntity = entitiesContext.tblCatFacts.Where(m => m.Id == id).FirstOrDefault();
                if (factEntity != null)
                {
                    factEntity.MediaUrl = null;
                    factEntity.MediaType = null;
                    factEntity.Display_Url = null;
                    factEntity.expanded_url = null;
                    factEntity.Url = null;
                    entitiesContext.Entry(factEntity).State = EntityState.Modified;
                }
                lblStatus.Text = "Deleting tweet completed. Id : " + id;
                entitiesContext.SaveChanges();
            }
        }

        private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //loveEntity = new tblCatLove();
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                lblStatus.Text = "Deleting tweet in progress";
                //TODO - Button Clicked - Execute Code Here
                //senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value
                int id = Convert.ToInt32(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Delete - ", ""));
                jokeEntity = entitiesContext.tblCatJokes.Where(m => m.Id == id).FirstOrDefault();
                if (jokeEntity != null)
                {
                    jokeEntity.MediaUrl = null;
                    jokeEntity.MediaType = null;
                    jokeEntity.Display_Url = null;
                    jokeEntity.expanded_url = null;
                    jokeEntity.Url = null;
                    entitiesContext.Entry(jokeEntity).State = EntityState.Modified;
                }
                entitiesContext.SaveChanges();
                lblStatus.Text = "Deleting tweet completed. Id : " + id;
            }
        }

        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            fetchLoveTweets();
            fetchJokeTweets();
            fetchFactsTweets();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            frmTwitterManager frmTM = new frmTwitterManager();
            frmTM.ShowDialog();
            this.Close();
        }
    }
}
