using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//추가
using System.Net.Http;
using System.Net;
using System.Threading;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //-----------------------------------
        // 백그라운드 수신 메세지
        //-----------------------------------
        private BackgroundWorker recvLEDWorker;
        private BackgroundWorker recvTMPWorker;
        private BackgroundWorker recvLightWorker;
        private BackgroundWorker recvDISWorker;


        private void conn_btn_Click(object sender, EventArgs e)
        {
            
            
            String port =  this.comboBox1.Items[  this.comboBox1.SelectedIndex  ].ToString();
            Console.WriteLine("PORT : " + port);
            HttpWebRequest request=null;
            HttpWebResponse response = null;
            try
            {   
                request =  (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/connection/" + port);
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;

                response = (HttpWebResponse)request.GetResponse();
           
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("RESPONSE CODE : " + response.StatusCode);
                    // 정보 메세지 
                    //------------------------------------------------
                        // 수신 스레드 객체 생성
                    //------------------------------------------------
                    // LED
                    recvLEDWorker = new BackgroundWorker();
                    recvLEDWorker.DoWork += recvLEDinfo;
                    // TMP
                    recvTMPWorker = new BackgroundWorker();
                    recvTMPWorker.DoWork += recvTMPinfo;
                    // LIGHT
                    recvLightWorker = new BackgroundWorker();
                    recvLightWorker.DoWork += recvLightinfo;
                    // DIS
                    recvDISWorker = new BackgroundWorker();
                    recvDISWorker.DoWork += recvDISinfo;

                    //------------------------------------------------
                    // 수신 스레드 실행
                    //------------------------------------------------
                    recvLEDWorker.RunWorkerAsync();
                    recvTMPWorker.RunWorkerAsync();
                    recvLightWorker.RunWorkerAsync();
                    recvDISWorker.RunWorkerAsync();


                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Ex : " + ex);
            }


        }

        // LED 메세지 스레드
        private void recvLEDinfo(object sender, EventArgs e)
        {
            while (!recvLEDWorker.CancellationPending)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/led");
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream =response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                // 메세지 담아주기
                //invoke(new Action(()=>{}))

                //LED
                Invoke(new Action(() =>
                {
                    this.LED_Txt.Text = reader.ReadToEnd();
                }));
                

                Thread.Sleep(1000);
            }
        }

                private void recvTMPinfo(object sender, EventArgs e)
        {
            while (!recvTMPWorker.CancellationPending)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/tmp");
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream =response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                // 메세지 담아주기
                //invoke(new Action(()=>{}))

                //LED
                Invoke(new Action(() =>
                {
                    this.TMP_Txt.Text = reader.ReadToEnd();
                }));
                

                Thread.Sleep(1000);
            }
        }

        private void recvLightinfo(object sender, EventArgs e)
        {
            while (!recvLightWorker.CancellationPending)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/light");
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                // 메세지 담아주기
                //invoke(new Action(()=>{}))

                //LED
                Invoke(new Action(() =>
                {
                    this.Light_Txt.Text = reader.ReadToEnd();
                }));


                Thread.Sleep(1000);
            }
        }

        private void recvDISinfo(object sender, EventArgs e)
        {
            while (!recvDISWorker.CancellationPending)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/distance");
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                // 메세지 담아주기
                //invoke(new Action(()=>{}))

                //LED
                Invoke(new Action(() =>
                {
                    this.DIS_Txt.Text = reader.ReadToEnd();
                }));


                Thread.Sleep(1000);
            }
        }






        //------------------------------------------------------------------------------------------------------------------------------------------

        private void led_on_btn_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/led/1");
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private void led_off_btn_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/led/0");
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        }
    }
}
