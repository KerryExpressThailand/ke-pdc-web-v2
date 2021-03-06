/****** Script for SelectTopNRows command from SSMS  ******/
USE POS

DECLARE @branchIDList varchar(1000) = 'HPRP';
DECLARE	@TbBranch TABLE (b_id varchar(10) primary key);
DECLARE @BranchID varchar(20),@Pos int;

DECLARE @menuIDList varchar(MAX) = '40d01185-95ec-4029-9e14-3c15a00109c3,bde9210b-89c3-426f-89ab-b70625a3948c'
DECLARE @TbMenu TABLE (menu_id uniqueidentifier primary key);
DECLARE @MenuID uniqueidentifier;

SET @branchIDList = LTRIM(RTRIM(@branchIDList))+ ',';
SET @Pos = CHARINDEX(',', @branchIDList, 1);
			
IF REPLACE(@branchIDList, ',', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @BranchID = LTRIM(RTRIM(LEFT(@branchIDList, @Pos - 1)));
		SET @branchIDList = RIGHT(@branchIDList, LEN(@branchIDList) - @Pos);
		SET @Pos = CHARINDEX(',', @branchIDList, 1);
		IF ISNULL(@BranchID, '') <> '' BEGIN
			INSERT INTO @TbBranch (b_id) VALUES (@BranchID);
		END
	END
END;


SET @menuIDList = LTRIM(RTRIM(@menuIDList))+ ',';
SET @Pos = CHARINDEX(',', @menuIDList, 1);
IF REPLACE(@menuIDList, ',', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @MenuID = LTRIM(RTRIM(LEFT(@menuIDList, @Pos - 1)));
		SET @menuIDList = RIGHT(@menuIDList, LEN(@menuIDList) - @Pos);
		SET @Pos = CHARINDEX(',', @menuIDList, 1);
		INSERT INTO @TbMenu (menu_id) VALUES (@MenuID);
	END
END;




--INSERT TO MASTER_USER
	INSERT INTO tb_master_user
				([userid],[pwd],[created_date],[created_by],[updated_date],[updated_by],[default_shop_id],[username],[role_id],[user_group_id],[profile_id])
	SELECT	 'shop' + LOWER(b.BranchID)
			,LEFT(NEWID(),1) +
				CHAR(ASCII('a')+(ABS(CHECKSUM(NEWID()))%25)) +
				CHAR(ASCII('A')+(ABS(CHECKSUM(NEWID()))%25)) +
				CHAR(ASCII('a')+(ABS(CHECKSUM(NEWID()))%25))
			,GETDATE()
			,'chettapong'
			,NULL
			,NULL
			,b.BranchID
			,b.branch_type + ' - ' + b.BranchID
			,1
			,3
			,NULL
	FROM [tb_Branch] AS b
	FULL OUTER JOIN tb_master_user AS u ON u.userid = 'shop' + LOWER(b.BranchID)
	WHERE BranchID IN (SELECT b_id FROM @TbBranch)
		AND u.userid IS NULL

	-- SELECT
	SELECT * FROM tb_master_user AS u
	WHERE u.userid IN (SELECT 'shop' + LOWER(b_id) FROM @TbBranch)
--INSERT TO MASTER_USER



--INSERT TO PDC_MAPPING_MENU
	INSERT INTO tb_pdc_mapping_user_menu
		(userid,menu_id,created_date,created_by)
	SELECT	 u.userid
			,m.menu_id
			,GETDATE()
			,'chettapong'
	FROM tb_master_user AS u
	FULL OUTER JOIN (
		SELECT menu_id FROM @TbMenu
	) AS m ON m.menu_id = m.menu_id
	LEFT JOIN (
		SELECT userid, menu_id FROM tb_pdc_mapping_user_menu
	) AS um ON u.userid = um.userid AND m.menu_id = um.menu_id
	WHERE u.userid IN (SELECT 'shop' + LOWER(b_id) FROM @TbBranch)
	AND um.menu_id IS NULL

	SELECT * FROM tb_pdc_mapping_user_menu
	WHERE userid IN (SELECT 'shop' + LOWER(b_id) FROM @TbBranch)
--INSERT TO PDC_MAPPING_MENU