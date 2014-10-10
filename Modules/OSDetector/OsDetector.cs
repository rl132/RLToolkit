using System;

namespace RLToolkit
{
	public static class OsDetector
	{
		public static OperatingSystem os;
		public enum OsSelection
		{
			Windows,
			Unix,
			Mac,
			Xbox,
			Unknown
		};
		public static OsSelection currentOs;
		// todo: add a EOL character def
		// todo: add a folder separator character

		public static void SetOS (OperatingSystem osInput)
		{
            LogManager.Instance.Log().Debug(string.Format("Setting OS to: {0}", osInput.ToString()));
			os = osInput;
		}

		public static OsSelection DetectOs ()
		{
            LogManager.Instance.Log().Debug("Trying to detect OS");
		    // in case we're not initialized, do this
            if (os == null) {
                LogManager.Instance.Log().Debug("OS not initialized. fetching environment.");
                os = Environment.OSVersion;
            }

			// figure out which OS we're using
			OperatingSystem opSys = os;
			PlatformID platform = opSys.Platform;

			switch (platform) 
			{
			case PlatformID.MacOSX:
				currentOs = OsSelection.Mac;
				break;
			case PlatformID.Unix:
				currentOs = OsSelection.Unix;
				break;
			case PlatformID.Win32NT:
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.WinCE:
				currentOs = OsSelection.Windows;
				break;
			case PlatformID.Xbox:
				currentOs = OsSelection.Xbox;
				break;
			default:
				currentOs = OsSelection.Unknown;
				break;
			}

            LogManager.Instance.Log().Debug(string.Format("current OS found: {0}", currentOs.ToString()));
			return currentOs;
		}
	}
}