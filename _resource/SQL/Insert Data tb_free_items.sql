/****** Script for SelectTopNRows command from SSMS  ******/

/** CATEGORY 2 IS SERVICE FEE IT **/
DECLARE @TB_FEE_MONTHLY_TRANSACTION TABLE ([branch_id] [varchar](10) NOT NULL,
	[month] [int] NOT NULL,
	[year] [int] NOT NULL,
	[item_id] [int] NOT NULL,
	[item_desc] [nvarchar](150) NOT NULL,
	[category_id] [int] NOT NULL,
	[item_amount] [int] NULL,
	[item_expense] [money] NULL,
	[remark] [nvarchar](150) NULL,
	[attachment] [nvarchar](150) NULL
	PRIMARY KEY ([branch_id], [month], [year], [item_id]))
--INSERT INTO tb_fee_items (item_id,item_desc,category_id) VALUES (@NewItemID,'PKG04', 5)

INSERT INTO @TB_FEE_MONTHLY_TRANSACTION
OUTPUT inserted.[item_id], inserted.item_desc, inserted.[category_id] INTO tb_fee_items
SELECT m.branch_id
      ,m.[month]
      ,m.[year]
      ,(SELECT MAX(item_id) AS ID FROM tb_fee_items  WITH(NOLOCK)) + ROW_NUMBER() OVER (ORDER BY m.branch_id) item_id
	  ,bk.item_desc
      ,m.category_id
      ,m.item_amount
      ,m.item_expense
      ,m.remark
      ,m.attachment
FROM [POS].[dbo].[tb_fee_monthly_transaction-bk] AS m
INNER JOIN [tb_fee_items-bk] AS bk ON bk.[item_id] = m.[item_id]
WHERE m.[category_id] = 2
	--AND m.[item_expense] > 0

--SELECT * FROM @TB_FEE_MONTHLY_TRANSACTION AS m

INSERT INTO tb_fee_monthly_transaction
	(m.branch_id
	,m.[month]
	,m.[year]
	,m.item_id
	,m.category_id
	,m.item_amount
	,m.item_expense
	,m.remark
	,m.attachment)
SELECT m.branch_id
      ,m.[month]
      ,m.[year]
      ,m.item_id
      ,m.category_id
      ,m.item_amount
      ,m.item_expense
      ,m.remark
      ,m.attachment
FROM @TB_FEE_MONTHLY_TRANSACTION AS m




INSERT INTO tb_fee_monthly_transaction
SELECT	m.branch_id
		,m.month
		,m.year
		,CASE WHEN m.category_id = 1 THEN
			1
		WHEN m.category_id = 2 THEN
			(SELECT MAX(item_id) AS ID FROM tb_fee_items  WITH(NOLOCK)) + ROW_NUMBER() OVER (ORDER BY m.branch_id)
		ELSE
			ISNULL(n.item_id, 0)
		END AS item_id
		--,bk.item_desc
		,m.category_id
		,m.item_amount
		,m.item_expense
		,m.remark
		,m.attachment
		--,COUNT(m.[branch_id]) AS NumOccurrences
FROM [tb_fee_monthly_transaction-bk] AS m
INNER JOIN [tb_fee_items-bk] AS bk ON bk.item_id = m.item_id
LEFT JOIN [tb_fee_items] AS n ON bk.item_desc = n.item_desc
WHERE m.category_id IN (1, /*3,*/ 4, 5, 6)
	AND m.[item_expense] > 0
--GROUP BY m.[branch_id],m.[month],m.[year],m.category_id,n.[item_id]
--HAVING ( COUNT(m.[branch_id]) = 1 )
