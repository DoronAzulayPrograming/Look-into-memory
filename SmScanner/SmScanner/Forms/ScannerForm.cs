using SmScanner.Controls;
using SmScanner.Core.Enums;
using SmScanner.Core.Exceptions;
using SmScanner.Core.Fotmattors;
using SmScanner.Core.Memory;
using SmScanner.Core.Modules;
using SmScanner.Core.Modules.MemoryScanner;
using SmScanner.Core.Modules.MemoryScanner.Comperer;
using SmScanner.Util;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class ScannerForm : Form
    {
        #region Label Fliker
        System.Windows.Forms.Timer timerFliker;
        bool dirUp = true;
        int r = 0, g = 255, b = 0, jump = 10;

        private void InitFliker()
        {
            timerFliker = new System.Windows.Forms.Timer();
            timerFliker.Enabled = true;
            timerFliker.Interval = 20;
            timerFliker.Tick += new EventHandler(timer_Tick);
        }
        private void StopFliter()
        {
            timerFliker.Stop();
            timerFliker.Enabled = false;
            labelFliker.BackColor = BackColor;
            r = 0; g = 255; b = 0;
            btnProcessBrowser.Location = new System.Drawing.Point(btnProcessBrowser.Location.X - 3, btnProcessBrowser.Location.Y - 3);
            btnProcessBrowser.Size = new System.Drawing.Size(btnProcessBrowser.Size.Width + 3, btnProcessBrowser.Size.Height + 3);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (r == 255 && dirUp) dirUp = false;
            else if (g == 0 && !dirUp) dirUp = true;

            if (g == 255 && !dirUp) dirUp = true;
            else if (r == 0 && dirUp) dirUp = false;

            if (dirUp) { r += jump; g -= jump; }
            else { r -= jump; g += jump; }

            if (r < 0) r = 0;
            else if (r > 255) r = 255;
            if (g < 0) g = 0;
            else if (g > 255) g = 255;

            labelFliker.BackColor = System.Drawing.Color.FromArgb(r, g, b);
        }
        #endregion

        public class ScanCompareTypeComboBox : EnumComboBox<ScanCompareType> { }
        public class ScanValueTypeComboBox : EnumComboBox<ScanValueType> { }

        private Scanner scanner;
        private MemoryViewForm memoryViewForm;
        private CancellationTokenSource cts;
        private bool isFirstScan;
        string currentProcessName = string.Empty;
        private const int MaxVisibleResults = 10000;

        public ScannerForm()
        {
            InitializeComponent();

            InitFliker();

            SetGuiFromSettings(ScanSettings.Default);

            OnValueTypeChanged();

            Reset();

            memoryViewForm = new MemoryViewForm(Program.RemoteProcess);

            btnFirstScan.Enabled = false;

            processKillToolStripMenuItem.Enabled = false;
            processResumeToolStripMenuItem.Enabled = false;
            processSuspendToolStripMenuItem.Enabled = false;
            processViewMemoryToolStripMenuItem.Enabled = false;
            processInformationToolStripMenuItem.Enabled = false;
        }

        private void resultListContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var cms = (ContextMenuStrip)sender;

            var isResultList = cms.SourceControl.Parent == resultsMemoryRecordList;

            toolStripMenuItemAddSelectedResultsToAddressList.Visible = isResultList;
            toolStripMenuItemChangeValue.Visible = !isResultList;
            toolStripMenuItemChangeDescription.Visible = !isResultList;
            toolStripMenuItemRemoveSelectedRecords.Visible = !isResultList;

            // Hide all other items if multiple records are selected.
            var multipleRecordsSelected = (isResultList ? resultsMemoryRecordList.SelectedRecords.Count : addressListMemoryRecordList.SelectedRecords.Count) > 1;
            for (var i = 4; i < cms.Items.Count; ++i)
                cms.Items[i].Visible = !multipleRecordsSelected;
        }
        private static MemoryRecordList GetMemoryRecordListFromMenuItem(object sender) =>
        (MemoryRecordList)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Parent;
        #region Events

        private void ScannerForm_Load(object sender, EventArgs e)
        {
            if (!Smdkd.Attach()) Program.ShowException(new DriverException("Not loaded."));
        }
        private void updateValuesTimer_Tick(object sender, EventArgs e)
        {
            resultsMemoryRecordList.RefreshValues(Program.RemoteProcess);
            addressListMemoryRecordList.RefreshValues(Program.RemoteProcess);
        }
        private async void btnNextScan_Click(object sender, EventArgs e)
        {
            if (!Program.RemoteProcess.IsValid) return;

            if (!isFirstScan)
            {
                btnFirstScan.Enabled = false;
                btnNextScan.Enabled = false;
                btnCencelScan.Visible = true;

                try
                {
                    var comparer = CreateComparer(scanner.Settings);

                    var report = new Progress<int>(i =>
                    {
                        progressBar.Value = i;
                        SetResultCount(scanner.TotalResultCount);
                    });
                    cts = new CancellationTokenSource();

                    await scanner.Search(comparer, report, cts.Token);

                    ShowScannerResults(scanner);

                    //undoIconButton.Enabled = scanner.CanUndoLastScan;
                }
                catch (Exception ex)
                {
                    Program.ShowException(ex);
                }

                btnFirstScan.Enabled = true;
                btnNextScan.Enabled = true;
                btnCencelScan.Visible = false;

                progressBar.Value = 0;
            }
        }
        private async void btnFirstScan_Click(object sender, EventArgs e)
        {
            if (isFirstScan)
            {
                try
                {
                    var settings = CreateSearchSettings();
                    var comparer = CreateComparer(settings);

                    await StartFirstScanEx(settings, comparer);
                }
                catch (Exception ex)
                {
                    Program.ShowException(ex);
                }

                return;
            }

            Reset();
        }
        private void btnCencelScan_Click(object sender, EventArgs e) => cts?.Cancel();
        private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var record = GetMemoryRecordListFromMenuItem(sender)?.SelectedRecord;
            if (record != null)
            {
                Clipboard.SetText(record.RealAddress.ToString("X"));
            }
        }
        private void changeValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var records = addressListMemoryRecordList.SelectedRecords?.Count > 0 ? addressListMemoryRecordList.SelectedRecords : null;
            if (records == null) return;
            MemoryRecord record = records[0];

            var cvf = new ChangeValueForm(record.ValueStr);
            if (cvf.ShowDialog(this) == DialogResult.OK)
            {
                Encoding encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;

                byte[] btyes = null;
                switch (record.ValueType)
                {
                    case ScanValueType.Byte:
                        btyes = BitConverter.GetBytes(byte.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.Short:
                        btyes = BitConverter.GetBytes(short.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.Integer:
                        btyes = BitConverter.GetBytes(int.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.Long:
                        btyes = BitConverter.GetBytes(long.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.Float:
                        btyes = BitConverter.GetBytes(float.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.Double:
                        btyes = BitConverter.GetBytes(double.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.ArrayOfBytes:
                        btyes = BitConverter.GetBytes(long.Parse(cvf.ValueText));
                        break;
                    case ScanValueType.String:
                        btyes = encoding.GetBytes(cvf.ValueText);
                        break;
                    case ScanValueType.Regex:
                        btyes = encoding.GetBytes(cvf.ValueText);
                        break;
                }

                foreach (var r in records)
                {
                    Program.RemoteProcess.WriteRemoteMemory(r.RealAddress, btyes);
                    r.RefreshValue(Program.RemoteProcess);
                }
            }
        }
        private void attacthProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pbf = new ProcessBrowserForm(currentProcessName);
            if (pbf.ShowDialog(this) == DialogResult.OK)
            {
                var selectedProcess = pbf.SelectedProcess;
                if (System.IO.Path.GetFileName(selectedProcess.Path).Equals(Program.Settings.LastProcess)) return;
                if (timerFliker.Enabled) StopFliter();
                AttachToProcess(pbf.SelectedProcess);
            }
        }
        private void copyAddressTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var record = GetMemoryRecordListFromMenuItem(sender)?.SelectedRecord;
            if (record != null)
            {
                if (record.ModuleName != null)
                    Clipboard.SetText($"{record.ModuleName} + {record.AddressOrOffset.ToString("X")}");
                else
                    Clipboard.SetText(record.AddressOrOffset.ToString("X"));
            }
        }
        private void changeDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var records = addressListMemoryRecordList.SelectedRecords?.Count > 0 ? addressListMemoryRecordList.SelectedRecords : null;
            if (records == null) return;
            var cvf = new ChangeValueForm(records[0].Description);
            if (cvf.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var r in records)
                    r.Description = cvf.ValueText;
                addressListMemoryRecordList.ClearSelection();
            }
        }
        private void removeSelectedRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var record in addressListMemoryRecordList.SelectedRecords)
                addressListMemoryRecordList.Records.Remove(record);
        }
        private void btnTransferSelectedResultToAddressList_Click(object sender, EventArgs e)
        {
            if (resultsMemoryRecordList.SelectedRecords?.Count <= 0) return;
            foreach (var record in resultsMemoryRecordList.SelectedRecords.Reverse())
                addressListMemoryRecordList.Records.Add(record);

            resultsMemoryRecordList.ClearSelection();
        }
        private void addSelectedResultsToAddressListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (resultsMemoryRecordList.SelectedRecords?.Count <= 0) return;
            foreach (var record in resultsMemoryRecordList.SelectedRecords.Reverse())
                addressListMemoryRecordList.Records.Add(record);
            resultsMemoryRecordList.ClearSelection();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();
        private void ScannerForm_FormClosing(object sender, FormClosingEventArgs e) => Smdkd.Detach();
        private void btnClearAddressList_Click(object sender, EventArgs e) => addressListMemoryRecordList.Clear();
        private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e) => OnValueTypeChanged();
        private void scanTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e) => OnCompareTypeChanged();
        private void processDetachToolStripMenuItem_Click(object sender, EventArgs e) => Program.RemoteProcess.Close();
        private void btnProcessBrowser_Click(object sender, EventArgs e) => attacthProcessToolStripMenuItem.PerformClick();
        private void resultsMemoryRecordList_RecordDoubleClick(object sender, MemoryRecord record) => addressListMemoryRecordList.Records.Add(record);
        private void processInformationToolStripMenuItem_Click(object sender, EventArgs e) => new ProcessInformationForm(Program.RemoteProcess).Show(this);
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastProcess = Program.Settings.LastProcess;
            if (string.IsNullOrEmpty(lastProcess))
            {
                reattachProcessToolStripMenuItem.Visible = false;
            }
            else
            {
                reattachProcessToolStripMenuItem.Visible = true;
                reattachProcessToolStripMenuItem.Text = $"Re-Attach to '{lastProcess}'";
            }
        }
        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastProcess = Program.Settings.LastProcess;
            if (string.IsNullOrEmpty(lastProcess))
            {
                processKillToolStripMenuItem.Enabled = false;
                processResumeToolStripMenuItem.Enabled = false;
                processSuspendToolStripMenuItem.Enabled = false;
                processViewMemoryToolStripMenuItem.Enabled = false;
                processInformationToolStripMenuItem.Enabled = false;
            }
            else
            {
                processKillToolStripMenuItem.Enabled = true;
                processResumeToolStripMenuItem.Enabled = true;
                processSuspendToolStripMenuItem.Enabled = true;
                processViewMemoryToolStripMenuItem.Enabled = true;
                processInformationToolStripMenuItem.Enabled = true;
            }
        }
        private void reattachProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastProcess = Program.Settings.LastProcess;
            if (string.IsNullOrEmpty(lastProcess))
            {
                return;
            }

            AttachToProcess(lastProcess);
        }
        private void controlProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.RemoteProcess.IsValid)
            {
                return;
            }

            var action = ControlRemoteProcessAction.Terminate;
            if (sender == processResumeToolStripMenuItem)
            {
                action = ControlRemoteProcessAction.Resume;
            }
            else if (sender == processSuspendToolStripMenuItem)
            {
                action = ControlRemoteProcessAction.Suspend;
            }

            Program.RemoteProcess.ControlRemoteProcess(action);
        }
        private void processViewMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoryViewForm.Show();
            memoryViewForm.BringToFront();
        }
        #endregion

        #region Helpers
        public void AttachToProcess(string processName)
        {
            
            Contract.Requires(processName != null);

            var info = Smdkd.EnumerateProcesses().FirstOrDefault(p => string.Equals(p.Name, processName, StringComparison.OrdinalIgnoreCase));
            if (info == null)
            {
                MessageBox.Show($"Process '{processName}' could not be found.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                Program.Settings.LastProcess = string.Empty;
            }
            else
            {
                AttachToProcess(info);
            }
        }
        public void AttachToProcess(Smdkd.SmProcessInfo info)
        {
            Contract.Requires(info != null);

            Program.RemoteProcess.Open(info);

            Program.RemoteProcess.UpdateProcessInformations();

            Program.Settings.LastProcess = Program.RemoteProcess.UnderlayingProcess.Name;

            Program.RemoteProcess.ProcessClosed += new RemoteProcessEvent(RemoteProcessClosed);
            SetGuiFromSettings(ScanSettings.Default);

            currentProcessName = System.IO.Path.GetFileName(info.Path);
            labelProcessSelected.Text = $"PID: {(info.IsWow64 ? info.Id.ToInt32() : info.Id.ToInt64())} , Name: {System.IO.Path.GetFileName(info.Path)}";

            btnFirstScan.Enabled = true;

            memoryViewForm.InitFromRandomAddress();
        }
        private void RemoteProcessClosed(RemoteProcess sender)
        {
            Program.Settings.LastProcess = null;

            SetGuiFromSettings(ScanSettings.Default);

            Reset();

            btnFirstScan.Enabled = false;

            processKillToolStripMenuItem.Enabled = false;
            processResumeToolStripMenuItem.Enabled = false;
            processSuspendToolStripMenuItem.Enabled = false;
            processInformationToolStripMenuItem.Enabled = false;

            labelProcessSelected.Text = $"No Process Selected";
        }



        /// <summary>
        /// Starts a new first scan with the provided settings and comparer.
        /// </summary>
        /// <param name="settings">The scan settings.</param>
        /// <param name="comparer">The comparer.</param>
        private async Task StartFirstScanEx(ScanSettings settings, IScanComparer comparer)
        {
            if (!Program.RemoteProcess.IsValid) return;

            btnFirstScan.Enabled = false;
            btnCencelScan.Visible = true;

            try
            {
                scanner = new Scanner(Program.RemoteProcess, settings);

                var report = new Progress<int>(i =>
                {
                    progressBar.Value = i;
                    SetResultCount(scanner.TotalResultCount);
                });
                cts = new CancellationTokenSource();

                await scanner.Search(comparer, report, cts.Token);

                ShowScannerResults(scanner);

                btnCencelScan.Visible = false;
                btnNextScan.Enabled = true;
                valueTypeComboBox.Enabled = false;

                floatingOptionsGroupBox.Enabled = false;
                stringOptionsGroupBox.Enabled = false;
                scanOptionsGroupBox.Enabled = false;

                isFirstScan = false;

                SetValidCompareTypes();
                OnCompareTypeChanged();
            }
            finally
            {
                btnFirstScan.Enabled = true;

                progressBar.Value = 0;
            }
        }
        /// <summary>
        /// Displays the total result count.
        /// </summary>
        /// <param name="count">Number of.</param>
        private void SetResultCount(int count)
        {
            string str = count.ToString("C"); //1736 = $1,736.00
            str = str.Substring(1, str.Length - 4); //$1736.00 = 1,736
            resultCountLabel.Text = count > MaxVisibleResults ? $"Found: {str} (only {MaxVisibleResults} shown)" : $"Found: {str}";
        }

        /// <summary>
        /// Shows some of the scanner results.
        /// </summary>
        public void ShowScannerResults(Scanner scanner)
        {
            Contract.Requires(scanner != null);

            SetResultCount(scanner.TotalResultCount);

            resultsMemoryRecordList.SetRecords(
                scanner.GetResults()
                    .Take(MaxVisibleResults)
                    .OrderBy(r => r.Address, IntPtrComparer.Instance)
                    .Select(r =>
                    {
                        var record = new MemoryRecord(r);
                        record.ResolveAddress(Program.RemoteProcess);
                        return record;
                    })
            );
        }
        private void Reset()
        {
            //scanner?.Dispose();
            //scanner = null;

            btnCencelScan.Visible = false;

            SetResultCount(0);
            resultsMemoryRecordList.Clear();

            btnFirstScan.Enabled = true;
            btnNextScan.Enabled = false;

            isHexCheckBox.Enabled = true;

            valueTypeComboBox.Enabled = true;
            //valueTypeComboBox.SelectedItem = valueTypeComboBox.Items.Cast<EnumDescriptionDisplay<ScanValueType>>().PredicateOrFirst(e => e.Value == ScanValueType.Integer);
            OnValueTypeChanged();

            floatingOptionsGroupBox.Enabled = true;
            stringOptionsGroupBox.Enabled = true;
            scanOptionsGroupBox.Enabled = true;

            isFirstScan = true;

            SetValidCompareTypes();
        }

        /// <summary>
        /// Creates the search settings from the user input.
        /// </summary>
        /// <returns>The scan settings.</returns>
        private ScanSettings CreateSearchSettings()
        {
            Contract.Ensures(Contract.Result<ScanSettings>() != null);

            var settings = new ScanSettings
            {
                ValueType = valueTypeComboBox.SelectedValue
            };

            long.TryParse(startAddressTextBox.Text, NumberStyles.HexNumber, null, out var startAddressVar);
            long.TryParse(stopAddressTextBox.Text, NumberStyles.HexNumber, null, out var endAddressVar);

            if (!Program.RemoteProcess.IsWow64)
            {
                settings.StartAddress = (IntPtr)startAddressVar;
                settings.StopAddress = (IntPtr)endAddressVar;
            }
            else
            {
                settings.StartAddress = unchecked((IntPtr)(int)startAddressVar);
                settings.StopAddress = unchecked((IntPtr)(int)endAddressVar);
            }


            settings.EnableFastScan = fastScanCheckBox.Checked;
            int.TryParse(fastScanAlignmentTextBox.Text, out var alignment);
            settings.FastScanAlignment = Math.Max(1, alignment);

            static SettingState CheckStateToSettingState(CheckState state)
            {
                switch (state)
                {
                    case CheckState.Checked:
                        return SettingState.Yes;
                    case CheckState.Unchecked:
                        return SettingState.No;
                    default:
                        return SettingState.Indeterminate;
                }
            }

            settings.ScanPrivateMemory = scanPrivateCheckBox.Checked;
            settings.ScanImageMemory = scanImageCheckBox.Checked;
            settings.ScanMappedMemory = scanMappedCheckBox.Checked; 
            settings.ScanWritableMemory = CheckStateToSettingState(scanWritableCheckBox.CheckState);
            settings.ScanExecutableMemory = CheckStateToSettingState(scanExecutableCheckBox.CheckState);
            settings.ScanCopyOnWriteMemory = SettingState.No; // CheckStateToSettingState(scanon.CheckState);

            return settings;
        }

        /// <summary>
        /// Creates the comparer from the user input.
        /// </summary>
        /// <returns>The scan comparer.</returns>
        private IScanComparer CreateComparer(ScanSettings settings)
        {
            Contract.Requires(settings != null);
            Contract.Ensures(Contract.Result<IScanComparer>() != null);

            var compareType = compareTypeComboBox.SelectedValue;
            var checkBothInputFields = compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual;

            if (settings.ValueType == ScanValueType.Byte || settings.ValueType == ScanValueType.Short || settings.ValueType == ScanValueType.Integer || settings.ValueType == ScanValueType.Long)
            {
                var numberStyle = isHexCheckBox.Checked ? NumberStyles.HexNumber : NumberStyles.Integer;
                if (!long.TryParse(dualValueBox.Value1, numberStyle, null, out var value1)) throw new InvalidInputException(dualValueBox.Value1);
                if (!long.TryParse(dualValueBox.Value2, numberStyle, null, out var value2) && checkBothInputFields) throw new InvalidInputException(dualValueBox.Value2);

                if (compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual)
                {
                    if (value1 > value2)
                    {
                        Utils.Swap(ref value1, ref value2);
                    }
                }

                switch (settings.ValueType)
                {
                    case ScanValueType.Byte:
                        return new ByteMemoryComparer(compareType, (byte)value1, (byte)value2);
                    case ScanValueType.Short:
                        return new ShortMemoryComparer(compareType, (short)value1, (short)value2);
                    case ScanValueType.Integer:
                        return new IntegerMemoryComparer(compareType, (int)value1, (int)value2);
                    case ScanValueType.Long:
                        return new LongMemoryComparer(compareType, value1, value2);
                }
            }
            else if (settings.ValueType == ScanValueType.Float || settings.ValueType == ScanValueType.Double)
            {
                int CalculateSignificantDigits(string input, NumberFormatInfo numberFormat)
                {
                    Contract.Requires(input != null);
                    Contract.Requires(numberFormat != null);

                    var digits = 0;

                    var decimalIndex = input.IndexOf(numberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
                    if (decimalIndex != -1)
                    {
                        digits = input.Length - 1 - decimalIndex;
                    }

                    return digits;
                }

                var nf1 = NumberFormat.GuessNumberFormat(dualValueBox.Value1);
                if (!double.TryParse(dualValueBox.Value1, NumberStyles.Float, nf1, out var value1)) throw new InvalidInputException(dualValueBox.Value1);
                var nf2 = NumberFormat.GuessNumberFormat(dualValueBox.Value2);
                if (!double.TryParse(dualValueBox.Value2, NumberStyles.Float, nf2, out var value2) && checkBothInputFields) throw new InvalidInputException(dualValueBox.Value2);

                if (compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual)
                {
                    if (value1 > value2)
                    {
                        Utils.Swap(ref value1, ref value2);
                    }
                }

                var significantDigits = Math.Max(
                    CalculateSignificantDigits(dualValueBox.Value1, nf1),
                    CalculateSignificantDigits(dualValueBox.Value2, nf2)
                );

                var roundMode = roundStrictRadioButton.Checked ? ScanRoundMode.Strict : roundLooseRadioButton.Checked ? ScanRoundMode.Normal : ScanRoundMode.Truncate;

                switch (settings.ValueType)
                {
                    case ScanValueType.Float:
                        return new FloatMemoryComparer(compareType, roundMode, significantDigits, (float)value1, (float)value2);
                    case ScanValueType.Double:
                        return new DoubleMemoryComparer(compareType, roundMode, significantDigits, value1, value2);
                }
            }
            else if (settings.ValueType == ScanValueType.ArrayOfBytes)
            {
                var pattern = BytePattern.Parse(dualValueBox.Value1);

                return new ArrayOfBytesMemoryComparer(pattern);
            }
            else if (settings.ValueType == ScanValueType.String || settings.ValueType == ScanValueType.Regex)
            {
                if (string.IsNullOrEmpty(dualValueBox.Value1))
                {
                    throw new InvalidInputException(dualValueBox.Value1);
                }

                var encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;
                if (settings.ValueType == ScanValueType.String)
                {
                    return new StringMemoryComparer(dualValueBox.Value1, encoding, caseSensitiveCheckBox.Checked);
                }
                else
                {
                    return new RegexStringMemoryComparer(dualValueBox.Value1, encoding, caseSensitiveCheckBox.Checked);
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Sets the input fields according to the provided settings.
        /// </summary>
        /// <param name="settings">The scan settings.</param>
        private void SetGuiFromSettings(ScanSettings settings)
        {
            Contract.Requires(settings != null);

            valueTypeComboBox.SelectedValue = settings.ValueType;

            startAddressTextBox.Text = settings.StartAddress.ToString(Program.AddressHexFormat);
            stopAddressTextBox.Text = settings.StopAddress.ToString(Program.AddressHexFormat);

            fastScanCheckBox.Checked = settings.EnableFastScan;
            fastScanAlignmentTextBox.Text = Math.Max(1, settings.FastScanAlignment).ToString();

            static CheckState SettingStateToCheckState(SettingState state)
            {
                switch (state)
                {
                    case SettingState.Yes:
                        return CheckState.Checked;
                    case SettingState.No:
                        return CheckState.Unchecked;
                    default:
                        return CheckState.Indeterminate;
                }
            }

            scanPrivateCheckBox.Checked = settings.ScanPrivateMemory;
            scanImageCheckBox.Checked = settings.ScanImageMemory;
            scanMappedCheckBox.Checked = settings.ScanMappedMemory;
            scanWritableCheckBox.CheckState = SettingStateToCheckState(settings.ScanWritableMemory);
            scanExecutableCheckBox.CheckState = SettingStateToCheckState(settings.ScanExecutableMemory);
        }

        /// <summary>
        /// Set input elements according to the selected compare type.
        /// </summary>
        private void OnCompareTypeChanged()
        {
            var enableHexCheckBox = true;
            var enableValueBox = true;
            var enableDualInput = false;

            switch (compareTypeComboBox.SelectedValue)
            {
                case ScanCompareType.Unknown:
                    enableHexCheckBox = false;
                    enableValueBox = false;
                    break;
                case ScanCompareType.Between:
                case ScanCompareType.BetweenOrEqual:
                    enableDualInput = true;
                    break;
            }

            switch (valueTypeComboBox.SelectedValue)
            {
                case ScanValueType.Float:
                case ScanValueType.Double:
                case ScanValueType.ArrayOfBytes:
                case ScanValueType.String:
                case ScanValueType.Regex:
                    isHexCheckBox.Checked = false;
                    enableHexCheckBox = false;
                    break;
            }

            isHexCheckBox.Enabled = enableHexCheckBox;
            dualValueBox.Enabled = enableValueBox;
            dualValueBox.ShowSecondInputField = enableDualInput;
        }

        /// <summary>
        /// Hide gui elements after the value type has changed.
        /// </summary>
		private void OnValueTypeChanged()
        {
            SetValidCompareTypes();

            var valueType = valueTypeComboBox.SelectedValue;

            switch (valueType)
            {
                case ScanValueType.Byte:
                case ScanValueType.Short:
                case ScanValueType.Integer:
                case ScanValueType.Long:
                    isHexCheckBox.Enabled = true;
                    break;
                case ScanValueType.Float:
                case ScanValueType.Double:
                case ScanValueType.ArrayOfBytes:
                case ScanValueType.String:
                case ScanValueType.Regex:
                    isHexCheckBox.Checked = false;
                    isHexCheckBox.Enabled = false;
                    break;
            }

            var alignment = 1;
            switch (valueType)
            {
                case ScanValueType.Short:
                    alignment = 2;
                    break;
                case ScanValueType.Float:
                case ScanValueType.Double:
                case ScanValueType.Integer:
                case ScanValueType.Long:
                    alignment = 4;
                    break;
            }
            fastScanAlignmentTextBox.Text = alignment.ToString();

            floatingOptionsGroupBox.Visible = valueType == ScanValueType.Float || valueType == ScanValueType.Double;
            stringOptionsGroupBox.Visible = valueType == ScanValueType.String || valueType == ScanValueType.Regex;
        }
        /// <summary>
        /// Sets valid compare types dependend on the selected value type.
        /// </summary>
        private void SetValidCompareTypes()
        {
            var compareType = compareTypeComboBox.SelectedValue;
            var valueType = valueTypeComboBox.SelectedValue;
            if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String || valueType == ScanValueType.Regex)
            {
                compareTypeComboBox.SetAvailableValues(ScanCompareType.Equal);
            }
            else if (isFirstScan)
            {
                compareTypeComboBox.SetAvailableValuesExclude(
                    ScanCompareType.Changed, ScanCompareType.NotChanged, ScanCompareType.Decreased,
                    ScanCompareType.DecreasedOrEqual, ScanCompareType.Increased, ScanCompareType.IncreasedOrEqual
                );
            }
            else
            {
                compareTypeComboBox.SetAvailableValuesExclude(ScanCompareType.Unknown);
            }

            compareTypeComboBox.SelectedValue = compareType;
        }

        #endregion
    }


    public static class LongEx
    {
        public static long Long(this Random rand,long min,long max)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
