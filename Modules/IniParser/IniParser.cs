using System;
using System.IO;
using System.Collections.Generic;

namespace RLToolkit
{
	public class IniParser
	{
		public List<DicConfiguration> content = new List<DicConfiguration>();
		public bool isEmpty = true;

		// constructors overloads
		public IniParser (string filename) : this(filename, AppDomain.CurrentDomain.BaseDirectory) {}

		public IniParser (string iniFilename, string iniFolder)
		{
            this.Log().Debug(string.Format("Creating a new IniParser with params: filename: \"{0}\", folder \"{1}\"", iniFilename, iniFolder));
			// exhaustivce constructor
			FileHandler fh = new FileHandler(iniFilename, iniFolder, false);
			List<string> raw = fh.ReadLines();
			fh.CloseStream();

			ConvertToDictionary(raw);
		}

		public IniParser (List<string> list)
		{
            this.Log().Debug(string.Format("Creating a new IniParser with params: list (numItem): {0}", list.Count.ToString()));
			ConvertToDictionary(list);
		}

		public IniParser ()
		{
            this.Log().Debug("Creating a new IniParser with no params.");
			// used for output mostly
			isEmpty = true;
		}

		public void ConvertToDictionary (List<string> rawData)
		{
            this.Log().Debug("Filling the Dictionary");
			DicConfiguration current = new DicConfiguration();
		
			foreach (string s in rawData)
			{
				if (s.Length == 0)
				{
                    this.Log().Debug("Empty string, skipping");
					continue;
				}

				if (IsLineComment(s))
			    {
					// skip line
                    this.Log().Debug("Comment line, skipping");
					continue;
				}

				if (IsHeader(s))
				{
                    this.Log().Debug(string.Format("Current line is a header: {0}", s));
                    // save the old one if not empty
					if (current.dicto.Count > 0)
					{
                        this.Log().Debug(string.Format("Adding current: {0}", current.header));
						content.Add(current);
					}

					// make a new one
					current = new DicConfiguration(s.Substring(1, s.Length -2));

					// next
					continue;
				}

				// not comment nor header. config!
				string[] splitData = s.Split("=".ToCharArray(0,1));
				if (splitData.GetLength(0) == 2)
				{
                    this.Log().Debug(string.Format("Adding to current: {0}={1}", splitData[0], splitData[1]));
					current.dicto.Add(splitData[0], splitData[1]);
				}
                else
                {
                    //invalid format. skipping
                    this.Log().Warn("invalid format for line: " + s);
                }
			}

			// if we have leftovers to save
			if (current.dicto.Count > 0)
			{
				this.Log().Debug(string.Format("Adding current: {0}", current.header));
                content.Add(current);
			}

			// set the isEmpty flag
			isEmpty = (content.Count == 0);
		}

		public bool IsHeader (string line)
		{
        	if ((line.Substring (0, 1) == "[") &&
				(line.Substring (line.Length-1, 1) == "]")) 
			{
                this.Log().Debug(string.Format("Line \"{0}\" is a header", line));
        		return true;
			}
            this.Log().Debug(string.Format("Line \"{0}\" is not a header", line));
			return false;
		}

		public bool IsLineComment (string line)
		{
			if (line.Substring (0, 1) == "#")
			{
                this.Log().Debug(string.Format("Line \"{0}\" is a comment", line));
				return true;
			}

			// todo: add more cases

            this.Log().Debug(string.Format("Line \"{0}\" is not a comment", line));
			return false;
		}

		public string GetValue (string header, string variable)
		{
            this.Log().Debug(string.Format("Trying to fetch value for \"{0}|{1}\"", header, variable));
			foreach (DicConfiguration dico in content)
			{
				if (dico.header.ToLower() == header.ToLower())
				{
					// we're in the right spot
					if (dico.dicto.ContainsKey(variable))
					{
						// we found it!
						string value = "";
						dico.dicto.TryGetValue(variable, out value);
                        this.Log().Debug(string.Format("found value {0}", value));
						return value;
					}
				}
			}
            this.Log().Debug("value not found");
			return "";
		}

		public bool AddDicConf (DicConfiguration toAdd)
		{
            this.Log().Debug(string.Format("trying to add Dictionary configuration: {0}", toAdd.header));
			bool found = false;

			if (toAdd.header == "") {
				this.Log().Debug("Header is empty");
                return false;
			}

			if (toAdd.dicto.Count == 0) {
                this.Log().Debug("Dictionary is empty");
				return false;
			}

			foreach (DicConfiguration d in content) {
				if (d.header.ToLower () == toAdd.header.ToLower ()) 
                {
                    this.Log().Debug("Found the Dictionary Confiuguration");
                    found = true;
				}
			}

			if (!found) {
                this.Log().Debug(string.Format("Adding toAdd: {0}", toAdd.header));
				content.Add(toAdd);
				isEmpty = false;
			}

			return (!found);
		}

		public bool AddDicEntry (string header, string variable, string value)
		{
            this.Log().Debug(string.Format("Trying to add a dictionary entry with params: header: {0}, variable: {1}, value: {2}", header, variable, value));

			Stack<DicConfiguration> old = new Stack<DicConfiguration> ();
			bool found = false;

			foreach (DicConfiguration d in content) {
				if (!found) {
					if (d.header.ToLower () == header.ToLower ()) {
						DicConfiguration newOne = d;
						if (!d.dicto.ContainsKey (variable)) {
                            this.Log().Debug("Found the dictionary entry, replacing it with the new content.");
							found = true;
							newOne.dicto.Add (variable, value);
						}
						else
						{
							// we already have that variable. exit
                            this.Log().Debug("Found the dictionary entry, which contains the variable already. Aborting.");
							return false;
						}
						old.Push (newOne);
					} else {
						old.Push (d);
					}
				} else {
					old.Push (d);
				}
			}

			if (found) {
				// replace the dicConfig list with the stack
				content = new List<DicConfiguration> ();
				foreach (DicConfiguration d in old) {
					content.Add (d);
				}
			}

			return found;
		}

		public bool WriteContent (string filename, string folder)
		{
            this.Log().Debug(string.Format("Trying to write the content of the Ini file with paramsL: filename: \"{0}\", folder: \"{1}\"", filename, folder));
            // if we're empty, no need to write
			if (content.Count == 0) {
				return false;
			}

			List<string> output = new List<string> ();
			foreach (DicConfiguration d in content) {
                this.Log().Debug(string.Format("Adding header: {0}", d.header));
				output.Add ("[" + d.header + "]");
				foreach(KeyValuePair<string, string> entry in d.dicto)
				{
                    this.Log().Debug(string.Format("Adding content: {0}={1}", entry.Key, entry.Value));
					output.Add (entry.Key + "=" + entry.Value);
				}
				output.Add ("");
			}

			FileHandler fh = new FileHandler (filename, folder, true);
			fh.WriteLines (output);
			fh.CloseStream ();

			return true;
		}

	}

}

