using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opdl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "language_pgDataSet.Client". При необходимости она может быть перемещена или удалена.
            // this.clientTableAdapter.Fill(this.language_pgDataSet.Client);
            LoadF(0, 0);
        }
        public static string[] ms = {"", "", ""};
        public string phone;
        public string email;
        public static string connectionString = @"Data Source =10.111.105.2,1433\SQLEXPRESS; Initial Catalog = Language_pg; User ID = 20.102k-10; Password = TrCk7orewq";
        public void LoadF(int load, int search)
        {
           
            if (load == 0)
            {
                cb_gender.Items.AddRange(new string[] { "All", "муж", "жен" });
                cb_kolv.Items.AddRange(new string[] { "10", "20", "50", "100" });
                cb_kolv.Text = "10";
                cb_gender.Text = "All";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlsort = string.Empty;
                connection.Open();
                if (cb_gender.Text == "All")
                    sqlsort = $"SELECT TOP {cb_kolv.Text} ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name FROM Client inner join Gender on Client.GenderCode = Gender.Code";
                else if (cb_gender.Text == "муж")
                    sqlsort = $"SELECT TOP {cb_kolv.Text} ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name FROM Client inner join Gender on Client.GenderCode = Gender.Code  Where Gender.Code = 1";
                else
                    sqlsort = $"SELECT TOP {cb_kolv.Text}  ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name FROM Client inner join Gender on Client.GenderCode = Gender.Code  Where Gender.Code = 2";
                if (search == 1)
                    sqlsort = $"Select ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name from	Client inner join Gender on Client.GenderCode = Gender.Code where LastName like '%{ms[0]}%'and FirstName like '%{ms[1]}%' and Patronymic like '%{ms[2]}%'";
                if(search == 2)
                    sqlsort = $"select ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name from Client inner join Gender on Client.GenderCode = Gender.Code where  Phone like '{phone} '";
                if(search == 3)
                    sqlsort = $"select ID, 'Имя' = FirstName,'Фамилия' =  LastName, 'Отчество' = Patronymic, 'Дата рождения'=Birthday, 'Дата регистрации'=RegistrationDate, Email, 'Телефон' = Phone, 'Пол'=Gender.Name from Client inner join Gender on Client.GenderCode = Gender.Code where Email like '%{email}%'";
                SqlCommand sqlCommand1 = new SqlCommand(sqlsort, connection);
                SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
                dataGridView1.DataSource = dataTable;
                connection.Close();
            }
            if (load == 0)
            {
                dataGridView1.Columns.Add("", "Последние посещение");
            }
        }
        private void btn_plus_Click(object sender, EventArgs e)
        {
            if (cb_kolv.SelectedIndex < 3)
                cb_kolv.Text = cb_kolv.Items[cb_kolv.SelectedIndex + 1].ToString();
            LoadF(1, 0);
        }
        private void btn_minus_Click(object sender, EventArgs e)
        {
            if (cb_kolv.SelectedIndex > 0)
                cb_kolv.Text = cb_kolv.Items[cb_kolv.SelectedIndex - 1].ToString();
            LoadF(1, 0);
        }

        private void cb_kolv_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_kolv.Text = cb_kolv.Items[cb_kolv.SelectedIndex].ToString();
            LoadF(1, 0);
        }

        private void cb_gender_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_gender.Text = cb_gender.Items[cb_gender.SelectedIndex].ToString();
            LoadF(1, 0);
        }

        private void tb_fio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string[] tempMass = new string[3];
                tempMass = tb_fio.Text.Split();

                for (int i = 0; i < tempMass.Length; i++)
                {
                    ms[i] = tempMass[i];
                }

                LoadF(1, 1);
            }
        }

        private void tb_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                phone = tb_phone.Text;
                int ind = phone.Length - 1;
                phone = phone.Remove(ind);
                LoadF(1, 2);
            }
        }

        private void tb_email_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                email = tb_email.Text;
                LoadF(1, 3);
            }
        }

        private void btn_newcl_Click(object sender, EventArgs e)
        {
            newClient_Form nvc = new newClient_Form();
            newClient_Form.connectionString = connectionString;
            nvc.Show();
            this.Hide();
            
        }
    }
}
