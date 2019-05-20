using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms_TestCreateFile_VCE.Classes;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;

namespace WindowsForms_TestCreateFile_VCE.Forms
{
    public partial class Work : Form
    {
        List<Socket> sockets = new List<Socket>();
        //Create Dictionary take value of radiobutton and text answer
        Dictionary<RadioButton, TextBox> answers = new Dictionary<RadioButton, TextBox>();
        //Create list of Question and it constains dictionary
        List<Question> questions = new List<Question>();//список запитань та відповідей
        //Create a link class SomeTest
        public SomeTest testss;
        //Create link Stream
        //Stream stream;
        //Create float variable for take value weight test
        public float  balTest = 0; 
        //Create float variable to determine sum of points test
        public float sumBall = 0;
       // The TCP client will connect to the server using an IP and a port
        TcpClient tcpClient;
        // The file stream will read bytes from the local file you are sending
        FileStream fstFile;
        // The network stream will send bytes to the server application
        NetworkStream strRemote;

        public Work()
        {
            InitializeComponent();
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //to view question in listbox
            var quest = questions[listBox1.SelectedIndex];
            label3.Text = quest.Title;
            textBox1.Text = quest.Text;
        }
        //Button Show,shows fields radiobutton and texBox
        private void button1_Click(object sender, EventArgs e)
        {
            //Clear fields groupBox
            groupBox1.Controls.Clear();
            //Clean up fields Dictionary
            answers.Clear();
            //Inicialize coordinates for fields in groupBox
            int x = 0, y = 20;
            //Take value numericUpDown and accordiding to the value we create radiobutton and text fields
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                //create radiobutton
                RadioButton r = new RadioButton();
                //Create location
                r.Location = new Point(x = 5, y);
                //Create parent to be displayed in to dropBox
                r.Parent = groupBox1;
                //Set value radiobutton
                r.Text = "";
                //Create textBox
                TextBox t = new TextBox();
               // Set location textBox
                t.Location = new Point(x = 110, y);
                //Set size textbox
                t.Size = new Size(300, 30);
                //Get parent  to be displayed in to dropBox
                t.Parent = groupBox1;
                //We increase y to add one to another
                y += 30;
                //Write to list answer collection radiobutton and text fields
                answers.Add(r, t);//колекція полів збережеться в Dictionary
            }
        }
        //Button Next
        private void button2_Click(object sender, EventArgs e)
        {
            //Write to  Question title,text fields and list answers
            var questy = new Question
            {
                Title = label3.Text,//саме запитання
                Text = textBox1.Text,//текст запитання
                answers = new List<Answer>()//List відповідей
            };

            //Write to list answers weight,bool correct(true or false) and text from textBox
            foreach (var item in answers)
            {
                questy.answers.Add(new Answer  //додати відповіді
                {
                    Weight = (float) Convert.ToDecimal(textBox6.Text),
                    Correct = item.Key.Checked,//додати радіоточки і перевірити на true or fauls
                    Text = item.Value.Text//додати текст радіоточки
                });

            }
            //Write to list<Question> Question quest(Title,Text and list answers)
            questions.Add((questy));
            //write to object SomeTest(testname,subject,author and List<Question>
             testss = new SomeTest
            {
                TestName = textBox2.Text,
                Subject = textBox3.Text,
                Author = textBox4.Text,
                quest = questions
            };
            //Add to listbox fields "Запитання"
            listBox1.Items.Add(label3.Text);
            //Add count to "Запитання" (запитання 1,запитання 2)
            label3.Text = "Question" + listBox1.Items.Count;
            //Clear field textbox
             textBox1.Text = "";
             //Get value by default
             numericUpDown1.Value = 2;
             //Clear fields groupBox
             groupBox1.Controls.Clear();
             
             //Take value Weight from textbox6
            balTest = (float)Convert.ToDecimal(textBox6.Text);
            //add value
            sumBall += balTest;
            //check value sumBall,if value > 100 stop create test
            if (sumBall >= 100)
            {
                //call method:StopTest for stop add question to test 
                StopTest();
            }
            else
            {
                //we output result sum Weight
                label11.Text = sumBall.ToString();
            }
            //Clear textBox
            textBox6.Text = "";
        }
       //button For Save
       /// <summary>
       /// In this method we make visible in form some component with working to saving file
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //close this button
            button3.Visible = false;
            //explanation about name of file
            label7.Visible=true;
            //field for enter name of file
            textBox5.Visible = true;
            //button save
            button4.Visible = true;
            
        }
        //Save file
        /// <summary>
        /// In this method we searilization file and saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(@"D:\CreateTestForClient\");
            //take from textBox name
            string nameFromTextBox5Text = textBox5.Text;
            //Create file with name + xml
            FileStream streame = File.Create(@"D:\CreateTestForClient\"+nameFromTextBox5Text + ".xml");
            //take full file name 
                string fileName = streame.Name;
            //Serializer to type class SomeTest
            XmlSerializer ser = new XmlSerializer(typeof(SomeTest));
                //Serializ file
                ser.Serialize(streame, testss);
                //Close Serialize file
                streame.Close();
          //info about file
           MessageBox.Show("File Save in => :  " + fileName);
           //Use context to add value to database
            using (MyContext context = new MyContext())
            {
                //check is a subject in data base
                Subject subject = context.Subjects.Where(x => x.Name == textBox3.Text ).SingleOrDefault();
                //if subject empty
                if (subject == null)
                {
                    Subject s1 = new Subject() { Name = textBox3.Text }; ////    INSERT Subject name
                    context.Subjects.Add(s1);
                    context.SaveChanges();
                }
                //take this  subject, subject id from database
                int rez = context.Subjects.Where(x=>x.Name==textBox3.Text).Select(x=>x.Id).Single(); 
                //add to database value(TextName) , filename  and subject id
                Test t1 = new Test() { Name = textBox2.Text,PathToFile = fileName, SubjectId = rez }; ////    INSERT Test
                context.Tests.Add(t1);
                context.SaveChanges();
            }

            label7.Visible = false;
            label7.Text="";
            label3.Visible = false;
            textBox5.Visible = false;
            button4.Visible = false;
            button3.Visible = false;
            button6.Visible = true;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }
        //Button Exit
        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Work_Load(object sender, EventArgs e)
        {

        }
        // method for close elements on form
        void StopTest()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Text="Test have 100 points , please save test!!!";
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";
            //textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            groupBox1.Controls.Clear();//очистка поля варіантів
            groupBox1.Visible = false;
            numericUpDown1.Visible = false;
            listBox1.ResetText();
            listBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
        }
        //button Create new test
        private void button6_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Text = "Question 0";
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = false;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";
            //textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = false;
            textBox6.Visible = true;
            groupBox1.Controls.Clear();//очистка поля варіантів
            groupBox1.Visible = true;
            numericUpDown1.Visible = true;
            listBox1.Items.Clear();
            listBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;
            button6.Visible = false;
        }
        //Send Message
        private void button11_Click(object sender, EventArgs e)
        {
            Socket listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry ipHost = Dns.Resolve(SystemInformation.ComputerName);  //Dns.Resolve(SystemInformation.ComputerName);
            IPAddress iPAddress = ipHost.AddressList[0];
            IPEndPoint iPEnd = new IPEndPoint(iPAddress, 5555);
            listenSock.Bind(iPEnd);
            Thread listnerThread = new Thread(ListenThreadFunction);
            listnerThread.IsBackground = true;
            listnerThread.Start(listenSock);
            label12.Visible = true;
            label13.Visible = true;
            listBox2.Visible = true;
            textBox7.Visible = true;
            button8.Visible = true;
            button10.Visible = true;
            button7.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
        }
        private void ListenThreadFunction(Object obj)
        {
            Socket s = obj as Socket;
            if (s == null) throw new ArgumentException("Error");
            s.Listen(5);
            while (true)
            {
                Socket tmp = s.Accept();
                sockets.Add(tmp);
                IPEndPoint remoteIpEndPoint = tmp.RemoteEndPoint as IPEndPoint;

                listBox2.Invoke(new Action(() => listBox2.Items.Add(remoteIpEndPoint.Address.ToString())));
                Thread thread = new Thread(ReceiveThreadFunction);
                thread.Start(tmp);
            }
        }

        private void Send(Socket s, string str)
        {
            byte[] sendByte = Encoding.ASCII.GetBytes(textBox7.Text);
            s.Send(sendByte);
        }

        private void ReceiveThreadFunction(object obj)
        {
            Socket s = obj as Socket;
            while (true)
            {
                try
                {
                    if (s == null) throw new ArgumentException("Error");
                    byte[] receiveByte = new byte[1024];
                    int count = s.Receive(receiveByte);
                    string data = Encoding.ASCII.GetString(receiveByte, 0, count);
                    //MessageBox.Show(data);
                }
                catch (Exception e)
                {
                    MessageBox.Show( "Connection Closed");
                    s.Close();
                }
               
            }
            
        }
        //Button Send
        private void button8_Click(object sender, EventArgs e)
        {
            string ip = listBox2.SelectedItem.ToString();//знаэм ip клієнта який підконектнився
            foreach (var i in sockets)
            {
                IPEndPoint lockaliPEndPoint = i.RemoteEndPoint as IPEndPoint;
                string ipclient = lockaliPEndPoint.Address.ToString();
                if (ipclient == ip)
                {
                    byte[] sendByte = Encoding.ASCII.GetBytes(textBox7.Text);
                    i.Send(sendByte);
                }
            }

           
        }
        //Button Exit from send message
        private void button10_Click(object sender, EventArgs e)
        {
            button10.Visible = false;
            listBox2.Items.Clear();
            textBox7.Text = "";
            button7.Visible = true;
            label12.Visible = false;
            label13.Visible = false;
            listBox2.Visible = false;
            textBox7.Visible = false;
            button8.Visible = false;
        }
        private void ConnectToServer(string ServerIP, int ServerPort)
        {
            // Create a new instance of a TCP client
            tcpClient = new TcpClient();
            try
            {
                // Connect the TCP client to the specified IP and port
                tcpClient.Connect(ServerIP, ServerPort);
                txtLog.Text += "Successfully connected to server\r\n";
            }
            catch (Exception exMessage)
            {
                // Display any possible error
                txtLog.Text += exMessage.Message;
            }
        }
        //Button Send File
        private void button12_Click(object sender, EventArgs e)
        {
            ConnectToServer(txtServer.Text, Convert.ToInt32(txtPort.Text));
            button7.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            label14.Visible = true;
            label15.Visible = true;
            button9.Visible = true;
            button14.Visible = true;
            lblPort.Visible = true;
            lblServer.Visible = true;
            txtPort.Visible = true;
            txtServer.Visible = true;
            txtLog.Visible = true;
        }
        //Button Disconect
        private void button14_Click(object sender, EventArgs e)
        {
            // Close connections and streams and update the log textbox
            tcpClient.Close();
            strRemote.Close();
            fstFile.Close();
            txtLog.Text += "Disconnected from server.\r\n";
            button7.Visible = true;
            label14.Visible = false;
            label15.Visible = false;
            button9.Visible = false;
            button14.Visible = false;
            lblPort.Visible = false;
            lblServer.Visible = false;
            txtPort.Visible = false;
            txtServer.Visible = false;
            txtLog.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // If tclClient is not connected, try a connection
            if (tcpClient.Connected == false)
            {
                // Call the ConnectToServer method and pass the parameters entered by the user
                ConnectToServer(txtServer.Text, Convert.ToInt32(txtPort.Text));
            }

            // Prompt the user for opening a file
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtLog.Text += "Sending file information\r\n";
                // Get a stream connected to the server
                strRemote = tcpClient.GetStream();
                byte[] byteSend = new byte[tcpClient.ReceiveBufferSize];
                // The file stream will read bytes from the file that the user has chosen
                fstFile = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                // Read the file as binary
                BinaryReader binFile = new BinaryReader(fstFile);

                // Get information about the opened file
                FileInfo fInfo = new FileInfo(openFile.FileName);

                // Get and store the file name
                string FileName = fInfo.Name;
                // Store the file name as a sequence of bytes
                byte[] ByteFileName = new byte[2048];
                ByteFileName = System.Text.Encoding.ASCII.GetBytes(FileName.ToCharArray());
                // Write the sequence of bytes (the file name) to the network stream
                strRemote.Write(ByteFileName, 0, ByteFileName.Length);

                // Get and store the file size
                long FileSize = fInfo.Length;
                // Store the file size as a sequence of bytes
                byte[] ByteFileSize = new byte[2048];
                ByteFileSize = System.Text.Encoding.ASCII.GetBytes(FileSize.ToString().ToCharArray());
                // Write the sequence of bytes (the file size) to the network stream
                strRemote.Write(ByteFileSize, 0, ByteFileSize.Length);

                txtLog.Text += "Sending the file " + FileName + " (" + FileSize + " bytes)\r\n";

                // Reset the number of read bytes
                int bytesSize = 0;
                // Define the buffer size
                byte[] downBuffer = new byte[2048];

                // Loop through the file stream of the local file
                while ((bytesSize = fstFile.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    // Write the data that composes the file to the network stream
                    strRemote.Write(downBuffer, 0, bytesSize);
                }

                // Update the log textbox and close the connections and streams
                txtLog.Text += "File sent. Closing streams and connections.\r\n";
                tcpClient.Close();
                strRemote.Close();
                fstFile.Close();
                txtLog.Text += "Streams and connections are now closed.\r\n";
            }
        }
        //Online options
        private void button7_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            button12.Visible = true;
            button13.Visible = true;
        }

        
    }
}
