/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2014 (12.0.2000)
    Source Database Engine Edition : Microsoft SQL Server Standard Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[sp_RPT311_SummaryCommission]    Script Date: 28/8/2560 15:24:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_PDC_Report_MonthlyCommissionSummary_Get]
	@UserID varchar(20), -- bphi
	@BranchIDList varchar(MAX), -- ASK,BANA,SLOM,
	@Month int, --2
	@Year int, --2017
	@Franchise int = 0,
	@VerifyID varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRANSACTION T1
	--//***** Begin Syntax *****//--
	
		--============================================================
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

		IF @VerifyID = ''
		BEGIN
			SET @VerifyID = NULL;
		END
		--============================================================
		
		--============================================================
		SELECT	 s.ERPID
				,s.BranchID
				,s.Month
				,s.Year
				,s.Vatable
				,s.CommissionRate
				,s.DropOffRate
				,s.DHLRate
				,s.CODRate
				,s.InsureRate
				,s.BSDRate
				,s.RTSPRate
				,s.PSPRate

				/****** EXCEL DETAIL ******/
				,s.Boxes
				,s.PackageSurcharge

				/****** PACKAGE DETAIL ******/
				,s.BoxMini
				,s.BoxS
				,s.BoxSPlus
				,s.BoxM
				,s.BoxMPlus
				,s.BoxL

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
				,s.IncomeRTSP
				,s.IncomePSP
				,CAST(ROUND(
					  s.IncomeTotalFreightRevenue
					+ s.IncomeDHL
					+ s.IncomeCOD
					+ s.IncomeInsurance
					+ s.IncomeSameday
					+ s.IncomeDropoff
					+ s.IncomeRTSP
					+ s.IncomePSP
				, 2) AS MONEY) AS TotalIncome
				,s.ExpenseCOD
				,s.ExpenseInsurance
				,s.ExpenseFeeManagement
				,s.ExpenseFeeIT
				,s.ExpenseFeeSupply
				,s.ExpenseFeeFacility
				,s.ExpenseSalesPackage
				,CAST(ROUND(
					  s.ExpenseCOD
					+ s.InsuranceAmount
					+ s.ExpenseFeeManagement
					+ s.ExpenseFeeIT
					+ s.ExpenseFeeSupply
					+ s.ExpenseFeeFacility
					+ s.ExpenseSalesPackage
				, 2) AS MONEY) AS TotalExpense
				,s.Adjustment
				,'' AS AdjustmentRemark
				,CAST(ROUND(
						(
							(s.IncomeTotalFreightRevenue + IncomeDHL + IncomeCOD + IncomeSameday + IncomeDropoff + IncomeRTSP + IncomePSP)
							-
							(ExpenseCOD + ExpenseInsurance + ExpenseFeeManagement + ExpenseFeeIT + ExpenseFeeSupply + ExpenseFeeFacility + ExpenseSalesPackage)
						)
						+ Adjustment
				, 2) AS MONEY) AS TotalCommission
				,CAST(ROUND(
					CASE WHEN s.Vatable = 1
					THEN
						(
							(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + IncomeRTSP + IncomePSP + (s.Adjustment))
							-
							(
								(
									(s.IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + IncomeRTSP + IncomePSP + (s.Adjustment))
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
									(IncomeTotalFreightRevenue + IncomeDHL + IncomeSameday + IncomeDropoff + IncomeRTSP + IncomePSP) + (Adjustment)
									+
									(IncomeCOD) - ExpenseCOD - ExpenseInsurance - ExpenseFeeManagement - ExpenseFeeIT - ExpenseFeeSupply - ExpenseFeeFacility - ExpenseSalesPackage
								)
								-
								((IncomeTotalFreightRevenue + s.IncomeDHL + s.IncomeSameday + s.IncomeDropoff + IncomeRTSP + IncomePSP + (s.Adjustment)) *0.03)
							)
						)
					END
				, 2) AS MONEY) AS NetCommission

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
					,ISNULL(rate.vat, 0) AS Vatable
					,ISNULL(rate.commission, 0) AS CommissionRate
					,ISNULL(rate.dropoff, 0) AS DropOffRate
					,ISNULL(rate.dhl, 0) AS DHLRate
					,ISNULL(rate.cod, 0) AS CODRate
					,ISNULL(rate.insure, 0) AS InsureRate
					,ISNULL(rate.bsd, 0) AS BSDRate
					,ISNULL(rate.rtsp, 0) AS RTSPRate
					,ISNULL(rate.psp, 0) AS PSPRate

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
						,CAST(ROUND(d.CODAmount * (rate.cod * 0.01), 2) AS MONEY) AS IncomeCOD
						,CAST(ROUND(d.InsuranceAmount * (rate.insure * 0.01), 2) AS MONEY) AS IncomeInsurance
						,CAST(ROUND(d.TotalSamedayRevenue * (rate.bsd * 0.01), 2) AS MONEY) AS IncomeSameday
						,d.DropOffRevenue * 2 AS IncomeDropoff
						,CAST(0 AS MONEY) AS IncomeRTSP
						,CAST(0 AS MONEY) AS IncomePSP
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
			FROM tb_branch AS b
			LEFT JOIN tb_master_verify_by_branch AS v
				ON b.BranchID = v.BranchID
			LEFT JOIN tb_commission_rate AS rate
				ON b.BranchID = rate.branch_id
				AND rate.as_of_month = (@Year*100)+@Month
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
							--AND m.[month] > 3
							--AND m.[year] > 2016
						)
					)
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
				WHERE	d.BranchID IN (SELECT b_id FROM @TbBranch)
					--AND DAY(d.ReportDate) = 3
					AND MONTH(d.ReportDate) = @Month--4
					AND YEAR(d.ReportDate) = @Year
					AND ISNULL(d.Captured, 0) = 1
				GROUP BY d.BranchID
			) AS d ON b.BranchID = d.BranchID
			LEFT JOIN (
				SELECT	 ""
						,1234 AS Revenue
			) AS d_rtsp ON
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
					AND ft.branch_id in (SELECT b_id FROM @TbBranch)
				GROUP BY ft.branch_id
			) AS p ON b.BranchID = p.branch_id
			WHERE b.BranchID <> ''
				AND b.BranchID IN (SELECT b_id FROM  @TbBranch)
				AND (
					@VerifyID IS NULL
					OR
					v.VerifyID = @VerifyID
				)
		) AS s
		WHERE s.[Year]*100+s.[Month] > 201702
		ORDER BY s.FcConfirmDate
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
END
