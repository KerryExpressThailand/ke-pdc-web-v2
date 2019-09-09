DECLARE @BranchIDList varchar(MAX) = 'BYAI,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI,SUKS,BBON,DONM,TSIT,ONUT,NAWA,LKAB,PINK,TPLU,NMIN,RMA2,BKEN,NAIN,BANA,SMUT,TEPA,BSTO,TKRU,NKAM,BPEE,TNPT,TUPM,TTAI,KBAN,EKKA,PANT,SAMK,NJOK,SLYA,MPTN,TYA3,TAIT,MINB,CHC4,PTNK,BROM,NLCH,KVIL,LPDU,SNBN,CWNA,PS43,TAC4,PBSK,SNDA,BSAE,SNOI,LAMB,BANB,LKAE,SUAS,ROMK,TYA6,BAPU,MCCS,AMTA,NKCS,KSWA,PTYA,BWIN,PYSC,MNKP,POKW,LUK2,SAP2,PYSL,PWET,BTEC,PNAM,ONTC,TTLY,FPAK,SEC2,SATU,SAI4,LAK4,SKMT,BAKY'
DECLARE @Month int = 4
DECLARE @Year int = 2017


SET @BranchIDList = 'BYAI'--,RSIT,MTNG,KKAW,MAHA,PKED,SCON,HPPY,BKAE,BBUA,TNON,SMAI'
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
		--============================================================

		SELECT	 a.BranchID
				,CASE WHEN a.PackageID = 'PKG07' THEN Quantity ELSE 0 END AS 'QuantityMini'
				,CASE WHEN a.PackageID = 'PKG07' THEN Amount ELSE 0 END AS 'AmountMini'
				,CASE WHEN a.PackageID = 'PKG03' THEN Quantity ELSE 0 END AS 'QuantityS'
				,CASE WHEN a.PackageID = 'PKG03' THEN Amount ELSE 0 END AS 'AmountS'
				,CASE WHEN a.PackageID = 'PKG08' THEN Quantity ELSE 0 END AS 'QuantitySPlus'
				,CASE WHEN a.PackageID = 'PKG08' THEN Amount ELSE 0 END AS 'AmountSPlus'
				,CASE WHEN a.PackageID = 'PKG04' THEN Quantity ELSE 0 END AS 'QuantityM'
				,CASE WHEN a.PackageID = 'PKG04' THEN Amount ELSE 0 END AS 'AmountM'
				,CASE WHEN a.PackageID = 'PKG18' THEN Quantity ELSE 0 END AS 'QuantityMPlus'
				,CASE WHEN a.PackageID = 'PKG18' THEN Amount ELSE 0 END AS 'AmountMPlus'
				,CASE WHEN a.PackageID = 'PKG05' THEN Quantity ELSE 0 END AS 'QuantityL'
				,CASE WHEN a.PackageID = 'PKG05' THEN Amount ELSE 0 END AS 'AmountL'
				
				,CASE WHEN a.PackageID = 'PKG54' THEN Quantity ELSE 0 END AS 'QuantityOrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG55' THEN Quantity ELSE 0 END AS 'QuantityOrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG56' THEN Quantity ELSE 0 END AS 'QuantityOrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG57' THEN Quantity ELSE 0 END AS 'QuantityOrangeTShirts'
				
				,CASE WHEN a.PackageID = 'PKG54' THEN Amount ELSE 0 END AS 'OrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG55' THEN Amount ELSE 0 END AS 'OrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG56' THEN Amount ELSE 0 END AS 'OrangeTShirts'
				,CASE WHEN a.PackageID = 'PKG57' THEN Amount ELSE 0 END AS 'OrangeTShirts'
		FROM (
			SELECT	 h.order_no AS OrderNo
					,h.branch_id AS BranchID
					,h.[status] AS [Status]
					,CONVERT(varchar,h.created_date,5) + ' ' + CONVERT(varchar(5),h.created_date,114) AS OrderDate
					,p.PackageID
					,p.PackageDesc
					,p.unit AS UnitText
					,i.qty
					,i.unit_price
					,ISNULL(i.packing_qty,0) AS Quantity
					,ISNULL(i.packing_qty * i.unit_price,0) AS Amount
			FROM tb_stock_order_item AS i 
				INNER JOIN tb_stock_order_head AS h with(nolock) ON i.order_no = h.order_no
				LEFT JOIN tb_Package AS p with(nolock) ON i.package_id = p.PackageID
			WHERE (h.branch_id in (SELECT b_id FROM @TbBranch))
				--AND (i.package_id in ('PKG07', 'PKG03', 'PKG08', 'PKG04', 'PKG18', 'PKG05'))
				AND (CONVERT(VARCHAR(6),h.delivery_date,112) = (@Year*100 + @Month))
		) AS a



		--SELECT	 b.ERP_ID AS ERPID
		--		,b.BranchID
		--FROM tb_Branch AS b
		--WHERE b.BranchID IN (SELECT b_id FROM @TbBranch)

		--SELECT	b.b_name
		--		,b.display_sequence
		--		,SUM(b.MiniQuantity) AS 'BoxMiniQuantity'
		--		,SUM(b.MiniAmount) AS 'BoxMiniAmount'
		--		,SUM(b.SQuantity) AS'BoxSQuantity'
		--		,SUM(b.SAmount) AS'BoxSAmount'
		--		,SUM(b.SPlusQuantity) AS 'BoxSPlusQuantity'
		--		,SUM(b.SPlusAmount) AS 'BoxSPlusAmount'
		--		,SUM(b.MQuantity) AS 'BoxMQuantity'
		--		,SUM(b.MAmount) AS 'BoxMAmount'
		--		,SUM(b.MPlusQuantity) AS 'BoxMPlusQuantity'
		--		,SUM(b.MPlusAmount) AS 'BoxMPlusAmount'
		--		,SUM(b.LQuantity) AS 'BoxLQuantity'
		--		,SUM(b.LAmount) AS 'BoxAmountL'
		--		,SUM(b.MiniAmount + b.SAmount + b.SPlusAmount + B.MAmount + B.MPlusAmount + B.LAmount ) AS 'Total'
		--FROM (
		--		SELECT a.b_name
		--			,CASE WHEN a.p_id = 'PKG07' THEN Quantity ELSE 0 END AS 'MiniQuantity'
		--			,CASE WHEN a.p_id = 'PKG07' THEN amt ELSE 0 END AS 'MiniAmount'
		--			,CASE WHEN a.p_id = 'PKG03' THEN Quantity ELSE 0 END AS 'SQuantity'
		--			,CASE WHEN a.p_id = 'PKG03' THEN amt ELSE 0 END AS 'SAmount'
		--			,CASE WHEN a.p_id = 'PKG08' THEN Quantity ELSE 0 END AS 'SPlusQuantity'
		--			,CASE WHEN a.p_id = 'PKG08' THEN amt ELSE 0 END AS 'SPlusAmount'
		--			,CASE WHEN a.p_id = 'PKG04' THEN Quantity ELSE 0 END AS 'MQuantity'
		--			,CASE WHEN a.p_id = 'PKG04' THEN amt ELSE 0 END AS 'MAmount'
		--			,CASE WHEN a.p_id = 'PKG18' THEN Quantity ELSE 0 END AS 'MPlusQuantity'
		--			,CASE WHEN a.p_id = 'PKG18' THEN amt ELSE 0 END AS 'MPlusAmount'
		--			,CASE WHEN a.p_id = 'PKG05' THEN Quantity ELSE 0 END AS 'LQuantity'
		--			,CASE WHEN a.p_id = 'PKG05' THEN amt ELSE 0 END AS 'LAmount'
		--			,a.display_sequence
		--		FROM (
		--				SELECT	h.order_no AS ord_no,
		--						h.branch_id AS b_name,
		--						h.[status] AS 'status',
		--						CONVERT(varchar,h.created_date,5) + ' ' + CONVERT(varchar(5),h.created_date,114) AS ord_date,
		--						p.PackageID p_id,
		--						p.PackageDesc AS p_desc,
		--						p.unit AS unit,
		--						i.qty AS ord_qty,
		--						i.unit_price AS unit_price,
		--						ISNULL(i.packing_qty,0) AS Quantity,
		--						ISNULL(i.packing_qty * i.unit_price,0) AS amt,
		--						b.display_sequence
		--				FROM tb_stock_order_item AS i 
		--					INNER JOIN tb_stock_order_head AS h with(nolock) ON i.order_no = h.order_no
		--					LEFT JOIN tb_Package AS p with(nolock) ON i.package_id = p.PackageID
		--					INNER JOIN tb_Branch AS b with(nolock) ON b.BranchId = h.branch_id
		--				WHERE (h.branch_id IN (SELECT BranchID FROM tb_Branch WHERE branch_type = 'FC-SHOP' ))
		--					AND h.branch_id = 'BYAI'
		--					AND (i.package_id IN('PKG07', 'PKG03', 'PKG08', 'PKG04', 'PKG18', 'PKG05'))
		--					AND (convert(varchar(8),h.created_date,112) BETWEEN '20170301' AND '20170331' )
		--					AND (h.[status] = 4 )
		--					AND PackageType =1
		--			) AS a
		--	) b
		--GROUP BY b.b_name, b.display_sequence
		--ORDER BY b.display_sequence