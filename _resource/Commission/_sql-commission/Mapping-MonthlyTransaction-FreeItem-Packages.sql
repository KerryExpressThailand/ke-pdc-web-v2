/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [branch_id]
      ,[month]
      ,[year]
      ,t.item_id
      ,i.item_desc
	  ,p.PackageDesc
      ,t.category_id
      ,[item_amount]
      ,[item_expense]
      ,t.remark
      ,[attachment]
FROM [POS].[dbo].[tb_fee_monthly_transaction] AS t
INNER JOIN tb_fee_items AS i ON t.item_id = i.item_id
INNER JOIN tb_Package AS p ON p.PackageID = i.item_desc
WHERE [year] = 2017 AND [month] = 7
--AND branch_id = 'MTNG'
AND branch_id = 'BBUA'
--AND branch_id = 'SAMK'
--AND branch_id = 'MPTN'
--AND branch_id = 'CWNA'
--AND branch_id = 'SKMT'