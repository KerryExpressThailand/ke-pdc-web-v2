declare @report_date varchar(8);
declare @branch_id varchar(10);

set @report_date ='20170329';
set @branch_id ='LPDU'

--Cover Page
--------------------------------------------------------------------------
SELECT b.BranchID as branch_id
,b.BranchName as branch_name
,b.branch_type AS shop_type
,FORMAT(Report_Date, 'dd-MMMM-yyyy', 'th') AS [date]
,ISNULL(TotalTransfer, 0) AS tot_transafer
,ISNULL(Shipment, 0) AS shipment
,ISNULL(Boxes, 0) AS boxes
,ISNULL(TransportService, 0) AS transport
,ISNULL(AMService, 0) AS am
,ISNULL(PUPService, 0) AS pup
,ISNULL(SATService, 0) AS sat
,ISNULL(RASService, 0) AS ras
,ISNULL(CODService, 0) AS cod
,ISNULL(INSURService, 0) AS insur
,ISNULL(PACKAGEService, 0) AS pkg
,ISNULL(SALEService, 0) AS sale_pkg
,ISNULL(LNTUPService, 0) AS topup
,ISNULL(DHLService, 0) AS dhl
,(
(ISNULL(TransportService, 0)
+ISNULL(AMService, 0)
+ISNULL(PUPService, 0)
+ISNULL(SATService, 0)
+ISNULL(RASService, 0)
+ISNULL(CODService, 0)
+ISNULL(INSURService, 0)
+ISNULL(PACKAGEService, 0)
+ISNULL(SALEService, 0)
+ISNULL(LNTUPService, 0))
-ISNULL(Discount, 0)) AS tot_service
,ISNULL(Cash, 0) AS cash
,ISNULL(Rabbit, 0) AS rabbit
,ISNULL(Credit, 0) AS credit
,ISNULL(LinePay, 0) AS linepay
,(ISNULL(Cash, 0)
+ISNULL(Rabbit, 0)
+ISNULL(Credit, 0)
+ISNULL(LinePay, 0)) AS tot_pay

,ISNULL(Transportation, 0) AS transportation
,ISNULL(VASSurcharge, 0) AS vas
,ISNULL(VAT, 0) AS vat
,ISNULL(Discount, 0) AS disc
,(
(ISNULL(Transportation, 0)
+ISNULL(VASSurcharge, 0)
+ISNULL(VAT, 0)
)
-ISNULL(Discount, 0)) AS tot_surcharge

,(ISNULL(TransportService, 0)
+ISNULL(AMService, 0)
+ISNULL(PUPService, 0)
+ISNULL(SATService, 0)
+ISNULL(RASService, 0)) AS tot_freight
FROM tb_EOD AS o
LEFT JOIN tb_Branch AS b ON b.BranchID = o.BranchID
WHERE	o.BranchID = @branch_id
AND CONVERT(VARCHAR, Report_Date,112) = @report_date;
					
--Shipment
--------------------------------------------------------------------------
select s.Consignment_No as con_no,
s.Sender_Name as s_name,
s.QTY as qty,
s.Weight as wt,
s.Service_Code as sc,
s.cod_amount as cod,
s.declare_value as [dec],
s.Surcharge as sur,
s.VasSurcharge as vas,
s.TotalDiscount as disc,
s.total_vat as vat,
s.ReceiptNo as receipt_no,
format(s.Created_Date,'dd/MM') as [date],
r.ReceivedType as  [type],
case when s.pickup_date is null then '' else 'X' end as sop
from tb_Shipment as s with(nolock)
inner join tb_ReceiptHead as r
on s.ReceiptNo = r.ReceiptNo
where s.ReceiptNo is not null
and s.[Status] <> 3
and s.BranchID=@branch_id
and convert(varchar,Created_Date,112)=@report_date
order by s.Consignment_No

--Shipment Footer
--------------------------------------------------------------------------
SELECT b.BranchID as branch_id
,ISNULL(Shipment, 0) AS total_shipment
,ISNULL(Boxes, 0) AS total_boxes
,ISNULL(Cash, 0) AS cash
,ISNULL(Rabbit, 0) AS rabbit
,ISNULL(Credit, 0) AS credit
,ISNULL(LinePay, 0) AS linepay
,ISNULL(Transportation, 0) AS total_surcharge
,ISNULL(VASSurcharge, 0) AS total_vas_surcharge
,ISNULL(Discount, 0) AS total_discount
,ISNULL(VAT, 0) AS total_vat
,(
(ISNULL(Transportation, 0)
+ISNULL(VASSurcharge, 0)
+ISNULL(VAT, 0)
)
-ISNULL(Discount, 0)) AS total,
isnull(TotalTransfer,0) as total_transfer
FROM tb_EOD AS o
LEFT JOIN tb_Branch AS b ON b.BranchID = o.BranchID
WHERE	o.BranchID = @branch_id
AND CONVERT(VARCHAR, Report_Date,112) = @report_date;

--None Shipment
--------------------------------------------------------------------------
select n.VASID as vas_id,
isnull(t.TaxInvoiceNo,'') as taxinvoice_no,
n.Remark as remark,
n.qty,
n.unit_price,
n.tot_vat,
n.created_by,
format(created_datetime,'dd/MM HH:mm') created_date
from tb_NoneShipment as n with(nolock)
left join tb_TaxInvoice as t with(nolock)
on n.TaxInvoiceNo = t.TaxInvoiceNo
where n.[status]=1
and isnull(n.canceled,0)=0
and convert(varchar,n.created_datetime,112)=@report_date
and n.BranchID=@branch_id
order by n.VASID


--None Shipment Footer
--------------------------------------------------------------------------
declare @tempTable table(topup money,pkg money,dhl money);
declare @tot_topup money;
declare @tot_pkg money;
declare @tot_dhl money;

insert into @tempTable values(0,0,0)

select @tot_topup=isnull(sum(n.tot_vat+n.before_vat),0)
from tb_NoneShipment as n with(nolock)
left join tb_TaxInvoice as t with(nolock)
on n.TaxInvoiceNo = t.TaxInvoiceNo
where n.[status]=1
and isnull(n.canceled,0)=0
and convert(varchar,n.created_datetime,112)=@report_date
and n.BranchID=@branch_id
and n.VASID ='LNTUP'

update @tempTable set topup=@tot_topup

select @tot_pkg=isnull(sum(n.tot_vat+n.before_vat),0)
from tb_NoneShipment as n with(nolock)
left join tb_TaxInvoice as t with(nolock)
on n.TaxInvoiceNo = t.TaxInvoiceNo
where n.[status]=1
and isnull(n.canceled,0)=0
and convert(varchar,n.created_datetime,112)=@report_date
and n.BranchID=@branch_id
and left(n.VASID,3) ='PKG'

update @tempTable set pkg=@tot_pkg

select @tot_dhl=isnull(sum(n.tot_vat+n.before_vat),0)
from tb_NoneShipment as n with(nolock)
left join tb_TaxInvoice as t with(nolock)
on n.TaxInvoiceNo = t.TaxInvoiceNo
where n.[status]=1
and isnull(n.canceled,0)=0
and convert(varchar,n.created_datetime,112)=@report_date
and n.BranchID=@branch_id
and n.VASID ='DHL'

update @tempTable set dhl=@tot_dhl

select * from @tempTable