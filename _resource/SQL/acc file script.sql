declare @export_date varchar(8);
declare @export_from varchar(8);
declare @branch_id varchar(10);
set @branch_id='SCON';
set @export_from='20170501';
set @export_date='20170528';
--'Cash Report
                SELECT s.Consignment_No as consignment_no, 
                          p.BranchID, 
                          p.BranchName, 
                          p.HomeAddress + ' ' + p.Road as payer_address1, 
                          p.District + ' ' + p.Amphur + ' ' + p.Province as payer_address2, 
                          p.PostalCode as payer_zipcode, 
                          p.Telephone as payer_telephone, 
                          s.Sender_Name as sender_name, 
                          s.Sender_Mobile1  
                          + CASE WHEN s.Sender_Mobile2<>'' THEN ';' + s.Sender_Mobile2 ELSE '' END 
                          + CASE WHEN s.Sender_Telephone<>'' THEN ';' + s.Sender_Telephone ELSE '' END 
                          + ';' as  sender_telephone, 
                          s.Recipient_Name as recipient_name, 
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
                          CASE WHEN s.Recipient_ContactPerson<>'' THEN s.Recipient_ContactPerson ELSE s.Recipient_Name END 
                          as recipient_contact_person, 
                          s.Service_Code as service_code, 
                          s.ParcelSize, 
                          s.Weight, 
                          s.Surcharge, 
                          s.VasSurcharge as VasSurcharge, 
                          s.TotalDiscount as Discount, 
                          s.total_vat as Vat, 
                          s.ReceiptNo, 
                          r.ReceiptDate, 
                          isnull(r.TaxInvoiceNo,'-') as TaxInvoiceNo, 
                          s.QTY, 
                          r.ReceivedType as Collect_Type, 
                          s.declare_value as DeclareValue, 
                          s.cod_amount as COD_Amount, 
                          s.cod_account_id as COD_Account_ID, 
                          isnull(cd.cod_charge,0) as COD, 
                          isnull(cd.insur_charge,0) as INSUR, 
                          isnull(cd.package_charge,0) as PKG, 
                          isnull(cd.am_charge,0) as AM, 
                          isnull(cd.pup_charge,0) as PUP, 
                          isnull(cd.flight_charge,0) as TRANS, 
                          isnull(cd.ras_charge,0) as RAS, 
                          isnull(cd.sat_charge,0) as SAT, 
                          CASE WHEN s.cod_amount>0 And s.declare_value > 0 THEN 'C/I' 
                          WHEN s.cod_amount>0 THEN 'C' 
                          WHEN s.declare_value > 0 THEN 'I' 
                          ELSE '' 
                          END as [C/I], 
                          ISNULL(s.seal_no,'') as [SEAL_NO] 
                          FROM dbo.tb_Shipment as s with (nolock) 
                          INNER JOIN tb_Branch as p with (nolock) 
                          ON s.BranchID = p.BranchID 
                          INNER JOIN dbo.tb_ReceiptHead as r with (nolock) 
                          ON s.ReceiptNo = r.ReceiptNo 
                          Left JOIN dbo.tb_TaxInvoice As t With (nolock) 
                          On r.TaxInvoiceNo = t.TaxInvoiceNo 
                          INNER JOIN ( 
                          SELECT a.Consignment_No, 
                          a.am_charge, 
                          a.pup_charge, 
                          a.ras_charge, 
                          a.sat_charge, 
                          a.flight_charge, 
                          b.cod_charge, 
                          b.insur_charge, 
                          b.package_charge 
                          FROM 
                          ( 
                              SELECT s.Consignment_No, 
                              SUM(CASE WHEN rd.ProductID='SVAM' THEN (isnull(rd.Amount,0) + isnull(rd.vat_amount,0)) ELSE 0 END) as am_charge, 
                              SUM(CASE WHEN rd.ProductID='SVPUP' THEN (isnull(rd.Amount,0) + isnull(rd.vat_amount,0)) ELSE 0 END) as pup_charge, 
                              SUM(CASE WHEN rd.ProductID='RAS' THEN (isnull(rd.Amount,0) + isnull(rd.vat_amount,0)) ELSE 0 END) as ras_charge, 
                              SUM(CASE WHEN rd.ProductID='SVSD' THEN (isnull(rd.Amount,0) + isnull(rd.vat_amount,0)) ELSE 0 END) as sat_charge, 
                              SUM(Case When LEFT(ISNULL(rd.ProductID,'TRANS'),5)='TRANS' THEN (isnull(rd.Amount,0) + isnull(rd.vat_amount,0)) ELSE 0 END) as flight_charge 
                              FROM tb_Shipment as s with (nolock) 
                              inner join tb_ReceiptHead as rh with (nolock) 
                              on s.ReceiptNo = rh.ReceiptNo 
                              inner join tb_ReceiptDetail as rd with (nolock) 
                              on s.ReceiptNo = rd.ReceiptNo 
                              and s.Consignment_No = rd.Consignment_No 
                              WHERE convert(varchar,rh.ReceiptDate,112) =@export_date
                              and isnull(rh.canceled,0) = 0 
                              GROUP BY s.Consignment_No 
                          ) as a 
                          LEFT JOIN 
                          ( 
                              SELECT s.Consignment_No, 
                              SUM(CASE WHEN td.ProductID='COD' THEN (isnull(td.Amount,0) + isnull(td.vat_amount,0)) ELSE 0 END) as cod_charge, 
                              SUM(CASE WHEN td.ProductID='INSUR' THEN (isnull(td.Amount,0) + isnull(td.vat_amount,0)) ELSE 0 END) as insur_charge, 
                              SUM(CASE WHEN LEFT(td.ProductID,3)='PKG' THEN (isnull(td.Amount,0) + isnull(td.vat_amount,0)) ELSE 0 END) as package_charge 
                              FROM tb_Shipment as s with (nolock) 
                              inner join tb_ReceiptHead as rh with (nolock) 
                              on s.ReceiptNo = rh.ReceiptNo 
                              inner join tb_TaxInvoice as tax with (nolock) 
                              on rh.TaxInvoiceNo = tax.TaxInvoiceNo 
                              inner join tb_TaxInvoiceDetail as td with (nolock) 
                              on tax.TaxInvoiceNo = td.TaxInvoiceNo 
                              and s.Consignment_No = td.Consignment_No 
                              WHERE convert(varchar,rh.ReceiptDate,112) =@export_date
                              and isnull(rh.canceled,0) = 0 
                              GROUP BY s.Consignment_No 
                          ) as b 
                          ON a.Consignment_No = b.Consignment_No 
                          ) as cd 
                          on s.Consignment_No = cd.Consignment_No 
                          WHERE convert(varchar,r.ReceiptDate,112) = @export_date
                          and isnull(r.canceled,0) = 0
						  and p.BranchID=@branch_id
--------------------------------------------------------------------------------------------
 --'Invoice
                SELECT case when isnull(v.canceled,0)=1 then '' else convert(varchar,v.TaxInvoiceDate,106) end as [TaxInvoice Date] 
                 , isnull(v.TaxInvoiceNo,'') as [TaxInvoice No.] 
                 , Case When isnull(v.canceled,0)=1 Then 'ยกเลิก' else isnull(v.Cust_Name,'') end as [Customer Name] 
                 , Case When isnull(v.canceled,0)=1 Then '-' else isnull(v.Cust_TaxID,'') end as [Customer TaxID] 
                 , Case When isnull(v.Cust_IsHQ,0)=1 Then 'X' else '' end as [Head Office] 
                 , isnull(v.Cust_BranchName,'') as [Branch Name] 
                 , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalAmount,0) End As [Sub Total] 
                 , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalVatAmount,0) End As [Total Vat] 
                 , Case When isnull(v.canceled,0)=1 Then 0 Else  (isnull(v.TotalAmount,0)+isnull(v.TotalVatAmount,0)) End As [Grand Total] 
                 , Case when (select distinct(n.TaxInvoiceNo) from tb_NoneShipment as n with(nolock) where n.TaxInvoiceNo=v.TaxInvoiceNo) Is null 
                 then 'Shipment' else 'None Shipment' end as [Type] 
                 from tb_TaxInvoice As v With (nolock)  
                 WHERE convert(varchar,v.TaxInvoiceDate,112) >= @export_from
                 and convert(varchar,v.TaxInvoiceDate,112) <= @export_date 
				 and v.BranchID =@branch_id
                 ORDER BY v.TaxInvoiceNo
----------------------------------------------------------------------------------------------
 --'LinePay Invoice Daily
                SELECT case when isnull(v.canceled,0)=1 then '' else convert(varchar,v.TaxInvoiceDate,106) end as [TaxInvoice Date]  
                ,Case When isnull(v.canceled,0)=1 Then '' else convert(varchar(5),v.TaxInvoiceDate,108) end as [Time]  
                , isnull(v.TaxInvoiceNo,'') as [TaxInvoice No.]  
                ,isnull(v.payment_gw_trans_no,'') as [Line Transaction No.]  
                ,isnull(h.ReceiptNo,'') as [Receipt No.]  
                , Case When isnull(v.canceled,0)=1 Then 'ยกเลิก' else isnull(v.Cust_Name,'') end as [Customer Name]  
                , Case When isnull(v.canceled,0)=1 Then '-' else isnull(v.Cust_TaxID,'') end as [Customer TaxID]  
                , Case When isnull(v.Cust_IsHQ,0)=1 Then 'X' else '' end as [Customer Head Office]  
                , isnull(v.Cust_BranchName,'') as [Customer Branch Name]  
                , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalAmount,0) End As Amount  
                , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalVatAmount,0) End As Vat  
                , Case When isnull(v.canceled,0)=1 Then 0 Else  (isnull(v.TotalAmount,0)+isnull(v.TotalVatAmount,0)) End As [Grand Total]  
                ,( select distinct stuff(  
                 (  
                  select ','+ Consignment_No FROM tb_ReceiptDetail WHERE ReceiptNo =h.ReceiptNo FOR XML PATH('')  
                  ),1,1,'') as Consignment_No from tb_ReceiptDetail where ReceiptNo=h.ReceiptNo) as [Consignment List]  
                 from tb_TaxInvoice As v With (nolock)   
                 left join tb_ReceiptHead as h on v.TaxInvoiceNo = h.TaxInvoiceNo  
                 WHERE v.ReceivedType = 'LinePay'  
                 And convert(varchar,v.TaxInvoiceDate,112) >= @export_from
                 and convert(varchar,v.TaxInvoiceDate,112) <= @export_date
                 And v.BranchID=@branch_id
                 ORDER BY v.TaxInvoiceNo
--------------------------------------------------------------------------------------------
 --'LinePay Invoice
                SELECT case when isnull(v.canceled,0)=1 then '' else convert(varchar,v.TaxInvoiceDate,106) end as [TaxInvoice Date]  
                ,Case When isnull(v.canceled,0)=1 Then '' else convert(varchar(5),v.TaxInvoiceDate,108) end as [Time]  
                , isnull(v.TaxInvoiceNo,'') as [TaxInvoice No.]  
                ,isnull(v.payment_gw_trans_no,'') as [Line Transaction No.]  
                ,isnull(h.ReceiptNo,'') as [Receipt No.]  
                , Case When isnull(v.canceled,0)=1 Then 'ยกเลิก' else isnull(v.Cust_Name,'') end as [Customer Name]  
                , Case When isnull(v.canceled,0)=1 Then '-' else isnull(v.Cust_TaxID,'') end as [Customer TaxID]  
                , Case When isnull(v.Cust_IsHQ,0)=1 Then 'X' else '' end as [Customer Head Office]  
                , isnull(v.Cust_BranchName,'') as [Customer Branch Name]  
                , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalAmount,0) End As Amount  
                , Case When isnull(v.canceled,0)=1 Then 0 Else isnull(v.TotalVatAmount,0) End As Vat  
                , Case When isnull(v.canceled,0)=1 Then 0 Else  (isnull(v.TotalAmount,0)+isnull(v.TotalVatAmount,0)) End As [Grand Total]  
                ,( select distinct stuff(  
                 (  
                  select ','+ Consignment_No FROM tb_ReceiptDetail WHERE ReceiptNo =h.ReceiptNo FOR XML PATH('')  
                  ),1,1,'') as Consignment_No from tb_ReceiptDetail where ReceiptNo=h.ReceiptNo) as [Consignment List]  
                 from tb_TaxInvoice As v With (nolock)   
                 left join tb_ReceiptHead as h on v.TaxInvoiceNo = h.TaxInvoiceNo  
                 WHERE v.ReceivedType = 'LinePay'  
                 And convert(varchar,v.TaxInvoiceDate,112) >= @export_from
                 and convert(varchar,v.TaxInvoiceDate,112) <= @export_date
				 and v.BranchID =@branch_id
                 ORDER BY v.TaxInvoiceNo
--------------------------------------------------------------------------------------------
 --'Line TopUp
               SELECT n.RecordID as run_no 
                  ,n.VASID As vas_id 
                  ,t.orderId 
                  ,case when len(t.oneTimeKey)=12 then (substring(t.oneTimeKey,1,4)+' '+substring(t.oneTimeKey,5,4)+' '+substring(t.oneTimeKey,9,4)) 
                  Else t.oneTimeKey End As customer_qr_code 
                  ,n.Remark As [description] 
                  ,n.unit_price As amount 
                  ,n.created_by As [by] 
                  ,n.HostName As client_name 
                  ,convert(varchar,n.created_datetime,120) As transaction_date 
                  ,n.[batch_no] 
                 From tb_NoneShipment As n 
                 where n.VASID ='LNTUP' 
                 And Convert(varchar, n.created_datetime, 112) ='  ExportDate  '

--------------------------------------------------------------------------------------------
  'Box Revenue
                strSQL = select tx.[TaxInvoiceNo], 
                 convert(varchar,tx.[TaxInvoiceDate],120) as [TaxInvoiceDate], 
                 tx.[ReceivedType], 
                 td.[ProductID], 
                 td.[Description], 
                 td.QTY as QTY, 
                 td.[Amount] + [vat_amount] as Total, 
                 isnull(td.[Consignment_No],'') as [Consignment_No], 
                 case when tx.canceled = 1 then 'Cancel' else '' end as Cancel, 
                 isnull(tx.[canceled_remark],'') as [Canceled_Remark], 
                 isnull(tx.[canceled_by],'') as [Canceled_By], 
                 case when tx.[canceled_date] is null then '' else convert(varchar,[canceled_date],120) end as [canceled_date], 
                 Case When isnull(td.[Consignment_No],'')='' and isnull(tx.canceled,0) = 0 then 'None Shipment' else 'Shipment' end as [Type] 
                 from tb_TaxInvoiceDetail as td with (nolock) 
                 inner join tb_TaxInvoice as tx with (nolock) 
                 on td.[TaxInvoiceNo] = tx.[TaxInvoiceNo] 
                 where LEFT(td.ProductID,3) = 'PKG' 
                 and convert(varchar,tx.TaxInvoiceDate,112) >= '  strDateForm  '  
                 and convert(varchar,tx.TaxInvoiceDate,112) <= '  strDateTo  ' 
                If Conn.State = ConnectionState.Closed Then Conn.Open()
                da = New SqlDataAdapter(strSQL, Conn)
                da.Fill(ds, BoxRevenue)