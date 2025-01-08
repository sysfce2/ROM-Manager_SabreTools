using SabreTools.DatItems.Formats;
using Xunit;

namespace SabreTools.DatItems.Test
{
    public class DatItemTests
    {
        #region Private Testing Classes

        /// <summary>
        /// Testing implementation of Models.Metadata.DatItem
        /// </summary>
        private class TestDatItemModel : Models.Metadata.DatItem
        {
            public const string NameKey = "__NAME__";
        }

        /// <summary>
        /// Testing implementation of DatItem
        /// </summary>
        private class TestDatItem : DatItem<TestDatItemModel>
        {
            protected override string? NameKey => TestDatItemModel.NameKey;

            protected override ItemType ItemType => ItemType.Blank;
        }

        #endregion

        #region CopyMachineInformation

        [Fact]
        public void CopyMachineInformation_NewItem_Overwrite()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            var romA = new Rom();

            var romB = new Rom();
            romB.RemoveField(DatItem.MachineKey);

            romA.CopyMachineInformation(romB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Null(actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        [Fact]
        public void CopyMachineInformation_EmptyItem_NoChange()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            var romA = new Rom();
            romA.SetFieldValue(DatItem.MachineKey, machineA);

            var romB = new Rom();
            romB.RemoveField(DatItem.MachineKey);

            romA.CopyMachineInformation(romB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Equal("machineA", actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        [Fact]
        public void CopyMachineInformation_NullMachine_NoChange()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            Machine? machineB = null;

            var romA = new Rom();
            romA.SetFieldValue(DatItem.MachineKey, machineA);

            var romB = new Rom();
            romB.SetFieldValue(DatItem.MachineKey, machineB);

            romA.CopyMachineInformation(romB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Equal("machineA", actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        [Fact]
        public void CopyMachineInformation_EmptyMachine_Overwrite()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            Machine? machineB = new Machine();

            var romA = new Rom();
            romA.SetFieldValue(DatItem.MachineKey, machineA);

            var romB = new Rom();
            romB.SetFieldValue(DatItem.MachineKey, machineB);

            romA.CopyMachineInformation(romB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Null(actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        [Fact]
        public void CopyMachineInformation_FilledMachine_Overwrite()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineB");

            var romA = new Rom();
            romA.SetFieldValue(DatItem.MachineKey, machineA);

            var romB = new Rom();
            romB.SetFieldValue(DatItem.MachineKey, machineB);

            romA.CopyMachineInformation(romB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Equal("machineB", actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        [Fact]
        public void CopyMachineInformation_MismatchedType_Overwrite()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineA");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "machineB");

            var romA = new Rom();
            romA.SetFieldValue(DatItem.MachineKey, machineA);

            var diskB = new Disk();
            diskB.SetFieldValue(DatItem.MachineKey, machineB);

            romA.CopyMachineInformation(diskB);
            var actualMachineA = romA.GetFieldValue<Machine>(DatItem.MachineKey);
            Assert.NotNull(actualMachineA);
            Assert.Equal("machineB", actualMachineA.GetStringFieldValue(Models.Metadata.Machine.NameKey));
        }

        #endregion

        #region CompareTo

        [Fact]
        public void CompareTo_NullOther_Returns1()
        {
            DatItem self = new Rom();
            DatItem? other = null;

            int actual = self.CompareTo(other);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareTo_DifferentOther_Returns1()
        {
            DatItem self = new Rom();
            self.SetName("name");

            DatItem? other = new Disk();
            other.SetName("name");

            int actual = self.CompareTo(other);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void CompareTo_Empty_Returns1()
        {
            DatItem self = new Rom();
            DatItem? other = new Rom();

            int actual = self.CompareTo(other);
            Assert.Equal(1, actual);
        }

        [Theory]
        [InlineData(null, null, 0)]
        [InlineData("name", null, 1)]
        [InlineData("name", "other", -1)]
        [InlineData(null, "name", -1)]
        [InlineData("other", "name", 1)]
        [InlineData("name", "name", 0)]
        public void CompareTo_NamesOnly(string? selfName, string? otherName, int expected)
        {
            DatItem self = new Rom();
            self.SetName(selfName);
            self.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");

            DatItem? other = new Rom();
            other.SetName(otherName);
            other.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");

            int actual = self.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Equals

        [Fact]
        public void Equals_Null_False()
        {
            DatItem self = new TestDatItem();
            DatItem? other = null;

            bool actual = self.Equals(other);
            Assert.False(actual);
        }

        [Fact]
        public void Equals_MismatchedType_False()
        {
            DatItem self = new TestDatItem();
            DatItem? other = new Rom();

            bool actual = self.Equals(other);
            Assert.False(actual);
        }

        [Fact]
        public void Equals_DefaultInternal_True()
        {
            DatItem self = new TestDatItem();
            DatItem? other = new TestDatItem();

            bool actual = self.Equals(other);
            Assert.True(actual);
        }

        [Fact]
        public void Equals_MismatchedInternal_False()
        {
            DatItem self = new TestDatItem();
            self.SetName("self");

            DatItem? other = new TestDatItem();
            other.SetName("other");

            bool actual = self.Equals(other);
            Assert.False(actual);
        }

        [Fact]
        public void Equals_EqualInternal_True()
        {
            DatItem self = new TestDatItem();
            self.SetName("name");

            DatItem? other = new TestDatItem();
            other.SetName("name");

            bool actual = self.Equals(other);
            Assert.True(actual);
        }

        #endregion

        #region GetDuplicateStatus

        [Fact]
        public void GetDuplicateStatus_NullOther_NoDupe()
        {
            DatItem item = new Rom();
            DatItem? lastItem = null;
            var actual = item.GetDuplicateStatus(lastItem);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatus_DifferentTypes_NoDupe()
        {
            var rom = new Rom();
            DatItem? lastItem = new Disk();
            var actual = rom.GetDuplicateStatus(lastItem);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatus_MismatchedHashes_NoDupe()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "BEEFDEAD");
            romA.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.SetFieldValue<Source?>(DatItem.SourceKey, new Source(1));
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatus(romB);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatus_DifferentSource_NameMatch_ExternalAll()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.SetFieldValue<Source?>(DatItem.SourceKey, new Source(1));
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatus(romB);
            Assert.Equal(DupeType.External | DupeType.All, actual);
        }

        [Fact]
        public void GetDuplicateStatus_DifferentSource_NoNameMatch_ExternalHash()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "not-name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.SetFieldValue<Source?>(DatItem.SourceKey, new Source(1));
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatus(romB);
            Assert.Equal(DupeType.External | DupeType.Hash, actual);
        }

        [Fact]
        public void GetDuplicateStatus_SameSource_NameMatch_InternalAll()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatus(romB);
            Assert.Equal(DupeType.Internal | DupeType.All, actual);
        }

        [Fact]
        public void GetDuplicateStatus_SameSource_NoNameMatch_InternalHash()
        {
            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "not-name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.SetFieldValue<Source?>(DatItem.SourceKey, new Source(0));
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatus(romB);
            Assert.Equal(DupeType.Internal | DupeType.Hash, actual);
        }

        #endregion

        // TODO: Change when Machine matching gets fixed
        #region GetDuplicateStatusDB

        [Fact]
        public void GetDuplicateStatusDB_NullOther_NoDupe()
        {
            Source? selfSource = null;
            Source? lastSource = null;

            DatItem item = new Rom();
            DatItem? lastItem = null;
            var actual = item.GetDuplicateStatusDB(selfSource, lastItem, lastSource);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_DifferentTypes_NoDupe()
        {
            Source? selfSource = null;
            Source? lastSource = null;

            var rom = new Rom();
            DatItem? lastItem = new Disk();
            var actual = rom.GetDuplicateStatusDB(selfSource, lastItem, lastSource);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_MismatchedHashes_NoDupe()
        {
            Source? sourceA = new Source(0);
            Source? sourceB = new Source(1);

            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "BEEFDEAD");
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatusDB(sourceA, romB, sourceB);
            Assert.Equal((DupeType)0x00, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_DifferentSource_NameMatch_ExternalAll()
        {
            Source? sourceA = new Source(0);
            Source? sourceB = new Source(1);

            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatusDB(sourceA, romB, sourceB);
            Assert.Equal(DupeType.External | DupeType.All, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_DifferentSource_NoNameMatch_ExternalHash()
        {
            Source? sourceA = new Source(0);
            Source? sourceB = new Source(1);

            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "not-name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatusDB(sourceA, romB, sourceB);
            Assert.Equal(DupeType.External | DupeType.Hash, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_SameSource_NameMatch_InternalAll()
        {
            Source? sourceA = new Source(0);
            Source? sourceB = new Source(0);

            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatusDB(sourceA, romB, sourceB);
            Assert.Equal(DupeType.Internal | DupeType.All, actual);
        }

        [Fact]
        public void GetDuplicateStatusDB_SameSource_NoNameMatch_InternalHash()
        {
            Source? sourceA = new Source(0);
            Source? sourceB = new Source(0);

            Machine? machineA = new Machine();
            machineA.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "name-same");

            Machine? machineB = new Machine();
            machineB.SetFieldValue<string?>(Models.Metadata.Machine.NameKey, "not-name-same");

            var romA = new Rom();
            romA.SetName("same-name");
            romA.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romA.CopyMachineInformation(machineA);

            var romB = new Rom();
            romB.SetName("same-name");
            romB.SetFieldValue(Models.Metadata.Rom.CRCKey, "DEADBEEF");
            romB.CopyMachineInformation(machineB);

            var actual = romA.GetDuplicateStatusDB(sourceA, romB, sourceB);
            Assert.Equal(DupeType.Internal | DupeType.Hash, actual);
        }

        #endregion

        #region PassesFilter

        // TODO: Implement PassesFilter tests
        // Does this need a test? Filter functionality
        // is already covered pretty thoroughly...

        #endregion

        #region GetKey

        // TODO: Implement GetKey tests

        #endregion

        #region GetName

        // TODO: Implement GetName tests

        #endregion

        #region SetName

        // TODO: Implement SetName tests

        #endregion

        #region Clone

        // TODO: Implement Clone tests

        #endregion

        #region GetInternalClone

        // TODO: Implement GetInternalClone tests

        #endregion
    }
}