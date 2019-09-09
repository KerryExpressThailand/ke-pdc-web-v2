DECLARE @From INT = 0;
DECLARE @To INT = 20;
DECLARE @VerifyID varchar(50);

DECLARE @BranchIDList varchar(MAX) = 'BYAI,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI,SUKS,BBON,DONM,TSIT,ONUT,NAWA,LKAB,PINK,TPLU,NMIN,RMA2,BKEN,NAIN,BANA,SMUT,TEPA,BSTO,TKRU,NKAM,BPEE,TNPT,TUPM,TTAI,KBAN,EKKA,PANT,SAMK,NJOK,SLYA,MPTN,TYA3,TAIT,MINB,CHC4,PTNK,BROM,NLCH,KVIL,LPDU,SNBN,CWNA,PS43,TAC4,PBSK,SNDA,BSAE,SNOI,LAMB,BANB,LKAE,SUAS,ROMK,TYA6,BAPU,MCCS,AMTA,NKCS,KSWA,PTYA,BWIN,PYSC,MNKP,POKW,LUK2,SAP2,PYSL,PWET,BTEC,PNAM,ONTC,TTLY,FPAK,SEC2,SATU,SAI4,LAK4,SKMT,BAKY'
--SET @BranchIDList = 'BAKY';
DECLARE @Month int = 3
DECLARE @Year int = 2017
DECLARE @Franchise int = 0;

	-- 1. Get Branch to Temp Table
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
	
	SELECT b.BranchID,
		rev.Boxes,
		rev.TotalRevenue,
		rev.PackageSurcharge,
		rev.FreightSurcharge,
		rate.commission as CommissionRate,
		CAST(rev.FreightSurcharge * rate.commission*0.01 AS MONEY) AS [FreightComm],
		rev.DHLRevenue + (mc.income_dhl_adjustment) AS DHLRevenue,
		((rev.DHLRevenue + (mc.income_dhl_adjustment)) * rate.dhl * 0.01) as [DHLComm],
		rev.CODAmount,
		rev.CODSurcharge,
		(rev.CODAmount * rate.cod * 0.01) as [CODComm],
		rev.InsurSurcharge,
		(rev.InsurSurcharge * rate.insure * 0.01) as [InsurComm],
		(rev.InsurSurcharge - (rev.InsurSurcharge * rate.insure * 0.01)) as [InsurKerry],
		rev.BSDAmount as [Sameday],
		(rev.BSDAmount * rate.bsd * 0.01) as [SamedayComm],
		rev.DropOffBox,
		(rev.DropOffBox * rate.dropoff) as [DropOffComm],
		Expense.ManagementFee as [Exp-ManagementFee],
		Expense.[ServiceFee-IT] as [Exp-ServiceFee-IT],
		Expense.[ServiceFee-Supply] as [Exp-ServiceFee-Supply],
		Expense.[ServiceFee-Facility] as [Exp-ServiceFee-Facility],
		Expense.[Adjustment-Other],
		Expense.[Box Mini],
		Expense.[Box S],
		Expense.[Box S+],
		Expense.[Box M],
		Expense.[Box M+],
		Expense.[Box L],
		Expense.[Sales-Package],

		-- F6+I6+K6+Q6+S6-L6-O6-T6-U6-V6-W6-AD6
		(rev.FreightSurcharge * rate.commission*0.01)	-- F6
		+ (rev.DHLRevenue * rate.dhl * 0.01)			-- I6
		+ (rev.CODAmount * rate.cod * 0.01)				-- K6
		+ (rev.BSDAmount * rate.bsd * 0.01)				-- Q6
		+ (rev.DropOffBox * rate.dropoff)				-- S6
		- rev.CODSurcharge								-- L6
		- (rev.InsurSurcharge - (rev.InsurSurcharge * rate.insure * 0.01)) -- O6
		- Expense.ManagementFee							-- T6
		- Expense.[ServiceFee-IT]						-- U6
		- Expense.[ServiceFee-Supply]					-- V6
		- Expense.[ServiceFee-Facility]					-- W6
		- Expense.[Sales-Package]						-- AD6
		+ (Expense.[Adjustment-Other])
		
		AS TotalComm
	FROM tb_branch AS b
	LEFT JOIN (
		SELECT r.BranchID,
			SUM(r.Boxes) as Boxes,
			SUM(ISNULL(r.Cash,0) 
				+ ISNULL(r.CashForService,0)
				+ ISNULL(r.Rabbit,0)
				+ ISNULL(r.Credit,0)
				+ ISNULL(r.LinePay,0)) AS TotalRevenue,
			SUM(ISNULL(r.PkgSurcharge,0)
				+ ISNULL(r.PkgService,0)) as PackageSurcharge,
			SUM(ISNULL(r.FreightSurcharge,0) 
				+ ISNULL(r.AMSurcharge,0)
				+ ISNULL(r.PUPSurcharge,0)
				+ ISNULL(r.SatDelSurcharge,0)
				+ ISNULL(r.RemoteAreaSurcharge,0)) as FreightSurcharge,
			SUM(ISNULL(r.DHLService,0)) as DHLRevenue,
			SUM(ISNULL(r.CODAmount,0)) as CODAmount,
			SUM(ISNULL(r.CODSurcharge,0)) as CODSurcharge,
			SUM(ISNULL(r.InsurSurcharge,0)) as InsurSurcharge,
			SUM(ISNULL(r.BSDForService,0)) as BSDAmount,
			SUM(ISNULL(r.DropOffBox,0)) as DropOffBox -- adjust amount from Friend
		FROM tb_daily_revenue as r with (nolock)
		WHERE r.BranchID in (SELECT b_id FROM @TbBranch)
		AND YEAR(r.ReportDate) = @Year
		AND Month(r.ReportDate) = @Month
		GROUP BY r.BranchID
	) AS rev ON b.BranchID = rev.BranchID
	LEFT JOIN
	(
		SELECT cr.branch_id,
			cr.commission,
			cr.dropoff,
			cr.dhl,
			cr.cod,
			cr.insure,
			cr.bsd,
			cr.rtsp
		FROM tb_Commission_Rate AS cr WITH (NOLOCK)
		WHERE cr.as_of_month = (@Year*100 + @Month)
	) AS rate ON b.BranchID = rate.branch_id
	LEFT JOIN
	(
		SELECT ft.branch_id,
			SUM(CASE WHEN ft.category_id=1 THEN ft.item_expense ELSE 0 end) as [ManagementFee],
			SUM(CASE WHEN ft.category_id=2 THEN ft.item_expense ELSE 0 end) as [ServiceFee-IT],
			SUM(CASE WHEN ft.category_id=3 THEN ft.item_expense ELSE 0 end) as [ServiceFee-Supply],
			SUM(CASE WHEN ft.category_id=4 THEN ft.item_expense ELSE 0 end) as [ServiceFee-Facility],
			SUM(CASE WHEN ft.category_id=5 THEN ft.item_expense ELSE 0 end) as [Sales-Package],
			SUM(CASE WHEN ft.category_id=6 THEN ft.item_expense ELSE 0 end) as [Adjustment-Other],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 48 THEN ft.item_amount ELSE 0 end) as [Box Mini],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 44 THEN ft.item_amount ELSE 0 end) as [Box S],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 49 THEN ft.item_amount ELSE 0 end) as [Box S+],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 45 THEN ft.item_amount ELSE 0 end) as [Box M],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 59 THEN ft.item_amount ELSE 0 end) as [Box M+],
			SUM(CASE WHEN ft.category_id=5 and ft.item_id = 46 THEN ft.item_amount ELSE 0 end) as [Box L]
		FROM tb_fee_monthly_transaction as ft with (nolock)
		INNER JOIN tb_fee_items as fi with (nolock)
		ON ft.item_id = fi.item_id
			AND ft.category_id = fi.category_id
		WHERE ft.[month] = @Month
			AND ft.[year] = @Year
			AND ft.branch_id in (SELECT b_id FROM @TbBranch)
		GROUP BY ft.branch_id
	) AS Expense
	ON b.BranchID = Expense.branch_id
	LEFT JOIN tb_monthly_commission AS mc WITH (NOLOCK)
	ON	b.BranchID = mc.branch_id 
			AND @Month = mc.[month]
			AND @Year = mc.[year]
	WHERE b.BranchID IN (SELECT b_id FROM  @TbBranch)
	--AND rate.commission IS NULL