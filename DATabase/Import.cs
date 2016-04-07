﻿using System;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using SabreTools.Helper;

namespace SabreTools
{
	/// <summary>
	/// Import data into the database from existing DATs
	/// </summary>
	public class Import
	{
		// Private instance variables
		private string _filepath;
		private string _connectionString;
		private Logger _logger;

		// Regex File Name Patterns
		private static string _defaultPattern = @"^(.+?) - (.+?) \((.*) (.*)\)\.dat$";
		private static string _mamePattern = @"^(.*)\.xml$";
		private static string _maybeIntroPattern = @"(.*?) \[T-En\].*\((\d{8})\)\.dat$";
		private static string _noIntroPattern = @"^(.*?) \((\d{8}-\d{6})_CM\)\.dat$";
		private static string _noIntroNumberedPattern = @"(.*? - .*?) \(\d.*?_CM\).dat";
		private static string _noIntroSpecialPattern = @"(.*? - .*?) \((\d{8})\)\.dat";
		private static string _nonGoodPattern = @"^(NonGood.*?)( .*)?.xml";
		private static string _redumpPattern = @"^(.*?) \((\d{8} \d{2}-\d{2}-\d{2})\)\.dat$";
		private static string _redumpBiosPattern = @"^(.*?) \(\d+\) \((\d{4}-\d{2}-\d{2})\)\.dat$";
		private static string _tosecPattern = @"^(.*?) - .* \(TOSEC-v(\d{4}-\d{2}-\d{2})_CM\)\.dat$";
		private static string _tosecSpecialPatternA = @"^(.*? - .*?) - .* \(TOSEC-v(\d{4}-\d{2}-\d{2})_CM\)\.dat$";
		private static string _tosecSpecialPatternB = @"^(.*? - .*? - .*?) - .* \(TOSEC-v(\d{4}-\d{2}-\d{2})_CM\)\.dat$";
		private static string _truripPattern = @"^(.*) - .* \(trurip_XML\)\.dat$";
		private static string _zandroPattern = @"^SMW-.*.xml";

		// Regex Mapped Name Patterns
		private static string _remappedPattern = @"^(.*) - (.*)$";

		// Regex Date Patterns
		private static string _defaultDatePattern = @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})";
		private static string _noIntroDatePattern = @"(\d{4})(\d{2})(\d{2})-(\d{2})(\d{2})(\d{2})";
		private static string _noIntroSpecialDatePattern = @"(\d{4})(\d{2})(\d{2})";
		private static string _redumpDatePattern = @"(\d{4})(\d{2})(\d{2}) (\d{2})-(\d{2})-(\d{2})";
		private static string _tosecDatePattern = @"(\d{4})-(\d{2})-(\d{2})";

		/// <summary>
		/// Possible DAT import classes
		/// </summary>
		private enum DatType
		{
			none = 0,
			Custom,
			MAME,
			NoIntro,
			Redump,
			TOSEC,
			TruRip,
			NonGood,
			MaybeIntro,
		}

		// Public instance variables
		public string FilePath
		{
			get { return _filepath; }
		}

		/// <summary>
		/// Initialize an Import object with the given information
		/// </summary>
		/// <param name="filepath">Path to the file that is going to be imported</param>
		/// <param name="connectionString">Connection string for SQLite</param>
		/// <param name="logger">Logger object for file or console output</param>
		public Import(string filepath, string connectionString, Logger logger)
		{
			if (File.Exists(filepath))
			{
				_filepath = filepath;
			}
			else
			{
				throw new IOException("File " + filepath + " does not exist!");
			}

			_connectionString = connectionString;
			_logger = logger;
		}

		/// <summary>
		/// Import the data from file into the database
		/// </summary>
		/// <returns>True if the data was imported, false otherwise</returns>
		public bool ImportData ()
		{
			// Determine which dattype we have
			string filename = Path.GetFileName(_filepath);
			GroupCollection fileinfo;
			DatType type = DatType.none;

			if (Regex.IsMatch(filename, _nonGoodPattern))
			{
				fileinfo = Regex.Match(filename, _nonGoodPattern).Groups;
				type = DatType.NonGood;
			}
			else if (Regex.IsMatch(filename, _maybeIntroPattern))
			{
				fileinfo = Regex.Match(filename, _maybeIntroPattern).Groups;
				type = DatType.MaybeIntro;
			}
			else if (Regex.IsMatch(filename, _noIntroPattern))
			{
				fileinfo = Regex.Match(filename, _noIntroPattern).Groups;
				type = DatType.NoIntro;
			}
			// For numbered DATs only
			else if (Regex.IsMatch(filename, _noIntroNumberedPattern))
			{
				fileinfo = Regex.Match(filename, _noIntroNumberedPattern).Groups;
				type = DatType.NoIntro;
			}
			// For N-Gage and Gizmondo only
			else if (Regex.IsMatch(filename, _noIntroSpecialPattern))
			{
				fileinfo = Regex.Match(filename, _noIntroSpecialPattern).Groups;
				type = DatType.NoIntro;
			}
			else if (Regex.IsMatch(filename, _redumpPattern))
			{
				fileinfo = Regex.Match(filename, _redumpPattern).Groups;
				type = DatType.Redump;
			}
			// For special BIOSes only
			else if (Regex.IsMatch(filename, _redumpBiosPattern))
			{
				fileinfo = Regex.Match(filename, _redumpBiosPattern).Groups;
				type = DatType.Redump;
			}
			else if (Regex.IsMatch(filename, _tosecPattern))
			{
				fileinfo = Regex.Match(filename, _tosecPattern).Groups;
				type = DatType.TOSEC;
			}
			else if (Regex.IsMatch(filename, _truripPattern))
			{
				fileinfo = Regex.Match(filename, _truripPattern).Groups;
				type = DatType.TruRip;
			}
			else if (Regex.IsMatch(filename, _zandroPattern))
			{
				filename = "Nintendo - Super Nintendo Entertainment System (Zandro " + File.GetLastWriteTime(_filepath).ToString("yyyyMMddHHmmss") + ").dat";
				fileinfo = Regex.Match(filename, _defaultPattern).Groups;
				type = DatType.Custom;
			}
			else if (Regex.IsMatch(filename, _defaultPattern))
			{
				fileinfo = Regex.Match(filename, _defaultPattern).Groups;
				type = DatType.Custom;
			}
			else if (Regex.IsMatch(filename, _mamePattern))
			{
				fileinfo = Regex.Match(filename, _mamePattern).Groups;
				type = DatType.MAME;
			}
			// If the type is still unmatched, the data can't be imported yet
			else
			{
				_logger.Error("File " + filename + " cannot be imported at this time because it is not a known pattern.\nPlease try again with an unrenamed version.");
				return false;
			}

			_logger.Log("Type detected: " + type.ToString());

			// Check for and extract import information from the file name based on type
			string manufacturer = "";
			string system = "";
			string source = "";
			string datestring = "";
			string date = "";

			switch (type)
			{
				case DatType.MAME:
					if (!Remapping.MAME.ContainsKey(fileinfo[1].Value))
					{
						_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
						return false;
					}
					GroupCollection mameInfo = Regex.Match(Remapping.MAME[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = mameInfo[1].Value;
					system = mameInfo[2].Value;
					source = "MAME";
					date = File.GetLastWriteTime(_filepath).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DatType.MaybeIntro:
					if (!Remapping.MaybeIntro.ContainsKey(fileinfo[1].Value))
					{
						_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
						return false;
					}
					GroupCollection maybeIntroInfo = Regex.Match(Remapping.MaybeIntro[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = maybeIntroInfo[1].Value;
					system = maybeIntroInfo[2].Value;
					source = "Maybe-Intro";
					datestring = fileinfo[2].Value;
					GroupCollection miDateInfo = Regex.Match(datestring, _noIntroSpecialDatePattern).Groups;
					date = miDateInfo[1].Value + "-" + miDateInfo[2].Value + "-" + miDateInfo[3].Value + " 00:00:00";
					break;
				case DatType.NoIntro:
					if (!Remapping.NoIntro.ContainsKey(fileinfo[1].Value))
					{
						_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
						return false;
					}
					GroupCollection nointroInfo = Regex.Match(Remapping.NoIntro[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = nointroInfo[1].Value;
					system = nointroInfo[2].Value;
					source = "no-Intro";
					if (fileinfo.Count < 2)
					{
						date = File.GetLastWriteTime(_filepath).ToString("yyyy-MM-dd HH:mm:ss");
					}
					else if (Regex.IsMatch(fileinfo[2].Value, _noIntroDatePattern))
					{
						datestring = fileinfo[2].Value;
						GroupCollection niDateInfo = Regex.Match(datestring, _noIntroDatePattern).Groups;
						date = niDateInfo[1].Value + "-" + niDateInfo[2].Value + "-" + niDateInfo[3].Value + " " +
							niDateInfo[4].Value + ":" + niDateInfo[5].Value + ":" + niDateInfo[6].Value;
					}
					else
					{
						datestring = fileinfo[2].Value;
						GroupCollection niDateInfo = Regex.Match(datestring, _noIntroSpecialDatePattern).Groups;
						date = niDateInfo[1].Value + "-" + niDateInfo[2].Value + "-" + niDateInfo[3].Value + " 00:00:00";
					}
					break;
				case DatType.NonGood:
					if (!Remapping.NonGood.ContainsKey(fileinfo[1].Value))
					{
						_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
						return false;
					}
					GroupCollection nonGoodInfo = Regex.Match(Remapping.NonGood[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = nonGoodInfo[1].Value;
					system = nonGoodInfo[2].Value;
					source = "NonGood";
					date = File.GetLastWriteTime(_filepath).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DatType.Redump:
					if (!Remapping.Redump.ContainsKey(fileinfo[1].Value))
					{
						// Handle special case mappings found only in Redump
						fileinfo = Regex.Match(filename, _redumpBiosPattern).Groups;

						if (!Remapping.Redump.ContainsKey(fileinfo[1].Value))
						{
							_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
							return false;
						}
					}
					GroupCollection redumpInfo = Regex.Match(Remapping.Redump[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = redumpInfo[1].Value;
					system = redumpInfo[2].Value;
					source = "Redump";
					datestring = fileinfo[2].Value;
					if (Regex.IsMatch(datestring, _redumpDatePattern))
					{
						GroupCollection rdDateInfo = Regex.Match(datestring, _redumpDatePattern).Groups;
						date = rdDateInfo[1].Value + "-" + rdDateInfo[2].Value + "-" + rdDateInfo[3].Value + " " +
							rdDateInfo[4].Value + ":" + rdDateInfo[5].Value + ":" + rdDateInfo[6].Value;
					}
					else
					{
						GroupCollection rdDateInfo = Regex.Match(datestring, _tosecDatePattern).Groups;
						date = rdDateInfo[1].Value + "-" + rdDateInfo[2].Value + "-" + rdDateInfo[3].Value + " 00:00:00";
					}
					
					break;
				case DatType.TOSEC:
					if (!Remapping.TOSEC.ContainsKey(fileinfo[1].Value))
					{
						// Handle special case mappings found only in TOSEC
						fileinfo = Regex.Match(filename, _tosecSpecialPatternA).Groups;

						if (!Remapping.TOSEC.ContainsKey(fileinfo[1].Value))
						{
							fileinfo = Regex.Match(filename, _tosecSpecialPatternB).Groups;

							if (!Remapping.TOSEC.ContainsKey(fileinfo[1].Value))
							{
								_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
								return false;
							}
						}
					}
					GroupCollection tosecInfo = Regex.Match(Remapping.TOSEC[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = tosecInfo[1].Value;
					system = tosecInfo[2].Value;
					source = "TOSEC";
					datestring = fileinfo[2].Value;
					GroupCollection toDateInfo = Regex.Match(datestring, _tosecDatePattern).Groups;
					date = toDateInfo[1].Value + "-" + toDateInfo[2].Value + "-" + toDateInfo[3].Value + " 00:00:00";
					break;
				case DatType.TruRip:
					if (!Remapping.TruRip.ContainsKey(fileinfo[1].Value))
					{
						_logger.Error("The filename " + fileinfo[1].Value + " could not be mapped! Please check the mappings and try again");
						return false;
					}
					GroupCollection truripInfo = Regex.Match(Remapping.TruRip[fileinfo[1].Value], _remappedPattern).Groups;

					manufacturer = truripInfo[1].Value;
					system = truripInfo[2].Value;
					source = "trurip";
					date = File.GetLastWriteTime(_filepath).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DatType.Custom:
				default:
					manufacturer = fileinfo[1].Value;
					system = fileinfo[2].Value;
					source = fileinfo[3].Value;
					datestring = fileinfo[4].Value;

					GroupCollection cDateInfo = Regex.Match(datestring, _defaultDatePattern).Groups;
					date = cDateInfo[1].Value + "-" + cDateInfo[2].Value + "-" + cDateInfo[3].Value + " " +
						cDateInfo[4].Value + ":" + cDateInfo[5].Value + ":" + cDateInfo[6].Value;
					break;
			}

			// Check to make sure that the manufacturer and system are valid according to the database
			int sysid = -1;
			string query = "SELECT id FROM systems WHERE manufacturer='" + manufacturer + "' AND system='" + system +"'";
			using (SQLiteConnection dbc = new SQLiteConnection(_connectionString))
			{
				dbc.Open();
				using (SQLiteCommand slc = new SQLiteCommand(query, dbc))
				{
					using (SQLiteDataReader sldr = slc.ExecuteReader())
					{
						// If nothing is found, tell the user and exit
						if (!sldr.HasRows)
						{
							_logger.Error("No suitable system for '" + filename + "' (" + manufacturer + " " + system + ") found! Please add the system and then try again.");
							return false;
						}

						// Set the system ID from the first found value
						sldr.Read();
						sysid = sldr.GetInt32(0);
					}
				}
			}

			// Check to make sure that the source is valid according to the database
			int srcid = -1;
			query = "SELECT id FROM sources WHERE name='" + source + "'";
			using (SQLiteConnection dbc = new SQLiteConnection(_connectionString))
			{
				dbc.Open();
				using (SQLiteCommand slc = new SQLiteCommand(query, dbc))
				{
					using (SQLiteDataReader sldr = slc.ExecuteReader())
					{
						// If nothing is found, tell the user and exit
						if (!sldr.HasRows)
						{
							_logger.Error("No suitable source for '" + filename + "' found! Please add the source and then try again.");
							return false;
						}

						// Set the source ID from the first found value
						sldr.Read();
						srcid = sldr.GetInt32(0);
					}
				}
			}

			// Attempt to load the current file as XML
			bool superdat = false;
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(File.ReadAllText(_filepath));
			}
			catch (XmlException)
			{
				doc.LoadXml(Converters.RomVaultToXML(File.ReadAllLines(_filepath)).ToString());
			}

			// Experimental looping using only XML parsing
			XmlNode node = doc.FirstChild;
			if (node != null && node.Name == "xml")
			{
				// Skip over everything that's not an element
				while (node.NodeType != XmlNodeType.Element)
				{
					node = node.NextSibling;
				}
			}

			// Once we find the main body, enter it
			if (node != null && (node.Name == "datafile" || node.Name == "softwarelist"))
			{
				node = node.FirstChild;
			}

			// Skip the header if it exists
			if (node != null && node.Name == "header")
			{
				// Check for SuperDAT mode
				if (node.SelectSingleNode("name").InnerText.Contains(" - SuperDAT"))
				{
					superdat = true;
				}

				// Skip over anything that's not an element
				while (node.NodeType != XmlNodeType.Element)
				{
					node = node.NextSibling;
				}
			}

			while (node != null)
			{
				if (node.NodeType == XmlNodeType.Element && (node.Name == "machine" || node.Name == "game" || node.Name == "software"))
				{
					long gameid = -1;
					string tempname = "";
					if (node.Name == "software")
					{
						tempname = node.SelectSingleNode("description").InnerText;
					}
					else
					{
						// There are rare cases where a malformed XML will not have the required attributes. We can only skip them.
						if (node.Attributes.Count == 0)
						{
							_logger.Error(@"A node with malformed XML has been found!
	For RV DATs, please make sure that all names and descriptions are quoted.
	For XML DATs, make sure that the DAT has all required information.");
							node = node.NextSibling;
							continue;
						}
						tempname = node.Attributes["name"].Value;
					}

					if (superdat)
					{
						tempname = Regex.Match(tempname, @".*?\\(.*)").Groups[1].Value;
					}

					gameid = AddGame(sysid, tempname, srcid);
					
					// Get the roms from the machine
					if (node.HasChildNodes)
					{
						// If this node has children, traverse the children
						foreach (XmlNode child in node.ChildNodes)
						{
							// If we find a rom or disk, add it
							if (child.NodeType == XmlNodeType.Element && (child.Name == "rom" || child.Name == "disk"))
							{
								// Take care of hex-sized files
								long size = -1;
								if (child.Attributes["size"] != null && child.Attributes["size"].Value.Contains("0x"))
								{
									size = Convert.ToInt64(child.Attributes["size"].Value, 16);
								}
								else if (child.Attributes["size"] != null)
								{
									size = Int64.Parse(child.Attributes["size"].Value);
								}

								AddRom(
									child.Name,
									gameid,
									child.Attributes["name"].Value,
									date,
									size,
									(child.Attributes["crc"] != null ? child.Attributes["crc"].Value.ToLowerInvariant().Trim() : ""),
									(child.Attributes["md5"] != null ? child.Attributes["md5"].Value.ToLowerInvariant().Trim() : ""),
									(child.Attributes["sha1"] != null ? child.Attributes["sha1"].Value.ToLowerInvariant().Trim() : "")
								);
							}
							// If we find the signs of a software list, traverse the children
							else if (child.NodeType == XmlNodeType.Element && child.Name == "part" && child.HasChildNodes)
							{
								foreach (XmlNode part in child.ChildNodes)
								{
									// If we find a dataarea, traverse the children
									if (part.NodeType == XmlNodeType.Element && part.Name == "dataarea")
									{
										foreach (XmlNode data in part.ChildNodes)
										{
											// If we find a rom or disk, add it
											if (data.NodeType == XmlNodeType.Element && (data.Name == "rom" || data.Name == "disk") && data.Attributes["name"] != null)
											{
												// Take care of hex-sized files
												long size = -1;
												if (data.Attributes["size"] != null && data.Attributes["size"].Value.Contains("0x"))
												{
													size = Convert.ToInt64(data.Attributes["size"].Value, 16);
												}
												else if (data.Attributes["size"] != null)
												{
													size = Int64.Parse(data.Attributes["size"].Value);
												}

												AddRom(
													data.Name,
													gameid,
													data.Attributes["name"].Value,
													date,
													size,
													(data.Attributes["crc"] != null ? data.Attributes["crc"].Value.ToLowerInvariant().Trim() : ""),
													(data.Attributes["md5"] != null ? data.Attributes["md5"].Value.ToLowerInvariant().Trim() : ""),
													(data.Attributes["sha1"] != null ? data.Attributes["sha1"].Value.ToLowerInvariant().Trim() : "")
												);
											}
										}
									}
								}
							}
						}
					}
				}
				node = node.NextSibling;
			}

			return true;
		}

		/// <summary>
		/// Add a game to the database if it doesn't already exist
		/// </summary>
		/// <param name="sysid">System ID for the game to be added with</param>
		/// <param name="machinename">Name of the game to be added</param>
		/// <param name="srcid">Source ID for the game to be added with</param>
		/// <returns>Game ID of the inserted (or found) game, -1 on error</returns>
		private long AddGame(int sysid, string machinename, int srcid)
		{
			// WoD gets rid of anything past the first "(" or "[" as the name, we will do the same
			string stripPattern = @"(([[(].*[\)\]] )?([^([]+))";
			Regex stripRegex = new Regex(stripPattern);
			Match stripMatch = stripRegex.Match(machinename);
			machinename = stripMatch.Groups[1].Value;

			// Run the name through the filters to make sure that it's correct
			machinename = Style.NormalizeChars(machinename);
			machinename = Style.RussianToLatin(machinename);
			machinename = Style.SearchPattern(machinename);
			machinename = machinename.Trim();

			long gameid = -1;
			string query = "SELECT id FROM games WHERE system=" + sysid +
				" AND name='" + machinename.Replace("'", "''") + "'" +
				" AND source=" + srcid;

			using (SQLiteConnection dbc = new SQLiteConnection(_connectionString))
			{
				dbc.Open();
				using (SQLiteCommand slc = new SQLiteCommand(query, dbc))
				{
					using (SQLiteDataReader sldr = slc.ExecuteReader())
					{
						// If nothing is found, add the game and get the insert ID
						if (!sldr.HasRows)
						{
							query = "INSERT INTO games (system, name, source)" +
								" VALUES (" + sysid + ", '" + machinename.Replace("'", "''") + "', " + srcid + ")";

							using (SQLiteCommand slc2 = new SQLiteCommand(query, dbc))
							{
								slc2.ExecuteNonQuery();
							}

							query = "SELECT last_insert_rowid()";
							using (SQLiteCommand slc2 = new SQLiteCommand(query, dbc))
							{
								gameid = (long)slc2.ExecuteScalar();
							}
						}
						// Otherwise, retrieve the ID
						else
						{
							sldr.Read();
							gameid = sldr.GetInt64(0);
						}
					}
				}
			}

			return gameid;
		}

		/// <summary>
		/// Add a file to the database if it doesn't already exist
		/// </summary>
		/// <param name="romtype">File type (either "rom" or "disk")</param>
		/// <param name="gameid">ID of the parent game to be mapped to</param>
		/// <param name="name">File name</param>
		/// <param name="date">Last updated date</param>
		/// <param name="size">File size in bytes</param>
		/// <param name="crc">CRC32 hash of the file</param>
		/// <param name="md5">MD5 hash of the file</param>
		/// <param name="sha1">SHA-1 hash of the file</param>
		/// <returns>True if the file exists or could be added, false on error</returns>
		private bool AddRom(string romtype, long gameid, string name, string date, long size, string crc, string md5, string sha1)
		{
			// WOD origninally stripped out any subdirs from the imported files, we do the same
			name = Path.GetFileName(name);

			// Run the name through the filters to make sure that it's correct
			name = Style.NormalizeChars(name);
			name = Style.RussianToLatin(name);
			name = Regex.Replace(name, @"(.*) \.(.*)", "$1.$2");

			if (romtype != "rom" && romtype != "disk")
			{
				romtype = "rom";
			}

			// Check to see if this exact file is in the database already
			string query = @"
SELECT files.id FROM files
	JOIN checksums
	ON files.id=checksums.file
	WHERE files.name='" + name.Replace("'", "''") + @"'
		AND files.type='" + romtype + @"' 
		AND files.setid=" + gameid + " " + 
		" AND checksums.size=" + size +
		" AND checksums.crc='" + crc + "'" +
		" AND checksums.md5='" + md5 + "'" +
		" AND checksums.sha1='" + sha1 + "'";
			using (SQLiteConnection dbc = new SQLiteConnection(_connectionString))
			{
				dbc.Open();
				using (SQLiteCommand slc = new SQLiteCommand(query, dbc))
				{
					using (SQLiteDataReader sldr = slc.ExecuteReader())
					{
						// If the file doesn't exist, add it
						if (!sldr.HasRows)
						{
							query = @"
INSERT INTO files (setid, name, type, lastupdated)
	VALUES (" + gameid + ", '" + name.Replace("'", "''") + "', '" + romtype + "', '" + date + "')";
							using (SQLiteCommand slc2 = new SQLiteCommand(query, dbc))
							{
								int affected = slc2.ExecuteNonQuery();

								// If the insert was successful, add the checksums for the file
								if (affected >= 1)
								{
									query = "SELECT last_insert_rowid()";
									long romid = -1;
									using (SQLiteCommand slc3 = new SQLiteCommand(query, dbc))
									{
										romid = (long)slc3.ExecuteScalar();
									}

									query = @"INSERT INTO checksums (file, size, crc, md5, sha1) VALUES (" +
										romid + ", " + size + ", '" + crc + "'" + ", '" + md5 + "'" + ", '" + sha1 + "')";
									using (SQLiteCommand slc3 = new SQLiteCommand(query, dbc))
									{
										affected = slc3.ExecuteNonQuery();
									}

									// If the insert of the checksums failed, that's bad
									if (affected < 1)
									{
										_logger.Error("There was an error adding checksums for " + name + " to the database!");
										return false;
									}
								}
								// Otherwise, something happened which is bad
								else
								{
									_logger.Error("There was an error adding " + name + " to the database!");
									return false;
								}
							}
						}
					}
				}
			}

			return true;
		}
	}
}
