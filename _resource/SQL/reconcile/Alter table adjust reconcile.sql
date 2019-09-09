CREATE TABLE [dbo].[tb_reconcile_adjust_master](
	[adjust_id] [varchar](3) NOT NULL,
	[adjust_desc] [varchar](100) NOT NULL,
	[is_active][bit] NULL,
	[created_by] [varchar](50) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](50) NULL,
	[updated_date] [datetime] NULL
 CONSTRAINT [PK_reconcile_adjust_master] PRIMARY KEY CLUSTERED 
(
	[adjust_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tb_reconcile_adjust_transaction]
(
	[branch_id] [varchar](10) NOT NULL,
	[report_date] [date] NOT NULL,
	[type_id] [varchar](100) NOT NULL,
	[adjust_id] [varchar](3) NOT NULL,
	[amount] [money] not null,
	[created_by] [varchar](50) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](50) NULL,
	[updated_date] [datetime] NULL
 CONSTRAINT [PK_tb_reconcile_adjust_transaction] PRIMARY KEY CLUSTERED 
(
	[branch_id],[report_date],[type_id],[adjust_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tb_reconcile_adjust_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_adjust_transaction_adjust_id] FOREIGN KEY([adjust_id])
REFERENCES [dbo].[tb_reconcile_adjust_master] ([adjust_id])


