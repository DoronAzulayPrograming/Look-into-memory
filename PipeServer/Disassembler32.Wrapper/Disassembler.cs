using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;

namespace Disassembler32.Wrapper
{
    public static class Disassembler
	{
		// The maximum number of bytes of a x86-64 instruction.
		public const int MaximumInstructionLength = 15;
		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <param name="maxInstructions">The maximum number of instructions to disassemble. If <paramref name="maxInstructions"/> is -1, all available instructions get returned.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public static IReadOnlyList<InstructionData> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IList<InstructionData>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<InstructionData>();

				Dll.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref InstructionData instruction) =>
				{
					instructions.Add(instruction);

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
		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/> which belong to the function.</returns>
		public static IReadOnlyList<InstructionData> DisassembleFunction(byte[] data, IntPtr virtualAddress)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<InstructionData>();

				// Read until first CC.
				Dll.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref InstructionData result) =>
				{
					if (result.Length == 1 && result.Data[0] == 0xCC)
					{
						return false;
					}

					instructions.Add(result);

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
		public static InstructionData RemoteGetPreviousInstruction(byte[] buffer, IntPtr address)
		{
			const int BufferShiftSize = 6 * MaximumInstructionLength;

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var bufferAddress = handle.AddrOfPinnedObject();
				var targetBufferAddress = bufferAddress + BufferShiftSize;

				var instruction = default(InstructionData);

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

					Dll.DisassembleCode(currentAddress, offset + 1, address - offset, false, (ref InstructionData data) =>
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
						return instruction;
					}
				}

				return instruction;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}
	}
	public class DisassembledInstruction
	{
		public IntPtr Address { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
		public string Instruction { get; set; }

		public bool IsValid => Length > 0;

		public DisassembledInstruction(ref InstructionData data)
		{
			Address = data.Address;
			Length = data.Length;
			Data = data.Data;
			Instruction = data.Instruction;
		}

		public override string ToString() => $"{Address.ToString("X08")} - {Instruction}";
	}

	public static class IntPtrEx
	{
		[Pure]
		[System.Diagnostics.DebuggerStepThrough]
		public static int CompareTo(this IntPtr lhs, IntPtr rhs)
		{
			return ((uint)lhs.ToInt32()).CompareTo((uint)rhs.ToInt32());
		}
	}
}
