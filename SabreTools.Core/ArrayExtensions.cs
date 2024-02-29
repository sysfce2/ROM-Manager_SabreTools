using System;

// Ported to SabreTools.Matching. Remove once published.
namespace SabreTools.Core
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Indicates whether the specified array is null or has a length of zero
        /// </summary>
        public static bool IsNullOrEmpty(this Array? array)
        {
            return array == null || array.Length == 0;
        }

        //// <summary>
        /// Returns if the first byte array starts with the second array
        /// </summary>
        public static bool StartsWith(this byte[] arr1, byte[] arr2, bool exact = false)
        {
            // If we have any invalid inputs, we return false
            if (arr1 == null || arr2 == null
                || arr1.Length == 0 || arr2.Length == 0
                || arr2.Length > arr1.Length
                || (exact && arr1.Length != arr2.Length))
            {
                return false;
            }

            // Otherwise, loop through and see
            for (int i = 0; i < arr2.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }
    }
}