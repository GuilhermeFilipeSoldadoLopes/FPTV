namespace FPTV.Models
{
    /// <summary>
    /// ErrorViewModel is a class that contains properties to store information about an error.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the RequestId should be shown.
        /// </summary>
        /// <returns>True if the RequestId is not empty or null, otherwise false.</returns>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}