using SmScanner.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SmScanner.Core.Extensions
{
	public static class DataGridViewExtension
	{
		public static IEnumerable<DataGridViewRow> GetVisibleRows(this DataGridView dgv)
		{
			var visibleRowsCount = dgv.DisplayedRowCount(true);
			var firstVisibleRowIndex = dgv.FirstDisplayedCell?.RowIndex ?? 0;
			var lastVisibleRowIndex = firstVisibleRowIndex + visibleRowsCount - 1;
			for (var i = firstVisibleRowIndex; i <= lastVisibleRowIndex; i++)
			{
				yield return dgv.Rows[i];
			}
		}

        public static void ScrollToDumpAddress(this DataGridView dataGrid, IntPtr address)
        {
            IntPtr temp_address = IntPtr.Zero;
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (HexDumpRecord)r.DataBoundItem);
            
            int index = records.GetClosestIndex(address);

            if (index < 0) return;

            dataGrid.ClearSelection();

            foreach (DataGridViewCell cell in dataGrid.Rows[index].Cells) cell.Selected = true;

            dataGrid.FirstDisplayedScrollingRowIndex = index;
        }
        public static void ScrollToDisassembleAddress(this DataGridView dataGrid, IntPtr address)
        {
            IntPtr temp_address = IntPtr.Zero;
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem);

            int index = records.GetClosestIndex(address, lookUp: false);
            if (index == -1) return;

            dataGrid.ClearSelection();
            dataGrid.Rows[index].Selected = true;
            dataGrid.FirstDisplayedScrollingRowIndex = index;
        }
        public static void ScrollToDisassembleAddress(this DataGridView dataGrid, IntPtr address, ScrollEventArgs e)
        {
            IntPtr temp_address = IntPtr.Zero;
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem);

            int index = records.GetClosestIndex(address, lookUp: false);
            if (index == -1) return;

            dataGrid.ClearSelection();
            e.NewValue = index;
            dataGrid.Rows[index].Selected = true;
        }
        public static void ScrollTo(this DataGridView dataGrid, HexDumpRecord record)
        {
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (HexDumpRecord)r.DataBoundItem);
            int index = records.GetClosestIndex(record.Address);

            if (index < 0) return;

            dataGrid.ClearSelection();

            foreach (DataGridViewCell cell in dataGrid.Rows[index].Cells) cell.Selected = true;

            dataGrid.FirstDisplayedScrollingRowIndex = index;
        }
        public static void ScrollTo(this DataGridView dataGrid, DisassembledRecord record)
        {
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem);
            int index = records.FindIndex(r => r.Address.Equals(record.Address));

            if (index < 0) return;

            dataGrid.ClearSelection();
            dataGrid.Rows[index].Selected = true;
            dataGrid.FirstDisplayedScrollingRowIndex = index;
        }
        public static void ScrollTo(this DataGridView dataGrid, DisassembledRecord record, ScrollEventArgs e)
        {
            var records = dataGrid.Rows.Cast<DataGridViewRow>().Select(r => (DisassembledRecord)r.DataBoundItem);
            int index = records.FindIndex(r => r.Address.Equals(record.Address));

            if (index < 0) return;

            dataGrid.ClearSelection();
            e.NewValue = index;
            dataGrid.Rows[index].Selected = true;
        }
    }
}
