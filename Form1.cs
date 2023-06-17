using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Ultimate_Predoctor
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "Ultimate Predictor ";
        private readonly string PREDICTION_CONFIG_PATH = $"{Environment.CurrentDirectory}\\predictionsConfigEN.json";
        private string[] _predictions;
        private Random _random = new Random();
        private int _indexIcon = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            bPredict.Enabled = false;
            PictureChange();

            await Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    UpdateProgressBar(i);
                    Text = $"{i}%";
                    Thread.Sleep(10);
                }
            });

            var index = _random.Next(_predictions.Length);
            var prediction = _predictions[index];

            MessageBox.Show(prediction);

            progressBar1.Value = 0;
            Text = APP_NAME;
            bPredict.Enabled = true;
            PictureChange();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            Text = APP_NAME;
            
            try
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_PATH);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
                
                if (_predictions == null || _predictions.Length == 0)
                {
                    MessageBox.Show("Can't predict at the moment.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                Close();
            }
        }

        private void UpdateProgressBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else
            {
                progressBar1.Value = i + 1;
            }
            progressBar1.Value = i;
        }

        private void PictureChange()
        {
            Image image = imageList1.Images[_indexIcon];
            Icon icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            Icon = icon;

            _indexIcon = (_indexIcon + 1) % imageList1.Images.Count;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}