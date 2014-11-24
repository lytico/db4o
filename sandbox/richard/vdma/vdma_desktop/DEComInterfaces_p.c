

/* this ALWAYS GENERATED file contains the proxy stub code */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Mon Oct 15 20:08:34 2007
 */
/* Compiler settings for .\DEComInterfaces.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#if !defined(_M_IA64) && !defined(_M_AMD64)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */
#pragma warning( disable: 4211 )  /* redefine extent to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma optimize("", off ) 

#define USE_STUBLESS_PROXY


/* verify that the <rpcproxy.h> version is high enough to compile this file*/
#ifndef __REDQ_RPCPROXY_H_VERSION__
#define __REQUIRED_RPCPROXY_H_VERSION__ 475
#endif


#include "rpcproxy.h"
#ifndef __RPCPROXY_H_VERSION__
#error this stub requires an updated version of <rpcproxy.h>
#endif // __RPCPROXY_H_VERSION__


#include "DEComInterfaces_h.h"

#define TYPE_FORMAT_STRING_SIZE   367                               
#define PROC_FORMAT_STRING_SIZE   2113                              
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   2            

typedef struct _MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } MIDL_TYPE_FORMAT_STRING;

typedef struct _MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } MIDL_PROC_FORMAT_STRING;


static RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const MIDL_TYPE_FORMAT_STRING __MIDL_TypeFormatString;
extern const MIDL_PROC_FORMAT_STRING __MIDL_ProcFormatString;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorVirtualMachineManager_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorVirtualMachineManager_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorVirtualTransport_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorVirtualTransport_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorDMAChannel_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDMAChannel_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorDebuggerHaltNotificationSink_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDebuggerHaltNotificationSink_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorDebugger_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDebugger_ProxyInfo;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO IDeviceEmulatorItem_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorItem_ProxyInfo;


extern const EXPR_EVAL ExprEvalRoutines[];
extern const USER_MARSHAL_ROUTINE_QUADRUPLE UserMarshalRoutines[ WIRE_MARSHAL_TABLE_SIZE ];

#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif

#if !(TARGET_IS_NT50_OR_LATER)
#error You need a Windows 2000 or later to run this stub because it uses these features:
#error   /robust command line switch.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will die there with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const MIDL_PROC_FORMAT_STRING __MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure GetProcessorFamily */


	/* Procedure GetVirtualMachineCount */

			0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x3 ),	/* 3 */
/*  8 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 10 */	NdrFcShort( 0x0 ),	/* 0 */
/* 12 */	NdrFcShort( 0x24 ),	/* 36 */
/* 14 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 16 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 18 */	NdrFcShort( 0x0 ),	/* 0 */
/* 20 */	NdrFcShort( 0x0 ),	/* 0 */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter pdwProcessorFamily */


	/* Parameter numberOfVMs */

/* 24 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 26 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 28 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */


	/* Return value */

/* 30 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 32 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 34 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure EnumerateVirtualMachines */

/* 36 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 38 */	NdrFcLong( 0x0 ),	/* 0 */
/* 42 */	NdrFcShort( 0x4 ),	/* 4 */
/* 44 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 46 */	NdrFcShort( 0x1c ),	/* 28 */
/* 48 */	NdrFcShort( 0x24 ),	/* 36 */
/* 50 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x3,		/* 3 */
/* 52 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 54 */	NdrFcShort( 0x1 ),	/* 1 */
/* 56 */	NdrFcShort( 0x0 ),	/* 0 */
/* 58 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter numberOfVMs */

/* 60 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 62 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 64 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter virtualMachineID */

/* 66 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 68 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 70 */	NdrFcShort( 0x1c ),	/* Type Offset=28 */

	/* Return value */

/* 72 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 74 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 76 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IsVirtualMachineRunning */

/* 78 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 80 */	NdrFcLong( 0x0 ),	/* 0 */
/* 84 */	NdrFcShort( 0x5 ),	/* 5 */
/* 86 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 88 */	NdrFcShort( 0x44 ),	/* 68 */
/* 90 */	NdrFcShort( 0x21 ),	/* 33 */
/* 92 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 94 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 96 */	NdrFcShort( 0x0 ),	/* 0 */
/* 98 */	NdrFcShort( 0x0 ),	/* 0 */
/* 100 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 102 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 104 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 106 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter isRunning */

/* 108 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 110 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 112 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 114 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 116 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 118 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ResetVirtualMachine */

/* 120 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 122 */	NdrFcLong( 0x0 ),	/* 0 */
/* 126 */	NdrFcShort( 0x6 ),	/* 6 */
/* 128 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 130 */	NdrFcShort( 0x49 ),	/* 73 */
/* 132 */	NdrFcShort( 0x8 ),	/* 8 */
/* 134 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 136 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 138 */	NdrFcShort( 0x0 ),	/* 0 */
/* 140 */	NdrFcShort( 0x0 ),	/* 0 */
/* 142 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 144 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 146 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 148 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter hardReset */

/* 150 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 152 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 154 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 156 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 158 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 160 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure CreateVirtualMachine */

/* 162 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 164 */	NdrFcLong( 0x0 ),	/* 0 */
/* 168 */	NdrFcShort( 0x7 ),	/* 7 */
/* 170 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 172 */	NdrFcShort( 0x0 ),	/* 0 */
/* 174 */	NdrFcShort( 0x8 ),	/* 8 */
/* 176 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x2,		/* 2 */
/* 178 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 180 */	NdrFcShort( 0x0 ),	/* 0 */
/* 182 */	NdrFcShort( 0x0 ),	/* 0 */
/* 184 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter commandLine */

/* 186 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 188 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 190 */	NdrFcShort( 0x36 ),	/* Type Offset=54 */

	/* Return value */

/* 192 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 194 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 196 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ShutdownVirtualMachine */

/* 198 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 200 */	NdrFcLong( 0x0 ),	/* 0 */
/* 204 */	NdrFcShort( 0x8 ),	/* 8 */
/* 206 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 208 */	NdrFcShort( 0x49 ),	/* 73 */
/* 210 */	NdrFcShort( 0x8 ),	/* 8 */
/* 212 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 214 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 216 */	NdrFcShort( 0x0 ),	/* 0 */
/* 218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 220 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 222 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 224 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 226 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter saveMachine */

/* 228 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 230 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 232 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 234 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 236 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 238 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RestoreVirtualMachine */

/* 240 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 242 */	NdrFcLong( 0x0 ),	/* 0 */
/* 246 */	NdrFcShort( 0x9 ),	/* 9 */
/* 248 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 250 */	NdrFcShort( 0x44 ),	/* 68 */
/* 252 */	NdrFcShort( 0x8 ),	/* 8 */
/* 254 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 256 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 258 */	NdrFcShort( 0x0 ),	/* 0 */
/* 260 */	NdrFcShort( 0x0 ),	/* 0 */
/* 262 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 264 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 266 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 268 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Return value */

/* 270 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 272 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 274 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure DeleteVirtualMachine */

/* 276 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 278 */	NdrFcLong( 0x0 ),	/* 0 */
/* 282 */	NdrFcShort( 0xa ),	/* 10 */
/* 284 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 286 */	NdrFcShort( 0x44 ),	/* 68 */
/* 288 */	NdrFcShort( 0x8 ),	/* 8 */
/* 290 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 292 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 294 */	NdrFcShort( 0x0 ),	/* 0 */
/* 296 */	NdrFcShort( 0x0 ),	/* 0 */
/* 298 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 300 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 302 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 304 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Return value */

/* 306 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 308 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 310 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetVirtualMachineName */

/* 312 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 314 */	NdrFcLong( 0x0 ),	/* 0 */
/* 318 */	NdrFcShort( 0xb ),	/* 11 */
/* 320 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 322 */	NdrFcShort( 0x44 ),	/* 68 */
/* 324 */	NdrFcShort( 0x8 ),	/* 8 */
/* 326 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x3,		/* 3 */
/* 328 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 330 */	NdrFcShort( 0x0 ),	/* 0 */
/* 332 */	NdrFcShort( 0x0 ),	/* 0 */
/* 334 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 336 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 338 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 340 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter virtualMachineName */

/* 342 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 344 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 346 */	NdrFcShort( 0x38 ),	/* Type Offset=56 */

	/* Return value */

/* 348 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 350 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 352 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure SetVirtualMachineName */

/* 354 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 356 */	NdrFcLong( 0x0 ),	/* 0 */
/* 360 */	NdrFcShort( 0xc ),	/* 12 */
/* 362 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 364 */	NdrFcShort( 0x44 ),	/* 68 */
/* 366 */	NdrFcShort( 0x8 ),	/* 8 */
/* 368 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 370 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 372 */	NdrFcShort( 0x0 ),	/* 0 */
/* 374 */	NdrFcShort( 0x0 ),	/* 0 */
/* 376 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 378 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 380 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 382 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter virtualMachineName */

/* 384 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 386 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 388 */	NdrFcShort( 0x36 ),	/* Type Offset=54 */

	/* Return value */

/* 390 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 392 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 394 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure BringVirtualMachineToFront */

/* 396 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 398 */	NdrFcLong( 0x0 ),	/* 0 */
/* 402 */	NdrFcShort( 0xd ),	/* 13 */
/* 404 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 406 */	NdrFcShort( 0x44 ),	/* 68 */
/* 408 */	NdrFcShort( 0x8 ),	/* 8 */
/* 410 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 412 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 414 */	NdrFcShort( 0x0 ),	/* 0 */
/* 416 */	NdrFcShort( 0x0 ),	/* 0 */
/* 418 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 420 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 422 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 424 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Return value */

/* 426 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 428 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 430 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ConfigureDevice */

/* 432 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 434 */	NdrFcLong( 0x0 ),	/* 0 */
/* 438 */	NdrFcShort( 0xe ),	/* 14 */
/* 440 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 442 */	NdrFcShort( 0x8 ),	/* 8 */
/* 444 */	NdrFcShort( 0x8 ),	/* 8 */
/* 446 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 448 */	0x8,		/* 8 */
			0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 450 */	NdrFcShort( 0x1 ),	/* 1 */
/* 452 */	NdrFcShort( 0x2 ),	/* 2 */
/* 454 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hwndParent */

/* 456 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 458 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 460 */	NdrFcShort( 0x58 ),	/* Type Offset=88 */

	/* Parameter lcidParent */

/* 462 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 464 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 466 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter bstrConfig */

/* 468 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 470 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 472 */	NdrFcShort( 0x7c ),	/* Type Offset=124 */

	/* Parameter pbstrConfig */

/* 474 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 476 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 478 */	NdrFcShort( 0x8e ),	/* Type Offset=142 */

	/* Return value */

/* 480 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 482 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 484 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetMACAddressCount */

/* 486 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 488 */	NdrFcLong( 0x0 ),	/* 0 */
/* 492 */	NdrFcShort( 0xf ),	/* 15 */
/* 494 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 496 */	NdrFcShort( 0x44 ),	/* 68 */
/* 498 */	NdrFcShort( 0x24 ),	/* 36 */
/* 500 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 502 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 504 */	NdrFcShort( 0x0 ),	/* 0 */
/* 506 */	NdrFcShort( 0x0 ),	/* 0 */
/* 508 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 510 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 512 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 514 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter numberOfMACs */

/* 516 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 518 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 520 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 522 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 524 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 526 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure EnumerateMACAddresses */

/* 528 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 530 */	NdrFcLong( 0x0 ),	/* 0 */
/* 534 */	NdrFcShort( 0x10 ),	/* 16 */
/* 536 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 538 */	NdrFcShort( 0x60 ),	/* 96 */
/* 540 */	NdrFcShort( 0x24 ),	/* 36 */
/* 542 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x4,		/* 4 */
/* 544 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 546 */	NdrFcShort( 0x1 ),	/* 1 */
/* 548 */	NdrFcShort( 0x0 ),	/* 0 */
/* 550 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 552 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 554 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 556 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter numberOfMacs */

/* 558 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 560 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 562 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter arrayOfMACAddresses */

/* 564 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 566 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 568 */	NdrFcShort( 0x98 ),	/* Type Offset=152 */

	/* Return value */

/* 570 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 572 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 574 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure VirtualMachineManagerVersion */

/* 576 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 578 */	NdrFcLong( 0x0 ),	/* 0 */
/* 582 */	NdrFcShort( 0x11 ),	/* 17 */
/* 584 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 586 */	NdrFcShort( 0x0 ),	/* 0 */
/* 588 */	NdrFcShort( 0x24 ),	/* 36 */
/* 590 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 592 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 594 */	NdrFcShort( 0x0 ),	/* 0 */
/* 596 */	NdrFcShort( 0x0 ),	/* 0 */
/* 598 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter version */

/* 600 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 602 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 604 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 606 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 608 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 610 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetDebuggerInterface */

/* 612 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 614 */	NdrFcLong( 0x0 ),	/* 0 */
/* 618 */	NdrFcShort( 0x12 ),	/* 18 */
/* 620 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 622 */	NdrFcShort( 0x44 ),	/* 68 */
/* 624 */	NdrFcShort( 0x8 ),	/* 8 */
/* 626 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x3,		/* 3 */
/* 628 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 630 */	NdrFcShort( 0x0 ),	/* 0 */
/* 632 */	NdrFcShort( 0x0 ),	/* 0 */
/* 634 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 636 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 638 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 640 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter ppDebugger */

/* 642 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 644 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 646 */	NdrFcShort( 0xa4 ),	/* Type Offset=164 */

	/* Return value */

/* 648 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 650 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 652 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Create */

/* 654 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 656 */	NdrFcLong( 0x0 ),	/* 0 */
/* 660 */	NdrFcShort( 0x3 ),	/* 3 */
/* 662 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 664 */	NdrFcShort( 0x4c ),	/* 76 */
/* 666 */	NdrFcShort( 0x24 ),	/* 36 */
/* 668 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x4,		/* 4 */
/* 670 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 672 */	NdrFcShort( 0x0 ),	/* 0 */
/* 674 */	NdrFcShort( 0x0 ),	/* 0 */
/* 676 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineID */

/* 678 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 680 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 682 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Parameter dmaChannel */

/* 684 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 686 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 688 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter transportID */

/* 690 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 692 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 694 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 696 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 698 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 700 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Delete */

/* 702 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 704 */	NdrFcLong( 0x0 ),	/* 0 */
/* 708 */	NdrFcShort( 0x4 ),	/* 4 */
/* 710 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 712 */	NdrFcShort( 0x8 ),	/* 8 */
/* 714 */	NdrFcShort( 0x8 ),	/* 8 */
/* 716 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 718 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 720 */	NdrFcShort( 0x0 ),	/* 0 */
/* 722 */	NdrFcShort( 0x0 ),	/* 0 */
/* 724 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter transportID */

/* 726 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 728 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 730 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 732 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 734 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 736 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Send */

/* 738 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 740 */	NdrFcLong( 0x0 ),	/* 0 */
/* 744 */	NdrFcShort( 0x5 ),	/* 5 */
/* 746 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 748 */	NdrFcShort( 0xe ),	/* 14 */
/* 750 */	NdrFcShort( 0x8 ),	/* 8 */
/* 752 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 754 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 756 */	NdrFcShort( 0x0 ),	/* 0 */
/* 758 */	NdrFcShort( 0x1 ),	/* 1 */
/* 760 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter transportID */

/* 762 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 764 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 766 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dataBuffer */

/* 768 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 770 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 772 */	NdrFcShort( 0xbe ),	/* Type Offset=190 */

	/* Parameter byteCount */

/* 774 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 776 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 778 */	0x6,		/* FC_SHORT */
			0x0,		/* 0 */

	/* Return value */

/* 780 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 782 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 784 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Receive */

/* 786 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 788 */	NdrFcLong( 0x0 ),	/* 0 */
/* 792 */	NdrFcShort( 0x6 ),	/* 6 */
/* 794 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 796 */	NdrFcShort( 0x2a ),	/* 42 */
/* 798 */	NdrFcShort( 0x22 ),	/* 34 */
/* 800 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x5,		/* 5 */
/* 802 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 804 */	NdrFcShort( 0x1 ),	/* 1 */
/* 806 */	NdrFcShort( 0x0 ),	/* 0 */
/* 808 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter transportID */

/* 810 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 812 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 814 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dataBuffer */

/* 816 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 818 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 820 */	NdrFcShort( 0xce ),	/* Type Offset=206 */

	/* Parameter byteCount */

/* 822 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 824 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 826 */	0x6,		/* FC_SHORT */
			0x0,		/* 0 */

	/* Parameter Timeout */

/* 828 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 830 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 832 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 834 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 836 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 838 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure SetVirtualMachineIDForTransport */

/* 840 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 842 */	NdrFcLong( 0x0 ),	/* 0 */
/* 846 */	NdrFcShort( 0x7 ),	/* 7 */
/* 848 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 850 */	NdrFcShort( 0x4c ),	/* 76 */
/* 852 */	NdrFcShort( 0x8 ),	/* 8 */
/* 854 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 856 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 858 */	NdrFcShort( 0x0 ),	/* 0 */
/* 860 */	NdrFcShort( 0x0 ),	/* 0 */
/* 862 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter transportID */

/* 864 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 866 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 868 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter virtualMachineID */

/* 870 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 872 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 874 */	NdrFcShort( 0x10 ),	/* Type Offset=16 */

	/* Return value */

/* 876 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 878 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 880 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Send */

/* 882 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 884 */	NdrFcLong( 0x0 ),	/* 0 */
/* 888 */	NdrFcShort( 0x3 ),	/* 3 */
/* 890 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 892 */	NdrFcShort( 0x6 ),	/* 6 */
/* 894 */	NdrFcShort( 0x8 ),	/* 8 */
/* 896 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 898 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 900 */	NdrFcShort( 0x0 ),	/* 0 */
/* 902 */	NdrFcShort( 0x1 ),	/* 1 */
/* 904 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dataBuffer */

/* 906 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 908 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 910 */	NdrFcShort( 0xe8 ),	/* Type Offset=232 */

	/* Parameter byteCount */

/* 912 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 914 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 916 */	0x6,		/* FC_SHORT */
			0x0,		/* 0 */

	/* Return value */

/* 918 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 920 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 922 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Receive */

/* 924 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 926 */	NdrFcLong( 0x0 ),	/* 0 */
/* 930 */	NdrFcShort( 0x4 ),	/* 4 */
/* 932 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 934 */	NdrFcShort( 0x22 ),	/* 34 */
/* 936 */	NdrFcShort( 0x22 ),	/* 34 */
/* 938 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x4,		/* 4 */
/* 940 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 942 */	NdrFcShort( 0x1 ),	/* 1 */
/* 944 */	NdrFcShort( 0x0 ),	/* 0 */
/* 946 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dataBuffer */

/* 948 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 950 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 952 */	NdrFcShort( 0xf8 ),	/* Type Offset=248 */

	/* Parameter byteCount */

/* 954 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 956 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 958 */	0x6,		/* FC_SHORT */
			0x0,		/* 0 */

	/* Parameter Timeout */

/* 960 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 962 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 964 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 966 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 968 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 970 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure HaltCallback */

/* 972 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 974 */	NdrFcLong( 0x0 ),	/* 0 */
/* 978 */	NdrFcShort( 0x3 ),	/* 3 */
/* 980 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 982 */	NdrFcShort( 0x26 ),	/* 38 */
/* 984 */	NdrFcShort( 0x8 ),	/* 8 */
/* 986 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x5,		/* 5 */
/* 988 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 990 */	NdrFcShort( 0x0 ),	/* 0 */
/* 992 */	NdrFcShort( 0x0 ),	/* 0 */
/* 994 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter HaltReason */

/* 996 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 998 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1000 */	0xd,		/* FC_ENUM16 */
			0x0,		/* 0 */

	/* Parameter Code */

/* 1002 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1004 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1006 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter Address */

/* 1008 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1010 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1012 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1014 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1016 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1018 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1020 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1022 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1024 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ContinueExecution */

/* 1026 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1028 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1032 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1034 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1036 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1038 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1040 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x1,		/* 1 */
/* 1042 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1044 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1046 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1048 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Return value */

/* 1050 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1052 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1054 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ContinueWithSingleStep */

/* 1056 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1058 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1062 */	NdrFcShort( 0x5 ),	/* 5 */
/* 1064 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1066 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1068 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1070 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 1072 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1074 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1076 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1078 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dwNumberOfSteps */

/* 1080 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1082 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1084 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1086 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1088 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1090 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1092 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1094 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1096 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Halt */

/* 1098 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1100 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1104 */	NdrFcShort( 0x6 ),	/* 6 */
/* 1106 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1108 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1110 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1112 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x1,		/* 1 */
/* 1114 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1116 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1118 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1120 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Return value */

/* 1122 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1124 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1126 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RegisterHaltNotification */

/* 1128 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1130 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1134 */	NdrFcShort( 0x7 ),	/* 7 */
/* 1136 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1138 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1140 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1142 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 1144 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1146 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1148 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1150 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter pSink */

/* 1152 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1154 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1156 */	NdrFcShort( 0x10a ),	/* Type Offset=266 */

	/* Parameter pdwNotificationCookie */

/* 1158 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1160 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1162 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1164 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1166 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1168 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure UnregisterHaltNotification */

/* 1170 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1172 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1176 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1178 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1180 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1182 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1184 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 1186 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1188 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1190 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1192 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dwNotificationCookie */

/* 1194 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1196 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1198 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1200 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1202 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1204 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ReadVirtualMemory */

/* 1206 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1208 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1212 */	NdrFcShort( 0x9 ),	/* 9 */
/* 1214 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 1216 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1218 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1220 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x6,		/* 6 */
/* 1222 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1224 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1226 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1228 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter Address */

/* 1230 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1232 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1234 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter NumBytesToRead */

/* 1236 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1238 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1240 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1242 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1244 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1246 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pbReadBuffer */

/* 1248 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1250 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1252 */	NdrFcShort( 0x120 ),	/* Type Offset=288 */

	/* Parameter pNumBytesActuallyRead */

/* 1254 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1256 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1258 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1260 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1262 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1264 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure WriteVirtualMemory */

/* 1266 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1268 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1272 */	NdrFcShort( 0xa ),	/* 10 */
/* 1274 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 1276 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1278 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1280 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1282 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1284 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1286 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1288 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter Address */

/* 1290 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1292 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1294 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter NumBytesToWrite */

/* 1296 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1298 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1300 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1302 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1304 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1306 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pbWriteBuffer */

/* 1308 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1310 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1312 */	NdrFcShort( 0x120 ),	/* Type Offset=288 */

	/* Parameter pNumBytesActuallyWritten */

/* 1314 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1316 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1318 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1320 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1322 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1324 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ReadPhysicalMemory */

/* 1326 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1328 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1332 */	NdrFcShort( 0xb ),	/* 11 */
/* 1334 */	NdrFcShort( 0x24 ),	/* x86 Stack size/offset = 36 */
/* 1336 */	NdrFcShort( 0x25 ),	/* 37 */
/* 1338 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1340 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x7,		/* 7 */
/* 1342 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1344 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1346 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1348 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter Address */

/* 1350 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1352 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1354 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter fUseIOSpace */

/* 1356 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1358 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1360 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Parameter NumBytesToRead */

/* 1362 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1364 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1366 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1368 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1370 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1372 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pbReadBuffer */

/* 1374 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1376 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1378 */	NdrFcShort( 0x130 ),	/* Type Offset=304 */

	/* Parameter pNumBytesActuallyRead */

/* 1380 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1382 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1384 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1386 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1388 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 1390 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure WritePhysicalMemory */

/* 1392 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1398 */	NdrFcShort( 0xc ),	/* 12 */
/* 1400 */	NdrFcShort( 0x24 ),	/* x86 Stack size/offset = 36 */
/* 1402 */	NdrFcShort( 0x25 ),	/* 37 */
/* 1404 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1406 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x7,		/* 7 */
/* 1408 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1410 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1412 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1414 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter Address */

/* 1416 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1418 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1420 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter fUseIOSpace */

/* 1422 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1424 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1426 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Parameter NumBytesToWrite */

/* 1428 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1430 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1432 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1434 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1436 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1438 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pbWriteBuffer */

/* 1440 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1442 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1444 */	NdrFcShort( 0x130 ),	/* Type Offset=304 */

	/* Parameter pNumBytesActuallyWritten */

/* 1446 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1448 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1450 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1452 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1454 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 1456 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure AddCodeBreakpoint */

/* 1458 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1460 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1464 */	NdrFcShort( 0xd ),	/* 13 */
/* 1466 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1468 */	NdrFcShort( 0x1d ),	/* 29 */
/* 1470 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1472 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x5,		/* 5 */
/* 1474 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1476 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1478 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1480 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter Address */

/* 1482 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1484 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1486 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter fIsVirtual */

/* 1488 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1490 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1492 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Parameter dwBypassCount */

/* 1494 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1496 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1498 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwBreakpointCookie */

/* 1500 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1502 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1504 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1506 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1508 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1510 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure SetBreakpointState */

/* 1512 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1514 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1518 */	NdrFcShort( 0xe ),	/* 14 */
/* 1520 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1522 */	NdrFcShort( 0xd ),	/* 13 */
/* 1524 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1526 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 1528 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1530 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1532 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1534 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dwBreakpointCookie */

/* 1536 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1538 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1540 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter fResetBypassCount */

/* 1542 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1544 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1546 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 1548 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1550 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1552 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetBreakpointState */

/* 1554 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1556 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1560 */	NdrFcShort( 0xf ),	/* 15 */
/* 1562 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1564 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1566 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1568 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 1570 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1572 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1574 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1576 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dwBreakpointCookie */

/* 1578 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1580 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1582 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwBypassedOccurrences */

/* 1584 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1586 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1588 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1590 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1592 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1594 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure DeleteBreakpoint */

/* 1596 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1598 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1602 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1604 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1606 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1608 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1610 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 1612 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1614 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1618 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dwBreakpointCookie */

/* 1620 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1622 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1624 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1626 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1628 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1630 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetContext */

/* 1632 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1634 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1638 */	NdrFcShort( 0x11 ),	/* 17 */
/* 1640 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1642 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1644 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1646 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 1648 */	0x8,		/* 8 */
			0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1650 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1652 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1654 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter pbContext */

/* 1656 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1658 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1660 */	NdrFcShort( 0x140 ),	/* Type Offset=320 */

	/* Parameter dwContextSize */

/* 1662 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1664 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1666 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1668 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1670 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1672 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1674 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1676 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1678 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure SetContext */

/* 1680 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1682 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1686 */	NdrFcShort( 0x12 ),	/* 18 */
/* 1688 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1690 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1692 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1694 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 1696 */	0x8,		/* 8 */
			0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1698 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1700 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1702 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter pbContext */

/* 1704 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1706 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1708 */	NdrFcShort( 0x140 ),	/* Type Offset=320 */

	/* Parameter dwContextSize */

/* 1710 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1712 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1714 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwCpuNum */

/* 1716 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1718 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1720 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1722 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1724 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1726 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure BringVirtualMachineToFront */

/* 1728 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1730 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1734 */	NdrFcShort( 0x9 ),	/* 9 */
/* 1736 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1738 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1740 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1742 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x1,		/* 1 */
/* 1744 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1746 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1748 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1750 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Return value */

/* 1752 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1754 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1756 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ResetVirtualMachine */

/* 1758 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1760 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1764 */	NdrFcShort( 0xa ),	/* 10 */
/* 1766 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1768 */	NdrFcShort( 0x5 ),	/* 5 */
/* 1770 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1772 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 1774 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1776 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1778 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1780 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hardReset */

/* 1782 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1784 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1786 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 1788 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1790 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1792 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ShutdownVirtualMachine */

/* 1794 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1796 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1800 */	NdrFcShort( 0xb ),	/* 11 */
/* 1802 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1804 */	NdrFcShort( 0x5 ),	/* 5 */
/* 1806 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1808 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 1810 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1812 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1814 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1816 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter saveMachine */

/* 1818 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1820 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1822 */	0x3,		/* FC_SMALL */
			0x0,		/* 0 */

	/* Return value */

/* 1824 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1826 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1828 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure BindToDMAChannel */

/* 1830 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1832 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1836 */	NdrFcShort( 0xc ),	/* 12 */
/* 1838 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1840 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1842 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1844 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x3,		/* 3 */
/* 1846 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1848 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1850 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1852 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter dmaChannel */

/* 1854 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1856 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1858 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter ppDMAChannel */

/* 1860 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 1862 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1864 */	NdrFcShort( 0x14c ),	/* Type Offset=332 */

	/* Return value */

/* 1866 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1868 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1870 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetVirtualMachineName */

/* 1872 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1874 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1878 */	NdrFcShort( 0xd ),	/* 13 */
/* 1880 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1882 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1884 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1886 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x2,		/* 2 */
/* 1888 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1892 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1894 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineName */

/* 1896 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 1898 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1900 */	NdrFcShort( 0x38 ),	/* Type Offset=56 */

	/* Return value */

/* 1902 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1904 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1906 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure SetVirtualMachineName */

/* 1908 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1910 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1914 */	NdrFcShort( 0xe ),	/* 14 */
/* 1916 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1918 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1920 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1922 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x2,		/* 2 */
/* 1924 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1926 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1928 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1930 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter virtualMachineName */

/* 1932 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1934 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1936 */	NdrFcShort( 0x36 ),	/* Type Offset=54 */

	/* Return value */

/* 1938 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1940 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1942 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetMACAddressCount */

/* 1944 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1946 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1950 */	NdrFcShort( 0xf ),	/* 15 */
/* 1952 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1954 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1956 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1958 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 1960 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1962 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1964 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1966 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter numberOfMACs */

/* 1968 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1970 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1972 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 1974 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1976 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1978 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure EnumerateMACAddresses */

/* 1980 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 1982 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1986 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1988 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1990 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1992 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1994 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x3,		/* 3 */
/* 1996 */	0x8,		/* 8 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1998 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2000 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2002 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter numberOfMacs */

/* 2004 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 2006 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2008 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter arrayOfMACAddresses */

/* 2010 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 2012 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2014 */	NdrFcShort( 0x162 ),	/* Type Offset=354 */

	/* Return value */

/* 2016 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2018 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2020 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure ConfigureDevice */

/* 2022 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 2024 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2028 */	NdrFcShort( 0x11 ),	/* 17 */
/* 2030 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2032 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2034 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2036 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 2038 */	0x8,		/* 8 */
			0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 2040 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2042 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2044 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hwndParent */

/* 2046 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 2048 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2050 */	NdrFcShort( 0x58 ),	/* Type Offset=88 */

	/* Parameter lcidParent */

/* 2052 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2054 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2056 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter bstrConfig */

/* 2058 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 2060 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2062 */	NdrFcShort( 0x7c ),	/* Type Offset=124 */

	/* Parameter pbstrConfig */

/* 2064 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 2066 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2068 */	NdrFcShort( 0x8e ),	/* Type Offset=142 */

	/* Return value */

/* 2070 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2072 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2074 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure GetDebuggerInterface */

/* 2076 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 2078 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2082 */	NdrFcShort( 0x12 ),	/* 18 */
/* 2084 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2086 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2088 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2090 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x2,		/* 2 */
/* 2092 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 2094 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2098 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter ppDebugger */

/* 2100 */	NdrFcShort( 0x13 ),	/* Flags:  must size, must free, out, */
/* 2102 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2104 */	NdrFcShort( 0xa4 ),	/* Type Offset=164 */

	/* Return value */

/* 2106 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2108 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2110 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const MIDL_TYPE_FORMAT_STRING __MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/*  4 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/*  6 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/*  8 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 10 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x8 ),	/* 8 */
/* 14 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 16 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 18 */	NdrFcShort( 0x10 ),	/* 16 */
/* 20 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 22 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 24 */	0x0,		/* 0 */
			NdrFcShort( 0xfff1 ),	/* Offset= -15 (10) */
			0x5b,		/* FC_END */
/* 28 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 30 */	NdrFcShort( 0x10 ),	/* 16 */
/* 32 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 34 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 36 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 38 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 40 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (16) */
/* 42 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 44 */	
			0x11, 0x0,	/* FC_RP */
/* 46 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (16) */
/* 48 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 50 */	0x3,		/* FC_SMALL */
			0x5c,		/* FC_PAD */
/* 52 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 54 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 56 */	
			0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 58 */	NdrFcShort( 0x2 ),	/* Offset= 2 (60) */
/* 60 */	
			0x13, 0x8,	/* FC_OP [simple_pointer] */
/* 62 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 64 */	
			0x12, 0x0,	/* FC_UP */
/* 66 */	NdrFcShort( 0x2 ),	/* Offset= 2 (68) */
/* 68 */	
			0x2a,		/* FC_ENCAPSULATED_UNION */
			0x48,		/* 72 */
/* 70 */	NdrFcShort( 0x4 ),	/* 4 */
/* 72 */	NdrFcShort( 0x2 ),	/* 2 */
/* 74 */	NdrFcLong( 0x48746457 ),	/* 1215587415 */
/* 78 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 80 */	NdrFcLong( 0x52746457 ),	/* 1383359575 */
/* 84 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 86 */	NdrFcShort( 0xffff ),	/* Offset= -1 (85) */
/* 88 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 90 */	NdrFcShort( 0x0 ),	/* 0 */
/* 92 */	NdrFcShort( 0x4 ),	/* 4 */
/* 94 */	NdrFcShort( 0x0 ),	/* 0 */
/* 96 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (64) */
/* 98 */	
			0x12, 0x0,	/* FC_UP */
/* 100 */	NdrFcShort( 0xe ),	/* Offset= 14 (114) */
/* 102 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 104 */	NdrFcShort( 0x2 ),	/* 2 */
/* 106 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 108 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 110 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 112 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 114 */	
			0x17,		/* FC_CSTRUCT */
			0x3,		/* 3 */
/* 116 */	NdrFcShort( 0x8 ),	/* 8 */
/* 118 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (102) */
/* 120 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 122 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 124 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 126 */	NdrFcShort( 0x1 ),	/* 1 */
/* 128 */	NdrFcShort( 0x4 ),	/* 4 */
/* 130 */	NdrFcShort( 0x0 ),	/* 0 */
/* 132 */	NdrFcShort( 0xffde ),	/* Offset= -34 (98) */
/* 134 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 136 */	NdrFcShort( 0x6 ),	/* Offset= 6 (142) */
/* 138 */	
			0x13, 0x0,	/* FC_OP */
/* 140 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (114) */
/* 142 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 144 */	NdrFcShort( 0x1 ),	/* 1 */
/* 146 */	NdrFcShort( 0x4 ),	/* 4 */
/* 148 */	NdrFcShort( 0x0 ),	/* 0 */
/* 150 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (138) */
/* 152 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 154 */	NdrFcShort( 0x1 ),	/* 1 */
/* 156 */	0x20,		/* Corr desc:  parameter,  */
			0x59,		/* FC_CALLBACK */
/* 158 */	NdrFcShort( 0x0 ),	/* 0 */
/* 160 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 162 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 164 */	
			0x11, 0x10,	/* FC_RP [pointer_deref] */
/* 166 */	NdrFcShort( 0x2 ),	/* Offset= 2 (168) */
/* 168 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 170 */	NdrFcLong( 0x1b48cad4 ),	/* 457755348 */
/* 174 */	NdrFcShort( 0xd013 ),	/* -12269 */
/* 176 */	NdrFcShort( 0x4b98 ),	/* 19352 */
/* 178 */	0xb5,		/* 181 */
			0x5,		/* 5 */
/* 180 */	0x76,		/* 118 */
			0x16,		/* 22 */
/* 182 */	0x2f,		/* 47 */
			0x9,		/* 9 */
/* 184 */	0xe8,		/* 232 */
			0xe9,		/* 233 */
/* 186 */	
			0x11, 0x0,	/* FC_RP */
/* 188 */	NdrFcShort( 0x2 ),	/* Offset= 2 (190) */
/* 190 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 192 */	NdrFcShort( 0x1 ),	/* 1 */
/* 194 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x0,		/*  */
/* 196 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 198 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 200 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 202 */	
			0x11, 0x0,	/* FC_RP */
/* 204 */	NdrFcShort( 0x2 ),	/* Offset= 2 (206) */
/* 206 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 208 */	NdrFcShort( 0x1 ),	/* 1 */
/* 210 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x54,		/* FC_DEREFERENCE */
/* 212 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 214 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 216 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x54,		/* FC_DEREFERENCE */
/* 218 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 220 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 222 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 224 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 226 */	0x6,		/* FC_SHORT */
			0x5c,		/* FC_PAD */
/* 228 */	
			0x11, 0x0,	/* FC_RP */
/* 230 */	NdrFcShort( 0x2 ),	/* Offset= 2 (232) */
/* 232 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 234 */	NdrFcShort( 0x1 ),	/* 1 */
/* 236 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x0,		/*  */
/* 238 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 240 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 242 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 244 */	
			0x11, 0x0,	/* FC_RP */
/* 246 */	NdrFcShort( 0x2 ),	/* Offset= 2 (248) */
/* 248 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 250 */	NdrFcShort( 0x1 ),	/* 1 */
/* 252 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x54,		/* FC_DEREFERENCE */
/* 254 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 256 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 258 */	0x27,		/* Corr desc:  parameter, FC_USHORT */
			0x54,		/* FC_DEREFERENCE */
/* 260 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 262 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 264 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 266 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 268 */	NdrFcLong( 0x4de85c9d ),	/* 1307073693 */
/* 272 */	NdrFcShort( 0x818c ),	/* -32372 */
/* 274 */	NdrFcShort( 0x40d8 ),	/* 16600 */
/* 276 */	0xbc,		/* 188 */
			0xa1,		/* 161 */
/* 278 */	0xbf,		/* 191 */
			0x23,		/* 35 */
/* 280 */	0x4,		/* 4 */
			0x5a,		/* 90 */
/* 282 */	0xcb,		/* 203 */
			0x6e,		/* 110 */
/* 284 */	
			0x11, 0x0,	/* FC_RP */
/* 286 */	NdrFcShort( 0x2 ),	/* Offset= 2 (288) */
/* 288 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 290 */	NdrFcShort( 0x1 ),	/* 1 */
/* 292 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 294 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 296 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 298 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 300 */	
			0x11, 0x0,	/* FC_RP */
/* 302 */	NdrFcShort( 0x2 ),	/* Offset= 2 (304) */
/* 304 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 306 */	NdrFcShort( 0x1 ),	/* 1 */
/* 308 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 310 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 312 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 314 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 316 */	
			0x11, 0x0,	/* FC_RP */
/* 318 */	NdrFcShort( 0x2 ),	/* Offset= 2 (320) */
/* 320 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 322 */	NdrFcShort( 0x1 ),	/* 1 */
/* 324 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 326 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 328 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 330 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 332 */	
			0x11, 0x10,	/* FC_RP [pointer_deref] */
/* 334 */	NdrFcShort( 0x2 ),	/* Offset= 2 (336) */
/* 336 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 338 */	NdrFcLong( 0x1679a8ae ),	/* 377071790 */
/* 342 */	NdrFcShort( 0xe814 ),	/* -6124 */
/* 344 */	NdrFcShort( 0x4b04 ),	/* 19204 */
/* 346 */	0x88,		/* 136 */
			0x1b,		/* 27 */
/* 348 */	0x98,		/* 152 */
			0xdf,		/* 223 */
/* 350 */	0x9a,		/* 154 */
			0xfb,		/* 251 */
/* 352 */	0xa4,		/* 164 */
			0xdf,		/* 223 */
/* 354 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 356 */	NdrFcShort( 0x1 ),	/* 1 */
/* 358 */	0x20,		/* Corr desc:  parameter,  */
			0x59,		/* FC_CALLBACK */
/* 360 */	NdrFcShort( 0x1 ),	/* 1 */
/* 362 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 364 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */

			0x0
        }
    };

static const USER_MARSHAL_ROUTINE_QUADRUPLE UserMarshalRoutines[ WIRE_MARSHAL_TABLE_SIZE ] = 
        {
            
            {
            HWND_UserSize
            ,HWND_UserMarshal
            ,HWND_UserUnmarshal
            ,HWND_UserFree
            },
            {
            BSTR_UserSize
            ,BSTR_UserMarshal
            ,BSTR_UserUnmarshal
            ,BSTR_UserFree
            }

        };


static void __RPC_USER IDeviceEmulatorItem_EnumerateMACAddressesExprEval_0001( PMIDL_STUB_MESSAGE pStubMsg )
{
    #pragma pack(4)
    struct _PARAM_STRUCT
        {
        IDeviceEmulatorItem *This;
        ULONG *numberOfMacs;
        BYTE ( *arrayOfMACAddresses )[  ];
        HRESULT _RetVal;
        };
    #pragma pack()
    struct _PARAM_STRUCT *pS	=	( struct _PARAM_STRUCT * )pStubMsg->StackTop;
    
    pStubMsg->Offset = 0;
    pStubMsg->MaxCount = ( unsigned long ) ( *pS->numberOfMacs * 6 );
}

static void __RPC_USER IDeviceEmulatorVirtualMachineManager_EnumerateMACAddressesExprEval_0000( PMIDL_STUB_MESSAGE pStubMsg )
{
    #pragma pack(4)
    struct _PARAM_STRUCT
        {
        IDeviceEmulatorItem *This;
        GUID *virtualMachineID;
        ULONG *numberOfMacs;
        BYTE ( *arrayOfMACAddresses )[  ];
        HRESULT _RetVal;
        };
    #pragma pack()
    struct _PARAM_STRUCT *pS	=	( struct _PARAM_STRUCT * )pStubMsg->StackTop;
    
    pStubMsg->Offset = 0;
    pStubMsg->MaxCount = ( unsigned long ) ( *pS->numberOfMacs * 6 );
}

static const EXPR_EVAL ExprEvalRoutines[] = 
    {
    IDeviceEmulatorVirtualMachineManager_EnumerateMACAddressesExprEval_0000
    ,IDeviceEmulatorItem_EnumerateMACAddressesExprEval_0001
    };



/* Standard interface: __MIDL_itf_DEComInterfaces_0000, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}} */


/* Object interface: IUnknown, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IDeviceEmulatorVirtualMachineManager, ver. 0.0,
   GUID={0x4bd3464d,0x8f1a,0x48a0,{0x8d,0xc9,0xd4,0x9d,0xb8,0x61,0xf6,0xc4}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorVirtualMachineManager_FormatStringOffsetTable[] =
    {
    0,
    36,
    78,
    120,
    162,
    198,
    240,
    276,
    312,
    354,
    396,
    432,
    486,
    528,
    576,
    612
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorVirtualMachineManager_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorVirtualMachineManager_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorVirtualMachineManager_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorVirtualMachineManager_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(19) _IDeviceEmulatorVirtualMachineManagerProxyVtbl = 
{
    &IDeviceEmulatorVirtualMachineManager_ProxyInfo,
    &IID_IDeviceEmulatorVirtualMachineManager,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::GetVirtualMachineCount */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::EnumerateVirtualMachines */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::IsVirtualMachineRunning */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::ResetVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::CreateVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::ShutdownVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::RestoreVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::DeleteVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::GetVirtualMachineName */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::SetVirtualMachineName */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::BringVirtualMachineToFront */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::ConfigureDevice */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::GetMACAddressCount */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::EnumerateMACAddresses */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::VirtualMachineManagerVersion */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualMachineManager::GetDebuggerInterface */
};

const CInterfaceStubVtbl _IDeviceEmulatorVirtualMachineManagerStubVtbl =
{
    &IID_IDeviceEmulatorVirtualMachineManager,
    &IDeviceEmulatorVirtualMachineManager_ServerInfo,
    19,
    0, /* pure interpreted */
    CStdStubBuffer_METHODS
};


/* Object interface: IDeviceEmulatorVirtualTransport, ver. 0.0,
   GUID={0x39583a47,0x3f35,0x4469,{0xa5,0x34,0xcb,0x78,0x4e,0x16,0x33,0x05}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorVirtualTransport_FormatStringOffsetTable[] =
    {
    654,
    702,
    738,
    786,
    840
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorVirtualTransport_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorVirtualTransport_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorVirtualTransport_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorVirtualTransport_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(8) _IDeviceEmulatorVirtualTransportProxyVtbl = 
{
    &IDeviceEmulatorVirtualTransport_ProxyInfo,
    &IID_IDeviceEmulatorVirtualTransport,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualTransport::Create */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualTransport::Delete */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualTransport::Send */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualTransport::Receive */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorVirtualTransport::SetVirtualMachineIDForTransport */
};

const CInterfaceStubVtbl _IDeviceEmulatorVirtualTransportStubVtbl =
{
    &IID_IDeviceEmulatorVirtualTransport,
    &IDeviceEmulatorVirtualTransport_ServerInfo,
    8,
    0, /* pure interpreted */
    CStdStubBuffer_METHODS
};


/* Object interface: IDeviceEmulatorDMAChannel, ver. 0.0,
   GUID={0x1679a8ae,0xe814,0x4b04,{0x88,0x1b,0x98,0xdf,0x9a,0xfb,0xa4,0xdf}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorDMAChannel_FormatStringOffsetTable[] =
    {
    882,
    924
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDMAChannel_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDMAChannel_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorDMAChannel_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDMAChannel_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(5) _IDeviceEmulatorDMAChannelProxyVtbl = 
{
    &IDeviceEmulatorDMAChannel_ProxyInfo,
    &IID_IDeviceEmulatorDMAChannel,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDMAChannel::Send */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDMAChannel::Receive */
};

const CInterfaceStubVtbl _IDeviceEmulatorDMAChannelStubVtbl =
{
    &IID_IDeviceEmulatorDMAChannel,
    &IDeviceEmulatorDMAChannel_ServerInfo,
    5,
    0, /* pure interpreted */
    CStdStubBuffer_METHODS
};


/* Standard interface: __MIDL_itf_DEComInterfaces_0264, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}} */


/* Object interface: IDeviceEmulatorDebuggerHaltNotificationSink, ver. 0.0,
   GUID={0x4de85c9d,0x818c,0x40d8,{0xbc,0xa1,0xbf,0x23,0x04,0x5a,0xcb,0x6e}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorDebuggerHaltNotificationSink_FormatStringOffsetTable[] =
    {
    972
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDebuggerHaltNotificationSink_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDebuggerHaltNotificationSink_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorDebuggerHaltNotificationSink_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDebuggerHaltNotificationSink_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(4) _IDeviceEmulatorDebuggerHaltNotificationSinkProxyVtbl = 
{
    &IDeviceEmulatorDebuggerHaltNotificationSink_ProxyInfo,
    &IID_IDeviceEmulatorDebuggerHaltNotificationSink,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebuggerHaltNotificationSink::HaltCallback */
};

const CInterfaceStubVtbl _IDeviceEmulatorDebuggerHaltNotificationSinkStubVtbl =
{
    &IID_IDeviceEmulatorDebuggerHaltNotificationSink,
    &IDeviceEmulatorDebuggerHaltNotificationSink_ServerInfo,
    4,
    0, /* pure interpreted */
    CStdStubBuffer_METHODS
};


/* Object interface: IDeviceEmulatorDebugger, ver. 0.0,
   GUID={0x1b48cad4,0xd013,0x4b98,{0xb5,0x05,0x76,0x16,0x2f,0x09,0xe8,0xe9}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorDebugger_FormatStringOffsetTable[] =
    {
    0,
    1026,
    1056,
    1098,
    1128,
    1170,
    1206,
    1266,
    1326,
    1392,
    1458,
    1512,
    1554,
    1596,
    1632,
    1680
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorDebugger_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDebugger_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorDebugger_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorDebugger_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(19) _IDeviceEmulatorDebuggerProxyVtbl = 
{
    &IDeviceEmulatorDebugger_ProxyInfo,
    &IID_IDeviceEmulatorDebugger,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::GetProcessorFamily */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::ContinueExecution */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::ContinueWithSingleStep */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::Halt */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::RegisterHaltNotification */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::UnregisterHaltNotification */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::ReadVirtualMemory */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::WriteVirtualMemory */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::ReadPhysicalMemory */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::WritePhysicalMemory */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::AddCodeBreakpoint */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::SetBreakpointState */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::GetBreakpointState */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::DeleteBreakpoint */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::GetContext */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorDebugger::SetContext */
};

const CInterfaceStubVtbl _IDeviceEmulatorDebuggerStubVtbl =
{
    &IID_IDeviceEmulatorDebugger,
    &IDeviceEmulatorDebugger_ServerInfo,
    19,
    0, /* pure interpreted */
    CStdStubBuffer_METHODS
};


/* Object interface: IParseDisplayName, ver. 0.0,
   GUID={0x0000011a,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IOleContainer, ver. 0.0,
   GUID={0x0000011b,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IOleItemContainer, ver. 0.0,
   GUID={0x0000011c,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IDeviceEmulatorItem, ver. 0.0,
   GUID={0x9c06bd4c,0x12b3,0x4991,{0xa5,0x12,0x8c,0x48,0x84,0xec,0x8b,0xb5}} */

#pragma code_seg(".orpc")
static const unsigned short IDeviceEmulatorItem_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    1728,
    1758,
    1794,
    1830,
    1872,
    1908,
    1944,
    1980,
    2022,
    2076
    };

static const MIDL_STUBLESS_PROXY_INFO IDeviceEmulatorItem_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorItem_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO IDeviceEmulatorItem_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
    &IDeviceEmulatorItem_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(19) _IDeviceEmulatorItemProxyVtbl = 
{
    &IDeviceEmulatorItem_ProxyInfo,
    &IID_IDeviceEmulatorItem,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* (void *) (INT_PTR) -1 /* IParseDisplayName::ParseDisplayName */ ,
    0 /* (void *) (INT_PTR) -1 /* IOleContainer::EnumObjects */ ,
    0 /* (void *) (INT_PTR) -1 /* IOleContainer::LockContainer */ ,
    0 /* (void *) (INT_PTR) -1 /* IOleItemContainer::GetObject */ ,
    0 /* (void *) (INT_PTR) -1 /* IOleItemContainer::GetObjectStorage */ ,
    0 /* (void *) (INT_PTR) -1 /* IOleItemContainer::IsRunning */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::BringVirtualMachineToFront */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::ResetVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::ShutdownVirtualMachine */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::BindToDMAChannel */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::GetVirtualMachineName */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::SetVirtualMachineName */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::GetMACAddressCount */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::EnumerateMACAddresses */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::ConfigureDevice */ ,
    (void *) (INT_PTR) -1 /* IDeviceEmulatorItem::GetDebuggerInterface */
};


static const PRPC_STUB_FUNCTION IDeviceEmulatorItem_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2
};

CInterfaceStubVtbl _IDeviceEmulatorItemStubVtbl =
{
    &IID_IDeviceEmulatorItem,
    &IDeviceEmulatorItem_ServerInfo,
    19,
    &IDeviceEmulatorItem_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};

static const MIDL_STUB_DESC Object_StubDesc = 
    {
    0,
    NdrOleAllocate,
    NdrOleFree,
    0,
    0,
    0,
    ExprEvalRoutines,
    0,
    __MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x50002, /* Ndr library version */
    0,
    0x600016e, /* MIDL Version 6.0.366 */
    0,
    UserMarshalRoutines,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0   /* Reserved5 */
    };

const CInterfaceProxyVtbl * _DEComInterfaces_ProxyVtblList[] = 
{
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorVirtualTransportProxyVtbl,
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorItemProxyVtbl,
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorVirtualMachineManagerProxyVtbl,
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorDebuggerHaltNotificationSinkProxyVtbl,
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorDMAChannelProxyVtbl,
    ( CInterfaceProxyVtbl *) &_IDeviceEmulatorDebuggerProxyVtbl,
    0
};

const CInterfaceStubVtbl * _DEComInterfaces_StubVtblList[] = 
{
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorVirtualTransportStubVtbl,
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorItemStubVtbl,
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorVirtualMachineManagerStubVtbl,
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorDebuggerHaltNotificationSinkStubVtbl,
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorDMAChannelStubVtbl,
    ( CInterfaceStubVtbl *) &_IDeviceEmulatorDebuggerStubVtbl,
    0
};

PCInterfaceName const _DEComInterfaces_InterfaceNamesList[] = 
{
    "IDeviceEmulatorVirtualTransport",
    "IDeviceEmulatorItem",
    "IDeviceEmulatorVirtualMachineManager",
    "IDeviceEmulatorDebuggerHaltNotificationSink",
    "IDeviceEmulatorDMAChannel",
    "IDeviceEmulatorDebugger",
    0
};

const IID *  _DEComInterfaces_BaseIIDList[] = 
{
    0,
    &IID_IOleItemContainer,
    0,
    0,
    0,
    0,
    0
};


#define _DEComInterfaces_CHECK_IID(n)	IID_GENERIC_CHECK_IID( _DEComInterfaces, pIID, n)

int __stdcall _DEComInterfaces_IID_Lookup( const IID * pIID, int * pIndex )
{
    IID_BS_LOOKUP_SETUP

    IID_BS_LOOKUP_INITIAL_TEST( _DEComInterfaces, 6, 4 )
    IID_BS_LOOKUP_NEXT_TEST( _DEComInterfaces, 2 )
    IID_BS_LOOKUP_NEXT_TEST( _DEComInterfaces, 1 )
    IID_BS_LOOKUP_RETURN_RESULT( _DEComInterfaces, 6, *pIndex )
    
}

const ExtendedProxyFileInfo DEComInterfaces_ProxyFileInfo = 
{
    (PCInterfaceProxyVtblList *) & _DEComInterfaces_ProxyVtblList,
    (PCInterfaceStubVtblList *) & _DEComInterfaces_StubVtblList,
    (const PCInterfaceName * ) & _DEComInterfaces_InterfaceNamesList,
    (const IID ** ) & _DEComInterfaces_BaseIIDList,
    & _DEComInterfaces_IID_Lookup, 
    6,
    2,
    0, /* table of [async_uuid] interfaces */
    0, /* Filler1 */
    0, /* Filler2 */
    0  /* Filler3 */
};
#pragma optimize("", on )
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* !defined(_M_IA64) && !defined(_M_AMD64)*/

