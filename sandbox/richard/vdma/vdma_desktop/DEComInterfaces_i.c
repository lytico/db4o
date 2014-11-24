

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


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

#pragma warning( disable: 4049 )  /* more than 64k source lines */


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorVirtualMachineManager,0x4bd3464d,0x8f1a,0x48a0,0x8d,0xc9,0xd4,0x9d,0xb8,0x61,0xf6,0xc4);


MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorVirtualTransport,0x39583a47,0x3f35,0x4469,0xa5,0x34,0xcb,0x78,0x4e,0x16,0x33,0x05);


MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorDMAChannel,0x1679a8ae,0xe814,0x4b04,0x88,0x1b,0x98,0xdf,0x9a,0xfb,0xa4,0xdf);


MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorDebuggerHaltNotificationSink,0x4de85c9d,0x818c,0x40d8,0xbc,0xa1,0xbf,0x23,0x04,0x5a,0xcb,0x6e);


MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorDebugger,0x1b48cad4,0xd013,0x4b98,0xb5,0x05,0x76,0x16,0x2f,0x09,0xe8,0xe9);


MIDL_DEFINE_GUID(IID, IID_IDeviceEmulatorItem,0x9c06bd4c,0x12b3,0x4991,0xa5,0x12,0x8c,0x48,0x84,0xec,0x8b,0xb5);


MIDL_DEFINE_GUID(IID, LIBID_DeviceEmulator,0xfd932356,0xc9b8,0x495c,0x98,0x03,0x69,0xd0,0xd4,0xa2,0x9c,0xa9);


MIDL_DEFINE_GUID(CLSID, CLSID_DeviceEmulatorVirtualMachineManager,0x063e2de8,0xaa5b,0x46e8,0x82,0x39,0xb8,0xf7,0xca,0x43,0xf4,0xc7);


MIDL_DEFINE_GUID(CLSID, CLSID_DeviceEmulatorVirtualTransport,0x8703b814,0xb436,0x4d99,0xb1,0x26,0xce,0x5d,0xf3,0x02,0x73,0x0f);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



