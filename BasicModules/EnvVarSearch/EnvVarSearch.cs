using System;
using System.Collections;
using System.Collections.Generic;

using RLToolkit.Logger;
using System.IO;

namespace RLToolkit.Basic
{
    /// <summary>
    /// OS Environment search
    /// </summary>
	public class EnvVarSearch
	{
		#region global vars
        private Dictionary<string, string> EnvVars;
		#endregion

		#region constructors
        /// <summary>
        /// Constructor with no parameters (uses the default environment variables)
        /// </summary>
		public EnvVarSearch () 
		{
			this.Log ().Debug ("Creating an Environment Search with the def. environment");
            // add everything in the global var in a Dictionary form.
            EnvVars = new Dictionary<string, string>();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                if ((de.Key != null) && (de.Key != null))
                {
                    this.Log().Debug(string.Format("Adding envvar key '{0}' with value '{1}'", de.Key.ToString(), de.Value.ToString()));
                    EnvVars.Add(de.Key.ToString(), de.Value.ToString());
                }
            }
		}

        /// <summary>
        /// Constructor that uses a supplied set of environment Variables (in Dictionary form already)
        /// </summary>
        /// <param name="e">The Environment variables to use</param>
        public EnvVarSearch (Dictionary<string, string> e)
		{
			this.Log ().Debug ("Creating an Environment Search with a specified environment");
            EnvVars = e;
		}
		#endregion

		#region methods

        /// <summary>
        /// Returns the environment variable dictionary, internal use only for testing.
        /// </summary>
        /// <returns>The dictionary</returns>
        internal Dictionary<string, string> GetDictionary()
        {
            this.Log().Debug("Fetching dictionary");
            return EnvVars;
        }

        /// <summary>
        /// Method to find if a path is in the environment PATH envvar
        /// </summary>
        /// <param name="path">The path to search for</param>
		/// <returns>returns True if found, False if not</returns>
		public bool IsPathInEnv(string path)
        {
            this.Log().Debug("Trying to search for a path in path envvar: " + path);
            if (EnvVars.ContainsKey("PATH"))
            {
                // try to get the path
                string pathValue = "";
                EnvVars.TryGetValue("PATH", out pathValue);

                if (String.IsNullOrWhiteSpace(pathValue))
                {
                    // empty path? warn!
                    this.Log().Warn("Empty path environment variable. something could be wrong.");
                    return false;
                }

                string lastChar = path.Substring(path.Length - 1, 1);
                if ((lastChar == "/") || (lastChar == @"\"))
                {
                    this.Log().Debug("Found a trailing slash. cutting it.");
                    path = path.Substring(0, path.Length - 1);
                }

                string[] arrayPaths = pathValue.Split(new char[] { ';', ':' });
                foreach (string s in arrayPaths)
                {
                    if (string.Compare(s, path, true) == 0)
                    {
                        this.Log().Debug("Path found");
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Method that itterates through the whole path environment and tries to find a specific file|path inside each folder
        /// </summary>
        /// <returns>returns the first path found that matches, or null if none</returns>
        /// <param name="path">the file|path to search for</param>
        public string FindInPath(string path)
		{
            this.Log().Debug("Trying to resolve a path|file in the path envvar: " + path);
            if (EnvVars.ContainsKey("PATH"))
            {
                // try to get the path
                string pathValue = "";
                EnvVars.TryGetValue("PATH", out pathValue);

                if (String.IsNullOrWhiteSpace(pathValue))
                {
                    // empty path? warn!
                    this.Log().Warn("Empty path environment variable. something could be wrong.");
                    return null;
                }

                string lastChar = path.Substring(path.Length - 1, 1);
                bool isFolder = false;
                if ((lastChar == "/") || (lastChar == @"\"))
                {
                    // Folder mode!
                    this.Log().Debug("Found a trailing slash. Folder mode and removing the slash.");
                    isFolder = true;
                    path = path.Substring(0, path.Length - 1);
                }

                string[] arrayPaths = pathValue.Split(new char[] { ';', ':' });
                string toCheck = "";
                foreach (string s in arrayPaths)
                {
                    toCheck = Path.Combine(s, path);
                    if (isFolder)
                    {
                        if (Directory.Exists(toCheck))
                        {
                            this.Log().Debug(string.Format("Found matching path: {0}", toCheck));
                            return toCheck;
                        }    
                    } else
                    {
                        if (File.Exists(toCheck))
                        {
                            this.Log().Debug(string.Format("Found matching file: {0}", toCheck));
                            return toCheck;
                        }    
                    }
                }
            }

            this.Log().Info("No path|file was resolved");
            return null;
		}
		#endregion
	}
}