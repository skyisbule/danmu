using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Barrage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window 
    {
        Thread BarrageThread = null;
        private UdpClient udpClient;

        public MainWindow() 
        {
            InitializeComponent();





        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            //在本机指定的端口接收
            SetBarrage("计算机协会", 1);


            //接收从远程主机发送过来的信息；

            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        udpClient = new UdpClient(9999);
                        IPEndPoint remote = null;
                        //关闭udpClient时此句会产生异常
                        byte[] bytes = udpClient.Receive(ref remote);
                        string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        SetBarrage(str, 1);
                    }
                    catch
                    {
                        //退出循环，结束线程
                        break;
                    }
                    finally
                    {
                        udpClient.Close();
                    }

                }
            }).Start();

            
            

        }

        public void SetBarrage(string content, int img)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Dictionary<object, double> ldi = new Dictionary<object, double>();

                int lines = (Convert.ToInt32(this.Height) - 10) / 40;
                int line = 0;

                Random rd = new Random();

                for (int i = 0; i < lines; i++) { ldi.Add(i, 0); }

                foreach (StackPanel item in canvas.Children)
                {
                    if (ldi.ContainsKey(item.Tag))
                    {
                        if (ldi[item.Tag] < Convert.ToDouble(item.GetValue(Canvas.LeftProperty)))
                        {
                            ldi[item.Tag] = Convert.ToDouble(item.GetValue(Canvas.LeftProperty));
                        }
                    }
                    else { ldi.Add(item.Tag, Convert.ToDouble(item.GetValue(Canvas.LeftProperty))); }
                }

                if (ldi.Count(m => m.Value == 0) > 1)
                {
                    var list = ldi.Where(m => m.Value == 0).ToList();

                    line = Convert.ToInt32(list[rd.Next(0, list.Count)].Key);
                }
                else
                {
                    line = Convert.ToInt32(ldi.OrderBy(m => m.Value).FirstOrDefault().Key);
                }

                #region StackPanel

                StackPanel sp = new StackPanel();

                sp.Tag = line;
                sp.Orientation = Orientation.Horizontal;
                sp.SetValue(Canvas.LeftProperty, this.Width);
                sp.SetValue(Canvas.TopProperty, Convert.ToDouble(40 * line) + 10);

                TextBlock tb = new TextBlock();

                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb.FontSize = 50;
                tb.Text = content;

                tb.Foreground = new SolidColorBrush(Color.FromRgb(
                    Convert.ToByte(rd.Next(0, 255)),
                    Convert.ToByte(rd.Next(0, 255)),
                    Convert.ToByte(rd.Next(0, 255))));

                sp.Children.Add(tb);

                /*
                GifImage gi = new GifImage();

                gi.Width = 24;
                gi.Height = 24;
                gi.Source = "images/e" + (img + 100) + ".gif";

                sp.Children.Add(gi);
                */


                canvas.Children.Add(sp);
                

                DoubleAnimation animation =
                    new DoubleAnimation(this.Width, -this.Width, new Duration(TimeSpan.FromSeconds(33)));

                animation.Completed += new EventHandler((object o, EventArgs arg) =>
                {
                    canvas.Children.Remove(sp);
                });

                sp.BeginAnimation(Canvas.LeftProperty, animation);

                #endregion
            }));
        }

        protected override void OnClosed(EventArgs e)
        {
            BarrageThread.Abort();

            base.OnClosed(e);

            Application.Current.Shutdown();
        }


        static void Recive(object o)
        {
            var send = o as Socket;
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = send.Receive(buffer);
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.UTF8.GetString(buffer, 0, effective);
               // SetBarrage(str, 1);
            }
        }


    }


}