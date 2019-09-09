DECLARE @BranchIDList varchar(MAX) = 'BYAI,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI,SUKS,BBON,DONM,TSIT,ONUT,NAWA,LKAB,PINK,TPLU,NMIN,RMA2,BKEN,NAIN,BANA,SMUT,TEPA,BSTO,TKRU,NKAM,BPEE,TNPT,TUPM,TTAI,KBAN,EKKA,PANT,SAMK,NJOK,SLYA,MPTN,TYA3,TAIT,MINB,CHC4,PTNK,BROM,NLCH,KVIL,LPDU,SNBN,CWNA,PS43,TAC4,PBSK,SNDA,BSAE,SNOI,LAMB,BANB,LKAE,SUAS,ROMK,TYA6,BAPU,MCCS,AMTA,NKCS,KSWA,PTYA,BWIN,PYSC,MNKP,POKW,LUK2,SAP2,PYSL,PWET,BTEC,PNAM,ONTC,TTLY,FPAK,SEC2,SATU,SAI4,LAK4,SKMT,BAKY'
DECLARE @Month int = 5
DECLARE @Year int = 2017

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

SELECT *
FROM (
	SELECT	 m.branch_id
			,t.[month]
			,t.[year]
			,t.item_id
			,t.category_id
			,t.item_expense
			,m.fee_management_verify_date
			,ROW_NUMBER() OVER(PARTITION BY m.branch_id ORDER BY t.[year],t.[month] DESC) num
	FROM tb_fee_monthly_transaction AS t
	LEFT JOIN tb_monthly_commission AS m
		ON t.branch_id = m.branch_id
		AND t.[month] = m.[month]
		AND t.[year] = m.[year]
	WHERE t.branch_id IN (SELECT b_id FROM  @TbBranch)
		AND t.[month] < @Month
		AND t.[year] <= @Year
		AND t.category_id = 1
		--AND m.fee_management_verify_date IS NOT NULL
) AS m
WHERE m.num = 1