using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class ProcessBrowserForm : Form
	{
		public string PreviousProcess = null;
		private const string NoPreviousProcess = "No previous process";

		private static readonly string[] commonProcesses =
		{
			"[system process]", "system", "svchost.exe", "services.exe", "wininit.exe",
			"smss.exe", "csrss.exe", "lsass.exe", "winlogon.exe", "wininit.exe", "dwm.exe"
		};

		/// <summary>Gets the selected process.</summary>
		public Smdkd.SmProcessInfo SelectedProcess => (dataGridViewProcesses.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView)
			?.Row
			?.Field<Smdkd.SmProcessInfo>("info");

		public ProcessBrowserForm(string previousProcess = null)
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			InitDatagrind();

			PreviousProcess = previousProcess;
		}

        #region Events

        private void ProcessBrowserForm_Load(object sender, EventArgs e)
		{

			labelPreviousProcessLink.Text = string.IsNullOrEmpty(PreviousProcess) ? NoPreviousProcess : PreviousProcess;

			RefreshProcessList();

			foreach (var row in dataGridViewProcesses.Rows.Cast<DataGridViewRow>())
			{
				if (row.Cells[1].Value as string == PreviousProcess)
				{
					dataGridViewProcesses.CurrentCell = row.Cells[1];
					break;
				}
			}
		}
		private void textBoxProcessName_TextChanged(object sender, EventArgs e) => ApplyFilter();

		private void btnRefreshProcessList_Click(object sender, EventArgs e) => RefreshProcessList();
		private void checkBoxFilter_CheckedChanged(object sender, EventArgs e) => RefreshProcessList();

		private void labelPreviousProcessLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			textBoxProcessName.Text = labelPreviousProcessLink.Text == NoPreviousProcess ? string.Empty : labelPreviousProcessLink.Text;
		}
		private void dataGridViewProcesses_CellDoubleClick(object sender, DataGridViewCellEventArgs e) => btnAttachToProcess.PerformClick();
		#endregion

		#region Helpers
		private void InitDatagrind()
		{
			if (!SystemInformation.TerminalServerSession)
			{
				Type dgvType = dataGridViewProcesses.GetType();
				PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
				  BindingFlags.Instance | BindingFlags.NonPublic);
				pi.SetValue(dataGridViewProcesses, true, null);
			}

			dataGridViewProcesses.AutoGenerateColumns = false;
			dataGridViewProcesses.Columns[0].FillWeight = 25;//icon
			dataGridViewProcesses.Columns[1].FillWeight = dataGridViewProcesses.Width * 0.25f;//name
			dataGridViewProcesses.Columns[2].FillWeight = 45;//pid
			dataGridViewProcesses.Columns[3].FillWeight = dataGridViewProcesses.Width * 0.40f;//path
			Resize += new EventHandler((s, e) =>
			{
				dataGridViewProcesses.Columns[0].FillWeight = 25;//icon
				dataGridViewProcesses.Columns[1].FillWeight = dataGridViewProcesses.Width * 0.20f;//name
				dataGridViewProcesses.Columns[2].FillWeight = 45;//pid
				dataGridViewProcesses.Columns[3].FillWeight = dataGridViewProcesses.Width * 0.50f;//path
			});
		}
		/// <summary>Queries all processes and displays them.</summary>
		private void RefreshProcessList()
		{
			var dt = new DataTable();
			dt.Columns.Add("icon", typeof(Image));
			dt.Columns.Add("name", typeof(string));
			dt.Columns.Add("id", typeof(IntPtr));
			dt.Columns.Add("path", typeof(string));
			dt.Columns.Add("info", typeof(Smdkd.SmProcessInfo));
			var shouldFilter = checkBoxFilter.Checked;


			var processAfterFilter = Smdkd.EnumerateProcesses()
									.Where(p => !shouldFilter || !commonProcesses.Contains(p.Name.ToLower()))
									.Where(p => !checkBoxFilter32Bit.Checked || !p.IsWow64)
									.Where(p => !checkBoxFilter64Bit.Checked ||  p.IsWow64);

			if (checkBoxFilterDuplicateWindows.Checked)
				processAfterFilter = processAfterFilter.Distinct(new Smdkd.DistinctProcessInfoComparer());

			foreach (var p in processAfterFilter)
			{
				var row = dt.NewRow();
				row["icon"] = p.Icon;
				row["name"] = p.Name;
				row["id"] = p.Id;
				row["path"] = p.Path;
				row["info"] = p;
				dt.Rows.Add(row);
			}

			dt.DefaultView.Sort = "name ASC";
			dataGridViewProcesses.DataSource = dt;


			ApplyFilter();
		}
		private void ApplyFilter()
		{
			var filter = textBoxProcessName.Text;
			if (!string.IsNullOrEmpty(filter))
			{
				filter = $"name like '%{filter}%' or path like '%{filter}%'";
			}
		   ((DataTable)dataGridViewProcesses.DataSource).DefaultView.RowFilter = filter;
		}

        #endregion
    }
}
