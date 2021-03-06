/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [TransactionNumber]
    ,[OriginalTransactionNumber]
    ,[TransactionDate]
    ,[MerchantID]
    ,[DeviceID]
    ,[Amount]
    ,[TransactionStatus]
    ,[OrderNumber]
    ,[PaymentMethod]
    ,[IsVoid]
    ,[CreatedDate]
FROM [POS].[dbo].[tb_RLP_Transaction]
WHERE CONVERT(VARCHAR, TransactionDate, 112) = '20170424' AND TransactionStatus IN('TOPUP','TOPUP_CANCEL') AND MerchantID = 'kerryexpress_tod'