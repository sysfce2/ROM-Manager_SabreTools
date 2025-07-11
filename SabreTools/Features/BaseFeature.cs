﻿using System.Collections.Generic;
using System.IO;
using SabreTools.Core.Filter;
using SabreTools.DatFiles;
using SabreTools.DatTools;
using SabreTools.FileTypes;
using SabreTools.Hashing;
using SabreTools.Help;
using SabreTools.IO.Logging;
using SabreTools.Reports;

namespace SabreTools.Features
{
    internal abstract class BaseFeature : TopLevel
    {
        #region Logging

        /// <summary>
        /// Logging object
        /// </summary>
        protected Logger _logger = new();

        #endregion

        #region Constructors

        public BaseFeature(string name, string flag, string description, string? longDescription = null)
            : base(name, flag, description, longDescription)
        {
        }

        public BaseFeature(string name, string[] flags, string description, string? longDescription = null)
            : base(name, flags, description, longDescription)
        {
        }

        #endregion

        #region Features

        #region Flag features

        internal const string AaruFormatsAsFilesValue = "aaruformats-as-files";
        internal static FlagFeature AaruFormatsAsFilesFlag => new(
            AaruFormatsAsFilesValue,
            ["-caf", "--aaruformats-as-files"],
            "Treat AaruFormats as files",
            longDescription: "Normally, AaruFormats would be processed using their internal hash to compare against the input DATs. This flag forces all AaruFormats to be treated like regular files.");

        internal const string AddBlankFilesValue = "add-blank-files";
        internal static FlagFeature AddBlankFilesFlag => new(
            AddBlankFilesValue,
            ["-ab", "--add-blank-files"],
            "Output blank files for folders",
            longDescription: "If this flag is set, then blank entries will be created for each of the empty directories in the source. This is useful for tools that require all folders be accounted for in the output DAT.");

        internal const string AddDateValue = "add-date";
        internal static FlagFeature AddDateFlag => new(
            AddDateValue,
            ["-ad", "--add-date"],
            "Add dates to items, where possible",
            longDescription: "If this flag is set, then the Date will be appended to each file information in the output DAT. The output format is standardized as \"yyyy/MM/dd HH:mm:ss\".");

        internal const string ArchivesAsFilesValue = "archives-as-files";
        internal static FlagFeature ArchivesAsFilesFlag => new(
            ArchivesAsFilesValue,
            ["-aaf", "--archives-as-files"],
            "Treat archives as files",
            longDescription: "Instead of trying to enumerate the files within archives, treat the archives as files themselves. This is good for uncompressed sets that include archives that should be read as-is.");

        internal const string BaddumpColumnValue = "baddump-column";
        internal static FlagFeature BaddumpColumnFlag => new(
            BaddumpColumnValue,
            ["-bc", "--baddump-column"],
            "Add baddump stats to output",
            longDescription: "Add a new column or field for counting the number of baddumps in the DAT.");

        internal const string BaseValue = "base";
        internal static FlagFeature BaseFlag => new(
            BaseValue,
            ["-ba", "--base"],
            "Use source DAT as base name for outputs",
            longDescription: "If splitting an entire folder of DATs, some output files may be normally overwritten since the names would be the same. With this flag, the original DAT name is used in the output name, in the format of \"Original Name(Dir - Name)\". This can be used in conjunction with --short to output in the format of \"Original Name (Name)\" instead.");

        internal const string BaseReplaceValue = "base-replace";
        internal static FlagFeature BaseReplaceFlag => new(
            BaseReplaceValue,
            ["-br", "--base-replace"],
            "Replace from base DATs in order",
            longDescription: "By default, no item names are changed except when there is a merge occurring. This flag enables users to define a DAT or set of base DATs to use as \"replacements\" for all input DATs. Note that the first found instance of an item in the base DAT(s) will be used and all others will be discarded. If no additional flag is given, it will default to updating names.");

        internal const string ByGameValue = "by-game";
        internal static FlagFeature ByGameFlag => new(
            ByGameValue,
            ["-bg", "--by-game"],
            "Diff against by game instead of hashes",
            longDescription: "By default, diffing against uses hashes to determine similar files. This flag enables using using each game as a comparision point instead.");

        internal const string ChdsAsFilesValue = "chds-as-files";
        internal static FlagFeature ChdsAsFilesFlag => new(
            ChdsAsFilesValue,
            ["-ic", "--chds-as-files"],
            "Treat CHDs as regular files",
            longDescription: "Normally, CHDs would be processed using their internal hash to compare against the input DATs. This flag forces all CHDs to be treated like regular files.");

        internal const string CleanValue = "clean";
        internal static FlagFeature CleanFlag => new(
            CleanValue,
            ["-clean", "--clean"],
            "Clean game names according to WoD standards",
            longDescription: "Game names will be sanitized to remove what the original WoD standards deemed as unneeded information, such as parenthesized or bracketed strings.");

        internal const string DatDeviceNonMergedValue = "dat-device-non-merged";
        internal static FlagFeature DatDeviceNonMergedFlag => new(
            DatDeviceNonMergedValue,
            ["-dnd", "--dat-device-non-merged"],
            "Create device non-merged sets",
            longDescription: "Preprocess the DAT to have child sets contain all items from the device references. This is incompatible with the other --dat-X flags.");

        internal const string DatFullMergedValue = "dat-full-merged";
        internal static FlagFeature DatFullMergedFlag => new(
            DatFullMergedValue,
            ["-dfm", "--dat-full-merged"],
            "Create fully merged sets",
            longDescription: "Preprocess the DAT to have parent sets contain all items from the children based on the cloneof tag while also performing deduplication within a parent. This is incompatible with the other --dat-X flags.");

        internal const string DatFullNonMergedValue = "dat-full-non-merged";
        internal static FlagFeature DatFullNonMergedFlag => new(
            DatFullNonMergedValue,
            ["-df", "--dat-full-non-merged"],
            "Create fully non-merged sets",
            longDescription: "Preprocess the DAT to have child sets contain all items from the parent sets based on the cloneof and romof tags as well as device references. This is incompatible with the other --dat-X flags.");

        internal const string DatMergedValue = "dat-merged";
        internal static FlagFeature DatMergedFlag => new(
            DatMergedValue,
            ["-dm", "--dat-merged"],
            "Force creating merged sets",
            longDescription: "Preprocess the DAT to have parent sets contain all items from the children based on the cloneof tag. This is incompatible with the other --dat-X flags.");

        internal const string DatNonMergedValue = "dat-non-merged";
        internal static FlagFeature DatNonMergedFlag => new(
            DatNonMergedValue,
            ["-dnm", "--dat-non-merged"],
            "Force creating non-merged sets",
            longDescription: "Preprocess the DAT to have child sets contain all items from the parent set based on the romof and cloneof tags. This is incompatible with the other --dat-X flags.");

        internal const string DatSplitValue = "dat-split";
        internal static FlagFeature DatSplitFlag => new(
            DatSplitValue,
            ["-ds", "--dat-split"],
            "Force creating split sets",
            longDescription: "Preprocess the DAT to remove redundant files between parents and children based on the romof and cloneof tags. This is incompatible with the other --dat-X flags.");

        internal const string DedupValue = "dedup";
        internal static FlagFeature DedupFlag => new(
            DedupValue,
            ["-dd", "--dedup"],
            "Enable deduping in the created DAT",
            longDescription: "For all outputted DATs, allow for hash deduping. This makes sure that there are effectively no duplicates in the output files. Cannot be used with game dedup.");

        internal const string DeleteValue = "delete";
        internal static FlagFeature DeleteFlag => new(
            DeleteValue,
            ["-del", "--delete"],
            "Delete fully rebuilt input files",
            longDescription: "Optionally, the input files, once processed and fully matched, can be deleted. This can be useful when the original file structure is no longer needed or if there is limited space on the source drive.");

        internal const string DepotValue = "depot";
        internal static FlagFeature DepotFlag => new(
            DepotValue,
            ["-dep", "--depot"],
            "Assume directories are Romba depots",
            longDescription: "Normally, input directories will be treated with no special format. If this flag is used, all input directories will be assumed to be Romba-style depots.");

        internal const string DeprecatedValue = "deprecated";
        internal static FlagFeature DeprecatedFlag => new(
            DeprecatedValue,
            ["-dpc", "--deprecated"],
            "Output 'game' instead of 'machine'",
            longDescription: "By default, Logiqx XML DATs output with the more modern \"machine\" tag for each set. This flag allows users to output the older \"game\" tag instead, for compatibility reasons. [Logiqx only]");

        internal const string DescriptionAsNameValue = "description-as-name";
        internal static FlagFeature DescriptionAsNameFlag => new(
            DescriptionAsNameValue,
            ["-dan", "--description-as-name"],
            "Use description instead of machine name",
            longDescription: "By default, all DATs are converted exactly as they are input. Enabling this flag allows for the machine names in the DAT to be replaced by the machine description instead. In most cases, this will result in no change in the output DAT, but a notable example would be a software list DAT where the machine names are generally DOS-friendly while the description is more complete.");

        internal const string DiffAgainstValue = "diff-against";
        internal static FlagFeature DiffAgainstFlag => new(
            DiffAgainstValue,
            ["-dag", "--diff-against"],
            "Diff all inputs against a set of base DATs",
            "This flag will enable a special type of diffing in which a set of base DATs are used as a comparison point for each of the input DATs. This allows users to get a slightly different output to cascaded diffing, which may be more useful in some cases. This is heavily influenced by the diffing model used by Romba.");

        internal const string DiffAllValue = "diff-all";
        internal static FlagFeature DiffAllFlag => new(
            DiffAllValue,
            ["-di", "--diff-all"],
            "Create diffdats from inputs (all standard outputs)",
            longDescription: "By default, all DATs are processed individually with the user-specified flags. With this flag enabled, input DATs are diffed against each other to find duplicates, no duplicates, and only in individuals.");

        internal const string DiffCascadeValue = "diff-cascade";
        internal static FlagFeature DiffCascadeFlag => new(
            DiffCascadeValue,
            ["-dc", "--diff-cascade"],
            "Enable cascaded diffing",
            longDescription: "This flag allows for a special type of diffing in which the first DAT is considered a base, and for each additional input DAT, it only leaves the files that are not in one of the previous DATs. This can allow for the creation of rollback sets or even just reduce the amount of duplicates across multiple sets.");

        internal const string DiffDuplicatesValue = "diff-duplicates";
        internal static FlagFeature DiffDuplicatesFlag => new(
            DiffDuplicatesValue,
            ["-did", "--diff-duplicates"],
            "Create diffdat containing just duplicates",
            longDescription: "All files that have duplicates outside of the original DAT are included.");

        internal const string DiffIndividualsValue = "diff-individuals";
        internal static FlagFeature DiffIndividualsFlag => new(
            DiffIndividualsValue,
            ["-dii", "--diff-individuals"],
            "Create diffdats for individual DATs",
            longDescription: "All files that have no duplicates outside of the original DATs are put into DATs that are named after the source DAT.");

        internal const string DiffNoDuplicatesValue = "diff-no-duplicates";
        internal static FlagFeature DiffNoDuplicatesFlag => new(
            DiffNoDuplicatesValue,
            ["-din", "--diff-no-duplicates"],
            "Create diffdat containing no duplicates",
            longDescription: "All files that have no duplicates outside of the original DATs are included.");

        internal const string DiffReverseCascadeValue = "diff-reverse-cascade";
        internal static FlagFeature DiffReverseCascadeFlag => new(
            DiffReverseCascadeValue,
            ["-drc", "--diff-reverse-cascade"],
            "Enable reverse cascaded diffing",
            longDescription: "This flag allows for a special type of diffing in which the last DAT is considered a base, and for each additional input DAT, it only leaves the files that are not in one of the previous DATs. This can allow for the creation of rollback sets or even just reduce the amount of duplicates across multiple sets.");

        internal const string ExtensionValue = "extension";
        internal static FlagFeature ExtensionFlag => new(
            ExtensionValue,
            ["-es", "--extension"],
            "Split DAT(s) by two file extensions",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on a list of input extensions. This can allow for combined DAT files, such as those combining two separate systems, to be split. Files with any extensions not listed in the input lists will be included in both outputted DAT files.");

        internal const string GameDedupValue = "game-dedup";
        internal static FlagFeature GameDedupFlag => new(
            GameDedupValue,
            ["-gdd", "--game-dedup"],
            "Enable deduping within games in the created DAT",
            longDescription: "For all outputted DATs, allow for hash deduping but only within the games, and not across the entire DAT. This makes sure that there are effectively no duplicates within each of the output sets. Cannot be used with standard dedup.");

        internal const string GamePrefixValue = "game-prefix";
        internal static FlagFeature GamePrefixFlag => new(
            GamePrefixValue,
            ["-gp", "--game-prefix"],
            "Add game name as a prefix",
            longDescription: "This flag allows for the name of the game to be used as a prefix to each file.");

        internal const string HashValue = "hash";
        internal static FlagFeature HashFlag => new(
            HashValue,
            ["-hs", "--hash"],
            "Split DAT(s) or folder by best-available hashes",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on the best available hash for each file within. The order of preference for the outputted DATs is as follows: Nodump, SHA-512, SHA-384, SHA-256, SHA-1, MD5, MD4, MD2, CRC (or worse).");

        internal const string HashOnlyValue = "hash-only";
        internal static FlagFeature HashOnlyFlag => new(
            HashOnlyValue,
            ["-ho", "--hash-only"],
            "Check files by hash only",
            longDescription: "This sets a mode where files are not checked based on name but rather hash alone. This allows verification of (possibly) incorrectly named folders and sets to be verified without worrying about the proper set structure to be there.");

        internal const string IncludeCrcValue = "include-crc";
        internal static FlagFeature IncludeCrcFlag => new(
            IncludeCrcValue,
            ["-crc", "--include-crc"],
            "Include CRC32 in output",
            longDescription: "This enables CRC32 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeMd2Value = "include-md2";
        internal static FlagFeature IncludeMd2Flag => new(
            IncludeMd2Value,
            ["-md2", "--include-md2"],
            "Include MD2 in output",
            longDescription: "This enables MD2 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeMd4Value = "include-md4";
        internal static FlagFeature IncludeMd4Flag => new(
            IncludeMd4Value,
            ["-md4", "--include-md4"],
            "Include MD4 in output",
            longDescription: "This enables MD4 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeMd5Value = "include-md5";
        internal static FlagFeature IncludeMd5Flag => new(
            IncludeMd5Value,
            ["-md5", "--include-md5"],
            "Include MD5 in output",
            longDescription: "This enables MD5 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeSha1Value = "include-sha1";
        internal static FlagFeature IncludeSha1Flag => new(
            IncludeSha1Value,
            ["-sha1", "--include-sha1"],
            "Include SHA-1 in output",
            longDescription: "This enables SHA-1 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeSha256Value = "include-sha256";
        internal static FlagFeature IncludeSha256Flag => new(
            IncludeSha256Value,
            ["-sha256", "--include-sha256"],
            "Include SHA-256 in output",
            longDescription: "This enables SHA-256 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeSha384Value = "include-sha384";
        internal static FlagFeature IncludeSha384Flag => new(
            IncludeSha384Value,
            ["-sha384", "--include-sha384"],
            "Include SHA-384 in output",
            longDescription: "This enables SHA-384 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeSha512Value = "include-sha512";
        internal static FlagFeature IncludeSha512Flag => new(
            IncludeSha512Value,
            ["-sha512", "--include-sha512"],
            "Include SHA-512 in output",
            longDescription: "This enables SHA-512 calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IncludeSpamSumValue = "include-spamsum";
        internal static FlagFeature IncludeSpamSumFlag => new(
            IncludeSpamSumValue,
            ["-spamsum", "--include-spamsum"],
            "Include SpamSum in output",
            longDescription: "This enables SpamSum calculation for each of the files. Adding this flag overrides the default hashing behavior of including CRC32, MD5, and SHA-1 hashes.");

        internal const string IndividualValue = "individual";
        internal static FlagFeature IndividualFlag => new(
            IndividualValue,
            ["-ind", "--individual"],
            "Process input DATs individually",
            longDescription: "In cases where DATs would be processed in bulk, this flag allows them to be processed on their own instead.");

        internal const string InplaceValue = "inplace";
        internal static FlagFeature InplaceFlag => new(
            InplaceValue,
            ["-ip", "--inplace"],
            "Write to the input directories, where possible",
            longDescription: "By default, files are written to the runtime directory (or the output directory, if set). This flag enables users to write out to the directory that the DATs originated from.");

        internal const string InverseValue = "inverse";
        internal static FlagFeature InverseFlag => new(
            InverseValue,
            ["-in", "--inverse"],
            "Rebuild only files not in DAT",
            longDescription: "Instead of the normal behavior of rebuilding using a DAT, this flag allows the user to use the DAT as a filter instead. All files that are found in the DAT will be skipped and everything else will be output in the selected format.");

        internal const string KeepEmptyGamesValue = "keep-empty-games";
        internal static FlagFeature KeepEmptyGamesFlag => new(
            KeepEmptyGamesValue,
            ["-keg", "--keep-empty-games"],
            "Keep originally empty sets from the input(s)",
            longDescription: "Normally, any sets that are considered empty will not be included in the output, this flag allows these empty sets to be added to the output.");

        internal const string LevelValue = "level";
        internal static FlagFeature LevelFlag => new(
            LevelValue,
            ["-ls", "--level"],
            "Split a SuperDAT or folder by lowest available level",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on the lowest available level of game name. That is, if a game name is top/mid/last, then it will create an output DAT for the parent directory \"mid\" in a folder called \"top\" with a game called \"last\".");

        internal const string MatchOfTagsValue = "match-of-tags";
        internal static FlagFeature MatchOfTagsFlag => new(
            MatchOfTagsValue,
            ["-ofg", "--match-of-tags"],
            "Allow cloneof and romof tags to match game name filters",
            longDescription: "If filter or exclude by game name is used, this flag will allow those filters to be checked against the romof and cloneof tags as well. This can allow for more advanced set-building, especially in arcade-based sets.");

        internal const string MergeValue = "merge";
        internal static FlagFeature MergeFlag => new(
            MergeValue,
            ["-m", "--merge"],
            "Merge the input DATs",
            longDescription: "By default, all DATs are processed individually with the user-specified flags. With this flag enabled, all of the input DATs are merged into a single output. This is best used with the dedup flag.");

        internal const string NoAutomaticDateValue = "no-automatic-date";
        internal static FlagFeature NoAutomaticDateFlag => new(
            NoAutomaticDateValue,
            ["-b", "--no-automatic-date"],
            "Don't include date in file name",
            longDescription: "Normally, the DAT will be created with the date in the file name in brackets. This flag removes that instead of the default.");

        internal const string NodumpColumnValue = "nodump-column";
        internal static FlagFeature NodumpColumnFlag => new(
            NodumpColumnValue,
            ["-nc", "--nodump-column"],
            "Add statistics for nodumps to output",
            longDescription: "Add a new column or field for counting the number of nodumps in the DAT.");

        internal const string OneGamePerRegionValue = "one-game-per-region";
        internal static FlagFeature OneGamePerRegionFlag => new(
            OneGamePerRegionValue,
            ["-1g1r", "--one-game-per-region"],
            "[EXPERIMENTAL] Try to ensure one game per user-defined region",
            longDescription: "This allows users to input a list of regions to use to filter on in order so only one game from each set of parent and clones will be included. This requires either cloneof or romof tags to function properly.");

        internal const string OneRomPerGameValue = "one-rom-per-game";
        internal static FlagFeature OneRomPerGameFlag => new(
            OneRomPerGameValue,
            ["-orpg", "--one-rom-per-game"],
            "Try to ensure each rom has its own game",
            longDescription: "In some cases, it is beneficial to have every rom put into its own output set as a subfolder of the original parent. This flag enables outputting each rom to its own game for this purpose.");

        internal const string OnlySameValue = "only-same";
        internal static FlagFeature OnlySameFlag => new(
            OnlySameValue,
            ["-ons", "--only-same"],
            "Only update description if machine name matches description",
            longDescription: "Normally, updating the description will always overwrite if the machine names are the same. With this flag, descriptions will only be overwritten if they are the same as the machine names.");

        internal const string QuickValue = "quick";
        internal static FlagFeature QuickFlag => new(
            QuickValue,
            ["-qs", "--quick"],
            "Enable quick scanning of archives",
            longDescription: "For all archives, if this flag is enabled, it will only use the header information to get the archive entries' file information. The upside to this is that it is the fastest option. On the downside, it can only get the CRC and size from most archive formats, leading to possible issues.");

        internal const string QuotesValue = "quotes";
        internal static FlagFeature QuotesFlag => new(
            QuotesValue,
            ["-q", "--quotes"],
            "Double-quote each item",
            longDescription: "This flag surrounds the item by double-quotes, not including the prefix or postfix.");

        internal const string RemoveExtensionsValue = "remove-extensions";
        internal static FlagFeature RemoveExtensionsFlag => new(
            RemoveExtensionsValue,
            ["-rme", "--remove-extensions"],
            "Remove all extensions from all items",
            longDescription: "For each item, remove the extension.");

        internal const string RemoveUnicodeValue = "remove-unicode";
        internal static FlagFeature RemoveUnicodeFlag => new(
            RemoveUnicodeValue,
            ["-ru", "--remove-unicode"],
            "Remove unicode characters from names",
            longDescription: "By default, the character set from the original file(s) will be used for item naming. This flag removes all Unicode characters from the item names, machine names, and machine descriptions.");

        internal const string ReverseBaseReplaceValue = "reverse-base-replace";
        internal static FlagFeature ReverseBaseReplaceFlag => new(
            ReverseBaseReplaceValue,
            ["-rbr", "--reverse-base-replace"],
            "Replace item names from base DATs in reverse",
            longDescription: "By default, no item names are changed except when there is a merge occurring. This flag enables users to define a DAT or set of base DATs to use as \"replacements\" for all input DATs. Note that the first found instance of an item in the last base DAT(s) will be used and all others will be discarded. If no additional flag is given, it will default to updating names.");

        internal const string RombaValue = "romba";
        internal static FlagFeature RombaFlag => new(
            RombaValue,
            ["-ro", "--romba"],
            "Treat like a Romba depot (requires SHA-1)",
            longDescription: "This flag allows reading and writing of DATs and output files to and from a Romba-style depot. This also implies TorrentGZ input and output for physical files. Where appropriate, Romba depot files will be created as well.");

        internal const string RomsValue = "roms";
        internal static FlagFeature RomsFlag => new(
            RomsValue,
            ["-r", "--roms"],
            "Output roms to miss instead of sets",
            longDescription: "By default, the outputted file will include the name of the game so this flag allows for the name of the rom to be output instead. [Missfile only]");

        internal const string SceneDateStripValue = "scene-date-strip";
        internal static FlagFeature SceneDateStripFlag => new(
            SceneDateStripValue,
            ["-sds", "--scene-date-strip"],
            "Remove date from scene-named sets",
            longDescription: "If this flag is enabled, sets with \"scene\" names will have the date removed from the beginning. For example \"01.01.01-Game_Name-GROUP\" would become \"Game_Name-Group\".");

        internal const string ScriptValue = "script";
        internal static FlagFeature ScriptFlag => new(
            ScriptValue,
            ["-sc", "--script"],
            "Enable script mode (no clear screen)",
            "For times when SabreTools is being used in a scripted environment, the user may not want the screen to be cleared every time that it is called. This flag allows the user to skip clearing the screen on run just like if the console was being redirected.");

        internal const string ShortValue = "short";
        internal static FlagFeature ShortFlag => new(
            ShortValue,
            ["-s", "--short"],
            "Use short output names",
            longDescription: "Instead of using ClrMamePro-style long names for DATs, use just the name of the folder as the name of the DAT. This can be used in conjunction with --base to output in the format of \"Original Name (Name)\" instead.");

        internal const string SingleSetValue = "single-set";
        internal static FlagFeature SingleSetFlag => new(
            SingleSetValue,
            ["-si", "--single-set"],
            "All game names replaced by '!'",
            longDescription: "This is useful for keeping all roms in a DAT in the same archive or folder.");

        internal const string SizeValue = "size";
        internal static FlagFeature SizeFlag => new(
            SizeValue,
            ["-szs", "--size"],
            "Split DAT(s) or folder by file sizes",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on the sizes of the files, specifically if the type is a Rom (most item types don't have sizes).");

        internal const string SkipArchivesValue = "skip-archives";
        internal static FlagFeature SkipArchivesFlag => new(
            SkipArchivesValue,
            ["-ska", "--skip-archives"],
            "Skip all archives",
            longDescription: "Skip any files that are treated like archives");

        internal const string SkipFilesValue = "skip-files";
        internal static FlagFeature SkipFilesFlag => new(
            SkipFilesValue,
            ["-skf", "--skip-files"],
            "Skip all non-archives",
            longDescription: "Skip any files that are not treated like archives");

        internal const string SkipFirstOutputValue = "skip-first-output";
        internal static FlagFeature SkipFirstOutputFlag => new(
            SkipFirstOutputValue,
            ["-sf", "--skip-first-output"],
            "Skip output of first DAT",
            longDescription: "In times where the first DAT does not need to be written out a second time, this will skip writing it. This can often speed up the output process.");

        // TODO: Should this just skip the item instead of the entire DAT?
        // The rationale behind skipping the entire DAT is that if one thing is missing, likely a lot more is missing
        // TDOO: Add to documentation
        internal const string StrictValue = "strict";
        internal static FlagFeature StrictFlag => new(
            StrictValue,
            ["-str", "--strict"],
            "Enable strict DAT creation",
            longDescription: "Instead of writing empty strings for null values when set as required, cancel writing the DAT entirely.");

        internal const string SuperdatValue = "superdat";
        internal static FlagFeature SuperdatFlag => new(
            SuperdatValue,
            ["-sd", "--superdat"],
            "Enable SuperDAT creation",
            longDescription: "Set the type flag to \"SuperDAT\" for the output DAT as well as preserving the directory structure of the inputted folder, if applicable.");

        internal const string TarValue = "tar";
        internal static FlagFeature TarFlag => new(
            TarValue,
            ["-tar", "--tar"],
            "Enable Tape ARchive output",
            longDescription: "Instead of outputting the files to folder, files will be rebuilt to Tape ARchive (TAR) files. This format is a standardized storage archive without any compression, usually used with other compression formats around it. It is widely used in backup applications and source code archives.");

        internal const string Torrent7zipValue = "torrent-7zip";
        internal static FlagFeature Torrent7zipFlag => new(
            Torrent7zipValue,
            ["-t7z", "--torrent-7zip"],
            "Enable Torrent7Zip output",
            longDescription: "Instead of outputting the files to folder, files will be rebuilt to Torrent7Zip (T7Z) files. This format is based on the LZMA container format 7Zip, but with custom header information. This is currently unused by any major application. Currently does not produce proper Torrent-compatible outputs.");

        internal const string TorrentGzipValue = "torrent-gzip";
        internal static FlagFeature TorrentGzipFlag => new(
            TorrentGzipValue,
            ["-tgz", "--torrent-gzip"],
            "Enable Torrent GZip output",
            longDescription: "Instead of outputting the files to folder, files will be rebuilt to TorrentGZ (TGZ) files. This format is based on the GZip archive format, but with custom header information and a file name replaced by the SHA-1 of the file inside. This is primarily used by external tool Romba (https://github.com/uwedeportivo/romba), but may be used more widely in the future.");

        internal const string TorrentZipValue = "torrent-zip";
        internal static FlagFeature TorrentZipFlag => new(
            TorrentZipValue,
            ["-tzip", "--torrent-zip"],
            "Enable Torrent Zip output",
            longDescription: "Instead of outputting files to folder, files will be rebuilt to TorrentZip (TZip) files. This format is based on the ZIP archive format, but with custom header information. This is primarily used by external tool RomVault (http://www.romvault.com/) and is already widely used.");

        internal const string TotalSizeValue = "total-size";
        internal static FlagFeature TotalSizeFlag => new(
            TotalSizeValue,
            ["-tis", "--total-size"],
            "Split DAT(s) or folder by total game sizes",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on the combined sizes of the games, splitting into individual chunks.");

        internal const string TrimValue = "trim";
        internal static FlagFeature TrimFlag => new(
            TrimValue,
            ["-trim", "--trim"],
            "Trim file names to fit NTFS length",
            longDescription: "In the cases where files will have too long a name, this allows for trimming the name of the files to the NTFS maximum length at most.");

        internal const string TypeValue = "type";
        internal static FlagFeature TypeFlag => new(
            TypeValue,
            ["-ts", "--type"],
            "Split DAT(s) or folder by file types (rom/disk)",
            longDescription: "For a DAT, or set of DATs, allow for splitting based on the types of the files, specifically if the type is a rom or a disk.");

        internal const string UpdateDatValue = "update-dat";
        internal static FlagFeature UpdateDatFlag => new(
            UpdateDatValue,
            ["-ud", "--update-dat"],
            "Output updated DAT to output directory",
            longDescription: "Once the files that were able to rebuilt are taken care of, a DAT of the files that could not be matched will be output to the output directory.");

        #endregion

        #region Int32 features

        internal const string DepotDepthInt32Value = "depot-depth";
        internal static Int32Feature DepotDepthInt32Input => new(
            DepotDepthInt32Value,
            ["-depd", "--depot-depth"],
            "Set depth of depot for inputs",
            longDescription: "Optionally, set the depth of input depots. Defaults to 4 deep otherwise.");

        internal const string RombaDepthInt32Value = "romba-depth";
        internal static Int32Feature RombaDepthInt32Input => new(
            RombaDepthInt32Value,
            ["-depr", "--romba-depth"],
            "Set depth of depot for outputs",
            longDescription: "Optionally, set the depth of output depots. Defaults to 4 deep otherwise.");

#if NET452_OR_GREATER || NETCOREAPP
        internal const string ThreadsInt32Value = "threads";
        internal static Int32Feature ThreadsInt32Input => new(
            ThreadsInt32Value,
            ["-mt", "--threads"],
            "Amount of threads to use (default = # cores)",
            longDescription: "Optionally, set the number of threads to use for the multithreaded operations. The default is the number of available machine threads; -1 means unlimited threads created.");
#endif

        #endregion

        #region Int64 features

        internal const string ChunkSizeInt64Value = "chunk-size";
        internal static Int64Feature ChunkSizeInt64Input => new(
            ChunkSizeInt64Value,
            ["-cs", "--chunk-size"],
            "Set a chunk size to output",
            longDescription: "Set the total game size to cut off at for each chunked DAT. It is recommended to use a sufficiently large size such as 1GB or else you may run into issues, especially if a single game could be larger than the size provided.");

        internal const string RadixInt64Value = "radix";
        internal static Int64Feature RadixInt64Input => new(
            RadixInt64Value,
            ["-rad", "--radix"],
            "Set the midpoint to split at",
            longDescription: "Set the size at which all roms less than the size are put in the first DAT, and everything greater than or equal goes in the second.");

        #endregion

        #region List<string> features

        internal const string BaseDatListValue = "base-dat";
        internal static ListFeature BaseDatListInput => new(
            BaseDatListValue,
            ["-bd", "--base-dat"],
            "Add a base DAT for processing",
            longDescription: "Add a DAT or folder of DATs to the base set to be used for all operations. Multiple instances of this flag are allowed.");

        internal const string DatListValue = "dat";
        internal static ListFeature DatListInput => new(
            DatListValue,
            ["-dat", "--dat"],
            "Input DAT to be used",
            longDescription: "User-supplied DAT for use in all operations. Multiple instances of this flag are allowed.");

        internal const string ExcludeFieldListValue = "exclude-field";
        internal static ListFeature ExcludeFieldListInput => new(
            ExcludeFieldListValue,
            ["-ef", "--exclude-field"],
            "Exclude a game/rom field from outputs",
            longDescription: "Exclude any valid item or machine field from outputs. Examples include: romof, publisher, and offset.");

        internal const string ExtAListValue = "exta";
        internal static ListFeature ExtaListInput => new(
            ExtAListValue,
            ["-exta", "--exta"],
            "Set extension to be included in first DAT",
            longDescription: "Set the extension to be used to populate the first DAT. Multiple instances of this flag are allowed.");

        internal const string ExtBListValue = "extb";
        internal static ListFeature ExtbListInput => new(
            ExtBListValue,
            ["-extb", "--extb"],
            "Set extension to be included in second DAT",
            longDescription: "Set the extension to be used to populate the second DAT. Multiple instances of this flag are allowed.");

        internal const string ExtraIniListValue = "extra-ini";
        internal static ListFeature ExtraIniListInput => new(
            ExtraIniListValue,
            ["-ini", "--extra-ini"],
            "Apply a MAME INI for given field(s)",
            longDescription: "Apply any valid MAME INI for any valid field in the DatFile. Inputs are of the form 'Field:path\\to\\ini'. Multiple instances of this flag are allowed.");

        internal const string FilterListValue = "filter";
        internal static ListFeature FilterListInput => new(
            FilterListValue,
            ["-fi", "--filter"],
            "Filter a game/rom field with the given value(s)",
            longDescription: "Filter any valid item or machine field from inputs. Filters are input in the form 'type.key=value' or 'type.key!=value', where the '!' signifies 'not matching'. Numeric values may also use extra operations, namely '>', '>=', '<', and '<='. Key examples include: item.romof, machine.category, and game.name. Additionally, the user can specify an exact match or full C#-style regex for pattern matching. Multiple instances of this flag are allowed.");

        internal const string OutputTypeListValue = "output-type";
        internal static ListFeature OutputTypeListInput => new(
            OutputTypeListValue,
            ["-ot", "--output-type"],
            "Output DATs to a specified format",
            longDescription: @"Add outputting the created DAT to known format. Multiple instances of this flag are allowed.

Possible values are:
    all              - All available DAT types
    ado, archive     - Archive.org file list
    am, attractmode  - AttractMode XML
    cmp, clrmamepro  - ClrMamePro
    csv              - Standardized Comma-Separated Value
    dc, doscenter    - DOSCenter
    lr, listrom      - MAME Listrom
    lx, listxml      - MAME Listxml
    miss, missfile   - GoodTools Missfile
    md2              - MD2
    md4              - MD4
    md5              - MD5
    msx, openmsx     - openMSX Software List
    ol, offlinelist  - OfflineList XML
    rc, romcenter    - RomCenter
    sj, sabrejson    - SabreJSON
    sx, sabrexml     - SabreDAT XML
    sfv              - SFV
    sha1             - SHA1
    sha256           - SHA256
    sha384           - SHA384
    sha512           - SHA512
    smdb, everdrive  - Everdrive SMDB
    sl, softwarelist - MAME Software List XML
    spamsum          - SpamSum
    ssv              - Standardized Semicolon-Separated Value
    tsv              - Standardized Tab-Separated Value
    xml, logiqx      - Logiqx XML");

        internal const string RegionListValue = "region";
        internal static ListFeature RegionListInput => new(
            RegionListValue,
            ["-reg", "--region"],
            "Add a region for 1G1R",
            longDescription: "Add a region (in order) for use with 1G1R filtering. If this is not supplied, then by default, only parent sets will be included in the output. Multiple instances of this flag are allowed.");

        internal const string ReportTypeListValue = "report-type";
        internal static ListFeature ReportTypeListInput => new(
            ReportTypeListValue,
            ["-srt", "--report-type"],
            "Output statistics to a specified format",
            longDescription: @"Add outputting the created DAT to known format. Multiple instances of this flag are allowed.

Possible values are:
    all              - All available DAT types
    csv              - Standardized Comma-Separated Value
    html             - HTML webpage
    ssv              - Standardized Semicolon-Separated Value
    text             - Generic textfile
    tsv              - Standardized Tab-Separated Value");

        internal const string UpdateFieldListValue = "update-field";
        internal static ListFeature UpdateFieldListInput => new(
            UpdateFieldListValue,
            ["-uf", "--update-field"],
            "Update a game/rom field from base DATs",
            longDescription: "Update any valid item or machine field from base DAT(s). Examples include: romof, publisher, and offset.");

        #endregion

        #region String features

        internal const string AddExtensionStringValue = "add-extension";
        internal static StringFeature AddExtensionStringInput => new(
            AddExtensionStringValue,
            ["-ae", "--add-extension"],
            "Add an extension to each item",
            longDescription: "Add a postfix extension to each full item name.");

        internal const string AuthorStringValue = "author";
        internal static StringFeature AuthorStringInput => new(
            AuthorStringValue,
            ["-au", "--author"],
            "Set the author of the DAT",
            longDescription: "Set the author header field for the output DAT(s)");

        internal const string CategoryStringValue = "category";
        internal static StringFeature CategoryStringInput => new(
            CategoryStringValue,
            ["-c", "--category"],
            "Set the category of the DAT",
            longDescription: "Set the category header field for the output DAT(s)");

        internal const string CommentStringValue = "comment";
        internal static StringFeature CommentStringInput => new(
            CommentStringValue,
            ["-co", "--comment"],
            "Set a new comment of the DAT",
            longDescription: "Set the comment header field for the output DAT(s)");

        internal const string DateStringValue = "date";
        internal static StringFeature DateStringInput => new(
            DateStringValue,
            ["-da", "--date"],
            "Set a new date",
            longDescription: "Set the date header field for the output DAT(s)");

        internal const string DescriptionStringValue = "description";
        internal static StringFeature DescriptionStringInput => new(
            DescriptionStringValue,
            ["-de", "--description"],
            "Set the description of the DAT",
            longDescription: "Set the description header field for the output DAT(s)");

        internal const string EmailStringValue = "email";
        internal static StringFeature EmailStringInput => new(
            EmailStringValue,
            ["-em", "--email"],
            "Set a new email of the DAT",
            longDescription: "Set the email header field for the output DAT(s)");

        internal const string FilenameStringValue = "filename";
        internal static StringFeature FilenameStringInput => new(
            FilenameStringValue,
            ["-f", "--filename"],
            "Set the external name of the DAT",
            longDescription: "Set the external filename for the output DAT(s)");

        internal const string ForceMergingStringValue = "forcemerging";
        internal static StringFeature ForceMergingStringInput => new(
            ForceMergingStringValue,
            ["-fm", "--forcemerging"],
            "Set force merging",
            longDescription: @"Set the forcemerging tag to the given value.
Possible values are: None, Split, Device, Merged, Nonmerged, Full");

        internal const string ForceNodumpStringValue = "forcenodump";
        internal static StringFeature ForceNodumpStringInput => new(
            ForceNodumpStringValue,
            ["-fn", "--forcenodump"],
            "Set force nodump",
            longDescription: @"Set the forcenodump tag to the given value.
Possible values are: None, Obsolete, Required, Ignore");

        internal const string ForcePackingStringValue = "forcepacking";
        internal static StringFeature ForcePackingStringInput => new(
            ForcePackingStringValue,
            ["-fp", "--forcepacking"],
            "Set force packing",
            longDescription: @"Set the forcepacking tag to the given value.
Possible values are: None, Zip, Unzip, Partial, Flat");

        internal const string HeaderStringValue = "header";
        internal static StringFeature HeaderStringInput => new(
            HeaderStringValue,
            ["-h", "--header"],
            "Set a header skipper to use, blank means all",
            longDescription: "Set the header special field for the output DAT(s). In file rebuilding, this flag allows for either all copier headers (using \"\") or specific copier headers by name (such as \"fds.xml\") to determine if a file matches or not.");

        internal const string HomepageStringValue = "homepage";
        internal static StringFeature HomepageStringInput => new(
            HomepageStringValue,
            ["-hp", "--homepage"],
            "Set a new homepage of the DAT",
            longDescription: "Set the homepage header field for the output DAT(s)");

        internal const string LogLevelStringValue = "log-level";
        internal static StringFeature LogLevelStringInput => new(
            LogLevelStringValue,
            ["-ll", "--log-level"],
            "Set the lowest log level for output",
            longDescription: @"Set the lowest log level for output.
Possible values are: None, Verbose, User, Warning, Error");

        internal const string NameStringValue = "name";
        internal static StringFeature NameStringInput => new(
            NameStringValue,
            ["-n", "--name"],
            "Set the internal name of the DAT",
            longDescription: "Set the name header field for the output DAT(s)");

        internal const string OutputDirStringValue = "output-dir";
        internal static StringFeature OutputDirStringInput => new(
            OutputDirStringValue,
            ["-out", "--output-dir"],
            "Set output directory",
            longDescription: "This sets an output folder to be used when the files are created. If a path is not defined, the runtime directory is used instead.");

        internal const string PostfixStringValue = "postfix";
        internal static StringFeature PostfixStringInput => new(
            PostfixStringValue,
            ["-post", "--postfix"],
            "Set postfix for all lines",
            longDescription: @"Set a generic postfix to be appended to all outputted lines.

Some special strings that can be used:
- %game% / %machine% - Replaced with the Game/Machine name
- %name% - Replaced with the Rom name
- %manufacturer% - Replaced with game Manufacturer
- %publisher% - Replaced with game Publisher
- %category% - Replaced with game Category
- %crc% - Replaced with the CRC
- %md2% - Replaced with the MD2
- %md4% - Replaced with the MD4
- %md5% - Replaced with the MD5
- %sha1% - Replaced with the SHA-1
- %sha256% - Replaced with the SHA-256
- %sha384% - Replaced with the SHA-384
- %sha512% - Replaced with the SHA-512
- %size% - Replaced with the size");

        internal const string PrefixStringValue = "prefix";
        internal static StringFeature PrefixStringInput => new(
            PrefixStringValue,
            ["-pre", "--prefix"],
            "Set prefix for all lines",
            longDescription: @"Set a generic prefix to be prepended to all outputted lines.

Some special strings that can be used:
- %game% / %machine% - Replaced with the Game/Machine name
- %name% - Replaced with the Rom name
- %manufacturer% - Replaced with game Manufacturer
- %publisher% - Replaced with game Publisher
- %category% - Replaced with game Category
- %crc% - Replaced with the CRC
- %md2% - Replaced with the MD2
- %md4% - Replaced with the MD4
- %md5% - Replaced with the MD5
- %sha1% - Replaced with the SHA-1
- %sha256% - Replaced with the SHA-256
- %sha384% - Replaced with the SHA-384
- %sha512% - Replaced with the SHA-512
- %size% - Replaced with the size");

        internal const string ReplaceExtensionStringValue = "replace-extension";
        internal static StringFeature ReplaceExtensionStringInput => new(
            ReplaceExtensionStringValue,
            ["-rep", "--replace-extension"],
            "Replace all extensions with specified",
            longDescription: "When an extension exists, replace it with the provided instead.");

        internal const string RootStringValue = "root";
        internal static StringFeature RootStringInput => new(
            RootStringValue,
            ["-r", "--root"],
            "Set a new rootdir",
            longDescription: "Set the rootdir (as used by SuperDAT mode) for the output DAT(s).");

        internal const string RootDirStringValue = "root-dir";
        internal static StringFeature RootDirStringInput => new(
            RootDirStringValue,
            ["-rd", "--root-dir"],
            "Set the root directory for calc",
            longDescription: "In the case that the files will not be stored from the root directory, a new root can be set for path length calculations.");

        internal const string UrlStringValue = "url";
        internal static StringFeature UrlStringInput => new(
            UrlStringValue,
            ["-u", "--url"],
            "Set a new URL of the DAT",
            longDescription: "Set the URL header field for the output DAT(s)");

        internal const string VersionStringValue = "version";
        internal static StringFeature VersionStringInput => new(
            VersionStringValue,
            ["-v", "--version"],
            "Set the version of the DAT",
            longDescription: "Set the version header field for the output DAT(s)");

        #endregion

        #endregion

        #region Fields

        /// <summary>
        /// Preconfigured Cleaner
        /// </summary>
        protected Cleaner? Cleaner { get; set; }

        /// <summary>
        /// Preconfigured ExtraIni set
        /// </summary>
        protected ExtraIni? Extras { get; set; }

        /// <summary>
        /// Preonfigured FilterRunner
        /// </summary>
        protected FilterRunner? FilterRunner { get; set; }

        /// <summary>
        /// Pre-configured DatHeader
        /// </summary>
        /// <remarks>Public because it's an indicator something went wrong</remarks>
        public DatHeader? Header { get; set; }

        /// <summary>
        /// Pre-configured DatModifiers
        /// </summary>
        /// <remarks>Public because it's an indicator something went wrong</remarks>
        protected DatModifiers? Modifiers { get; set; }

        /// <summary>
        /// Output directory
        /// </summary>
        protected string? OutputDir { get; set; }

        /// <summary>
        /// Pre-configured Remover
        /// </summary>
        protected Remover? Remover { get; set; }

        /// <summary>
        /// Determines if scripting mode is enabled
        /// </summary>
        public bool ScriptMode { get; protected set; }

        /// <summary>
        /// Pre-configured Splitter
        /// </summary>
        protected MergeSplit? Splitter { get; set; }

        #endregion

        #region Add Feature Groups

        /// <summary>
        /// Add common features
        /// </summary>
        protected void AddCommonFeatures()
        {
            AddFeature(ScriptFlag);
            AddFeature(LogLevelStringInput);
#if NET452_OR_GREATER || NETCOREAPP
            AddFeature(ThreadsInt32Input);
#endif
        }

        /// <summary>
        /// Add Filter-specific features
        /// </summary>
        protected void AddFilteringFeatures()
        {
            AddFeature(FilterListInput);
            AddFeature(MatchOfTagsFlag);
            //AddFeature(PerMachineFlag); // TODO: Add and implement this flag
        }

        /// <summary>
        /// Add Header-specific features
        /// </summary>
        protected void AddHeaderFeatures()
        {
            // Header Values
            AddFeature(FilenameStringInput);
            AddFeature(NameStringInput);
            AddFeature(DescriptionStringInput);
            AddFeature(RootStringInput);
            AddFeature(CategoryStringInput);
            AddFeature(VersionStringInput);
            AddFeature(DateStringInput);
            AddFeature(AuthorStringInput);
            AddFeature(EmailStringInput);
            AddFeature(HomepageStringInput);
            AddFeature(UrlStringInput);
            AddFeature(CommentStringInput);
            AddFeature(HeaderStringInput);
            AddFeature(SuperdatFlag);
            AddFeature(ForceMergingStringInput);
            AddFeature(ForceNodumpStringInput);
            AddFeature(ForcePackingStringInput);

            // Header Filters
            AddFeature(ExcludeFieldListInput);
            AddFeature(OneGamePerRegionFlag);
            this[OneGamePerRegionFlag]!.AddFeature(RegionListInput);
            AddFeature(OneRomPerGameFlag);
            AddFeature(SceneDateStripFlag);
        }

        /// <summary>
        /// Add internal split/merge features
        /// </summary>
        protected void AddInternalSplitFeatures()
        {
            AddFeature(DatMergedFlag);
            AddFeature(DatFullMergedFlag);
            AddFeature(DatSplitFlag);
            AddFeature(DatNonMergedFlag);
            AddFeature(DatDeviceNonMergedFlag);
            AddFeature(DatFullNonMergedFlag);
        }

        #endregion

        public override bool ProcessFeatures(Dictionary<string, Feature?> features)
        {
            // Generic feature flags
            Cleaner = GetCleaner(features);
            Extras = GetExtras(features);
            FilterRunner = GetFilterRunner(features);
            Header = GetDatHeader(features);
            Modifiers = GetDatModifiers(features);
            OutputDir = GetString(features, OutputDirStringValue)?.Trim('"');
            Remover = GetRemover(features);
            ScriptMode = GetBoolean(features, ScriptValue);
            Splitter = GetSplitter(features);

            // Handle logging levels
            string? logLevel = GetString(features, LogLevelStringValue);
            if (logLevel?.ToLowerInvariant() == "none")
            {
                LoggerImpl.LowestLogLevel = LogLevel.VERBOSE;
                // Do not set a filename to avoid writing to a file
            }
            else
            {
                LoggerImpl.LowestLogLevel = logLevel.AsLogLevel();
                LoggerImpl.SetFilename(GetLogFilename(), true);
            }

            // Setup default logging
            LoggerImpl.AppendPrefix = true;
            LoggerImpl.ThrowOnError = false;
            LoggerImpl.Start();

            // Set threading flag, if necessary
#if NET452_OR_GREATER || NETCOREAPP
            if (features.ContainsKey(ThreadsInt32Value))
                Core.Globals.MaxThreads = GetInt32(features, ThreadsInt32Value);
#endif

            // Failure conditions
            if (Header == null)
                return false;
            if (Modifiers == null)
                return false;

            return true;
        }

        /// <summary>
        /// Generate the required log filename for output
        /// </summary>
        private static string GetLogFilename()
        {
#if NET20 || NET35 || NET40 || NET452
            string runtimeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
#else
            string runtimeDir = System.AppContext.BaseDirectory;
#endif

#if NET20 || NET35
            return Path.Combine(Path.Combine(runtimeDir, "logs"), "sabretools.log");
#else
            return Path.Combine(runtimeDir, "logs", "sabretools.log");
#endif
        } 

        #region Protected Specific Extraction

        /// <summary>
        /// Get include from scan from feature list
        /// </summary>
        protected static HashType[] GetIncludeInScan(Dictionary<string, Feature?> features)
        {
            List<HashType> includeInScan = [];

            if (GetBoolean(features, IncludeCrcValue))
                includeInScan.Add(HashType.CRC32);
            if (GetBoolean(features, IncludeMd2Value))
                includeInScan.Add(HashType.MD2);
            if (GetBoolean(features, IncludeMd4Value))
                includeInScan.Add(HashType.MD4);
            if (GetBoolean(features, IncludeMd5Value))
                includeInScan.Add(HashType.MD5);
            if (GetBoolean(features, IncludeSha1Value))
                includeInScan.Add(HashType.SHA1);
            if (GetBoolean(features, IncludeSha256Value))
                includeInScan.Add(HashType.SHA256);
            if (GetBoolean(features, IncludeSha384Value))
                includeInScan.Add(HashType.SHA384);
            if (GetBoolean(features, IncludeSha512Value))
                includeInScan.Add(HashType.SHA512);
            if (GetBoolean(features, IncludeSpamSumValue))
                includeInScan.Add(HashType.SpamSum);

            // Fallback to "Standard" if no flags are set
            if (includeInScan.Count == 0)
                includeInScan = [HashType.CRC32, HashType.MD5, HashType.SHA1];

            return [.. includeInScan];
        }

        /// <summary>
        /// Get OutputFormat from feature list
        /// </summary>
        protected static OutputFormat GetOutputFormat(Dictionary<string, Feature?> features)
        {
            if (GetBoolean(features, TarValue))
                return OutputFormat.TapeArchive;
            else if (GetBoolean(features, Torrent7zipValue))
                return OutputFormat.Torrent7Zip;
            else if (GetBoolean(features, TorrentGzipValue))
                return OutputFormat.TorrentGzip;
            //else if (GetBoolean(features, SharedTorrentRarValue))
            //    return OutputFormat.TorrentRar;
            //else if (GetBoolean(features, SharedTorrentXzValue))
            //    return OutputFormat.TorrentXZ;
            else if (GetBoolean(features, TorrentZipValue))
                return OutputFormat.TorrentZip;
            else
                return OutputFormat.Folder;
        }

        /// <summary>
        /// Get SkipFileType from feature list
        /// </summary>
        protected static SkipFileType GetSkipFileType(Dictionary<string, Feature?> features)
        {
            if (GetBoolean(features, SkipArchivesValue))
                return SkipFileType.Archive;
            else if (GetBoolean(features, SkipFilesValue))
                return SkipFileType.File;
            else
                return SkipFileType.None;
        }

        /// <summary>
        /// Get SplittingMode from feature list
        /// </summary>
        protected static SplittingMode GetSplittingMode(Dictionary<string, Feature?> features)
        {
            SplittingMode splittingMode = SplittingMode.None;

            if (GetBoolean(features, ExtensionValue))
                splittingMode |= SplittingMode.Extension;
            if (GetBoolean(features, HashValue))
                splittingMode |= SplittingMode.Hash;
            if (GetBoolean(features, LevelValue))
                splittingMode |= SplittingMode.Level;
            if (GetBoolean(features, SizeValue))
                splittingMode |= SplittingMode.Size;
            if (GetBoolean(features, TotalSizeValue))
                splittingMode |= SplittingMode.TotalSize;
            if (GetBoolean(features, TypeValue))
                splittingMode |= SplittingMode.Type;

            return splittingMode;
        }

        /// <summary>
        /// Get StatReportFormat from feature list
        /// </summary>
        protected static StatReportFormat GetStatReportFormat(Dictionary<string, Feature?> features)
        {
            StatReportFormat statDatFormat = StatReportFormat.None;

            foreach (string rt in GetList(features, ReportTypeListValue))
            {
                statDatFormat |= GetStatReportFormat(rt);
            }

            return statDatFormat;
        }

        /// <summary>
        /// Get TreatAsFile from feature list
        /// </summary>
        protected static TreatAsFile GetTreatAsFile(Dictionary<string, Feature?> features)
        {
            TreatAsFile asFile = 0x00;
            if (GetBoolean(features, AaruFormatsAsFilesValue))
                asFile |= TreatAsFile.AaruFormat;
            if (GetBoolean(features, ArchivesAsFilesValue))
                asFile |= TreatAsFile.Archive;
            if (GetBoolean(features, ChdsAsFilesValue))
                asFile |= TreatAsFile.CHD;

            return asFile;
        }

        /// <summary>
        /// Get update Machine fields from feature list
        /// </summary>
        protected static List<string> GetUpdateMachineFields(Dictionary<string, Feature?> features)
        {
            List<string> updateFields = [];
            foreach (string fieldName in GetList(features, UpdateFieldListValue))
            {
                // Ensure the field is valid
                try
                {
                    var key = new FilterKey(fieldName);
                    if (key.ItemName != Models.Metadata.MetadataFile.MachineKey)
                        continue;

                    updateFields.Add(key.FieldName);
                }
                catch { }
            }

            return updateFields;
        }

        /// <summary>
        /// Get update DatItem fields from feature list
        /// </summary>
        protected static Dictionary<string, List<string>> GetUpdateDatItemFields(Dictionary<string, Feature?> features)
        {
            Dictionary<string, List<string>> updateFields = [];
            foreach (string fieldName in GetList(features, UpdateFieldListValue))
            {
                // Ensure the field is valid
                try
                {
                    var key = new FilterKey(fieldName);
                    if (key.ItemName == Models.Metadata.MetadataFile.HeaderKey || key.ItemName == Models.Metadata.MetadataFile.MachineKey)
                        continue;

                    if (!updateFields.ContainsKey(key.ItemName))
                        updateFields[key.ItemName] = [];

                    updateFields[key.ItemName].Add(key.FieldName);
                }
                catch { }
            }

            return updateFields;
        }

        /// <summary>
        /// Get UpdateMode from feature list
        /// </summary>
        protected static UpdateMode GetUpdateMode(Dictionary<string, Feature?> features)
        {
            UpdateMode updateMode = UpdateMode.None;

            if (GetBoolean(features, DiffAllValue))
                updateMode |= UpdateMode.AllDiffs;

            if (GetBoolean(features, BaseReplaceValue))
                updateMode |= UpdateMode.BaseReplace;

            if (GetBoolean(features, DiffAgainstValue))
                updateMode |= UpdateMode.DiffAgainst;

            if (GetBoolean(features, DiffCascadeValue))
                updateMode |= UpdateMode.DiffCascade;

            if (GetBoolean(features, DiffDuplicatesValue))
                updateMode |= UpdateMode.DiffDupesOnly;

            if (GetBoolean(features, DiffIndividualsValue))
                updateMode |= UpdateMode.DiffIndividualsOnly;

            if (GetBoolean(features, DiffNoDuplicatesValue))
                updateMode |= UpdateMode.DiffNoDupesOnly;

            if (GetBoolean(features, DiffReverseCascadeValue))
                updateMode |= UpdateMode.DiffReverseCascade;

            if (GetBoolean(features, MergeValue))
                updateMode |= UpdateMode.Merge;

            if (GetBoolean(features, ReverseBaseReplaceValue))
                updateMode |= UpdateMode.ReverseBaseReplace;

            return updateMode;
        }

        #endregion

        #region Private Specific Extraction

        /// <summary>
        /// Get Cleaner from feature list
        /// </summary>
        private static Cleaner GetCleaner(Dictionary<string, Feature?> features)
        {
            Cleaner cleaner = new()
            {
                Normalize = GetBoolean(features, CleanValue),
                DedupeRoms = GetDedupeType(features),
                DescriptionAsName = GetBoolean(features, DescriptionAsNameValue),
                KeepEmptyGames = GetBoolean(features, KeepEmptyGamesValue),
                OneGamePerRegion = GetBoolean(features, OneGamePerRegionValue),
                RegionList = GetList(features, RegionListValue),
                OneRomPerGame = GetBoolean(features, OneRomPerGameValue),
                RemoveUnicode = GetBoolean(features, RemoveUnicodeValue),
                Root = GetString(features, RootDirStringValue),
                SceneDateStrip = GetBoolean(features, SceneDateStripValue),
                Single = GetBoolean(features, SingleSetValue),
                Trim = GetBoolean(features, TrimValue),
            };

            return cleaner;
        }

        /// <summary>
        /// Get DatHeader from feature list
        /// </summary>
        private DatHeader? GetDatHeader(Dictionary<string, Feature?> features)
        {
            var datHeader = new DatHeader();
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.AuthorKey, GetString(features, AuthorStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.CategoryKey, GetString(features, CategoryStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.CommentKey, GetString(features, CommentStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.DateKey, GetString(features, DateStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.DescriptionKey, GetString(features, DescriptionStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.EmailKey, GetString(features, EmailStringValue));
            datHeader.SetFieldValue<string?>(DatHeader.FileNameKey, GetString(features, FilenameStringValue));
            datHeader.SetFieldValue<MergingFlag>(Models.Metadata.Header.ForceMergingKey, GetString(features, ForceMergingStringValue).AsMergingFlag());
            datHeader.SetFieldValue<NodumpFlag>(Models.Metadata.Header.ForceNodumpKey, GetString(features, ForceNodumpStringValue).AsNodumpFlag());
            datHeader.SetFieldValue<PackingFlag>(Models.Metadata.Header.ForceNodumpKey, GetString(features, ForcePackingStringValue).AsPackingFlag());
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.HeaderKey, GetString(features, HeaderStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.HomepageKey, GetString(features, HomepageStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.NameKey, GetString(features, NameStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.RootDirKey, GetString(features, RootStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.TypeKey, GetBoolean(features, SuperdatValue) ? "SuperDAT" : null);
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.UrlKey, GetString(features, UrlStringValue));
            datHeader.SetFieldValue<string?>(Models.Metadata.Header.VersionKey, GetString(features, VersionStringValue));

            bool deprecated = GetBoolean(features, DeprecatedValue);
            foreach (string ot in GetList(features, OutputTypeListValue))
            {
                DatFormat dftemp = GetDatFormat(ot);
                if (dftemp == 0x00)
                {
                    _logger.Error($"{ot} is not a recognized DAT format");
                    return null;
                }

                // Handle deprecated Logiqx
                DatFormat currentFormat = datHeader.GetFieldValue<DatFormat>(DatHeader.DatFormatKey);
                if (dftemp == DatFormat.Logiqx && deprecated)
                    datHeader.SetFieldValue(DatHeader.DatFormatKey, currentFormat | DatFormat.LogiqxDeprecated);
                else
                    datHeader.SetFieldValue(DatHeader.DatFormatKey, currentFormat | dftemp);
            }

            return datHeader;
        }

        /// <summary>
        /// Get DatModifiers from feature list
        /// </summary>
        private DatModifiers? GetDatModifiers(Dictionary<string, Feature?> features)
        {
            // Get the depot information
            var inputDepot = new DepotInformation(
                GetBoolean(features, DepotValue),
                GetInt32(features, DepotDepthInt32Value));
            var outputDepot = new DepotInformation(
                GetBoolean(features, RombaValue),
                GetInt32(features, RombaDepthInt32Value));

            var datModifiers = new DatModifiers();

            datModifiers.Prefix = GetString(features, PrefixStringValue);
            datModifiers.Postfix = GetString(features, PostfixStringValue);
            datModifiers.AddExtension = GetString(features, AddExtensionStringValue);
            datModifiers.RemoveExtension = GetBoolean(features, RemoveExtensionsValue);
            datModifiers.ReplaceExtension = GetString(features, ReplaceExtensionStringValue);
            datModifiers.GameName = GetBoolean(features, GamePrefixValue);
            datModifiers.Quotes = GetBoolean(features, QuotesValue);
            datModifiers.UseRomName = GetBoolean(features, RomsValue);
            datModifiers.InputDepot = inputDepot;
            datModifiers.OutputDepot = outputDepot;

            return datModifiers;
        }

        /// <summary>
        /// Get DedupeType from feature list
        /// </summary>
        private static DedupeType GetDedupeType(Dictionary<string, Feature?> features)
        {
            if (GetBoolean(features, DedupValue))
                return DedupeType.Full;
            else if (GetBoolean(features, GameDedupValue))
                return DedupeType.Game;
            else
                return DedupeType.None;
        }

        /// <summary>
        /// Get ExtraIni from feature list
        /// </summary>
        private static ExtraIni GetExtras(Dictionary<string, Feature?> features)
        {
            ExtraIni extraIni = new();
            extraIni.PopulateFromList(GetList(features, ExtraIniListValue));
            return extraIni;
        }

        /// <summary>
        /// Get FilterRunner from feature list
        /// </summary>
        private static FilterRunner GetFilterRunner(Dictionary<string, Feature?> features)
        {
            // Populate filters
            List<string> filterPairs = GetList(features, FilterListValue);

            // Include 'of" in game filters
            bool matchOfTags = GetBoolean(features, MatchOfTagsValue);
            if (matchOfTags)
            {
                // TODO: Support this use case somehow
            }

            var filterRunner = new FilterRunner(filterPairs.ToArray());

            return filterRunner;
        }

        /// <summary>
        /// Get Remover from feature list
        /// </summary>
        private static Remover GetRemover(Dictionary<string, Feature?> features)
        {
            Remover remover = new();

            // Populate field exclusions
            List<string> exclusionFields = GetList(features, ExcludeFieldListValue);
            remover.PopulateExclusionsFromList(exclusionFields);

            return remover;
        }

        /// <summary>
        /// Get Splitter from feature list
        /// </summary>
        private static MergeSplit GetSplitter(Dictionary<string, Feature?> features)
        {
            MergeSplit splitter = new()
            {
                SplitType = GetSplitType(features),
            };
            return splitter;
        }

        /// <summary>
        /// Get SplitType from feature list
        /// </summary>
        private static MergingFlag GetSplitType(Dictionary<string, Feature?> features)
        {
            MergingFlag splitType = MergingFlag.None;
            if (GetBoolean(features, DatDeviceNonMergedValue))
                splitType = MergingFlag.DeviceNonMerged;
            else if (GetBoolean(features, DatFullMergedValue))
                splitType = MergingFlag.FullMerged;
            else if (GetBoolean(features, DatFullNonMergedValue))
                splitType = MergingFlag.FullNonMerged;
            else if (GetBoolean(features, DatMergedValue))
                splitType = MergingFlag.Merged;
            else if (GetBoolean(features, DatNonMergedValue))
                splitType = MergingFlag.NonMerged;
            else if (GetBoolean(features, DatSplitValue))
                splitType = MergingFlag.Split;

            return splitType;
        }

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Get DatFormat value from input string
        /// </summary>
        /// <param name="input">String to get value from</param>
        /// <returns>DatFormat value corresponding to the string</returns>
        protected static DatFormat GetDatFormat(string input)
        {
            return (input?.Trim().ToLowerInvariant()) switch
            {
                "all" => DatFormat.ALL,
                "ado" or "archive" => DatFormat.ArchiveDotOrg,
                "am" or "attractmode" => DatFormat.AttractMode,
                "cmp" or "clrmamepro" => DatFormat.ClrMamePro,
                "csv" => DatFormat.CSV,
                "dc" or "doscenter" => DatFormat.DOSCenter,
                "everdrive" or "smdb" => DatFormat.EverdriveSMDB,
                "json" or "sj" or "sabrejson" => DatFormat.SabreJSON,
                "lr" or "listrom" => DatFormat.Listrom,
                "lx" or "listxml" => DatFormat.Listxml,
                "md2" => DatFormat.RedumpMD2,
                "md4" => DatFormat.RedumpMD4,
                "md5" => DatFormat.RedumpMD5,
                "miss" or "missfile" => DatFormat.MissFile,
                "msx" or "openmsx" => DatFormat.OpenMSX,
                "ol" or "offlinelist" => DatFormat.OfflineList,
                "rc" or "romcenter" => DatFormat.RomCenter,
                "sd" or "sabredat" or "sx" or "sabrexml" => DatFormat.SabreXML,
                "sfv" => DatFormat.RedumpSFV,
                "sha1" => DatFormat.RedumpSHA1,
                "sha256" => DatFormat.RedumpSHA256,
                "sha384" => DatFormat.RedumpSHA384,
                "sha512" => DatFormat.RedumpSHA512,
                "sl" or "softwarelist" => DatFormat.SoftwareList,
                "spamsum" => DatFormat.RedumpSpamSum,
                "ssv" => DatFormat.SSV,
                "tsv" => DatFormat.TSV,
                "xml" or "logiqx" => DatFormat.Logiqx,
                _ => 0x0,
            };
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Get StatReportFormat value from input string
        /// </summary>
        /// <param name="input">String to get value from</param>
        /// <returns>StatReportFormat value corresponding to the string</returns>
        private static StatReportFormat GetStatReportFormat(string input)
        {
            return input?.Trim().ToLowerInvariant() switch
            {
                "all" => StatReportFormat.All,
                "csv" => StatReportFormat.CSV,
                "html" => StatReportFormat.HTML,
                "ssv" => StatReportFormat.SSV,
                "text" => StatReportFormat.Textfile,
                "tsv" => StatReportFormat.TSV,
                _ => 0x0,
            };
        }

        #endregion
    }
}
