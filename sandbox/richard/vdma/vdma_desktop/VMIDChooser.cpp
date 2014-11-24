//
// Copyright (c) 2003 Microsoft Corporation
//
#include "stdafx.h"
#include "DEComInterfaces_h.h"

IDeviceEmulatorVirtualMachineManager *g_pVMM;
GUID *g_pVMID;

#define MAX_ITEM_LENGTH 60

ULONG g_NumberOfVMs;
GUID *g_VMIDList;
IMalloc *g_pMalloc;

void ClearVMIDList(void)
{
    g_NumberOfVMs = 0;
    if (g_VMIDList) {
        free(g_VMIDList);
        g_VMIDList = NULL;
    }

}

int RefreshVMIDList()
{
    HRESULT hr;

    // Clear out g_VMIDList
    ClearVMIDList();

    // Connect to a DeviceEmulatorVirtualMachineManager
    if (NULL == g_pVMM) {
        hr = CoCreateInstance(CLSID_DeviceEmulatorVirtualMachineManager,
            NULL,
            CLSCTX_LOCAL_SERVER|CLSCTX_INPROC_SERVER|CLSCTX_INPROC_HANDLER,
            IID_IDeviceEmulatorVirtualMachineManager,
            (LPVOID*)&g_pVMM);
        if (FAILED(hr)) {
            return -1;
        }
    }

    // Enumerate all of the VMIDs
    ULONG TotalNumberOfVMs;
    hr = g_pVMM->GetVirtualMachineCount(&TotalNumberOfVMs);
    if (FAILED(hr)) {
        return -1;
    }
    g_VMIDList = (GUID*)malloc(sizeof(GUID)*TotalNumberOfVMs);
    if (NULL == g_VMIDList) {
        return -1;
    }
    GUID * TotalVMIDList = (GUID*)malloc(sizeof(GUID)*TotalNumberOfVMs);
    if (NULL == TotalVMIDList) {
        return -1;
    }

    hr = g_pVMM->EnumerateVirtualMachines(&TotalNumberOfVMs, TotalVMIDList);
    if (FAILED(hr)) {
        return -1;
    }

    for (ULONG i=0; i<TotalNumberOfVMs; ++i) {
        boolean bIsRunning;

        hr = g_pVMM->IsVirtualMachineRunning(&TotalVMIDList[i], &bIsRunning);
        if (FAILED(hr)) {
            continue;
        }
        if (bIsRunning) {
            LPOLESTR vmName;

            if (FAILED(g_pVMM->GetVirtualMachineName(&TotalVMIDList[i], &vmName))) {
                wchar_t Buffer[MAX_ITEM_LENGTH];
                if (!FAILED(StringFromGUID2(TotalVMIDList[i], Buffer, sizeof(Buffer)/sizeof(Buffer[0])))) {
                    g_VMIDList[g_NumberOfVMs++] = TotalVMIDList[i];
                }
            } else {
                g_pMalloc->Free(vmName);
                g_VMIDList[g_NumberOfVMs++] = TotalVMIDList[i];
            }
        }
    }
    free(TotalVMIDList);

	return (int)TotalNumberOfVMs;
}

bool ChooseVMID(GUID *pVMID)
{
	HRESULT hresult = CoGetMalloc(1, &g_pMalloc);
    if (FAILED(hresult)) {
		if (hresult == E_INVALIDARG)
		{
			fwprintf(stderr, L"E_INVALIDARG\n");
		}
		if (hresult == E_OUTOFMEMORY)
		{
			fwprintf(stderr, L"E_OUTOFMEMORY\n");
		}

        return false;
    }

    g_pVMID = pVMID;
	int count = RefreshVMIDList();
	if (count <= 0) {
		fwprintf(stderr, L"There is no VMID found!\n");
		return false;
	}
	else {
		*g_pVMID = g_VMIDList[0];
		//list all VMs
		for (int i = 0; i < count; i++) 
		{
			wchar_t GUID_string[60];
			StringFromGUID2(g_VMIDList[i], GUID_string, sizeof(GUID_string)/sizeof(GUID_string[0]));
			fwprintf(stderr, L"VMID[%d]: %s\n", i, GUID_string);
		}
	}

    if (g_pVMM) {
        g_pVMM->Release();
        g_pVMM = NULL;
    }
    ClearVMIDList();
    g_pMalloc->Release();
    g_pMalloc = NULL;

    if (g_pVMID == NULL) { // The API returns -1 if there is an error.  We'll use 0 as 'success'.
        return false;
    }
    g_pVMID = NULL;
    return true;
}
