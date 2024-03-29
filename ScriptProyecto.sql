USE [Gr03Proy2]
GO
/****** Object:  User [Gr03Proy2]    Script Date: 11/22/2019 2:26:01 PM ******/
CREATE USER [Gr03Proy2] FOR LOGIN [Gr03Proy2] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Gr03Proy2]
GO
/****** Object:  UserDefinedFunction [dbo].[ObtenerProyectoActualDelEmpleado]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ObtenerProyectoActualDelEmpleado](@idEmpleado varchar(20))
RETURNS varchar(30)AsBeginDECLARE @nombreProyecto nvarchar(30);Select @nombreProyecto = P.nombre 
From Empleado E inner join TrabajaEn TE on TE.idEmpleadoFK = E.idEmpleadoPK
inner join Proyecto P on TE.idProyectoFK = P.idProyectoAID
where E.idEmpleadoPK = @idEmpleado and TE.estado = 'Activo'
Return @nombreProyecto
End;




GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[cedulaPK] [varchar](20) NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[apellido1] [varchar](30) NOT NULL,
	[apellido2] [varchar](30) NULL,
	[empresa] [varchar](30) NOT NULL,
	[provincia] [varchar](30) NOT NULL,
	[canton] [varchar](30) NOT NULL,
	[distrito] [varchar](30) NOT NULL,
	[direccionExacta] [varchar](300) NULL,
	[telefono] [varchar](15) NULL,
	[correo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[cedulaPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[idEmpleadoPK] [varchar](20) NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[apellido1] [varchar](30) NOT NULL,
	[apellido2] [varchar](30) NULL,
	[correo] [varchar](30) NOT NULL,
	[telefono] [varchar](15) NULL,
	[fechaNacimiento] [date] NOT NULL,
	[distrito] [varchar](30) NOT NULL,
	[canton] [varchar](30) NOT NULL,
	[provincia] [varchar](30) NOT NULL,
	[direccion] [varchar](30) NULL,
	[estado] [varchar](30) NOT NULL,
	[tipoTrabajo] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[idEmpleadoPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HabilidadBlanda]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HabilidadBlanda](
	[idEmpleadoFK] [varchar](20) NOT NULL,
	[habilidad] [varchar](30) NOT NULL,
 CONSTRAINT [PK_HabilidadBlanda] PRIMARY KEY CLUSTERED 
(
	[idEmpleadoFK] ASC,
	[habilidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HabilidadTecnica]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HabilidadTecnica](
	[idEmpleadoFK] [varchar](20) NOT NULL,
	[habilidad] [varchar](30) NOT NULL,
 CONSTRAINT [PK_HabilidadTecnica] PRIMARY KEY CLUSTERED 
(
	[idEmpleadoFK] ASC,
	[habilidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialReqTester]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialReqTester](
	[idProyectoFK] [int] NOT NULL,
	[idReqFK] [int] NOT NULL,
	[idEmpleadoFK] [varchar](20) NOT NULL,
	[horas] [int] NULL,
	[fechaInicio] [datetime] NOT NULL,
	[fechaFin] [datetime] NULL,
	[estado] [varchar](20) NOT NULL,
 CONSTRAINT [PK_HistorialReqTester] PRIMARY KEY CLUSTERED 
(
	[idProyectoFK] ASC,
	[idReqFK] ASC,
	[idEmpleadoFK] ASC,
	[fechaInicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyecto]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyecto](
	[idProyectoAID] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[objetivo] [varchar](200) NULL,
	[estado] [varchar](30) NOT NULL,
	[duracionReal] [int] NOT NULL,
	[duracionEstimada] [int] NOT NULL,
	[fechaInicio] [date] NOT NULL,
	[fechaFinalizacion] [date] NOT NULL,
	[cedulaClienteFK] [varchar](20) NULL,
	[cantidadReq] [int] NOT NULL,
 CONSTRAINT [PK_Proyecto] PRIMARY KEY CLUSTERED 
(
	[idProyectoAID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prueba]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prueba](
	[idProyectoFK] [int] NOT NULL,
	[idReqFK] [int] NOT NULL,
	[idPruebaPK] [int] NOT NULL,
	[nombre] [varchar](60) NOT NULL,
	[resultadoFinal] [varchar](60) NOT NULL,
	[propositoPrueba] [varchar](60) NULL,
	[entradaDatos] [varchar](200) NULL,
	[resultadoEsperado] [varchar](200) NULL,
	[estado] [varchar](60) NOT NULL,
	[imagen] [varbinary](max) NULL,
	[descripcionError] [varchar](200) NULL,
	[flujoPrueba] [varchar](200) NULL,
 CONSTRAINT [PK_Prueba] PRIMARY KEY CLUSTERED 
(
	[idProyectoFK] ASC,
	[idReqFK] ASC,
	[idPruebaPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requerimiento]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requerimiento](
	[idReqPK] [int] NOT NULL,
	[idProyectoFK] [int] NOT NULL,
	[cedulaTesterFK] [varchar](20) NULL,
	[nombre] [varchar](40) NOT NULL,
	[complejidad] [varchar](20) NOT NULL,
	[tiempoEstimado] [int] NOT NULL,
	[tiempoReal] [int] NULL,
	[descripcion] [varchar](100) NULL,
	[fechaDeInicio] [date] NOT NULL,
	[fechaDeFinalizacion] [date] NULL,
	[estado] [varchar](30) NOT NULL,
	[resultado] [bit] NULL,
	[detallesResultado] [varchar](250) NULL,
	[estadoResultado] [varchar](30) NULL,
	[cantidadDePruebas] [int] NOT NULL,
 CONSTRAINT [PK_Requerimiento] PRIMARY KEY CLUSTERED 
(
	[idReqPK] ASC,
	[idProyectoFK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tester]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tester](
	[idEmpleadoFK] [varchar](20) NOT NULL,
	[cantidadRequerimientos] [int] NOT NULL,
 CONSTRAINT [PK_Tester] PRIMARY KEY CLUSTERED 
(
	[idEmpleadoFK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrabajaEn]    Script Date: 11/22/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrabajaEn](
	[idProyectoFK] [int] NOT NULL,
	[idEmpleadoFK] [varchar](20) NOT NULL,
	[rol] [varchar](30) NOT NULL,
	[estado] [varchar](30) NOT NULL,
 CONSTRAINT [PK_TrabajaEn] PRIMARY KEY CLUSTERED 
(
	[idProyectoFK] ASC,
	[idEmpleadoFK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201909272143398_InitialCreate', N'ProyectoIntegrador.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5CDB6EDC36107D2FD07F10F4D416CECA9726488DDD14CEDA6E8DC617649DA06F0157E2AE85489422518E8D225FD6877E527FA14389BAF1A2CBAEBCBB2E02042B727866381C92C3E1D0FFFEFDCFF8D707DF33EE7114BB01999807A37DD3C0C40E1C972C276642172F5E9BBFBEF9FEBBF199E33F181F73BA2346072D493C31EF280D8F2D2BB6EFB08FE291EFDA5110070B3AB203DF424E601DEEEFFF621D1C5818204CC0328CF1FB8450D7C7E9077C4E0362E39026C8BB0C1CECC5BC1C6A6629AA71857C1C87C8C613F3260A1EB14D830B42F13202FC689435328D13CF4520D00C7B0BD3408404145110F7F8438C67340AC872164201F26E1F430C740BE4C59877E3B824EFDAA3FD43D623AB6C9843D9494C03BF27E0C111579125365F49D166A14250E219289B3EB25EA78A9C98170E4E8BDE071E284064783CF522463C312F0B1627717885E9286F38CA20CF2380FB1A449F4755C43DA373BBBDC2A40E47FBECDF9E314D3C9A447842704223E4ED1937C9DC73ED3FF0E36DF01993C9D1C17C71F4FAE52BE41CBDFA191FBDACF614FA0A74B50228028B097104B2E145D17FD3B0EAED2CB161D1ACD226D30AD812CC0ED3B8440FEF3059D23B983787AF4DE3DC7DC04E5EC28DEB0371613241231A25F07995781E9A7BB8A8B71A79B2FF1BB81EBE7C3508D72B74EF2ED3A117F8C3C489605EBDC75E5A1BDFB96136BD6AE3FD89939D4781CFBEEBF695D57E9A054964B3CE045A925B142D31AD4B37B64AE3ED64D20C6A78B3CE5177DFB499A4B2792B495987569909398B4DCF865CDEA7E5DBD9E24EC210062F352DA6912683D3EE592301044C42222D0DE9A0AB2111E8E0FF795D3CF391EB0DB03076E002AEC9C28D7C5CF4F26D006688486F996F501CC3BAE0FC8EE2BB06D1E1E700A2CFB09D4460AE338AFCF0C9B9DDDC05045F25FE9CCD82CDF11A6C686EBF06E708E65C744658ABB5F1DE05F6E720A167C43945147FA0760EC83E6F5DBF3BC020E29CD8368EE3733066EC4C03F0BC734058638E0E7BC3B1756ADB8EC9D443AEAFF64C8415F5534E5A7A276A0AC943D190A9BC942651DF054B9774133527D58B9A51B48ACAC9FA8ACAC0BA49CA29F582A604AD72665483F97DE9080DEFF8A5B0BBEFF9ADB779EBD6828A1A67B042E2DF30C1112C63CE0DA21447A41C812EEBC6369C8574F818D327DF9B524E1F91970CCD6AA5D9902E02C3CF861476F767432A2614DFBB0EF34A3A1C87726280EF44AF3E69B5CF3941B24D4F875A3737CD7C336B806EBA9CC47160BBE92C5004C27818A32E3FF870467B4C23EB8D1817818E81A1BB6CCB8312E89B291AD53539C51EA6D838B1B340E114C53672643542879C1E82E53BAA42B0323E5217EE278927583A8E5823C40E4131CC549750795AB8C47643E4B56A4968D9710B637D2F788835A738C484316CD54417E6EA700813A0E0230C4A9B86C656C5E29A0D51E3B5EAC6BCCD852DC75D8A526CC4265B7C678D5D72FFED490CB359631B30CE66957411401BDADB8681F2B34A5703100F2EBB66A0C2894963A0DCA5DA8881D635B60503ADABE4D919687644ED3AFEC27975D7CCB37E50DEFCB6DEA8AE2DD8664D1F3B669A99EF096D28B4C0916C9EA77356891FA8E2700672F2F359CC5D5DD14418F80CD37AC8A6F477957EA8D50C221A51136069682DA0FC5250029226540FE1F2585EA374DC8BE8019BC7DD1A61F9DA2FC0566C40C6AE5E8E5608F557A8A271763A7D143D2BAC4132F24E87850A8EC220C4C5ABDEF10E4AD1C56565C574F185FB78C3958EF1C16850508BE7AA5152DE99C1B5949B66BB96540E591F976C2D2D09EE93464B796706D712B7D17625299C821E6EC15A2AAA6FE1034DB63CD251EC3645DDD8CA52A778C1D8D2E4588D2F5118BA6459C9B9E225C62C4BB89ABE98F54F41F2330CCB8E15994885B405271A446889855A600D929EBB514C4F114573C4E23C53C797C8947BAB66F9CF5956B74F7910F37D20A766BF8BF099E62ABFB6E5CA3E09873A878EFACCB149A3E90A33503737582A1CF250A408E04F032FF189DECFD2B7CEAEF1AAEDB31219616C09F24B7E94A434C9DBAD8F40A7F191E7C6B063557833AB8F971E42A7F5DC17ADEA5DE79FEA51F27055154517C2DADAF8E9DC9A55C64C741CFB0F592BC2D3CC329EAD5205E0453D312A090F1258A5AE3B6A3D27A58A59AFE98E28249E542185AA1E5256D34B6A42562B56C2D368544DD19D839C505245976BBB232B524BAAD08AEA15B015328B75DD5115D9275560457577EC3215455C4B77781FD31E67D6DDC8B283EF7A3B9906E36916C66136C2CAFD7E15A852DC138BDFE04B60BC7C278D4A7BFA5BD7A8B2B0C77A46A5C1D0AF43B50BF2FA32D478ABAFC7ACDD7AD796FAA65B7F3D5E3FD37D520391CE802249C1BD380B0A67BE313F7FB53FBE910E64198969E46A846DFE31A6D81F3182D1EC8B37F55CCC16F59CE012117781639A657A9887FB0787C2C39DDD794463C5B1E329CEAFBA9734F531DB40D216B947917D87223985628D872625A8149DBE200E7E98987FA5AD8ED34007FB9516EF1917F107E27E49A0E2364AB0F14D4E091D26F1BEC3538F42D06FCFE20D4577955FFCF9296BBA675C47309D8E8D7D41D1AB0C7FFD65452F69B2A66B48B3F27B8BE73BDB6ACF1794A8C26C59FDB5C2DCA583BC54C8A5FCC1470F3FF6154DF91A612D44C58B83A1F00651A1EE45C12A58DAD7040E7CD2F43541BFCEAA5F17AC229AF665814BFA8389EF0ABA2F4379CB2DEE438A73D32696A454CFAD79D96B25696E7B6F92D2B7D79AE8728A760FB841D3B0D773519E597AF3605BA7227B7930EC6DDAFD93A72CEF4A9672E9B46F37397993F9C80DB74CFFAB34E41D489C5324026D3FD978D3B6A60B04EF78C666BF94E21D3336BECD6F3F7178D3C6A60B10EFB8B1F54A0FDE315BDBD6FEB9654BEBBC856E3DD957CE5BD25CE8A8A2C86DC9BC59C81D8EFFF3008C20F328B33798EAECB1A6CCD7168625899EA93E6D4D642C4D1C89AF44D1CCB65F5FF986DFD8594ED3CC5693ECD9C49BAFFF8DBC394D336F4D0AE536D29095498CAAD4F09675AC29A7EA39A51DD77AD292E5DEE6B336DECE3FA72CE34194529B3D9ADBE5E793543C884A869C3A3D9288E58B62D83B2B7FCB11F6EFD85D9610EC2F3B126CD776CD82E6822C827CF31624CA498408CD25A6C8812DF524A2EE02D914AA59003A7D449E06F5D835C81C3B17E43AA16142A1CBD89F7BB5801773029AF8A799D27599C7D721FB8A87E80288E9B2C0FD35799BB89E53C87DAE880969209877C1C3BD6C2C290BFB2E1F0BA4AB807404E2EA2B9CA25BEC871E80C5D76486EEF12AB281F9BDC34B643F9611401D48FB40D4D53E3E75D132427ECC31CAF6F00936ECF80F6FFE036252FA4FD2540000, N'6.2.0-61023')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4', N'Cliente')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Lider')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Soporte')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Tester')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cca2e99e-28a3-4731-b452-4beead054522', N'1')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'096b1b0e-1044-4163-8f5b-6aa0bc0fa8c5', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'961d6ba4-46e8-4c0d-9081-672e24fe3987', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b494cacf-7b39-46ef-bbd7-0300b0e7a06d', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c2a59dfd-40b2-403f-b3d4-57cd46fba539', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1a93ba09-f400-4409-812d-4e61e1416608', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'23421471-80d8-4db2-b441-8c276090825b', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'45f5dc76-4d50-41a5-9230-7140e2bcef84', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'567d299d-9a4e-41fb-a197-543a036f9e53', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6c318b0e-48d1-4ab2-9f5d-e73d9d18503b', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6e477ca9-92c9-43de-b932-52dd68139380', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'87235377-3873-49af-acc4-5f8fac3e5ee1', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a46c728c-acc9-4c78-8b80-a0ab472d14ff', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c007ccb2-6fef-4d91-809c-b37e855375a8', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'21b0c691-8503-482e-b4e0-cf982abd6d52', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'21d683f5-a694-4f8e-8057-51a3306bc7fa', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'35d75fe8-6fbe-4b08-9a97-a7b1806baca0', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ddb525b0-7c88-4e52-b1cb-f047ff222899', N'4')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'096b1b0e-1044-4163-8f5b-6aa0bc0fa8c5', N'dieg0cr98@gmail.com', 0, N'AJGBPSFYtsarNNkhrkiqJvQFOgf9vtLczsDxySws0JHRbAGWZuwRPxvCjmKqDPsrcw==', N'21db9c56-f9f1-4459-a625-a6efa0c02cd4', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@gmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'1a93ba09-f400-4409-812d-4e61e1416608', N'dieg0cr98@tester2.com', 0, N'AFDSxcZCSZtK04mWvs76bZ8+uwu6UNB+tzGgMIJh6m5XmTWKqcLuy7NlWwkzsGXp/A==', N'439a9041-8318-4b9d-baa9-6f0dfba33b14', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@tester2.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'21b0c691-8503-482e-b4e0-cf982abd6d52', N'consultasAvazadas@cliente.com', 0, N'AAayifRJWBqiFTvhIvj6d0WN96UCAc2R8lxBUdjehHFPQgP8y394DrJCUI/5yybvgw==', N'7ff45023-8ff5-430d-bf19-04b7e97f3a3b', NULL, 0, 0, NULL, 1, 0, N'consultasAvazadas@cliente.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'21d683f5-a694-4f8e-8057-51a3306bc7fa', N'jrcc@hotmail.com', 0, N'AHjeWFXVLtBMR5JOceRdL3PbAU5ZoaDd7yTgyG/uuvmTYe/9mjRgUW00HbsyXrPFhQ==', N'f7843cad-6709-4769-b3b4-cffff9124beb', NULL, 0, 0, NULL, 0, 0, N'jrcc@hotmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'23421471-80d8-4db2-b441-8c276090825b', N'dieg0cr98@tester1.com', 0, N'AMlq7xOWqaOYsYbpv060DU7FpnzRJA+YSIxqnJuJfLxgW/jiSzLAjChQLk91DiHC6Q==', N'806c9f7b-2536-435c-9cc5-4593739df6f9', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@tester1.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'35d75fe8-6fbe-4b08-9a97-a7b1806baca0', N'abenavidesc@hotmail.com', 0, N'ACvRyeFgytyCSptuEp/oXPTf/nCsdxYE5lw18DEPqv5veHO+WFlYVgL6xCNy8aBwMQ==', N'b59ed595-5f92-40a2-8138-b91a82145adc', NULL, 0, 0, NULL, 0, 0, N'abenavidesc@hotmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'45f5dc76-4d50-41a5-9230-7140e2bcef84', N'georg@gmail.com', 0, N'AHs6u6W4RjOTo74c9uCqRLaBE6bbp0+GwVPNarKjkiNSwZBKysHjSSJtSQfVc256+w==', N'bb6aed1e-a702-4f9a-a04f-72e9f864e7d3', NULL, 0, 0, NULL, 0, 0, N'georg@gmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'567d299d-9a4e-41fb-a197-543a036f9e53', N'jorge.carboni@carma.com', 0, N'ACQvNG5g193HJl+u0P8cp0wOkrEioY6oiI6H0h1XkGgq2Qr+/LhVyXL/a25cH5e5ow==', N'15356550-e698-45de-b6a5-59ec17d44459', NULL, 0, 0, NULL, 0, 0, N'jorge.carboni@carma.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6c318b0e-48d1-4ab2-9f5d-e73d9d18503b', N'tester2@tester2.com', 0, N'AHlGp44oqYGW7KJGC/qCzdWb4TmDfzmvj8wnaV+kV8HVAjZrLSxQJ25H0OZ1nWuAlg==', N'8a24e9e8-e745-4e12-a80e-f03b204cb0bd', NULL, 0, 0, NULL, 1, 0, N'tester2@tester2.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6e477ca9-92c9-43de-b932-52dd68139380', N'dieg0cr98@tester5.com', 0, N'ANATugBQehOkXm62aWVfn1MHWCbNGR/+b5aEkX4v/KVyvW5IsruyeSYMP/TR/sekVA==', N'd0ac40cb-4984-4dd9-a682-6dc4a0d31dfa', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@tester5.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'87235377-3873-49af-acc4-5f8fac3e5ee1', N'manuel@gmail.com', 0, N'AKuRPoo28b81/eD1dLEu5ZD76R4EnxKBXskoHg7kpBMLhCTHtXXAeclDGMn53qNtiA==', N'f78b5470-ab11-413e-9876-e690f06b489b', NULL, 0, 0, NULL, 0, 0, N'manuel@gmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'961d6ba4-46e8-4c0d-9081-672e24fe3987', N'javier@javier.com', 0, N'AGHjeqfngMn2LB7hcFXwEUvVsmV58E29L9gmq0jdPZjfWdnU5ABmpZZVjZxMVIqbyQ==', N'18ee2a23-0ab0-4693-be24-1051fb2cc95e', NULL, 0, 0, NULL, 1, 0, N'javier@javier.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'a46c728c-acc9-4c78-8b80-a0ab472d14ff', N'dieg0cr98@tester4.com', 0, N'ABaXEmXOJ/4ORjGTX5lKGbs8grsnG1Zyn8E7VzFzYyTx0xVg3a+kY64QZRKrMJ5dmg==', N'19923115-5ef3-4f72-b879-37d3e19af9f5', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@tester4.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b494cacf-7b39-46ef-bbd7-0300b0e7a06d', N'lider@lider.com', 0, N'ACz0XCVqWc5/pe5uZrlR8PHA6Il26KB7SDhh+cqoyiHejt4X0tqR+dXPX0nkwZfirQ==', N'63d235f0-a93f-4a0c-8d06-8b10d488db71', NULL, 0, 0, NULL, 1, 0, N'lider@lider.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'c007ccb2-6fef-4d91-809c-b37e855375a8', N'dieg0cr98@tester3.com', 0, N'ACVgKqnbSv1mqP5oVuW4klWHAVjccm36+AgeSaHys+rxEStH4QZhCqqnm5N07huAJQ==', N'44acb95f-9c24-4473-92d3-f24258715352', NULL, 0, 0, NULL, 1, 0, N'dieg0cr98@tester3.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'c2a59dfd-40b2-403f-b3d4-57cd46fba539', N'liderlibre@lider.com', 0, N'ANGhw0rboEpXjuSvPAaMB+Bn1wFIYzBPcm0AUrRReyzLDocmFHA0geKUuXb0Zy/QiA==', N'a0a0a749-43bc-4354-84a6-5005f71c4219', NULL, 0, 0, NULL, 1, 0, N'liderlibre@lider.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'cca2e99e-28a3-4731-b452-4beead054522', N'admin@admin.com', 0, N'AFV4jyFQZC8daBaILWEipps9Cjh8GwQSU0+1dFsk4bLrEN/ay1/9UTOapBK5UXTXHw==', N'3d68dc6a-be18-4aeb-987a-cc28c3606247', NULL, 0, 0, NULL, 1, 0, N'admin@admin.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'ddb525b0-7c88-4e52-b1cb-f047ff222899', N'gabriela.salazar@ucr.ac.cr', 0, N'AFPezsAMuus56tH5ioIcVG2Ru6H6DvKrcvjUXoq0sgKZQ2Hzq/Wl91gQ6xc4pCINmQ==', N'8eb77970-f0c4-42f8-a820-453b542726ee', NULL, 0, 0, NULL, 0, 0, N'gabriela.salazar@ucr.ac.cr')
INSERT [dbo].[Cliente] ([cedulaPK], [nombre], [apellido1], [apellido2], [empresa], [provincia], [canton], [distrito], [direccionExacta], [telefono], [correo]) VALUES (N'11223344', N'Consultas', N'Avanzadas', N'Cliente', N'Ucr', N'San Jose', N'San Jose', N'San Jose', N'ucr', N'12345678', N'consultasAvazadas@cliente.com')
INSERT [dbo].[Cliente] ([cedulaPK], [nombre], [apellido1], [apellido2], [empresa], [provincia], [canton], [distrito], [direccionExacta], [telefono], [correo]) VALUES (N'117240933', N'Cliente', N'Pruebas', N'Requerimientos', N'UCR', N'Cartago', N'La Unión', N'Tres Rios', N'Far far away', N'84238536', N'abenavidesc@hotmail.com')
INSERT [dbo].[Cliente] ([cedulaPK], [nombre], [apellido1], [apellido2], [empresa], [provincia], [canton], [distrito], [direccionExacta], [telefono], [correo]) VALUES (N'12345', N'Jose', N'A', N'C', N'ucr', N'San José', N'Pérez Zeledón', N'San Isidro De El General', N'a', N'123', N'jrcc@hotmail.com')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'1', N'Admin', N'Admin', NULL, N'admin@admin.com', NULL, CAST(N'1800-01-01' AS Date), N'San José', N'San José', N'San José', NULL, N'Ocupado', N'Jefe de Soporte')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'1026509874', N'Josefina', N'Pujol', N'Mesalles', N'josefina.pujol@ucr.ac.cr', N'', CAST(N'1880-01-01' AS Date), N'Carmen', N'Central', N'San José', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'106990375', N'Ricardo', N'Villalon', N'Fonseca', N'ricardo.villalon@ucr.ac.cr', N'', CAST(N'1967-04-20' AS Date), N'Carmen', N'Central', N'San José', N'Buena Vista', N'Disponible', N'Jefe de Soporte')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'109870354', N'Jorge', N'Carboni', N'Blanco', N'jorge.carboni@carma.com', N'', CAST(N'1967-09-29' AS Date), N'Carmen', N'Central', N'San José', N'', N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'116610331', N'Javier', N'Padilla', N'', N'javier@javier.com', N'', CAST(N'1969-11-27' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Ocupado', N'Lider')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010001', N'Alvaro ', N'Gascon', N'Cortina', N'dieg0cr98@tester1.com', NULL, CAST(N'1800-01-01' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010002', N'Aurora', N'Roma', N'Nicolas', N'dieg0cr98@tester2.com', NULL, CAST(N'1800-01-01' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010003', N'Hugo ', N'Valenzuela', N'Galisteo', N'dieg0cr98@tester3.com', NULL, CAST(N'1800-01-01' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010004', N'Isabel ', N'Aguera', N'Calzada', N'dieg0cr98@tester4.com', NULL, CAST(N'1800-01-01' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010005', N'Diego', N'Contreras', NULL, N'dieg0cr98@gmail.com', NULL, CAST(N'1998-01-28' AS Date), N'Salitral', N'Santa Ana', N'San José', NULL, N'Trabajando', N'Lider')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'117010006', N'Irene ', N'Pozuelo', N'Mansilla', N'dieg0cr98@tester5.com', NULL, CAST(N'1800-01-01' AS Date), N'Santa Ana', N'Santa Ana', N'San José', NULL, N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12321342342134', N'Steriel', N'ca', N'abvas', N'jrcc25@hotmail.com', N'3423123321123', CAST(N'2012-11-03' AS Date), N'Santo Tomás', N'Santo Domingo', N'Heredia', N'abc', N'Despedido', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345671', N'PrimerTester', N'Equipo', N'Equipo', N'tester1@tester.com', N'', CAST(N'1990-11-04' AS Date), N'La Asuncion', N'Belén', N'Heredia', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345672', N'SegundoTester', N'Equipo', N'Equipo', N'tester2@tester2.com', N'', CAST(N'1990-09-04' AS Date), N'Carmen', N'Central', N'San José', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345673', N'TercerTester', N'Equipo', N'Equipo', N'tester3@gmail.com', N'', CAST(N'1990-11-11' AS Date), N'Carmen', N'Central', N'San José', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345674', N'CuartoTester', N'Equipo', N'Equipo', N'tester4@tester.com', N'', CAST(N'1990-11-21' AS Date), N'Puntarenas', N'Central', N'Puntarenas', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345675', N'QuintoTester', N'Equipo', N'Equipo', N'tester5@tester.com', N'', CAST(N'1990-11-07' AS Date), N'Liberia', N'Liberia', N'Guanacaste', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345676', N'SextoTester', N'Equipo', N'Equipo', N'tester6@tester.com', N'', CAST(N'1990-11-01' AS Date), N'Carmen', N'Central', N'San José', N'', N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'12345678', N'Lider', N'Equipo', N'', N'lider@equipo.com', N'', CAST(N'1955-10-01' AS Date), N'San Pedro', N'Montes De Oca', N'San José', NULL, N'Disponible', N'Lider')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'1248448787', N'Sandra', N'Kikut', N'Valverde', N'kikut@kikut', N'22222222222', CAST(N'1950-05-02' AS Date), N'Carmen', N'Central', N'San José', N'', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'134259874', N'Ricardo', N'Tester', N'2', N'correo@requerimientos2.com', N'', CAST(N'1999-01-19' AS Date), N'Puerto Carrillo', N'Hojancha', N'Guanacaste', NULL, N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'203450966', N'Manuel', N'Rodriguez', N'Salazar', N'manuel@gmail.com', N'', CAST(N'1967-01-11' AS Date), N'San Rafael', N'La Unión', N'Cartago', N'', N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'207440918', N'Kendall', N'Lara', N'Castro', N'kendal.lara@gmail.com', N'88783288', CAST(N'1997-09-26' AS Date), N'Desamparados', N'Central', N'Alajuela', N'', N'Ocupado', N'Lider')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'207550918', N'Alexandra', N'Siles', N'Alvarado', N'alexandra.siles@ucr.ac.cr', N'86500751', CAST(N'1996-08-06' AS Date), N'Fortuna', N'Bagaces', N'Guanacaste', N'POR ahi', N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'209480473', N'George', N'Stockli', N'Jimenez', N'georg@gmail.com', N'', CAST(N'1957-01-01' AS Date), N'Carmen', N'Central', N'San José', N'', N'Disponible', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'279456285', N'Juan', N'Tester', N'3', N'correo@requerimientos3.com', N'83207191', CAST(N'1992-08-22' AS Date), N'Carrandi', N'Matina', N'Limón', NULL, N'Ocupado', N'Tester')
INSERT [dbo].[Empleado] ([idEmpleadoPK], [nombre], [apellido1], [apellido2], [correo], [telefono], [fechaNacimiento], [distrito], [canton], [provincia], [direccion], [estado], [tipoTrabajo]) VALUES (N'571238596', N'Alejandro', N'Tester', N'Tester', N'correo@requerimientos1.com', N'86214302', CAST(N'2005-07-23' AS Date), N'Cachí', N'Paraíso', N'Cartago', N'', N'Ocupado', N'Tester')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'109870354', N'comunicacion')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'109870354', N'liderazgo')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'116610331', N'Comunicacion')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'116610331', N'Liderazgo,Asertividad')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'12345671', N'Comunicación')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'12345672', N'Comunicación')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'12345673', N'Liderazgo')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'liderazgo')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'207440918', N'Comunicacion')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'207440918', N'Ingles')
INSERT [dbo].[HabilidadBlanda] ([idEmpleadoFK], [habilidad]) VALUES (N'209480473', N'comunicacion')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'109870354', N'c#')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'109870354', N'java')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'109870354', N'sql')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'12345671', N'Python')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'12345674', N'Python')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'12345675', N'C++,C#')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'asdf')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'c++')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'dftghtryhrty')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'eq')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'fag')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'fgew')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'gbertfghjrtyjhrtyj')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'ghrtyj')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'gtrg')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'java')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'rhtygrv')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'rtyjrty')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'203450966', N'vergbegrhrtyhjrty')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'207440918', N'C#')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'207440918', N'java')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'207550918', N'C++')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'207550918', N'java')
INSERT [dbo].[HabilidadTecnica] ([idEmpleadoFK], [habilidad]) VALUES (N'209480473', N'java')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (4, 1, N'1248448787', NULL, CAST(N'2019-11-07T10:20:20.010' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (4, 2, N'1248448787', NULL, CAST(N'2019-11-07T10:20:32.407' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 1, N'117010001', NULL, CAST(N'2019-11-04T21:58:08.767' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 2, N'117010002', NULL, CAST(N'2019-11-04T21:58:15.950' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 3, N'117010003', 20, CAST(N'2019-11-01T00:00:00.000' AS DateTime), CAST(N'2019-11-05T00:00:00.000' AS DateTime), N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 4, N'117010004', NULL, CAST(N'2019-11-04T22:00:20.603' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 5, N'117010006', NULL, CAST(N'2019-11-04T22:00:52.290' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 6, N'117010003', 50, CAST(N'2019-11-04T22:01:49.020' AS DateTime), CAST(N'2019-11-09T00:20:13.917' AS DateTime), N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 7, N'117010003', 12, CAST(N'2019-11-10T00:00:00.000' AS DateTime), CAST(N'2019-11-11T00:00:00.000' AS DateTime), N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 8, N'117010006', NULL, CAST(N'2019-11-04T22:03:27.920' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 9, N'117010001', NULL, CAST(N'2019-11-04T22:05:37.963' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 10, N'117010003', 21, CAST(N'2019-11-15T00:00:00.000' AS DateTime), CAST(N'2019-11-16T00:00:00.000' AS DateTime), N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 11, N'117010004', 87, CAST(N'2019-11-05T20:06:32.183' AS DateTime), CAST(N'2019-11-09T11:40:28.233' AS DateTime), N'Inactivo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (6, 12, N'117010001', NULL, CAST(N'2019-11-11T17:20:54.123' AS DateTime), NULL, N'Activo')
INSERT [dbo].[HistorialReqTester] ([idProyectoFK], [idReqFK], [idEmpleadoFK], [horas], [fechaInicio], [fechaFin], [estado]) VALUES (16, 1, N'12345672', 21, CAST(N'2019-11-21T16:26:26.003' AS DateTime), CAST(N'2019-11-22T13:06:01.870' AS DateTime), N'Inactivo')
SET IDENTITY_INSERT [dbo].[Proyecto] ON 

INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (4, N'TestingTriggers', N'', N'Preparación', 0, 0, CAST(N'2019-11-02' AS Date), CAST(N'1800-01-01' AS Date), NULL, 2)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (5, N'Pruebas Equipo', N'Todo lo relacionado a pruebas del módulo equipo, no tocar porfavor.', N'Activo', 0, 0, CAST(N'2019-11-02' AS Date), CAST(N'2019-11-12' AS Date), N'117240933', 0)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (6, N'ProyectoConsultasAvanzadas', N'Mostrar datos para las consultas avanzadas', N'Terminado', 103, 0, CAST(N'2019-11-04' AS Date), CAST(N'2019-11-10' AS Date), N'11223344', 12)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (10, N'ProyectoConsulta5', N'', N'Terminado', 40, 10, CAST(N'2019-11-07' AS Date), CAST(N'1800-01-01' AS Date), N'117240933', 0)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (11, N'p', N'', N'Preparación', 0, 0, CAST(N'2019-11-11' AS Date), CAST(N'1800-01-01' AS Date), N'11223344', 1)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (15, N'PrimerProyecto', N'Prueba editar un proyecto de forma automatica', N'Preparación', 0, 99910, CAST(N'2019-11-15' AS Date), CAST(N'1800-01-01' AS Date), N'11223344', 0)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (16, N'Proyecto para pruebas de prueb', N'', N'Preparación', 0, 0, CAST(N'2019-11-16' AS Date), CAST(N'1800-01-01' AS Date), N'12345', 1)
INSERT [dbo].[Proyecto] ([idProyectoAID], [nombre], [objetivo], [estado], [duracionReal], [duracionEstimada], [fechaInicio], [fechaFinalizacion], [cedulaClienteFK], [cantidadReq]) VALUES (17, N'Proyecto de prueba', N'', N'Preparación', 0, 0, CAST(N'2019-11-21' AS Date), CAST(N'1800-01-01' AS Date), N'11223344', 0)
SET IDENTITY_INSERT [dbo].[Proyecto] OFF
INSERT [dbo].[Prueba] ([idProyectoFK], [idReqFK], [idPruebaPK], [nombre], [resultadoFinal], [propositoPrueba], [entradaDatos], [resultadoEsperado], [estado], [imagen], [descripcionError], [flujoPrueba]) VALUES (16, 1, 1, N'Primera prueba', N'Incompleto', N'', N'Un montón de paja', N'Que sirva', N'Cancelado', 0x, N'', N'Hacer una prueba y dar al botón de aceptar')
INSERT [dbo].[Prueba] ([idProyectoFK], [idReqFK], [idPruebaPK], [nombre], [resultadoFinal], [propositoPrueba], [entradaDatos], [resultadoEsperado], [estado], [imagen], [descripcionError], [flujoPrueba]) VALUES (16, 1, 2, N'Segunda prueba', N'Incompleto', N'Probar el modificar', N'El nombre del requerimiento', N'Qué no me permita crear una prueba con el nombre "Priemra prueba"', N'Cancelado', 0x, N'', N'')
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (1, 4, N'1248448787', N'Testing', N'Medio', 8, NULL, N'', CAST(N'2019-11-02' AS Date), NULL, N'Activo', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (1, 6, N'117010001', N'PrimerRequerimiento', N'Simple', 20, NULL, N'', CAST(N'2019-11-04' AS Date), CAST(N'2019-11-10' AS Date), N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (1, 11, NULL, N'saf', N'Simple', 0, NULL, N'', CAST(N'2019-11-11' AS Date), NULL, N'Cancelado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (1, 16, NULL, N'Primer requerimiento', N'Simple', 0, NULL, N'', CAST(N'2019-11-16' AS Date), NULL, N'Cancelado', NULL, N'', NULL, 2)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (2, 4, N'1248448787', N'SegundoReq', N'Complejo', 15, 2, N'', CAST(N'2019-11-07' AS Date), NULL, N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (2, 6, N'117010002', N'SegundoRequerimiento', N'Muy Complejo', 12, NULL, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (3, 6, N'117010003', N'TercerRequerimiento', N'Medio', 11, 20, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, NULL, NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (4, 6, N'117010004', N'CuartoRequerimiento', N'Medio', 9, NULL, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, NULL, NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (5, 6, N'117010006', N'QuintoRequerimiento', N'Medio', 40, NULL, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, NULL, NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (6, 6, N'117010003', N'SextoRequerimiento', N'Muy Complejo', 100, 50, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, NULL, NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (7, 6, N'117010003', N'SétimoRequerimiento', N'Complejo', 8, NULL, N'', CAST(N'2019-11-04' AS Date), CAST(N'2019-11-11' AS Date), N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (8, 6, N'117010006', N'OctavoRequerimiento', N'Simple', 0, NULL, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, NULL, NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (9, 6, N'117010001', N'NovenoRequerimiento', N'Simple', 0, NULL, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', 1, N'zdasdsd', N'Fallido', 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (10, 6, N'117010003', N'DécimoRequerimiento', N'Muy Complejo', 20, 21, N'', CAST(N'2019-11-04' AS Date), NULL, N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (11, 6, NULL, N'UndécimoRequerimiento', N'Medio', 10, NULL, N'', CAST(N'2019-11-05' AS Date), NULL, N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Requerimiento] ([idReqPK], [idProyectoFK], [cedulaTesterFK], [nombre], [complejidad], [tiempoEstimado], [tiempoReal], [descripcion], [fechaDeInicio], [fechaDeFinalizacion], [estado], [resultado], [detallesResultado], [estadoResultado], [cantidadDePruebas]) VALUES (12, 6, N'117010001', N'Testing232', N'Medio', 15, NULL, N'', CAST(N'2019-11-11' AS Date), CAST(N'2019-11-11' AS Date), N'Terminado', NULL, N'', NULL, 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'1026509874', -2)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'109870354', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'117010001', 3)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'117010002', 1)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'117010003', 4)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'117010004', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'117010006', 2)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12321342342134', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345671', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345672', -1)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345673', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345674', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345675', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'12345676', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'1248448787', 5)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'134259874', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'203450966', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'209480473', 0)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'279456285', 1)
INSERT [dbo].[Tester] ([idEmpleadoFK], [cantidadRequerimientos]) VALUES (N'571238596', 0)
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (4, N'117010001', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (4, N'117010002', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (4, N'1248448787', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (5, N'116610331', N'Lider', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (5, N'117010006', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (5, N'12345671', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010001', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010002', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010003', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010004', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010005', N'Lider', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (6, N'117010006', N'Tester', N'Inactivo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'117010005', N'Lider', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'12345672', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'12345673', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'12345674', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'12345675', N'Tester', N'Activo')
INSERT [dbo].[TrabajaEn] ([idProyectoFK], [idEmpleadoFK], [rol], [estado]) VALUES (16, N'207550918', N'Tester', N'Activo')
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/22/2019 2:26:02 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Cliente_correo]    Script Date: 11/22/2019 2:26:02 PM ******/
ALTER TABLE [dbo].[Cliente] ADD  CONSTRAINT [UQ_Cliente_correo] UNIQUE NONCLUSTERED 
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Empleado_correo]    Script Date: 11/22/2019 2:26:02 PM ******/
ALTER TABLE [dbo].[Empleado] ADD  CONSTRAINT [UQ_Empleado_correo] UNIQUE NONCLUSTERED 
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Proyecto_nombre]    Script Date: 11/22/2019 2:26:02 PM ******/
ALTER TABLE [dbo].[Proyecto] ADD  CONSTRAINT [UQ_Proyecto_nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ('1800-01-01') FOR [fechaNacimiento]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ('Disponible') FOR [estado]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ('Tester') FOR [tipoTrabajo]
GO
ALTER TABLE [dbo].[HistorialReqTester] ADD  DEFAULT ('Despedido') FOR [idEmpleadoFK]
GO
ALTER TABLE [dbo].[HistorialReqTester] ADD  DEFAULT ('Activo') FOR [estado]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ('Preparación') FOR [estado]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ('0') FOR [duracionReal]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ('0') FOR [duracionEstimada]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ('1800-01-01') FOR [fechaInicio]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ('1800-01-01') FOR [fechaFinalizacion]
GO
ALTER TABLE [dbo].[Proyecto] ADD  DEFAULT ((0)) FOR [cantidadReq]
GO
ALTER TABLE [dbo].[Prueba] ADD  DEFAULT ('Incompleto') FOR [resultadoFinal]
GO
ALTER TABLE [dbo].[Prueba] ADD  DEFAULT ('Incompleto') FOR [resultadoEsperado]
GO
ALTER TABLE [dbo].[Prueba] ADD  DEFAULT ('No iniciado') FOR [estado]
GO
ALTER TABLE [dbo].[Requerimiento] ADD  DEFAULT ('Medio') FOR [complejidad]
GO
ALTER TABLE [dbo].[Requerimiento] ADD  DEFAULT ((0)) FOR [tiempoEstimado]
GO
ALTER TABLE [dbo].[Requerimiento] ADD  DEFAULT ('1800-01-01') FOR [fechaDeInicio]
GO
ALTER TABLE [dbo].[Requerimiento] ADD  DEFAULT ('Preparacion') FOR [estado]
GO
ALTER TABLE [dbo].[Requerimiento] ADD  DEFAULT ((0)) FOR [cantidadDePruebas]
GO
ALTER TABLE [dbo].[Tester] ADD  DEFAULT ((0)) FOR [cantidadRequerimientos]
GO
ALTER TABLE [dbo].[TrabajaEn] ADD  DEFAULT ('Tester') FOR [rol]
GO
ALTER TABLE [dbo].[TrabajaEn] ADD  DEFAULT ('Activo') FOR [estado]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[HabilidadBlanda]  WITH CHECK ADD  CONSTRAINT [FK_HabilidadBlanda_idEmpleadoFK] FOREIGN KEY([idEmpleadoFK])
REFERENCES [dbo].[Empleado] ([idEmpleadoPK])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HabilidadBlanda] CHECK CONSTRAINT [FK_HabilidadBlanda_idEmpleadoFK]
GO
ALTER TABLE [dbo].[HabilidadTecnica]  WITH CHECK ADD  CONSTRAINT [FK_idEmpleadoFK] FOREIGN KEY([idEmpleadoFK])
REFERENCES [dbo].[Empleado] ([idEmpleadoPK])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HabilidadTecnica] CHECK CONSTRAINT [FK_idEmpleadoFK]
GO
ALTER TABLE [dbo].[HistorialReqTester]  WITH CHECK ADD  CONSTRAINT [FK_HistorialReqTester_idEmpleadoFK] FOREIGN KEY([idEmpleadoFK])
REFERENCES [dbo].[Tester] ([idEmpleadoFK])
GO
ALTER TABLE [dbo].[HistorialReqTester] CHECK CONSTRAINT [FK_HistorialReqTester_idEmpleadoFK]
GO
ALTER TABLE [dbo].[HistorialReqTester]  WITH CHECK ADD  CONSTRAINT [FK_HistorialReqTester_idReqFK] FOREIGN KEY([idReqFK], [idProyectoFK])
REFERENCES [dbo].[Requerimiento] ([idReqPK], [idProyectoFK])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HistorialReqTester] CHECK CONSTRAINT [FK_HistorialReqTester_idReqFK]
GO
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Cliente] FOREIGN KEY([cedulaClienteFK])
REFERENCES [dbo].[Cliente] ([cedulaPK])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Cliente]
GO
ALTER TABLE [dbo].[Prueba]  WITH CHECK ADD  CONSTRAINT [FK_Prueba_Requerimiento] FOREIGN KEY([idReqFK], [idProyectoFK])
REFERENCES [dbo].[Requerimiento] ([idReqPK], [idProyectoFK])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prueba] CHECK CONSTRAINT [FK_Prueba_Requerimiento]
GO
ALTER TABLE [dbo].[Requerimiento]  WITH CHECK ADD  CONSTRAINT [FK_Req_CedulaTester] FOREIGN KEY([cedulaTesterFK])
REFERENCES [dbo].[Empleado] ([idEmpleadoPK])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Requerimiento] CHECK CONSTRAINT [FK_Req_CedulaTester]
GO
ALTER TABLE [dbo].[Requerimiento]  WITH CHECK ADD  CONSTRAINT [FK_Requerimiento_IdProy] FOREIGN KEY([idProyectoFK])
REFERENCES [dbo].[Proyecto] ([idProyectoAID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Requerimiento] CHECK CONSTRAINT [FK_Requerimiento_IdProy]
GO
ALTER TABLE [dbo].[Tester]  WITH CHECK ADD  CONSTRAINT [FK_Tester_Empleado] FOREIGN KEY([idEmpleadoFK])
REFERENCES [dbo].[Empleado] ([idEmpleadoPK])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tester] CHECK CONSTRAINT [FK_Tester_Empleado]
GO
ALTER TABLE [dbo].[TrabajaEn]  WITH CHECK ADD  CONSTRAINT [FK_TrabajaEn_empleado] FOREIGN KEY([idEmpleadoFK])
REFERENCES [dbo].[Empleado] ([idEmpleadoPK])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TrabajaEn] CHECK CONSTRAINT [FK_TrabajaEn_empleado]
GO
ALTER TABLE [dbo].[TrabajaEn]  WITH CHECK ADD  CONSTRAINT [FK_TrabajaEn_proy] FOREIGN KEY([idProyectoFK])
REFERENCES [dbo].[Proyecto] ([idProyectoAID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrabajaEn] CHECK CONSTRAINT [FK_TrabajaEn_proy]
GO
ALTER TABLE [dbo].[TrabajaEn]  WITH CHECK ADD  CONSTRAINT [CK_TrabajaEn_rol] CHECK  (([rol]='Tester' OR [rol]='Lider'))
GO
ALTER TABLE [dbo].[TrabajaEn] CHECK CONSTRAINT [CK_TrabajaEn_rol]
GO
/****** Object:  StoredProcedure [dbo].[HabilidadesBlandasEmpleado]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[HabilidadesBlandasEmpleado](@id varchar(20))
AS
SELECT B.habilidad 
	FROM HabilidadBlanda B
	WHERE B.idEmpleadoFK = @id
GO
/****** Object:  StoredProcedure [dbo].[HabilidadesTecnicasEmpleado]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[HabilidadesTecnicasEmpleado](@id varchar(20))
AS
SELECT T.habilidad AS 'Técnicas'
	FROM HabilidadTecnica T 
	WHERE T.idEmpleadoFK = @id
GO
/****** Object:  StoredProcedure [dbo].[nombreTester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[nombreTester] @idProyecto INT AS
SELECT E.nombre, E.apellido1, E.idEmpleadoPK
FROM Empleado E
WHERE idEmpleadoPK IN (SELECT idEmpleadoFK
					  FROM HistorialReqTester
					  WHERE idProyectoFK = @idProyecto
					  AND estado = 'Activo')
GO
/****** Object:  StoredProcedure [dbo].[testersActivos]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[testersActivos]
As
Select E.nombre + ' ' + ISNULL(E.apellido1,'') + ' ' + ISNULL(E.apellido2,'') as [Nombre Empleado], E.idEmpleadoPK as Cedula, E.estado, P.nombre as [Nombre Proyecto] , P.idProyectoAID
From Empleado E inner join TrabajaEn TE on TE.idEmpleadoFK = E.idEmpleadoPK
inner join Proyecto P on TE.idProyectoFK = P.idProyectoAID
where E.tipoTrabajo = 'Tester' and TE.estado = 'Activo'
GO
/****** Object:  StoredProcedure [dbo].[TestersAsignables]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[TestersAsignables] @idProyecto INTEGER AS
	SELECT E.idEmpleadoPK, E.nombre, E.apellido1
	FROM Proyecto P JOIN TrabajaEn T 
	ON P.idProyectoAID = T.idProyectoFK 
	JOIN Empleado E 
	ON T.idEmpleadoFK = E.idEmpleadoPK 
	JOIN Tester TEST 
	ON E.idEmpleadoPK  = TEST.idEmpleadoFK             
	WHERE T.idProyectoFK = @idProyecto AND E.tipoTrabajo = 'Tester' AND TEST.cantidadRequerimientos < 10
GO
/****** Object:  StoredProcedure [dbo].[USP_CambiarCedulaCliente]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_CambiarCedulaCliente] @cedulaVieja varchar(20), @cedulaNueva varchar(20)
AS
	UPDATE Cliente
	SET cedulaPK = @cedulaNueva
	WHERE cedulaPK = @cedulaVieja
GO
/****** Object:  StoredProcedure [dbo].[USP_CantidadReqATester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_CantidadReqATester] @idProyecto INTEGER
AS
Select E.idEmpleadoPK ,E.nombre,E.apellido1,E.apellido2, Count(*) as Cantidad
From Requerimiento R inner join Tester T on R.cedulaTesterFK = T.idEmpleadoFK inner join Empleado E on T.idEmpleadoFK = E.idEmpleadoPK
where R.idProyectoFK = @idProyecto
Group by E.idEmpleadoPK ,E.nombre,E.apellido1,E.apellido2
GO
/****** Object:  StoredProcedure [dbo].[USP_ContarRequerimientosTester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
   Cuenta la cantidad de requerimientos que tiene asignados un tester.
*/
CREATE PROC [dbo].[USP_ContarRequerimientosTester](@idTester CHAR(20),@reqs INT OUTPUT)
AS
     BEGIN
     SELECT @reqs = T.cantidadRequerimientos
     FROM Tester T
     WHERE T.idEmpleadoFK = @idTester
     END
GO
/****** Object:  StoredProcedure [dbo].[USP_DuracionesProyecto]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_DuracionesProyecto] @permiso INTEGER , @rol INTEGER, @idUsuario VARCHAR (20)
AS
          
		   IF (@permiso = 1) --Puede ver todos los proyectos
				Begin
						
						Select idProyectoAID, nombre, duracionEstimada, duracionReal
						From Proyecto
						where estado = 'Terminado'
						
				END
            ELSE
				BEGIN
               
					IF (@permiso = 2) --Solo puede ver los proyectos en los que participa
						BEGIN
                   
							IF (@rol = 3) --Si es cliente
								BEGIN
									--Selecciona todos los proyectos que tengan la cedula del cliente asociada
									Select idProyectoAID, nombre, duracionEstimada, duracionReal
									From Proyecto
									Where cedulaClienteFK = @idUsuario and estado = 'Terminado'

								END
							ELSE
								BEGIN
									Select P.idProyectoAID, P.nombre, duracionEstimada, duracionReal
									From Proyecto P inner join TrabajaEn TE on TE.idProyectoFK = P.idProyectoAID
									Where TE.idEmpleadoFK = @idUsuario and P.estado = 'Terminado'
								END

						END
               
					ELSE
					--No tiene permisos
						Select idProyectoAID, nombre, estado
						From Proyecto  
						where idProyectoAID = 0
				END



GO
/****** Object:  StoredProcedure [dbo].[USP_DuracionReqTester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_DuracionReqTester]  @idProyecto INTEGER, @idTester varchar(20)
As
--Diego Contreras
Select R.idReqPK, R.nombre, R.tiempoEstimado , sum(HR.horas) as [tiempoReal]
From HistorialReqTester HR inner join Requerimiento R on HR.idReqFK = R.idReqPK 
where R.idProyectoFK = @idProyecto and HR.idEmpleadoFK = @idTester 
Group by R.idReqPK, R.nombre, R.tiempoEstimado
GO
/****** Object:  StoredProcedure [dbo].[USP_editEmployeeId]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_editEmployeeId] @cedulaVieja VARCHAR(30), @cedulaNueva VARCHAR(30) AS
UPDATE Empleado
SET idEmpleadoPK = @cedulaNueva
WHERE idEmpleadoPK = @cedulaVieja
GO
/****** Object:  StoredProcedure [dbo].[USP_EquipoCheckLider]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
    Retorna 1 si hay un  lider en el equipo del proyecto con el id pasado.
            0 en caso contrario.
*/
CREATE PROC [dbo].[USP_EquipoCheckLider](@idProyecto INT,@liderFlag INT OUTPUT)
AS
     BEGIN
        SELECT @liderFlag = COUNT(E.idEmpleadoPK)
        FROM TrabajaEn TB JOIN Empleado E
        ON TB.idEmpleadoFK = E.idEmpleadoPK
        WHERE TB.idProyectoFK = @idProyecto 
			  AND TB.rol = 'Lider' 
			  AND TB.estado = 'Activo'
    END
GO
/****** Object:  StoredProcedure [dbo].[USP_EquipoCheckTesters]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
   Cuenta los testers del equipo para el proyecto con el ID pasado como parámetro.
*/
CREATE PROC [dbo].[USP_EquipoCheckTesters](@idProyecto INT,@testers INT OUTPUT)
AS
     BEGIN
        SELECT @testers = COUNT(E.idEmpleadoPK)
        FROM TrabajaEn TB JOIN Empleado E
        ON TB.idEmpleadoFK = E.idEmpleadoPK
        WHERE TB.idProyectoFK = @idProyecto 
			  AND TB.rol = 'Tester' 
			  AND TB.estado = 'Activo'
    END
GO
/****** Object:  StoredProcedure [dbo].[USP_FechaInicioFinRequerimiento]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_FechaInicioFinRequerimiento] @idProyecto integer , @idEmpleado varchar(20)  
As
Select  R.nombre,R.estado,R.complejidad, min(HR.fechaInicio) as [fechaInicio], max(HR.fechaFin) as [fechaFin], SUM(horas) as [horas]
From HistorialReqTester HR inner join Requerimiento R on HR.idReqFK = R.idReqPK and HR.idProyectoFK = R.idProyectoFK
where HR.idEmpleadoFK = @idEmpleado and HR.idProyectoFK = @idProyecto
Group by  R.nombre,R.estado,R.complejidad
GO
/****** Object:  StoredProcedure [dbo].[USP_FechaInioFinTesterProyecto]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure  [dbo].[USP_FechaInioFinTesterProyecto] @idEmpleadoFK varchar(20)
as


Select P.idProyectoAID ,P.nombre, min(HR.fechaInicio) as [fechaInicio], max(HR.fechaFin) as [fechaFin], SUM(horas) as [horas]
From HistorialReqTester HR inner join Proyecto P on HR.idProyectoFK = P.idProyectoAID
where HR.idEmpleadoFK = @idEmpleadoFK
Group by  P.idProyectoAID ,P.nombre
GO
/****** Object:  StoredProcedure [dbo].[USP_GetEmpleadosDeLider]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[USP_GetEmpleadosDeLider] @cedulaLider VARCHAR(30) AS

SELECT * 
FROM Empleado E, TrabajaEn TE
WHERE E.idEmpleadoPK = TE.idEmpleadoFK
AND idProyectoFK IN (SELECT TE.idProyectoFK
					FROM Empleado E, TrabajaEn TE
					WHERE E.idEmpleadoPK = TE.idEmpleadoFK
					AND TE.idEmpleadoFK = @cedulaLider)
--EXEC USP_getProyectoDeLider '207440918'

--SELECT * FROM TrabajaEn









GO
/****** Object:  StoredProcedure [dbo].[USP_GetEquipo]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
--SP Para obtener testers disponibles de la tabla Empleado.
CREATE PROC USP_GetTestersDisponibles
AS
	SELECT *
	FROM Empleado E
	WHERE E.estado = 'Disponible' AND E.tipoTrabajo = 'Tester'
GO
*/

/*
--SP Para obtener líderes disponibles de la tabla Empleado.
CREATE PROC USP_GetLideresDisponibles
AS
SELECT *
FROM Empleado E
WHERE E.estado = 'Disponible' AND E.tipoTrabajo = 'Lider'
RETURN
GO
*/

CREATE PROC [dbo].[USP_GetEquipo] (@id_proyecto INT)
AS
	BEGIN
		SELECT 
			E.idEmpleadoPK,
			E.nombre,
			E.apellido1,
			E.estado,
			E.tipoTrabajo
		FROM Proyecto P JOIN TrabajaEn TB
		ON P.idProyectoAID = TB.idProyectoFK
		JOIN Empleado E
		ON TB.idEmpleadoFK = E.idEmpleadoPK
		WHERE P.idProyectoAID = @id_proyecto AND TB.estado = 'Activo'
	END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetLideresDisponibles]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
--SP Para obtener testers disponibles de la tabla Empleado.
CREATE PROC USP_GetTestersDisponibles
AS
	SELECT *
	FROM Empleado E
	WHERE E.estado = 'Disponible' AND E.tipoTrabajo = 'Tester'
GO
*/

--SP Para obtener líderes disponibles de la tabla Empleado.
CREATE PROC [dbo].[USP_GetLideresDisponibles]
AS
SELECT *
FROM Empleado E
WHERE E.estado = 'Disponible' AND E.tipoTrabajo = 'Lider'
RETURN
GO
/****** Object:  StoredProcedure [dbo].[USP_GetTestersDisponibles]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SP Para obtener testers disponibles de la tabla Empleado.
CREATE PROC [dbo].[USP_GetTestersDisponibles]
AS
	SELECT *
	FROM Empleado E
	WHERE E.estado = 'Disponible' AND E.tipoTrabajo = 'Tester'
GO
/****** Object:  StoredProcedure [dbo].[USP_GetTestersPorHabilidades]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetTestersPorHabilidades](@habilidades VARCHAR(30))
AS
    BEGIN
        SELECT E.idEmpleadoPK
        FROM Empleado E
        LEFT JOIN HabilidadBlanda HB
        ON E.idEmpleadoPK = HB.idEmpleadoFK
        LEFT JOIN HabilidadTecnica HK
        ON E.idEmpleadoPK = HK.idEmpleadoFK
        WHERE HB.habilidad LIKE '%'+@habilidades+'%'    
            OR    HK.habilidad LIKE '%'+@habilidades+'%'
    END
GO
/****** Object:  StoredProcedure [dbo].[USP_obtenerEdad]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_obtenerEdad] (@id varchar(20))
AS
BEGIN 
SELECT DATEDIFF(YEAR, fechaNacimiento, GETDATE())
FROM Empleado
WHERE idEmpleadoPK =@id

END;
GO
/****** Object:  StoredProcedure [dbo].[USP_ObtenerProyectosUsuario]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_ObtenerProyectosUsuario] @permiso INTEGER , @rol INTEGER, @idUsuario VARCHAR (20)	AS
          
		   IF (@permiso = 1) --Puede ver todos los proyectos
				Begin
						
						Select P.idProyectoAID AS [idProyecto], 
							   P.nombre AS [nombreProyecto], 
							   E.nombre + ' ' + E.apellido1 AS [nombreEmpleado]						
						From Proyecto P Join TrabajaEn TB
						On P.idProyectoAID = TB.idProyectoFK
						Join Empleado E
						On TB.idEmpleadoFK = E.idEmpleadoPK
						Where P.estado = 'Terminado' AND E.tipoTrabajo = 'Lider'
						
				END
            ELSE
				BEGIN
               
					IF (@permiso = 2) --Solo puede ver los proyectos en los que participa
						BEGIN
                   
							IF (@rol = 3) --Si es cliente
								BEGIN
									--Selecciona todos los proyectos que tengan la cedula del cliente asociada
									Select P.idProyectoAID AS [idProyecto], 
										   P.nombre AS [nombreProyecto], 
										   E.nombre + ' ' + E.apellido1 AS [nombreEmpleado]								
									From Proyecto P Join TrabajaEn TB
									On P.idProyectoAID = TB.idProyectoFK
									Join Empleado E
									On TB.idEmpleadoFK = E.idEmpleadoPK
									Where P.estado = 'Terminado' AND E.tipoTrabajo = 'Lider'
										  AND P.cedulaClienteFK = @idUsuario

								END
							ELSE
								BEGIN
									--Selecciona todos los proyectos que tengan la cedula del cliente asociada
									Select P.idProyectoAID AS [idProyecto], 
										P.nombre AS [nombreProyecto], 
										E.nombre + ' ' + E.apellido1 AS [nombreEmpleado]								
									From Proyecto P Join TrabajaEn TB
									On P.idProyectoAID = TB.idProyectoFK
									Join Empleado E
									On TB.idEmpleadoFK = E.idEmpleadoPK
									Where P.estado = 'Terminado' AND E.tipoTrabajo = 'Lider'
									      AND TB.idEmpleadoFK = @idUsuario
								END

						END
               
					ELSE
					--No tiene permisos
						--Selecciona todos los proyectos que tengan la cedula del cliente asociada
									Select P.idProyectoAID AS [idProyecto], 
										 P.nombre AS [nombreProyecto], 
										 E.nombre + ' ' + E.apellido1 AS [nombreEmpleado]							
									From Proyecto P Join TrabajaEn TB
									On P.idProyectoAID = TB.idProyectoFK
									Join Empleado E
									On TB.idEmpleadoFK = E.idEmpleadoPK
									Where P.estado = 'Terminado' AND E.tipoTrabajo = 'Lider'
										  AND P.idProyectoAID = 0
				END
GO
/****** Object:  StoredProcedure [dbo].[USP_TestersDisponibleAsignado]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_TestersDisponibleAsignado] AS
SELECT E.nombre + ' ' + ISNULL(E.apellido1,'') + ' ' + ISNULL(E.apellido2,'') as [Nombre Empleado],
E.idEmpleadoPK as Cedula, 
E.estado, 
IsNull(dbo.ObtenerProyectoActualDelEmpleado(E.idEmpleadoPK),' ') as [Nombre Proyecto]FROM Empleado EWHERE E.tipoTrabajo = 'Tester' and E.estado != 'Despedido'ORDER BY [Nombre Proyecto]




















GO
/****** Object:  Trigger [dbo].[actualizarCorreoSeguridad]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[actualizarCorreoSeguridad]
ON [dbo].[Cliente]
AFTER UPDATE
AS
	DECLARE @nuevoCorreo VARCHAR(50),@viejoCorreo VARCHAR(50)
BEGIN
	SET NOCOUNT ON;
	IF UPDATE(correo)
		BEGIN
			SELECT @viejoCorreo = (SELECT correo FROM DELETED)
			SELECT @nuevoCorreo = (SELECT correo FROM INSERTED)

			UPDATE AspNetUsers
			SET Email = @nuevoCorreo, UserName = @nuevoCorreo
			WHERE UserName = @viejoCorreo
		END
END
GO
ALTER TABLE [dbo].[Cliente] ENABLE TRIGGER [actualizarCorreoSeguridad]
GO
/****** Object:  Trigger [dbo].[verificarEdad]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[verificarEdad]
ON [dbo].[Empleado] 
FOR INSERT, UPDATE
AS
BEGIN
DECLARE @fecha DATE
SELECT @fecha =(SELECT fechaNacimiento FROM INSERTED)
IF(DATEDIFF(YEAR, @fecha, GETDATE()) < 18)
	 ROLLBACK TRANSACTION
END
GO
ALTER TABLE [dbo].[Empleado] ENABLE TRIGGER [verificarEdad]
GO
/****** Object:  Trigger [dbo].[TR_AU_Proyecto]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_AU_Proyecto]
ON [dbo].[Proyecto]
For UPDATE
AS

	IF UPDATE(estado)
	Begin
		DECLARE @estado VARCHAR(30)
		DECLARE @idProyecto  INTEGER
		DECLARE @horas INTEGER
		Select @estado = (Select estado from Inserted)
		Select @idProyecto = (Select idProyectoAID  from Inserted)
		IF(@estado = 'Terminado')
			
			Begin
			--Cambia los estados
				update Requerimiento
				set estado = 'Terminado'
				where idProyectoFK = @idProyecto
			--Libera a las empleados que trabajaban en el proyecto
				update TrabajaEn
				set estado = 'Inactivo'
				where idProyectoFK = @idProyecto
			--Calcula las horas 
			Select @horas = ISNULL(sum(tiempoReal),0)
			From Requerimiento
			where idProyectoFK = @idProyecto

			--Actualiza la duracionReal y la fecha Final en el Proyecto
			update Proyecto
			set duracionReal = @horas, fechaFinalizacion = GETDATE()
			where idProyectoAID = @idProyecto
			END
		IF( @estado = 'Cancelado')
			Begin
			--Cambia los estados
				update Requerimiento
				set estado = 'Cancelado'
				where idProyectoFK = @idProyecto

			--Libera a las empleados que trabajaban en el proyecto
				update TrabajaEn
				set estado = 'Inactivo'
				where idProyectoFK = @idProyecto
		   
		   --Calcula las horas 
			Select @horas = ISNULL(sum(tiempoReal),0)
			From Requerimiento
			where idProyectoFK = 1

			--Actualiza la duracionReal y la fecha Final en el Proyecto
			update Proyecto
			set duracionReal = @horas, fechaFinalizacion = GETDATE()
			where idProyectoAID = @idProyecto
			End
			

	End
GO
ALTER TABLE [dbo].[Proyecto] ENABLE TRIGGER [TR_AU_Proyecto]
GO
/****** Object:  Trigger [dbo].[TR_ActualiceIdPrueba]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ActualiceIdPrueba]
ON [dbo].[Prueba]
INSTEAD OF INSERT
AS
BEGIN

	DECLARE @idProy INTEGER, @idReq INTEGER;

	SELECT @idProy = idProyectoFK, @idReq = idReqFK
	FROM INSERTED;

	INSERT INTO Prueba
	SELECT N.idProyectoFK, N.idReqFK, R.cantidadDePruebas + 1, N.nombre, N.resultadoFinal,
	N.propositoPrueba, N.entradaDatos, N.resultadoEsperado, N.estado,
	N.imagen, N.descripcionError, N.flujoPrueba
	FROM Requerimiento R, INSERTED N
	WHERE R.idProyectoFK = @idProy AND R.idReqPK = @idReq;

	UPDATE Requerimiento
	SET cantidadDePruebas = cantidadDePruebas + 1
	WHERE idProyectoFK = @idProy AND idReqPK = @idReq;

END
GO
ALTER TABLE [dbo].[Prueba] ENABLE TRIGGER [TR_ActualiceIdPrueba]
GO
/****** Object:  Trigger [dbo].[actualiceIDRequerimiento]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[actualiceIDRequerimiento]
ON [dbo].[Requerimiento]
INSTEAD OF INSERT 
AS
BEGIN

	DECLARE @idProy INTEGER;

	SELECT @idProy = (SELECT idProyectoFK FROM INSERTED);

	INSERT INTO Requerimiento
	SELECT P.cantidadReq + 1, N.idProyectoFK, N.cedulaTesterFK, N.nombre, N.complejidad,
	N.tiempoEstimado, N.tiempoReal, N.descripcion, N.fechaDeInicio, N.fechaDeFinalizacion, N.estado,
	N.resultado, N.detallesResultado ,N.estadoResultado, N.cantidadDePruebas
	FROM Proyecto P, INSERTED N
	WHERE P.idProyectoAID = @idProy;

	UPDATE Proyecto
	SET cantidadReq = cantidadReq + 1
	WHERE idProyectoAID = @idPRoy;

END
GO
ALTER TABLE [dbo].[Requerimiento] ENABLE TRIGGER [actualiceIDRequerimiento]
GO
/****** Object:  Trigger [dbo].[canceleRequerimiento]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[canceleRequerimiento]
ON [dbo].[Requerimiento]
AFTER UPDATE
AS
BEGIN
	IF UPDATE(estado)
	BEGIN
		--Declaramos las variables que utilizaremos para cancelar todo lo asociado al requerimiento
		DECLARE @idProyecto INTEGER, @idRequerimiento INTEGER, @cedulaTester VARCHAR(20), @estado VARCHAR(30), @cantidadPruebas INTEGER;

		--Obtenemos los valores de la llave primaria del requerimiento que se desea cancelar
		SELECT @idProyecto = R.idProyectoFK, @idRequerimiento = R.idReqPK, @cedulaTester = R.cedulaTesterFK, @estado = R.estado
		FROM Requerimiento R, inserted i
		WHERE R.idReqPK = i.idReqPK AND R.idProyectoFK = i.idProyectoFK;

		--Si se actualizó el estado a cancelado, hacemos cosas
		IF @estado = 'Cancelado'
		BEGIN
			--Cancelamos todas las pruebas que contenga ese requerimiento
			UPDATE Prueba
			SET estado = 'Cancelado'
			WHERE idProyectoFK = @idProyecto AND idReqFK = @idRequerimiento;

			--Si hay un tester asociado
			IF @cedulaTester IS NOT NULL
			BEGIN
				
				--Lo ponemos como inactivo en este requerimiento
				UPDATE HistorialReqTester
				SET fechaFin = GETDATE(), estado = 'Inactivo', horas = DATEDIFF(HOUR, fechaInicio, GETDATE())
				WHERE idEmpleadoFK = @cedulaTester AND idReqFK = @idRequerimiento AND idProyectoFK = @idProyecto;

				--Lo desasociamos de el
				UPDATE Requerimiento
				SET cedulaTesterFK = NULL
				WHERE idReqPK = @idRequerimiento AND idProyectoFK = @idProyecto;

				--Y actualizamos la cantidad de requerimientos que este tiene asociados
				UPDATE Tester
				SET cantidadRequerimientos = cantidadRequerimientos - 1
				WHERE idEmpleadoFK = @cedulaTester;
			END
		END
	END
END
GO
ALTER TABLE [dbo].[Requerimiento] ENABLE TRIGGER [canceleRequerimiento]
GO
/****** Object:  Trigger [dbo].[insertarActualiceTester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertarActualiceTester]
ON [dbo].[Requerimiento]
AFTER INSERT
AS

	DECLARE @cedulaNueva VARCHAR(20), @idProyecto INTEGER, @idRequerimiento INTEGER;

	Print 'Insert' + @cedulaNueva + ' ' + @idProyecto + ' ' + @idRequerimiento
	SELECT @cedulaNueva = cedulaTesterFK, @idProyecto = idProyectoFK, @idRequerimiento = idReqPK
	FROM INSERTED;

	IF @cedulaNueva IS NOT NULL
	BEGIN
		UPDATE Tester
		SET cantidadRequerimientos = cantidadRequerimientos + 1
		WHERE idEmpleadoFK = @cedulaNueva;
			
		INSERT INTO HistorialReqTester(idProyectoFK , idReqFK, idEmpleadoFK, fechaInicio, estado )
		VALUES(@idProyecto, @idRequerimiento, @cedulaNueva, GETDATE(), 'Activo');
	END
GO
ALTER TABLE [dbo].[Requerimiento] ENABLE TRIGGER [insertarActualiceTester]
GO
/****** Object:  Trigger [dbo].[modificarActualiceTester]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[modificarActualiceTester]
ON [dbo].[Requerimiento]
AFTER UPDATE
AS

	IF(UPDATE(cedulaTesterFK))
	Begin
		DECLARE @cedulaVieja VARCHAR(20), @cedulaNueva VARCHAR(20), @idProyecto INTEGER, @idRequerimiento INTEGER;
		SELECT @cedulaNueva = I.cedulaTesterFK, @idProyecto = I.idProyectoFK, @idRequerimiento = I.idReqPK, @cedulaVieja = D.cedulaTesterFK
		FROM INSERTED I, DELETED D;

		Print 'Vieja ' + ISNULL(@cedulaVieja,'Null') + ' Nueva ' + ISNULL(@cedulaNueva,'Null')

		IF @cedulaVieja IS NULL
			BEGIN
			Print 'Cedula Vieja is null'
				IF @cedulaNueva is not null
					BEGIN
					Print 'Cedula Nueva is not null'
					UPDATE Tester
					SET cantidadRequerimientos = cantidadRequerimientos + 1
					WHERE idEmpleadoFK = @cedulaNueva;

					INSERT INTO HistorialReqTester(idProyectoFK , idReqFK, idEmpleadoFK, fechaInicio, estado )
					VALUES(@idProyecto, @idRequerimiento, @cedulaNueva, GETDATE(), 'Activo');
					END



			END
		Else
			BEGIN
			Print 'Cedula Vieja is not null'
			    IF @cedulaNueva IS  NULL
					BEGIN
						Print 'Cedula Nueva is  null'
						UPDATE HistorialReqTester
						SET fechaFin = GETDATE(), estado = 'Inactivo', horas = DATEDIFF(HOUR, fechaInicio, GETDATE())
						WHERE idEmpleadoFK = @cedulaVieja AND idReqFK = @idRequerimiento AND idProyectoFK = @idProyecto;

						UPDATE Tester
						SET cantidadRequerimientos = cantidadRequerimientos - 1
						WHERE idEmpleadoFK = @cedulaVieja;
					END
				Else
					BEGIN
						IF(@cedulaVieja <> @cedulaNueva)
							Begin
								Print 'Cedulas Diferentes'
								--Actualiza el tester anterior 
								UPDATE HistorialReqTester
								SET fechaFin = GETDATE(), estado = 'Inactivo', horas = DATEDIFF(HOUR, fechaInicio, GETDATE())
								WHERE idEmpleadoFK = @cedulaVieja AND idReqFK = @idRequerimiento AND idProyectoFK = @idProyecto;

								UPDATE Tester
								SET cantidadRequerimientos = cantidadRequerimientos - 1
								WHERE idEmpleadoFK = @cedulaVieja;

								UPDATE Tester
								SET cantidadRequerimientos = cantidadRequerimientos + 1
								WHERE idEmpleadoFK = @cedulaVieja;

								INSERT INTO HistorialReqTester(idProyectoFK , idReqFK, idEmpleadoFK, fechaInicio, estado )
								VALUES(@idProyecto, @idRequerimiento, @cedulaNueva, GETDATE(), 'Activo');

							END
					END
			END
	END


GO
ALTER TABLE [dbo].[Requerimiento] ENABLE TRIGGER [modificarActualiceTester]
GO
/****** Object:  Trigger [dbo].[termineRequerimiento]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[termineRequerimiento]
ON [dbo].[Requerimiento]
AFTER UPDATE
AS
BEGIN 
	IF UPDATE(estado)
	BEGIN
		DECLARE @idProyecto INTEGER, @idRequerimiento INTEGER, @cedulaTester VARCHAR(20), @estado VARCHAR(30), @fechaFin DATE;

		SELECT @idProyecto = R.idProyectoFK, @idRequerimiento = R.idReqPK, @cedulaTester = R.cedulaTesterFK, @estado = R.estado, @fechaFin = R.fechaDeFinalizacion
		FROM Requerimiento R, inserted i
		WHERE R.idReqPK = i.idReqPK AND R.idProyectoFK = i.idProyectoFK;

		IF @estado = 'Terminado'
		BEGIN
			print('El estado está terminado')
			IF @fechaFin IS NULL
			BEGIN
				print('La fecha de fin es nula')
				UPDATE Requerimiento
				SET fechaDeFinalizacion = GETDATE()
				WHERE idProyectoFK = @idProyecto AND idReqPK = @idRequerimiento;
			END
		END
	END
END
GO
ALTER TABLE [dbo].[Requerimiento] ENABLE TRIGGER [termineRequerimiento]
GO
/****** Object:  Trigger [dbo].[TR_AU_TrabajaEn]    Script Date: 11/22/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_AU_TrabajaEn]
ON [dbo].[TrabajaEn]
AFTER UPDATE
AS

	IF UPDATE(estado)
	Begin
		
		DECLARE @estado VARCHAR(30)
		DECLARE @idTester  varchar(30)
		DECLARE @horas INTEGER
		Declare @counter int; --variable de conteo de tuplas


		Select @counter = 1

		DECLARE my_cursor CURSOR LOCAL FOR
		SELECT i.estado,i.idEmpleadoFK
		FROM  inserted i

		OPEN my_cursor
		FETCH NEXT FROM my_cursor INTO @estado,  @idTester
		while(@@FETCH_STATUS <> -1 )
		BEGIN
			IF(@estado = 'Inactivo')
			Begin
				update Empleado
				set estado = 'Disponible'
				where idEmpleadoPK = @idTester
			End
			Else
				Begin
					update Empleado
					set estado = 'Ocupado'
					where idEmpleadoPK = @idTester
				End
			FETCH NEXT FROM my_cursor INTO @estado,  @idTester
		end
		close my_cursor
		deallocate my_cursor

		--IF(@estado = 'Inactivo')
		--	Begin
		--		update Empleado
		--		set estado = 'Disponible'
		--		where idEmpleadoPK = @idTester
		--	End
		--Else
		--	Begin
		--		update Empleado
		--		set estado = 'Ocupado'
		--		where idEmpleadoPK = @idTester
		--	End
	End
GO
ALTER TABLE [dbo].[TrabajaEn] ENABLE TRIGGER [TR_AU_TrabajaEn]
GO
