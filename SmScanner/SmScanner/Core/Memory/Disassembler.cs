using SmScanner.Core.Extensions;
using SmScanner.Core.Interfaces;
using SmScanner.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SmScanner.Core.Memory
{
	public class Disassembler
	{
		readonly IDisassemblerWrapper disassemblerWrapper;
        public Disassembler(IDisassemblerWrapper disassemblerWrapper)
        {
			this.disassemblerWrapper = disassemblerWrapper;
		}
        // The maximum number of bytes of a x86-64 instruction.
        public const int MaximumInstructionLength = 15;

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code in bytes.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(bool isWow64, IntPtr address, byte[] buffer)
		{
			Contract.Requires(buffer != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			if (isWow64)
				return disassemblerWrapper.DisassembleCode(buffer, address, -1);
			else
				return DisassembleCode(buffer, address, -1);
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code in bytes.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(IRemoteMemoryReader process, IntPtr address, int length)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			return RemoteDisassembleCode(process, address, length, -1);
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code in bytes.</param>
		/// <param name="maxInstructions">The maximum number of instructions to disassemble. If <paramref name="maxInstructions"/> is -1, all available instructions get returned.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(IRemoteMemoryReader process, IntPtr address, int length, int maxInstructions)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, length);

			if(process.IsWow64)
				return disassemblerWrapper.DisassembleCode(buffer, address, maxInstructions);
			else
				return DisassembleCode(buffer, address, maxInstructions);
		}

		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <param name="maxInstructions">The maximum number of instructions to disassemble. If <paramref name="maxInstructions"/> is -1, all available instructions get returned.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<DisassembledInstruction>();

                Smdkd.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref Smdkd.InstructionData instruction) =>
                 {
                     instructions.Add(new DisassembledInstruction(ref instruction));

                     return maxInstructions == -1 || instructions.Count < maxInstructions;
                 });

                return instructions;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="maxLength"/>) in the remote process until the first 0xCC instruction.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="maxLength">The maximum maxLength of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/> which belong to the function.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleFunction(IRemoteMemoryReader process, IntPtr address, int maxLength)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, maxLength);

			if (process.IsWow64)
				return disassemblerWrapper.DisassembleFunction(buffer, address);
			else
				return DisassembleFunction(buffer, address);
		}

		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/> which belong to the function.</returns>
		public IReadOnlyList<DisassembledInstruction> DisassembleFunction(byte[] data, IntPtr virtualAddress)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<DisassembledInstruction>();

				// Read until first CC.
				Smdkd.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref Smdkd.InstructionData result) =>
				{
					if (result.Length == 1 && result.Data[0] == 0xCC)
					{
						return false;
					}

					instructions.Add(new DisassembledInstruction(ref result));

					return true;
				});

				return instructions;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Tries to find and disassembles the instruction prior to the given address.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <returns>The prior instruction.</returns>
		public DisassembledInstruction RemoteGetPreviousInstruction(IRemoteMemoryReader process, IntPtr address)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<DisassembledInstruction>() != null);

			const int TotalBufferSize = 7 * MaximumInstructionLength;
			const int BufferShiftSize = 6 * MaximumInstructionLength;

			var buffer = process.ReadRemoteMemory(address - BufferShiftSize, TotalBufferSize);

			if (process.IsWow64)
				return disassemblerWrapper.RemoteGetPreviousInstruction(buffer, address);
			else
				return RemoteGetPreviousInstruction(buffer, address);
		}

		/// <summary>Tries to find and disassembles the instruction prior to the given address.</summary>
		/// <param name="buffer">The data to disassemble.</param>
		/// <param name="address">The address of the code.</param>
		/// <returns>The prior instruction.</returns>
		public DisassembledInstruction RemoteGetPreviousInstruction(byte[] buffer, IntPtr address)
		{
			const int BufferShiftSize = 6 * MaximumInstructionLength;

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var bufferAddress = handle.AddrOfPinnedObject();
				var targetBufferAddress = bufferAddress + BufferShiftSize;

				var instruction = default(Smdkd.InstructionData);

				foreach (var offset in new[]
				{
					6 * MaximumInstructionLength,
					4 * MaximumInstructionLength,
					2 * MaximumInstructionLength,
					1 * MaximumInstructionLength,
					14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
				})
				{
					var currentAddress = targetBufferAddress - offset;

					Smdkd.DisassembleCode(currentAddress, offset + 1, address - offset, false, (ref Smdkd.InstructionData data) =>
					{
						var nextAddress = currentAddress + data.Length;
						if (nextAddress.CompareTo(targetBufferAddress) > 0)
						{
							return false;
						}

						instruction = data;

						currentAddress = nextAddress;

						return true;
					});

					if (currentAddress == targetBufferAddress)
					{
						return new DisassembledInstruction(ref instruction);
					}
				}

				return null;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Tries to find the start address of the function <paramref name="address"/> points into.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address inside the function.</param>
		/// <returns>The start address of the function (maybe) or <see cref="IntPtr.Zero"/> if no start address could be found.</returns>
		public IntPtr RemoteGetFirstFunctionStartAddress(IEnumerable<DisassembledInstruction> instructions)
		{
			DisassembledInstruction next;
			DisassembledInstruction prev;
			DisassembledInstruction current;
			var list = instructions.OrderByDescending(ins => ins.Address, Util.IntPtrComparer.Instance).ToList();
			int length = instructions.Count();
			for (int i = 1; i < length-1; i++)
			{
				current = list[i];
				prev = list[i + 1];
				next = list[i - 1];

				// Search for 00. - ??? - sm unreadable address
				if (current.Instruction.StartsWith("???") && current.Length == 1 && current.Data[0] == 0x00)
				{
					if (i > 0)
						return list[i - 1].Address;
					else
						return current.Address;
				}
				// Search for C3. - ret - basic end of function
				else if (current.Length == 1 && current.Data[0] == 0xC3)
				{
					return next.Address;
				}
				// Search for two CC in a row. - int 3 - padding
				else if (current.Length == 1 && current.Data[0] == 0xCC && prev.Length == 1 && prev.Data[0] == 0xCC)
				{
					return next.Address;
				}
				// Search for one CC and one 00 in a row. - int 3 - padding
				else if (current.Length == 1 && current.Data[0] == 0xCC && prev.Length == 2 && prev.Data[0] == 0x00 && prev.Data[1] == 0x00)
				{
					return next.Address;
				}
				// Search for two 00 in a row. - add [eax],al - padding 
				else if (current.Length == 2 && current.Data[0] == 0x00 && current.Data[1] == 0x00)
				{
					return next.Address;
				}

			}

			return IntPtr.Zero;
		}
		
		IntPtr GetJumpAddress(DisassembledInstruction instruction, IEnumerable<DisassembledInstruction> instructions)
		{
			IntPtr jumpAddress = IntPtr.Zero;
			int indexOf = instruction.Instruction.IndexOf("0x");
			if (indexOf != -1)
			{
				indexOf += 2;
				int lastIndex = instruction.Instruction.SmFindAddressInString(indexOf);
				var addressStr = instruction.Instruction.SmSubstring(indexOf, lastIndex);
				if (long.TryParse(addressStr, System.Globalization.NumberStyles.HexNumber, null, out var addr))
				{
					jumpAddress = (IntPtr)addr;
					var rangeList = instructions.Where(i => i.Address.CompareTo(jumpAddress) < 0);
					var jumpIns = rangeList.Where(i => i.Instruction.StartsWith("j") && i.Instruction.IndexOf("0x") != -1);
				}
			}

			return jumpAddress;
        }
		public IntPtr RemoteGetFirstFunctionEndAddress(IEnumerable<DisassembledInstruction> instructions)
		{
			DisassembledInstruction current;
			var list = instructions.OrderBy(ins => ins.Address, Util.IntPtrComparer.Instance).ToList();
			IntPtr tempAddress = IntPtr.Zero;
			IntPtr jumpAddress = IntPtr.Zero;
			int length = instructions.Count();

			int indexOf;
			bool useOnlyFirstJump = true;
			bool isJumpSet = false;
			for (int i = 0; i < length; i++)
			{
				current = list[i];
                if (current.Instruction.StartsWith("???") && current.Length == 1 && current.Data[0] == 0x00)
                {
					if (i > 0)
						return list[i - 1].Address;
					else
						return current.Address;
				}

                if (useOnlyFirstJump && !isJumpSet)
				{
					if (current.Instruction.StartsWith("j") && (indexOf = current.Instruction.IndexOf("0x")) != -1)
					{
						tempAddress = GetJumpAddress(current, instructions);
						if (jumpAddress.CompareTo(tempAddress) < 0)
                        {
							jumpAddress = tempAddress;
							isJumpSet = true;
						}
					}
				}
				else if (!useOnlyFirstJump)
				{
					if (current.Instruction.StartsWith("j") && (indexOf = current.Instruction.IndexOf("0x")) != -1)
					{
						tempAddress = GetJumpAddress(current, instructions);
						if (jumpAddress.CompareTo(tempAddress) < 0)
						{
							jumpAddress = tempAddress;
						}
					}
				}

				// Search for C3. - ret - basic end of function
				if (current.Length == 1 && current.Data[0] == 0xC3)
				{
					if (current.Address.CompareTo(jumpAddress) < 0)
						return jumpAddress;
					else
						return current.Address;
				}
				// Search for CC. - int 3 - padding
				else if (current.Length == 1 && current.Data[0] == 0xCC)
				{
					if (current.Address.CompareTo(jumpAddress) < 0)
						return jumpAddress;
					else
						return current.Address;
				}
				// Search for 00. - add [eax],al - padding 
				else if (current.Length == 2 && current.Data[0] == 0x00 && current.Data[1] == 0x00)
				{
					if(current.Address.CompareTo(jumpAddress) < 0)
						return jumpAddress;
					else
						return current.Address;
				}
			}

			return IntPtr.Zero;
		}

	}

	public class DistinctDisassembledInstructionComparer : IEqualityComparer<DisassembledInstruction>
	{
		public bool Equals(DisassembledInstruction x, DisassembledInstruction y)
		{
			return x.Address.Equals(y.Address);
		}

		public int GetHashCode(DisassembledInstruction obj)
		{
			return obj.Address.GetHashCode();
		}
	}
	public class DisassembledInstruction
	{
		public IntPtr Address { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
		public string Instruction { get; set; }

		public bool IsValid => Length > 0;

		public DisassembledInstruction(IntPtr address)
		{
			Address = address;
			Length = 1;
			Data = new byte[15];
			Instruction = "???";
		}
		public DisassembledInstruction(IntPtr address, int length, byte[] data, string instraction)
		{
			Address = address;
			Length = length;
			Data = data.Clone() as byte[];
			Instruction = instraction.Clone() as string;
		}
		public DisassembledInstruction(ref Smdkd.InstructionData data)
		{
			Address = data.Address;
			Length = data.Length;
			Data = data.Data;
			Instruction = data.Instruction;
		}

		public override string ToString() => $"{Address.ToString(Program.AddressHexFormat)} - {Instruction}";
	}

}
