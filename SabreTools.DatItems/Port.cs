﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using SabreTools.Core;
using SabreTools.Filtering;
using Newtonsoft.Json;

namespace SabreTools.DatItems
{
    /// <summary>
    /// Represents a single port on a machine
    /// </summary>
    [JsonObject("port"), XmlRoot("port")]
    public class Port : DatItem
    {
        #region Fields

        /// <summary>
        /// Tag for the port
        /// </summary>
        [JsonProperty("tag", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// List of analogs on the port
        /// </summary>
        [JsonProperty("analogs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("analogs")]
        public List<Analog> Analogs { get; set; }

        [JsonIgnore]
        public bool AnalogsSpecified { get { return Analogs != null && Analogs.Count > 0; } }

        #endregion

        #region Accessors

        /// <inheritdoc/>
        public override void SetFields(
            Dictionary<DatItemField, string> datItemMappings,
            Dictionary<MachineField, string> machineMappings)
        {
            // Set base fields
            base.SetFields(datItemMappings, machineMappings);

            // Handle Port-specific fields
            if (datItemMappings.Keys.Contains(DatItemField.Tag))
                Tag = datItemMappings[DatItemField.Tag];

            if (AnalogsSpecified)
            {
                foreach (Analog analog in Analogs)
                {
                    analog.SetFields(datItemMappings, machineMappings);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a default, empty Port object
        /// </summary>
        public Port()
        {
            ItemType = ItemType.Port;
        }

        #endregion

        #region Cloning Methods

        public override object Clone()
        {
            return new Port()
            {
                ItemType = this.ItemType,
                DupeType = this.DupeType,

                Machine = this.Machine.Clone() as Machine,
                Source = this.Source.Clone() as Source,
                Remove = this.Remove,

                Tag = this.Tag,
                Analogs = this.Analogs,
            };
        }

        #endregion

        #region Comparision Methods

        public override bool Equals(DatItem other)
        {
            // If we don't have a Port, return false
            if (ItemType != other.ItemType)
                return false;

            // Otherwise, treat it as a Port
            Port newOther = other as Port;

            // If the Port information matches
            bool match = (Tag == newOther.Tag);
            if (!match)
                return match;

            // If the analogs match
            if (AnalogsSpecified)
            {
                foreach (Analog analog in Analogs)
                {
                    match &= newOther.Analogs.Contains(analog);
                }
            }

            return match;
        }

        #endregion

        #region Filtering

        /// <inheritdoc/>
        public override bool PassesFilter(Cleaner cleaner, bool sub = false)
        {
            // Check common fields first
            if (!base.PassesFilter(cleaner, sub))
                return false;

            // Filter on tag
            if (!Filter.PassStringFilter(cleaner.DatItemFilter.Tag, Tag))
                return false;

            // Filter on individual analogs
            if (AnalogsSpecified)
            {
                foreach (Analog analog in Analogs)
                {
                    if (!analog.PassesFilter(cleaner, true))
                        return false;
                }
            }

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
            if (datItemFields.Contains(DatItemField.Tag))
                Tag = null;

            if (AnalogsSpecified)
            {
                foreach (Analog analog in Analogs)
                {
                    analog.RemoveFields(datItemFields, machineFields);
                }
            }
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

            // If we don't have a Port to replace from, ignore specific fields
            if (item.ItemType != ItemType.Port)
                return;

            // Cast for easier access
            Port newItem = item as Port;

            // Replace the fields
            if (datItemFields.Contains(DatItemField.Name))
                Tag = newItem.Tag;

            // DatItem_Analog_* doesn't make sense here
            // since not every analog under the other item
            // can replace every analog under this item
        }

        #endregion
    }
}
