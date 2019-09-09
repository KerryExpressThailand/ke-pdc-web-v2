declare @branch_id varchar(10)
set @branch_id='BYAI';
declare @report_date varchar(8)
set @report_date='20170424';

Select '' AS booking_no, 
 s.Consignment_No As consignment_no, 
 CASE WHEN isnull(s.cod_amount, 0) > 0 THEN s.cod_account_id ELSE '' END as ref_no, 
 p.DMSID as payerid, 
 p.Sender_Name As payer_name, 
 p.Sender_HomeAddress + ' ' + p.Sender_Road as payer_address1, 
             p.Sender_District + ' ' + p.Sender_Amphur + ' ' + p.Sender_Province as payer_address2, 
             p.Sender_PostalCode as payer_zipcode, 
 p.Sender_Mobile1 As payer_telephone, 
 '' as payer_fax, 
             'R' as payment_mode, 
             s.Sender_Name as sender_name, 
 p.Sender_HomeAddress + ' ' + p.Sender_Road as sender_address1, 
 p.Sender_District + ' ' + p.Sender_Amphur + ' ' + p.Sender_Province as sender_address2, 
 p.Sender_PostalCode As sender_zipcode, 
 s.Sender_Mobile1 
	+ CASE WHEN s.Sender_Mobile2<>'' THEN ';' + s.Sender_Mobile2 ELSE '' END 
	+ CASE WHEN s.Sender_Telephone<>'' THEN ';' + s.Sender_Telephone ELSE '' END 
	+ ';' as  sender_telephone, 
 '' as sender_fax, 
 s.Sender_Name as sender_contact_person, 
 s.Recipient_Name As recipient_name, 
 s.Recipient_Address 
	+ CASE WHEN s.Recipient_Village<>'' THEN ' หมู่บ้าน/อาคาร ' + s.Recipient_Village ELSE '' END 
	+ CASE WHEN s.Recipient_Moo<>'' THEN ' หมู่ที่ ' + s.Recipient_Moo ELSE '' END 
	+ CASE WHEN s.Recipient_Soi<>'' THEN ' ซอย ' + s.Recipient_Soi ELSE '' END 
	+ CASE WHEN s.Recipient_Road<>'' THEN ' ถนน ' + s.Recipient_Road ELSE '' END 
	as recipient_address1, 
 s.Recipient_District 
	+ ' ' + s.Recipient_Amphur 
	+ ' ' + s.Recipient_Province as recipient_address2, 
 s.Recipient_PostalCode as recipient_zipcode, 
 s.Recipient_Mobile1 
	+ CASE WHEN s.Recipient_Mobile2<>'' THEN ';' + s.Recipient_Mobile2 ELSE '' END 
	+ CASE WHEN s.Recipient_Telephone<>'' THEN ';' + s.Recipient_Telephone ELSE '' END 
	+ ';' as recipient_telephone, 
 '' as recipient_fax, 
 Case WHEN s.Recipient_ContactPerson<>'' THEN s.Recipient_ContactPerson ELSE s.Recipient_Name END 
	as recipient_contact_person, 
 s.Service_Code as service_code, 
 CAST(isnull(s.declare_value, 0) As varchar) As declare_value, 
 '' as payment_type, 
                 '' as commodity_code, 
                 s.Remark as remark, 
 CAST(isnull(s.cod_amount, 0) As varchar) As cod_amount, 
 'N' as return_pod_hc, 
                 'N' as return_invoice_hc 
                 From dbo.tb_CutOffDetail as d with (nolock) 
 INNER Join dbo.tb_Shipment as s with (nolock) 
 On d.Consignment_No = s.Consignment_No 
 INNER Join dbo.tb_Branch as p with (nolock) 
 On s.BranchID = p.BranchID 
 WHERE Convert(varchar, d.ScannedTime, 112) = @report_date
 and p.BranchID=@branch_id
--Consignments


SELECT d.Consignment_No as consignment_no,
 pd.Pkg_No As mps_code,
 p.Length As pkg_length,	
 p.Breadth as pkg_breadth,
 p.Height as pkg_height,
 ROUND(pd.Weight,2) as pkg_wt,
 1 as pkg_qty
 FROM dbo.tb_CutOffDetail as d with (nolock)
 INNER JOIN dbo.tb_Shipment as s with (nolock)
 ON d.Consignment_No = s.Consignment_No
 inner join dbo.tb_PackageDetail as pd with (nolock)
 on s.Consignment_No = pd.Consignment_No
 inner join dbo.tb_Package as p with (nolock)
 on pd.PackageType = p.PackageID
 WHERE convert(varchar,d.ScannedTime,112) =@report_date
 and d.BranchID=@branch_id
 order by d.Consignment_No,pd.Pkg_No;
--Packages