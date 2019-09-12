namespace csharp Manulife.DNC.MSAD.Contracts

service PaymentService { 
    TrxnResult Save(1:TrxnRecord trxn) 
}

enum TrxnResult { 
    SUCCESS = 0, 
    FAILED = 1, 
}

struct TrxnRecord { 
    1: required i64 TrxnId; 
    2: required string TrxnName; 
    3: required i32 TrxnAmount; 
    4: required string TrxnType; 
    5: optional string Remark; 
}