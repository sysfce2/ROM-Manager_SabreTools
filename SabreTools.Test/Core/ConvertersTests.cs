using SabreTools.Core;
using SabreTools.Core.Tools;
using Xunit;

namespace SabreTools.Test.Core
{
    public class ConvertersTests
    {
        #region String to Enum

        [Theory]
        [InlineData(null, ChipType.NULL)]
        [InlineData("cpu", ChipType.CPU)]
        [InlineData("audio", ChipType.Audio)]
        public void AsChipTypeTest(string field, ChipType expected)
        {
            ChipType actual = field.AsChipType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, ControlType.NULL)]
        [InlineData("joy", ControlType.Joy)]
        [InlineData("stick", ControlType.Stick)]
        [InlineData("paddle", ControlType.Paddle)]
        [InlineData("pedal", ControlType.Pedal)]
        [InlineData("lightgun", ControlType.Lightgun)]
        [InlineData("positional", ControlType.Positional)]
        [InlineData("dial", ControlType.Dial)]
        [InlineData("trackball", ControlType.Trackball)]
        [InlineData("mouse", ControlType.Mouse)]
        [InlineData("only_buttons", ControlType.OnlyButtons)]
        [InlineData("keypad", ControlType.Keypad)]
        [InlineData("keyboard", ControlType.Keyboard)]
        [InlineData("mahjong", ControlType.Mahjong)]
        [InlineData("hanafuda", ControlType.Hanafuda)]
        [InlineData("gambling", ControlType.Gambling)]
        public void AsControlTypeTest(string field, ControlType expected)
        {
            ControlType actual = field.AsControlType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, DatHeaderField.NULL)]
        [InlineData("datname", DatHeaderField.NULL)]
        [InlineData("dat-datname", DatHeaderField.Name)]
        [InlineData("dat.datname", DatHeaderField.Name)]
        [InlineData("dat_datname", DatHeaderField.Name)]
        [InlineData("dat datname", DatHeaderField.Name)]
        [InlineData("datheader-datname", DatHeaderField.Name)]
        [InlineData("datheader.datname", DatHeaderField.Name)]
        [InlineData("datheader_datname", DatHeaderField.Name)]
        [InlineData("datheader datname", DatHeaderField.Name)]
        [InlineData("header-datname", DatHeaderField.Name)]
        [InlineData("header.datname", DatHeaderField.Name)]
        [InlineData("header_datname", DatHeaderField.Name)]
        [InlineData("header datname", DatHeaderField.Name)]
        [InlineData("DAT.DATNAME", DatHeaderField.Name)]
        [InlineData("dAt.DAtnamE", DatHeaderField.Name)]
        public void AsDatHeaderFieldProcessingTest(string field, DatHeaderField expected)
        {
            // TODO: Write new test for all supported fields
            DatHeaderField actual = field.AsDatHeaderField();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, DatItemField.NULL)]
        [InlineData("name", DatItemField.NULL)]
        [InlineData("item-name", DatItemField.Name)]
        [InlineData("item.name", DatItemField.Name)]
        [InlineData("item_name", DatItemField.Name)]
        [InlineData("item name", DatItemField.Name)]
        [InlineData("datitem-name", DatItemField.Name)]
        [InlineData("datitem.name", DatItemField.Name)]
        [InlineData("datitem_name", DatItemField.Name)]
        [InlineData("datitem name", DatItemField.Name)]
        [InlineData("ITEM.NAME", DatItemField.Name)]
        [InlineData("iTeM.namE", DatItemField.Name)]
        public void AsDatItemFieldProcessingTest(string field, DatItemField expected)
        {
            // TODO: Write new test for all supported fields
            DatItemField actual = field.AsDatItemField();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, DeviceType.NULL)]
        [InlineData("unknown", DeviceType.Unknown)]
        [InlineData("cartridge", DeviceType.Cartridge)]
        [InlineData("floppydisk", DeviceType.FloppyDisk)]
        [InlineData("harddisk", DeviceType.HardDisk)]
        [InlineData("cylinder", DeviceType.Cylinder)]
        [InlineData("cassette", DeviceType.Cassette)]
        [InlineData("punchcard", DeviceType.PunchCard)]
        [InlineData("punchtape", DeviceType.PunchTape)]
        [InlineData("printout", DeviceType.Printout)]
        [InlineData("serial", DeviceType.Serial)]
        [InlineData("parallel", DeviceType.Parallel)]
        [InlineData("snapshot", DeviceType.Snapshot)]
        [InlineData("quickload", DeviceType.QuickLoad)]
        [InlineData("memcard", DeviceType.MemCard)]
        [InlineData("cdrom", DeviceType.CDROM)]
        [InlineData("magtape", DeviceType.MagTape)]
        [InlineData("romimage", DeviceType.ROMImage)]
        [InlineData("midiin", DeviceType.MIDIIn)]
        [InlineData("midiout", DeviceType.MIDIOut)]
        [InlineData("picture", DeviceType.Picture)]
        [InlineData("vidfile", DeviceType.VidFile)]
        public void AsDeviceTypeTest(string field, DeviceType expected)
        {
            DeviceType actual = field.AsDeviceType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, DisplayType.NULL)]
        [InlineData("raster", DisplayType.Raster)]
        [InlineData("vector", DisplayType.Vector)]
        [InlineData("lcd", DisplayType.LCD)]
        [InlineData("svg", DisplayType.SVG)]
        [InlineData("unknown", DisplayType.Unknown)]
        public void AsDisplayTypeTest(string field, DisplayType expected)
        {
            DisplayType actual = field.AsDisplayType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, Endianness.NULL)]
        [InlineData("big", Endianness.Big)]
        [InlineData("little", Endianness.Little)]
        public void AsEndiannessTest(string field, Endianness expected)
        {
            Endianness actual = field.AsEndianness();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, FeatureStatus.NULL)]
        [InlineData("unemulated", FeatureStatus.Unemulated)]
        [InlineData("imperfect", FeatureStatus.Imperfect)]
        public void AsFeatureStatusTest(string field, FeatureStatus expected)
        {
            FeatureStatus actual = field.AsFeatureStatus();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, FeatureType.NULL)]
        [InlineData("protection", FeatureType.Protection)]
        [InlineData("palette", FeatureType.Palette)]
        [InlineData("graphics", FeatureType.Graphics)]
        [InlineData("sound", FeatureType.Sound)]
        [InlineData("controls", FeatureType.Controls)]
        [InlineData("keyboard", FeatureType.Keyboard)]
        [InlineData("mouse", FeatureType.Mouse)]
        [InlineData("microphone", FeatureType.Microphone)]
        [InlineData("camera", FeatureType.Camera)]
        [InlineData("disk", FeatureType.Disk)]
        [InlineData("printer", FeatureType.Printer)]
        [InlineData("lan", FeatureType.Lan)]
        [InlineData("wan", FeatureType.Wan)]
        [InlineData("timing", FeatureType.Timing)]
        public void AsFeatureTypeTest(string field, FeatureType expected)
        {
            FeatureType actual = field.AsFeatureType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, ItemStatus.NULL)]
        [InlineData("none", ItemStatus.None)]
        [InlineData("no", ItemStatus.None)]
        [InlineData("good", ItemStatus.Good)]
        [InlineData("baddump", ItemStatus.BadDump)]
        [InlineData("nodump", ItemStatus.Nodump)]
        [InlineData("yes", ItemStatus.Nodump)]
        [InlineData("verified", ItemStatus.Verified)]
        public void AsItemStatusTest(string field, ItemStatus expected)
        {
            ItemStatus actual = field.AsItemStatus();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, ItemType.NULL)]
        [InlineData("adjuster", ItemType.Adjuster)]
        [InlineData("analog", ItemType.Analog)]
        [InlineData("archive", ItemType.Archive)]
        [InlineData("biosset", ItemType.BiosSet)]
        [InlineData("blank", ItemType.Blank)]
        [InlineData("chip", ItemType.Chip)]
        [InlineData("condition", ItemType.Condition)]
        [InlineData("configuration", ItemType.Configuration)]
        [InlineData("control", ItemType.Control)]
        [InlineData("dataarea", ItemType.DataArea)]
        [InlineData("device", ItemType.Device)]
        [InlineData("deviceref", ItemType.DeviceReference)]
        [InlineData("device_ref", ItemType.DeviceReference)]
        [InlineData("dipswitch", ItemType.DipSwitch)]
        [InlineData("disk", ItemType.Disk)]
        [InlineData("diskarea", ItemType.DiskArea)]
        [InlineData("display", ItemType.Display)]
        [InlineData("driver", ItemType.Driver)]
        [InlineData("extension", ItemType.Extension)]
        [InlineData("feature", ItemType.Feature)]
        [InlineData("file", ItemType.File)]
        [InlineData("info", ItemType.Info)]
        [InlineData("input", ItemType.Input)]
        [InlineData("instance", ItemType.Instance)]
        [InlineData("location", ItemType.Location)]
        [InlineData("media", ItemType.Media)]
        [InlineData("part", ItemType.Part)]
        [InlineData("partfeature", ItemType.PartFeature)]
        [InlineData("part_feature", ItemType.PartFeature)]
        [InlineData("port", ItemType.Port)]
        [InlineData("ramoption", ItemType.RamOption)]
        [InlineData("ram_option", ItemType.RamOption)]
        [InlineData("release", ItemType.Release)]
        [InlineData("releasedetails", ItemType.ReleaseDetails)]
        [InlineData("release_details", ItemType.ReleaseDetails)]
        [InlineData("rom", ItemType.Rom)]
        [InlineData("sample", ItemType.Sample)]
        [InlineData("serials", ItemType.Serials)]
        [InlineData("setting", ItemType.Setting)]
        [InlineData("sharedfeat", ItemType.SharedFeature)]
        [InlineData("shared_feat", ItemType.SharedFeature)]
        [InlineData("sharedfeature", ItemType.SharedFeature)]
        [InlineData("shared_feature", ItemType.SharedFeature)]
        [InlineData("slot", ItemType.Slot)]
        [InlineData("slotoption", ItemType.SlotOption)]
        [InlineData("slot_option", ItemType.SlotOption)]
        [InlineData("softwarelist", ItemType.SoftwareList)]
        [InlineData("software_list", ItemType.SoftwareList)]
        [InlineData("sound", ItemType.Sound)]
        [InlineData("sourcedetails", ItemType.SourceDetails)]
        [InlineData("source_details", ItemType.SourceDetails)]
        public void AsItemTypeTest(string field, ItemType expected)
        {
            ItemType actual = field.AsItemType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, LoadFlag.NULL)]
        [InlineData("load16_byte", LoadFlag.Load16Byte)]
        [InlineData("load16_word", LoadFlag.Load16Word)]
        [InlineData("load16_word_swap", LoadFlag.Load16WordSwap)]
        [InlineData("load32_byte", LoadFlag.Load32Byte)]
        [InlineData("load32_word", LoadFlag.Load32Word)]
        [InlineData("load32_word_swap", LoadFlag.Load32WordSwap)]
        [InlineData("load32_dword", LoadFlag.Load32DWord)]
        [InlineData("load64_word", LoadFlag.Load64Word)]
        [InlineData("load64_word_swap", LoadFlag.Load64WordSwap)]
        [InlineData("reload", LoadFlag.Reload)]
        [InlineData("fill", LoadFlag.Fill)]
        [InlineData("continue", LoadFlag.Continue)]
        [InlineData("reload_plain", LoadFlag.ReloadPlain)]
        [InlineData("ignore", LoadFlag.Ignore)]
        public void AsLoadFlagTest(string field, LoadFlag expected)
        {
            LoadFlag actual = field.AsLoadFlag();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, LogLevel.VERBOSE)]
        [InlineData("verbose", LogLevel.VERBOSE)]
        [InlineData("user", LogLevel.USER)]
        [InlineData("warning", LogLevel.WARNING)]
        [InlineData("error", LogLevel.ERROR)]
        public void AsLogLevelTest(string field, LogLevel expected)
        {
            LogLevel actual = field.AsLogLevel();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, MachineField.NULL)]
        [InlineData("name", MachineField.NULL)]
        [InlineData("game-name", MachineField.Name)]
        [InlineData("game.name", MachineField.Name)]
        [InlineData("game_name", MachineField.Name)]
        [InlineData("game name", MachineField.Name)]
        [InlineData("machine-name", MachineField.Name)]
        [InlineData("machine.name", MachineField.Name)]
        [InlineData("machine_name", MachineField.Name)]
        [InlineData("machine name", MachineField.Name)]
        [InlineData("GAME.NAME", MachineField.Name)]
        [InlineData("gAmE.namE", MachineField.Name)]
        public void AsMachineFieldProcessingTest(string field, MachineField expected)
        {
            MachineField actual = field.AsMachineField();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, MachineField.NULL)]
        [InlineData("game.board", MachineField.Board)]
        [InlineData("game.buttons", MachineField.Buttons)]
        [InlineData("game.category", MachineField.Category)]
        [InlineData("game.cloneof", MachineField.CloneOf)]
        [InlineData("game.clone_of", MachineField.CloneOf)]
        [InlineData("game.comment", MachineField.Comment)]
        [InlineData("game.extra", MachineField.Comment)]
        [InlineData("game.control", MachineField.Control)]
        [InlineData("game.country", MachineField.Country)]
        [InlineData("game.crc", MachineField.CRC)]
        [InlineData("game.hascrc", MachineField.CRC)]
        [InlineData("game.has_crc", MachineField.CRC)]
        [InlineData("game.desc", MachineField.Description)]
        [InlineData("game.description", MachineField.Description)]
        [InlineData("game.developer", MachineField.Developer)]
        [InlineData("game.displaycount", MachineField.DisplayCount)]
        [InlineData("game.display_count", MachineField.DisplayCount)]
        [InlineData("game.displaytype", MachineField.DisplayType)]
        [InlineData("game.display_type", MachineField.DisplayType)]
        [InlineData("game.enabled", MachineField.Enabled)]
        [InlineData("game.genmsxid", MachineField.GenMSXID)]
        [InlineData("game.genmsx_id", MachineField.GenMSXID)]
        [InlineData("game.gen_msxid", MachineField.GenMSXID)]
        [InlineData("game.gen_msx_id", MachineField.GenMSXID)]
        [InlineData("game.genre", MachineField.Genre)]
        [InlineData("game.history", MachineField.History)]
        [InlineData("game.manufacturer", MachineField.Manufacturer)]
        [InlineData("game.name", MachineField.Name)]
        [InlineData("game.cloneofid", MachineField.NoIntroCloneOfId)]
        [InlineData("game.nointrocloneofid", MachineField.NoIntroCloneOfId)]
        [InlineData("game.nointro_cloneofid", MachineField.NoIntroCloneOfId)]
        [InlineData("game.no_intro_cloneofid", MachineField.NoIntroCloneOfId)]
        [InlineData("game.no_intro_clone_of_id", MachineField.NoIntroCloneOfId)]
        [InlineData("game.id", MachineField.NoIntroId)]
        [InlineData("game.nointroid", MachineField.NoIntroId)]
        [InlineData("game.nointro_id", MachineField.NoIntroId)]
        [InlineData("game.no_intro_id", MachineField.NoIntroId)]
        [InlineData("game.players", MachineField.Players)]
        [InlineData("game.publisher", MachineField.Publisher)]
        [InlineData("game.ratings", MachineField.Ratings)]
        [InlineData("game.rebuildto", MachineField.RebuildTo)]
        [InlineData("game.rebuild_to", MachineField.RebuildTo)]
        [InlineData("game.relatedto", MachineField.RelatedTo)]
        [InlineData("game.related_to", MachineField.RelatedTo)]
        [InlineData("game.romof", MachineField.RomOf)]
        [InlineData("game.rom_of", MachineField.RomOf)]
        [InlineData("game.rotation", MachineField.Rotation)]
        [InlineData("game.runnable", MachineField.Runnable)]
        [InlineData("game.sampleof", MachineField.SampleOf)]
        [InlineData("game.sample_of", MachineField.SampleOf)]
        [InlineData("game.score", MachineField.Score)]
        [InlineData("game.sourcefile", MachineField.SourceFile)]
        [InlineData("game.source_file", MachineField.SourceFile)]
        [InlineData("game.amstatus", MachineField.Status)]
        [InlineData("game.am_status", MachineField.Status)]
        [InlineData("game.gamestatus", MachineField.Status)]
        [InlineData("game.supportstatus", MachineField.Status)]
        [InlineData("game.support_status", MachineField.Status)]
        [InlineData("game.subgenre", MachineField.Subgenre)]
        [InlineData("game.sub_genre", MachineField.Subgenre)]
        [InlineData("game.supported", MachineField.Supported)]
        [InlineData("game.system", MachineField.System)]
        [InlineData("game.msxsystem", MachineField.System)]
        [InlineData("game.msx_system", MachineField.System)]
        [InlineData("game.titleid", MachineField.TitleID)]
        [InlineData("game.title_id", MachineField.TitleID)]
        [InlineData("game.type", MachineField.Type)]
        [InlineData("game.year", MachineField.Year)]
        public void AsMachineFieldTest(string field, MachineField expected)
        {
            MachineField actual = field.AsMachineField();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, MachineType.None)]
        [InlineData("none", MachineType.None)]
        [InlineData("bios", MachineType.Bios)]
        [InlineData("dev", MachineType.Device)]
        [InlineData("device", MachineType.Device)]
        [InlineData("mech", MachineType.Mechanical)]
        [InlineData("mechanical", MachineType.Mechanical)]
        public void AsMachineTypeTest(string field, MachineType expected)
        {
            MachineType actual = field.AsMachineType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, MergingFlag.None)]
        [InlineData("none", MergingFlag.None)]
        [InlineData("split", MergingFlag.Split)]
        [InlineData("merged", MergingFlag.Merged)]
        [InlineData("nonmerged", MergingFlag.NonMerged)]
        [InlineData("unmerged", MergingFlag.NonMerged)]
        [InlineData("fullmerged", MergingFlag.FullMerged)]
        [InlineData("device", MergingFlag.DeviceNonMerged)]
        [InlineData("devicenonmerged", MergingFlag.DeviceNonMerged)]
        [InlineData("deviceunmerged", MergingFlag.DeviceNonMerged)]
        [InlineData("full", MergingFlag.FullNonMerged)]
        [InlineData("fullnonmerged", MergingFlag.FullNonMerged)]
        [InlineData("fullunmerged", MergingFlag.FullNonMerged)]
        public void AsMergingFlagTest(string field, MergingFlag expected)
        {
            MergingFlag actual = field.AsMergingFlag();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, NodumpFlag.None)]
        [InlineData("none", NodumpFlag.None)]
        [InlineData("obsolete", NodumpFlag.Obsolete)]
        [InlineData("required", NodumpFlag.Required)]
        [InlineData("ignore", NodumpFlag.Ignore)]
        public void AsNodumpFlagTest(string field, NodumpFlag expected)
        {
            NodumpFlag actual = field.AsNodumpFlag();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, OpenMSXSubType.NULL)]
        [InlineData("rom", OpenMSXSubType.Rom)]
        [InlineData("megarom", OpenMSXSubType.MegaRom)]
        [InlineData("sccpluscart", OpenMSXSubType.SCCPlusCart)]
        public void AsOpenMSXSubTypeTest(string field, OpenMSXSubType expected)
        {
            OpenMSXSubType actual = field.AsOpenMSXSubType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, PackingFlag.None)]
        [InlineData("none", PackingFlag.None)]
        [InlineData("yes", PackingFlag.Zip)]
        [InlineData("zip", PackingFlag.Zip)]
        [InlineData("no", PackingFlag.Unzip)]
        [InlineData("unzip", PackingFlag.Unzip)]
        [InlineData("partial", PackingFlag.Partial)]
        [InlineData("flat", PackingFlag.Flat)]
        public void AsPackingFlagTest(string field, PackingFlag expected)
        {
            PackingFlag actual = field.AsPackingFlag();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, Relation.NULL)]
        [InlineData("eq", Relation.Equal)]
        [InlineData("ne", Relation.NotEqual)]
        [InlineData("gt", Relation.GreaterThan)]
        [InlineData("le", Relation.LessThanOrEqual)]
        [InlineData("lt", Relation.LessThan)]
        [InlineData("ge", Relation.GreaterThanOrEqual)]
        public void AsRelationTest(string field, Relation expected)
        {
            Relation actual = field.AsRelation();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, Runnable.NULL)]
        [InlineData("no", Runnable.No)]
        [InlineData("partial", Runnable.Partial)]
        [InlineData("yes", Runnable.Yes)]
        public void AsRunnableTest(string field, Runnable expected)
        {
            Runnable actual = field.AsRunnable();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, SoftwareListStatus.None)]
        [InlineData("none", SoftwareListStatus.None)]
        [InlineData("original", SoftwareListStatus.Original)]
        [InlineData("compatible", SoftwareListStatus.Compatible)]
        public void AsSoftwareListStatusTest(string field, SoftwareListStatus expected)
        {
            SoftwareListStatus actual = field.AsSoftwareListStatus();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, Supported.NULL)]
        [InlineData("no", Supported.No)]
        [InlineData("unsupported", Supported.No)]
        [InlineData("partial", Supported.Partial)]
        [InlineData("yes", Supported.Yes)]
        [InlineData("supported", Supported.Yes)]
        public void AsSupportedTest(string field, Supported expected)
        {
            Supported actual = field.AsSupported();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, SupportStatus.NULL)]
        [InlineData("good", SupportStatus.Good)]
        [InlineData("imperfect", SupportStatus.Imperfect)]
        [InlineData("preliminary", SupportStatus.Preliminary)]
        public void AsSupportStatusTest(string field, SupportStatus expected)
        {
            SupportStatus actual = field.AsSupportStatus();
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Enum to String

        [Theory]
        [InlineData(ChipType.NULL, null)]
        [InlineData(ChipType.CPU, "cpu")]
        [InlineData(ChipType.Audio, "audio")]
        public void FromChipTypeTest(ChipType field, string expected)
        {
            string actual = field.FromChipType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ControlType.NULL, null)]
        [InlineData(ControlType.Joy, "joy")]
        [InlineData(ControlType.Stick, "stick")]
        [InlineData(ControlType.Paddle, "paddle")]
        [InlineData(ControlType.Pedal, "pedal")]
        [InlineData(ControlType.Lightgun, "lightgun")]
        [InlineData(ControlType.Positional, "positional")]
        [InlineData(ControlType.Dial, "dial")]
        [InlineData(ControlType.Trackball, "trackball")]
        [InlineData(ControlType.Mouse, "mouse")]
        [InlineData(ControlType.OnlyButtons, "only_buttons")]
        [InlineData(ControlType.Keypad, "keypad")]
        [InlineData(ControlType.Keyboard, "keyboard")]
        [InlineData(ControlType.Mahjong, "mahjong")]
        [InlineData(ControlType.Hanafuda, "hanafuda")]
        [InlineData(ControlType.Gambling, "gambling")]
        public void FromControlTypeTest(ControlType field, string expected)
        {
            string actual = field.FromControlType();
            Assert.Equal(expected, actual);
        }

        // TODO: Write new test for all supported DatHeaderField values
        // TODO: Write new test for all supported DatItemField values

        [Theory]
        [InlineData(DeviceType.NULL, null)]
        [InlineData(DeviceType.Unknown, "unknown")]
        [InlineData(DeviceType.Cartridge, "cartridge")]
        [InlineData(DeviceType.FloppyDisk, "floppydisk")]
        [InlineData(DeviceType.HardDisk, "harddisk")]
        [InlineData(DeviceType.Cylinder, "cylinder")]
        [InlineData(DeviceType.Cassette, "cassette")]
        [InlineData(DeviceType.PunchCard, "punchcard")]
        [InlineData(DeviceType.PunchTape, "punchtape")]
        [InlineData(DeviceType.Printout, "printout")]
        [InlineData(DeviceType.Serial, "serial")]
        [InlineData(DeviceType.Parallel, "parallel")]
        [InlineData(DeviceType.Snapshot, "snapshot")]
        [InlineData(DeviceType.QuickLoad, "quickload")]
        [InlineData(DeviceType.MemCard, "memcard")]
        [InlineData(DeviceType.CDROM, "cdrom")]
        [InlineData(DeviceType.MagTape, "magtape")]
        [InlineData(DeviceType.ROMImage, "romimage")]
        [InlineData(DeviceType.MIDIIn, "midiin")]
        [InlineData(DeviceType.MIDIOut, "midiout")]
        [InlineData(DeviceType.Picture, "picture")]
        [InlineData(DeviceType.VidFile, "vidfile")]
        public void FromDeviceTypeTest(DeviceType field, string expected)
        {
            string actual = field.FromDeviceType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(DisplayType.NULL, null)]
        [InlineData(DisplayType.Raster, "raster")]
        [InlineData(DisplayType.Vector, "vector")]
        [InlineData(DisplayType.LCD, "lcd")]
        [InlineData(DisplayType.SVG, "svg")]
        [InlineData(DisplayType.Unknown, "unknown")]
        public void FromDisplayTypeTest(DisplayType field, string expected)
        {
            string actual = field.FromDisplayType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Endianness.NULL, null)]
        [InlineData(Endianness.Big, "big")]
        [InlineData(Endianness.Little, "little")]
        public void FromEndiannessTest(Endianness field, string expected)
        {
            string actual = field.FromEndianness();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(FeatureStatus.NULL, null)]
        [InlineData(FeatureStatus.Unemulated, "unemulated")]
        [InlineData(FeatureStatus.Imperfect, "imperfect")]
        public void FromFeatureStatusTest(FeatureStatus field, string expected)
        {
            string actual = field.FromFeatureStatus();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(FeatureType.NULL, null)]
        [InlineData(FeatureType.Protection, "protection")]
        [InlineData(FeatureType.Palette, "palette")]
        [InlineData(FeatureType.Graphics, "graphics")]
        [InlineData(FeatureType.Sound, "sound")]
        [InlineData(FeatureType.Controls, "controls")]
        [InlineData(FeatureType.Keyboard, "keyboard")]
        [InlineData(FeatureType.Mouse, "mouse")]
        [InlineData(FeatureType.Microphone, "microphone")]
        [InlineData(FeatureType.Camera, "camera")]
        [InlineData(FeatureType.Disk, "disk")]
        [InlineData(FeatureType.Printer, "printer")]
        [InlineData(FeatureType.Lan, "lan")]
        [InlineData(FeatureType.Wan, "wan")]
        [InlineData(FeatureType.Timing, "timing")]
        public void FromFeatureTypeTest(FeatureType field, string expected)
        {
            string actual = field.FromFeatureType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ItemStatus.NULL, true, null)]
        [InlineData(ItemStatus.NULL, false, null)]
        [InlineData(ItemStatus.None, true, "no")]
        [InlineData(ItemStatus.None, false, "none")]
        [InlineData(ItemStatus.Good, true, "good")]
        [InlineData(ItemStatus.Good, false, "good")]
        [InlineData(ItemStatus.BadDump, true, "baddump")]
        [InlineData(ItemStatus.BadDump, false, "baddump")]
        [InlineData(ItemStatus.Nodump, true, "yes")]
        [InlineData(ItemStatus.Nodump, false, "nodump")]
        [InlineData(ItemStatus.Verified, true, "verified")]
        [InlineData(ItemStatus.Verified, false, "verified")]
        public void FromItemStatusTest(ItemStatus field, bool yesno, string expected)
        {
            string actual = field.FromItemStatus(yesno);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ItemType.NULL, null)]
        [InlineData(ItemType.Adjuster, "adjuster")]
        [InlineData(ItemType.Analog, "analog")]
        [InlineData(ItemType.Archive, "archive")]
        [InlineData(ItemType.BiosSet, "biosset")]
        [InlineData(ItemType.Blank, "blank")]
        [InlineData(ItemType.Chip, "chip")]
        [InlineData(ItemType.Condition, "condition")]
        [InlineData(ItemType.Configuration, "configuration")]
        [InlineData(ItemType.Control, "control")]
        [InlineData(ItemType.DataArea, "dataarea")]
        [InlineData(ItemType.Device, "device")]
        [InlineData(ItemType.DeviceReference, "device_ref")]
        [InlineData(ItemType.DipSwitch, "dipswitch")]
        [InlineData(ItemType.Disk, "disk")]
        [InlineData(ItemType.DiskArea, "diskarea")]
        [InlineData(ItemType.Display, "display")]
        [InlineData(ItemType.Driver, "driver")]
        [InlineData(ItemType.Extension, "extension")]
        [InlineData(ItemType.Feature, "feature")]
        [InlineData(ItemType.File, "file")]
        [InlineData(ItemType.Info, "info")]
        [InlineData(ItemType.Input, "input")]
        [InlineData(ItemType.Instance, "instance")]
        [InlineData(ItemType.Location, "location")]
        [InlineData(ItemType.Media, "media")]
        [InlineData(ItemType.Part, "part")]
        [InlineData(ItemType.PartFeature, "part_feature")]
        [InlineData(ItemType.Port, "port")]
        [InlineData(ItemType.RamOption, "ramoption")]
        [InlineData(ItemType.Release, "release")]
        [InlineData(ItemType.ReleaseDetails, "release_details")]
        [InlineData(ItemType.Rom, "rom")]
        [InlineData(ItemType.Sample, "sample")]
        [InlineData(ItemType.Serials, "serials")]
        [InlineData(ItemType.Setting, "setting")]
        [InlineData(ItemType.SharedFeature, "sharedfeat")]
        [InlineData(ItemType.Slot, "slot")]
        [InlineData(ItemType.SlotOption, "slotoption")]
        [InlineData(ItemType.SoftwareList, "softwarelist")]
        [InlineData(ItemType.Sound, "sound")]
        [InlineData(ItemType.SourceDetails, "source_details")]
        public void FromItemTypeTest(ItemType field, string expected)
        {
            string actual = field.FromItemType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(LoadFlag.NULL, null)]
        [InlineData(LoadFlag.Load16Byte, "load16_byte")]
        [InlineData(LoadFlag.Load16Word, "load16_word")]
        [InlineData(LoadFlag.Load16WordSwap, "load16_word_swap")]
        [InlineData(LoadFlag.Load32Byte, "load32_byte")]
        [InlineData(LoadFlag.Load32Word, "load32_word")]
        [InlineData(LoadFlag.Load32WordSwap, "load32_word_swap")]
        [InlineData(LoadFlag.Load32DWord, "load32_dword")]
        [InlineData(LoadFlag.Load64Word, "load64_word")]
        [InlineData(LoadFlag.Load64WordSwap, "load64_word_swap")]
        [InlineData(LoadFlag.Reload, "reload")]
        [InlineData(LoadFlag.Fill, "fill")]
        [InlineData(LoadFlag.Continue, "continue")]
        [InlineData(LoadFlag.ReloadPlain, "reload_plain")]
        [InlineData(LoadFlag.Ignore, "ignore")]
        public void FromLoadFlagTest(LoadFlag field, string expected)
        {
            string actual = field.FromLoadFlag();
            Assert.Equal(expected, actual);
        }

        // TODO: Write new test for all supported MachineField values

        [Theory]
        [InlineData(MachineType.None, true, "none")]
        [InlineData(MachineType.None, false, "none")]
        [InlineData(MachineType.Bios, true, "bios")]
        [InlineData(MachineType.Bios, false, "bios")]
        [InlineData(MachineType.Device, true, "dev")]
        [InlineData(MachineType.Device, false, "device")]
        [InlineData(MachineType.Mechanical, true, "mech")]
        [InlineData(MachineType.Mechanical, false, "mechanical")]
        public void FromMachineTypeTest(MachineType field, bool old, string expected)
        {
            string actual = field.FromMachineType(old);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(MergingFlag.None, true, "none")]
        [InlineData(MergingFlag.None, false, "none")]
        [InlineData(MergingFlag.Split, true, "split")]
        [InlineData(MergingFlag.Split, false, "split")]
        [InlineData(MergingFlag.Merged, true, "merged")]
        [InlineData(MergingFlag.Merged, false, "merged")]
        [InlineData(MergingFlag.NonMerged, true, "unmerged")]
        [InlineData(MergingFlag.NonMerged, false, "nonmerged")]
        [InlineData(MergingFlag.FullMerged, true, "fullmerged")]
        [InlineData(MergingFlag.FullMerged, false, "fullmerged")]
        [InlineData(MergingFlag.DeviceNonMerged, true, "deviceunmerged")]
        [InlineData(MergingFlag.DeviceNonMerged, false, "device")]
        [InlineData(MergingFlag.FullNonMerged, true, "fullunmerged")]
        [InlineData(MergingFlag.FullNonMerged, false, "full")]
        public void FromMergingFlagTest(MergingFlag field, bool romcenter, string expected)
        {
            string actual = field.FromMergingFlag(romcenter);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(NodumpFlag.None, "none")]
        [InlineData(NodumpFlag.Obsolete, "obsolete")]
        [InlineData(NodumpFlag.Required, "required")]
        [InlineData(NodumpFlag.Ignore, "ignore")]
        public void FromNodumpFlagTest(NodumpFlag field, string expected)
        {
            string actual = field.FromNodumpFlag();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(OpenMSXSubType.NULL, null)]
        [InlineData(OpenMSXSubType.Rom, "rom")]
        [InlineData(OpenMSXSubType.MegaRom, "megarom")]
        [InlineData(OpenMSXSubType.SCCPlusCart, "sccpluscart")]
        public void FromOpenMSXSubTypeTest(OpenMSXSubType field, string expected)
        {
            string actual = field.FromOpenMSXSubType();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(PackingFlag.None, true, "none")]
        [InlineData(PackingFlag.None, false, "none")]
        [InlineData(PackingFlag.Zip, true, "yes")]
        [InlineData(PackingFlag.Zip, false, "zip")]
        [InlineData(PackingFlag.Unzip, true, "no")]
        [InlineData(PackingFlag.Unzip, false, "unzip")]
        [InlineData(PackingFlag.Partial, true, "partial")]
        [InlineData(PackingFlag.Partial, false, "partial")]
        [InlineData(PackingFlag.Flat, true, "flat")]
        [InlineData(PackingFlag.Flat, false, "flat")]
        public void FromPackingFlagTest(PackingFlag field, bool yesno, string expected)
        {
            string actual = field.FromPackingFlag(yesno);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Relation.NULL, null)]
        [InlineData(Relation.Equal, "eq")]
        [InlineData(Relation.NotEqual, "ne")]
        [InlineData(Relation.GreaterThan, "gt")]
        [InlineData(Relation.LessThanOrEqual, "le")]
        [InlineData(Relation.LessThan, "lt")]
        [InlineData(Relation.GreaterThanOrEqual, "ge")]
        public void FromRelationTest(Relation field, string expected)
        {
            string actual = field.FromRelation();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Runnable.NULL, null)]
        [InlineData(Runnable.No, "no")]
        [InlineData(Runnable.Partial, "partial")]
        [InlineData(Runnable.Yes, "yes")]
        public void FromRunnableTest(Runnable field, string expected)
        {
            string actual = field.FromRunnable();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(SoftwareListStatus.None, "none")]
        [InlineData(SoftwareListStatus.Original, "original")]
        [InlineData(SoftwareListStatus.Compatible, "compatible")]
        public void FromSoftwareListStatusTest(SoftwareListStatus field, string expected)
        {
            string actual = field.FromSoftwareListStatus();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Supported.NULL, true, null)]
        [InlineData(Supported.NULL, false, null)]
        [InlineData(Supported.No, true, "unsupported")]
        [InlineData(Supported.No, false, "no")]
        [InlineData(Supported.Partial, true, "partial")]
        [InlineData(Supported.Partial, false, "partial")]
        [InlineData(Supported.Yes, true, "supported")]
        [InlineData(Supported.Yes, false, "yes")]
        public void FromSupportedTest(Supported field, bool verbose, string expected)
        {
            string actual = field.FromSupported(verbose);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(SupportStatus.NULL, null)]
        [InlineData(SupportStatus.Good, "good")]
        [InlineData(SupportStatus.Imperfect, "imperfect")]
        [InlineData(SupportStatus.Preliminary, "preliminary")]
        public void FromSupportStatusTest(SupportStatus field, string expected)
        {
            string actual = field.FromSupportStatus();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}