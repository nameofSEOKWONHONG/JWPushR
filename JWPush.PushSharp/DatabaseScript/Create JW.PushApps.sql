USE [ZIKGONG]
GO

/****** 개체: Table [JW].[PushApps] 스크립트 날짜: 2016-12-26 오후 5:52:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [JW].[PushApps] (
    [PushNo]                              INT             IDENTITY (1, 1) NOT NULL,
    [ManageName]                          NVARCHAR (50)   NOT NULL,
    [iOSUseYn]                            CHAR (1)        NULL,
    [iOSCert]                             VARBINARY (MAX) NULL,
    [iOSCertPass]                         NVARCHAR (50)   NULL,
    [GcmUseYn]                            CHAR (1)        NULL,
    [GcmSenderId]                         NVARCHAR (50)   NULL,
    [GcmAuthToken]                        NVARCHAR (50)   NULL,
    [GcmOptionalApplicationIdPackageName] NVARCHAR (50)   NULL,
    [WnsUseYn]                            CHAR (1)        NULL,
    [WnsPackageName]                      NVARCHAR (50)   NULL,
    [WnsPackageSid]                       NVARCHAR (50)   NULL,
    [WnsClientSecret]                     NVARCHAR (50)   NULL,
    [EditId]                              NVARCHAR (50)   NOT NULL,
    [EditDtm]                             DATETIME        NOT NULL
);


