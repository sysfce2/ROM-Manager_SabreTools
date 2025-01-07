using SabreTools.Core.Tools;
using SabreTools.DatFiles;
using Xunit;

namespace SabreTools.Core.Test.Tools
{
    // TODO: Remove reliance on anything but SabreTools.Core
    public class ConvertersTests
    {
        #region String to Enum

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
        public void AsMergingFlagTest(string? field, MergingFlag expected)
        {
            MergingFlag actual = field.AsEnumValue<MergingFlag>();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, NodumpFlag.None)]
        [InlineData("none", NodumpFlag.None)]
        [InlineData("obsolete", NodumpFlag.Obsolete)]
        [InlineData("required", NodumpFlag.Required)]
        [InlineData("ignore", NodumpFlag.Ignore)]
        public void AsNodumpFlagTest(string? field, NodumpFlag expected)
        {
            NodumpFlag actual = field.AsEnumValue<NodumpFlag>();
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
        [InlineData("fileonly", PackingFlag.FileOnly)]
        public void AsPackingFlagTest(string? field, PackingFlag expected)
        {
            PackingFlag actual = field.AsEnumValue<PackingFlag>();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("INVALID", null)]
        [InlineData("yes", true)]
        [InlineData("True", true)]
        [InlineData("no", false)]
        [InlineData("False", false)]
        public void AsYesNoTest(string? field, bool? expected)
        {
            bool? actual = field.AsYesNo();
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Enum to String

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
        public void FromMergingFlagTest(MergingFlag field, bool useSecond, string? expected)
        {
            string? actual = field.AsStringValue<MergingFlag>(useSecond);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(NodumpFlag.None, "none")]
        [InlineData(NodumpFlag.Obsolete, "obsolete")]
        [InlineData(NodumpFlag.Required, "required")]
        [InlineData(NodumpFlag.Ignore, "ignore")]
        public void FromNodumpFlagTest(NodumpFlag field, string? expected)
        {
            string? actual = field.AsStringValue<NodumpFlag>();
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
        [InlineData(PackingFlag.FileOnly, true, "fileonly")]
        [InlineData(PackingFlag.FileOnly, false, "fileonly")]
        public void FromPackingFlagTest(PackingFlag field, bool useSecond, string? expected)
        {
            string? actual = field.AsStringValue<PackingFlag>(useSecond);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(true, "yes")]
        [InlineData(false, "no")]
        public void FromYesNo(bool? field, string? expected)
        {
            string? actual = field.FromYesNo();
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Generators

        [Theory]
        [InlineData(MergingFlag.None, 12)]
        [InlineData(NodumpFlag.None, 4)]
        [InlineData(PackingFlag.None, 8)]
        public void GenerateToEnumTest<T>(T value, int expected)
        {
            var actual = Converters.GenerateToEnum<T>();
            Assert.Equal(default, value);
            Assert.Equal(expected, actual.Keys.Count);
        }

        #endregion
    }
}