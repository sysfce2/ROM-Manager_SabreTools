namespace SabreTools.Models.Listrom
{
    public class Dat
    {
        public Set[]? Set { get; set; }

        #region DO NOT USE IN PRODUCTION

        /// <remarks>Should be empty</remarks>
        public string[]? ADDITIONAL_ELEMENTS { get; set; }

        #endregion
    }
}