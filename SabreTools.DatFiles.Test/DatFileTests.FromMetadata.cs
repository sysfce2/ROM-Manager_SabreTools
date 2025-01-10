using System;
using System.Linq;
using Xunit;

namespace SabreTools.DatFiles.Test
{
    public partial class DatFileTests
    {
        #region ConvertFromMetadata

        [Fact]
        public void ConvertFromMetadata_Null()
        {
            Models.Metadata.MetadataFile? item = null;

            DatFile datFile = new Formats.Logiqx(null, deprecated: false);
            datFile.ConvertFromMetadata(item, "filename", indexId: 0, keep: true, statsOnly: false);

            Assert.Empty(datFile.Items);
            Assert.Empty(datFile.ItemsDB.GetItems());
        }

        [Fact]
        public void ConvertFromMetadata_Empty()
        {
            Models.Metadata.MetadataFile? item = new Models.Metadata.MetadataFile();

            DatFile datFile = new Formats.Logiqx(null, deprecated: false);
            datFile.ConvertFromMetadata(item, "filename", indexId: 0, keep: true, statsOnly: false);

            Assert.Empty(datFile.Items);
            Assert.Empty(datFile.ItemsDB.GetItems());
        }

        [Fact]
        public void ConvertFromMetadata_FilledHeader()
        {
            Models.OfflineList.CanOpen canOpen = new Models.OfflineList.CanOpen
            {
                Extension = ["ext"],
            };

            Models.OfflineList.Images images = new Models.OfflineList.Images();

            Models.OfflineList.Infos infos = new Models.OfflineList.Infos();

            Models.OfflineList.NewDat newDat = new Models.OfflineList.NewDat();

            Models.OfflineList.Search search = new Models.OfflineList.Search();

            Models.Metadata.Header? header = new Models.Metadata.Header
            {
                [Models.Metadata.Header.AuthorKey] = "author",
                [Models.Metadata.Header.BiosModeKey] = "merged",
                [Models.Metadata.Header.BuildKey] = "build",
                [Models.Metadata.Header.CanOpenKey] = canOpen,
                [Models.Metadata.Header.CategoryKey] = "category",
                [Models.Metadata.Header.CommentKey] = "comment",
                [Models.Metadata.Header.DateKey] = "date",
                [Models.Metadata.Header.DatVersionKey] = "datversion",
                [Models.Metadata.Header.DebugKey] = "yes",
                [Models.Metadata.Header.DescriptionKey] = "description",
                [Models.Metadata.Header.EmailKey] = "email",
                [Models.Metadata.Header.EmulatorVersionKey] = "emulatorversion",
                [Models.Metadata.Header.ForceMergingKey] = "merged",
                [Models.Metadata.Header.ForceNodumpKey] = "required",
                [Models.Metadata.Header.ForcePackingKey] = "zip",
                [Models.Metadata.Header.ForceZippingKey] = "yes",
                [Models.Metadata.Header.HeaderKey] = "header",
                [Models.Metadata.Header.HomepageKey] = "homepage",
                [Models.Metadata.Header.IdKey] = "id",
                [Models.Metadata.Header.ImagesKey] = images,
                [Models.Metadata.Header.ImFolderKey] = "imfolder",
                [Models.Metadata.Header.InfosKey] = infos,
                [Models.Metadata.Header.LockBiosModeKey] = "yes",
                [Models.Metadata.Header.LockRomModeKey] = "yes",
                [Models.Metadata.Header.LockSampleModeKey] = "yes",
                [Models.Metadata.Header.MameConfigKey] = "mameconfig",
                [Models.Metadata.Header.NameKey] = "name",
                [Models.Metadata.Header.NewDatKey] = newDat,
                [Models.Metadata.Header.NotesKey] = "notes",
                [Models.Metadata.Header.PluginKey] = "plugin",
                [Models.Metadata.Header.RefNameKey] = "refname",
                [Models.Metadata.Header.RomModeKey] = "merged",
                [Models.Metadata.Header.RomTitleKey] = "romtitle",
                [Models.Metadata.Header.RootDirKey] = "rootdir",
                [Models.Metadata.Header.SampleModeKey] = "merged",
                [Models.Metadata.Header.SchemaLocationKey] = "schemalocation",
                [Models.Metadata.Header.ScreenshotsHeightKey] = "screenshotsheight",
                [Models.Metadata.Header.ScreenshotsWidthKey] = "screenshotsWidth",
                [Models.Metadata.Header.SearchKey] = search,
                [Models.Metadata.Header.SystemKey] = "system",
                [Models.Metadata.Header.TimestampKey] = "timestamp",
                [Models.Metadata.Header.TypeKey] = "type",
                [Models.Metadata.Header.UrlKey] = "url",
                [Models.Metadata.Header.VersionKey] = "version",
            };

            Models.Metadata.Machine[]? machines = null;

            Models.Metadata.MetadataFile? item = new Models.Metadata.MetadataFile
            {
                [Models.Metadata.MetadataFile.HeaderKey] = header,
                [Models.Metadata.MetadataFile.MachineKey] = machines,
            };

            DatFile datFile = new Formats.Logiqx(null, deprecated: false);
            datFile.ConvertFromMetadata(item, "filename", indexId: 0, keep: true, statsOnly: false);

            DatHeader datHeader = datFile.Header;
            Assert.Equal("author", datHeader.GetStringFieldValue(Models.Metadata.Header.AuthorKey));
            Assert.Equal("merged", datHeader.GetStringFieldValue(Models.Metadata.Header.BiosModeKey));
            Assert.Equal("build", datHeader.GetStringFieldValue(Models.Metadata.Header.BuildKey));
            Assert.Equal("ext", datHeader.GetStringFieldValue(Models.Metadata.Header.CanOpenKey));
            Assert.Equal("category", datHeader.GetStringFieldValue(Models.Metadata.Header.CategoryKey));
            Assert.Equal("comment", datHeader.GetStringFieldValue(Models.Metadata.Header.CommentKey));
            Assert.Equal("date", datHeader.GetStringFieldValue(Models.Metadata.Header.DateKey));
            Assert.Equal("datversion", datHeader.GetStringFieldValue(Models.Metadata.Header.DatVersionKey));
            Assert.True(datHeader.GetBoolFieldValue(Models.Metadata.Header.DebugKey));
            Assert.Equal("description", datHeader.GetStringFieldValue(Models.Metadata.Header.DescriptionKey));
            Assert.Equal("email", datHeader.GetStringFieldValue(Models.Metadata.Header.EmailKey));
            Assert.Equal("emulatorversion", datHeader.GetStringFieldValue(Models.Metadata.Header.EmulatorVersionKey));
            Assert.Equal("merged", datHeader.GetStringFieldValue(Models.Metadata.Header.ForceMergingKey));
            Assert.Equal("required", datHeader.GetStringFieldValue(Models.Metadata.Header.ForceNodumpKey));
            Assert.Equal("zip", datHeader.GetStringFieldValue(Models.Metadata.Header.ForcePackingKey));
            Assert.True(datHeader.GetBoolFieldValue(Models.Metadata.Header.ForceZippingKey));
            Assert.Equal("header", datHeader.GetStringFieldValue(Models.Metadata.Header.HeaderKey));
            Assert.Equal("homepage", datHeader.GetStringFieldValue(Models.Metadata.Header.HomepageKey));
            Assert.Equal("id", datHeader.GetStringFieldValue(Models.Metadata.Header.IdKey));
            Assert.NotNull(datHeader.GetStringFieldValue(Models.Metadata.Header.ImagesKey));
            Assert.Equal("imfolder", datHeader.GetStringFieldValue(Models.Metadata.Header.ImFolderKey));
            Assert.NotNull(datHeader.GetStringFieldValue(Models.Metadata.Header.InfosKey));
            Assert.True(datHeader.GetBoolFieldValue(Models.Metadata.Header.LockBiosModeKey));
            Assert.True(datHeader.GetBoolFieldValue(Models.Metadata.Header.LockRomModeKey));
            Assert.True(datHeader.GetBoolFieldValue(Models.Metadata.Header.LockSampleModeKey));
            Assert.Equal("mameconfig", datHeader.GetStringFieldValue(Models.Metadata.Header.MameConfigKey));
            Assert.Equal("name", datHeader.GetStringFieldValue(Models.Metadata.Header.NameKey));
            Assert.NotNull(datHeader.GetStringFieldValue(Models.Metadata.Header.NewDatKey));
            Assert.Equal("notes", datHeader.GetStringFieldValue(Models.Metadata.Header.NotesKey));
            Assert.Equal("plugin", datHeader.GetStringFieldValue(Models.Metadata.Header.PluginKey));
            Assert.Equal("refname", datHeader.GetStringFieldValue(Models.Metadata.Header.RefNameKey));
            Assert.Equal("merged", datHeader.GetStringFieldValue(Models.Metadata.Header.RomModeKey));
            Assert.Equal("romtitle", datHeader.GetStringFieldValue(Models.Metadata.Header.RomTitleKey));
            Assert.Equal("rootdir", datHeader.GetStringFieldValue(Models.Metadata.Header.RootDirKey));
            Assert.Equal("merged", datHeader.GetStringFieldValue(Models.Metadata.Header.SampleModeKey));
            Assert.Equal("schemalocation", datHeader.GetStringFieldValue(Models.Metadata.Header.SchemaLocationKey));
            Assert.Equal("screenshotsheight", datHeader.GetStringFieldValue(Models.Metadata.Header.ScreenshotsHeightKey));
            Assert.Equal("screenshotsWidth", datHeader.GetStringFieldValue(Models.Metadata.Header.ScreenshotsWidthKey));
            Assert.NotNull(datHeader.GetStringFieldValue(Models.Metadata.Header.SearchKey));
            Assert.Equal("system", datHeader.GetStringFieldValue(Models.Metadata.Header.SystemKey));
            Assert.Equal("timestamp", datHeader.GetStringFieldValue(Models.Metadata.Header.TimestampKey));
            Assert.Equal("type", datHeader.GetStringFieldValue(Models.Metadata.Header.TypeKey));
            Assert.Equal("url", datHeader.GetStringFieldValue(Models.Metadata.Header.UrlKey));
            Assert.Equal("version", datHeader.GetStringFieldValue(Models.Metadata.Header.VersionKey));
        }

        [Fact]
        public void ConvertFromMetadata_FilledMachine()
        {
            Models.Metadata.Header? header = null;

            // Used by multiple items
            Models.Metadata.Condition condition = new Models.Metadata.Condition
            {
                [Models.Metadata.Condition.ValueKey] = "value",
                [Models.Metadata.Condition.MaskKey] = "mask",
                [Models.Metadata.Condition.RelationKey] = "eq",
                [Models.Metadata.Condition.TagKey] = "tag",
            };

            Models.Metadata.Adjuster adjuster = new Models.Metadata.Adjuster
            {
                [Models.Metadata.Adjuster.ConditionKey] = condition,
                [Models.Metadata.Adjuster.DefaultKey] = true,
                [Models.Metadata.Adjuster.NameKey] = "name",
            };

            Models.Metadata.Archive archive = new Models.Metadata.Archive
            {
                [Models.Metadata.Archive.NameKey] = "name",
            };

            Models.Metadata.BiosSet biosSet = new Models.Metadata.BiosSet
            {
                [Models.Metadata.BiosSet.DefaultKey] = true,
                [Models.Metadata.BiosSet.DescriptionKey] = "description",
                [Models.Metadata.BiosSet.NameKey] = "name",
            };

            Models.Metadata.Machine machine = new Models.Metadata.Machine
            {
                [Models.Metadata.Machine.AdjusterKey] = new Models.Metadata.Adjuster[] { adjuster },
                [Models.Metadata.Machine.ArchiveKey] = new Models.Metadata.Archive[] { archive },
                [Models.Metadata.Machine.BiosSetKey] = new Models.Metadata.BiosSet[] { biosSet },
                [Models.Metadata.Machine.BoardKey] = "board",
                [Models.Metadata.Machine.ButtonsKey] = "buttons",
                [Models.Metadata.Machine.CategoryKey] = "category",
                [Models.Metadata.Machine.ChipKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.CloneOfKey] = "cloneof",
                [Models.Metadata.Machine.CloneOfIdKey] = "cloneofid",
                [Models.Metadata.Machine.CommentKey] = "comment",
                [Models.Metadata.Machine.CompanyKey] = "company",
                [Models.Metadata.Machine.ConfigurationKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.ControlKey] = "control",
                [Models.Metadata.Machine.CountryKey] = "country",
                [Models.Metadata.Machine.DescriptionKey] = "description",
                [Models.Metadata.Machine.DeviceKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DeviceRefKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DipSwitchKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DirNameKey] = "dirname",
                [Models.Metadata.Machine.DiskKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DisplayCountKey] = "displaycount",
                [Models.Metadata.Machine.DisplayKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DisplayTypeKey] = "displaytype",
                [Models.Metadata.Machine.DriverKey] = "REPLACE", // Type
                [Models.Metadata.Machine.DumpKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.DuplicateIDKey] = "duplicateid",
                [Models.Metadata.Machine.EmulatorKey] = "emulator",
                [Models.Metadata.Machine.ExtraKey] = "extra",
                [Models.Metadata.Machine.FavoriteKey] = "favorite",
                [Models.Metadata.Machine.FeatureKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.GenMSXIDKey] = "genmsxid",
                [Models.Metadata.Machine.HistoryKey] = "history",
                [Models.Metadata.Machine.IdKey] = "id",
                [Models.Metadata.Machine.Im1CRCKey] = "deadbeef",
                [Models.Metadata.Machine.Im2CRCKey] = "deadbeef",
                [Models.Metadata.Machine.ImageNumberKey] = "imagenumber",
                [Models.Metadata.Machine.InfoKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.InputKey] = "REPLACE", // Type
                [Models.Metadata.Machine.IsBiosKey] = "yes",
                [Models.Metadata.Machine.IsDeviceKey] = "yes",
                [Models.Metadata.Machine.IsMechanicalKey] = "yes",
                [Models.Metadata.Machine.LanguageKey] = "language",
                [Models.Metadata.Machine.LocationKey] = "location",
                [Models.Metadata.Machine.ManufacturerKey] = "manufacturer",
                [Models.Metadata.Machine.MediaKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.NameKey] = "name",
                [Models.Metadata.Machine.NotesKey] = "notes",
                [Models.Metadata.Machine.PartKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.PlayedCountKey] = "playedcount",
                [Models.Metadata.Machine.PlayedTimeKey] = "playedtime",
                [Models.Metadata.Machine.PlayersKey] = "players",
                [Models.Metadata.Machine.PortKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.PublisherKey] = "publisher",
                [Models.Metadata.Machine.RamOptionKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.RebuildToKey] = "rebuildto",
                [Models.Metadata.Machine.ReleaseKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.ReleaseNumberKey] = "releasenumber",
                [Models.Metadata.Machine.RomKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.RomOfKey] = "romof",
                [Models.Metadata.Machine.RotationKey] = "rotation",
                [Models.Metadata.Machine.RunnableKey] = "yes",
                [Models.Metadata.Machine.SampleKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.SampleOfKey] = "sampleof",
                [Models.Metadata.Machine.SaveTypeKey] = "savetype",
                [Models.Metadata.Machine.SharedFeatKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.SlotKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.SoftwareListKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.SoundKey] = "REPLACE", // Type
                [Models.Metadata.Machine.SourceFileKey] = "sourcefile",
                [Models.Metadata.Machine.SourceRomKey] = "sourcerom",
                [Models.Metadata.Machine.StatusKey] = "status",
                [Models.Metadata.Machine.SupportedKey] = "yes",
                [Models.Metadata.Machine.SystemKey] = "system",
                [Models.Metadata.Machine.TagsKey] = "tags",
                [Models.Metadata.Machine.TruripKey] = "REPLACE", // Type
                [Models.Metadata.Machine.VideoKey] = "REPLACE", // Type array
                [Models.Metadata.Machine.YearKey] = "year",
            };

            Models.Metadata.Machine[]? machines = [machine];

            // TODO: Build a machine with one of every item

            Models.Metadata.MetadataFile? item = new Models.Metadata.MetadataFile
            {
                [Models.Metadata.MetadataFile.HeaderKey] = header,
                [Models.Metadata.MetadataFile.MachineKey] = machines,
            };

            DatFile datFile = new Formats.Logiqx(null, deprecated: false);
            datFile.ConvertFromMetadata(item, "filename", indexId: 0, keep: true, statsOnly: false);

            DatItems.Machine actualMachine = Assert.Single(datFile.ItemsDB.GetMachines()).Value;

            Assert.Equal("board", actualMachine.GetStringFieldValue(Models.Metadata.Machine.BoardKey));
            Assert.Equal("buttons", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ButtonsKey));
            Assert.Equal("category", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CategoryKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ChipKey)); // Type array
            Assert.Equal("cloneof", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CloneOfKey));
            Assert.Equal("cloneofid", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CloneOfIdKey));
            Assert.Equal("comment", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CommentKey));
            Assert.Equal("company", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CompanyKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ConfigurationKey)); // Type array
            Assert.Equal("control", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ControlKey));
            Assert.Equal("country", actualMachine.GetStringFieldValue(Models.Metadata.Machine.CountryKey));
            Assert.Equal("description", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DescriptionKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DeviceKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DeviceRefKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DipSwitchKey)); // Type array
            Assert.Equal("dirname", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DirNameKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DiskKey)); // Type array
            Assert.Equal("displaycount", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DisplayCountKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DisplayKey)); // Type array
            Assert.Equal("displaytype", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DisplayTypeKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DriverKey)); // Type
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DumpKey)); // Type array
            Assert.Equal("duplicateid", actualMachine.GetStringFieldValue(Models.Metadata.Machine.DuplicateIDKey));
            Assert.Equal("emulator", actualMachine.GetStringFieldValue(Models.Metadata.Machine.EmulatorKey));
            Assert.Equal("extra", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ExtraKey));
            Assert.Equal("favorite", actualMachine.GetStringFieldValue(Models.Metadata.Machine.FavoriteKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.FeatureKey)); // Type array
            Assert.Equal("genmsxid", actualMachine.GetStringFieldValue(Models.Metadata.Machine.GenMSXIDKey));
            Assert.Equal("history", actualMachine.GetStringFieldValue(Models.Metadata.Machine.HistoryKey));
            Assert.Equal("id", actualMachine.GetStringFieldValue(Models.Metadata.Machine.IdKey));
            Assert.Equal("deadbeef", actualMachine.GetStringFieldValue(Models.Metadata.Machine.Im1CRCKey));
            Assert.Equal("deadbeef", actualMachine.GetStringFieldValue(Models.Metadata.Machine.Im2CRCKey));
            Assert.Equal("imagenumber", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ImageNumberKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.InfoKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.InputKey)); // Type
            Assert.Equal("yes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.IsBiosKey));
            Assert.Equal("yes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.IsDeviceKey));
            Assert.Equal("yes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.IsMechanicalKey));
            Assert.Equal("language", actualMachine.GetStringFieldValue(Models.Metadata.Machine.LanguageKey));
            Assert.Equal("location", actualMachine.GetStringFieldValue(Models.Metadata.Machine.LocationKey));
            Assert.Equal("manufacturer", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ManufacturerKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.MediaKey)); // Type array
            Assert.Equal("name", actualMachine.GetStringFieldValue(Models.Metadata.Machine.NameKey));
            Assert.Equal("notes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.NotesKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PartKey)); // Type array
            Assert.Equal("playedcount", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PlayedCountKey));
            Assert.Equal("playedtime", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PlayedTimeKey));
            Assert.Equal("players", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PlayersKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PortKey)); // Type array
            Assert.Equal("publisher", actualMachine.GetStringFieldValue(Models.Metadata.Machine.PublisherKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RamOptionKey)); // Type array
            Assert.Equal("rebuildto", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RebuildToKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ReleaseKey)); // Type array
            Assert.Equal("releasenumber", actualMachine.GetStringFieldValue(Models.Metadata.Machine.ReleaseNumberKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RomKey)); // Type array
            Assert.Equal("romof", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RomOfKey));
            Assert.Equal("rotation", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RotationKey));
            Assert.Equal("yes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.RunnableKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SampleKey)); // Type array
            Assert.Equal("sampleof", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SampleOfKey));
            Assert.Equal("savetype", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SaveTypeKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SharedFeatKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SlotKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SoftwareListKey)); // Type array
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SoundKey)); // Type
            Assert.Equal("sourcefile", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SourceFileKey));
            Assert.Equal("sourcerom", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SourceRomKey));
            Assert.Equal("status", actualMachine.GetStringFieldValue(Models.Metadata.Machine.StatusKey));
            Assert.Equal("yes", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SupportedKey));
            Assert.Equal("system", actualMachine.GetStringFieldValue(Models.Metadata.Machine.SystemKey));
            Assert.Equal("tags", actualMachine.GetStringFieldValue(Models.Metadata.Machine.TagsKey));
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.TruripKey)); // Type
            // Assert.Equal("REPLACE", actualMachine.GetStringFieldValue(Models.Metadata.Machine.VideoKey)); // Type
            Assert.Equal("year", actualMachine.GetStringFieldValue(Models.Metadata.Machine.YearKey));

            // Aggregate for easier validation
            DatItems.DatItem[] datItems = datFile.Items
                .SelectMany(kvp => kvp.Value ?? [])
                .ToArray();

            DatItems.Formats.Adjuster? actualAdjuster = Array.Find(datItems, item => item is DatItems.Formats.Adjuster) as DatItems.Formats.Adjuster;
            Assert.NotNull(actualAdjuster);
            Assert.True(actualAdjuster.GetBoolFieldValue(Models.Metadata.Adjuster.DefaultKey));
            Assert.Equal("name", actualAdjuster.GetStringFieldValue(Models.Metadata.Adjuster.NameKey));

            DatItems.Formats.Condition? actualAdjusterCondition = actualAdjuster.GetFieldValue<DatItems.Formats.Condition>(Models.Metadata.Adjuster.ConditionKey);
            Assert.NotNull(actualAdjusterCondition);
            Assert.Equal("value", actualAdjusterCondition.GetStringFieldValue(Models.Metadata.Condition.ValueKey));
            Assert.Equal("mask", actualAdjusterCondition.GetStringFieldValue(Models.Metadata.Condition.MaskKey));
            Assert.Equal("eq", actualAdjusterCondition.GetStringFieldValue(Models.Metadata.Condition.RelationKey));
            Assert.Equal("tag", actualAdjusterCondition.GetStringFieldValue(Models.Metadata.Condition.TagKey));

            DatItems.Formats.Archive? actualArchive = Array.Find(datItems, item => item is DatItems.Formats.Archive) as DatItems.Formats.Archive;
            Assert.NotNull(actualArchive);
            Assert.Equal("name", actualArchive.GetStringFieldValue(Models.Metadata.Archive.NameKey));

            DatItems.Formats.BiosSet? actualBiosSet = Array.Find(datItems, item => item is DatItems.Formats.BiosSet) as DatItems.Formats.BiosSet;
            Assert.NotNull(actualBiosSet);
            Assert.True(actualBiosSet.GetBoolFieldValue(Models.Metadata.BiosSet.DefaultKey));
            Assert.Equal("description", actualBiosSet.GetStringFieldValue(Models.Metadata.BiosSet.DescriptionKey));
            Assert.Equal("name", actualBiosSet.GetStringFieldValue(Models.Metadata.BiosSet.NameKey));

            // TODO: Validate all fields
        }

        #endregion
    }
}