USE [POS]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_master]    Script Date: 1/16/2019 6:17:56 PM ******/
DROP TABLE [dbo].[tb_reconcile_summary_master]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_history]    Script Date: 1/16/2019 6:17:56 PM ******/
DROP TABLE [dbo].[tb_reconcile_summary_history]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_history]    Script Date: 1/16/2019 6:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_summary_history](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[branch_id] [varchar](10) NOT NULL,
	[report_date] [date] NOT NULL,
	[type_id] [varchar](10) NOT NULL,
	[amount] [money] NOT NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[verified_flag] [bit] NULL,
	[verified_by] [varchar](20) NULL,
	[verified_date] [datetime] NULL,
	[confirm_flag] [bit] NULL,
	[confirm_by] [varchar](20) NULL,
	[confirm_date] [datetime] NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_summary_history] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_master]    Script Date: 1/16/2019 6:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_summary_master](
	[branch_id] [varchar](10) NOT NULL,
	[report_date] [date] NOT NULL,
	[type_id] [varchar](10) NOT NULL,
	[amount] [money] NOT NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[verified_flag] [bit] NULL,
	[verified_date] [datetime] NULL,
	[verified_by] [varchar](20) NULL,
	[confirm_flag] [bit] NULL,
	[confirm_date] [datetime] NULL,
	[confirm_by] [varchar](20) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_summary_master] PRIMARY KEY CLUSTERED 
(
	[branch_id] ASC,
	[report_date] ASC,
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[tb_reconcile_payment_type] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue] FOREIGN KEY([branch_id], [report_date])
REFERENCES [dbo].[tb_daily_revenue] ([BranchID], [ReportDate])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[tb_reconcile_payment_type] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type]
GO
