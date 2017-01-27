﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SabreTools.Helper.Data;
using SabreTools.Helper.Skippers;
using SabreTools.Helper.Tools;
using SharpCompress.Common;

#if MONO
using System.IO;
#else
using Alphaleonis.Win32.Filesystem;

using SearchOption = System.IO.SearchOption;
#endif

namespace SabreTools.Helper.Dats
{
	public partial class DatFile
	{
		#region Rebuilding and Verifying [MODULAR DONE, FOR NOW]

		/// <summary>
		/// Process the DAT and find all matches in input files and folders
		/// </summary>
		/// <param name="inputs">List of input files/folders to check</param>
		/// <param name="outDir">Output directory to use to build to</param>
		/// <param name="tempDir">Temporary directory for archive extraction</param>
		/// <param name="quickScan">True to enable external scanning of archives, false otherwise</param>
		/// <param name="date">True if the date from the DAT should be used if available, false otherwise</param>
		/// <param name="delete">True if input files should be deleted, false otherwise</param>
		/// <param name="inverse">True if the DAT should be used as a filter instead of a template, false otherwise</param>
		/// <param name="outputFormat">Output format that files should be written to</param>
		/// <param name="romba">True if files should be output in Romba depot folders, false otherwise</param>
		/// <param name="archiveScanLevel">ArchiveScanLevel representing the archive handling levels</param>
		/// <param name="updateDat">True if the updated DAT should be output, false otherwise</param>
		/// <param name="headerToCheckAgainst">Populated string representing the name of the skipper to use, a blank string to use the first available checker, null otherwise</param>
		/// <param name="logger">Logger object for file and console output</param>
		/// <returns>True if rebuilding was a success, false otherwise</returns>
		public bool RebuildToOutput(List<string> inputs, string outDir, string tempDir, bool quickScan, bool date,
			bool delete, bool inverse, OutputFormat outputFormat, bool romba, ArchiveScanLevel archiveScanLevel, bool updateDat,
			string headerToCheckAgainst, int maxDegreeOfParallelism, Logger logger)
		{
			#region Perform setup

			// If the DAT is not populated and inverse is not set, inform the user and quit
			if (Count == 0 && !inverse)
			{
				logger.User("No entries were found to rebuild, exiting...");
				return false;
			}

			// Check that the output directory exists
			if (!Directory.Exists(outDir))
			{
				Directory.CreateDirectory(outDir);
				outDir = Path.GetFullPath(outDir);
			}

			// Check the temp directory
			if (String.IsNullOrEmpty(tempDir))
			{
				tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			}

			// Then create or clean the temp directory
			if (!Directory.Exists(tempDir))
			{
				Directory.CreateDirectory(tempDir);
			}
			else
			{
				FileTools.CleanDirectory(tempDir);
			}

			// Preload the Skipper list
			int listcount = Skipper.List.Count;

			#endregion

			bool success = true;

			#region Rebuild from sources in order

			switch (outputFormat)
			{
				case OutputFormat.Folder:
					logger.User("Rebuilding all files to directory");
					break;
				case OutputFormat.TapeArchive:
					logger.User("Rebuilding all files to TAR");
					break;
				case OutputFormat.Torrent7Zip:
					logger.User("Rebuilding all files to Torrent7Z");
					break;
				case OutputFormat.TorrentGzip:
					logger.User("Rebuilding all files to TorrentGZ");
					break;
				case OutputFormat.TorrentLrzip:
					logger.User("Rebuilding all files to TorrentLRZ");
					break;
				case OutputFormat.TorrentRar:
					logger.User("Rebuilding all files to TorrentRAR");
					break;
				case OutputFormat.TorrentXZ:
					logger.User("Rebuilding all files to TorrentXZ");
					break;
				case OutputFormat.TorrentZip:
					logger.User("Rebuilding all files to TorrentZip");
					break;
			}
			DateTime start = DateTime.Now;

			// Now loop through all of the files in all of the inputs
			foreach (string input in inputs)
			{
				// If the input is a file
				if (File.Exists(input))
				{
					logger.Verbose("Checking file: '" + input + "'");
					RebuildToOutputHelper(input, outDir, tempDir, quickScan, date, delete, inverse,
						outputFormat, romba, archiveScanLevel, updateDat, headerToCheckAgainst, maxDegreeOfParallelism, logger);
				}

				// If the input is a directory
				else if (Directory.Exists(input))
				{
					logger.Verbose("Checking directory: '" + input + "'");
					foreach (string file in Directory.EnumerateFiles(input, "*", SearchOption.AllDirectories))
					{
						logger.Verbose("Checking file: '" + file + "'");
						RebuildToOutputHelper(file, outDir, tempDir, quickScan, date, delete, inverse,
							outputFormat, romba, archiveScanLevel, updateDat, headerToCheckAgainst, maxDegreeOfParallelism, logger);
					}
				}
			}

			logger.User("Rebuilding complete in: " + DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss\.fffff"));

			#endregion

			return success;
		}

		/// <summary>
		/// Attempt to add a file to the output if it matches
		/// </summary>
		/// <param name="file">Name of the file to process</param>
		/// <param name="outDir">Output directory to use to build to</param>
		/// <param name="tempDir">Temporary directory for archive extraction</param>
		/// <param name="quickScan">True to enable external scanning of archives, false otherwise</param>
		/// <param name="date">True if the date from the DAT should be used if available, false otherwise</param>
		/// <param name="delete">True if input files should be deleted, false otherwise</param>
		/// <param name="inverse">True if the DAT should be used as a filter instead of a template, false otherwise</param>
		/// <param name="outputFormat">Output format that files should be written to</param>
		/// <param name="romba">True if files should be output in Romba depot folders, false otherwise</param>
		/// <param name="archiveScanLevel">ArchiveScanLevel representing the archive handling levels</param>
		/// <param name="updateDat">True if the updated DAT should be output, false otherwise</param>
		/// <param name="headerToCheckAgainst">Populated string representing the name of the skipper to use, a blank string to use the first available checker, null otherwise</param>
		/// <param name="logger">Logger object for file and console output</param>
		private void RebuildToOutputHelper(string file, string outDir, string tempDir, bool quickScan, bool date,
			bool delete, bool inverse, OutputFormat outputFormat, bool romba, ArchiveScanLevel archiveScanLevel, bool updateDat,
			string headerToCheckAgainst, int maxDegreeOfParallelism, Logger logger)
		{
			// If we somehow have a null filename, return
			if (file == null)
			{
				return;
			}

			// Define the temporary directory
			string tempSubDir = Path.GetFullPath(Path.Combine(tempDir, Path.GetRandomFileName())) + Path.DirectorySeparatorChar;

			// Set the deletion variables
			bool usedExternally = false;
			bool usedInternally = false;

			// Get the required scanning level for the file
			bool shouldExternalProcess = false;
			bool shouldInternalProcess = false;
			ArchiveTools.GetInternalExternalProcess(file, archiveScanLevel, logger, out shouldExternalProcess, out shouldInternalProcess);

			// If we're supposed to scan the file externally
			if (shouldExternalProcess)
			{
				Rom rom = FileTools.GetFileInfo(file, logger, noMD5: quickScan, noSHA1: quickScan, header: headerToCheckAgainst);
				usedExternally = RebuildToOutputIndividual(rom, file, outDir, tempSubDir, date, inverse, outputFormat,
					romba, updateDat, false /* isZip */, headerToCheckAgainst, logger);
			}

			// If we're supposed to scan the file internally
			if (shouldInternalProcess)
			{
				// If quickscan is set, do so
				if (quickScan)
				{
					List<Rom> extracted = ArchiveTools.GetArchiveFileInfo(file, logger);
					usedInternally = true;

					foreach (Rom rom in extracted)
					{
						usedInternally &= RebuildToOutputIndividual(rom, file, outDir, tempSubDir, date, inverse, outputFormat,
							romba, updateDat, true /* isZip */, headerToCheckAgainst, logger);
					}
				}
				// Otherwise, attempt to extract the files to the temporary directory
				else
				{
					bool encounteredErrors = ArchiveTools.ExtractArchive(file, tempSubDir, archiveScanLevel, logger);

					// If the file was an archive and was extracted successfully, check it
					if (!encounteredErrors)
					{
						usedInternally = true;

						logger.Verbose(Path.GetFileName(file) + " treated like an archive");
						List<string> extracted = Directory.EnumerateFiles(tempSubDir, "*", SearchOption.AllDirectories).ToList();
						foreach (string entry in extracted)
						{
							Rom rom = FileTools.GetFileInfo(entry, logger, noMD5: quickScan, noSHA1: quickScan, header: headerToCheckAgainst);
							usedInternally &= RebuildToOutputIndividual(rom, entry, outDir, tempSubDir, date, inverse, outputFormat,
								romba, updateDat, false /* isZip */, headerToCheckAgainst, logger);
						}
					}
					// Otherwise, just get the info on the file itself
					else if (File.Exists(file))
					{
						Rom rom = FileTools.GetFileInfo(file, logger, noMD5: quickScan, noSHA1: quickScan, header: headerToCheckAgainst);
						usedExternally = RebuildToOutputIndividual(rom, file, outDir, tempSubDir, date, inverse, outputFormat,
							romba, updateDat, false /* isZip */, headerToCheckAgainst, logger);
					}
				}
			}

			// If we are supposed to delete the file, do so
			if (delete && (usedExternally || usedInternally))
			{
				try
				{
					logger.Verbose("Attempting to delete input file '" + file + "'");
					File.Delete(file);
					logger.Verbose("File '" + file + "' deleted");
				}
				catch (Exception ex)
				{
					logger.Error("An error occurred while trying to delete '" + file + "' " + ex.ToString());
				}
			}

			// Now delete the temp directory
			try
			{
				Directory.Delete(tempSubDir, true);
			}
			catch { }
		}

		/// <summary>
		/// Find duplicates and rebuild individual files to output
		/// </summary>
		/// <param name="rom">Information for the current file to rebuild from</param>
		/// <param name="file">Name of the file to process</param>
		/// <param name="outDir">Output directory to use to build to</param>
		/// <param name="tempDir">Temporary directory for archive extraction</param>
		/// <param name="date">True if the date from the DAT should be used if available, false otherwise</param>
		/// <param name="inverse">True if the DAT should be used as a filter instead of a template, false otherwise</param>
		/// <param name="outputFormat">Output format that files should be written to</param>
		/// <param name="romba">True if files should be output in Romba depot folders, false otherwise</param>
		/// <param name="updateDat">True if the updated DAT should be output, false otherwise</param>
		/// <param name="isZip">True if the input file is an archive, false otherwise</param>
		/// <param name="headerToCheckAgainst">Populated string representing the name of the skipper to use, a blank string to use the first available checker, null otherwise</param>
		/// <param name="logger">Logger object for file and console output</param>
		/// <returns>True if the file was able to be rebuilt, false otherwise</returns>
		private bool RebuildToOutputIndividual(Rom rom, string file, string outDir, string tempDir, bool date,
			bool inverse, OutputFormat outputFormat, bool romba, bool updateDat, bool isZip, string headerToCheckAgainst, Logger logger)
		{
			// Set the output value
			bool rebuilt = false;

			// Find if the file has duplicates in the DAT
			bool hasDuplicates = rom.HasDuplicates(this, logger);

			// If it has duplicates and we're not filtering, rebuild it
			if (hasDuplicates && !inverse)
			{
				// Get the list of duplicates to rebuild to
				List<DatItem> dupes = rom.GetDuplicates(this, logger, remove: updateDat);

				// If we don't have any duplicates, continue
				if (dupes.Count == 0)
				{
					return rebuilt;
				}

				// If we have an archive input, get the real name of the file to use
				if (isZip)
				{
					// Otherwise, extract the file to the temp folder
					file = ArchiveTools.ExtractItem(file, rom.Name, tempDir, logger);
				}				

				// If we couldn't extract the file, then continue,
				if (String.IsNullOrEmpty(file))
				{
					return rebuilt;
				}

				logger.User("Matches found for '" + file + "', rebuilding accordingly...");

				// Now loop through the list and rebuild accordingly
				foreach (Rom item in dupes)
				{
					rebuilt = true;

					switch (outputFormat)
					{
						case OutputFormat.Folder:
							string outfile = Path.Combine(outDir, Style.RemovePathUnsafeCharacters(item.Machine.Name), item.Name);

							// Make sure the output folder is created
							Directory.CreateDirectory(Path.GetDirectoryName(outfile));

							// Now copy the file over
							try
							{
								File.Copy(file, outfile);
								if (date && !String.IsNullOrEmpty(item.Date))
								{
									File.SetCreationTime(outfile, DateTime.Parse(item.Date));
								}

								rebuilt &= true;
							}
							catch
							{
								rebuilt = false;
							}

							break;
						case OutputFormat.TapeArchive:
							rebuilt &= ArchiveTools.WriteTAR(file, outDir, item, logger, date: date);
							break;
						case OutputFormat.Torrent7Zip:
							break;
						case OutputFormat.TorrentGzip:
							rebuilt &= ArchiveTools.WriteTorrentGZ(file, outDir, romba, logger);
							break;
						case OutputFormat.TorrentLrzip:
							break;
						case OutputFormat.TorrentRar:
							break;
						case OutputFormat.TorrentXZ:
							break;
						case OutputFormat.TorrentZip:
							rebuilt &= ArchiveTools.WriteTorrentZip(file, outDir, item, logger, date: date);
							break;
					}
				}

				// And now clear the temp folder to get rid of any transient files if we unzipped
				if (isZip)
				{
					try
					{
						Directory.Delete(tempDir, true);
					}
					catch { }
				}
			}

			// If we have no duplicates and we're filtering, rebuild it
			else if (!hasDuplicates && inverse)
			{
				string machinename = null;

				// If we have an archive input, get the real name of the file to use
				if (isZip)
				{
					// Otherwise, extract the file to the temp folder
					machinename = Style.GetFileNameWithoutExtension(file);
					file = ArchiveTools.ExtractItem(file, rom.Name, tempDir, logger);
				}

				// If we couldn't extract the file, then continue,
				if (String.IsNullOrEmpty(file))
				{
					return rebuilt;
				}

				// Get the item from the current file
				Rom item = FileTools.GetFileInfo(file, logger);
				item.Machine = new Machine()
				{
					Name = Style.GetFileNameWithoutExtension(item.Name),
					Description = Style.GetFileNameWithoutExtension(item.Name),
				};

				// If we are coming from an archive, set the correct machine name
				if (machinename != null)
				{
					item.Machine.Name = machinename;
					item.Machine.Description = machinename;
				}

				logger.User("Matches found for '" + file + "', rebuilding accordingly...");

				// Now rebuild to the output file
				switch (outputFormat)
				{
					case OutputFormat.Folder:
						string outfile = Path.Combine(outDir, Style.RemovePathUnsafeCharacters(item.Machine.Name), item.Name);

						// Make sure the output folder is created
						Directory.CreateDirectory(Path.GetDirectoryName(outfile));

						// Now copy the file over
						try
						{
							File.Copy(file, outfile);
							if (date && !String.IsNullOrEmpty(item.Date))
							{
								File.SetCreationTime(outfile, DateTime.Parse(item.Date));
							}

							rebuilt &= true;
						}
						catch
						{
							rebuilt &= false;
						}

						break;
					case OutputFormat.TapeArchive:
						rebuilt &= ArchiveTools.WriteTAR(file, outDir, item, logger, date: date);
						break;
					case OutputFormat.Torrent7Zip:
						break;
					case OutputFormat.TorrentGzip:
						rebuilt &= ArchiveTools.WriteTorrentGZ(file, outDir, romba, logger);
						break;
					case OutputFormat.TorrentLrzip:
						break;
					case OutputFormat.TorrentRar:
						break;
					case OutputFormat.TorrentXZ:
						break;
					case OutputFormat.TorrentZip:
						rebuilt &= ArchiveTools.WriteTorrentZip(file, outDir, item, logger, date: date);
						break;
				}

				// And now clear the temp folder to get rid of any transient files if we unzipped
				if (isZip)
				{
					try
					{
						Directory.Delete(tempDir, true);
					}
					catch { }
				}
			}

			return rebuilt;
		}

		/// <summary>
		/// Process the DAT and verify the output directory
		/// </summary>
		/// <param name="datFile">DAT to use to verify the directory</param>
		/// <param name="inputs">List of input directories to compare against</param>
		/// <param name="tempDir">Temporary directory for archive extraction</param>
		/// <param name="headerToCheckAgainst">Populated string representing the name of the skipper to use, a blank string to use the first available checker, null otherwise</param>
		/// <param name="logger">Logger object for file and console output</param>
		/// <returns>True if verification was a success, false otherwise</returns>
		public bool VerifyDirectory(List<string> inputs, string tempDir, string headerToCheckAgainst, Logger logger)
		{
			// First create or clean the temp directory
			if (!Directory.Exists(tempDir))
			{
				Directory.CreateDirectory(tempDir);
			}
			else
			{
				FileTools.CleanDirectory(tempDir);
			}

			bool success = true;

			/*
			We want the cross section of what's the folder and what's in the DAT. Right now, it just has what's in the DAT that's not in the folder
			*/

			// Then, loop through and check each of the inputs
			logger.User("Processing files:\n");
			foreach (string input in inputs)
			{
				PopulateFromDir(input, false /* noMD5 */, false /* noSHA1 */, true /* bare */, false /* archivesAsFiles */,
					true /* enableGzip */, false /* addBlanks */, false /* addDate */, tempDir /* tempDir */, false /* copyFiles */,
					headerToCheckAgainst, 4 /* maxDegreeOfParallelism */, logger);
			}

			// Setup the fixdat
			DatFile matched = new DatFile(this);
			matched.Reset();
			matched.FileName = "fixDat_" + matched.FileName;
			matched.Name = "fixDat_" + matched.Name;
			matched.Description = "fixDat_" + matched.Description;
			matched.DatFormat = DatFormat.Logiqx;

			// Now that all files are parsed, get only files found in directory
			bool found = false;
			foreach (string key in Keys)
			{
				List<DatItem> roms = this[key];
				List<DatItem> newroms = DatItem.Merge(roms, logger);
				foreach (Rom rom in newroms)
				{
					if (rom.SourceID == 99)
					{
						found = true;
						matched.Add(rom.Size + "-" + rom.CRC, rom);
					}
				}
			}

			// Now output the fixdat to the main folder
			if (found)
			{
				matched.WriteToFile("", logger, stats: true);
			}
			else
			{
				logger.User("No fixDat needed");
			}

			return success;
		}

		#endregion
	}
}
