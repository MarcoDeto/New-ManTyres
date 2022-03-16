namespace ManTyres.COMMON.Utils
{
    public enum SubscriptionType
    {
        None,
        WAS,
        WSSplus,
        WSS
    }

    public enum QuotationStatus
    {
        Pending, // 0
        Accepted, // 1
        Forwarded, // 3 => 2
        Paid, // 4 => 3
        ActivationRequested, // 5 => 4
        Activated, // 5 => 6 => 5
        EURejected, // 6 => 7 => 6
        VSGRejected // 7 => 8 => 7  
    }

    public enum LogType
    {
        AssociatedDIx,
        AssociatedTE,
        AssociatedWS,
        AssociatedSUB,
        RegisteredMaintenance,
        RelocatedEquipment,
        RequestedQuotation
    }

    public enum UserRole
    {
        Administrator,
        Manager,
        Marketer,
        Worker,
        Distributor,
        Customer,
        Developer
    }

    public enum ClaimStatus
    {
        Draft,
        Check,
        Open,
        Working,
        WorkingReqInfo,
        ClosedRefused,
        ClosedApproved
    }

   public enum DistStatus
   {
      Active,
      Inactive,
      Incomplete
   }

    public enum Language
    {
        de,
        en,
        es,
        fr,
        it
    }

    public enum DataBridgeStatusCode
    {
        Ok = 200
    }
}
