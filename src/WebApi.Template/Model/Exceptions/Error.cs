namespace WebApi.Template.Model.Exceptions
{
    public class Error
    {
        /// <summary>
        /// Holds Field Name for Validation Exception, 
        /// Operation Name for Business exception
        /// </summary>
        public string ErrorKey { get; set; }
        /// <summary>
        /// Code which will be Identify by UI [Can be used for Transalations]
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Populate Exact Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
