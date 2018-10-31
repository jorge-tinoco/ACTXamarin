
namespace TaskMobile.Exceptions
{
    /// <summary>
    /// Exceptions severity.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Information.
        /// </summary>
        None,

        /// <summary>
        /// May or not require attention.
        /// </summary>
        Low,

        /// <summary>
        /// Requires attention in the future.
        /// </summary>
        Medium,

        /// <summary>
        /// Requires immediate  attention
        /// </summary>
        High,

        /// <summary>
        /// Is not possible continue working
        /// </summary>
        Intense
    }
}
