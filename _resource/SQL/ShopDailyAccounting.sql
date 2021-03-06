USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[sp_RPT316_ShopDaily]    Script Date: 2/6/2560 15:21:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chettapong Pinsuwan
-- Create date: 20170524
-- Description:	Get data for Shop Daily Dashboard
-- =============================================
ALTER PROCEDURE [dbo].[sp_RPT316_ShopDaily]
	@UserID VARCHAR(20), -- bphi
	@BranchIDList VARCHAR(MAX), -- ASK,BANA,SLOM,
	@Date VARCHAR(8) --20170504
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRANSACTION T1
	--//***** Begin Syntax *****//--
	
		--============================================================
		DECLARE	@TbBranch TABLE (b_id varchar(10) primary key);
		DECLARE @BranchID varchar(20),@Pos int;
	
		SET @BranchIDList = LTRIM(RTRIM(@BranchIDList))+ ',';
		SET @Pos = CHARINDEX(',', @BranchIDList, 1);
	
		IF REPLACE(@BranchIDList, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @BranchID = LTRIM(RTRIM(LEFT(@BranchIDList, @Pos - 1)));
				SET @BranchIDList = RIGHT(@BranchIDList, LEN(@BranchIDList) - @Pos);
				SET @Pos = CHARINDEX(',', @BranchIDList, 1);
				INSERT INTO @TbBranch (b_id) VALUES (@BranchID);
			END
		END;
		--============================================================

		--============================================================
		-- Cash Report
		SELECT	 s.Consignment_No AS ConsignmentNo
				,p.BranchID
				,p.BranchName
				,p.HomeAddress + ' ' + p.Road AS PayerAddress1
				,p.District + ' ' + p.Amphur + ' ' + p.Province AS PayerAddress2
				,p.PostalCode AS PayerZipcode
				,p.Telephone AS PayerTelephone
				,s.Sender_Name AS SenderName
				,s.Sender_Mobile1
					+ CASE WHEN ISNULL(s.Sender_Mobile2, '') <> '' THEN ';' + s.Sender_Mobile2 ELSE '' END
					+ CASE WHEN ISNULL(s.Sender_Telephone, '') <>'' THEN ';' + s.Sender_Telephone ELSE '' END
					+ ';' AS SenderTelephone
				,s.Recipient_Name AS RecipientName
				,s.Recipient_Address
					+ CASE WHEN ISNULL(s.Recipient_Village, '') <> '' THEN N' หมู่บ้าน/อาคาร ' + s.Recipient_Village ELSE '' END
					+ CASE WHEN ISNULL(s.Recipient_Moo, '') <> '' THEN N' หมู่ที่ ' + s.Recipient_Moo ELSE '' END
					+ CASE WHEN ISNULL(s.Recipient_Soi, '') <> '' THEN N' ซอย ' + s.Recipient_Soi ELSE '' END
					+ CASE WHEN ISNULL(s.Recipient_Road, '') <> '' THEN N' ถนน ' + s.Recipient_Road ELSE '' END
				AS RecipientAddress1
				,s.Recipient_District 
					+ ' ' + s.Recipient_Amphur 
					+ ' ' + s.Recipient_Province
				AS RecipientAddress2
				,s.Recipient_PostalCode AS RecipientZipcode
				,s.Recipient_Mobile1 
					+ CASE WHEN ISNULL(s.Recipient_Mobile2, '') <> '' THEN ';' + s.Recipient_Mobile2 ELSE '' END 
					+ CASE WHEN ISNULL(s.Recipient_Telephone, '') <> '' THEN ';' + s.Recipient_Telephone ELSE '' END 
					+ ';'
				AS RecipientTelephone
				,CASE WHEN ISNULL(s.Recipient_ContactPerson, '') <> ''
					THEN s.Recipient_ContactPerson
					ELSE s.Recipient_Name END 
				AS RecipientContactPerson
				,s.Service_Code AS ServiceCode
				,s.ParcelSize
				,s.[Weight]
				,s.Surcharge
				,s.VasSurcharge AS VASSurcharge
				,s.TotalDiscount AS Discount
				,s.total_vat AS Vat
				,s.ReceiptNo
				,r.ReceiptDate
				,ISNULL(r.TaxInvoiceNo,'-') AS TaxInvoiceNo
				,s.QTY AS Quantity
				,r.ReceivedType AS CollectType
				,s.declare_value AS DeclareValue
				,s.cod_amount AS CODAmount
				,s.cod_account_id AS CODAccountID
				,ISNULL(cd.cod_charge,0) AS SurchargeCOD
				,ISNULL(cd.insur_charge,0) AS SurchargeINSUR
				,ISNULL(cd.package_charge,0) AS SurchargePKG
				,ISNULL(cd.am_charge,0) AS SurchargeAM
				,ISNULL(cd.pup_charge,0) AS SurchargePUP
				,ISNULL(cd.flight_charge,0) AS SurchargeTRANS
				,ISNULL(cd.ras_charge,0) AS SurchargeRAS
				,ISNULL(cd.sat_charge,0) AS SurchargeSAT
				,CASE
					WHEN s.cod_amount > 0 AND s.declare_value > 0 THEN 'C/I'
					WHEN s.cod_amount > 0 THEN 'C'
					WHEN s.declare_value > 0 THEN 'I'
					ELSE NULL
				END AS CI
				,s.seal_no AS SEALNO
		FROM dbo.tb_Shipment AS s WITH (NOLOCK)
		INNER JOIN tb_Branch AS p WITH (NOLOCK) ON s.BranchID = p.BranchID
		INNER JOIN dbo.tb_ReceiptHead AS r WITH (NOLOCK)ON s.ReceiptNo = r.ReceiptNo
		Left JOIN dbo.tb_TaxInvoice AS t WITH (NOLOCK)ON r.TaxInvoiceNo = t.TaxInvoiceNo
		INNER JOIN (
			SELECT	 a.Consignment_No
					,a.am_charge
					,a.pup_charge
					,a.ras_charge
					,a.sat_charge
					,a.flight_charge
					,b.cod_charge
					,b.insur_charge
					,b.package_charge
			FROM (
				SELECT	 s.Consignment_No
						,SUM(CASE WHEN rd.ProductID='SVAM' THEN (ISNULL(rd.Amount,0) + ISNULL(rd.vat_amount,0)) ELSE 0 END) AS am_charge
						,SUM(CASE WHEN rd.ProductID='SVPUP' THEN (ISNULL(rd.Amount,0) + ISNULL(rd.vat_amount,0)) ELSE 0 END) AS pup_charge
						,SUM(CASE WHEN rd.ProductID='RAS' THEN (ISNULL(rd.Amount,0) + ISNULL(rd.vat_amount,0)) ELSE 0 END) AS ras_charge
						,SUM(CASE WHEN rd.ProductID='SVSD' THEN (ISNULL(rd.Amount,0) + ISNULL(rd.vat_amount,0)) ELSE 0 END) AS sat_charge
						,SUM(CASE WHEN LEFT(ISNULL(rd.ProductID,'TRANS'),5)='TRANS' THEN (ISNULL(rd.Amount,0) + ISNULL(rd.vat_amount,0)) ELSE 0 END) AS flight_charge
				FROM tb_Shipment AS s WITH (NOLOCK)
				INNER JOIN tb_ReceiptHead AS rh WITH (NOLOCK)ON s.ReceiptNo = rh.ReceiptNo 
				INNER JOIN tb_ReceiptDetail AS rd WITH (NOLOCK)ON s.ReceiptNo = rd.ReceiptNo AND s.Consignment_No = rd.Consignment_No 
				WHERE CONVERT(varchar,rh.ReceiptDate,112) = @Date
					AND ISNULL(rh.canceled,0) = 0 
				GROUP BY s.Consignment_No 
			) AS a
			LEFT JOIN (
				SELECT	 s.Consignment_No
						,SUM(CASE WHEN td.ProductID='COD' THEN (ISNULL(td.Amount,0) + ISNULL(td.vat_amount,0)) ELSE 0 END) AS cod_charge
						,SUM(CASE WHEN td.ProductID='INSUR' THEN (ISNULL(td.Amount,0) + ISNULL(td.vat_amount,0)) ELSE 0 END) AS insur_charge
						,SUM(CASE WHEN LEFT(td.ProductID,3)='PKG' THEN (ISNULL(td.Amount,0) + ISNULL(td.vat_amount,0)) ELSE 0 END) AS package_charge 
				FROM tb_Shipment AS s WITH (NOLOCK)
				INNER JOIN tb_ReceiptHead AS rh WITH (NOLOCK)ON s.ReceiptNo = rh.ReceiptNo 
				INNER JOIN tb_TaxInvoice AS tax WITH (NOLOCK)ON rh.TaxInvoiceNo = tax.TaxInvoiceNo 
				INNER JOIN tb_TaxInvoiceDetail AS td WITH (NOLOCK)ON tax.TaxInvoiceNo = td.TaxInvoiceNo AND s.Consignment_No = td.Consignment_No 
				WHERE CONVERT(varchar,rh.ReceiptDate,112) = @Date
					AND ISNULL(rh.canceled,0) = 0 
				GROUP BY s.Consignment_No 
			) AS b ON a.Consignment_No = b.Consignment_No 
		) AS cd ON s.Consignment_No = cd.Consignment_No
		WHERE CONVERT(varchar,r.ReceiptDate, 112) = @Date
			AND ISNULL(r.canceled, 0) = 0
			AND p.BranchID IN (SELECT b_id FROM @TbBranch)




		-- Invoice
		SELECT	CASE WHEN ISNULL(v.canceled,0) = 1
					THEN ''
					ELSE CONVERT(varchar,v.TaxInvoiceDate,106)
				END AS TaxInvoiceDate
				,v.TaxInvoiceNo
				,CASE WHEN ISNULL(v.canceled,0) = 1 THEN 'ยกเลิก' ELSE ISNULL(v.Cust_Name,'') END AS CustomerName
				,CASE WHEN ISNULL(v.canceled,0) = 1 THEN '-' ELSE ISNULL(v.Cust_TaxID,'') END AS CustomerTaxID
				,v.Cust_IsHQ AS HeadOffice
				,ISNULL(v.Cust_BranchName, '') AS BranchName
				,CASE WHEN ISNULL(v.canceled,0) = 1 THEN 0 ELSE ISNULL(v.TotalAmount,0) END AS SubTotal
				,CASE WHEN ISNULL(v.canceled,0) = 1 THEN 0 ELSE ISNULL(v.TotalVatAmount,0) END AS TotalVat
				,CASE WHEN ISNULL(v.canceled,0) = 1 THEN 0 ELSE  (ISNULL(v.TotalAmount,0)+ISNULL(v.TotalVatAmount,0)) END AS  [Grand Total] 
				,CASE WHEN (SELECT DISTINCT(n.TaxInvoiceNo) FROM tb_NoneShipment AS n WITH(NOLOCK) WHERE n.TaxInvoiceNo = v.TaxInvoiceNo) IS NULL
					THEN 'Shipment' ELSE 'None Shipment'
				END AS [Type]
		FROM tb_TaxInvoice AS v WITH (NOLOCK)
		WHERE CONVERT(varchar,v.TaxInvoiceDate,112) >= LEFT(@Date, 6) + '01'
			AND CONVERT(varchar,v.TaxInvoiceDate,112) <= @Date
			AND v.BranchID IN (SELECT b_id FROM @TbBranch)
		ORDER BY v.TaxInvoiceNo




		-- LinePay Invoice Daily
		SELECT	CASE WHEN ISNULL(v.canceled,0) = 1
					THEN NULL
					ELSE CONVERT(varchar, v.TaxInvoiceDate, 106)
				END AS TaxInvoiceDate
                ,CASE WHEN ISNULL(v.canceled,0)=1
					Then NULL
					ELSE CONVERT(varchar(5),v.TaxInvoiceDate,108)
				END AS TaxInvoiceTime
                ,ISNULL(v.TaxInvoiceNo,'') AS TaxInvoiceNo
                ,''/*ISNULL(v.payment_gw_trans_no,'')*/ AS LineTransactionNo
                ,ISNULL(h.ReceiptNo,'') AS [Receipt No.]  
                
				,CASE WHEN ISNULL(v.canceled, 0) = 1
					THEN 'ยกเลิก' ELSE ISNULL(v.Cust_Name,'')
				END AS [Customer Name]  
                
				,CASE WHEN ISNULL(v.canceled, 0) = 1
					THEN '-' ELSE v.Cust_TaxID
				END AS [CustomerTaxID]
                
				,CASE WHEN ISNULL(v.Cust_IsHQ, 0) = 1
					THEN 'X' ELSE NULL
				END AS CustomerHeadOffice
                
				,v.Cust_BranchName AS CustomerBranchName
                
				,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN 0 ELSE ISNULL(v.TotalAmount,0)
				END AS Amount
                
				,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN 0 ELSE ISNULL(v.TotalVatAmount,0)
				END AS Vat
                
				,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN 0 ELSE  (ISNULL(v.TotalAmount,0) + ISNULL(v.TotalVatAmount, 0))
				END AS GrandTotal
                
				,(SELECT DISTINCT STUFF(  
					(SELECT ','+ Consignment_No
					FROM tb_ReceiptDetail
					WHERE ReceiptNo = h.ReceiptNo
					FOR XML PATH('')),1,1,'')
					AS Consignment_No
					FROM tb_ReceiptDetail
					WHERE ReceiptNo=h.ReceiptNo
				) AS ConsignmentList
		FROM tb_TaxInvoice AS v WITH (NOLOCK)
		LEFT JOIN tb_ReceiptHead AS h ON v.TaxInvoiceNo = h.TaxInvoiceNo
		WHERE v.ReceivedType = 'LinePay'  
			AND CONVERT(varchar,v.TaxInvoiceDate,112) >= LEFT(@Date, 6) + '01'
			AND CONVERT(varchar,v.TaxInvoiceDate,112) <= @Date
			AND v.BranchID IN (SELECT b_id FROM @TbBranch)
		ORDER BY v.TaxInvoiceNo




		-- LinePay Invoice
		SELECT	CASE WHEN ISNULL(v.canceled, 0) = 1
					THEN NULL
					ELSE CONVERT(varchar,v.TaxInvoiceDate, 106)
				END AS TaxInvoiceDate
                ,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN NULL
					ELSE CONVERT(varchar(5), v.TaxInvoiceDate, 108)
				END AS TaxInvoiceTime
                ,v.TaxInvoiceNo AS TaxInvoiceNo
                ,''/*ISNULL(v.payment_gw_trans_no, '')*/ AS LineTransactionNo
                ,h.ReceiptNo  
                
				,CASE WHEN ISNULL(v.canceled,0)=1
					THEN 'ยกเลิก'
					ELSE ISNULL(v.Cust_Name,'')
				END AS [Customer Name]  
                
				,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN '-'
					ELSE ISNULL(v.Cust_TaxID, '')
				END AS [Customer TaxID]  
                
				,CASE WHEN ISNULL(v.Cust_IsHQ,0)=1
					THEN 'X'
					ELSE ''
				END AS CustomerHeadOffice
                
				,v.Cust_BranchName AS CustomerBranchName
                ,CASE WHEN ISNULL(v.canceled,0) = 1
				THEN 0 ELSE ISNULL(v.TotalAmount,0)
				END AS Amount

                ,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN 0
					ELSE ISNULL(v.TotalVatAmount,0)
				END AS Vat

                ,CASE WHEN ISNULL(v.canceled,0) = 1
					THEN 0
					ELSE  (ISNULL(v.TotalAmount,0) + ISNULL(v.TotalVatAmount, 0)) END
				AS GrandTotal
				,(SELECT DISTINCT STUFF(
					(SELECT ','+ Consignment_No FROM tb_ReceiptDetail WHERE ReceiptNo = h.ReceiptNo FOR XML PATH('')),1,1,'') AS Consignment_No
					FROM tb_ReceiptDetail
					WHERE ReceiptNo=h.ReceiptNo
				) AS ConsignmentList
				FROM tb_TaxInvoice AS v WITH (NOLOCK)
					LEFT JOIN tb_ReceiptHead AS h ON v.TaxInvoiceNo = h.TaxInvoiceNo
				WHERE v.ReceivedType = 'LinePay'
					AND CONVERT(varchar,v.TaxInvoiceDate,112) >= LEFT(@Date, 6) + '01'
					AND CONVERT(varchar,v.TaxInvoiceDate,112) <= @Date
					AND v.BranchID IN (SELECT b_id FROM @TbBranch)
				ORDER BY v.TaxInvoiceNo
				
				
				
				
				-- Line TopUp
				SELECT	n.RecordID AS RunNo
						,n.VASID AS VASID
						,'xxxxxxxx'/*t.orderId*/ AS OrderID
						,CASE WHEN LEN(t.oneTimeKey) = 12
							THEN (SUBSTRING(t.oneTimeKey,1,4) + ' ' + SUBSTRING(t.oneTimeKey,5,4) + ' ' + SUBSTRING(t.oneTimeKey,9,4))
							ELSE t.oneTimeKey
						END AS CustomerQRCode
						,n.Remark AS [Description]
						,n.unit_price AS Amount
						,n.created_by AS [By]
						,n.HostName AS ClientName
						,CONVERT(varchar,n.created_datetime,120) AS TransactionDate
						,n.batch_no AS BatchNo
				FROM tb_NoneShipment AS n 
				WHERE n.VASID ='LNTUP' 
					AND CONVERT(varchar, n.created_datetime, 112) = '  ExportDate  '
				
				-- Box Revenue
				SELECT	 tx.TaxInvoiceNo
						,CONVERT(varchar, tx.TaxInvoiceDate, 120) AS TaxInvoiceDate
						,tx.ReceivedType
						,td.ProductID
						,td.[Description]
						,td.QTY
						,td.Amount + vat_amount AS Total
						,td.Consignment_No AS ConsignmentNo
						,tx.canceled AS Cancel
						,tx.canceled_remark AS CanceledRemark
						,tx.canceled_by AS CanceledBy
						,CASE WHEN tx.canceled_date IS NULL
							THEN NULL
							ELSE CONVERT(varchar, canceled_date, 120)
						END AS CanceledDate
						,CASE WHEN ISNULL(td.Consignment_No, '') = '' AND ISNULL(tx.canceled, 0) = 0
							THEN 'None Shipment'
							ELSE 'Shipment'
						END AS [Type]
				FROM tb_TaxInvoiceDetail AS tdWITH (NOLOCK)
				INNER JOIN tb_TaxInvoice AS txWITH (NOLOCK) ON td.[TaxInvoiceNo] = tx.[TaxInvoiceNo] 
				WHERE LEFT(td.ProductID,3) = 'PKG' 
					AND CONVERT(varchar,tx.TaxInvoiceDate,112) >= LEFT(@Date, 6) + '01'
					AND CONVERT(varchar,tx.TaxInvoiceDate,112) <= @Date
		--============================================================
	--============================================================

	--//***** Finally Process *****//--
	IF (@@ERROR <> 0)
	BEGIN
		rollback transaction T1
	END ELSE
	BEGIN
		commit transaction T1
	END;
END