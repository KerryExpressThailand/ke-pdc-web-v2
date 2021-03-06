DECLARE @From INT = 0;
DECLARE @To INT = 20;
DECLARE @VerifyID varchar(50);

DECLARE @BranchIDList varchar(MAX) = 'BYAI,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI,SUKS,BBON,DONM,TSIT,ONUT,NAWA,LKAB,PINK,TPLU,NMIN,RMA2,BKEN,NAIN,BANA,SMUT,TEPA,BSTO,TKRU,NKAM,BPEE,TNPT,TUPM,TTAI,KBAN,EKKA,PANT,SAMK,NJOK,SLYA,MPTN,TYA3,TAIT,MINB,CHC4,PTNK,BROM,NLCH,KVIL,LPDU,SNBN,CWNA,PS43,TAC4,PBSK,SNDA,BSAE,SNOI,LAMB,BANB,LKAE,SUAS,ROMK,TYA6,BAPU,MCCS,AMTA,NKCS,KSWA,PTYA,BWIN,PYSC,MNKP,POKW,LUK2,SAP2,PYSL,PWET,BTEC,PNAM,ONTC,TTLY,FPAK,SEC2,SATU,SAI4,LAK4,SKMT,BAKY'
DECLARE @Month int = 2
DECLARE @Year int = 2017

DECLARE @SortingByBranchID BIT = NULL;
DECLARE @SortingByManagementFee BIT = NULL;
DECLARE @SortingByServiceFeeIT BIT = NULL;
DECLARE @SortingByServiceFeeSupply BIT = NULL;
DECLARE @SortingByFacility BIT = NULL;
DECLARE @SortingBySalesPackage BIT = NULL;
DECLARE @SortingByAdjustment BIT = NULL;
DECLARE @SortingByFeeManagementVerifyBy BIT = NULL;
DECLARE @SortingByFeeManagementVerifyDate BIT = NULL;
DECLARE @SortingByFeeItVerifyBy BIT = NULL;
DECLARE @SortingByFeeItVerifyDate BIT = NULL;
DECLARE @SortingByFeeSupplyVerifyBy BIT = NULL;
DECLARE @SortingByFeeSupplyVerifyDate BIT = NULL;
DECLARE @SortingByFeeFacilityVerifyBy BIT = NULL;
DECLARE @SortingByFeeFacilityVerifyDate BIT = NULL;
DECLARE @SortingBySalesPackageVerifyBy BIT = NULL;
DECLARE @SortingBySalesPackageVerifyDate BIT = NULL;
DECLARE @SortingByFCConfirmBy BIT = NULL;
DECLARE @SortingByFCConfirmDate BIT = NULL;

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

				--SELECT	 t.branch_id
				--		,t.[month]
				--		,t.[year]
				--		,t.item_id
				--		,i.item_desc
				--		,p.PackageDesc
				--		,t.category_id
				--		,t.item_amount
				--		,t.item_expense
				--		,t.remark
				--		,t.attachment
				--FROM tb_fee_monthly_transaction AS t
				--INNER JOIN tb_fee_items AS i ON t.item_id = i.item_id
				--LEFT JOIN tb_Package AS p ON i.item_desc = p.PackageID
				--WHERE t.item_id >= 129--branch_id IN (SELECT b_id FROM  @TbBranch)
				--	AND t.[month] = @Month
				--	AND t.[year] = @Year
				--	AND t.category_id = 5

SELECT	 b.ERP_ID AS ERPID
		,b.BranchID
		,t.ManagementFee
		,t.ServiceFeeIT
		,t.ServiceFeeSupply
		,t.Facility
		,t.SalesPackage
		,t.Adjustment
		,m.fee_management_verify_by		AS FeeManagementVerifyBy
		,m.fee_management_verify_date	AS FeeManagementVerifyDate
		,m.fee_it_verify_by				AS FeeItVerifyBy
		,m.fee_it_verify_date			AS FeeItVerifyDate
		,m.fee_supply_verify_by			AS FeeSupplyVerifyBy
		,m.fee_supply_verify_date		AS FeeSupplyVerifyDate
		,CASE WHEN v.[Status] = 1 THEN m.fee_facility_verify_by ELSE 'system' END AS FeeFacilityVerifyBy
		,CASE WHEN v.[Status] = 1 THEN m.fee_facility_verify_date ELSE GETDATE() END AS FeeFacilityVerifyDate
		,m.sales_package_verify_by		AS SalesPackageVerifyBy
		,m.sales_package_verify_date	AS SalesPackageVerifyDate
		,m.fc_confirm_by				AS FcConfirmBy
		,m.fc_confirm_date				AS FcConfirmDate
FROM tb_branch AS b
LEFT JOIN (
	SELECT	 t.branch_id
			,t.[year]
			,t.[month]
			,SUM(t.[1]) AS ManagementFee
			,SUM(t.[2]) AS ServiceFeeIT
			,SUM(t.[3]) AS ServiceFeeSupply
			,SUM(t.[4]) AS Facility
			,SUM(t.[5]) AS SalesPackage
			,SUM(t.[6]) AS Adjustment
	FROM (
		SELECT t.branch_id
			,t.[year]
			,t.[month]
			,CASE WHEN t.category_id = 1 THEN t.item_expense ELSE 0 END AS '1'
			,CASE WHEN t.category_id = 2 THEN t.item_expense ELSE 0 END AS '2'
			,CASE WHEN t.category_id = 3 THEN t.item_expense ELSE 0 END AS '3'
			,CASE WHEN t.category_id = 4 THEN t.item_expense ELSE 0 END AS '4'
			,CASE WHEN t.category_id = 5 THEN t.item_expense ELSE 0 END AS '5'
			,CASE WHEN t.category_id = 6 THEN t.item_expense ELSE 0 END AS '6'
		FROM (
			SELECT branch_id
				,[month]
				,[year]
				,item_id
				,category_id
				,item_amount
				,item_expense
				,remark
				,attachment
			FROM tb_fee_monthly_transaction
			WHERE branch_id IN (SELECT b_id FROM  @TbBranch)
				AND [month] = @Month
				AND [year] = @Year
		) AS t
	) AS t
	GROUP BY t.branch_id, t.[month], t.[year]
) AS t ON b.BranchID = t.branch_id
LEFT JOIN tb_monthly_commission AS m
	ON t.branch_id = m.branch_id
	AND t.[month] = m.[month]
	AND t.[year] = m.[year]
LEFT JOIN tb_master_verify_by_branch AS v
	ON t.branch_id = v.BranchID
WHERE b.BranchID IN (SELECT b_id FROM  @TbBranch) AND (@VerifyID IS NULL OR v.VerifyID = @VerifyID)
ORDER BY
	CASE WHEN @SortingByBranchID				= 1 THEN b.BranchID						ELSE '' END ASC,
	CASE WHEN @SortingByBranchID				= 0 THEN b.BranchID						ELSE '' END DESC,
	CASE WHEN @SortingByManagementFee			= 1 THEN t.ManagementFee				ELSE '' END ASC,
	CASE WHEN @SortingByManagementFee			= 0 THEN t.ManagementFee				ELSE '' END DESC,
	CASE WHEN @SortingByServiceFeeIT			= 1 THEN t.ServiceFeeIT					ELSE '' END ASC,
	CASE WHEN @SortingByServiceFeeIT			= 0 THEN t.ServiceFeeIT					ELSE '' END DESC,
	CASE WHEN @SortingByServiceFeeSupply		= 1 THEN t.ServiceFeeSupply				ELSE '' END ASC,
	CASE WHEN @SortingByServiceFeeSupply		= 0 THEN t.ServiceFeeSupply				ELSE '' END DESC,
	CASE WHEN @SortingByFacility				= 1 THEN t.Facility						ELSE '' END ASC,
	CASE WHEN @SortingByFacility				= 0 THEN t.Facility						ELSE '' END DESC,
	CASE WHEN @SortingBySalesPackage			= 1 THEN t.SalesPackage					ELSE '' END ASC,
	CASE WHEN @SortingBySalesPackage			= 0 THEN t.SalesPackage					ELSE '' END DESC,
	CASE WHEN @SortingByAdjustment				= 1 THEN t.Adjustment					ELSE '' END ASC,
	CASE WHEN @SortingByAdjustment				= 0 THEN t.Adjustment					ELSE '' END DESC,
	CASE WHEN @SortingByFeeManagementVerifyBy	= 1 THEN m.fee_management_verify_by		ELSE '' END ASC,
	CASE WHEN @SortingByFeeManagementVerifyBy	= 0 THEN m.fee_management_verify_by		ELSE '' END DESC,
	CASE WHEN @SortingByFeeManagementVerifyDate	= 1 THEN m.fee_management_verify_date	ELSE '' END ASC,
	CASE WHEN @SortingByFeeManagementVerifyDate	= 0 THEN m.fee_management_verify_date	ELSE '' END DESC,
	CASE WHEN @SortingByFeeItVerifyBy			= 1 THEN m.fee_it_verify_by				ELSE '' END ASC,
	CASE WHEN @SortingByFeeItVerifyBy			= 0 THEN m.fee_it_verify_by				ELSE '' END DESC,
	CASE WHEN @SortingByFeeItVerifyDate			= 1 THEN m.fee_it_verify_date			ELSE '' END ASC,
	CASE WHEN @SortingByFeeItVerifyDate			= 0 THEN m.fee_it_verify_date			ELSE '' END DESC,
	CASE WHEN @SortingByFeeSupplyVerifyBy		= 1 THEN m.fee_supply_verify_by			ELSE '' END ASC,
	CASE WHEN @SortingByFeeSupplyVerifyBy		= 0 THEN m.fee_supply_verify_by			ELSE '' END DESC,
	CASE WHEN @SortingByFeeSupplyVerifyDate		= 1 THEN m.fee_supply_verify_date		ELSE '' END ASC,
	CASE WHEN @SortingByFeeSupplyVerifyDate		= 0 THEN m.fee_supply_verify_date		ELSE '' END DESC,
	CASE WHEN @SortingByFeeFacilityVerifyBy		= 1 THEN m.fee_facility_verify_by		ELSE '' END ASC,
	CASE WHEN @SortingByFeeFacilityVerifyBy		= 0 THEN m.fee_facility_verify_by		ELSE '' END DESC,
	CASE WHEN @SortingByFeeFacilityVerifyDate	= 1 THEN m.fee_facility_verify_date		ELSE '' END ASC,
	CASE WHEN @SortingByFeeFacilityVerifyDate	= 0 THEN m.fee_facility_verify_date		ELSE '' END DESC,
	CASE WHEN @SortingBySalesPackageVerifyBy	= 1 THEN m.sales_package_verify_by		ELSE '' END ASC,
	CASE WHEN @SortingBySalesPackageVerifyBy	= 0 THEN m.sales_package_verify_by		ELSE '' END DESC,
	CASE WHEN @SortingBySalesPackageVerifyDate	= 1 THEN m.sales_package_verify_date	ELSE '' END ASC,
	CASE WHEN @SortingBySalesPackageVerifyDate	= 0 THEN m.sales_package_verify_date	ELSE '' END DESC,
	CASE WHEN @SortingByFCConfirmBy				= 1 THEN m.fc_confirm_by				ELSE '' END ASC,
	CASE WHEN @SortingByFCConfirmBy				= 0 THEN m.fc_confirm_by				ELSE '' END DESC,
	CASE WHEN @SortingByFCConfirmDate			= 1 THEN m.fc_confirm_date				ELSE '' END ASC,
	CASE WHEN @SortingByFCConfirmDate			= 0 THEN m.fc_confirm_date				ELSE '' END DESC
OFFSET @From ROWS 
FETCH NEXT @To ROWS ONLY
