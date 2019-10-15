using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Seminarski
{
    public partial class FormLogin : Form
    {
        const String STR_USERNAME = "Unesite korisničko ime";
        const String STR_PASS = "Unesite lozinku";
        const char CHR_PASS = '•';

        public void ClearAll()
        {
          textBoxUser.Text = STR_USERNAME;
          textBoxPass.Text = STR_PASS;
          textBoxUser.Focus();
        }

        public FormLogin()
        {
            InitializeComponent();
        }

        private void ValidateFields()
        {
          buttonLogin.Enabled = (textBoxUser.Text.Length>0 && textBoxPass.Text.Length>0 && textBoxUser.Text!=STR_USERNAME && textBoxPass.Text!=STR_PASS && !Program.loggedIn);
        }

        private void textBoxUser_Click(object sender, EventArgs e)
        {
            if (textBoxUser.Text == STR_USERNAME)
                textBoxUser.Text = "";
        }

        private void textBoxPass_Click(object sender, EventArgs e)
        {
            if (textBoxPass.Text == STR_PASS)
                textBoxPass.Text = "";
        }

        private void checkBoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPass.Checked)
                textBoxPass.PasswordChar = '\0';
            else
                textBoxPass.PasswordChar = CHR_PASS;
        }

        private void textBoxUser_Leave(object sender, EventArgs e)
        {
          if(textBoxUser.Text.Length == 0)
           textBoxUser.Text = STR_USERNAME;
        }

        private void textBoxPass_Leave(object sender, EventArgs e)
        {
          if(textBoxPass.Text.Length == 0)
           textBoxPass.Text = STR_PASS;
        }

        private void textBoxUser_TextChanged(object sender, EventArgs e)
        {
          ValidateFields();
        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {
          ValidateFields();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
          if(e.KeyData == Keys.Escape)
           Close();
        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader er = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("SELECT * FROM accounts WHERE user='{0}' AND pass='{1}'", textBoxUser.Text, textBoxPass.Text);
                er = cmd.ExecuteReader();
                if (er.HasRows)
                {
                    Form cp;
                    Program.user = textBoxUser.Text;
                    WindowState = FormWindowState.Minimized;
                    cp = new ControlPanel();
                    cp.Text = "Sistem - " + Program.user;
                    cp.Show();
                    Program.loggedIn = true;
                }
                else
                    MessageBox.Show("Uneli ste pogrešne podatke.\nPokušajte ponovo.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                er.Close();
                conn.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                er.Close();
                er.Dispose();
            }
        }

        private void textBoxPass_KeyDown(object sender, KeyEventArgs e)
        {
          if(e.KeyData == Keys.Enter)
           buttonLogin.PerformClick();
        }

        private void textBoxUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
             buttonLogin.PerformClick();
        }

        private void FormLogin_Resize(object sender, EventArgs e)
        {
          ClearAll();
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            Focus();
        }

        private void picInfo_Click(object sender, EventArgs e)
        {
            Form about;
            about = new About();
            about.ShowDialog();
        }

        private void picInfo_MouseEnter(object sender, EventArgs e)
        {
            picInfo.Cursor = Cursors.Hand;
        }

        private void picInfo_MouseLeave(object sender, EventArgs e)
        {
            picInfo.Cursor = Cursors.Default;
        }
    }
}
