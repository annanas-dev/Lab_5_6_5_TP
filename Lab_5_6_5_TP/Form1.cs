using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Lab_5_6_5_TP
{
    public partial class Form1 : Form
    {

        private List<Control> templateControls = new List<Control>();

        public Form1()
        {
            InitializeComponent();

            foreach (Control control in tabPage1.Controls)
            {
                templateControls.Add(control);
            }

        }

        
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создание новой вкладки
            TabPage tabPage = new TabPage("Набор данных " + (tabControl1.TabCount + 1));
            tabControl1.TabPages.Add(tabPage);

            // Копирование элементов управления с первой вкладки
            CopyControlsFromTemplate(tabPage);

            // Добавление DataGridView на новую вкладку
            AddDataGridView(tabPage);

        }
        

        private void CopyControlsFromTemplate(TabPage tabPage)
        {
            // Проверяем, что на новой вкладке еще нет элементов управления
            if (tabPage.Controls.Count == 0)
            {
                // Добавляем элементы управления из шаблона
                foreach (Control control in templateControls)
                {
                    Control newControl = (Control)Activator.CreateInstance(control.GetType());
                    newControl.Location = control.Location;
                    newControl.Size = control.Size;
                    if (control is TextBox)
                    {
                        ((TextBox)newControl).Text = ((TextBox)control).Text;
                    }
                    
                    tabPage.Controls.Add(newControl);
                    if (control is DataGrid)
                    {
                        DataGrid datagrid = (DataGrid)control;
                        ((DataGrid)newControl).Location = datagrid.Location;
                    }
                }
            }
        }

        private void AddDataGridView(TabPage tabPage)
        {
            // Добавляем DataGridView на новую вкладку
            DataGridView dataGridView = new DataGridView();
            dataGridView.Name = "dataGridView1";
            dataGridView.Dock = DockStyle.Bottom;
            dataGridView.Location = tabPage.Location;
            tabPage.Controls.Add(dataGridView);
            dataGridView.BringToFront();
            AddColumnsToDataGridView(dataGridView); // Добавляем столбцы
        }

        private void AddColumnsToDataGridView(DataGridView dataGridView)
        {
            
            // Добавляем столбцы в DataGridView
            dataGridView.Columns.Add("Column1", "X");
            dataGridView.Columns.Add("Column2", "Y");
            dataGridView.Columns.Add("Column3", "Result");

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        private void ProcessTabPage(TabPage tabPage)
        {
            // Получаем значения x0, xk, hx, y0, Ny, hy
            TextBox textBoxX0 = FindControl<TextBox>(tabPage, "textBox1");
            TextBox textBoxXk = FindControl<TextBox>(tabPage, "textBox2");
            TextBox textBoxStepX = FindControl<TextBox>(tabPage, "textBox3");

            TextBox textBoxY0 = FindControl<TextBox>(tabPage, "textBox4");
            TextBox textBoxNy = FindControl<TextBox>(tabPage, "textBox5");
            TextBox textBoxStepY = FindControl<TextBox>(tabPage, "textBox6");

            // Находим DataGridView
            TabPage selectedTabPage = tabControl1.SelectedTab;
            DataGridView dataGridView = selectedTabPage.Controls.OfType<DataGridView>().FirstOrDefault();

            // Проверяем, что все контролы были найдены
            if (textBoxX0 != null && textBoxXk != null && textBoxStepX != null &&
                textBoxY0 != null && textBoxNy != null && textBoxStepY != null && dataGridView != null)
            {
                // Рассчитываем и выводим точки для текущей вкладки
                dataGridView.Rows.Clear();
                double x0 = double.Parse(textBoxX0.Text);
                double xk = double.Parse(textBoxXk.Text);
                double hx = double.Parse(textBoxStepX.Text);
                double y0 = double.Parse(textBoxY0.Text);
                int Ny = int.Parse(textBoxNy.Text);
                double hy = double.Parse(textBoxStepY.Text);
                TabPage tab = tabControl1.SelectedTab;

                for (double x = x0; x <= xk; x += hx)
                {
                    for (double y = y0; y <= y0 + Ny * hy; y += hy)
                    {
                        double result = CalculateFunction(x, y);
                        dataGridView.Rows.Add(x, y, result);
                    }
                }
            }
            else
            {
                // Выводим информацию о том, какие элементы управления были найдены на вкладке
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Элементы управления на вкладке:");
                foreach (Control control in tabPage.Controls)
                {
                    sb.AppendLine($"Control name: {control.Name}");
                }
                MessageBox.Show(sb.ToString(), "Ошибка: Не удалось найти все элементы управления на вкладке.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private double CalculateFunction(double x, double y)
        {
            return y * Math.Pow(10, Math.Log(x) + 1);
        }

        private T FindControl<T>(TabPage tabPage, string name) where T : Control
        {
            foreach (Control control in tabPage.Controls)
            {
                if (control.Name == name && control is T)
                {
                    return (T)control;
                }
            }
            return null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                ProcessTabPage(tabPage);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Вывод всех точек из всех вкладок в DataGridView
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                ProcessTabPage(tabPage);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Dock = DockStyle.Bottom;
        }

        private void e(object sender, EventArgs e)
        {
            textBox2.Text = null;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = null;
        }
    }
}
