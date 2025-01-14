﻿#if NET40_OR_GREATER || NETCOREAPP
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
#if NET40_OR_GREATER || NETCOREAPP
using System.Threading.Tasks;
#endif
using System.Xml.Serialization;
using Newtonsoft.Json;
using SabreTools.Core.Tools;
using SabreTools.DatItems;
using SabreTools.DatItems.Formats;
using SabreTools.Hashing;
using SabreTools.IO.Logging;
using SabreTools.Matching.Compare;

namespace SabreTools.DatFiles
{
    /// <summary>
    /// Item dictionary with statistics, bucketing, and sorting
    /// </summary>
    [JsonObject("items"), XmlRoot("items")]
    public class ItemDictionary
    {
        #region Private instance variables

        /// <summary>
        /// Determine the bucketing key for all items
        /// </summary>
        private ItemKey _bucketedBy;

        /// <summary>
        /// Determine merging type for all items
        /// </summary>
        private DedupeType _mergedBy;

        /// <summary>
        /// Internal dictionary for the class
        /// </summary>
#if NET40_OR_GREATER || NETCOREAPP
        private readonly ConcurrentDictionary<string, List<DatItem>?> _items = [];
#else
        private readonly Dictionary<string, List<DatItem>?> _items = [];
#endif

        /// <summary>
        /// Logging object
        /// </summary>
        private readonly Logger _logger;

        #endregion

        #region Fields

        /// <summary>
        /// Get the keys in sorted order from the file dictionary
        /// </summary>
        /// <returns>List of the keys in sorted order</returns>
        [JsonIgnore, XmlIgnore]
        public string[] SortedKeys
        {
            get
            {
                List<string> keys = [.. _items.Keys];
                keys.Sort(new NaturalComparer());
                return [.. keys];
            }
        }

        /// <summary>
        /// DAT statistics
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public DatStatistics DatStatistics { get; } = new DatStatistics();

        #endregion

        #region Constructors

        /// <summary>
        /// Generic constructor
        /// </summary>
        public ItemDictionary()
        {
            _bucketedBy = ItemKey.NULL;
            _mergedBy = DedupeType.None;
            _logger = new Logger(this);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Add a DatItem to the dictionary after checking
        /// </summary>
        /// <param name="item">Item data to check against</param>
        /// <param name="statsOnly">True to only add item statistics while parsing, false otherwise</param>
        /// <returns>The key for the item</returns>
        public string AddItem(DatItem item, bool statsOnly)
        {
            string key;

            // If we have a Disk, Media, or Rom, clean the hash data
            if (item is Disk disk)
            {
                // If the file has aboslutely no hashes, skip and log
                if (disk.GetStringFieldValue(Models.Metadata.Disk.StatusKey).AsEnumValue<ItemStatus>() != ItemStatus.Nodump
                    && string.IsNullOrEmpty(disk.GetStringFieldValue(Models.Metadata.Disk.MD5Key))
                    && string.IsNullOrEmpty(disk.GetStringFieldValue(Models.Metadata.Disk.SHA1Key)))
                {
                    _logger.Verbose($"Incomplete entry for '{disk.GetName()}' will be output as nodump");
                    disk.SetFieldValue<string?>(Models.Metadata.Disk.StatusKey, ItemStatus.Nodump.AsStringValue());
                }

                item = disk;
            }
            if (item is Media media)
            {
                // If the file has aboslutely no hashes, skip and log
                if (string.IsNullOrEmpty(media.GetStringFieldValue(Models.Metadata.Media.MD5Key))
                    && string.IsNullOrEmpty(media.GetStringFieldValue(Models.Metadata.Media.SHA1Key))
                    && string.IsNullOrEmpty(media.GetStringFieldValue(Models.Metadata.Media.SHA256Key))
                    && string.IsNullOrEmpty(media.GetStringFieldValue(Models.Metadata.Media.SpamSumKey)))
                {
                    _logger.Verbose($"Incomplete entry for '{media.GetName()}' will be output as nodump");
                }

                item = media;
            }
            else if (item is Rom rom)
            {
                long? size = rom.GetInt64FieldValue(Models.Metadata.Rom.SizeKey);

                // If we have the case where there is SHA-1 and nothing else, we don't fill in any other part of the data
                if (size == null && !rom.HasHashes())
                {
                    // No-op, just catch it so it doesn't go further
                    //logger.Verbose($"{Header.GetStringFieldValue(DatHeader.FileNameKey)}: Entry with only SHA-1 found - '{rom.GetName()}'");
                }

                // If we have a rom and it's missing size AND the hashes match a 0-byte file, fill in the rest of the info
                else if ((size == 0 || size == null)
                    && (string.IsNullOrEmpty(rom.GetStringFieldValue(Models.Metadata.Rom.CRCKey)) || rom.HasZeroHash()))
                {
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SizeKey, Constants.SizeZero.ToString());
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.CRCKey, ZeroHash.CRC32Str);
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.MD2Key, null); // ZeroHash.GetString(HashType.MD2)
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.MD4Key, null); // ZeroHash.GetString(HashType.MD4)
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.MD5Key, ZeroHash.MD5Str);
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SHA1Key, ZeroHash.SHA1Str);
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SHA256Key, null); // ZeroHash.SHA256Str;
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SHA384Key, null); // ZeroHash.SHA384Str;
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SHA512Key, null); // ZeroHash.SHA512Str;
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.SpamSumKey, null); // ZeroHash.SpamSumStr;
                }

                // If the file has no size and it's not the above case, skip and log
                else if (rom.GetStringFieldValue(Models.Metadata.Rom.StatusKey).AsEnumValue<ItemStatus>() != ItemStatus.Nodump && (size == 0 || size == null))
                {
                    //logger.Verbose($"{Header.GetStringFieldValue(DatHeader.FileNameKey)}: Incomplete entry for '{rom.GetName()}' will be output as nodump");
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.StatusKey, ItemStatus.Nodump.AsStringValue());
                }

                // If the file has a size but aboslutely no hashes, skip and log
                else if (rom.GetStringFieldValue(Models.Metadata.Rom.StatusKey).AsEnumValue<ItemStatus>() != ItemStatus.Nodump
                    && size != null && size > 0
                    && !rom.HasHashes())
                {
                    //logger.Verbose($"{Header.GetStringFieldValue(DatHeader.FileNameKey)}: Incomplete entry for '{rom.GetName()}' will be output as nodump");
                    rom.SetFieldValue<string?>(Models.Metadata.Rom.StatusKey, ItemStatus.Nodump.AsStringValue());
                }

                item = rom;
            }

            // Get the key and add the file
            key = GetBucketKey(item, _bucketedBy, lower: true, norename: true);

            // If only adding statistics, we add an empty key for games and then just item stats
            if (statsOnly)
            {
                EnsureBucketingKey(key);
                DatStatistics.AddItemStatistics(item);
            }
            else
            {
                AddItem(key, item);
            }

            return key;
        }

        /// <summary>
        /// Remove any keys that have null or empty values
        /// </summary>
        internal void ClearEmpty()
        {
            foreach (string key in SortedKeys)
            {
#if NET40_OR_GREATER || NETCOREAPP
                // If the key doesn't exist, skip
                if (!_items.TryGetValue(key, out var value))
                    continue;

                // If the value is null, remove
                else if (value == null)
                    _items.TryRemove(key, out _);

                // If there are no non-blank items, remove
                else if (value!.FindIndex(i => i != null && i is not Blank) == -1)
                    _items.TryRemove(key, out _);
#else
                // If the key doesn't exist, skip
                if (!_items.ContainsKey(key))
                    continue;

                // If the value is null, remove
                else if (_items[key] == null)
                    _items.Remove(key);

                // If there are no non-blank items, remove
                else if (_items[key]!.FindIndex(i => i != null && i is not Blank) == -1)
                    _items.Remove(key);
#endif
            }
        }

        /// <summary>
        /// Remove all items marked for removal
        /// </summary>
        internal void ClearMarked()
        {
            foreach (string key in SortedKeys)
            {
                // Perform filtering on items
                List<DatItem> list = GetItemsForBucket(key, filter: true);

                // Remove the current list
                RemoveBucket(key);

                // Add the filtered list back
#if NET40_OR_GREATER || NETCOREAPP
                _ = _items.TryAdd(key, list);
#else
                _items[key] = list;
#endif
            }
        }

        /// <summary>
        /// Get if the file dictionary contains the key
        /// </summary>
        /// <param name="key">Key in the dictionary to check</param>
        /// <returns>True if the key exists, false otherwise</returns>
        public bool ContainsKey(string key)
        {
            // If the key is null, we return false since keys can't be null
            if (key == null)
                return false;

            // Explicit lock for some weird corner cases
            lock (key)
            {
                return _items.ContainsKey(key);
            }
        }

        /// <summary>
        /// Ensure the key exists in the items dictionary
        /// </summary>
        /// <param name="key">Key to ensure</param>
        public void EnsureBucketingKey(string key)
        {
            // If the key is missing from the dictionary, add it
#if NET40_OR_GREATER || NETCOREAPP
            _items.GetOrAdd(key, []);
#else
            if (!_items.ContainsKey(key))
                _items[key] = [];
#endif
        }

        /// <summary>
        /// Get the items associated with a bucket name
        /// </summary>
        /// <param name="bucketName">Name of the bucket to retrive items for</param>
        /// <param name="filter">Indicates if RemoveKey filtering is performed</param>
        /// <returns>List representing the bucket items, empty on missing</returns>
        public List<DatItem> GetItemsForBucket(string? bucketName, bool filter = false)
        {
            if (bucketName == null)
                return [];

#if NET40_OR_GREATER || NETCOREAPP
            if (!_items.TryGetValue(bucketName, out var items))
                return [];
#else
            if (!_items.ContainsKey(bucketName))
                return [];

            var items = _items[bucketName];
#endif

            if (items == null)
                return [];

            var datItems = new List<DatItem>();
            foreach (DatItem item in items)
            {
                if (!filter || item.GetBoolFieldValue(DatItem.RemoveKey) != true)
                    datItems.Add(item);
            }

            return datItems;
        }

        /// <summary>
        /// Remove a key from the file dictionary if it exists
        /// </summary>
        /// <param name="key">Key in the dictionary to remove</param>
        public bool RemoveBucket(string key)
        {
#if NET40_OR_GREATER || NETCOREAPP
            bool removed = _items.TryRemove(key, out var list);
#else
            if (!_items.ContainsKey(key))
                return false;

            bool removed = true;
            var list = _items[key];
            _items.Remove(key);
#endif
            if (list == null)
                return removed;

            foreach (var item in list)
            {
                DatStatistics.RemoveItemStatistics(item);
            }

            return removed;
        }

        /// <summary>
        /// Remove the first instance of a value from the file dictionary if it exists
        /// </summary>
        /// <param name="key">Key in the dictionary to remove from</param>
        /// <param name="value">Value to remove from the dictionary</param>
        public bool RemoveItem(string key, DatItem value)
        {
            // Explicit lock for some weird corner cases
            lock (key)
            {
                // If the key doesn't exist, return
#if NET40_OR_GREATER || NETCOREAPP
                if (!_items.TryGetValue(key, out var list) || list == null)
                    return false;
#else
                if (!_items.ContainsKey(key))
                    return false;

                var list = _items[key];
                if (list == null)
                    return false;
#endif

                // If the value doesn't exist in the key, return
                if (!list.Exists(i => i.Equals(value)))
                    return false;

                // Remove the statistics first
                DatStatistics.RemoveItemStatistics(value);

                return list.Remove(value);
            }
        }

        /// <summary>
        /// Override the internal ItemKey value
        /// </summary>
        /// <param name="newBucket"></param>
        public void SetBucketedBy(ItemKey newBucket)
        {
            _bucketedBy = newBucket;
        }

        /// <summary>
        /// Add a value to the file dictionary
        /// </summary>
        /// <param name="key">Key in the dictionary to add to</param>
        /// <param name="value">Value to add to the dictionary</param>
        internal void AddItem(string key, DatItem value)
        {
            // Explicit lock for some weird corner cases
            lock (key)
            {
                // Ensure the key exists
                EnsureBucketingKey(key);

                // If item is null, don't add it
                if (value == null)
                    return;

                // Now add the value
                _items[key]!.Add(value);

                // Now update the statistics
                DatStatistics.AddItemStatistics(value);
            }
        }

        #endregion

        #region Bucketing

        /// <summary>
        /// Take the arbitrarily bucketed Files Dictionary and convert to one bucketed by a user-defined method
        /// </summary>
        /// <param name="bucketBy">ItemKey enum representing how to bucket the individual items</param>
        /// <param name="dedupeType">Dedupe type that should be used</param>
        /// <param name="lower">True if the key should be lowercased (default), false otherwise</param>
        /// <param name="norename">True if games should only be compared on game and file name, false if system and source are counted</param>
        public void BucketBy(ItemKey bucketBy, DedupeType dedupeType, bool lower = true, bool norename = true)
        {
            // If we have a situation where there's no dictionary or no keys at all, we skip
            if (_items == null || _items.Count == 0)
                return;

            // If the sorted type isn't the same, we want to sort the dictionary accordingly
            if (_bucketedBy != bucketBy && bucketBy != ItemKey.NULL)
            {
                _logger.User($"Organizing roms by {bucketBy}");
                PerformBucketing(bucketBy, lower, norename);
            }

            // If the merge type isn't the same, we want to merge the dictionary accordingly
            if (_mergedBy != dedupeType)
            {
                _logger.User($"Deduping roms by {dedupeType}");
                PerformDeduplication(bucketBy, dedupeType);
            }
            // If the merge type is the same, we want to sort the dictionary to be consistent
            else
            {
                _logger.User($"Sorting roms by {bucketBy}");
                PerformSorting();
            }
        }

        /// <summary>
        /// List all duplicates found in a DAT based on a DatItem
        /// </summary>
        /// <param name="datItem">Item to try to match</param>
        /// <param name="sorted">True if the DAT is already sorted accordingly, false otherwise (default)</param>
        /// <returns>List of matched DatItem objects</returns>
        /// <remarks>This also sets the remove flag on any duplicates found</remarks>
        internal List<DatItem> GetDuplicates(DatItem datItem, bool sorted = false)
        {
            // Check for an empty rom list first
            if (DatStatistics.TotalCount == 0)
                return [];

            // We want to get the proper key for the DatItem
            string key = SortAndGetKey(datItem, sorted);

            // Get the items for the current key, if possible
            List<DatItem> roms = GetItemsForBucket(key, filter: false);
            if (roms.Count == 0)
                return [];

            // Remove the current key
            RemoveBucket(key);

            // Try to find duplicates
            List<DatItem> output = [];
            for (int i = 0; i < roms.Count; i++)
            {
                DatItem other = roms[i];
                if (other.GetBoolFieldValue(DatItem.RemoveKey) == true)
                {
                    AddItem(key, other);
                    continue;
                }

                if (datItem.Equals(other))
                {
                    other.SetFieldValue<bool?>(DatItem.RemoveKey, true);
                    output.Add(other);
                }

                AddItem(key, other);
            }

            return output;
        }

        /// <summary>
        /// Check if a DAT contains the given DatItem
        /// </summary>
        /// <param name="datItem">Item to try to match</param>
        /// <param name="sorted">True if the DAT is already sorted accordingly, false otherwise (default)</param>
        /// <returns>True if it contains the rom, false otherwise</returns>
        internal bool HasDuplicates(DatItem datItem, bool sorted = false)
        {
            // Check for an empty rom list first
            if (DatStatistics.TotalCount == 0)
                return false;

            // We want to get the proper key for the DatItem
            string key = SortAndGetKey(datItem, sorted);

            // If the key doesn't exist, return the empty list
            if (!ContainsKey(key))
                return false;

            // Try to find duplicates
            List<DatItem> roms = GetItemsForBucket(key);
            return roms.FindIndex(r => datItem.Equals(r)) > -1;
        }

        /// <summary>
        /// Get the highest-order Field value that represents the statistics
        /// </summary>
        private ItemKey GetBestAvailable()
        {
            // Get the required counts
            long diskCount = DatStatistics.GetItemCount(ItemType.Disk);
            long mediaCount = DatStatistics.GetItemCount(ItemType.Media);
            long romCount = DatStatistics.GetItemCount(ItemType.Rom);
            long nodumpCount = DatStatistics.GetStatusCount(ItemStatus.Nodump);

            // If all items are supposed to have a SHA-512, we bucket by that
            if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.SHA512))
                return ItemKey.SHA512;

            // If all items are supposed to have a SHA-384, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.SHA384))
                return ItemKey.SHA384;

            // If all items are supposed to have a SHA-256, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.SHA256))
                return ItemKey.SHA256;

            // If all items are supposed to have a SHA-1, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.SHA1))
                return ItemKey.SHA1;

            // If all items are supposed to have a MD5, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.MD5))
                return ItemKey.MD5;

            // If all items are supposed to have a MD4, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.MD4))
                return ItemKey.MD4;

            // If all items are supposed to have a MD2, we bucket by that
            else if (diskCount + mediaCount + romCount - nodumpCount == DatStatistics.GetHashCount(HashType.MD2))
                return ItemKey.MD2;

            // Otherwise, we bucket by CRC
            else
                return ItemKey.CRC;
        }

        /// <summary>
        /// Get the bucketing key for a given item
        /// <param name="datItem">The current item</param>
        /// <param name="bucketBy">ItemKey value representing what key to get</param>
        /// <param name="lower">True if the key should be lowercased, false otherwise</param>
        /// <param name="norename">True if games should only be compared on game and file name, false if system and source are counted</param>
        /// </summary>
        private static string GetBucketKey(DatItem datItem, ItemKey bucketBy, bool lower, bool norename)
        {
            if (datItem == null)
                return string.Empty;

            // Treat NULL like machine
            if (bucketBy == ItemKey.NULL)
                bucketBy = ItemKey.Machine;

            // Get the bucket key
            return datItem.GetKey(bucketBy, lower, norename);
        }

        /// <summary>
        /// Perform bucketing based on the item key provided
        /// </summary>
        /// <param name="bucketBy">ItemKey enum representing how to bucket the individual items</param>
        /// <param name="lower">True if the key should be lowercased, false otherwise</param>
        /// <param name="norename">True if games should only be compared on game and file name, false if system and source are counted</param>
        private void PerformBucketing(ItemKey bucketBy, bool lower, bool norename)
        {
            // Set the sorted type
            _bucketedBy = bucketBy;

            // Reset the merged type since this might change the merge
            _mergedBy = DedupeType.None;

            // First do the initial sort of all of the roms inplace
            List<string> oldkeys = [.. SortedKeys];

#if NET452_OR_GREATER || NETCOREAPP
            Parallel.For(0, oldkeys.Count, Core.Globals.ParallelOptions, k =>
#elif NET40_OR_GREATER
            Parallel.For(0, oldkeys.Count, k =>
#else
            for (int k = 0; k < oldkeys.Count; k++)
#endif
            {
                string key = oldkeys[k];
                if (GetItemsForBucket(key).Count == 0)
                    RemoveBucket(key);

                // Now add each of the roms to their respective keys
                for (int i = 0; i < GetItemsForBucket(key).Count; i++)
                {
                    DatItem item = GetItemsForBucket(key)[i];
                    if (item == null)
                        continue;

                    // We want to get the key most appropriate for the given sorting type
                    string newkey = item.GetKey(bucketBy, lower, norename);

                    // If the key is different, move the item to the new key
                    if (newkey != key)
                    {
                        AddItem(newkey, item);
                        bool removed = RemoveItem(key, item);
                        if (!removed)
                            break;

                        i--; // This make sure that the pointer stays on the correct since one was removed
                    }
                }

                // If the key is now empty, remove it
                if (GetItemsForBucket(key).Count == 0)
                    RemoveBucket(key);
#if NET40_OR_GREATER || NETCOREAPP
            });
#else
            }
#endif
        }

        /// <summary>
        /// Perform deduplication based on the deduplication type provided
        /// </summary>
        /// <param name="bucketBy">ItemKey enum representing how to bucket the individual items</param>
        /// <param name="dedupeType">Dedupe type that should be used</param>
        private void PerformDeduplication(ItemKey bucketBy, DedupeType dedupeType)
        {
            // Set the sorted type
            _mergedBy = dedupeType;

#if NET452_OR_GREATER || NETCOREAPP
            Parallel.ForEach(SortedKeys, Core.Globals.ParallelOptions, key =>
#elif NET40_OR_GREATER
            Parallel.ForEach(SortedKeys, key =>
#else
            foreach (var key in SortedKeys)
#endif
            {
                // Get the possibly unsorted list
                List<DatItem> sortedList = GetItemsForBucket(key);

                // Sort the list of items to be consistent
                DatFileTool.Sort(ref sortedList, false);

                // If we're merging the roms, do so
                if (dedupeType == DedupeType.Full || (dedupeType == DedupeType.Game && bucketBy == ItemKey.Machine))
                    sortedList = DatFileTool.Merge(sortedList);

                // Add the list back to the dictionary
                RemoveBucket(key);
                sortedList.ForEach(item => AddItem(key, item));
#if NET40_OR_GREATER || NETCOREAPP
            });
#else
            }
#endif
        }

        /// <summary>
        /// Perform inplace sorting of the dictionary
        /// </summary>
        private void PerformSorting()
        {
#if NET452_OR_GREATER || NETCOREAPP
            Parallel.ForEach(SortedKeys, Core.Globals.ParallelOptions, key =>
#elif NET40_OR_GREATER
            Parallel.ForEach(SortedKeys, key =>
#else
            foreach (var key in SortedKeys)
#endif
            {
                // Get the possibly unsorted list
                List<DatItem> sortedList = GetItemsForBucket(key);

                // Sort the list of items to be consistent
                DatFileTool.Sort(ref sortedList, false);

                // Add the list back to the dictionary
                RemoveBucket(key);
                sortedList.ForEach(item => AddItem(key, item));
#if NET40_OR_GREATER || NETCOREAPP
            });
#else
            }
#endif
        }

        /// <summary>
        /// Sort the input DAT and get the key to be used by the item
        /// </summary>
        /// <param name="datItem">Item to try to match</param>
        /// <param name="sorted">True if the DAT is already sorted accordingly, false otherwise (default)</param>
        /// <returns>Key to try to use</returns>
        private string SortAndGetKey(DatItem datItem, bool sorted = false)
        {
            // If we're not already sorted, take care of it
            if (!sorted)
                BucketBy(GetBestAvailable(), DedupeType.None);

            // Now that we have the sorted type, we get the proper key
            return GetBucketKey(datItem, _bucketedBy, lower: true, norename: true);
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Recalculate the statistics for the Dat
        /// </summary>
        public void RecalculateStats()
        {
            // Wipe out any stats already there
            DatStatistics.ResetStatistics();

            // If we have a blank Dat in any way, return
            if (_items == null)
                return;

            // Loop through and add
            foreach (string key in _items.Keys)
            {
                List<DatItem>? datItems = _items[key];
                if (datItems == null)
                    continue;

                foreach (DatItem item in datItems)
                {
                    DatStatistics.AddItemStatistics(item);
                }
            }
        }

        #endregion
    }
}
