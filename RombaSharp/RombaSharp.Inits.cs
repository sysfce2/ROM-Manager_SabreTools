﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;

using SabreTools.Library.Data;
using SabreTools.Library.DatFiles;
using SabreTools.Library.DatItems;
using SabreTools.Library.Tools;

#if MONO
using System.IO;
#else
using Alphaleonis.Win32.Filesystem;
#endif

namespace RombaSharp
{
	public partial class RombaSharp
	{
		#region Init Methods

		/// <summary>
		/// Wrap adding files to the depots
		/// </summary>
		/// <param name="inputs">List of input folders to use</param>
		/// <param name="onlyNeeded">True if only files in the database and don't exist are added, false otherwise</param>
		/// <param name="resume">Resume a previously interrupted operation from the specified path</param>
		/// <param name="includeZips">flag value == 1 means: add Zip files themselves into the depot in addition to their contents, flag value > 1 means add Zip files themselves but don't add content</param>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="includeGZips">flag value == 1 means: add GZip files themselves into the depot in addition to their contents, flag value > 1 means add GZip files themselves but don't add content</param>
		/// <param name="include7Zips">flag value == 1 means: add 7Zip files themselves into the depot in addition to their contents, flag value > 1 means add 7Zip files themselves but don't add content</param>
		/// <param name="skipInitialScan">True to skip the initial scan of the files to determine amount of work, false otherwise</param>
		/// <param name="useGolangZip">True to use go zip implementation instead of zlib, false otherwise</param>
		/// <param name="noDb">True to archive into depot but do not touch DB index and ignore only-needed flag, false otherwise</param>
		/// TODO: Verify implementation
		private static void InitArchive(
			List<string> inputs,
			bool onlyNeeded,
			string resume,
			int includeZips,
			int workers,
			int includeGZips,
			int include7Zips,
			bool skipInitialScan,
			bool useGolangZip,
			bool noDb)
		{
			Globals.Logger.Error("This feature is not yet implemented: archive");

			// First we want to get just all directories from the inputs
			List<string> onlyDirs = new List<string>();
			foreach (string input in inputs)
			{
				if (Directory.Exists(input))
				{
					onlyDirs.Add(Path.GetFullPath(input));
				}
			}

			// Then process all of the input directories into an internal DAT
			DatFile df = new DatFile();
			foreach (string dir in onlyDirs)
			{
				// TODO: All instances of Hash.DeepHashes should be made into 0x0 eventually
				df.PopulateFromDir(dir, Hash.DeepHashes, false, false, SkipFileType.None, false, false, _tmpdir, false, null, true);
				df.PopulateFromDir(dir, Hash.DeepHashes, false, true, SkipFileType.None, false, false, _tmpdir, false, null, true);
			}

			// Create an empty Dat for files that need to be rebuilt
			DatFile need = new DatFile();

			// Open the database connection
			SqliteConnection dbc = new SqliteConnection(_connectionString);
			dbc.Open();

			// Now that we have the Dats, add the files to the database
			string crcquery = "INSERT OR IGNORE INTO crc (crc) VALUES";
			string md5query = "INSERT OR IGNORE INTO md5 (md5) VALUES";
			string sha1query = "INSERT OR IGNORE INTO sha1 (sha1, depot) VALUES";
			string crcsha1query = "INSERT OR IGNORE INTO crcsha1 (crc, sha1) VALUES";
			string md5sha1query = "INSERT OR IGNORE INTO md5sha1 (md5, sha1) VALUES";

			foreach (string key in df.Keys)
			{
				List<DatItem> datItems = df[key];
				foreach (Rom rom in datItems)
				{
					// If we care about if the file exists, check the databse first
					if (onlyNeeded)
					{
						string query = "SELECT * FROM crcsha1 JOIN md5sha1 ON crcsha1.sha1=md5sha1.sha1"
									+ " WHERE crcsha1.crc=\"" + rom.CRC + "\""
									+ " OR md5sha1.md5=\"" + rom.MD5 + "\""
									+ " OR md5sha1.sha1=\"" + rom.SHA1 + "\"";
						SqliteCommand slc = new SqliteCommand(query, dbc);
						SqliteDataReader sldr = slc.ExecuteReader();
						
						if (sldr.HasRows)
						{
							// Add to the queries
							if (!String.IsNullOrWhiteSpace(rom.CRC))
							{
								crcquery += " (\"" + rom.CRC + "\"),";
							}
							if (!String.IsNullOrWhiteSpace(rom.MD5))
							{
								md5query += " (\"" + rom.MD5 + "\"),";
							}
							if (!String.IsNullOrWhiteSpace(rom.SHA1))
							{
								sha1query += " (\"" + rom.SHA1 + "\", \"" + _depots.Keys.ToList()[0] + "\"),";

								if (!String.IsNullOrWhiteSpace(rom.CRC))
								{
									crcsha1query += " (\"" + rom.CRC + "\", \"" + rom.SHA1 + "\"),";
								}
								if (!String.IsNullOrWhiteSpace(rom.MD5))
								{
									md5sha1query += " (\"" + rom.MD5 + "\", \"" + rom.SHA1 + "\"),";
								}
							}

							// Add to the Dat
							need.Add(key, rom);
						}
					}
					// Otherwise, just add the file to the list
					else
					{
						// Add to the queries
						if (!String.IsNullOrWhiteSpace(rom.CRC))
						{
							crcquery += " (\"" + rom.CRC + "\"),";
						}
						if (!String.IsNullOrWhiteSpace(rom.MD5))
						{
							md5query += " (\"" + rom.MD5 + "\"),";
						}
						if (!String.IsNullOrWhiteSpace(rom.SHA1))
						{
							sha1query += " (\"" + rom.SHA1 + "\", \"" + _depots.Keys.ToList()[0] + "\"),";

							if (!String.IsNullOrWhiteSpace(rom.CRC))
							{
								crcsha1query += " (\"" + rom.CRC + "\", \"" + rom.SHA1 + "\"),";
							}
							if (!String.IsNullOrWhiteSpace(rom.MD5))
							{
								md5sha1query += " (\"" + rom.MD5 + "\", \"" + rom.SHA1 + "\"),";
							}
						}

						// Add to the Dat
						need.Add(key, rom);
					}
				}
			}

			// Now run the queries, if they're populated
			if (crcquery != "INSERT OR IGNORE INTO crc (crc) VALUES")
			{
				SqliteCommand slc = new SqliteCommand(crcquery.TrimEnd(','), dbc);
				slc.ExecuteNonQuery();
				slc.Dispose();
			}
			if (md5query != "INSERT OR IGNORE INTO md5 (md5) VALUES")
			{
				SqliteCommand slc = new SqliteCommand(md5query.TrimEnd(','), dbc);
				slc.ExecuteNonQuery();
				slc.Dispose();
			}
			if (sha1query != "INSERT OR IGNORE INTO sha1 (sha1, depot) VALUES")
			{
				SqliteCommand slc = new SqliteCommand(sha1query.TrimEnd(','), dbc);
				slc.ExecuteNonQuery();
				slc.Dispose();
			}
			if (crcsha1query != "INSERT OR IGNORE INTO crcsha1 (crc, sha1) VALUES")
			{
				SqliteCommand slc = new SqliteCommand(crcsha1query.TrimEnd(','), dbc);
				slc.ExecuteNonQuery();
				slc.Dispose();
			}
			if (md5sha1query != "INSERT OR IGNORE INTO md5sha1 (md5, sha1) VALUES")
			{
				SqliteCommand slc = new SqliteCommand(md5sha1query.TrimEnd(','), dbc);
				slc.ExecuteNonQuery();
				slc.Dispose();
			}

			// Create the sorting object to use and rebuild the needed files
			ArchiveScanLevel asl = Utilities.GetArchiveScanLevelFromNumbers((onlyNeeded ? 0 : 1), (onlyNeeded ? 0 : 1), (onlyNeeded ? 0 : 1), (onlyNeeded ? 0 : 1));
			need.RebuildGeneric(onlyDirs, _depots.Keys.ToList()[0], false /*quickScan*/, false /*date*/,
				false /*delete*/, false /*inverse*/, OutputFormat.TorrentGzip, true /*romba*/, asl, false /*updateDat*/,
				null /*headerToCheckAgainst*/, true /* chdsAsFiles */);
		}

		/// <summary>
		/// Wrap building all files from a set of DATs
		/// </summary>
		/// <param name="inputs">List of input DATs to rebuild from</param>
		/// <param name="outdat">Output file</param>
		/// <paran name="fixdatOnly">True to only fix dats and don't generate torrentzips, false otherwise</paran>
		/// <param name="copy">True if files should be copied to output, false for rebuild</param>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="subworkers">How many subworkers to launch for each worker, default from config</param>
		/// TODO: Verify implementation
		private static void InitBuild(
			List<string> inputs,
			string outdat,
			bool fixdatOnly,
			bool copy,
			int workers,
			int subworkers)
		{
			// Verify the filenames
			Dictionary<string, string> foundDats = GetValidDats(inputs);

			// Create a base output folder
			if (!Directory.Exists("out"))
			{
				Directory.CreateDirectory("out");
			}

			// Now that we have the dictionary, we can loop through and output to a new folder for each
			foreach (string key in foundDats.Keys)
			{
				// Get the DAT file associated with the key
				DatFile datFile = new DatFile();
				datFile.Parse(Path.Combine(_dats, foundDats[key]), 0, 0);

				// Create the new output directory if it doesn't exist
				string outputFolder = Path.Combine("out", Path.GetFileNameWithoutExtension(foundDats[key]));
				if (!Directory.Exists(outputFolder))
				{
					Directory.CreateDirectory(outputFolder);
				}

				// Get all online depots
				List<string> onlineDepots = _depots.Where(d => d.Value.Item2).Select(d => d.Key).ToList();

				// Now scan all of those depots and rebuild
				ArchiveScanLevel asl = Utilities.GetArchiveScanLevelFromNumbers(1, 1, 1, 1);
				datFile.RebuildDepot(onlineDepots, outputFolder, false /*date*/,
					false /*delete*/, false /*inverse*/, (copy ? OutputFormat.TorrentGzip : OutputFormat.TorrentZip), copy,
					false /*updateDat*/, null /*headerToCheckAgainst*/);
			}
		}

		/// <summary>
		/// Wrap cancelling a long-running job
		/// </summary>
		private static void InitCancel()
		{
			Globals.Logger.User("This feature is obsolete: cancel");
		}

		/// <summary>
		/// Wrap printing dat stats
		/// </summary>
		/// <param name="inputs">List of input DATs to get stats from</param>
		private static void InitDatStats(List<string> inputs)
		{
			// If we have no inputs listed, we want to use datroot
			if (inputs == null || inputs.Count == 0)
			{
				inputs = new List<string>();
				inputs.Add(Path.GetFullPath(_dats));
			}

			// Now output the stats for all inputs
			DatFile.OutputStats(inputs, "rombasharp-datstats", null /* outDir */, true /* single */, true /* baddumpCol */, true /* nodumpCol */, StatReportFormat.Textfile);
		}

		/// <summary>
		/// Wrap printing db stats
		/// </summary>
		private static void InitDbStats()
		{
			DisplayDBStats();
		}

		/// <summary>
		/// Wrap creating a diffdat for a given old and new dat
		/// </summary>
		/// <param name="outdat">Output file</param>
		/// <param name="old">Old DAT file</param>
		/// <param name="newdat">New DAT file</param>
		/// <param name="name">Name value in DAT header</param>
		/// <param name="description">Description value in DAT header</param>
		private static void InitDiffDat(
			string outdat,
			string old,
			string newdat,
			string name,
			string description)
		{
			// Ensure the output directory
			Utilities.EnsureOutputDirectory(outdat, create: true);

			// Check that all required files exist
			if (!File.Exists(old))
			{
				Globals.Logger.Error("File '{0}' does not exist!", old);
				return;
			}
			if (!File.Exists(newdat))
			{
				Globals.Logger.Error("File '{0}' does not exist!", newdat);
				return;
			}

			// Create the encapsulating datfile
			DatFile datfile = new DatFile()
			{
				Name = name,
				Description = description,
			};

			// Create the inputs
			List<string> dats = new List<string>();
			dats.Add(newdat);
			List<string> basedats = new List<string>();
			basedats.Add(old);

			// Now run the diff on the inputs
			datfile.DetermineUpdateType(dats, basedats, outdat, UpdateMode.DiffAgainst, false /* inplace */, false /* skip */,
				true /* bare */, false /* clean */, false /* remUnicode */, false /* descAsName */, new Filter(), SplitType.None,
				ReplaceMode.None, false /* onlySame */);
		}

		/// <summary>
		/// Wrap creating a dir2dat from a given source
		/// </summary>
		/// <param name="outdat">Output file</param>
		/// <param name="source">Source directory</param>
		/// <param name="name">Name value in DAT header</param>
		/// <param name="description">Description value in DAT header</param>
		private static void InitDir2Dat(
			string outdat,
			string source,
			string name,
			string description)
		{
			// Ensure the output directory
			Utilities.EnsureOutputDirectory(outdat, create: true);

			// Check that all required directories exist
			if (!Directory.Exists(source))
			{
				Globals.Logger.Error("File '{0}' does not exist!", source);
				return;
			}

			// Create the encapsulating datfile
			DatFile datfile = new DatFile()
			{
				Name = (String.IsNullOrWhiteSpace(name) ? "untitled" : name),
				Description = description,
			};

			// Now run the D2D on the input and write out
			// TODO: All instances of Hash.DeepHashes should be made into 0x0 eventually
			datfile.PopulateFromDir(source, Hash.DeepHashes, true /* bare */, false /* archivesAsFiles */, SkipFileType.None, false /* addBlanks */,
				false /* addDate */, _tmpdir, false /* copyFiles */, null /* headerToCheckAgainst */, true /* chdsAsFiles */);
			datfile.Write(outDir: outdat);
		}

		/// <summary>
		/// Wrap creating a diffdat for a given old and new dat
		/// </summary>
		/// <param name="outdat">Output file</param>
		/// <param name="old">Old DAT file</param>
		/// <param name="newdat">New DAT file</param>
		private static void InitEDiffDat(
			string outdat,
			string old,
			string newdat)
		{
			// Ensure the output directory
			Utilities.EnsureOutputDirectory(outdat, create: true);

			// Check that all required files exist
			if (!File.Exists(old))
			{
				Globals.Logger.Error("File '{0}' does not exist!", old);
				return;
			}
			if (!File.Exists(newdat))
			{
				Globals.Logger.Error("File '{0}' does not exist!", newdat);
				return;
			}

			// Create the encapsulating datfile
			DatFile datfile = new DatFile();

			// Create the inputs
			List<string> dats = new List<string>();
			dats.Add(newdat);
			List<string> basedats = new List<string>();
			basedats.Add(old);

			// Now run the diff on the inputs
			datfile.DetermineUpdateType(dats, basedats, outdat, UpdateMode.DiffAgainst, false /* inplace */, false /* skip */,
				true /* bare */, false /* clean */, false /* remUnicode */, false /* descAsName */, new Filter(), SplitType.None,
				ReplaceMode.None, false /* onlySame */);
		}

		/// <summary>
		/// Wrap exporting the database to CSV
		/// </summary>
		/// TODO: Verify implementation
		private static void InitExport()
		{
			ExportDatabase();
		}

		/// <summary>
		/// Wrap creating a fixdat for each Dat
		/// </summary>
		/// <param name="inputs">List of input DATs to get fixdats for</param>
		/// <param name="outdat">Output directory</param>
		/// <paran name="fixdatOnly">True to only fix dats and don't generate torrentzips, false otherwise</paran>
		/// <param name = "workers" > How many workers to launch for the job, default from config</param>
		/// <param name="subworkers">How many subworkers to launch for each worker, default from config</param>
		/// TODO: Implement
		private static void InitFixdat(
			List<string> inputs,
			string outdat,
			bool fixdatOnly,
			int workers,
			int subworkers)
		{
			Globals.Logger.Error("This feature is not yet implemented: fixdat");
		}

		/// <summary>
		/// Wrap importing CSVs into the database
		/// </summary>
		/// <param name="inputs">List of input CSV files to import information from</param>
		/// TODO: Implement
		private static void InitImport(List<string> inputs)
		{
			Globals.Logger.Error("This feature is not yet implemented: import");
		}

		/// <summary>
		/// Wrap looking up if hashes exist in the database
		/// </summary>
		/// <param name="inputs">List of input strings representing hashes to check for</param>
		/// <param name="size">Size to limit hash by, -1 otherwise</param>
		/// <param name="outdat">Output directory</param>
		/// TODO: Verify implementation
		private static void InitLookup(
			List<string> inputs,
			long size,
			string outdat)
		{
			Globals.Logger.Error("This feature is not yet implemented: lookup");

			// First, try to figure out what type of hash each is by length and clean it
			List<string> crc = new List<string>();
			List<string> md5 = new List<string>();
			List<string> sha1 = new List<string>();
			foreach (string input in inputs)
			{
				string temp = "";
				if (input.Length == Constants.CRCLength)
				{
					temp = Utilities.CleanHashData(input, Constants.CRCLength);
					if (!String.IsNullOrWhiteSpace(temp))
					{
						crc.Add(temp);
					}
				}
				else if (input.Length == Constants.MD5Length)
				{
					temp = Utilities.CleanHashData(input, Constants.MD5Length);
					if (!String.IsNullOrWhiteSpace(temp))
					{
						md5.Add(temp);
					}
				}
				else if (input.Length == Constants.SHA1Length)
				{
					temp = Utilities.CleanHashData(input, Constants.SHA1Length);
					if (!String.IsNullOrWhiteSpace(temp))
					{
						sha1.Add(temp);
					}
				}
			}

			SqliteConnection dbc = new SqliteConnection(_connectionString);
			dbc.Open();

			// Now, search for each of them and return true or false for each
			foreach (string input in crc)
			{
				string query = "SELECT * FROM crc WHERE crc=\"" + input + "\"";
				SqliteCommand slc = new SqliteCommand(query, dbc);
				SqliteDataReader sldr = slc.ExecuteReader();
				if (sldr.HasRows)
				{
					Globals.Logger.User("For hash '{0}' there were {1} matches in the database", input, sldr.RecordsAffected);
				}
				else
				{
					Globals.Logger.User("Hash '{0}' had no matches in the database", input);
				}

				sldr.Dispose();
				slc.Dispose();
			}
			foreach (string input in md5)
			{
				string query = "SELECT * FROM md5 WHERE md5=\"" + input + "\"";
				SqliteCommand slc = new SqliteCommand(query, dbc);
				SqliteDataReader sldr = slc.ExecuteReader();
				if (sldr.HasRows)
				{
					Globals.Logger.User("For hash '{0}' there were {1} matches in the database", input, sldr.RecordsAffected);
				}
				else
				{
					Globals.Logger.User("Hash '{0}' had no matches in the database", input);
				}

				sldr.Dispose();
				slc.Dispose();
			}
			foreach (string input in sha1)
			{
				string query = "SELECT * FROM sha1 WHERE sha1=\"" + input + "\"";
				SqliteCommand slc = new SqliteCommand(query, dbc);
				SqliteDataReader sldr = slc.ExecuteReader();
				if (sldr.HasRows)
				{
					Globals.Logger.User("For hash '{0}' there were {1} matches in the database", input, sldr.RecordsAffected);
				}
				else
				{
					Globals.Logger.User("Hash '{0}' had no matches in the database", input);
				}

				sldr.Dispose();
				slc.Dispose();
			}

			dbc.Dispose();
		}

		/// <summary>
		/// Wrap printing memory stats
		/// </summary>
		private static void InitMemstats()
		{
			Globals.Logger.User("This feature is obsolete: cancel");
		}

		/// <summary>
		/// Wrap merging an external depot into an existing one
		/// </summary>
		/// <param name="inputs">List of input depots to merge in</param>
		/// <param name="onlyNeeded">True if only files in the database and don't exist are added, false otherwise</param>
		/// <param name="resume">Resume a previously interrupted operation from the specified path</param>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="skipInitialScan">True to skip the initial scan of the files to determine amount of work, false otherwise</param>
		/// TODO: Add way of specifying "current depot" since that's what Romba relies on
		/// TODO: Implement
		private static void InitMerge(
			List<string> inputs,
			bool onlyNeeded,
			string resume,
			int workers,
			bool skipInitialscan)
		{
			Globals.Logger.Error("This feature is not yet implemented: merge");
		}

		/// <summary>
		/// Wrap creating a havefile and a missfile for each Dat
		/// </summary>
		/// <param name="inputs">List of DAT files to get a miss and have for, empty means all</param>
		/// TODO: Implement
		private static void InitMiss(List<string> inputs)
		{
			Globals.Logger.Error("This feature is not yet implemented: miss");
		}

		/// <summary>
		/// Wrap showing progress of currently running command
		/// </summary>
		private static void InitProgress()
		{
			Globals.Logger.User("This feature is obsolete: progress");
		}

		/// <summary>
		/// Wrap backing up of no longer needed files from the depots
		/// </summary>
		/// <param name="backup">Backup directory where backup files are moved to</param>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="depot">List of depots to scan files in, empty means all</param>
		/// <param name="dats">List of DATs to use as the basis of scanning, empty means all</param>
		/// <param name="logOnly">True if only the output of the operation is shown, false to actually run</param>
		/// TODO: Implement
		private static void InitPurgeBackup(
			string backup,
			int workers,
			List<string> depot,
			List<string> dats,
			bool logOnly)
		{
			Globals.Logger.Error("This feature is not yet implemented: purge-backup");
		}

		/// <summary>
		/// Wrap deleting of no longer needed files from the depots
		/// </summary>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="depot">List of depots to scan files in, empty means all</param>
		/// <param name="dats">List of DATs to use as the basis of scanning, empty means all</param>
		/// <param name="logOnly">True if only the output of the operation is shown, false to actually run</param>
		/// TODO: Implement
		private static void InitPurgeDelete(
			int workers,
			List<string> depot,
			List<string> dats,
			bool logOnly)
		{
			Globals.Logger.Error("This feature is not yet implemented: purge-delete");
		}

		/// <summary>
		/// Wrap refreshing the database with potentially new dats
		/// </summary>
		/// <param name="workers">How many workers to launch for the job, default from config</param>
		/// <param name="missingSha1s">Write paths of dats with missing sha1s into this file</param>
		/// TODO: Verify implementation
		private static void InitRefreshDats(
			int workers,
			string missingSha1s)
		{
			Globals.Logger.Error("This feature is not yet implemented: refresh-dats");

			RefreshDatabase();
		}

		/// <summary>
		/// Wrap rescanning depots
		/// </summary>
		/// <param name="inputs">List of depots to rescan, empty means all</param>
		/// TODO: Verify implementation
		private static void InitRescanDepots(List<string> inputs)
		{
			Globals.Logger.Error("This feature is not yet implemented: rescan-depots");

			foreach (string depot in inputs)
			{
				Rescan(depot);
			}
		}

		/// <summary>
		/// Wrap gracefully shutting down the server
		/// </summary>
		private static void InitShutdown()
		{
			Globals.Logger.User("This feature is obsolete: shutdown");
		}

		/// <summary>
		/// Wrap printing the version
		/// </summary>
		private static void InitVersion()
		{
			Globals.Logger.User("RombaSharp version: {0}", Constants.Version);
		}

		#endregion
	}
}
