	DECLARE @Month int = 2
	DECLARE @Year int = 2019
	
	
	BEGIN TRANSACTION T1
	--//***** Begin Syntax *****//--
		
		--============================================================
		UPDATE m
		SET  [income_freight] = ISNULL(s.IncomeTotalFreightRevenue, 0)
			,[income_dhl] = ISNULL(s.IncomeDHL, 0)
			,[income_dhl_adjustment] = ISNULL(s.DHLAdjustment, 0)
			,[income_cod] = ISNULL(s.IncomeCOD, 0)
			,[income_insurance] = ISNULL(s.IncomeInsurance, 0)
			,[income_sameday] = ISNULL(s.IncomeSameday, 0)
			,[income_dropoff] = ISNULL(s.IncomeDropoff, 0)
			,[expense_cod] = ISNULL(s.ExpenseCOD, 0)
			,[expense_insurance] = ISNULL(s.ExpenseInsurance, 0)
			,[expense_fee_management] = ISNULL(s.ExpenseFeeManagement, 0)
			,[expense_fee_it] = ISNULL(s.ExpenseFeeIT, 0)
			,[expense_fee_supply] = ISNULL(s.ExpenseFeeSupply, 0)
			,[expense_fee_facility] = ISNULL(s.ExpenseFeeFacility, 0)
			,[expense_sales_package] = ISNULL(s.ExpenseSalesPackage, 0)
			,[adjustment] = ISNULL(s.Adjustment, 0)
			,[total_commission] = ISNULL(CAST(ROUND(
						(
							(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeCOD + s.IncomeSameday + s.IncomeDropoff + s.IncomeRTSP + s.IncomePSP)
							-
							(s.ExpenseCOD + s.ExpenseInsurance + s.ExpenseFeeManagement + s.ExpenseFeeIT + s.ExpenseFeeSupply + s.ExpenseFeeFacility + s.ExpenseSalesPackage)
						)
						+ s.Adjustment
				, 2) AS MONEY), 0)
			,[net_commission] = ISNULL(CAST(ROUND(
					CASE WHEN s.Vatable = 1
					THEN
						(
							(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + s.IncomeRTSP + s.IncomePSP + (s.Adjustment))
							-
							(
								(
									(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + s.IncomeRTSP + s.IncomePSP + (s.Adjustment))
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
									(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + s.IncomeRTSP + s.IncomePSP) + (s.Adjustment)
									+
									(s.IncomeCOD) - s.ExpenseCOD - s.ExpenseInsurance - s.ExpenseFeeManagement - s.ExpenseFeeIT - s.ExpenseFeeSupply - s.ExpenseFeeFacility - s.ExpenseSalesPackage
								)
								-
								((IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + s.IncomeRTSP + s.IncomePSP + (s.Adjustment)) *0.03)
							)
						)
					END
				, 2) AS MONEY), 0)
			,[expense_to_erp] = ISNULL(CAST(ROUND(
					(s.CODAmount * 0.02)
					+
					(s.InsuranceAmount * 0.7)
					+
					s.ExpenseFeeManagement
					+
					s.ExpenseFeeIT
					+
					s.ExpenseFeeSupply
					+
					s.ExpenseFeeFacility
					+
					s.ExpenseSalesPackage
				, 2) AS MONEY), 0)
			,[fc_confirm_by] = 'system'
			,[fc_confirm_date] = GETDATE()
		FROM tb_monthly_commission AS m
		INNER JOIN (
			SELECT	 b.ERP_ID AS ERPID
					,m.branch_id AS BranchID
					,ISNULL(m.[month], @Month) AS [Month]
					,ISNULL(m.[year], @Year) AS [Year]
					,ISNULL(rate.vat, 0) AS Vatable
					,ISNULL(rate.commission, 0) AS CommissionRate

					/****** EXCEL DETAIL ******/
					,d.Boxes
					,d.PackageSurcharge

					/****** PACKAGE DETAIL ******/
					,p.BoxMini
					,p.BoxS
					,p.BoxSPlus
					,p.BoxM
					,p.BoxMPlus
					,p.BoxL

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
					,CAST(ROUND((d.CODSurcharge / 3) * 100, 2) AS MONEY) AS CODAmount
					--,d.CODAmount
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
						,CAST(ROUND(
							(d.TotalFreightRevenue * ISNULL(rate.commission, 0))
							/
							100
						, 2) AS MONEY) AS IncomeTotalFreightRevenue
						,CAST(ROUND(
							(d.DHLAmount + (m.income_dhl_adjustment))
							*
							(rate.dhl * 0.01)
						, 2) AS MONEY) AS IncomeDHL
						,CAST(ROUND(d.CODSurcharge / 3, 2) AS MONEY) AS IncomeCOD
						,CAST(ROUND(d.InsuranceAmount * (rate.insure * 0.01), 2) AS MONEY) AS IncomeInsurance
						,CAST(ROUND(d.TotalSamedayRevenue * (rate.bsd * 0.01), 2) AS MONEY) AS IncomeSameday
						,d.DropOffRevenue * 2 AS IncomeDropoff
						,m.income_rtsp AS IncomeRTSP
						,m.income_psp AS IncomePSP
						-- =============================================

						/* Expense */
						-- =============================================
						,d.CODSurcharge AS ExpenseCOD
						,CAST(ROUND(
							d.InsuranceAmount
							-
							(d.InsuranceAmount * (rate.insure * 0.01))
						, 2) AS MONEY) AS ExpenseInsurance
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
			FROM tb_monthly_commission AS m WITH (NOLOCK)
			INNER JOIN tb_branch AS b ON m.branch_id = b.BranchID
			LEFT JOIN tb_master_verify_by_branch AS v
				ON m.branch_id = v.BranchID
			LEFT JOIN tb_commission_rate AS rate
				ON m.branch_id = rate.branch_id
				AND rate.as_of_month = (@Year*100)+@Month
			LEFT JOIN (
				SELECT	 d.BranchID
						--,d.ReportDate
						--,*
						,SUM(d.Boxes) AS Boxes
						,SUM(
							  ISNULL(d.PkgSurcharge,0)
							+ ISNULL(d.PkgService,0)
						) AS PackageSurcharge

						,SUM(
							  ISNULL(d.Cash, 0)
							+ ISNULL(d.CashForService, 0)
							-- + ISNULL(d.LineTopUpService, 0)
						) AS Cash
						,SUM(ISNULL(d.Rabbit, 0)) AS Rabbit
						,SUM(ISNULL(d.Credit, 0)) AS CreditCard
						,SUM(ISNULL(d.LinePay, 0)) AS LinePay
						,SUM(
							  ISNULL(d.FreightSurcharge,0) 
							+ ISNULL(d.AMSurcharge,0)
							+ ISNULL(d.PUPSurcharge,0)
							+ ISNULL(d.SatDelSurcharge,0)
							+ ISNULL(d.RemoteAreaSurcharge,0)
							+ ISNULL(d.CODSurcharge,0)
							+ ISNULL(d.InsurSurcharge,0)
							+ ISNULL(d.PkgSurcharge,0)
							+ ISNULL(d.PkgService,0)
						) AS TotalRevenue

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
				WHERE MONTH(d.ReportDate) = @Month
					AND YEAR(d.ReportDate) = @Year
					AND ISNULL(d.Captured, 0) = 1
				GROUP BY d.BranchID
			) AS d ON m.branch_id = d.BranchID
			LEFT JOIN (
				SELECT ft.branch_id,
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 48 THEN ft.item_amount ELSE 0 end) as [BoxMini],
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 44 THEN ft.item_amount ELSE 0 end) as [BoxS],
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 49 THEN ft.item_amount ELSE 0 end) as [BoxSPlus],
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 45 THEN ft.item_amount ELSE 0 end) as [BoxM],
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 59 THEN ft.item_amount ELSE 0 end) as [BoxMPlus],
					SUM(CASE WHEN ft.category_id=5 and ft.item_id = 46 THEN ft.item_amount ELSE 0 end) as [BoxL]
				FROM tb_fee_monthly_transaction as ft with (nolock)
				INNER JOIN tb_fee_items as fi with (nolock)
				ON ft.item_id = fi.item_id
					and ft.category_id = fi.category_id
				WHERE ft.[month] = @Month
					AND ft.[year] = @Year
				GROUP BY ft.branch_id
			) AS p ON m.branch_id = p.branch_id
			WHERE m.[year] = @Year AND m.[Month] = @Month
		) AS s ON m.branch_id = s.BranchID AND m.[year] = s.Year AND m.[month] = s.[Month]
		WHERE m.[year] = @Year
			AND m.[month] = @Month
			AND ISNULL(m.send_to_erp, 0) = 0
			AND s.FcConfirmDate IS NULL
			AND s.FeeFacilityVerifyDate		IS NOT NULL
			AND s.FeeManagementVerifyDate	IS NOT NULL
			AND s.FeeItVerifyDate			IS NOT NULL
			AND s.FeeSupplyVerifyDate		IS NOT NULL
			AND s.SalesPackageVerifyDate	IS NOT NULL
		--============================================================
		
	--============================================================

	--//***** Finally Process *****//--
	IF (@@ERROR <> 0)
	BEGIN
		rollback transaction T1
	END ELSE
	BEGIN
		commit transaction T1
	END;