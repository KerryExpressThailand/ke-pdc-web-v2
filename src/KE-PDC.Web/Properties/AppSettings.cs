public class AppSettings
{
    public string Fullname { get; set; }
    public string Initials { get; set; }
    public string SiteTitle { get; set; }
    public string CMSApiEndpoint { get; set; }
    public string CMSApiAppId { get; set; }
    public string CMSApiAppKey { get; set; }
    public string KESEDICODApiEndpoint { get; set; }
    public LineNotifyToken LineNotifyToken { get; set; }
}

public class LineNotifyToken
{
    public string PDCCOD1AccountReject { get; set; }
}