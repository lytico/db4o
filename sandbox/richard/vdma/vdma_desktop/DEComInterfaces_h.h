

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


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


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __DEComInterfaces_h_h__
#define __DEComInterfaces_h_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IDeviceEmulatorVirtualMachineManager_FWD_DEFINED__
#define __IDeviceEmulatorVirtualMachineManager_FWD_DEFINED__
typedef interface IDeviceEmulatorVirtualMachineManager IDeviceEmulatorVirtualMachineManager;
#endif 	/* __IDeviceEmulatorVirtualMachineManager_FWD_DEFINED__ */


#ifndef __IDeviceEmulatorVirtualTransport_FWD_DEFINED__
#define __IDeviceEmulatorVirtualTransport_FWD_DEFINED__
typedef interface IDeviceEmulatorVirtualTransport IDeviceEmulatorVirtualTransport;
#endif 	/* __IDeviceEmulatorVirtualTransport_FWD_DEFINED__ */


#ifndef __IDeviceEmulatorDMAChannel_FWD_DEFINED__
#define __IDeviceEmulatorDMAChannel_FWD_DEFINED__
typedef interface IDeviceEmulatorDMAChannel IDeviceEmulatorDMAChannel;
#endif 	/* __IDeviceEmulatorDMAChannel_FWD_DEFINED__ */


#ifndef __IDeviceEmulatorDebuggerHaltNotificationSink_FWD_DEFINED__
#define __IDeviceEmulatorDebuggerHaltNotificationSink_FWD_DEFINED__
typedef interface IDeviceEmulatorDebuggerHaltNotificationSink IDeviceEmulatorDebuggerHaltNotificationSink;
#endif 	/* __IDeviceEmulatorDebuggerHaltNotificationSink_FWD_DEFINED__ */


#ifndef __IDeviceEmulatorDebugger_FWD_DEFINED__
#define __IDeviceEmulatorDebugger_FWD_DEFINED__
typedef interface IDeviceEmulatorDebugger IDeviceEmulatorDebugger;
#endif 	/* __IDeviceEmulatorDebugger_FWD_DEFINED__ */


#ifndef __IDeviceEmulatorItem_FWD_DEFINED__
#define __IDeviceEmulatorItem_FWD_DEFINED__
typedef interface IDeviceEmulatorItem IDeviceEmulatorItem;
#endif 	/* __IDeviceEmulatorItem_FWD_DEFINED__ */


#ifndef __DeviceEmulatorVirtualMachineManager_FWD_DEFINED__
#define __DeviceEmulatorVirtualMachineManager_FWD_DEFINED__

#ifdef __cplusplus
typedef class DeviceEmulatorVirtualMachineManager DeviceEmulatorVirtualMachineManager;
#else
typedef struct DeviceEmulatorVirtualMachineManager DeviceEmulatorVirtualMachineManager;
#endif /* __cplusplus */

#endif 	/* __DeviceEmulatorVirtualMachineManager_FWD_DEFINED__ */


#ifndef __DeviceEmulatorVirtualTransport_FWD_DEFINED__
#define __DeviceEmulatorVirtualTransport_FWD_DEFINED__

#ifdef __cplusplus
typedef class DeviceEmulatorVirtualTransport DeviceEmulatorVirtualTransport;
#else
typedef struct DeviceEmulatorVirtualTransport DeviceEmulatorVirtualTransport;
#endif /* __cplusplus */

#endif 	/* __DeviceEmulatorVirtualTransport_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

/* interface __MIDL_itf_DEComInterfaces_0000 */
/* [local] */ 

const ULONG kDETypeLibrary_MajorVersion = 1;
const ULONG kDETypeLibrary_MinorVersion = 0;



extern RPC_IF_HANDLE __MIDL_itf_DEComInterfaces_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_DEComInterfaces_0000_v0_0_s_ifspec;

#ifndef __IDeviceEmulatorVirtualMachineManager_INTERFACE_DEFINED__
#define __IDeviceEmulatorVirtualMachineManager_INTERFACE_DEFINED__

/* interface IDeviceEmulatorVirtualMachineManager */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorVirtualMachineManager;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4bd3464d-8f1a-48a0-8dc9-d49db861f6c4")
    IDeviceEmulatorVirtualMachineManager : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVirtualMachineCount( 
            /* [retval][out] */ ULONG *numberOfVMs) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE EnumerateVirtualMachines( 
            /* [out][in] */ ULONG *numberOfVMs,
            /* [size_is][out] */ GUID virtualMachineID[  ]) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE IsVirtualMachineRunning( 
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ boolean *isRunning) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ResetVirtualMachine( 
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ boolean hardReset) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE CreateVirtualMachine( 
            /* [in] */ LPOLESTR commandLine) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ShutdownVirtualMachine( 
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ boolean saveMachine) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RestoreVirtualMachine( 
            /* [in] */ GUID *virtualMachineID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE DeleteVirtualMachine( 
            /* [in] */ GUID *virtualMachineID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVirtualMachineName( 
            /* [in] */ GUID *virtualMachineID,
            /* [out] */ LPOLESTR *virtualMachineName) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetVirtualMachineName( 
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ LPOLESTR virtualMachineName) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE BringVirtualMachineToFront( 
            /* [in] */ GUID *virtualMachineID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ConfigureDevice( 
            /* [in] */ HWND hwndParent,
            /* [in] */ LCID lcidParent,
            /* [in] */ BSTR bstrConfig,
            /* [out] */ BSTR *pbstrConfig) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMACAddressCount( 
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ ULONG *numberOfMACs) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE EnumerateMACAddresses( 
            /* [in] */ GUID *virtualMachineID,
            /* [out][in] */ ULONG *numberOfMacs,
            /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE VirtualMachineManagerVersion( 
            /* [out] */ DWORD *version) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetDebuggerInterface( 
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorVirtualMachineManagerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorVirtualMachineManager * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorVirtualMachineManager * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVirtualMachineCount )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [retval][out] */ ULONG *numberOfVMs);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *EnumerateVirtualMachines )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [out][in] */ ULONG *numberOfVMs,
            /* [size_is][out] */ GUID virtualMachineID[  ]);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *IsVirtualMachineRunning )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ boolean *isRunning);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ResetVirtualMachine )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ boolean hardReset);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *CreateVirtualMachine )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ LPOLESTR commandLine);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ShutdownVirtualMachine )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ boolean saveMachine);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *RestoreVirtualMachine )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *DeleteVirtualMachine )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVirtualMachineName )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [out] */ LPOLESTR *virtualMachineName);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetVirtualMachineName )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ LPOLESTR virtualMachineName);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *BringVirtualMachineToFront )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ConfigureDevice )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ HWND hwndParent,
            /* [in] */ LCID lcidParent,
            /* [in] */ BSTR bstrConfig,
            /* [out] */ BSTR *pbstrConfig);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetMACAddressCount )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ ULONG *numberOfMACs);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *EnumerateMACAddresses )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [out][in] */ ULONG *numberOfMacs,
            /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]);
        
        HRESULT ( STDMETHODCALLTYPE *VirtualMachineManagerVersion )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [out] */ DWORD *version);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetDebuggerInterface )( 
            IDeviceEmulatorVirtualMachineManager * This,
            /* [in] */ GUID *virtualMachineID,
            /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger);
        
        END_INTERFACE
    } IDeviceEmulatorVirtualMachineManagerVtbl;

    interface IDeviceEmulatorVirtualMachineManager
    {
        CONST_VTBL struct IDeviceEmulatorVirtualMachineManagerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorVirtualMachineManager_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorVirtualMachineManager_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorVirtualMachineManager_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorVirtualMachineManager_GetVirtualMachineCount(This,numberOfVMs)	\
    (This)->lpVtbl -> GetVirtualMachineCount(This,numberOfVMs)

#define IDeviceEmulatorVirtualMachineManager_EnumerateVirtualMachines(This,numberOfVMs,virtualMachineID)	\
    (This)->lpVtbl -> EnumerateVirtualMachines(This,numberOfVMs,virtualMachineID)

#define IDeviceEmulatorVirtualMachineManager_IsVirtualMachineRunning(This,virtualMachineID,isRunning)	\
    (This)->lpVtbl -> IsVirtualMachineRunning(This,virtualMachineID,isRunning)

#define IDeviceEmulatorVirtualMachineManager_ResetVirtualMachine(This,virtualMachineID,hardReset)	\
    (This)->lpVtbl -> ResetVirtualMachine(This,virtualMachineID,hardReset)

#define IDeviceEmulatorVirtualMachineManager_CreateVirtualMachine(This,commandLine)	\
    (This)->lpVtbl -> CreateVirtualMachine(This,commandLine)

#define IDeviceEmulatorVirtualMachineManager_ShutdownVirtualMachine(This,virtualMachineID,saveMachine)	\
    (This)->lpVtbl -> ShutdownVirtualMachine(This,virtualMachineID,saveMachine)

#define IDeviceEmulatorVirtualMachineManager_RestoreVirtualMachine(This,virtualMachineID)	\
    (This)->lpVtbl -> RestoreVirtualMachine(This,virtualMachineID)

#define IDeviceEmulatorVirtualMachineManager_DeleteVirtualMachine(This,virtualMachineID)	\
    (This)->lpVtbl -> DeleteVirtualMachine(This,virtualMachineID)

#define IDeviceEmulatorVirtualMachineManager_GetVirtualMachineName(This,virtualMachineID,virtualMachineName)	\
    (This)->lpVtbl -> GetVirtualMachineName(This,virtualMachineID,virtualMachineName)

#define IDeviceEmulatorVirtualMachineManager_SetVirtualMachineName(This,virtualMachineID,virtualMachineName)	\
    (This)->lpVtbl -> SetVirtualMachineName(This,virtualMachineID,virtualMachineName)

#define IDeviceEmulatorVirtualMachineManager_BringVirtualMachineToFront(This,virtualMachineID)	\
    (This)->lpVtbl -> BringVirtualMachineToFront(This,virtualMachineID)

#define IDeviceEmulatorVirtualMachineManager_ConfigureDevice(This,hwndParent,lcidParent,bstrConfig,pbstrConfig)	\
    (This)->lpVtbl -> ConfigureDevice(This,hwndParent,lcidParent,bstrConfig,pbstrConfig)

#define IDeviceEmulatorVirtualMachineManager_GetMACAddressCount(This,virtualMachineID,numberOfMACs)	\
    (This)->lpVtbl -> GetMACAddressCount(This,virtualMachineID,numberOfMACs)

#define IDeviceEmulatorVirtualMachineManager_EnumerateMACAddresses(This,virtualMachineID,numberOfMacs,arrayOfMACAddresses)	\
    (This)->lpVtbl -> EnumerateMACAddresses(This,virtualMachineID,numberOfMacs,arrayOfMACAddresses)

#define IDeviceEmulatorVirtualMachineManager_VirtualMachineManagerVersion(This,version)	\
    (This)->lpVtbl -> VirtualMachineManagerVersion(This,version)

#define IDeviceEmulatorVirtualMachineManager_GetDebuggerInterface(This,virtualMachineID,ppDebugger)	\
    (This)->lpVtbl -> GetDebuggerInterface(This,virtualMachineID,ppDebugger)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_GetVirtualMachineCount_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [retval][out] */ ULONG *numberOfVMs);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_GetVirtualMachineCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_EnumerateVirtualMachines_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [out][in] */ ULONG *numberOfVMs,
    /* [size_is][out] */ GUID virtualMachineID[  ]);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_EnumerateVirtualMachines_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_IsVirtualMachineRunning_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [retval][out] */ boolean *isRunning);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_IsVirtualMachineRunning_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_ResetVirtualMachine_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [in] */ boolean hardReset);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_ResetVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_CreateVirtualMachine_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ LPOLESTR commandLine);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_CreateVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_ShutdownVirtualMachine_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [in] */ boolean saveMachine);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_ShutdownVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_RestoreVirtualMachine_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_RestoreVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_DeleteVirtualMachine_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_DeleteVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_GetVirtualMachineName_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [out] */ LPOLESTR *virtualMachineName);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_GetVirtualMachineName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_SetVirtualMachineName_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [in] */ LPOLESTR virtualMachineName);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_SetVirtualMachineName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_BringVirtualMachineToFront_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_BringVirtualMachineToFront_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_ConfigureDevice_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ HWND hwndParent,
    /* [in] */ LCID lcidParent,
    /* [in] */ BSTR bstrConfig,
    /* [out] */ BSTR *pbstrConfig);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_ConfigureDevice_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_GetMACAddressCount_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [retval][out] */ ULONG *numberOfMACs);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_GetMACAddressCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_EnumerateMACAddresses_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [out][in] */ ULONG *numberOfMacs,
    /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_EnumerateMACAddresses_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_VirtualMachineManagerVersion_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [out] */ DWORD *version);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_VirtualMachineManagerVersion_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualMachineManager_GetDebuggerInterface_Proxy( 
    IDeviceEmulatorVirtualMachineManager * This,
    /* [in] */ GUID *virtualMachineID,
    /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger);


void __RPC_STUB IDeviceEmulatorVirtualMachineManager_GetDebuggerInterface_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorVirtualMachineManager_INTERFACE_DEFINED__ */


#ifndef __IDeviceEmulatorVirtualTransport_INTERFACE_DEFINED__
#define __IDeviceEmulatorVirtualTransport_INTERFACE_DEFINED__

/* interface IDeviceEmulatorVirtualTransport */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorVirtualTransport;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("39583a47-3f35-4469-a534-cb784e163305")
    IDeviceEmulatorVirtualTransport : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Create( 
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ ULONG dmaChannel,
            /* [out] */ ULONG *transportID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Delete( 
            /* [in] */ ULONG transportID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Send( 
            /* [in] */ ULONG transportID,
            /* [size_is][in] */ const BYTE *dataBuffer,
            /* [in] */ USHORT byteCount) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Receive( 
            /* [in] */ ULONG transportID,
            /* [length_is][size_is][out] */ BYTE *dataBuffer,
            /* [out][in] */ USHORT *byteCount,
            /* [in] */ ULONG Timeout) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetVirtualMachineIDForTransport( 
            /* [in] */ ULONG transportID,
            /* [in] */ GUID *virtualMachineID) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorVirtualTransportVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorVirtualTransport * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorVirtualTransport * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Create )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ GUID *virtualMachineID,
            /* [in] */ ULONG dmaChannel,
            /* [out] */ ULONG *transportID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Delete )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ ULONG transportID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Send )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ ULONG transportID,
            /* [size_is][in] */ const BYTE *dataBuffer,
            /* [in] */ USHORT byteCount);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Receive )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ ULONG transportID,
            /* [length_is][size_is][out] */ BYTE *dataBuffer,
            /* [out][in] */ USHORT *byteCount,
            /* [in] */ ULONG Timeout);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetVirtualMachineIDForTransport )( 
            IDeviceEmulatorVirtualTransport * This,
            /* [in] */ ULONG transportID,
            /* [in] */ GUID *virtualMachineID);
        
        END_INTERFACE
    } IDeviceEmulatorVirtualTransportVtbl;

    interface IDeviceEmulatorVirtualTransport
    {
        CONST_VTBL struct IDeviceEmulatorVirtualTransportVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorVirtualTransport_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorVirtualTransport_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorVirtualTransport_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorVirtualTransport_Create(This,virtualMachineID,dmaChannel,transportID)	\
    (This)->lpVtbl -> Create(This,virtualMachineID,dmaChannel,transportID)

#define IDeviceEmulatorVirtualTransport_Delete(This,transportID)	\
    (This)->lpVtbl -> Delete(This,transportID)

#define IDeviceEmulatorVirtualTransport_Send(This,transportID,dataBuffer,byteCount)	\
    (This)->lpVtbl -> Send(This,transportID,dataBuffer,byteCount)

#define IDeviceEmulatorVirtualTransport_Receive(This,transportID,dataBuffer,byteCount,Timeout)	\
    (This)->lpVtbl -> Receive(This,transportID,dataBuffer,byteCount,Timeout)

#define IDeviceEmulatorVirtualTransport_SetVirtualMachineIDForTransport(This,transportID,virtualMachineID)	\
    (This)->lpVtbl -> SetVirtualMachineIDForTransport(This,transportID,virtualMachineID)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualTransport_Create_Proxy( 
    IDeviceEmulatorVirtualTransport * This,
    /* [in] */ GUID *virtualMachineID,
    /* [in] */ ULONG dmaChannel,
    /* [out] */ ULONG *transportID);


void __RPC_STUB IDeviceEmulatorVirtualTransport_Create_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualTransport_Delete_Proxy( 
    IDeviceEmulatorVirtualTransport * This,
    /* [in] */ ULONG transportID);


void __RPC_STUB IDeviceEmulatorVirtualTransport_Delete_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualTransport_Send_Proxy( 
    IDeviceEmulatorVirtualTransport * This,
    /* [in] */ ULONG transportID,
    /* [size_is][in] */ const BYTE *dataBuffer,
    /* [in] */ USHORT byteCount);


void __RPC_STUB IDeviceEmulatorVirtualTransport_Send_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualTransport_Receive_Proxy( 
    IDeviceEmulatorVirtualTransport * This,
    /* [in] */ ULONG transportID,
    /* [length_is][size_is][out] */ BYTE *dataBuffer,
    /* [out][in] */ USHORT *byteCount,
    /* [in] */ ULONG Timeout);


void __RPC_STUB IDeviceEmulatorVirtualTransport_Receive_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorVirtualTransport_SetVirtualMachineIDForTransport_Proxy( 
    IDeviceEmulatorVirtualTransport * This,
    /* [in] */ ULONG transportID,
    /* [in] */ GUID *virtualMachineID);


void __RPC_STUB IDeviceEmulatorVirtualTransport_SetVirtualMachineIDForTransport_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorVirtualTransport_INTERFACE_DEFINED__ */


#ifndef __IDeviceEmulatorDMAChannel_INTERFACE_DEFINED__
#define __IDeviceEmulatorDMAChannel_INTERFACE_DEFINED__

/* interface IDeviceEmulatorDMAChannel */
/* [hidden][unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorDMAChannel;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1679a8ae-e814-4b04-881b-98df9afba4df")
    IDeviceEmulatorDMAChannel : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Send( 
            /* [size_is][in] */ const BYTE *dataBuffer,
            /* [in] */ USHORT byteCount) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Receive( 
            /* [length_is][size_is][out] */ BYTE *dataBuffer,
            /* [out][in] */ USHORT *byteCount,
            /* [in] */ ULONG Timeout) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorDMAChannelVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorDMAChannel * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorDMAChannel * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorDMAChannel * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Send )( 
            IDeviceEmulatorDMAChannel * This,
            /* [size_is][in] */ const BYTE *dataBuffer,
            /* [in] */ USHORT byteCount);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Receive )( 
            IDeviceEmulatorDMAChannel * This,
            /* [length_is][size_is][out] */ BYTE *dataBuffer,
            /* [out][in] */ USHORT *byteCount,
            /* [in] */ ULONG Timeout);
        
        END_INTERFACE
    } IDeviceEmulatorDMAChannelVtbl;

    interface IDeviceEmulatorDMAChannel
    {
        CONST_VTBL struct IDeviceEmulatorDMAChannelVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorDMAChannel_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorDMAChannel_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorDMAChannel_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorDMAChannel_Send(This,dataBuffer,byteCount)	\
    (This)->lpVtbl -> Send(This,dataBuffer,byteCount)

#define IDeviceEmulatorDMAChannel_Receive(This,dataBuffer,byteCount,Timeout)	\
    (This)->lpVtbl -> Receive(This,dataBuffer,byteCount,Timeout)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorDMAChannel_Send_Proxy( 
    IDeviceEmulatorDMAChannel * This,
    /* [size_is][in] */ const BYTE *dataBuffer,
    /* [in] */ USHORT byteCount);


void __RPC_STUB IDeviceEmulatorDMAChannel_Send_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorDMAChannel_Receive_Proxy( 
    IDeviceEmulatorDMAChannel * This,
    /* [length_is][size_is][out] */ BYTE *dataBuffer,
    /* [out][in] */ USHORT *byteCount,
    /* [in] */ ULONG Timeout);


void __RPC_STUB IDeviceEmulatorDMAChannel_Receive_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorDMAChannel_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_DEComInterfaces_0264 */
/* [local] */ 

typedef 
enum _PDEVICEEMULATOR_HALT_REASON_TYPE
    {	haltreasonNone	= 0,
	haltreasonUser	= haltreasonNone + 1,
	haltreasonException	= haltreasonUser + 1,
	haltreasonBp	= haltreasonException + 1,
	haltreasonStep	= haltreasonBp + 1,
	haltreasonUnknown	= haltreasonStep + 1
    } 	DEVICEEMULATOR_HALT_REASON_TYPE;



extern RPC_IF_HANDLE __MIDL_itf_DEComInterfaces_0264_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_DEComInterfaces_0264_v0_0_s_ifspec;

#ifndef __IDeviceEmulatorDebuggerHaltNotificationSink_INTERFACE_DEFINED__
#define __IDeviceEmulatorDebuggerHaltNotificationSink_INTERFACE_DEFINED__

/* interface IDeviceEmulatorDebuggerHaltNotificationSink */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorDebuggerHaltNotificationSink;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4de85c9d-818c-40d8-bca1-bf23045acb6e")
    IDeviceEmulatorDebuggerHaltNotificationSink : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE HaltCallback( 
            /* [in] */ DEVICEEMULATOR_HALT_REASON_TYPE HaltReason,
            /* [in] */ DWORD Code,
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD dwCpuNum) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorDebuggerHaltNotificationSinkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorDebuggerHaltNotificationSink * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorDebuggerHaltNotificationSink * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorDebuggerHaltNotificationSink * This);
        
        HRESULT ( STDMETHODCALLTYPE *HaltCallback )( 
            IDeviceEmulatorDebuggerHaltNotificationSink * This,
            /* [in] */ DEVICEEMULATOR_HALT_REASON_TYPE HaltReason,
            /* [in] */ DWORD Code,
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD dwCpuNum);
        
        END_INTERFACE
    } IDeviceEmulatorDebuggerHaltNotificationSinkVtbl;

    interface IDeviceEmulatorDebuggerHaltNotificationSink
    {
        CONST_VTBL struct IDeviceEmulatorDebuggerHaltNotificationSinkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorDebuggerHaltNotificationSink_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorDebuggerHaltNotificationSink_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorDebuggerHaltNotificationSink_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorDebuggerHaltNotificationSink_HaltCallback(This,HaltReason,Code,Address,dwCpuNum)	\
    (This)->lpVtbl -> HaltCallback(This,HaltReason,Code,Address,dwCpuNum)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebuggerHaltNotificationSink_HaltCallback_Proxy( 
    IDeviceEmulatorDebuggerHaltNotificationSink * This,
    /* [in] */ DEVICEEMULATOR_HALT_REASON_TYPE HaltReason,
    /* [in] */ DWORD Code,
    /* [in] */ DWORD64 Address,
    /* [in] */ DWORD dwCpuNum);


void __RPC_STUB IDeviceEmulatorDebuggerHaltNotificationSink_HaltCallback_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorDebuggerHaltNotificationSink_INTERFACE_DEFINED__ */


#ifndef __IDeviceEmulatorDebugger_INTERFACE_DEFINED__
#define __IDeviceEmulatorDebugger_INTERFACE_DEFINED__

/* interface IDeviceEmulatorDebugger */
/* [hidden][unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorDebugger;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1b48cad4-d013-4b98-b505-76162f09e8e9")
    IDeviceEmulatorDebugger : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE GetProcessorFamily( 
            /* [out] */ DWORD *pdwProcessorFamily) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ContinueExecution( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ContinueWithSingleStep( 
            DWORD dwNumberOfSteps,
            /* [in] */ DWORD dwCpuNum) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Halt( void) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE RegisterHaltNotification( 
            /* [in] */ IDeviceEmulatorDebuggerHaltNotificationSink *pSink,
            /* [out] */ DWORD *pdwNotificationCookie) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE UnregisterHaltNotification( 
            /* [in] */ DWORD dwNotificationCookie) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ReadVirtualMemory( 
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD NumBytesToRead,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][out] */ BYTE *pbReadBuffer,
            /* [out] */ DWORD *pNumBytesActuallyRead) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE WriteVirtualMemory( 
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD NumBytesToWrite,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][in] */ const BYTE *pbWriteBuffer,
            /* [out] */ DWORD *pNumBytesActuallyWritten) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ReadPhysicalMemory( 
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fUseIOSpace,
            /* [in] */ DWORD NumBytesToRead,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][out] */ BYTE *pbReadBuffer,
            /* [out] */ DWORD *pNumBytesActuallyRead) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE WritePhysicalMemory( 
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fUseIOSpace,
            /* [in] */ DWORD NumBytesToWrite,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][in] */ const BYTE *pbWriteBuffer,
            /* [out] */ DWORD *pNumBytesActuallyWritten) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AddCodeBreakpoint( 
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fIsVirtual,
            /* [in] */ DWORD dwBypassCount,
            /* [out] */ DWORD *pdwBreakpointCookie) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetBreakpointState( 
            /* [in] */ DWORD dwBreakpointCookie,
            /* [in] */ boolean fResetBypassCount) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetBreakpointState( 
            /* [in] */ DWORD dwBreakpointCookie,
            /* [out] */ DWORD *pdwBypassedOccurrences) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE DeleteBreakpoint( 
            /* [in] */ DWORD dwBreakpointCookie) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetContext( 
            /* [size_is][out][in] */ BYTE *pbContext,
            /* [in] */ DWORD dwContextSize,
            /* [in] */ DWORD dwCpuNum) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SetContext( 
            /* [size_is][out][in] */ BYTE *pbContext,
            /* [in] */ DWORD dwContextSize,
            /* [in] */ DWORD dwCpuNum) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorDebuggerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorDebugger * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorDebugger * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetProcessorFamily )( 
            IDeviceEmulatorDebugger * This,
            /* [out] */ DWORD *pdwProcessorFamily);
        
        HRESULT ( STDMETHODCALLTYPE *ContinueExecution )( 
            IDeviceEmulatorDebugger * This);
        
        HRESULT ( STDMETHODCALLTYPE *ContinueWithSingleStep )( 
            IDeviceEmulatorDebugger * This,
            DWORD dwNumberOfSteps,
            /* [in] */ DWORD dwCpuNum);
        
        HRESULT ( STDMETHODCALLTYPE *Halt )( 
            IDeviceEmulatorDebugger * This);
        
        HRESULT ( STDMETHODCALLTYPE *RegisterHaltNotification )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ IDeviceEmulatorDebuggerHaltNotificationSink *pSink,
            /* [out] */ DWORD *pdwNotificationCookie);
        
        HRESULT ( STDMETHODCALLTYPE *UnregisterHaltNotification )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD dwNotificationCookie);
        
        HRESULT ( STDMETHODCALLTYPE *ReadVirtualMemory )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD NumBytesToRead,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][out] */ BYTE *pbReadBuffer,
            /* [out] */ DWORD *pNumBytesActuallyRead);
        
        HRESULT ( STDMETHODCALLTYPE *WriteVirtualMemory )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD64 Address,
            /* [in] */ DWORD NumBytesToWrite,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][in] */ const BYTE *pbWriteBuffer,
            /* [out] */ DWORD *pNumBytesActuallyWritten);
        
        HRESULT ( STDMETHODCALLTYPE *ReadPhysicalMemory )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fUseIOSpace,
            /* [in] */ DWORD NumBytesToRead,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][out] */ BYTE *pbReadBuffer,
            /* [out] */ DWORD *pNumBytesActuallyRead);
        
        HRESULT ( STDMETHODCALLTYPE *WritePhysicalMemory )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fUseIOSpace,
            /* [in] */ DWORD NumBytesToWrite,
            /* [in] */ DWORD dwCpuNum,
            /* [size_is][in] */ const BYTE *pbWriteBuffer,
            /* [out] */ DWORD *pNumBytesActuallyWritten);
        
        HRESULT ( STDMETHODCALLTYPE *AddCodeBreakpoint )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD64 Address,
            /* [in] */ boolean fIsVirtual,
            /* [in] */ DWORD dwBypassCount,
            /* [out] */ DWORD *pdwBreakpointCookie);
        
        HRESULT ( STDMETHODCALLTYPE *SetBreakpointState )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD dwBreakpointCookie,
            /* [in] */ boolean fResetBypassCount);
        
        HRESULT ( STDMETHODCALLTYPE *GetBreakpointState )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD dwBreakpointCookie,
            /* [out] */ DWORD *pdwBypassedOccurrences);
        
        HRESULT ( STDMETHODCALLTYPE *DeleteBreakpoint )( 
            IDeviceEmulatorDebugger * This,
            /* [in] */ DWORD dwBreakpointCookie);
        
        HRESULT ( STDMETHODCALLTYPE *GetContext )( 
            IDeviceEmulatorDebugger * This,
            /* [size_is][out][in] */ BYTE *pbContext,
            /* [in] */ DWORD dwContextSize,
            /* [in] */ DWORD dwCpuNum);
        
        HRESULT ( STDMETHODCALLTYPE *SetContext )( 
            IDeviceEmulatorDebugger * This,
            /* [size_is][out][in] */ BYTE *pbContext,
            /* [in] */ DWORD dwContextSize,
            /* [in] */ DWORD dwCpuNum);
        
        END_INTERFACE
    } IDeviceEmulatorDebuggerVtbl;

    interface IDeviceEmulatorDebugger
    {
        CONST_VTBL struct IDeviceEmulatorDebuggerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorDebugger_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorDebugger_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorDebugger_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorDebugger_GetProcessorFamily(This,pdwProcessorFamily)	\
    (This)->lpVtbl -> GetProcessorFamily(This,pdwProcessorFamily)

#define IDeviceEmulatorDebugger_ContinueExecution(This)	\
    (This)->lpVtbl -> ContinueExecution(This)

#define IDeviceEmulatorDebugger_ContinueWithSingleStep(This,dwNumberOfSteps,dwCpuNum)	\
    (This)->lpVtbl -> ContinueWithSingleStep(This,dwNumberOfSteps,dwCpuNum)

#define IDeviceEmulatorDebugger_Halt(This)	\
    (This)->lpVtbl -> Halt(This)

#define IDeviceEmulatorDebugger_RegisterHaltNotification(This,pSink,pdwNotificationCookie)	\
    (This)->lpVtbl -> RegisterHaltNotification(This,pSink,pdwNotificationCookie)

#define IDeviceEmulatorDebugger_UnregisterHaltNotification(This,dwNotificationCookie)	\
    (This)->lpVtbl -> UnregisterHaltNotification(This,dwNotificationCookie)

#define IDeviceEmulatorDebugger_ReadVirtualMemory(This,Address,NumBytesToRead,dwCpuNum,pbReadBuffer,pNumBytesActuallyRead)	\
    (This)->lpVtbl -> ReadVirtualMemory(This,Address,NumBytesToRead,dwCpuNum,pbReadBuffer,pNumBytesActuallyRead)

#define IDeviceEmulatorDebugger_WriteVirtualMemory(This,Address,NumBytesToWrite,dwCpuNum,pbWriteBuffer,pNumBytesActuallyWritten)	\
    (This)->lpVtbl -> WriteVirtualMemory(This,Address,NumBytesToWrite,dwCpuNum,pbWriteBuffer,pNumBytesActuallyWritten)

#define IDeviceEmulatorDebugger_ReadPhysicalMemory(This,Address,fUseIOSpace,NumBytesToRead,dwCpuNum,pbReadBuffer,pNumBytesActuallyRead)	\
    (This)->lpVtbl -> ReadPhysicalMemory(This,Address,fUseIOSpace,NumBytesToRead,dwCpuNum,pbReadBuffer,pNumBytesActuallyRead)

#define IDeviceEmulatorDebugger_WritePhysicalMemory(This,Address,fUseIOSpace,NumBytesToWrite,dwCpuNum,pbWriteBuffer,pNumBytesActuallyWritten)	\
    (This)->lpVtbl -> WritePhysicalMemory(This,Address,fUseIOSpace,NumBytesToWrite,dwCpuNum,pbWriteBuffer,pNumBytesActuallyWritten)

#define IDeviceEmulatorDebugger_AddCodeBreakpoint(This,Address,fIsVirtual,dwBypassCount,pdwBreakpointCookie)	\
    (This)->lpVtbl -> AddCodeBreakpoint(This,Address,fIsVirtual,dwBypassCount,pdwBreakpointCookie)

#define IDeviceEmulatorDebugger_SetBreakpointState(This,dwBreakpointCookie,fResetBypassCount)	\
    (This)->lpVtbl -> SetBreakpointState(This,dwBreakpointCookie,fResetBypassCount)

#define IDeviceEmulatorDebugger_GetBreakpointState(This,dwBreakpointCookie,pdwBypassedOccurrences)	\
    (This)->lpVtbl -> GetBreakpointState(This,dwBreakpointCookie,pdwBypassedOccurrences)

#define IDeviceEmulatorDebugger_DeleteBreakpoint(This,dwBreakpointCookie)	\
    (This)->lpVtbl -> DeleteBreakpoint(This,dwBreakpointCookie)

#define IDeviceEmulatorDebugger_GetContext(This,pbContext,dwContextSize,dwCpuNum)	\
    (This)->lpVtbl -> GetContext(This,pbContext,dwContextSize,dwCpuNum)

#define IDeviceEmulatorDebugger_SetContext(This,pbContext,dwContextSize,dwCpuNum)	\
    (This)->lpVtbl -> SetContext(This,pbContext,dwContextSize,dwCpuNum)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_GetProcessorFamily_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [out] */ DWORD *pdwProcessorFamily);


void __RPC_STUB IDeviceEmulatorDebugger_GetProcessorFamily_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_ContinueExecution_Proxy( 
    IDeviceEmulatorDebugger * This);


void __RPC_STUB IDeviceEmulatorDebugger_ContinueExecution_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_ContinueWithSingleStep_Proxy( 
    IDeviceEmulatorDebugger * This,
    DWORD dwNumberOfSteps,
    /* [in] */ DWORD dwCpuNum);


void __RPC_STUB IDeviceEmulatorDebugger_ContinueWithSingleStep_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_Halt_Proxy( 
    IDeviceEmulatorDebugger * This);


void __RPC_STUB IDeviceEmulatorDebugger_Halt_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_RegisterHaltNotification_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ IDeviceEmulatorDebuggerHaltNotificationSink *pSink,
    /* [out] */ DWORD *pdwNotificationCookie);


void __RPC_STUB IDeviceEmulatorDebugger_RegisterHaltNotification_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_UnregisterHaltNotification_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD dwNotificationCookie);


void __RPC_STUB IDeviceEmulatorDebugger_UnregisterHaltNotification_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_ReadVirtualMemory_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD64 Address,
    /* [in] */ DWORD NumBytesToRead,
    /* [in] */ DWORD dwCpuNum,
    /* [size_is][out] */ BYTE *pbReadBuffer,
    /* [out] */ DWORD *pNumBytesActuallyRead);


void __RPC_STUB IDeviceEmulatorDebugger_ReadVirtualMemory_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_WriteVirtualMemory_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD64 Address,
    /* [in] */ DWORD NumBytesToWrite,
    /* [in] */ DWORD dwCpuNum,
    /* [size_is][in] */ const BYTE *pbWriteBuffer,
    /* [out] */ DWORD *pNumBytesActuallyWritten);


void __RPC_STUB IDeviceEmulatorDebugger_WriteVirtualMemory_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_ReadPhysicalMemory_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD64 Address,
    /* [in] */ boolean fUseIOSpace,
    /* [in] */ DWORD NumBytesToRead,
    /* [in] */ DWORD dwCpuNum,
    /* [size_is][out] */ BYTE *pbReadBuffer,
    /* [out] */ DWORD *pNumBytesActuallyRead);


void __RPC_STUB IDeviceEmulatorDebugger_ReadPhysicalMemory_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_WritePhysicalMemory_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD64 Address,
    /* [in] */ boolean fUseIOSpace,
    /* [in] */ DWORD NumBytesToWrite,
    /* [in] */ DWORD dwCpuNum,
    /* [size_is][in] */ const BYTE *pbWriteBuffer,
    /* [out] */ DWORD *pNumBytesActuallyWritten);


void __RPC_STUB IDeviceEmulatorDebugger_WritePhysicalMemory_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_AddCodeBreakpoint_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD64 Address,
    /* [in] */ boolean fIsVirtual,
    /* [in] */ DWORD dwBypassCount,
    /* [out] */ DWORD *pdwBreakpointCookie);


void __RPC_STUB IDeviceEmulatorDebugger_AddCodeBreakpoint_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_SetBreakpointState_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD dwBreakpointCookie,
    /* [in] */ boolean fResetBypassCount);


void __RPC_STUB IDeviceEmulatorDebugger_SetBreakpointState_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_GetBreakpointState_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD dwBreakpointCookie,
    /* [out] */ DWORD *pdwBypassedOccurrences);


void __RPC_STUB IDeviceEmulatorDebugger_GetBreakpointState_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_DeleteBreakpoint_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [in] */ DWORD dwBreakpointCookie);


void __RPC_STUB IDeviceEmulatorDebugger_DeleteBreakpoint_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_GetContext_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [size_is][out][in] */ BYTE *pbContext,
    /* [in] */ DWORD dwContextSize,
    /* [in] */ DWORD dwCpuNum);


void __RPC_STUB IDeviceEmulatorDebugger_GetContext_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


HRESULT STDMETHODCALLTYPE IDeviceEmulatorDebugger_SetContext_Proxy( 
    IDeviceEmulatorDebugger * This,
    /* [size_is][out][in] */ BYTE *pbContext,
    /* [in] */ DWORD dwContextSize,
    /* [in] */ DWORD dwCpuNum);


void __RPC_STUB IDeviceEmulatorDebugger_SetContext_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorDebugger_INTERFACE_DEFINED__ */


#ifndef __IDeviceEmulatorItem_INTERFACE_DEFINED__
#define __IDeviceEmulatorItem_INTERFACE_DEFINED__

/* interface IDeviceEmulatorItem */
/* [hidden][unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IDeviceEmulatorItem;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("9c06bd4c-12b3-4991-a512-8c4884ec8bb5")
    IDeviceEmulatorItem : public IOleItemContainer
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE BringVirtualMachineToFront( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ResetVirtualMachine( 
            /* [in] */ boolean hardReset) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ShutdownVirtualMachine( 
            /* [in] */ boolean saveMachine) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE BindToDMAChannel( 
            /* [in] */ ULONG dmaChannel,
            /* [retval][out] */ IDeviceEmulatorDMAChannel **ppDMAChannel) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVirtualMachineName( 
            /* [out] */ LPOLESTR *virtualMachineName) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetVirtualMachineName( 
            /* [in] */ LPOLESTR virtualMachineName) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMACAddressCount( 
            /* [retval][out] */ ULONG *numberOfMACs) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE EnumerateMACAddresses( 
            /* [out][in] */ ULONG *numberOfMacs,
            /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ConfigureDevice( 
            /* [in] */ HWND hwndParent,
            /* [in] */ LCID lcidParent,
            /* [in] */ BSTR bstrConfig,
            /* [out] */ BSTR *pbstrConfig) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetDebuggerInterface( 
            /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IDeviceEmulatorItemVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IDeviceEmulatorItem * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IDeviceEmulatorItem * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IDeviceEmulatorItem * This);
        
        HRESULT ( STDMETHODCALLTYPE *ParseDisplayName )( 
            IDeviceEmulatorItem * This,
            /* [unique][in] */ IBindCtx *pbc,
            /* [in] */ LPOLESTR pszDisplayName,
            /* [out] */ ULONG *pchEaten,
            /* [out] */ IMoniker **ppmkOut);
        
        HRESULT ( STDMETHODCALLTYPE *EnumObjects )( 
            IDeviceEmulatorItem * This,
            /* [in] */ DWORD grfFlags,
            /* [out] */ IEnumUnknown **ppenum);
        
        HRESULT ( STDMETHODCALLTYPE *LockContainer )( 
            IDeviceEmulatorItem * This,
            /* [in] */ BOOL fLock);
        
        HRESULT ( STDMETHODCALLTYPE *GetObject )( 
            IDeviceEmulatorItem * This,
            /* [in] */ LPOLESTR pszItem,
            /* [in] */ DWORD dwSpeedNeeded,
            /* [unique][in] */ IBindCtx *pbc,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        HRESULT ( STDMETHODCALLTYPE *GetObjectStorage )( 
            IDeviceEmulatorItem * This,
            /* [in] */ LPOLESTR pszItem,
            /* [unique][in] */ IBindCtx *pbc,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvStorage);
        
        HRESULT ( STDMETHODCALLTYPE *IsRunning )( 
            IDeviceEmulatorItem * This,
            /* [in] */ LPOLESTR pszItem);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *BringVirtualMachineToFront )( 
            IDeviceEmulatorItem * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ResetVirtualMachine )( 
            IDeviceEmulatorItem * This,
            /* [in] */ boolean hardReset);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ShutdownVirtualMachine )( 
            IDeviceEmulatorItem * This,
            /* [in] */ boolean saveMachine);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *BindToDMAChannel )( 
            IDeviceEmulatorItem * This,
            /* [in] */ ULONG dmaChannel,
            /* [retval][out] */ IDeviceEmulatorDMAChannel **ppDMAChannel);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVirtualMachineName )( 
            IDeviceEmulatorItem * This,
            /* [out] */ LPOLESTR *virtualMachineName);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetVirtualMachineName )( 
            IDeviceEmulatorItem * This,
            /* [in] */ LPOLESTR virtualMachineName);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetMACAddressCount )( 
            IDeviceEmulatorItem * This,
            /* [retval][out] */ ULONG *numberOfMACs);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *EnumerateMACAddresses )( 
            IDeviceEmulatorItem * This,
            /* [out][in] */ ULONG *numberOfMacs,
            /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ConfigureDevice )( 
            IDeviceEmulatorItem * This,
            /* [in] */ HWND hwndParent,
            /* [in] */ LCID lcidParent,
            /* [in] */ BSTR bstrConfig,
            /* [out] */ BSTR *pbstrConfig);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetDebuggerInterface )( 
            IDeviceEmulatorItem * This,
            /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger);
        
        END_INTERFACE
    } IDeviceEmulatorItemVtbl;

    interface IDeviceEmulatorItem
    {
        CONST_VTBL struct IDeviceEmulatorItemVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IDeviceEmulatorItem_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IDeviceEmulatorItem_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IDeviceEmulatorItem_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IDeviceEmulatorItem_ParseDisplayName(This,pbc,pszDisplayName,pchEaten,ppmkOut)	\
    (This)->lpVtbl -> ParseDisplayName(This,pbc,pszDisplayName,pchEaten,ppmkOut)


#define IDeviceEmulatorItem_EnumObjects(This,grfFlags,ppenum)	\
    (This)->lpVtbl -> EnumObjects(This,grfFlags,ppenum)

#define IDeviceEmulatorItem_LockContainer(This,fLock)	\
    (This)->lpVtbl -> LockContainer(This,fLock)


#define IDeviceEmulatorItem_GetObject(This,pszItem,dwSpeedNeeded,pbc,riid,ppvObject)	\
    (This)->lpVtbl -> GetObject(This,pszItem,dwSpeedNeeded,pbc,riid,ppvObject)

#define IDeviceEmulatorItem_GetObjectStorage(This,pszItem,pbc,riid,ppvStorage)	\
    (This)->lpVtbl -> GetObjectStorage(This,pszItem,pbc,riid,ppvStorage)

#define IDeviceEmulatorItem_IsRunning(This,pszItem)	\
    (This)->lpVtbl -> IsRunning(This,pszItem)


#define IDeviceEmulatorItem_BringVirtualMachineToFront(This)	\
    (This)->lpVtbl -> BringVirtualMachineToFront(This)

#define IDeviceEmulatorItem_ResetVirtualMachine(This,hardReset)	\
    (This)->lpVtbl -> ResetVirtualMachine(This,hardReset)

#define IDeviceEmulatorItem_ShutdownVirtualMachine(This,saveMachine)	\
    (This)->lpVtbl -> ShutdownVirtualMachine(This,saveMachine)

#define IDeviceEmulatorItem_BindToDMAChannel(This,dmaChannel,ppDMAChannel)	\
    (This)->lpVtbl -> BindToDMAChannel(This,dmaChannel,ppDMAChannel)

#define IDeviceEmulatorItem_GetVirtualMachineName(This,virtualMachineName)	\
    (This)->lpVtbl -> GetVirtualMachineName(This,virtualMachineName)

#define IDeviceEmulatorItem_SetVirtualMachineName(This,virtualMachineName)	\
    (This)->lpVtbl -> SetVirtualMachineName(This,virtualMachineName)

#define IDeviceEmulatorItem_GetMACAddressCount(This,numberOfMACs)	\
    (This)->lpVtbl -> GetMACAddressCount(This,numberOfMACs)

#define IDeviceEmulatorItem_EnumerateMACAddresses(This,numberOfMacs,arrayOfMACAddresses)	\
    (This)->lpVtbl -> EnumerateMACAddresses(This,numberOfMacs,arrayOfMACAddresses)

#define IDeviceEmulatorItem_ConfigureDevice(This,hwndParent,lcidParent,bstrConfig,pbstrConfig)	\
    (This)->lpVtbl -> ConfigureDevice(This,hwndParent,lcidParent,bstrConfig,pbstrConfig)

#define IDeviceEmulatorItem_GetDebuggerInterface(This,ppDebugger)	\
    (This)->lpVtbl -> GetDebuggerInterface(This,ppDebugger)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_BringVirtualMachineToFront_Proxy( 
    IDeviceEmulatorItem * This);


void __RPC_STUB IDeviceEmulatorItem_BringVirtualMachineToFront_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_ResetVirtualMachine_Proxy( 
    IDeviceEmulatorItem * This,
    /* [in] */ boolean hardReset);


void __RPC_STUB IDeviceEmulatorItem_ResetVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_ShutdownVirtualMachine_Proxy( 
    IDeviceEmulatorItem * This,
    /* [in] */ boolean saveMachine);


void __RPC_STUB IDeviceEmulatorItem_ShutdownVirtualMachine_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_BindToDMAChannel_Proxy( 
    IDeviceEmulatorItem * This,
    /* [in] */ ULONG dmaChannel,
    /* [retval][out] */ IDeviceEmulatorDMAChannel **ppDMAChannel);


void __RPC_STUB IDeviceEmulatorItem_BindToDMAChannel_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_GetVirtualMachineName_Proxy( 
    IDeviceEmulatorItem * This,
    /* [out] */ LPOLESTR *virtualMachineName);


void __RPC_STUB IDeviceEmulatorItem_GetVirtualMachineName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_SetVirtualMachineName_Proxy( 
    IDeviceEmulatorItem * This,
    /* [in] */ LPOLESTR virtualMachineName);


void __RPC_STUB IDeviceEmulatorItem_SetVirtualMachineName_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_GetMACAddressCount_Proxy( 
    IDeviceEmulatorItem * This,
    /* [retval][out] */ ULONG *numberOfMACs);


void __RPC_STUB IDeviceEmulatorItem_GetMACAddressCount_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_EnumerateMACAddresses_Proxy( 
    IDeviceEmulatorItem * This,
    /* [out][in] */ ULONG *numberOfMacs,
    /* [size_is][out] */ BYTE arrayOfMACAddresses[  ]);


void __RPC_STUB IDeviceEmulatorItem_EnumerateMACAddresses_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_ConfigureDevice_Proxy( 
    IDeviceEmulatorItem * This,
    /* [in] */ HWND hwndParent,
    /* [in] */ LCID lcidParent,
    /* [in] */ BSTR bstrConfig,
    /* [out] */ BSTR *pbstrConfig);


void __RPC_STUB IDeviceEmulatorItem_ConfigureDevice_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring] */ HRESULT STDMETHODCALLTYPE IDeviceEmulatorItem_GetDebuggerInterface_Proxy( 
    IDeviceEmulatorItem * This,
    /* [retval][out] */ IDeviceEmulatorDebugger **ppDebugger);


void __RPC_STUB IDeviceEmulatorItem_GetDebuggerInterface_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IDeviceEmulatorItem_INTERFACE_DEFINED__ */



#ifndef __DeviceEmulator_LIBRARY_DEFINED__
#define __DeviceEmulator_LIBRARY_DEFINED__

/* library DeviceEmulator */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_DeviceEmulator;

EXTERN_C const CLSID CLSID_DeviceEmulatorVirtualMachineManager;

#ifdef __cplusplus

class DECLSPEC_UUID("063e2de8-aa5b-46e8-8239-b8f7ca43f4c7")
DeviceEmulatorVirtualMachineManager;
#endif

EXTERN_C const CLSID CLSID_DeviceEmulatorVirtualTransport;

#ifdef __cplusplus

class DECLSPEC_UUID("8703b814-b436-4d99-b126-ce5df302730f")
DeviceEmulatorVirtualTransport;
#endif
#endif /* __DeviceEmulator_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  HWND_UserSize(     unsigned long *, unsigned long            , HWND * ); 
unsigned char * __RPC_USER  HWND_UserMarshal(  unsigned long *, unsigned char *, HWND * ); 
unsigned char * __RPC_USER  HWND_UserUnmarshal(unsigned long *, unsigned char *, HWND * ); 
void                      __RPC_USER  HWND_UserFree(     unsigned long *, HWND * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


