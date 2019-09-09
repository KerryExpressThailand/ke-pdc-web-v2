DECLARE @UserID VARCHAR(20) = 'bphi';
DECLARE @BranchIDList VARCHAR(MAX) = 'TNON';
DECLARE @Date VARCHAR(8) = '20170517';


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


/* Cash Report */
SELECT	 s.Consignment_No AS ConsignmentNo
		,s.BranchID
		,b.BranchName
		,'xxx' AS PayerAddress1
		,'xxx' AS PayerAddress2
		,'xxx' AS PayerPostCode
		,'xxx' AS PayerTelephone
		,s.Sender_Name AS SenderName
		,s.Sender_Mobile1 AS SenderTelephone
		,s.Recipient_Name AS RecipientName
		,'xxx' AS RecipientAddress1
		,'xxx' AS RecipientAddress2
		,s.Recipient_PostalCode AS RecipientPostalCode
		,s.Recipient_Telephone AS RecipientTelephone
		,CASE WHEN ISNULL(s.Recipient_ContactPerson, '') = '' THEN s.Recipient_Name ELSE s.Recipient_ContactPerson END AS RecipientContactPerson
		,s.Service_Code AS ServiceCode
		,s.ParcelSize
		,s.[Weight]
		,s.VasSurcharge
		,s.TotalDiscount
		,s.total_vat AS TotalVat
		,s.ReceiptNo
		,s.StatusDate AS ReceiptDate
		,'xxx' AS TaxInvoiceNo
		,999 AS Quantity
		,'xxx' AS CollectType
		,s.declare_value AS DeclareValue

		,s.cod_amount AS CODAmount
		,s.cod_account_id AS CODAccountID

		,CAST(s.cod_amount * 0.03 AS money) AS SurchargeCOD
		,9.99 AS SurchargeInsurance
		,9.99 AS SurchargePackage
		,9.99 AS SurchargeAM
		,9.99 AS SurchargePUP
		,s.Surcharge AS SurchargeTransfer
		,9.99 AS SurchargeRAS
		,9.99 AS SurchargeSAT
		,'' AS CI
		,'xxx' AS SealNo
FROM tb_Shipment AS s
INNER JOIN tb_Branch AS b ON b.BranchID = s.BranchID
WHERE CONVERT(varchar, Created_Date, 112) = @Date
	AND s.BranchID IN (SELECT b_id FROM @TbBranch)
	AND s.Consignment_No IN ('TNON000247127', 'TNON000247105')