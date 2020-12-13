﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using SabreTools.Core;
using SabreTools.Core.Tools;
using SabreTools.Filtering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SabreTools.DatItems
{
    /// <summary>
    /// Represents which Chip(s) is associated with a set
    /// </summary>
    [JsonObject("chip"), XmlRoot("chip")]
    public class Chip : DatItem
    {
        #region Fields

        /// <summary>
        /// Name of the item
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Internal tag
        /// </summary>
        [JsonProperty("tag", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Type of the chip
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        [XmlElement("type")]
        public ChipType ChipType { get; set; }

        [JsonIgnore]
        public bool ChipTypeSpecified { get { return ChipType != ChipType.NULL; } }

        /// <summary>
        /// Clock speed
        /// </summary>
        [JsonProperty("clock", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("clock")]
        public long? Clock { get; set; }

        [JsonIgnore]
        public bool ClockTypeSpecified { get { return Clock != null; } }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the name to use for a DatItem
        /// </summary>
        /// <returns>Name if available, null otherwise</returns>
        public override string GetName()
        {
            return Name;
        }

        /// <inheritdoc/>
        public override void SetFields(
            Dictionary<DatItemField, string> datItemMappings,
            Dictionary<MachineField, string> machineMappings)
        {
            // Set base fields
            base.SetFields(datItemMappings, machineMappings);

            // Handle Chip-specific fields
            if (datItemMappings.Keys.Contains(DatItemField.Name))
                Name = datItemMappings[DatItemField.Name];

            if (datItemMappings.Keys.Contains(DatItemField.Tag))
                Tag = datItemMappings[DatItemField.Tag];

            if (datItemMappings.Keys.Contains(DatItemField.ChipType))
                ChipType = datItemMappings[DatItemField.ChipType].AsChipType();

            if (datItemMappings.Keys.Contains(DatItemField.Clock))
                Clock = Utilities.CleanLong(datItemMappings[DatItemField.Clock]);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a default, empty Chip object
        /// </summary>
        public Chip()
        {
            Name = string.Empty;
            ItemType = ItemType.Chip;
        }

        #endregion

        #region Cloning Methods

        public override object Clone()
        {
            return new Chip()
            {
                ItemType = this.ItemType,
                DupeType = this.DupeType,

                Machine = this.Machine.Clone() as Machine,
                Source = this.Source.Clone() as Source,
                Remove = this.Remove,

                Name = this.Name,
                Tag = this.Tag,
                ChipType = this.ChipType,
                Clock = this.Clock,
            };
        }

        #endregion

        #region Comparision Methods

        public override bool Equals(DatItem other)
        {
            // If we don't have a chip, return false
            if (ItemType != other.ItemType)
                return false;

            // Otherwise, treat it as a chip
            Chip newOther = other as Chip;

            // If the chip information matches
            return (Name == newOther.Name
                && Tag == newOther.Tag
                && ChipType == newOther.ChipType
                && Clock == newOther.Clock);
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Clean a DatItem according to the cleaner
        /// </summary>
        /// <param name="cleaner">Cleaner to implement</param>
        public override void Clean(Cleaner cleaner)
        {
            // Clean common items first
            base.Clean(cleaner);

            // If we're stripping unicode characters, strip item name
            if (cleaner?.RemoveUnicode == true)
                Name = RemoveUnicodeCharacters(Name);

            // If we are in NTFS trim mode, trim the game name
            if (cleaner?.Trim == true)
            {
                // Windows max name length is 260
                int usableLength = 260 - Machine.Name.Length - (cleaner.Root?.Length ?? 0);
                if (Name.Length > usableLength)
                {
                    string ext = Path.GetExtension(Name);
                    Name = Name.Substring(0, usableLength - ext.Length);
                    Name += ext;
                }
            }
        }

        /// <inheritdoc/>
        public override bool PassesFilter(Cleaner cleaner, bool sub = false)
        {
            // Check common fields first
            if (!base.PassesFilter(cleaner, sub))
                return false;

            // Filter on item name
            if (!Filter.PassStringFilter(cleaner.DatItemFilter.Name, Name))
                return false;

            // DatItem_Tag
            if (!Filter.PassStringFilter(cleaner.DatItemFilter.Tag, Tag))
                return false;

            // DatItem_ChipType
            if (cleaner.DatItemFilter.ChipType.MatchesPositive(ChipType.NULL, ChipType) == false)
                return false;
            if (cleaner.DatItemFilter.ChipType.MatchesNegative(ChipType.NULL, ChipType) == true)
                return false;

            // DatItem_Clock
            if (!Filter.PassLongFilter(cleaner.DatItemFilter.Clock, Clock))
                return false;

            return true;
        }

        /// <inheritdoc/>
        public override void RemoveFields(
            List<DatItemField> datItemFields,
            List<MachineField> machineFields)
        {
            // Remove common fields first
            base.RemoveFields(datItemFields, machineFields);

            // Remove the fields
            if (datItemFields.Contains(DatItemField.Name))
                Name = null;

            if (datItemFields.Contains(DatItemField.Tag))
                Tag = null;

            if (datItemFields.Contains(DatItemField.ChipType))
                ChipType = ChipType.NULL;

            if (datItemFields.Contains(DatItemField.Clock))
                Clock = null;
        }

        /// <summary>
        /// Set internal names to match One Rom Per Game (ORPG) logic
        /// </summary>
        public override void SetOneRomPerGame()
        {
            string[] splitname = Name.Split('.');
            Machine.Name += $"/{string.Join(".", splitname.Take(splitname.Length > 1 ? splitname.Length - 1 : 1))}";
            Name = Path.GetFileName(Name);
        }

        #endregion

        #region Sorting and Merging

        /// <inheritdoc/>
        public override void ReplaceFields(
            DatItem item,
            List<DatItemField> datItemFields,
            List<MachineField> machineFields)
        {
            // Replace common fields first
            base.ReplaceFields(item, datItemFields, machineFields);

            // If we don't have a Chip to replace from, ignore specific fields
            if (item.ItemType != ItemType.Chip)
                return;

            // Cast for easier access
            Chip newItem = item as Chip;

            // Replace the fields
            if (datItemFields.Contains(DatItemField.Name))
                Name = newItem.Name;

            if (datItemFields.Contains(DatItemField.Tag))
                Tag = newItem.Tag;

            if (datItemFields.Contains(DatItemField.ChipType))
                ChipType = newItem.ChipType;

            if (datItemFields.Contains(DatItemField.Clock))
                Clock = newItem.Clock;
        }

        #endregion
    }
}
