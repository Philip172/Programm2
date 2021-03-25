using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Programm2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //данные для приложения
        IPEndPoint ipPoint;// контейнер для IP-адреса, он будет получен позже
        int money_now = 0;// контейнер для кол-ва денег, использующийся при передаче этого параметра на сервер


        static int port = 8005; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static string unical_code = "_money2"; // уникальный код для этого Игрока

        int money = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            money++;
            textBox1.Text = money.ToString();
        }

        async private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            money_now = 0;

            money_now = money;

            try
            {
                // Создаём сокет
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                // Подключаемся к удаленному хосту
                socket.Connect(ipPoint);

                // Отправляем сообщение удалённому хосту
                string message = money_now.ToString();
                byte[] data = Encoding.Unicode.GetBytes(message + unical_code);
                socket.Send(data);

                // Получаем ответ от удалённого хоста
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                textBox2.Text = builder.ToString();

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                textBox2.Text = "ОШИБКА:\n" + ex.Message;
            }
        }
    }
}
