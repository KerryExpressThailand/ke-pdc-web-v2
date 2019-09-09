DECLARE @items TABLE (item_id int primary key, item_desc nvarchar(150), category_id int)
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

INSERT INTO @TB_FEE_MONTHLY_TRANSACTION
OUTPUT inserted.[item_id], inserted.item_desc, inserted.[category_id] INTO @items
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


SELECT * FROM @items
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