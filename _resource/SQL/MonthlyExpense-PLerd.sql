DECLARE @Year int,
@Month int;

SET @Year = 2017;
SET @Month = 4;

--- Parameter ---
DECLARE @tbBranch TABLE (BranchID varchar(20));

-- 1. Management Fee
	DECLARE @LastMonth datetime;
	SET @LastMonth = DATEADD(day,-1,convert(datetime,CAST(@Year*10000 + @Month*100 + 1 as varchar)));

	DELETE @tbBranch;
	INSERT INTO @tbBranch (BranchID)
	SELECT b.BranchID
	FROM tb_Branch as b with (nolock)
	WHERE b.branch_type = 'FC-SHOP'
	and b.BranchID not in (
		SELECT mc.branch_id
		FROM tb_monthly_commission  as mc with (nolock)
		WHERE mc.[Year] = @Year
			and mc.[Month] = @Month
			and mc.fee_management_verify_date is not null
	)
	
	DELETE tb_fee_monthly_transaction
	WHERE branch_id in (SELECT BranchID FROM @tbBranch)
		and [year] = @Year
		and [month] = @Month
		and category_id = 1;

	INSERT INTO tb_fee_monthly_transaction
		   ([branch_id]
		  ,[month]
		  ,[year]
		  ,[item_id]
		  ,[category_id]
		  ,[item_amount]	-- qty
		  ,[item_expense]	-- amount
		  )
	SELECT ft.branch_id,
		@Month as [Month],
		@Year as [Year],
		ft.item_id,
		ft.category_id,
		ft.item_amount,
		ft.item_expense
	FROM tb_fee_monthly_transaction as ft (nolock)
	WHERE ft.category_id = 1
	and ft.item_id = 1
	and cast(ft.[year]*100 + ft.[month] as varchar) = convert(varchar(6),@LastMonth,112)
	and ft.branch_id in (SELECT BranchID FROM @tbBranch);

-- 2. Service Supply
	DELETE @tbBranch;
	INSERT INTO @tbBranch (BranchID)
	SELECT b.BranchID
	FROM tb_Branch as b with (nolock)
	WHERE b.branch_type = 'FC-SHOP'
	and b.BranchID not in (
		SELECT mc.branch_id
		FROM tb_monthly_commission  as mc with (nolock)
		WHERE mc.[Year] = @Year
			and mc.[Month] = @Month
			and mc.fee_supply_verify_date is not null
	)

	-- DELETE Existing Record for not yet verify
	DELETE tb_fee_monthly_transaction
	WHERE branch_id in (SELECT BranchID FROM @tbBranch)
		and [Year] = @Year
		and [Month] = @Month
		and category_id = 3 -- Supply

	-- Not yet verify Sale Package
	INSERT INTO tb_fee_monthly_transaction
		   ([branch_id]
		  ,[month]
		  ,[year]
		  ,[item_id]
		  ,[category_id]
		  ,[item_amount]	-- qty
		  ,[item_expense]	-- amount
		  )
	SELECT h.branch_id,
		@Month as [Month],
		@Year as [Year],
		fee.item_id,
		fee.category_id,
		SUM(i.packing_qty) as qty,
		SUM(ISNULL(i.packing_qty,0) * i.unit_price) as amt
     FROM tb_stock_order_item as i 
        INNER JOIN tb_stock_order_head as h 
		ON i.order_no = h.order_no
		INNER JOIN tb_Package as p with (nolock)
		on i.package_id = p.PackageID
		INNER JOIN
			(
				SELECT fi.item_desc as package_id,
					max(fi.item_id) as item_id,
					max(fi.category_id) as category_id
				FROM tb_fee_items as fi (nolock)
				WHERE fi.category_id = 3 -- Supply
				GROUP BY fi.item_desc
			) as fee
		ON i.package_id = fee.package_id
     where (h.branch_id in (SELECT BranchID FROM @tbBranch))
       and (convert(varchar(6),h.delivery_date,112) = (@Year*100 + @Month))
	   and p.PackageType in (4,5)
	GROUP BY h.branch_id,
		fee.item_id,
		fee.category_id,
		i.package_id

-- 3. Sale Package
	DELETE @tbBranch;
	INSERT INTO @tbBranch (BranchID)
	SELECT b.BranchID
	FROM tb_Branch as b with (nolock)
	WHERE b.branch_type = 'FC-SHOP'
	and b.BranchID not in (
		SELECT mc.branch_id
		FROM tb_monthly_commission  as mc with (nolock)
		WHERE mc.[Year] = @Year
			and mc.[Month] = @Month
			and mc.sales_package_verify_date is not null
	)

	-- DELETE Existing Record for not yet verify
	DELETE tb_fee_monthly_transaction
	WHERE branch_id in (SELECT BranchID FROM @tbBranch)
		and [Year] = @Year
		and [Month] = @Month
		and category_id = 5 -- Sale Package

	-- Not yet verify Sale Package
	INSERT INTO tb_fee_monthly_transaction
		   ([branch_id]
		  ,[month]
		  ,[year]
		  ,[item_id]
		  ,[category_id]
		  ,[item_amount]	-- qty
		  ,[item_expense]	-- amount
		  )
	SELECT h.branch_id,
		@Month as [Month],
		@Year as [Year],
		fee.item_id,
		fee.category_id,
		SUM(i.packing_qty) as qty,
		SUM(ISNULL(i.packing_qty,0) * i.unit_price) as amt
     FROM tb_stock_order_item as i 
        INNER JOIN tb_stock_order_head as h 
		ON i.order_no = h.order_no
		INNER JOIN
			(
				SELECT fi.item_desc as package_id,
					max(fi.item_id) as item_id,
					max(fi.category_id) as category_id
				FROM tb_fee_items as fi (nolock)
				WHERE fi.item_desc in ('PKG07', 'PKG03', 'PKG08', 'PKG04', 'PKG18', 'PKG05')
				GROUP BY fi.item_desc
			) as fee
		ON i.package_id = fee.package_id
     where (h.branch_id in (SELECT BranchID FROM @tbBranch))
       and (i.package_id in ('PKG07', 'PKG03', 'PKG08', 'PKG04', 'PKG18', 'PKG05'))
       and (convert(varchar(6),h.delivery_date,112) = (@Year*100 + @Month))
	GROUP BY h.branch_id,
		fee.item_id,
		fee.category_id,
		i.package_id