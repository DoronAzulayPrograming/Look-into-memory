using SmScanner.Core.Enums;
using SmScanner.Core.Extensions;
using SmScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmScanner.Core.Modules
{
	public delegate void RemoteProcessEvent(RemoteProcess sender);
	public class RemoteProcess : IRemoteProcess
	{
		private readonly Dictionary<IntPtr, string> rttiCache = new Dictionary<IntPtr, string>();
		public bool IsWow64 { get; private set; } = true;
        private IntPtr handle;
		private Smdkd.SmProcessInfo process;
		private readonly object processSync = new object();
		private readonly List<Smdkd.SmModule> modules = new List<Smdkd.SmModule>();
		private readonly List<Smdkd.SmSection> sections = new List<Smdkd.SmSection>();
		public bool IsValid => process != null && Smdkd.IsProcessValid(handle);
		public Smdkd.SmProcessInfo UnderlayingProcess => process;
		/// <summary>Event which gets invoked when a process was opened.</summary>
		public event RemoteProcessEvent ProcessAttached;

		/// <summary>Event which gets invoked before a process gets closed.</summary>
		public event RemoteProcessEvent ProcessClosing;

		/// <summary>Event which gets invoked after a process was closed.</summary>
		public event RemoteProcessEvent ProcessClosed;

		/// <summary>Gets a copy of the current modules list. This list may change if the remote process (un)loads a module.</summary>
		public IEnumerable<Smdkd.SmModule> Modules
		{
			get
			{
				lock (modules)
				{
					return new List<Smdkd.SmModule>(modules);
				}
			}
		}

		/// <summary>Gets a copy of the current sections list. This list may change if the remote process (un)loads a section.</summary>
		public IEnumerable<Smdkd.SmSection> Sections
		{
			get
			{
				lock (sections)
				{
					return new List<Smdkd.SmSection>(sections);
				}
			}
		}


		/// <summary>Opens the given process to gather informations from.</summary>
		/// <param name="info">The process information.</param>
		public void Open(Smdkd.SmProcessInfo info)
		{
			Contract.Requires(info != null);

			if (process?.Id != info.Id)
			{
				lock (processSync)
				{
					Close();

					process = info;
					IsWow64 = info.IsWow64;

					Smdkd.OpenRemoteProcess(process.Id, out handle);
				}

				ProcessAttached?.Invoke(this);
			}
		}
		/// <summary>Closes the underlaying process. If the debugger is attached, it will automaticly detached.</summary>
		public void Close()
		{
			if (process != null)
			{
				ProcessClosing?.Invoke(this);

				lock (processSync)
				{

					handle = IntPtr.Zero;

					process = null;

					IsWow64 = true;
				}

				ProcessClosed?.Invoke(this);
			}
		}
		public void Dispose()
		{
			Close();
		}
		#region ReadMemory

		public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer)
		{
			Contract.Requires(buffer != null);
			Contract.Ensures(Contract.ValueAtReturn(out buffer) != null);

			return ReadRemoteMemoryIntoBuffer(address, ref buffer, 0, buffer.Length);
		}

		public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer, int offset, int length)
		{
			Contract.Requires(buffer != null);
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);
			Contract.Requires(offset + length <= buffer.Length);
			Contract.Ensures(Contract.ValueAtReturn(out buffer) != null);
			Contract.EndContractBlock();

			if (!IsValid)
			{
				Close();

				buffer.FillWithZero();

				return false;
			}

			return Smdkd.ReadMemBlock(process.Id, address, (IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, buffer, length);
		}

		public byte[] ReadRemoteMemory(IntPtr address, int size)
		{
			Contract.Requires(size >= 0);
			Contract.Ensures(Contract.Result<byte[]>() != null);

			var data = new byte[size];
			ReadRemoteMemoryIntoBuffer(address, ref data);
			return data;
		}



		public string ReadRemoteRuntimeTypeInformation(IntPtr address)
		{
			if (address.MayBeValid())
			{
				if (!rttiCache.TryGetValue(address, out var rtti))
				{
					var objectLocatorPtr = this.ReadRemoteIntPtr(address - IntPtr.Size);
					if (objectLocatorPtr.MayBeValid())
					{

#if RECLASSNET64
						rtti = ReadRemoteRuntimeTypeInformation64(objectLocatorPtr);
#else
						rtti = ReadRemoteRuntimeTypeInformation32(objectLocatorPtr);
#endif

						rttiCache[address] = rtti;
					}
				}
				return rtti;
			}

			return null;
		}

		private string ReadRemoteRuntimeTypeInformation32(IntPtr address)
		{
			var classHierarchyDescriptorPtr = this.ReadRemoteIntPtr(address + 0x10);
			if (classHierarchyDescriptorPtr.MayBeValid())
			{
				var baseClassCount = this.ReadRemoteInt32(classHierarchyDescriptorPtr + 8);
				if (baseClassCount > 0 && baseClassCount < 25)
				{
					var baseClassArrayPtr = this.ReadRemoteIntPtr(classHierarchyDescriptorPtr + 0xC);
					if (baseClassArrayPtr.MayBeValid())
					{
						var sb = new StringBuilder();
						for (var i = 0; i < baseClassCount; ++i)
						{
							var baseClassDescriptorPtr = this.ReadRemoteIntPtr(baseClassArrayPtr + (4 * i));
							if (baseClassDescriptorPtr.MayBeValid())
							{
								var typeDescriptorPtr = this.ReadRemoteIntPtr(baseClassDescriptorPtr);
								if (typeDescriptorPtr.MayBeValid())
								{
									var name = this.ReadRemoteStringUntilFirstNullCharacter(typeDescriptorPtr + 0x0C, Encoding.UTF8, 60);
									if (name.EndsWith("@@"))
									{
										name = WinApi.UndecorateSymbolName("?" + name);
									}

									sb.Append(name);
									sb.Append(" : ");

									continue;
								}
							}

							break;
						}

						if (sb.Length != 0)
						{
							sb.Length -= 3;

							return sb.ToString();
						}
					}
				}
			}

			return null;
		}

		private string ReadRemoteRuntimeTypeInformation64(IntPtr address)
		{
			int baseOffset = this.ReadRemoteInt32(address + 0x14);
			if (baseOffset != 0)
			{
				var baseAddress = address - baseOffset;

				var classHierarchyDescriptorOffset = this.ReadRemoteInt32(address + 0x10);
				if (classHierarchyDescriptorOffset != 0)
				{
					var classHierarchyDescriptorPtr = baseAddress + classHierarchyDescriptorOffset;

					var baseClassCount = this.ReadRemoteInt32(classHierarchyDescriptorPtr + 0x08);
					if (baseClassCount > 0 && baseClassCount < 25)
					{
						var baseClassArrayOffset = this.ReadRemoteInt32(classHierarchyDescriptorPtr + 0x0C);
						if (baseClassArrayOffset != 0)
						{
							var baseClassArrayPtr = baseAddress + baseClassArrayOffset;

							var sb = new StringBuilder();
							for (var i = 0; i < baseClassCount; ++i)
							{
								var baseClassDescriptorOffset = this.ReadRemoteInt32(baseClassArrayPtr + (4 * i));
								if (baseClassDescriptorOffset != 0)
								{
									var baseClassDescriptorPtr = baseAddress + baseClassDescriptorOffset;

									var typeDescriptorOffset = this.ReadRemoteInt32(baseClassDescriptorPtr);
									if (typeDescriptorOffset != 0)
									{
										var typeDescriptorPtr = baseAddress + typeDescriptorOffset;

										var name = this.ReadRemoteStringUntilFirstNullCharacter(typeDescriptorPtr + 0x14, Encoding.UTF8, 60);
										if (string.IsNullOrEmpty(name))
										{
											break;
										}

										if (name.EndsWith("@@"))
										{
											name = WinApi.UndecorateSymbolName("?" + name);
										}

										sb.Append(name);
										sb.Append(" : ");

										continue;
									}
								}

								break;
							}

							if (sb.Length != 0)
							{
								sb.Length -= 3;

								return sb.ToString();
							}
						}
					}
				}
			}

			return null;
		}
		#endregion
		#region WriteMemory

		public bool WriteRemoteMemory(IntPtr address, byte[] data)
		{
			Contract.Requires(data != null);

			if (!IsValid)
			{
				return false;
			}

			return Smdkd.WriteMemBlock((IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, data, process.Id, address, data.Length);
		}
		public bool WriteRemoteMemory(IntPtr address, char[] data)
		{
			Contract.Requires(data != null);

			if (!IsValid)
			{
				return false;
			}

			return Smdkd.WriteMemBlock((IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, data, process.Id, address, data.Length);
		}

		#endregion

		public Smdkd.SmSection GetSectionToPointer(IntPtr address)
		{
			lock (sections)
			{
				var index = sections.BinarySearch(s => address.CompareToRange(s.Start, s.End));
				return index < 0 ? null : sections[index];
			}
		}
		public Smdkd.SmModule GetModuleToPointer(IntPtr address)
		{
			lock (modules)
			{
				var index = modules.BinarySearch(m => address.CompareToRange(m.Start, m.End));
				return index < 0 ? null : modules[index];
			}
		}
		public Smdkd.SmModule GetModuleByName(string name)
		{
			lock (modules)
			{
				return modules
					.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			}
		}


		public bool EnumerateRemoteSectionsAndModules(out List<Smdkd.SmSection> _sections, out List<Smdkd.SmModule> _modules)
		{
			if (!IsValid)
			{
				_sections = null;
				_modules = null;

				return false;
			}

			_sections = new List<Smdkd.SmSection>();
			_modules = new List<Smdkd.SmModule>();

			Smdkd.EnumerateRemoteSectionsAndModules(process.Id, _sections.Add, _modules.Add);

			return true;
		}

		/// <summary>Updates the process informations.</summary>
		public void UpdateProcessInformations()
		{
			UpdateProcessInformationsAsync().Wait();
		}

		/// <summary>Updates the process informations asynchronous.</summary>
		/// <returns>The Task.</returns>
		public Task UpdateProcessInformationsAsync()
		{
			Contract.Ensures(Contract.Result<Task>() != null);

            if (!IsValid)
			{
				lock (modules)
				{
					modules.Clear();
				}
				lock (sections)
				{
					sections.Clear();
				}

				// TODO: Mono doesn't support Task.CompletedTask at the moment.
				//return Task.CompletedTask;
				return Task.FromResult(true);
            }


            return Task.Run(() =>
            {
				EnumerateRemoteSectionsAndModules(out var newSections, out var newModules);
				newModules.Sort((m1, m2) => m1.Start.CompareTo(m2.Start));
				newSections.Sort((s1, s2) => s1.Start.CompareTo(s2.Start));

				lock (modules)
				{
					modules.Clear();
					modules.AddRange(newModules);
				}
				lock (sections)
				{
					sections.Clear();
					sections.AddRange(newSections);
				}
			});
        }


		public void ControlRemoteProcess(ControlRemoteProcessAction action)
		{
			if (!IsValid)
			{
				return;
			}

			if (action == ControlRemoteProcessAction.Resume)
				Smdkd.ResumeRemoteProcess(handle);
			else if(action == ControlRemoteProcessAction.Suspend)
				Smdkd.SuspendRemoteProcess(handle);
            else
            {
				Smdkd.TerminateRemoteProcess(handle, Smdkd.TerminationMethod.TerminateEx);
				Close();
			}
		}
	}
}
