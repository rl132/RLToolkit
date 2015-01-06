using System;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Os detector.
    /// </summary>
	public static class OsDetector
	{
        /// <summary>The operating system. Mainly for internal use only.</summary>
		public static OperatingSystem os;

        ///<summary>the OS types available</summary>
		public enum OsSelection
		{
            /// <summary>windows-type of OS</summary>
			Windows,

            /// <summary>unix-type of OS</summary>
            Unix,

            /// <summary>Max-type of OS</summary>
			Mac,

            /// <summary>Xbox-type of OS</summary>
			Xbox,

            /// <summary>unknown type</summary>
            Unknown
		};

        /// <summary>The current os from the enum</summary>
		public static OsSelection currentOs;

		// todo: add a EOL character def
		// todo: add a folder separator character

        /// <summary>
        /// Manually override the operating system. Internal use only
        /// </summary>
        /// <param name="osInput">the new OS to use</param>
		public static void SetOS (OperatingSystem osInput)
		{
            LogManager.Instance.Log().Debug(string.Format("Setting OS to: {0}", osInput.ToString()));
			os = osInput;
		}

        /// <summary>
        /// Method that will detect which Operating system is used
        /// </summary>
        /// <returns>The os from the enum</returns>
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