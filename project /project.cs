//Form1
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class Form1 : Form
    {
        private List<Workout> workouts = new List<Workout>();
        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
            UpdateStatistics();
        }

        private void SetupDataGridView()
        {
            dataGridViewWorkouts.Columns.Clear();

        
            dataGridViewWorkouts.AutoGenerateColumns = false;
            dataGridViewWorkouts.AllowUserToAddRows = false;

            
            DataGridViewTextBoxColumn nameCol = new DataGridViewTextBoxColumn();
            nameCol.HeaderText = "Название тренировки";
            nameCol.Name = "Name";
            dataGridViewWorkouts.Columns.Add(nameCol);

            
            DataGridViewTextBoxColumn dateCol = new DataGridViewTextBoxColumn();
            dateCol.HeaderText = "Дата";
            dateCol.Name = "Date";
            dateCol.DefaultCellStyle.Format = "d"; // Короткий формат даты
            dataGridViewWorkouts.Columns.Add(dateCol);

           
            DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
            chkCol.HeaderText = "Выполнено";
            chkCol.Name = "IsCompleted";
            dataGridViewWorkouts.Columns.Add(chkCol);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            if (form.ShowDialog() == DialogResult.OK)
            {
                workouts.Add(new Workout
                {
                    Name = form.WorkoutName,
                    Date = form.WorkoutDate,
                    IsCompleted = form.IsCompleted
                });
                RefreshData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkouts.SelectedRows.Count == 0) return;

            var selected = workouts[dataGridViewWorkouts.SelectedRows[0].Index];
            Form2 form = new Form2();
            form.WorkoutName = selected.Name;
            form.WorkoutDate = selected.Date;
            form.IsCompleted = selected.IsCompleted;

            if (form.ShowDialog() == DialogResult.OK)
            {
                selected.Name = form.WorkoutName;
                selected.Date = form.WorkoutDate;
                selected.IsCompleted = form.IsCompleted;
                RefreshData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkouts.SelectedRows.Count == 0) return;

            workouts.RemoveAt(dataGridViewWorkouts.SelectedRows[0].Index);
            RefreshData();
        }

        private void RefreshData()
        {
            dataGridViewWorkouts.Rows.Clear();
            foreach (var workout in workouts)
            {
                int rowIndex = dataGridViewWorkouts.Rows.Add();
                DataGridViewRow row = dataGridViewWorkouts.Rows[rowIndex];
                row.Cells["Name"].Value = workout.Name;
                row.Cells["Date"].Value = workout.Date;
                row.Cells["IsCompleted"].Value = workout.IsCompleted;
            }
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            int total = workouts.Count;
            int completed = workouts.Count(w => w.IsCompleted);

            if (total > 0)
            {
                double percentage = (double)completed / total * 100;
                lblStatistics.Text = $"Выполнено: {completed} из {total} ({percentage:F1}%)";
            }
            else
            {
                lblStatistics.Text = "Нет тренировок для отображения статистики";
            }
        }

        private void dataGridViewWorkouts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2) 
            {
                workouts[e.RowIndex].IsCompleted = (bool)dataGridViewWorkouts.Rows[e.RowIndex].Cells[2].Value;
                UpdateStatistics();
            }
        }
    }
}


//Form2
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class Form2 : Form
    {

        public string WorkoutName { get; set; }
        public DateTime WorkoutDate { get; set; }
        public bool IsCompleted { get; set; }

        public Form2()
        {
            InitializeComponent();
            WorkoutDate = DateTime.Today;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorkoutName.Text))
            {
                MessageBox.Show("Введите название тренировки", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WorkoutName = txtWorkoutName.Text;
            WorkoutDate = dtpWorkoutDate.Value;
            IsCompleted = chkIsCompleted.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            txtWorkoutName.Text = WorkoutName;
            dtpWorkoutDate.Value = WorkoutDate;
            chkIsCompleted.Checked = IsCompleted;
        }
    }
}


//Workout
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    public class Workout
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
    }
}
