USE [POS]
GO

/** CATEGORY 1 IS MANAGEMENT FEE **/
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES (1, 'MANAGEMENT_FEE', 1)


/** CATEGORY 3 IS SERVICE FEE SUPPLY **/
INSERT INTO [dbo].[tb_fee_items] ([item_id], [item_desc], [category_id])
SELECT	(SELECT MAX(item_id) AS ID FROM tb_fee_items  WITH(NOLOCK)) + ROW_NUMBER() OVER (ORDER BY p.PackageID) ID
		,p.PackageID
		,3 AS Category
FROM tb_Package AS p
WHERE p.PackageType IN (4, 5)


/** CATEGORY 4 IS FACILITY **/
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'RENT', 4)
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'ELECTRICITY_BILL', 4)
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'WATER_BILL', 4)
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'TELEPHONE_BILL', 4)
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'MOBILE_BILL', 4)
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'INTERNET_BILL', 4)


/** CATEGORY 5 IS SALES PACKAGE **/
--INSERT INTO [dbo].[tb_fee_items] ([item_id], [item_desc], [category_id])
--SELECT	(SELECT MAX(item_id) AS ID FROM tb_fee_items  WITH(NOLOCK)) + ROW_NUMBER() OVER (ORDER BY p.PackageID) ID
--		,p.PackageID
--		,5 AS Category
--FROM tb_Package AS p
--WHERE p.PackageType = 1


/** CATEGORY 6 IS ADJUSTMENT **/
--INSERT INTO tb_fee_items ([item_id], [item_desc], [category_id]) VALUES ((SELECT MAX(item_id)+1 AS ID FROM tb_fee_items  WITH(NOLOCK)), 'ADJUSTMENT', 6)

GO


