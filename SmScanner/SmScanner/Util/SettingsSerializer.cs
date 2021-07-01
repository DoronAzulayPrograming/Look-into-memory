using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SmScanner.Util
{
	internal sealed class SettingsSerializer
	{
		private const string XmlRootElement = "Settings";
		private const string XmlGeneralElement = "General";
		private const string XmlMemoryViewElement = "MemoryView";
		private const string XmlDisplayElement = "Display";
		private const string XmlColorsElement = "Colors";
		private const string XmlCustomDataElement = "CustomData";

		#region Read Settings

		public static Settings Load()
		{
			EnsureSettingsDirectoryAvailable();

			var settings = new Settings();

			try
			{
				var path = Path.Combine(PathUtil.SettingsFolderPath, Constants.SettingsFile);

				using var sr = new StreamReader(path);

				var document = XDocument.Load(sr);
				var root = document.Root;

				var general = root?.Element(XmlGeneralElement);
				if (general != null)
				{
					XElementSerializer.TryRead(general, nameof(settings.LastProcess), e => settings.LastProcess = XElementSerializer.ToString(e));
					XElementSerializer.TryRead(general, nameof(settings.StayOnTop), e => settings.StayOnTop = XElementSerializer.ToBool(e));
					XElementSerializer.TryRead(general, nameof(settings.RunAsAdmin), e => settings.RunAsAdmin = XElementSerializer.ToBool(e));
				}

				var memoryView = root?.Element(XmlMemoryViewElement);
				if (memoryView != null)
				{
					XElementSerializer.TryRead(memoryView, nameof(settings.HexBufferSize), e => settings.HexBufferSize = XElementSerializer.ToInt(e));
					XElementSerializer.TryRead(memoryView, nameof(settings.DissassemblerBufferSize), e => settings.DissassemblerBufferSize = XElementSerializer.ToInt(e));
				}

				var colors = root?.Element(XmlColorsElement);
				if (colors != null)
				{
					XElementSerializer.TryRead(colors, nameof(settings.BackgroundColor), e => settings.BackgroundColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.SelectedColor), e => settings.SelectedColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.HiddenColor), e => settings.HiddenColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.OffsetColor), e => settings.OffsetColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.AddressColor), e => settings.AddressColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.HexColor), e => settings.HexColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.TypeColor), e => settings.TypeColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.NameColor), e => settings.NameColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.ValueColor), e => settings.ValueColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.IndexColor), e => settings.IndexColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.CommentColor), e => settings.CommentColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.TextColor), e => settings.TextColor = XElementSerializer.ToColor(e));
					XElementSerializer.TryRead(colors, nameof(settings.VTableColor), e => settings.VTableColor = XElementSerializer.ToColor(e));
				}
				var customData = root?.Element(XmlCustomDataElement);
				if (customData != null)
				{
					settings.CustomData.Deserialize(customData);
				}
			}
			catch
			{
				// ignored
			}

			return settings;
		}

		#endregion

		#region Write Settings

		public static void Save(Settings settings)
		{
			Contract.Requires(settings != null);

			EnsureSettingsDirectoryAvailable();

			var path = Path.Combine(PathUtil.SettingsFolderPath, Constants.SettingsFile);

			using var sw = new StreamWriter(path);

			var document = new XDocument(
				new XComment($"{Constants.ApplicationName} {Constants.ApplicationVersion} by {Constants.Author}"),
				new XComment($"Website: {Constants.HomepageUrl}"),
				new XElement(
					XmlRootElement,
					new XElement(
						XmlGeneralElement,
						XElementSerializer.ToXml(nameof(settings.LastProcess), settings.LastProcess),
						XElementSerializer.ToXml(nameof(settings.StayOnTop), settings.StayOnTop),
						XElementSerializer.ToXml(nameof(settings.RunAsAdmin), settings.RunAsAdmin)
					),
					new XElement(
						XmlMemoryViewElement,
						XElementSerializer.ToXml(nameof(settings.HexBufferSize), settings.HexBufferSize),
						XElementSerializer.ToXml(nameof(settings.DissassemblerBufferSize), settings.DissassemblerBufferSize)
					),
					new XElement(
						XmlColorsElement,
						XElementSerializer.ToXml(nameof(settings.BackgroundColor), settings.BackgroundColor),
						XElementSerializer.ToXml(nameof(settings.SelectedColor), settings.SelectedColor),
						XElementSerializer.ToXml(nameof(settings.HiddenColor), settings.HiddenColor),
						XElementSerializer.ToXml(nameof(settings.OffsetColor), settings.OffsetColor),
						XElementSerializer.ToXml(nameof(settings.AddressColor), settings.AddressColor),
						XElementSerializer.ToXml(nameof(settings.HexColor), settings.HexColor),
						XElementSerializer.ToXml(nameof(settings.TypeColor), settings.TypeColor),
						XElementSerializer.ToXml(nameof(settings.NameColor), settings.NameColor),
						XElementSerializer.ToXml(nameof(settings.ValueColor), settings.ValueColor),
						XElementSerializer.ToXml(nameof(settings.IndexColor), settings.IndexColor),
						XElementSerializer.ToXml(nameof(settings.CommentColor), settings.CommentColor),
						XElementSerializer.ToXml(nameof(settings.TextColor), settings.TextColor),
						XElementSerializer.ToXml(nameof(settings.VTableColor), settings.VTableColor)
					),
					settings.CustomData.Serialize(XmlCustomDataElement)
				)
			);

			document.Save(sw);
		}

		#endregion

		private static void EnsureSettingsDirectoryAvailable()
		{
			try
			{
				if (Directory.Exists(PathUtil.SettingsFolderPath) == false)
				{
					Directory.CreateDirectory(PathUtil.SettingsFolderPath);
				}
			}
			catch
			{
				// ignored
			}
		}
	}
}
