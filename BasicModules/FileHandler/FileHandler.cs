using System;
using System.IO;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    /// <summary>
    /// FileHandler
    /// </summary>
	public class FileHandler
	{
		// variables
		private StreamReader sReader;
		private List<String> readerContent = new List<String>();
		private StreamWriter sWriter;

        /// <summary>
        /// If the FileHandler is ready to be used
        /// </summary>
		public bool isReady = false;

        /// <summary>
        /// The full path of the file in use
        /// </summary>
		public string fullPath;

        /// <summary>
        /// If the FileHandler is in Write Mode
        /// </summary>
		public bool isWrite = false;

        /// <summary>
        /// Partial constructor
        /// </summary>
        /// <param name="filename">full or partial filename to use</param>
        public FileHandler (string filename) : this(filename, AppDomain.CurrentDomain.BaseDirectory, false) {}

        /// <summary>
        /// Partial constructor
        /// </summary>
        /// <param name="filename">full or partial filename to use</param>
        /// <param name="folder">folder to use</param>
        public FileHandler (string filename, string folder) : this(filename, folder, false) {}

        /// <summary>
        /// Partial constructor
        /// </summary>
        /// <param name="filename">full or partial filename to use</param>
        /// <param name="forWrite">If set to <c>true</c>, the FileHandle will be in write mode.</param>
        public FileHandler (string filename, bool forWrite) : this(filename, AppDomain.CurrentDomain.BaseDirectory, forWrite) {}
		
        /// <summary>
        /// Exhaustive constructor for the FileHandler.
        /// </summary>
        /// <param name="filename">full or partial filename to use</param>
        /// <param name="folder">folder to use</param>
        /// <param name="forWrite">If set to <c>true</c>, the FileHandle will be in write mode.</param>
		public FileHandler (string filename, string folder, bool forWrite)
		{ // exhaustive constructor
            this.Log().Debug(string.Format("FileHandler created with params: Filename: \"{0}\", folder: \"{1}\", ForWrite: {2}", filename, folder, forWrite.ToString()));

			// test if the filenme provided is a complete path
            if (forWrite)
            {
                if (folder == null || folder == "")
                {
                    // use the filename as fullpath
                    fullPath = filename;
                    this.Log().Debug("no folder provided, using filename as full path");
                }
                else
                {
                    fullPath = Path.Combine(folder, filename);
                    this.Log().Debug("combining folder and filename.");
                }
            } 
            else
            {
                if (File.Exists(filename))
                {
                    fullPath = filename;
                    this.Log().Debug("Filename exists already, using this as target.");
                }
                else
                {
                    fullPath = Path.Combine(folder, filename);
                    this.Log().Debug("Filename doesn't exist, combining folder and filename.");
                }
            }

			// cache the params
			isWrite = forWrite;

			if (!isWrite) {
				if (!File.Exists (fullPath)) {
                    this.Log().Warn("Filename for read doesn't exists. Cannot read!");
					throw new FileNotFoundException (String.Format ("File: \"{0}\" doesn't exist.", fullPath));
				}
			}

			// open the file in the stream
			try
			{
				if (!forWrite) 
				{
					sReader = new StreamReader(fullPath);
				}
				else
				{
					if (File.Exists (fullPath)) 
					{
						if (File.GetAttributes(fullPath).HasFlag(FileAttributes.ReadOnly))
						{
                            this.Log().Warn("Filename for write is read-only. Cannot open.");
							throw new FileHandlerException("File is read-only. Cannot open for write.");
						}
					}

					sWriter = new StreamWriter(fullPath);
				}
			}
			catch (FileHandlerException) 
			{
				//throw up
				throw;
			}
			catch (Exception e)
			{
				// something went wrong, throw it back up
                this.Log().Fatal("Something went terribly wrong, unknown exception caught.\n" + e.Message);
				throw;
			}

			// we're ready to be used
			isReady = true;
		}

        /// <summary>
        /// Fetch the stream used for the current FileHandler
        /// </summary>
        /// <returns>The stream object</returns>
		public object GetStream ()
		{
			// in case somebody wants to have the raw stream
			if (!isReady) 
			{
                this.Log().Warn("Trying to get the Stream while not initialized");
				throw new FileHandlerException("Not initialized.");
			}
			if (isWrite) 
			{
				return sWriter;
			}
			else
			{
				return sReader;
			}
		}

        /// <summary>
        /// Method to read the lines of a FileHandler set in read mode.
        /// </summary>
        /// <returns>The List of string output of the lines</returns>
		public List<string> ReadLines()
		{
			if (!isReady) 
			{
                this.Log().Warn("Trying to read while not initialized");
				throw new FileHandlerException("Not initialized.");
			}

			if (isWrite)
			{
                this.Log().Warn("Trying to read on a write access mode.");
				this.CloseStream ();
				throw new FileHandlerException("Wrong access mode.");
			}

			try
			{
				// read all lines
				while (!sReader.EndOfStream)
				{
					// read and stack
					readerContent.Add(sReader.ReadLine());
				}
                this.Log().Debug(string.Format("Read {0} lines of content", readerContent.Count.ToString()));
			}
			catch (Exception e)
			{
				// something went wrong, throw it back up
                this.Log().Fatal("Something went terribly wrong, unknown exception caught.\n" + e.Message);
                throw;
			}

			// return the content of the file
			return readerContent;
		}

        /// <summary>
        /// Method to write the lines to a FileHandler set in write mode.
        /// </summary>
        /// <returns><c>true</c>, if lines was writed, <c>false</c> otherwise.</returns>
        /// <param name="lines">List of string as input</param>
		public bool WriteLines (List<string> lines)
		{
			if (!isReady) {
                this.Log().Warn("Trying to write while not initialized");
				throw new FileHandlerException("Not initialized.");
			}

			if (!isWrite) {
                this.Log().Warn("Trying to write on a read access mode.");
				this.CloseStream ();
				throw new FileHandlerException("Wrong access mode.");
			}

			try
			{
				foreach (string s in lines) {
                    this.Log().Debug("Writing: " + s);
                    sWriter.WriteLine (s);
				}
                this.Log().Debug(string.Format("wrote {0} lines of content", lines.Count.ToString()));
			}
			catch (Exception e)
			{
			    this.Log().Fatal("Something went terribly wrong, unknown exception caught.\n" + e.Message);
            	// something went terribly wrong, throw it back up
                throw;
			}

			return true;
		}

        /// <summary>
        /// Method to closes the stream used
        /// </summary>
        /// <returns><c>true</c>, if stream was closed, <c>false</c> otherwise.</returns>
		public bool CloseStream()
		{
            this.Log().Debug("Trying to close stream");
			if (!isReady) 
			{
                this.Log().Warn("Trying to close stream while not initialized");
				throw new FileHandlerException("Not initialized.");
			}

			try
			{
				if (isWrite)
				{
					sWriter.Close();
				}
				else
				{
					sReader.Close();
				}
			}
			catch (Exception e)
			{
				// something went wrong, throw it back up
                this.Log().Fatal("Something went terribly wrong, unknown exception caught.\n" + e.Message);
                throw;
			}

			isReady = false;
			isWrite = false;

			return true;
		}
	}
}