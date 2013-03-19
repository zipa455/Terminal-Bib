using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using MySql.Data;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;



namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    unsafe public partial class MainWindow : Window
    {
            [DllImport("spnxreader.dll")]
            public static extern bool SpnxReaderOpen(int* pxHandle);
         [DllImport("spnxreader.dll")]
            public static extern int SpnxReaderReceiveW26T(int xHandle, void* pBuffer, int nBufferSize, int nTimeout);
            List<Book> rows = new List<Book>();
            List<Book> rows2 = new List<Book>();

            String sConnectionString = "Database=***;Data Source=*****;User Id=****;Password=****;charset=utf8";
            System.Data.DataTable dt2;
            System.Data.DataTable dt1;
            

        public MainWindow()
        {
            InitializeComponent();
            

        }

        static char[] c = new char[3];

        unsafe private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool a;
            int a1;
            int b;
            int c;
            
            a = SpnxReaderOpen(&b);
            if (a) button1.Content = "OK";
            else
            {
                button1.Content = "FAIL";
                return;
            }
            a1 = SpnxReaderReceiveW26T(b,  &c , 3, 1000);
            if (a1 > 0)
            {
                string ot = (c & 0x000000ff).ToString() + ((c & 0x0000ff00)+(c & 0x00ff0000)/0x10000).ToString() ;
                button1.Content = ot;            
            }
            else if (a1 == 0)
            {
                button1.Content = "Время истекло";
            }

        }
         delegate void StrDel3(List<Book> z);
         private void Qr(object txt1)
        {
            StrDel3 stradd3 = (z) => {listView1.ItemsSource=z; };
            
            string sSQL = "SELECT author, title, biblionumber, copyrightdate FROM biblio WHERE title LIKE '%" + txt1 + "%' OR author LIKE '" + txt1 + "%'";
            
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();

            List<Book> rows = new List<Book>();
            ///выполняем запрос, который возвращает результат
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(sSQL, sConnectionString);
            ///если ошибок нет
            if (result.HasError == false)
            {
                ///очищаем таблицу для вывода результата
                ///
                rows.Clear();
                ///заполняем таблицу на основе данных запроса
                dt1 = result.ResultData;                
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    rows.Add(new Book(result.ResultData.Rows[i][0].ToString(), result.ResultData.Rows[i][1].ToString(), result.ResultData.Rows[i][2].ToString(), result.ResultData.Rows[i][3].ToString()));
                   
                }
                col(rows);
                
                listView1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd3,rows);
                
                listView1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
                {
                    listView1.Items.Refresh();
                }));

                listView1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
                {
                    grid2.Visibility= Visibility.Hidden;
                }));
                //TODO: Присобачить год издания книги. 090001500099
                //                                     09000 марк код
                //                                     15 хуйта непонятная
                //                                     00099 смещение относительно RU                
            }            
            else
            {                
                MessageBox.Show(result.ErrorText);
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {            
            grid2.Visibility = Visibility.Visible;
            textBlock1.Text = "Пожалуйста, подождите";
            Thread tsk = new Thread(Qr);
            tsk.Start(textBox1.Text);            
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            rows2.Add((Book)listView1.SelectedItem);            
            col(rows2);
            listView2.ItemsSource = rows2;
            listView2.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView1.ItemsSource = rows;
            listView2.ItemsSource = rows2;            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
           List<Book> rows = new List<Book>();
           
           col(rows);
           listView1.ItemsSource = rows;
           listView1.Items.Refresh();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            rows2.Remove((Book)listView2.SelectedItem);
            col(rows2);
            listView2.Items.Refresh();
        }
        public delegate void StrDel2(Visibility str);
        public delegate void StrDel(string str);
        void SetQuery()
        {
            //Создаем новый объект потока (Thread)           
            //запускаем поток
            MySqlLib.MySqlData.MySqlExecute.MyResult result1 = new MySqlLib.MySqlData.MySqlExecute.MyResult();            
            //grid2.Visibility = Visibility.Visible;
            StrDel2 stradd2 = (x) => { grid2.Visibility = x; };
            StrDel stradd = (x) => { textBlock1.Text = x; };
            grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2,Visibility.Visible);            
            bool a;
            int a1;
            int b;
            int c;
            //char* c=stackalloc char[3];
            //void* cc = c.;
            //char[] c = new char[3];
            //cc c;
            //goto m;
            a = SpnxReaderOpen(&b);
            if (a == false)             
            {
                //textBlock1.Text = "Ошибка ридера. Обратитесь в абонемент.";
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd,
                   "Ошибка ридера. Обратитесь в абонемент.");                
                Thread.Sleep(5000);
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2, Visibility.Hidden);
                return;
            }
            a1 = SpnxReaderReceiveW26T(b, &c, 3, 3000);
            string ot = "";
            if (a1 > 0)
            {
				/// Хитрый формат данных на читю билетах
                /*ot ="00" +  (c & 0x000000ff).ToString() + 
                    ((c & 0x0000f000)/0x1000).ToString() + 
                    ((c & 0x00000f00)/0x100).ToString() +
                        ((c & 0x00f00000)/0x100000).ToString() +
                            ((c & 0x000f0000)/0x1000).ToString();*/
                    string ot1 = "00000";
                    ot1 = ((c & 0x0000ff00) + (c & 0x00ff0000) / 0x10000).ToString();
                    if(ot1.Length == 4) ot1 = "0" + ot1;
                    if (ot1.Length == 3) ot1 = "00" + ot1;
                    if (ot1.Length == 2) ot1 = "000" + ot1;
                    if (ot1.Length == 1) ot1 = "0000" + ot1;
                    if (ot1.Length == 0) ot1 = "00000" + ot1;
                    ot = "00" + (c & 0x000000ff).ToString() + ot1;
                //button1.Content = ot;
            }
            else if (a1 == 0)
            {
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd,
                   "Время истекло, а Вы не успели приложить читательский билет к ридеру. Попробуйте повторить." );                
                Thread.Sleep(5000);
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2, Visibility.Hidden);
                return;
            }

            //return;
            //this.re
            //m:
            //ot = "0023829182";
            //MessageBox.Show(ot);
            string sSQL = "SELECT borrowernumber, surname, firstname, debarred  FROM borrowers WHERE cardnumber ='" + ot + "' LIMIT 1";
            MySqlLib.MySqlData.MySqlExecuteData.MyResultData result = new MySqlLib.MySqlData.MySqlExecuteData.MyResultData();
            result = MySqlLib.MySqlData.MySqlExecuteData.SqlReturnDataset(sSQL, sConnectionString);

            if (result.HasError == true)
            {
                MessageBox.Show(result.ErrorText);
                return;
            }

            if (result.ResultData.DefaultView.Count == 0)
            {
                //textBlock1.Text = "Ваша учётная запись не найдена.\n Обратитесь в абонемент.";                
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd,
                   "Ваша учётная запись не найдена.\n Обратитесь в абонемент.");
                Thread.Sleep(5000);
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2, Visibility.Hidden);
                return;
            }
            List<User> rows3 = new List<User>();
            System.Data.DataTable gr;
            gr = result.ResultData;
            if (gr.Rows[0][3].ToString() != "00/00/0000" && gr.Rows[0][3].ToString() != "0" && gr.Rows[0][3].ToString() != "" && gr.Rows[0][3].ToString() != "NULL")
            {
                //textBlock1.Text = "Ваша учётная запись заблокированна. \n Обратитесь в абонемент.";                
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd,
                   "Время истекло, а Вы не успели приложить читательский билет к ридеру. Попробуйте повторить.");
                Thread.Sleep(5000);
                grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2, Visibility.Hidden);
                return;
            }
            //MessageBox.Show("f");
            for (int i = 0; i < rows2.Count; i++)
            {
                sSQL = "INSERT INTO `koha`.`reserves` (`borrowernumber`, `reservedate`, `biblionumber`, `constrainttype`, `branchcode`, `notificationdate`, `reminderdate`, `cancellationdate`, `reservenotes`, `priority`, `found`, `timestamp`, `itemnumber`, `waitingdate`, `expirationdate`, `lowestPriority`, `suspend`, `suspend_until`) VALUES ('"
                       + gr.Rows[0][0] + "' , CURDATE(),  '" + rows2[i].biblionumber + "', 'a', '1', NULL, NULL, NULL, '', NULL, NULL, CURRENT_TIMESTAMP, NULL, NULL, NULL, '', '0', NULL)";

                result1 = MySqlLib.MySqlData.MySqlExecute.SqlNoneQuery(sSQL, sConnectionString);
                if (result1.HasError == true) { MessageBox.Show(result1.ErrorText); return; }
            }
            //textBlock1.Text = gr.Rows[0][1].ToString() + " " + gr.Rows[0][2] + ", Ваш заказ принят";
            grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd,
                   gr.Rows[0][1].ToString() + " " + gr.Rows[0][2].ToString() + ", Ваш заказ принят");            
            Thread.Sleep(5000);
            grid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, stradd2, Visibility.Hidden);
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "Пожалуйста, приложите читательский билет к ридеру для подверждения заказа";
            Task tsk = new Task(SetQuery);
            tsk.Start();              
        }

        static public void func(Grid grid2)
        {
            grid2.Visibility = Visibility.Visible;
            Thread.Sleep(5000);
            grid2.Visibility = Visibility.Hidden;
        }

         void col(List<Book> rows)
        {
            //for (int i = 0; i < rows.Count; i++)
            //    if (i % 2 == 0) rows[i].color = "Green";
            //    else rows[i].color = "";
        }

         private void listView1_SourceUpdated(object sender, DataTransferEventArgs e)
         {

         }

        private void Test_Bt_Click(object sender, RoutedEventArgs e)
        {
            //Frame1.
            Frame1.Source=(new Uri("query.xaml", UriKind.Relative));
        }
    }
    class Book
    {
        public string title { get; set; }
        public string author { get; set; }
        public string biblionumber { get; set; }
        public string copyrightdate { get; set; }
        public Book(string t, string a, string y,string c)
        {
            title = t;
            author = a;
            biblionumber = y;
            copyrightdate = c;
        }
    }
    class User
    {
        public string borrowernumber { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string debarred { get; set; }
        public User(string b, string s, string f, string d)
        {
            borrowernumber=b;
            surname = s;
            firstname = f;
            debarred = d;
        }
    }
}
