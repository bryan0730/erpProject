using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProjectERP
{
    public partial class Form1 : Form
    {
        string strcoon = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        public Form1()
        {
            InitializeComponent();

        }
        private string loginCheck(string name, string number)
        {
            string rank = "";
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {
                conn.Open();
                string sql = "SELECT manage FROM employee WHERE empName='" + name + "' AND empNumber='" + number + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() == true)
                {
                    rank = rdr["manage"].ToString();
                }
                else
                {
                    rank = "3";
                }
                rdr.Close();
            }
            return rank;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form2 form2 = new Form2();
            form2.Owner = this;
            form2.Show();

            //string empName = idTextBox.Text;
            //string empNumber = pwTextBox.Text;

            //if (loginCheck(empName, empNumber) == "0")
            //{
            //    this.Visible = false;
            //    Form2 form2 = new Form2();
            //    form2.Owner = this;
            //    form2.Show();
            //}
            //else if (loginCheck(empName, empNumber) == "1")
            //{
            //    this.Visible = false;
            //    Form3 form3 = new Form3();
            //    form3.Owner = this;
            //    form3.Show();
            //}
            //else
            //{
            //    MessageBox.Show("아이디 또는 비밀번호 잘못되었습니다.");
            //}
        }
    }
}
