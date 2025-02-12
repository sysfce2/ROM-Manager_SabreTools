﻿using System.Collections.Generic;
using SabreTools.DatFiles;
using SabreTools.DatItems;
using SabreTools.DatTools;
using SabreTools.Hashing;
using SabreTools.Help;
using SabreTools.IO;
using SabreTools.IO.Logging;

namespace SabreTools.Features
{
    internal class Verify : BaseFeature
    {
        public const string Value = "Verify";

        public Verify()
        {
            Name = Value;
            Flags.AddRange(["ve", "verify"]);
            Description = "Verify a folder against DATs";
            _featureType = ParameterType.Flag;
            LongDescription = "When used, this will use an input DAT or set of DATs to blindly check against an input folder. The base of the folder is considered the base for the combined DATs and games are either the directories or archives within. This will only do a direct verification of the items within and will create a fixdat afterwards for missing files.";

            // Common Features
            AddCommonFeatures();

            AddFeature(DatListInput);
            AddFeature(DepotFlag);
            this[DepotFlag]!.AddFeature(DepotDepthInt32Input);
            AddFeature(OutputDirStringInput);
            AddFeature(HashOnlyFlag);
            AddFeature(QuickFlag);
            AddFeature(HeaderStringInput);
            AddFeature(AaruFormatsAsFilesFlag);
            AddFeature(ChdsAsFilesFlag);
            AddFeature(IndividualFlag);
            AddInternalSplitFeatures();
            AddFeature(ExtraIniListInput);
            AddFilteringFeatures();
        }

        public override bool ProcessFeatures(Dictionary<string, Feature?> features)
        {
            // If the base fails, just fail out
            if (!base.ProcessFeatures(features))
                return false;

            // Get a list of files from the input datfiles
            var datfiles = GetList(features, DatListValue);
            var datfilePaths = PathTool.GetFilesOnly(datfiles);

            // Get feature flags
            TreatAsFile asFile = GetTreatAsFile(features);
            bool hashOnly = GetBoolean(features, HashOnlyValue);
            bool quickScan = GetBoolean(features, QuickValue);
            HashType[] hashes = quickScan ? [HashType.CRC32] : [HashType.CRC32, HashType.MD5, HashType.SHA1];
            var dfd = new DatTools.DatFromDir(hashes, SkipFileType.None, addBlanks: false);

            // If we are in individual mode, process each DAT on their own
            if (GetBoolean(features, IndividualValue))
            {
                foreach (ParentablePath datfile in datfilePaths)
                {
                    // Parse in from the file
                    DatFile datdata = DatFileTool.CreateDatFile();
                    Parser.ParseInto(datdata, datfile.CurrentPath, int.MaxValue, keep: true);

                    // Perform additional processing steps
                    Extras!.ApplyExtras(datdata);
                    Extras!.ApplyExtrasDB(datdata);
                    Splitter!.ApplySplitting(datdata, useTags: true);
                    datdata.ExecuteFilters(FilterRunner!);
                    Cleaner!.ApplyCleaning(datdata);
                    Remover!.ApplyRemovals(datdata);

                    // Set depot information
                    var inputDepot = Modifiers!.InputDepot;
                    datdata.Modifiers.InputDepot = inputDepot?.Clone() as DepotInformation;

                    // If we have overridden the header skipper, set it now
                    string? headerSkipper = Header!.GetStringFieldValue(Models.Metadata.Header.HeaderKey);
                    if (!string.IsNullOrEmpty(headerSkipper))
                        datdata.Header.SetFieldValue<string?>(Models.Metadata.Header.HeaderKey, headerSkipper);

                    // If we have the depot flag, respect it
                    if (inputDepot?.IsActive ?? false)
                    {
                        Verification.VerifyDepot(datdata, Inputs);
                    }
                    else
                    {
                        // Loop through and add the inputs to check against
                        _logger.User("Processing files:\n");
                        foreach (string input in Inputs)
                        {
                            dfd.PopulateFromDir(datdata, input, asFile);
                        }

                        Verification.VerifyGeneric(datdata, hashOnly);
                        //Verification.VerifyGenericDB(datdata, hashOnly);
                    }

                    // Now write out if there are any items left
                    Writer.WriteStatsToConsole(datdata);
                    Writer.Write(datdata, OutputDir!);
                }
            }
            // Otherwise, process all DATs into the same output
            else
            {
                var watch = new InternalStopwatch("Populating internal DAT");

                // Add all of the input DATs into one huge internal DAT
                DatFile datdata = DatFileTool.CreateDatFile();
                foreach (ParentablePath datfile in datfilePaths)
                {
                    Parser.ParseInto(datdata, datfile.CurrentPath, int.MaxValue, keep: true);
                }

                // Perform additional processing steps
                Extras!.ApplyExtras(datdata);
                Extras!.ApplyExtrasDB(datdata);
                Splitter!.ApplySplitting(datdata, useTags: true);
                datdata.ExecuteFilters(FilterRunner!);
                Cleaner!.ApplyCleaning(datdata);
                Remover!.ApplyRemovals(datdata);

                // Set depot information
                var inputDepot = Modifiers!.InputDepot;
                datdata.Modifiers.InputDepot = inputDepot?.Clone() as DepotInformation;

                // If we have overridden the header skipper, set it now
                string? headerSkipper = Header!.GetStringFieldValue(Models.Metadata.Header.HeaderKey);
                if (!string.IsNullOrEmpty(headerSkipper))
                    datdata.Header.SetFieldValue<string?>(Models.Metadata.Header.HeaderKey, headerSkipper);

                watch.Stop();

                // If we have the depot flag, respect it
                if (inputDepot?.IsActive ?? false)
                {
                    Verification.VerifyDepot(datdata, Inputs);
                }
                else
                {
                    // Loop through and add the inputs to check against
                    _logger.User("Processing files:\n");
                    foreach (string input in Inputs)
                    {
                        dfd.PopulateFromDir(datdata, input, asFile);
                    }

                    Verification.VerifyGeneric(datdata, hashOnly);
                    //Verification.VerifyGenericDB(datdata, hashOnly);
                }

                // Now write out if there are any items left
                Writer.WriteStatsToConsole(datdata);
                Writer.Write(datdata, OutputDir!);
            }

            return true;
        }
    }
}
