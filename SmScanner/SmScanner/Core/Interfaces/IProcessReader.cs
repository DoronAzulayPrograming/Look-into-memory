using System;
using System.Collections.Generic;

namespace SmScanner.Core.Interfaces
{
	public interface IProcessReader : IRemoteMemoryReader
	{

		Smdkd.SmSection GetSectionToPointer(IntPtr address);

		Smdkd.SmModule GetModuleToPointer(IntPtr address);

		Smdkd.SmModule GetModuleByName(string name);

		bool EnumerateRemoteSectionsAndModules(out List<Smdkd.SmSection> sections, out List<Smdkd.SmModule> modules);
	}
}
