﻿using SmScanner.Core.Enums;
using System;

namespace SmScanner.Core.Modules
{
	public enum SettingState
	{
		Yes,
		No,
		Indeterminate
	}

	public class ScanSettings
	{
		public IntPtr StartAddress { get; set; } = IntPtr.Zero;
		public IntPtr StopAddress { get; set; } = Program.RemoteProcess.IsWow64 ? (IntPtr)int.MaxValue : (IntPtr)long.MaxValue;
		public SettingState ScanWritableMemory { get; set; } = SettingState.Yes;
		public SettingState ScanExecutableMemory { get; set; } = SettingState.Indeterminate;
		public SettingState ScanCopyOnWriteMemory { get; set; } = SettingState.No;
		public bool ScanPrivateMemory { get; set; } = true;
		public bool ScanImageMemory { get; set; } = true;
		public bool ScanMappedMemory { get; set; } = false;
		public bool EnableFastScan { get; set; } = true;
		public int FastScanAlignment { get; set; } = 4;
		public ScanValueType ValueType { get; set; } = ScanValueType.Integer;

		public static ScanSettings Default => new ScanSettings();
	}
}
