using Newtonsoft.Json;
using SmScanner.Core.Extensions;
using SmScanner.Core.Interfaces;
using SmScanner.Core.Memory;
using SmScanner.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmScanner.Controls
{
    public delegate void DisassembleRecordListRefreshCompletedEventHandler();
    public delegate void DisassembleRecordListRecordDoubleClickEventHandler(object sender, DisassembledRecord record);
    public partial class DisassembleRecordList : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<DisassembledRecord> Records => bindings;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DisassembledRecord SelectedRecord => GetSelectedRecords().FirstOrDefault();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<DisassembledRecord> SelectedRecords => GetSelectedRecords().ToList();
        public void ScrollTo(DisassembledRecord record) => disassemblerDataGridView.ScrollTo(record);
        public void ScrollTo(IntPtr address) => disassemblerDataGridView.ScrollToDisassembleAddress(address);
        public event DisassembleRecordListRecordDoubleClickEventHandler RecordDoubleClick;
        public event DisassembleRecordListRefreshCompletedEventHandler RefreshCompleted;
        public override ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }

        //private int READ_SIZE = 1024;
        private int READ_SIZE { get => Program.Settings.DissassemblerBufferSize; }

        private List<AsmIns> asmIns32;
        private List<AsmIns> asmIns64;
        private IRemoteProcess process;
        private Disassembler disassembler;
        private DisassemblerWrapper wrapper;
        private readonly BindingList<DisassembledRecord> bindings;

        IntPtr scroll_to_address;
        IEnumerable<DisassembledRecord> records_to_select;
        bool refreshInJob = false;
        bool isFirstFillFinish = false;

        private IEnumerable<DisassembledRecord> GetSelectedRecords() => disassemblerDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem);
        
        public DisassembleRecordList()
        {

            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (!SystemInformation.TerminalServerSession) 
                Program.SetControlDoubleBuffered(disassemblerDataGridView);

            if (Program.DesignMode) return;

            asmIns32 = new List<AsmIns>();
            StreamReader reader = new StreamReader($"{Environment.CurrentDirectory}\\AsmsInstraction32.json");
            string jsonFormat = reader.ReadToEnd();
            asmIns32 = JsonConvert.DeserializeObject<List<AsmIns>>(jsonFormat);
            
            asmIns64 = new List<AsmIns>();
            reader = new StreamReader($"{Environment.CurrentDirectory}\\AsmsInstraction64.json");
            jsonFormat = reader.ReadToEnd();
            asmIns64 = JsonConvert.DeserializeObject<List<AsmIns>>(jsonFormat);

            disassemblerDataGridView.AutoGenerateColumns = false;
            disassemblerDataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.DarkBlue;

            bindings = new BindingList<DisassembledRecord>
            {
                AllowNew = true,
                AllowEdit = true,
                RaiseListChangedEvents = true
            };
            disassemblerDataGridView.DataSource = bindings;

            disassemblerDataGridView.SelectionChanged += new EventHandler(disassemblerDataGridView_SelectionChanged);
            disassemblerDataGridView.Scroll += new ScrollEventHandler(disassemblerDataGrid_Scroll);
            disassemblerDataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(disassemblerDataGridCell_Painting);
            disassemblerDataGridView.CellDoubleClick += new DataGridViewCellEventHandler(disassemblerDataGridView_CellDoubleClick);

            RefreshCompleted = new DisassembleRecordListRefreshCompletedEventHandler(disassemblerDataGridView_RefreshCompleted);
        }

        public void Init(IRemoteProcess process, IDisassemblerWrapper wrapper)
        {
            Contract.Requires(process != null);

            this.process = process;

            this.wrapper = wrapper.Clone();
            disassembler = new Disassembler(wrapper);
        }

        #region Helpers
        public void RefreshRecords()
        {
            bool isFixd = false;
            DisassembledRecord prev;
            lock (Records)
            {
                if (Records[0].Refresh(process, disassembler))
                    isFixd = true;
                for (int i = 1; i < Records.Count; i++)
                {
                    if (!isFixd && Records[i].Refresh(process, disassembler))
                    {
                        isFixd = true;
                        continue;
                    }

                    if (!isFixd) continue;

                    prev = Records[i - 1];

                    Records[i].Address = prev.Address.Add(prev.Length);
                    Records[i].Refresh(process, disassembler);
                }
            }

            disassemblerDataGridView.Invalidate();
        }
        public Task RefreshRecordsAsync(Disassembler disassembler)
        {
            return Task.Run(() =>
            {
                bool isFixd = false;
                DisassembledRecord prev;
                lock (Records)
                {
                    if (Records[0].Refresh(process, disassembler))
                        isFixd = true;
                    for (int i = 1; i < Records.Count; i++)
                    {
                        if (!isFixd && Records[i].Refresh(process, disassembler))
                        {
                            isFixd = true;
                            continue;
                        }

                        if (!isFixd) continue;

                        prev = Records[i - 1];

                        Records[i].Address = prev.Address.Add(prev.Length);
                        Records[i].Refresh(process, disassembler);
                    }
                }

                if (!IsDisposed)
                    BeginInvoke(new Action(() => { disassemblerDataGridView.Invalidate(); }));
            });
        }

        public void RefreshVisibleRecords()
        {
            if (refreshInJob) return;
            refreshInJob = true;
            /*var visibleRows = disassemblerDataGridView.GetVisibleRows().Cast<DataGridViewRow>();

            if (visibleRows.Count() < 1) return;

            int lastIndex = visibleRows.Last().Index;
            int firstIndex = visibleRows.First().Index;
            firstIndex += firstIndex == 0 ? 1 : 0;

            int size = 10;
            firstIndex = firstIndex > size ? firstIndex - size : 1;
            lastIndex = lastIndex + size < Records.Count-5 ? lastIndex + size : Records.Count - 5;

            var list = Records.ToList().GetRange(firstIndex, lastIndex - firstIndex);*/
            var list = disassemblerDataGridView.GetVisibleRows().Cast<DataGridViewRow>().Select(r=>(DisassembledRecord)r.DataBoundItem).ToList();

            lock (list)
            {
                bool isFixd = false;
                DisassembledRecord prev;

                if (list.First().Refresh(process, disassembler))
                    isFixd = true;
                for (int i = 1; i < list.Count; i++)
                {
                    if (!isFixd && list[i].Refresh(process, disassembler))
                    {
                        isFixd = true;
                        continue;
                    }

                    if (!isFixd) continue;

                    prev = list[i - 1];

                    list[i].Address = prev.Address.Add(prev.Length);
                    list[i].Refresh(process, disassembler);
                }
            }

            disassemblerDataGridView.Invalidate();
            refreshInJob = false;
        }
        public Task RefreshVisibleRecordsAsync(Disassembler disassembler)
        {
            if (refreshInJob || !isFirstFillFinish) return Task.FromResult(0);
            refreshInJob = true;

            return Task.Run(() =>
            {
                /*var visibleRows = disassemblerDataGridView.GetVisibleRows().Cast<DataGridViewRow>();
                var visibleRocords = visibleRows.Select(r=>(DisassembledRecord)r.DataBoundItem);

                if (visibleRows.Count() < 1) return;

                int lastIndex = visibleRows.Last().Index;
                int firstIndex = visibleRows.First().Index;
                firstIndex += firstIndex == 0 ? 1 : 0;

                int size = 10;
                firstIndex = firstIndex > size ? firstIndex - size : 1;
                lastIndex = lastIndex + size < Records.Count - 5 ? lastIndex + size : Records.Count - 5;

                var list = Records.ToList().GetRange(firstIndex, lastIndex - firstIndex);*/
                var list = disassemblerDataGridView.GetVisibleRows().Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem).ToList();


                lock (list)
                {
                    bool isFixd = false;
                    DisassembledRecord prev;

                    if (list.First().Refresh(process, disassembler))
                        isFixd = true;
                    for (int i = 1; i < list.Count; i++)
                    {
                        if (!isFixd && list[i].Refresh(process, disassembler))
                        {
                            isFixd = true;
                            continue;
                        }

                        if (!isFixd) continue;

                        prev = list[i - 1];

                        list[i].Address = prev.Address.Add(prev.Length);
                        list[i].Refresh(process, disassembler);
                    }
                }

                if(!IsDisposed)
                    BeginInvoke(new Action(() => { disassemblerDataGridView.Invalidate(); }));

                refreshInJob = false;
            });
        }

        public void ReadAndLoadAssemble(IntPtr address, int size)
        {
            byte[] bytes = new byte[size];

            bool status = process.ReadRemoteMemoryIntoBuffer(address, ref bytes);

            IntPtr _address = address;
            if (status)
            {
                var disassembleDumps = disassembler.RemoteDisassembleCode(process.IsWow64, _address, bytes);

                 isFirstFillFinish = true;

                SetRecords(disassembleDumps
                    .Distinct(new DistinctDisassembledInstructionComparer())
                    .Select(d => { return new DisassembledRecord(ref d); }));
            }
            else
            {
                List<DisassembledInstruction> disassembleDumps = new List<DisassembledInstruction>();
                for (int i = 0; i < bytes.Length; i++)
                {
                    disassembleDumps.Add(new DisassembledInstruction(_address));
                    _address = _address.Add(1);
                }

                isFirstFillFinish = false;

                SetRecords(disassembleDumps
                    .Distinct(new DistinctDisassembledInstructionComparer())
                    .Select(d => { return new DisassembledRecord(ref d); }));

                Task.Run(() => {
                    var wrapperCloned = wrapper.Clone();
                    wrapperCloned.Connect();
                    var disassem = new Disassembler(wrapperCloned);

                    RefreshRecordsAsync(disassem).Wait();

                    isFirstFillFinish = true;
                    BeginInvoke(new Action(() => { RefreshCompleted?.Invoke(); }));
                    wrapperCloned.Close();
                });
            }
        }

        public void GoToAddress(IntPtr address)
        {
            ReadAndLoadAssemble(address.Sub(READ_SIZE/2),READ_SIZE);

            scroll_to_address = address;

            disassemblerDataGridView.ScrollToDisassembleAddress(address);
            currentAddressLabel.Text = SelectedRecord.address;
        }
        public void GoToAddress(string address_str)
        {
            long address = 0;
            string offsetStr;
            if (address_str.Contains('+'))
            {
                var array = address_str.Split('+');
                var module = process.GetModuleByName(array[0]);
                if (module != null)
                {
                    offsetStr = array[1];
                    var offset = long.Parse(offsetStr, System.Globalization.NumberStyles.HexNumber);
                    address = module.Start.Add((IntPtr)offset).ToInt64();
                }
            }
            else
            {
                var module = process.GetModuleByName(address_str);
                if (module != null)
                    address = module.Start.ToInt64();
            }

            if (address > 0)
                GoToAddress((IntPtr)address);
        }
        private int FindAddressInString(string text, int start_index)
        {
            int last_index = -1;
            string text_lower_case = text.ToLower();
            for (int i = start_index; i < text_lower_case.Length; i++)
            {
                if (text_lower_case[i] == ' ' && last_index == -1) continue;
                if ((text_lower_case[i] >= '0' && text_lower_case[i] <= '9') || (text_lower_case[i] >= 'a' && text_lower_case[i] <= 'f'))
                    last_index = i;
                else
                    break;
            }

            return last_index;
        }
        public void SetRecords(IEnumerable<DisassembledRecord> records)
        {
            bindings.Clear();

            bindings.RaiseListChangedEvents = false;

            foreach (var record in records) bindings.Add(record);

            bindings.RaiseListChangedEvents = true;
            bindings.ResetBindings();
        }
        private void GoToAddressInternal(IntPtr address, IntPtr scroll_to_address)
        {
            ReadAndLoadAssemble(address,READ_SIZE);

            this.scroll_to_address = scroll_to_address;

            disassemblerDataGridView.ScrollToDisassembleAddress(scroll_to_address);
            currentAddressLabel.Text = SelectedRecord.address;
        }
        private string GetAsmDescriptionFromRecord(DisassembledRecord record)
        {
            /* 
             2. get the instraction cmd from first record
             3. search in asm list for the asm cmd ins
                if found display the description else 
                display unknown ins
             */

            string asmCmd = record.Instruction.TrimStart().Split(' ')[0];
            if (asmCmd == null)
                asmCmd = record.Instruction;
            AsmIns asmIns;
            string printToLabel = "Unknown instraction.";
            if (process.IsWow64)
            {
                asmIns = asmIns32.SingleOrDefault(i => i.Name.ToLower().Equals(asmCmd));
                if (asmIns != null)
                    printToLabel = asmIns.Description;
            }
            else
            {
                asmIns = asmIns64.SingleOrDefault(i => i.Name.ToLower().Equals(asmCmd));
                if (asmIns != null)
                    printToLabel = asmIns.Description;
            }

            return printToLabel;
        }
        private void SelectRecordsInternal(IEnumerable<DisassembledRecord> records)
        {
            Contract.Requires(records != null);

            disassemblerDataGridView.ClearSelection();

            foreach (var record in records)
            {
                int index = Records.GetClosestIndex(record.Address);
                if (index < 0) continue;
                disassemblerDataGridView.Rows[index].Selected = true;
            }
        }
        public void SelectRecords(IntPtr startAddress,IntPtr endAddress)
        {
            int startIndex = Records.GetClosestIndex(startAddress);
            if (startIndex < 0) return;
            int endIndex = Records.GetClosestIndex(endAddress);
            if (endIndex < 0) return;

            for (int i = startIndex; i <= endIndex; i++)
            {
                disassemblerDataGridView.Rows[i].Selected = true;
            }
        }
        public void SelectRecords(IEnumerable<DisassembledRecord> records)
        {
            Contract.Requires(records != null);

            records_to_select = records;
            SelectRecordsInternal(records_to_select);
        }


        private void OnRecordDoubleClick(DisassembledRecord record)
        {
            var evt = RecordDoubleClick;
            evt?.Invoke(this, record);
        }

        #region DrawingContext 

        private class DrawingContext
        {
            public string Text { get; set; }
            public int Height { get; set; }
            public Font Font { get; set; }
            public Color Color { get; set; }
            public Color SelectedColor { get; set; }
            public bool IsSelected { get; set; }
            public SolidBrush SolidBrush { get => new SolidBrush(!IsSelected ? Color : SelectedColor); }
            public RectangleF Rect { get; set; }
            public StringFormat StringFormat { get; set; }

            public DrawingContext(ref DrawingContext context)
            {
                Height = context.Height;
                IsSelected = context.IsSelected;
                Text = context.Text.Clone() as string;
                Font = context.Font.Clone() as Font;
                Color = Color.FromArgb(context.Color.ToArgb());
                SelectedColor = Color.FromArgb(context.SelectedColor.ToArgb());
                Rect = new RectangleF(context.Rect.Location, context.Rect.Size);
                StringFormat = context.StringFormat.Clone() as StringFormat;
            }
            public DrawingContext(string text, Font font, Color color, Color selected_color, PointF point, int height, StringFormat stringFormat)
            {
                Text = text;
                Height = height;
                Font = font;
                Color = color;
                SelectedColor = selected_color;
                StringFormat = stringFormat;
            }

            public void Draw(Graphics graphics) => graphics.DrawString(Text, Font, SolidBrush, Rect, StringFormat);
        }
        public static RectangleF CreateTextRectAngle(Graphics graphics, PointF point, float height, string text, Font font)
        {
            SizeF size = graphics.MeasureString(text, font);
            return new RectangleF(point, new SizeF(size.Width, height));
        }

        private enum SplitDrawingContext
        {
            None,
            ByAddress,
            ByBrackets,
            ByOperator,
        }

        private Color _baseColor = Color.Red;
        private Color _baseSelectedColor = Color.Magenta;

        private Color _addressColor = Color.Blue;
        private Color _addressSelectedColor = Color.Yellow;

        private Color _cmdColor = Color.Black;
        private Color _cmdSelectedColor = Color.White;

        private Color _bracketsColor = Color.Black;
        private Color _bracketsSelectedColor = Color.White;

        private List<DrawingContext> SplitTextToDrawingContext(Font font, string text, SplitDrawingContext split)
        {
            switch (split)
            {
                case SplitDrawingContext.ByAddress:
                    return SplitTextToAddress(font, text);
                case SplitDrawingContext.ByBrackets:
                    return SplitTextToBrackets(font, text);
                case SplitDrawingContext.ByOperator:
                    return SplitTextToOperator(font, text);
                default:
                    return TextToDrawingContext(font, text);
            }
        }
        private List<DrawingContext> TextToDrawingContext(Font font, string text)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DrawingContext context;
            List<DrawingContext> contexts = null;

            contexts = new List<DrawingContext>();

            context = new DrawingContext(
                text,
                font,
                _baseColor,
                _baseSelectedColor,
                new PointF(0, 0),
                0,
                sf
            );

            contexts.Add(context);

            return contexts;
        }
        private List<DrawingContext> SplitTextToAddress(Font font, string text)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DrawingContext beforeContext;
            DrawingContext middleContext;
            DrawingContext afterContext;
            List<DrawingContext> contexts = null;

            int step = 2;
            int isContainNumber = text.IndexOf("0x");
            int tryes = 10;
            if (isContainNumber == -1)
            {
                step = 0;
                while (isContainNumber == -1 && tryes > 0)
                {
                    isContainNumber = text.IndexOf((--tryes).ToString());
                }
            }

            if (isContainNumber != -1)
            {
                contexts = new List<DrawingContext>();
                int lastIndex = FindAddressInString(text, isContainNumber + step);
                string beforeAddress = text.SmSubstring(0, isContainNumber - 1).Trim();
                string address = text.SmSubstring(isContainNumber, lastIndex).Trim();
                string afterAddress = text.SmSubstring(lastIndex + 1, text.Length - 1).Trim();

                beforeContext = TextToDrawingContext(font, beforeAddress)[0];

                Color bc = _addressColor;
                Color sc = _addressSelectedColor;
                if (long.TryParse(address.SmSubstring(2), System.Globalization.NumberStyles.HexNumber, null, out var temp))
                {
                    var module = process.GetModuleToPointer((IntPtr)temp);
                    if (module != null)
                    {
                        bc = Color.SeaGreen;
                        sc = Color.LimeGreen;
                        address = $"{module.Name}+{((IntPtr)temp).Sub(module.Start).ToString("X")}";
                    }
                }
                middleContext = new DrawingContext(
                    address,
                    font,
                    bc,
                    sc,
                    new PointF(beforeContext.Rect.Location.X + beforeContext.Rect.Width, 0),
                    0,
                    sf
                );

                afterContext = TextToDrawingContext(font, afterAddress)[0];

                contexts.Add(beforeContext);
                contexts.Add(middleContext);
                contexts.Add(afterContext);
            }

            return contexts;
        }
        private List<DrawingContext> SplitTextToOperator(Font font, string text)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DrawingContext beforeContext;
            DrawingContext middleContext;
            DrawingContext afterContext;
            List<DrawingContext> contexts = null;

            int isContainNumber = text.IndexOf("+");
            if (isContainNumber == -1) isContainNumber = text.IndexOf("-");
            if (isContainNumber == -1) isContainNumber = text.IndexOf("*");
            if (isContainNumber == -1) isContainNumber = text.IndexOf("/");

            if (isContainNumber != -1)
            {
                contexts = new List<DrawingContext>();
                string beforeOperator = text.SmSubstring(0, isContainNumber - 1).Trim();
                string _operator = text.SmSubstring(isContainNumber, isContainNumber).Trim();
                string afterOperator = text.SmSubstring(isContainNumber + 1, text.Length - 1).Trim();

                beforeContext = TextToDrawingContext(font, beforeOperator)[0];

                middleContext = new DrawingContext(
                    _operator,
                    font,
                    _bracketsColor,
                    _bracketsSelectedColor,
                    new PointF(beforeContext.Rect.Location.X + beforeContext.Rect.Width, 0),
                    0,
                    sf
                );

                afterContext = TextToDrawingContext(font, afterOperator)[0];

                contexts.Add(beforeContext);
                contexts.Add(middleContext);
                contexts.Add(afterContext);
            }

            return contexts;
        }
        private List<DrawingContext> SplitTextToBrackets(Font font, string text)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DrawingContext beforeContext;
            DrawingContext middleContext;
            DrawingContext afterContext;
            List<DrawingContext> contexts = null;

            int f = text.SmIndexOf('[');
            int l = text.SmIndexOf(']');
            if (f != -1 && l != -1 && l < text.Length)
            {
                contexts = new List<DrawingContext>();

                string beforeBrackets = text.SmSubstring(0, f - 1).Trim();
                string middleBrackets = text.SmSubstring(f + 1, l - 1).Trim();
                string afterBrackets = text.SmSubstring(l + 1).Trim();

                beforeContext = TextToDrawingContext(font, beforeBrackets)[0];

                var splitOpenContext = new DrawingContext(
                    "[",
                    font,
                    _bracketsColor,
                    _bracketsSelectedColor,
                    new PointF(beforeContext.Rect.Location.X + beforeContext.Rect.Width, beforeContext.Rect.Location.Y),
                    0,
                    sf
                );

                middleContext = TextToDrawingContext(font, middleBrackets)[0];

                var splitCloseContext = new DrawingContext(
                    "]",
                    font,
                    _bracketsColor,
                    _bracketsSelectedColor,
                    new PointF(middleContext.Rect.Location.X + middleContext.Rect.Width, middleContext.Rect.Location.Y),
                    0,
                    sf
                );

                afterContext = TextToDrawingContext(font, afterBrackets)[0];

                contexts.Add(beforeContext);
                contexts.Add(splitOpenContext);
                contexts.Add(middleContext);
                contexts.Add(splitCloseContext);
                contexts.Add(afterContext);
            }

            return contexts;
        }
        private List<DrawingContext> SplitContext(Font font, List<DrawingContext> contexts, SplitDrawingContext split)
        {
            List<DrawingContext> tempContexts = null;
            List<DrawingContext> tempContexts2 = null;
            List<DrawingContext> tempContexts3 = null;
            List<DrawingContext> tempContexts4 = null;

            List<DrawingContext> _contexts = contexts.Select(c => new DrawingContext(ref c)).ToList();

            tempContexts = SplitTextToDrawingContext(font, contexts[0].Text, split);
            tempContexts2 = SplitTextToDrawingContext(font, contexts[2].Text, split);

            if (tempContexts != null)
            {
                _contexts.RemoveAt(0);
                _contexts.InsertRange(0, tempContexts);
                tempContexts3 = SplitContext(font, tempContexts, split);
            }

            if (tempContexts2 != null)
            {
                _contexts.InsertRange(_contexts.Count - 1, tempContexts2);
                _contexts.RemoveAt(_contexts.Count - 1);
                tempContexts4 = SplitContext(font, tempContexts2, split);
            }

            if (tempContexts3 != null && tempContexts4 != null)
            {
                _contexts = contexts.Select(c => new DrawingContext(ref c)).ToList();

                _contexts.RemoveAt(0);
                _contexts.InsertRange(0, tempContexts3);

                _contexts.InsertRange(_contexts.Count - 1, tempContexts4);
                _contexts.RemoveAt(_contexts.Count - 1);
            }
            else if (tempContexts3 != null)
            {
                _contexts.RemoveAt(0);
                _contexts.RemoveAt(0);
                _contexts.RemoveAt(0);
                _contexts.InsertRange(0, tempContexts3);
            }
            else if (tempContexts4 != null)
            {
                _contexts.RemoveAt(_contexts.Count - 1);
                _contexts.RemoveAt(_contexts.Count - 1);
                _contexts.InsertRange(_contexts.Count - 1, tempContexts4);
                _contexts.RemoveAt(_contexts.Count - 1);
            }
            return _contexts;
        }
        #endregion

        #endregion

        #region Events
        private void disassemblerDataGridView_RefreshCompleted()
        {
            if (scroll_to_address == IntPtr.Zero && (records_to_select == null || records_to_select.Count() < 1)) return;

            if (scroll_to_address != IntPtr.Zero)
            {
                disassemblerDataGridView.ScrollToDisassembleAddress(scroll_to_address);
                currentAddressLabel.Text = SelectedRecord.address;
                scroll_to_address = IntPtr.Zero;
            }

            if (records_to_select != null && records_to_select.Count() > 0)
            {
                SelectRecordsInternal(records_to_select);
                records_to_select = null;
            }
        }
        private void disassemblerDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var record = SelectedRecords.FirstOrDefault();
            if (record == null) return;

            currentCmdDescreptionLabel.Text = GetAsmDescriptionFromRecord(record);
        }

        private void disassemblerDataGrid_Scroll(object sender, ScrollEventArgs e)
        {
            var visibleRows = disassemblerDataGridView.GetVisibleRows();
            var visibleRecords = visibleRows.Select(r => (DisassembledRecord)r.DataBoundItem);

            var rfRecord = bindings.First().Clone();
            var rlRecord = bindings.Last().Clone();


            var fVRecord = visibleRecords.First().Clone();
            var lVRecord = visibleRecords.Last().Clone();


            currentAddressLabel.Text = visibleRecords.First().address;

            if (fVRecord.Address.Equals(rfRecord.Address))
            {
                GoToAddressInternal(lVRecord.Address.Sub(READ_SIZE-200), lVRecord.Address);
            }
            else if (lVRecord.Address.Equals(rlRecord.Address))
            {
                GoToAddressInternal(fVRecord.Address.Sub(200), fVRecord.Address);
            }
            else
            {
                //Task.Run(() => RefreshVisibleRecords());
            }
        }
        private void disassemblerDataGridCell_Painting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != 2) return;

            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

            DataGridViewCell cell = disassemblerDataGridView[e.ColumnIndex, e.RowIndex];
            string content = cell.Value?.ToString().ToLower().Trim();

            if (string.IsNullOrEmpty(content)) return;//to remove

            string cmd = string.Empty;
            var contentArray = content.Split(' ');
            if (content.Contains("lock"))
            {
                cmd += $"{contentArray[0]} ";
                cmd += $"{contentArray[1]} ";
            } else cmd += $"{contentArray[0]} ";

            int lastLen = content.Length;
            content = content.Replace(cmd, string.Empty);
            if(lastLen == content.Length)

                content = content.Replace(cmd.Trim(), string.Empty);
            cmd += "\t";

            List<DrawingContext> contextList = new List<DrawingContext>();

            Font font = new Font(e.CellStyle.Font.FontFamily, 10f, FontStyle.Regular);
            Font fontBold = new Font(e.CellStyle.Font.FontFamily, 10f, FontStyle.Bold);

            var cmdContext = SplitTextToDrawingContext(fontBold, cmd, SplitDrawingContext.None);
            cmdContext[0].Rect = CreateTextRectAngle(e.Graphics,
                    new PointF(e.CellBounds.Location.X, e.CellBounds.Y),
                    e.CellBounds.Height, cmdContext[0].Text, cmdContext[0].Font);
            cmdContext[0].Color = Color.Black;
            cmdContext[0].SelectedColor = Color.White;

            contextList.Add(cmdContext[0]);


            var contexts = SplitTextToDrawingContext(font, content, SplitDrawingContext.ByBrackets);
            if (contexts == null)
            {
                contexts = SplitTextToDrawingContext(font, content, SplitDrawingContext.ByAddress);
                if (contexts == null)
                    contexts = SplitTextToDrawingContext(font, content, SplitDrawingContext.None);
            }
            else
            {
                List<DrawingContext> tempContexts = null;
                // search Operators
                var middleContexts = SplitTextToDrawingContext(font, contexts[2].Text, SplitDrawingContext.ByOperator);
                if (middleContexts != null)
                {
                    tempContexts = null;
                    middleContexts = SplitContext(font, middleContexts, SplitDrawingContext.ByOperator);
                    for (int i = 0; i < middleContexts.Count - 2;)
                    {
                        tempContexts = new List<DrawingContext>();
                        tempContexts.Add(middleContexts[i]);
                        tempContexts.Add(middleContexts[i + 1]);
                        tempContexts.Add(middleContexts[i + 2]);

                        var tempAddress = SplitContext(font, tempContexts, SplitDrawingContext.ByAddress);

                        middleContexts.RemoveAt(i);
                        middleContexts.RemoveAt(i);
                        middleContexts.RemoveAt(i);

                        middleContexts.InsertRange(i, tempAddress);

                        i += tempAddress.Count - 1;
                    }

                    tempContexts = new List<DrawingContext>();
                    tempContexts.Add(contexts[contexts.Count - 1]);
                    tempContexts.Add(TextToDrawingContext(font, "")[0]);
                    tempContexts.Add(TextToDrawingContext(font, "")[0]);
                    tempContexts = SplitContext(font, tempContexts, SplitDrawingContext.ByAddress);

                    contexts.RemoveAt(2);
                    contexts.InsertRange(2, middleContexts);

                    contexts.InsertRange(contexts.Count - 1, tempContexts);
                    contexts.RemoveAt(contexts.Count - 1);
                }
                else
                {
                    middleContexts = SplitTextToDrawingContext(font, contexts[2].Text, SplitDrawingContext.ByAddress);
                    if (middleContexts != null)
                    {
                        tempContexts = null;
                        middleContexts = SplitContext(font, middleContexts, SplitDrawingContext.ByOperator);
                        for (int i = 0; i < middleContexts.Count - 2;)
                        {
                            tempContexts = new List<DrawingContext>();
                            tempContexts.Add(middleContexts[i]);
                            tempContexts.Add(middleContexts[i + 1]);
                            tempContexts.Add(middleContexts[i + 2]);

                            var tempAddress = SplitContext(font, tempContexts, SplitDrawingContext.ByAddress);

                            middleContexts.RemoveAt(i);
                            middleContexts.RemoveAt(i);
                            middleContexts.RemoveAt(i);

                            middleContexts.InsertRange(i, tempAddress);

                            i += tempAddress.Count - 1;
                        }

                        tempContexts = new List<DrawingContext>();
                        tempContexts.Add(contexts[contexts.Count - 1]);
                        tempContexts.Add(TextToDrawingContext(font, "")[0]);
                        tempContexts.Add(TextToDrawingContext(font, "")[0]);
                        tempContexts = SplitContext(font, tempContexts, SplitDrawingContext.ByAddress);

                        contexts.RemoveAt(2);
                        contexts.InsertRange(2, middleContexts);

                        contexts.InsertRange(contexts.Count - 1, tempContexts);
                        contexts.RemoveAt(contexts.Count - 1);
                    }
                    else
                    {

                        tempContexts = new List<DrawingContext>();
                        tempContexts.Add(contexts[contexts.Count - 1]);
                        tempContexts.Add(TextToDrawingContext(font, "")[0]);
                        tempContexts.Add(TextToDrawingContext(font, "")[0]);
                        tempContexts = SplitContext(font, tempContexts, SplitDrawingContext.ByAddress);

                        contexts.InsertRange(contexts.Count - 1, tempContexts);
                        contexts.RemoveAt(contexts.Count - 1);
                    }
                }
            }

            contexts[0].Rect = CreateTextRectAngle(e.Graphics,
                    new PointF(cmdContext[0].Rect.Location.X + cmdContext[0].Rect.Width - 4, cmdContext[0].Rect.Location.Y),
                    e.CellBounds.Height, contexts[0].Text, contexts[0].Font);

            contextList.AddRange(contexts);


            contextList = contextList.Where(c => !string.IsNullOrWhiteSpace(c.Text)).Select(c => { c.IsSelected = cell.Selected; return new DrawingContext(ref c); }).ToList();

            for (int i = 1; i < contextList.Count; i++)
            {
                var context = contextList[i];
                var prevContext = contextList[i - 1];

                context.Rect = CreateTextRectAngle(e.Graphics,
                    new PointF(prevContext.Rect.Location.X + prevContext.Rect.Width - 2, prevContext.Rect.Location.Y),
                    e.CellBounds.Height, context.Text, context.Font);
            }

            foreach (var context in contextList)
                context.Draw(e.Graphics);

            e.Handled = true;
        }
        private void disassemblerDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = disassemblerDataGridView.HitTest(e.X, e.Y);
                if (hti.RowIndex < 0) return;
                var row = disassemblerDataGridView.Rows[hti.RowIndex];
                if (!row.Selected && !(ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control))
                    disassemblerDataGridView.ClearSelection();
                row.Selected = true;
            }
        }
        private void disassemblerDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OnRecordDoubleClick((DisassembledRecord)disassemblerDataGridView.Rows[e.RowIndex].DataBoundItem);
        }
        private void disassemblerDataGridView_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = ContextMenuStrip;
        }
        #endregion
    }

    public class DistinctDisassembledRecordComparer : IEqualityComparer<DisassembledRecord>
    {
        public bool Equals(DisassembledRecord x, DisassembledRecord y)
        {
            return x.Address.Equals(y.Address);
        }

        public int GetHashCode(DisassembledRecord obj)
        {
            return obj.Address.GetHashCode();
        }
    }
    public class DisassembledRecord
    {
        public string address
        {
            get
            {
                if(string.IsNullOrEmpty(ModuleName))
                    return Address.ToString(Program.AddressHexFormat);
                else
                    return $"{ModuleName}+{Address.Sub(ModuleAddress).ToString("X")}";
            }
        }

        string GetInsOrderd()
        {
            string instracionsBytes = string.Empty;
            List<string> addresses = Instruction.SmGetAddressList(Program.AddressHexFormat);

            instracionsBytes = string.Join("", Data.Take(Length).Reverse().Select(b => $"{b:X2}"));

            int lastLen = 0;
            for (int i = 0; i < addresses.Count; i++)
            {
                lastLen = instracionsBytes.Length;
                instracionsBytes = instracionsBytes.Replace(addresses[i], string.Empty);
                if (lastLen == instracionsBytes.Length)
                {
                    addresses.RemoveAt(i);
                    i--;
                }
            }

            string newIns = string.Empty;
            for (int i = instracionsBytes.Length - 1; i > 0; i -= 2) newIns += $"{instracionsBytes[i - 1]}{instracionsBytes[i]} ";

            foreach (var address in addresses) newIns += $"{address.SmReverse().SmEndianReverse()} ";

            return newIns;
        }
        public string bytes
        {
            get
            {

                string instracionsBytes = GetInsOrderd();

                return instracionsBytes;
            }
        }
        public string opcode { get => Instruction; }
        public string ModuleName { get; set; }
        public IntPtr ModuleAddress { get; set; }

        public IntPtr Offset { get; set; }
        public IntPtr Address { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }
        public string Instruction { get; set; }

        public bool IsValid => Length > 0;

        public DisassembledRecord()
        {
        }
        public DisassembledRecord(ref DisassembledRecord data)
        {
            Address = data.Address;
            Length = data.Length;
            Data = data.Data;
            Instruction = data.Instruction;

            // Ex
            ModuleName = data.ModuleName;
            ModuleAddress = data.ModuleAddress;
        }
        public DisassembledRecord(ref DisassembledInstruction data)
        {
            Address = data.Address;
            Length = data.Length;
            Data = data.Data;
            Instruction = data.Instruction;
        }
        public DisassembledRecord(ref DisassembledInstruction data, string module_name, IntPtr module_address)
        {
            Address = data.Address;
            Length = data.Length;
            Data = data.Data;
            Instruction = data.Instruction;
            ModuleName = module_name;
            ModuleAddress = module_address;
        }

        public override string ToString() => $"{Address.ToString(Program.AddressHexFormat)} - {Instruction}";

        public bool Refresh(IProcessReader reader, Disassembler disassembler)
        {
            return RefreshAsync(reader,disassembler).Result;
        }
        public Task<bool> RefreshAsync(IProcessReader reader, Disassembler disassembler)
        {
            return Task.Run(() =>
            {
                byte[] data = new byte[Data.Length];
                bool status = true;
                if (reader.ReadRemoteMemoryIntoBuffer(Address, ref data))
                {
                    var instructions = disassembler.RemoteDisassembleCode(reader, Address, Data.Length);
                    if (instructions == null) return false;

                    Offset = Address = instructions[0].Address;
                    Length = instructions[0].Length;
                    Array.Copy(instructions[0].Data, Data, Data.Length);
                    Instruction = instructions[0].Instruction;

                    var module = reader.GetModuleToPointer(Address);
                    if (module != null)
                    {
                        ModuleName = module.Name;
                        Offset = Address.Sub(module.Start);
                        ModuleAddress = module.Start;
                    }

                }
                else
                {
                    status = false;
                    Length = 1;
                    Data = new byte[15];
                    Instruction = "???";
                    ModuleName = string.Empty;
                    ModuleAddress = IntPtr.Zero;
                }
                return status;
            });
        }

        public DisassembledRecord Clone() => new DisassembledRecord()
        {
            Address = Address,
            Length = Length,
            Data = Data.Clone() as byte[],
            Instruction = Instruction.Clone() as string,

            // Ex
            ModuleName = !string.IsNullOrEmpty(ModuleName) ? ModuleName.Clone() as string : null,
            ModuleAddress = ModuleAddress
        };

        public DisassembledInstruction ToInstraction() => new DisassembledInstruction(Address, Length, Data, Instruction);
    }

    public class DistinctAsmIns : IEqualityComparer<AsmIns>
    {
        public bool Equals(AsmIns x, AsmIns y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(AsmIns obj)
        {
            return obj.Name.GetHashCode();
        }
    }
    public class AsmIns
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
