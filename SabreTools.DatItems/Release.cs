﻿using System.Collections.Generic;
using System.Xml.Serialization;

using SabreTools.Core;
using Newtonsoft.Json;

namespace SabreTools.DatItems
{
    /// <summary>
    /// Represents release information about a set
    /// </summary>
    [JsonObject("release"), XmlRoot("release")]
    public class Release : DatItem
    {
        #region Fields

        /// <summary>
        /// Name of the item
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Release region(s)
        /// </summary>
        [JsonProperty("region", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("region")]
        public string Region { get; set; }

        /// <summary>
        /// Release language(s)
        /// </summary>
        [JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("language")]
        public string Language { get; set; }

        /// <summary>
        /// Date of release
        /// </summary>
        [JsonProperty("date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("date")]
        public string Date { get; set; }

        /// <summary>
        /// Default release, if applicable
        /// </summary>
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("default")]
        public bool? Default { get; set; }

        [JsonIgnore]
        public bool DefaultSpecified { get { return Default != null; } }

        #endregion

        #region Accessors

        /// <inheritdoc/>
        public override string GetName() => Name;

        /// <inheritdoc/>
        public override void SetName(string name) => Name = name;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a default, empty Release object
        /// </summary>
        public Release()
        {
            Name = string.Empty;
            ItemType = ItemType.Release;
            Region = string.Empty;
            Language = string.Empty;
            Date = string.Empty;
            Default = null;
        }

        #endregion

        #region Cloning Methods

        public override object Clone()
        {
            return new Release()
            {
                ItemType = this.ItemType,
                DupeType = this.DupeType,

                Machine = this.Machine.Clone() as Machine,
                Source = this.Source.Clone() as Source,
                Remove = this.Remove,

                Name = this.Name,
                Region = this.Region,
                Language = this.Language,
                Date = this.Date,
                Default = this.Default,
            };
        }

        #endregion

        #region Comparision Methods

        public override bool Equals(DatItem other)
        {
            // If we don't have a release return false
            if (ItemType != other.ItemType)
                return false;

            // Otherwise, treat it as a Release
            Release newOther = other as Release;

            // If the archive information matches
            return (Name == newOther.Name
                && Region == newOther.Region
                && Language == newOther.Language
                && Date == newOther.Date
                && Default == newOther.Default);
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

            // If we don't have a Release to replace from, ignore specific fields
            if (item.ItemType != ItemType.Release)
                return;

            // Cast for easier access
            Release newItem = item as Release;

            // Replace the fields
            if (datItemFields.Contains(DatItemField.Name))
                Name = newItem.Name;

            if (datItemFields.Contains(DatItemField.Region))
                Region = newItem.Region;

            if (datItemFields.Contains(DatItemField.Language))
                Language = newItem.Language;

            if (datItemFields.Contains(DatItemField.Date))
                Date = newItem.Date;

            if (datItemFields.Contains(DatItemField.Default))
                Default = newItem.Default;
        }

        #endregion
    }
}
