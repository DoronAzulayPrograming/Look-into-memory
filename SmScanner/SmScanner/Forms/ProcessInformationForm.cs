using System;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using SmScanner.Core.Modules;
using System.Drawing;
using System.Threading.Tasks;

namespace SmScanner.Forms
{
    public partial class ProcessInformationForm : Form
    {
        string prevValue;
        readonly RemoteProcess process;
        public ProcessInformationForm(RemoteProcess process)
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.process = process;

            tabControl.ImageList = new ImageList();
            tabControl.ImageList.Images.Add(Properties.Resources.B16x16_Category);
            tabControl.ImageList.Images.Add(Properties.Resources.B16x16_Page_White_Stack);
            tabPageModules.ImageIndex = 0;
            tabPageSections.ImageIndex = 1;

            InitDatagrids();

            FillOutTheForm();
        }

        private void InitDatagrids()
        {
            if (!SystemInformation.TerminalServerSession)
            {
                Program.SetControlDoubleBuffered(modulesDataGridView);
                Program.SetControlDoubleBuffered(sectionsDataGridView);
            }

            modulesDataGridView.AutoGenerateColumns = false;
            sectionsDataGridView.AutoGenerateColumns = false;

            DataGridViewCellCancelEventHandler copyOldValue = new DataGridViewCellCancelEventHandler((s, e) => prevValue =
            ((DataGridView)s)[e.ColumnIndex, e.RowIndex].Value.ToString());

            DataGridViewCellEventHandler restoreOldValue = new DataGridViewCellEventHandler((s, e) =>
            ((DataGridView)s)[e.ColumnIndex, e.RowIndex].Value = prevValue);

            modulesDataGridView.CellBeginEdit += copyOldValue;
            modulesDataGridView.CellEndEdit += restoreOldValue;

            sectionsDataGridView.CellBeginEdit += copyOldValue;
            sectionsDataGridView.CellEndEdit += restoreOldValue;

            // Resize the master DataGridView columns to fit the newly loaded data.
            modulesDataGridView.AutoResizeColumns();
            sectionsDataGridView.AutoResizeColumns();

            // Configure the details DataGridView so that its columns automatically
            // adjust their widths when the data changes.
            modulesDataGridView.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.AllCells;
            sectionsDataGridView.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void FillOutTheForm()
        {
            string hexFormat = Program.AddressHexFormat;
            var process = this.process.UnderlayingProcess;

            tabPageModules.Text = $"Modules Found: {this.process.Modules.Count()}.";
            tabPageSections.Text = $"Sections Found: {this.process.Sections.Count()}.";
        }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterToParent();

            var modules = process.Modules;
            var sections = process.Sections;

            var sectionsTable = new DataTable();
            sectionsTable.Columns.Add("address", typeof(string));
            sectionsTable.Columns.Add("size", typeof(string));
            sectionsTable.Columns.Add("name", typeof(string));
            sectionsTable.Columns.Add("protection", typeof(string));
            sectionsTable.Columns.Add("type", typeof(string));
            sectionsTable.Columns.Add("module", typeof(string));
            sectionsTable.Columns.Add("section", typeof(Smdkd.SmSection));


            var modulesTable = new DataTable();
            modulesTable.Columns.Add("icon", typeof(Icon));
            modulesTable.Columns.Add("name", typeof(string));
            modulesTable.Columns.Add("address", typeof(string));
            modulesTable.Columns.Add("size", typeof(string));
            modulesTable.Columns.Add("path", typeof(string));
            modulesTable.Columns.Add("module", typeof(Smdkd.SmModule));

            await Task.Run(() =>
            {
                if (process.EnumerateRemoteSectionsAndModules(out var sections, out var modules))
                {
                    foreach (var section in sections)
                    {
                        var row = sectionsTable.NewRow();
                        row["address"] = section.Start.ToString(Program.AddressHexFormat);
                        row["size"] = section.Size.ToString(Program.AddressHexFormat);
                        row["name"] = section.Name;
                        row["protection"] = section.Protection.ToString();
                        row["type"] = section.Type.ToString();
                        row["module"] = section.ModuleName;
                        row["section"] = section;
                        sectionsTable.Rows.Add(row);
                    }
                    foreach (var module in modules)
                    {
                        var row = modulesTable.NewRow();
                        row["icon"] = WinApi.GetIconForFile(module.Path);
                        row["name"] = module.Name;
                        row["address"] = module.Start.ToString(Program.AddressHexFormat);
                        row["size"] = module.Size.ToString(Program.AddressHexFormat);
                        row["path"] = module.Path;
                        row["module"] = module;
                        modulesTable.Rows.Add(row);
                    }
                }
            });

            sectionsDataGridView.DataSource = sectionsTable;
            modulesDataGridView.DataSource = modulesTable;
        }
    }
}
