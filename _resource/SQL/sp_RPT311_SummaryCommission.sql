DECLARE @From INT = 0;
DECLARE @To INT = 20;
DECLARE @VerifyID varchar(50);

DECLARE @BranchIDList varchar(MAX) = 'BYAI,RSIT,KKAW,MTNG,MAHA,PKED,SCON,BKAE,BBUA,HPPY,TNON,SMAI,BBON,TSIT,SUKS,DONM,NAWA,LKAB,ONUT,SMUT,TEPA,BPEE,NAIN,TUPM,TNPT,BANA,TTAI,NKAM,PANT,KBAN,SAMK,EKKA,TAIT,NMIN,TPLU,PINK,RMA2,BSTO,PTNK,MPTN,NJOK,CHC4,SLYA,TKRU,BKEN,MINB,TYA3,BROM,NLCH,KVIL,LPDU,PS43,TAC4,BSAE,LAMB,SNOI,CWNA,SAP2,SNBN,SNDA,LKAE,SUAS,LUK2,ROMK,BANB,KSWA,MCCS,PBSK,PYSL,AMTA,TYA6,BAPU,NKCS,POKW,PTYA,BWIN,MNKP,ONTC,PNAM,PYSC,PWET,BTEC,FPAK,SAI4,BAKY,SKMT,'
DECLARE @Month int = 3
DECLARE @Year int = 2017
DECLARE @Franchise int = 0;

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






SELECT	 s.ERPID
		,s.BranchID
		,s.Month
		,s.Year
		,s.Vatable
		,s.CommissionRate

		/****** EXCEL DETAIL ******/

		/****** CASH SUMMARY ******/
		,s.Cash
		,s.Rabbit
		,s.CreditCard
		,s.LinePay
		,s.TotalRevenue
		,s.Transport
		,s.AM
		,s.PUP
		,s.SATDelivery
		,s.RAS
		,s.TotalFreightRevenue
		,s.DHLAmount
		,s.DHLAdjustment
		,s.CODAmount
		,s.InsuranceAmount
		,s.SamedayCITY
		,s.SamedayCITYN
		,s.SamedayGRABEX
		,s.TotalSamedayRevenue
		,s.DropOffRevenue

		/****** COMMISSION SUMMARY ******/
		,s.IncomeTotalFreightRevenue
		,s.IncomeDHL
		,s.IncomeCOD
		,s.IncomeInsurance
		,s.IncomeSameday
		,s.IncomeDropoff
		,s.ExpenseCOD
		,s.ExpenseInsurance
		,s.ExpenseFeeManagement
		,s.ExpenseFeeIT
		,s.ExpenseFeeSupply
		,s.ExpenseFeeFacility
		,s.ExpenseSalesPackage
		,s.Adjustment
		,CAST(
				(
					(s.IncomeTotalFreightRevenue + IncomeDHL + IncomeCOD + IncomeSameday + IncomeDropoff)
					-
					(ExpenseCOD + ExpenseInsurance + ExpenseFeeManagement + ExpenseFeeIT + ExpenseFeeSupply + ExpenseFeeFacility + ExpenseSalesPackage)
				)
				+ Adjustment
		AS MONEY) AS TotalCommission
		,CAST(
			CASE WHEN Vatable = 1
			THEN
				(
					(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + (s.Adjustment))
					-
					(
						(
							(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + (s.Adjustment))
							* 100/107
						)*0.03
					)
					+
					((s.IncomeCOD) - s.ExpenseCOD - s.ExpenseInsurance - s.ExpenseFeeManagement - s.ExpenseFeeIT - s.ExpenseFeeSupply - s.ExpenseFeeFacility - s.ExpenseSalesPackage)
				)
			ELSE
				(
					(
						(
							(IncomeTotalFreightRevenue + IncomeDHL + IncomeSameday + IncomeDropoff) + (Adjustment)
							+
							((IncomeCOD) - ExpenseCOD - ExpenseInsurance - ExpenseFeeManagement - ExpenseFeeIT - ExpenseFeeSupply - ExpenseFeeFacility - ExpenseSalesPackage) - (CASE WHEN Adjustment < 0 THEN Adjustment ELSE 0 END)
						)
						-
						((IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + (s.Adjustment)) *0.03)
					)
				)
			END
		AS MONEY) AS NetCommission

		/****** VERIFY STATUS ******/
		,s.FeeManagementVerifyBy
		,s.FeeManagementVerifyDate
		,s.FeeItVerifyBy
		,s.FeeItVerifyDate
		,s.FeeSupplyVerifyBy
		,s.FeeSupplyVerifyDate
		,s.FeeFacilityVerifyBy
		,s.FeeFacilityVerifyDate
		,s.SalesPackageVerifyBy
		,s.SalesPackageVerifyDate

		/****** FC CONFIRM STATUS ******/
		,s.FcConfirmBy
		,s.FcConfirmDate

		/****** ERP STATUS ******/
		,s.PRNo
		,s.PRDate
		,s.SendToERP
		,s.SendToERPDate
FROM (
	SELECT	 b.ERP_ID AS ERPID
			,b.BranchID
			,ISNULL(m.[month], @Month) AS [Month]
			,ISNULL(m.[year], @Year) AS [Year]
			,ISNULL(b.fc_vatable, 0) AS Vatable
			,ISNULL(b.commission_rate, 0) AS CommissionRate

			/****** CASH SUMMARY ******/
			-- =============================================
			,d.Cash
			,d.Rabbit
			,d.CreditCard
			,d.LinePay
			,d.TotalRevenue
			,d.Transport
			,d.AM
			,d.PUP
			,d.SATDelivery
			,d.RAS
			,d.TotalFreightRevenue
			,d.DHLAmount
			,m.income_dhl_adjustment AS DHLAdjustment
			,d.CODAmount
			,d.InsuranceAmount
			,d.SamedayCITY
			,d.SamedayCITYN
			,d.SamedayGRABEX
			,d.TotalSamedayRevenue
			,d.DropOffRevenue
			-- =============================================

			/****** COMMISSION SUMMARY ******/
			-- =============================================
				/* INCOME */
				-- =============================================
				,CAST(
					(d.TotalFreightRevenue * ISNULL(b.commission_rate, 0))
					/
					100
				AS MONEY) AS IncomeTotalFreightRevenue
				,CAST(
					(d.DHLAmount + (m.income_dhl_adjustment))
					*
					0.15
				AS MONEY) AS IncomeDHL
				,CAST(d.CODAmount * 0.01 AS MONEY) AS IncomeCOD
				,CAST(d.InsuranceAmount * 0.3 AS MONEY) AS IncomeInsurance
				,CAST(d.TotalSamedayRevenue * 0.3 AS MONEY) AS IncomeSameday
				,d.DropOffRevenue * 2 AS IncomeDropoff
				-- =============================================

				/* Expense */
				-- =============================================
				,d.CODSurcharge AS ExpenseCOD
				,CAST(d.InsuranceAmount * 0.7 AS MONEY) AS ExpenseInsurance
				,m.expense_fee_management AS ExpenseFeeManagement
				,m.expense_fee_it AS ExpenseFeeIT
				,m.expense_fee_supply AS ExpenseFeeSupply
				,m.expense_fee_facility AS ExpenseFeeFacility
				,m.expense_sales_package AS ExpenseSalesPackage
				,m.adjustment AS Adjustment
				-- =============================================
			-- =============================================


			/****** VERIFY STATUS ******/
			-- =============================================
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
			-- =============================================

			/****** FC CONFIRM STATUS ******/
			-- =============================================
			,m.fc_confirm_by				AS FcConfirmBy
			,m.fc_confirm_date				AS FcConfirmDate
			-- =============================================

			/****** ERP STATUS ******/
			-- =============================================
			,m.pr_no AS PRNo
			,m.pr_date AS PRDate
			,ISNULL(m.send_to_erp, 0) AS SendToERP
			,m.send_to_erp_date AS SendToERPDate
			-- =============================================
	FROM tb_branch AS b
	LEFT JOIN tb_master_verify_by_branch AS v
		ON b.BranchID = v.BranchID
	LEFT JOIN tb_monthly_commission AS m WITH (NOLOCK)
		ON	b.BranchID = m.branch_id 
			AND @Month = m.[month]
			AND @Year = m.[year]
			AND (
				@Franchise = 0
				OR
				(
					(
						m.fee_facility_verify_date IS NOT NULL
						OR
						ISNULL(v.[Status], 0) = 0
					)
					AND m.fee_management_verify_date	IS NOT NULL
					AND m.fee_it_verify_date			IS NOT NULL
					AND m.fee_supply_verify_date		IS NOT NULL
					AND m.sales_package_verify_date		IS NOT NULL
					AND m.[month] > 3
					AND m.[year] > 2016
				)
			)
	LEFT JOIN (
		SELECT	 d.BranchID
				--,d.ReportDate
				--,*
				,SUM(
					  ISNULL(d.Cash, 0)
					+ ISNULL(d.CashForService, 0)
					-- + ISNULL(d.LineTopUpService, 0)
				) AS Cash
				,SUM(ISNULL(d.Rabbit, 0)) AS Rabbit
				,SUM(ISNULL(d.Credit, 0)) AS CreditCard
				,SUM(ISNULL(d.LinePay, 0)) AS LinePay
				,SUM((
					  ISNULL(d.Cash, 0)
					+ ISNULL(d.CashForService, 0)
					--+ ISNULL(d.LineTopUpService, 0) --********
					+ ISNULL(d.Rabbit, 0)
					+ ISNULL(d.Credit, 0)
					+ ISNULL(d.LinePay, 0)
				)) AS TotalRevenue

				,SUM(ISNULL(d.FreightSurcharge, 0)) AS Transport
				,SUM(ISNULL(d.AMSurcharge, 0)) AS AM
				,SUM(ISNULL(d.PUPSurcharge, 0)) AS PUP
				,SUM(ISNULL(d.SatDelSurcharge, 0)) AS SATDelivery
				,SUM(ISNULL(d.RemoteAreaSurcharge, 0)) AS RAS
				,SUM((
					  ISNULL(d.FreightSurcharge, 0)
					+ ISNULL(d.AMSurcharge, 0)
					+ ISNULL(d.PUPSurcharge, 0)
					+ ISNULL(d.SatDelSurcharge, 0)
					+ ISNULL(d.RemoteAreaSurcharge, 0)
				)) AS TotalFreightRevenue

				,SUM(ISNULL(d.DHLService, 0)) AS DHLAmount
				,SUM(ISNULL(d.CODAmount, 0)) AS CODAmount
				,SUM(ISNULL(d.InsurSurcharge, 0)) AS InsuranceAmount
		
				,SUM(ISNULL(d.CITYSurcharge, 0)) AS SamedayCITY
				,SUM(ISNULL(d.CITYNSurcharge, 0)) AS SamedayCITYN
				,SUM(ISNULL(d.GRABEXSurcharge, 0)) AS SamedayGRABEX
				,SUM((
					  ISNULL(d.CITYSurcharge, 0)
					+ ISNULL(d.CITYNSurcharge, 0)
					+ ISNULL(d.GRABEXSurcharge, 0)
				)) AS TotalSamedayRevenue
				,SUM(ISNULL(d.DropOffBox, 0)) AS DropOffRevenue
				,SUM(ISNULL(d.CODSurcharge, 0)) AS CODSurcharge
		FROM tb_daily_revenue AS d WITH (NOLOCK)
		WHERE	d.BranchID IN (SELECT b_id FROM @TbBranch)
			--AND DAY(d.ReportDate) = 3
			AND MONTH(d.ReportDate) = @Month--4
			AND YEAR(d.ReportDate) = @Year
			AND ISNULL(d.Captured, 0) = 1
		GROUP BY d.BranchID
	) AS d ON b.BranchID = d.BranchID
	WHERE b.BranchID <> ''
		AND b.BranchID IN (SELECT b_id FROM  @TbBranch)
		AND (
			@VerifyID IS NULL
			OR
			v.VerifyID = @VerifyID
		)
) AS s