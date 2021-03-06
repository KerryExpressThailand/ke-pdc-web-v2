/****** Script for SelectTopNRows command from SSMS  ******/
SELECT	 branch_type AS [Branch Type]
		,BranchID AS [Branch ID]
		,BranchName AS [Branch Name]
		,userid AS [Username]
		,pwd AS [Password]
FROM [POS].[dbo].[tb_master_user] AS u
INNER JOIN tb_Branch AS b ON u.default_shop_id = b.BranchID
WHERE b.branch_type = 'DCSP-SHOP' AND userid LIKE 'shop%'