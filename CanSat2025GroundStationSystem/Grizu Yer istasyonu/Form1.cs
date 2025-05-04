using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Data.SqlClient;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Globalization;
using System.Windows.Controls;
using System.Reflection.Emit;



namespace Grizu_Yer_istasyonu
{
    public partial class Form1 : Form
    {
        private float roll = 0f;
        private float pitch = 0f;
        private float yaw = 0f;

        int portsayisi = 0;
        public Form1()
        {
            InitializeComponent();
        }
 
        private void Form1_Load(object sender, EventArgs e)
        {
            //Data Gried Viev Düzenlemeleri
            dataGridView1.ColumnCount = 13;
            dataGridView1.Columns[0].Name = "Team ID";
            dataGridView1.Columns[1].Name = "Mission Time";
            dataGridView1.Columns[2].Name = "Packet Count";
            dataGridView1.Columns[3].Name = "Mode";
            dataGridView1.Columns[4].Name = "State";
            dataGridView1.Columns[5].Name = "Altitude";
            dataGridView1.Columns[6].Name = "Tempurature";
            dataGridView1.Columns[7].Name = "Pressure";
            dataGridView1.Columns[8].Name = "Voltage";
            dataGridView1.Columns[9].Name = "GYRO_R";
            dataGridView1.Columns[10].Name = "GYRO_P";
            dataGridView1.Columns[11].Name = "GYRO_Y";
            dataGridView1.Columns[12].Name = "ACCEL_R";

            dataGridView2.ColumnCount = 12;
            dataGridView2.Columns[0].Name = "ACCEL_P";
            dataGridView2.Columns[1].Name = "ACCEL_Y";
            dataGridView2.Columns[2].Name = "MAG_R";
            dataGridView2.Columns[3].Name = "MAG_P";
            dataGridView2.Columns[4].Name = "MAG_Y";
            dataGridView2.Columns[5].Name = "AUTO GYRO RR";
            dataGridView2.Columns[6].Name = "GPS Time";
            dataGridView2.Columns[7].Name = "GPS ALT";
            dataGridView2.Columns[8].Name = "GPS LAT";
            dataGridView2.Columns[9].Name = "GPS LONG";
            dataGridView2.Columns[10].Name = "GPS SATS";
            dataGridView2.Columns[11].Name = "CMD ECHO";

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 70;
            dataGridView1.Columns[4].Width = 55;
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[6].Width = 120;
            dataGridView1.Columns[7].Width = 90;
            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[9].Width = 90;
            dataGridView1.Columns[10].Width = 90;
            dataGridView1.Columns[11].Width = 90;
            dataGridView1.Columns[12].Width = 98;

            dataGridView2.Columns[0].Width = 100;
            dataGridView2.Columns[1].Width = 100;
            dataGridView2.Columns[2].Width = 80;
            dataGridView2.Columns[3].Width = 80;
            dataGridView2.Columns[4].Width = 80;
            dataGridView2.Columns[5].Width = 130;
            dataGridView2.Columns[6].Width = 90;
            dataGridView2.Columns[7].Width = 90;
            dataGridView2.Columns[8].Width = 90;
            dataGridView2.Columns[9].Width = 90;
            dataGridView2.Columns[10].Width = 90;
            dataGridView2.Columns[11].Width = 90;

            //Harita Kodları
            gMapControl1.MapProvider = GMapProviders.GoogleMap; 
            GMaps.Instance.Mode = AccessMode.ServerOnly;       
            gMapControl1.Position = new PointLatLng(41.449560015109554, 31.758533316730077); 
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 18;
            gMapControl1.Zoom = 14;

            //Serial port control paneli düzenlemeleri
            foreach (var seriPort in SerialPort.GetPortNames())
            {
                comboBoxPort.Items.Add(seriPort);
                portsayisi++;
            }
            if (portsayisi > 0)
            {
                comboBoxPort.SelectedIndex = 1;
            }
            comboBoxBaudrate.Items.Add(9600);
            comboBoxBaudrate.Items.Add(19200);
            comboBoxBaudrate.Items.Add(115200);
            comboBoxBaudrate.SelectedIndex = 1;
            serialPort1.ReadTimeout = 500;

            comboBoxCommand.Items.Add("CX ON");
            comboBoxCommand.Items.Add("CX OF");
            comboBoxCommand.Items.Add("CAL");
            comboBoxCommand.Items.Add("MEC ON");
            comboBoxCommand.Items.Add("MEC OF");
            comboBoxCommand.Items.Add("ST");
            comboBoxCommand.Items.Add("SIM ON");
            comboBoxCommand.Items.Add("SIM OF");

            GL.ClearColor(Color.Aqua);
            timer1.Interval = 1;

        }
        //Satır Kayması Sağlayacak
        int currentRow = 0;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                
                if (currentRow < dataGridView1.Rows.Count - 1)
                {
                    float.TryParse(dataGridView1.Rows[currentRow].Cells[9].Value?.ToString(), out float x);
                    float.TryParse(dataGridView1.Rows[currentRow].Cells[10].Value?.ToString(), out float y);
                    float.TryParse(dataGridView1.Rows[currentRow].Cells[11].Value?.ToString(), out float z);
                    
                }
                currentRow++;

                if (currentRow >= dataGridView1.Rows.Count - 1)
                {
                    currentRow = 0;
                }
            }
           
            glControl1.Invalidate();

        }

        private void comboBoxPort_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboBoxPort.Text;
                string mesaj = "New Port Name: " + comboBoxPort.Text;
                MessageBox.Show(mesaj);
            }
            else
            {
                string mesaj = "Try Again After Closing the Serial Port!";
                MessageBox.Show(mesaj);
            }
        }

        private void btnopen_Click_1(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBoxPort.Items.Count > 0)
                {
                    serialPort1.PortName = comboBoxPort.Text;
                }
                serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudrate.Text);
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("Port is Open: " + serialPort1.PortName + "-" + serialPort1.BaudRate);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("An error occurred while opening the port:" + ex.Message);
                }

            }

            if (serialPort1.IsOpen)
            {
                btnopen.Enabled = false;
                btnclose.Enabled = true;
            }
        }

        private void btnclose_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.WriteTimeout = 100;
                serialPort1.Close();
                MessageBox.Show("Port is Close: " + serialPort1.PortName + "-" + serialPort1.BaudRate);
            }
            if (!serialPort1.IsOpen)
            {
                btnopen.Enabled = true;
                btnclose.Enabled = false;
            }
        }
        private void btnrefresh_Click_1(object sender, EventArgs e)
        {
            comboBoxPort.Items.Clear();
            portsayisi = 0;
            if (!serialPort1.IsOpen)
            {
                foreach (var seriPort in SerialPort.GetPortNames())
                {
                    comboBoxPort.Items.Add(seriPort);
                    portsayisi++;
                }
                string mesaj = "Port entries have been refreshed!";
                MessageBox.Show(mesaj);
                if (comboBoxPort.Items.Count > 0)
                {
                    comboBoxPort.SelectedIndex = 1;
                }
            }
            else
            {
                string mesaj = "Try Again After Closing the Port!";
                MessageBox.Show(mesaj);
            }
        }
        private void comboBoxBaudrate_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudrate.Text);
                string mesaj = "New Port Baudrate: " + comboBoxBaudrate.Text;
                MessageBox.Show(mesaj);
            }
            else
            {
                string mesaj = "Try Again After Closing the Serial Port!";
                MessageBox.Show(mesaj);
            }
        }
        /*private void cxon_Click(object sender, EventArgs e)
        {
            string oncommand = "CMD,3159,CX,ON";

            serialPort1.WriteLine(oncommand);

            Console.WriteLine("Start command is sent");
        }

        private void cxoff_Click(object sender, EventArgs e)
        {
            string offcommand = "CMD,3159,CX,OFF";

            serialPort1.WriteLine(offcommand);

            Console.WriteLine("Stop command is sent");
        }

        private bool simulationMode = false;

        private void simulation_Click(object sender, EventArgs e)
        {
            if (simulationMode)
            {
                string stopSim = "CMD,3159,SIM,DISABLE";
                serialPort1.WriteLine(stopSim);
                Console.WriteLine("Simulation mode is deactivated");
            }
            else
            {
                string startSim = "CMD,3159,SIM,ENABLE";
                serialPort1.WriteLine(startSim);
                Console.WriteLine("Simulation mode is active");
            }

            // Modu değiştir
            simulationMode = !simulationMode;
        }

        private void calib_Click(object sender, EventArgs e)
        {
            string calcommand = "CMD,3159,CAL";

            serialPort1.WriteLine(calcommand);

            Console.WriteLine("Payload calibrated to zero altitude");
        }

        private void simp_Click(object sender, EventArgs e)
        {
            if (!simulationMode == false)
            {
                string pressure = "CMD,1000,SIMP,101325";

                serialPort1.WriteLine(pressure);
                Console.WriteLine("Pressure data is sent");

            }
            else
            {
                simp.Enabled = false;

            }
        }

        private void mec_Click(object sender, EventArgs e)
        {
            string meccommand = "CMD,3159,MEC,<DEVICE>,<ON_OFF>";

            serialPort1.WriteLine(meccommand);

            Console.WriteLine("Command is sent");
        }

        private void settime_Click(object sender, EventArgs e)
        {

        }
        */
        private void btn_send_Click(object sender, EventArgs e)
        {
            string oncommand = "CMD,3159,CX,ON";
            string offcommand = "CMD,3159,CX,OFF";
            string calcommand = "CMD,3159,CAL";
            string meccommandon= "CMD,3159,MEC,<DEVICE>,ON";
            string meccommandoff = "CMD,3159,MEC,<DEVICE>,OFF";
            string settime = "CMD,3159,ST,<UTC_TIME>|GPS";
            string simon = "CMD,3159,SIM,DISABLE";
            string simoff = "CMD,3159,SIM,ENABLE";
            string pressure = "CMD,1000,SIMP,101325";

            if (comboBoxCommand.Text=="CX ON")
            {
                serialPort1.WriteLine(oncommand);

                Console.WriteLine("Start command is sent");
            }
            else if (comboBoxCommand.Text== "CX OF")
            {
                serialPort1.WriteLine(offcommand);

                Console.WriteLine("Stop command is sent");
            }
            else if (comboBoxCommand.Text == "CAL")
            {
                serialPort1.WriteLine(calcommand);

                Console.WriteLine("Payload calibrated to zero altitude");
            }
            else if (comboBoxCommand.Text == "MEC ON")
            {
                serialPort1.WriteLine(meccommandon);

                Console.WriteLine("Mechanism on command is sent");
            }
            else if (comboBoxCommand.Text == "MEC OFF")
            {
                serialPort1.WriteLine(meccommandoff);

                Console.WriteLine("Mechanism off command is sent");
            }
            else if (comboBoxCommand.Text == "ST")
            {
                serialPort1.WriteLine(settime);

                Console.WriteLine("Set time command is sent");
            }
            else if (comboBoxCommand.Text == "SIM ON")
            {
                serialPort1.WriteLine(simon);

                Console.WriteLine("Simulation mode is active");

                comboBoxCommand.Items.Add("SIMP");
                if (comboBoxCommand.Text == "SIMP")
                {
                    serialPort1.WriteLine(pressure);
                    Console.WriteLine("Pressure data is sent");
                }
                else
                {
                    Console.WriteLine("Please pick a command to sent");
                }
            }
            else if (comboBoxCommand.Text == "SIM OFF")
            {
                serialPort1.WriteLine(simoff);

                Console.WriteLine("Simulation mode is deactivated");
            }
            else
            {
                Console.WriteLine("Please pick a command to sent");
            }

        }

        int sayac = 0;

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string telemetrypacket = serialPort1.ReadLine()?.Trim();
            string[] dataParts = telemetrypacket.Split(',');
            sayac++;
            // GYRO_R, GYRO_P, GYRO_Y sıraları → index 9, 10, 11
            float gyroRoll = float.Parse(dataParts[9]);
            float gyroPitch = float.Parse(dataParts[10]);
            float gyroYaw = float.Parse(dataParts[11]);

            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);
            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f);
            GL.Rotate(roll, 0.0f, 0.0f, 1.0f);

            string dosyaYolu = @"C:\Users\Özge\OneDrive\Desktop\Grizu Yer istasyonu\kayit.csv";

            // CSV'yi oluştur
            StringBuilder csv = new StringBuilder();
            if (!File.Exists(dosyaYolu))
            {
                File.WriteAllText(dosyaYolu,
                "TEAM_ID,MISSION_TIME,PACKET_COUNT,MODE,STATE,ALTITUDE,TEMPERATURE,PRESSURE,VOLTAGE,GYRO_R,GYRO_P,GYRO_Y,ACCEL_R,ACCEL_P,ACCEL_Y,MAG_R,MAG_P,MAG_Y,AUTO_GYRO_ROTATION_RATE,GPS_TIME,GPS_ALTITUDE,GPS_LATITUDE,GPS_LONGITUDE,GPS_SATS,CMD_ECHO\n",
                Encoding.UTF8);
            }

            // Dosyaya yaz
            File.AppendAllText(dosyaYolu, telemetrypacket + "\n", Encoding.UTF8);

            timer1.Start();

            glControl1.Invalidate();

            try
            {
                if (dataParts.Length >= 24)
                {
                    string latitude = dataParts[22].Trim();
                    string longitude = dataParts[23].Trim();
                    double latitudeValue = double.Parse(latitude, CultureInfo.InvariantCulture);
                    double longitudeValue = double.Parse(longitude, CultureInfo.InvariantCulture);
                    PointLatLng konum = new PointLatLng(latitudeValue, longitudeValue);
                    // Haritayı konuma merkezle
                    gMapControl1.Position = konum;

                    this.Invoke(new MethodInvoker(delegate
                    {
                        
                        chart3.Series["TEMPURATURE"].Points.AddXY(sayac, dataParts[6]);
                        chart8.Series["PRESSURE"].Points.AddXY(sayac, dataParts[7]);
                        chart6.Series["ALTİTUDE"].Points.AddXY(sayac, dataParts[5]);
                        chart5.Series["VOLTAGE"].Points.AddXY(sayac, dataParts[8]);

                        
                        dataGridView1.Rows.Insert(0, dataParts.Take(13).Select(p => p.Trim()).ToArray());
                        dataGridView2.Rows.Insert(0, dataParts.Skip(13).Take(12).Select(p => p.Trim()).ToArray());

                        // DataGridView satır sınırını kontrol etme
                        if (dataGridView1.Rows.Count > 100)
                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                        if (dataGridView2.Rows.Count > 100)
                            dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);

                        gMapControl1.Position = new PointLatLng(latitudeValue, longitudeValue);
                        gMapControl1.Overlays.Clear();
                        GMapOverlay markersOverlay = new GMapOverlay("markers");
                        GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(latitudeValue, longitudeValue), GMarkerGoogleType.red_dot);
                        marker.ToolTipText = $"Latitude: {latitudeValue}, Longitude: {longitudeValue}";
                        marker.ToolTipMode = MarkerTooltipMode.Always;
                        markersOverlay.Markers.Add(marker);
                        gMapControl1.Overlays.Add(markersOverlay);
                    }));
                }
                else
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        MessageBox.Show("Geçersiz veri formatı: " + telemetrypacket);
                    }));
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show("Veri okuma hatası: " + ex.Message);
                }));
            }

        }
        private void glControl1_Load_1(object sender, EventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.DepthTest);
        }
        private void silindir(float step, float topla, float radius, float dikey1, float dikey2)
        {
            float eski_step = 0.1f;
            GL.Begin(BeginMode.Quads);//Y EKSEN CIZIM DAİRENİN
            while (step <= 360)
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(255, 0, 0));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(255, 0, 0));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(255, 0, 0));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(255, 0, 0));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);
                float ciz2_x = (float)(radius * Math.Cos((step + 2) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 2) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
            step = eski_step;
            topla = step;
            while (step <= 180)// UST KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);
                float ciz2_x = (float)(radius * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);
                step += topla;
            }
            step = eski_step;
            topla = step;
            while (step <= 180)//ALT KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                float ciz2_x = (float)(radius * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();
        }


        private void glControl1_Paint_1(object sender, PaintEventArgs e)
        {
            float step = 1.0f;
            float topla = step;
            float radius = 5.0f;
            float dikey1 = radius, dikey2 = -radius;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, 4 / 3, 1, 10000);
            Matrix4 lookat = Matrix4.LookAt(25, 0, 0, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            //GL.Rotate(roll, 1.0, 0.0, 0.0);//ÖNEMLİ
            //GL.Rotate(z, 0.0, 1.0, 0.0);
            //GL.Rotate(y, 0.0, 0.0, 1.0);
            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);
            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f);
            GL.Rotate(roll, 0.0f, 0.0f, 1.0f);

            silindir(step, topla, radius, 3, -5);
            silindir(0.01f, topla, 0.5f, 9, 9.7f);
            silindir(0.01f, topla, 0.1f, 5, dikey1 + 5);
            koni(0.01f, 0.01f, radius, 3.0f, 3, 5);
            koni(0.01f, 0.01f, radius, 2.0f, -5.0f, -10.0f);
            Pervane(9.0f, 11.0f, 0.2f, 0.5f);

            GL.Begin(BeginMode.Lines);
            GL.Color3(Color.FromArgb(250, 0, 0));
            GL.Vertex3(-30.0, 0.0, 0.0);
            GL.Vertex3(30.0, 0.0, 0.0);
            GL.Color3(Color.FromArgb(0, 0, 0));
            GL.Vertex3(0.0, 30.0, 0.0);
            GL.Vertex3(0.0, -30.0, 0.0);
            GL.Color3(Color.FromArgb(0, 0, 250));
            GL.Vertex3(0.0, 0.0, 30.0);
            GL.Vertex3(0.0, 0.0, -30.0);
            GL.End();
            //GraphicsContext.CurrentContext.VSync = true;
            glControl1.SwapBuffers();
        }


        private void koni(float step, float topla, float radius1, float radius2, float dikey1, float dikey2)
        {
            float eski_step = 0.1f;
            GL.Begin(BeginMode.Lines);//Y EKSEN CIZIM DAİRENİN
            while (step <= 360)
            {
                if (step < 45)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 90)
                    GL.Color3(1.0, 0.0, 0.0);
                else if (step < 135)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 180)
                    GL.Color3(1.0, 0.0, 0.0);
                else if (step < 225)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 270)
                    GL.Color3(1.0, 0.0, 0.0);
                else if (step < 315)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 360)
                    GL.Color3(1.0, 0.0, 0.0);
                float ciz1_x = (float)(radius1 * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius1 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);
                float ciz2_x = (float)(radius2 * Math.Cos(step * Math.PI / 180F));
                float ciz2_y = (float)(radius2 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
            step = eski_step;
            topla = step;
            while (step <= 180)// UST KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(255, 1, 1));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                float ciz1_x = (float)(radius2 * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius2 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                float ciz2_x = (float)(radius2 * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius2 * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            step = eski_step;
            topla = step;
            GL.End();
        }
        private void Pervane(float yukseklik, float uzunluk, float kalinlik, float egiklik)
        {
            float radius = 10, angle = 45.0f;
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.Red);
            GL.Vertex3(uzunluk, yukseklik, kalinlik);
            GL.Vertex3(uzunluk, yukseklik + egiklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik + egiklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik, kalinlik);
            GL.Color3(Color.Red);
            GL.Vertex3(-uzunluk, yukseklik + egiklik, kalinlik);
            GL.Vertex3(-uzunluk, yukseklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik + egiklik, kalinlik);
            GL.Color3(Color.White);
            GL.Vertex3(kalinlik, yukseklik, -uzunluk);
            GL.Vertex3(-kalinlik, yukseklik + egiklik, -uzunluk);
            GL.Vertex3(-kalinlik, yukseklik + egiklik, 0.0);//+
            GL.Vertex3(kalinlik, yukseklik, 0.0);//-
            GL.Color3(Color.White);
            GL.Vertex3(kalinlik, yukseklik + egiklik, +uzunluk);
            GL.Vertex3(-kalinlik, yukseklik, +uzunluk);
            GL.Vertex3(-kalinlik, yukseklik, 0.0);
            GL.Vertex3(kalinlik, yukseklik + egiklik, 0.0);
            GL.End();
        }

    }
}



