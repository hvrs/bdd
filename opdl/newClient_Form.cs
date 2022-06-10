using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opdl
{
    public partial class newClient_Form : Form
    {
        public newClient_Form()
        {
            InitializeComponent();
        }

        private void tb_close_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        public static string connectionString;
        public static int maxID;
        private void newClient_Form_Load(object sender, EventArgs e)
        {
            tb_phone.MaxLength = 19;
            dp_birthday.Value = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlsort = $"select MAX(ID) from Client";
                SqlCommand sqlCommand1 = new SqlCommand(sqlsort, connection);
                maxID = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                connection.Close();
            }
            cb_gender.Items.AddRange(new string[] {"муж", "жен" });
            cb_gender.Text = "муж";
            maxID++;
            lbl_id.Text = maxID.ToString();


        }

        private void tb_save_Click(object sender, EventArgs e)
        {
            int gender;
            if ((tb_lastname.Text != string.Empty && tb_name.Text != string.Empty && tb_patronyc.Text != string.Empty && tb_phone.Text != string.Empty && tb_email.Text != string.Empty))
            {
                if (cb_gender.SelectedIndex == 0)
                    gender = 1;
                else 
                    gender = 2;
                MessageBox.Show(Convert.ToString(DateTime.Now));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = $"Insert into Client (ID, FirstName, LastName, Patronymic, Birthday, RegistrationDate, Email, Phone, GenderCode, PhotoPath) VALUES({maxID},'{tb_name.Text}', '{tb_lastname.Text}', '{tb_patronyc.Text}', '{dp_birthday.Value}', '{DateTime.Now:R}', '{tb_email.Text}', '{tb_phone.Text}', {gender}, '-'); ";
                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression, connection);
                    sqlCommand1.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
