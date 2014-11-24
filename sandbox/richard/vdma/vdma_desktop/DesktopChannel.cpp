/*++

Copyright (c) 2003 Microsoft Corporation

--*/

#include "stdafx.h"
#include "DesktopChannel.h"

#define VIRT_CHANNEL_BASE       0x80000000
#define ADDRES_CHANNEL_NUM      0x80000001

DesktopDMAChannel::DesktopDMAChannel(GUID * VMID)
{
    m_pVT = NULL;
    m_State = ChannelDisconnected;
    memcpy( &m_VMID, VMID, sizeof(GUID));

    InitializeCriticalSection( &m_ChannelLockRead );
    InitializeCriticalSection( &m_ChannelLockWrite );
}

DesktopDMAChannel::DesktopDMAChannel()
{
    m_pVT   = NULL;
    m_State = ChannelUninitialized;
    InitializeCriticalSection( &m_ChannelLockRead );
    InitializeCriticalSection( &m_ChannelLockWrite );
}

DesktopDMAChannel::~DesktopDMAChannel()
{
    Close();
    DeleteCriticalSection( &m_ChannelLockRead );
    DeleteCriticalSection( &m_ChannelLockWrite );

}

HRESULT DesktopDMAChannel::Init(GUID * VMID)
{
    if ( m_State != ChannelUninitialized )
        return E_FAIL;

    m_pVT = NULL;
    m_State = ChannelDisconnected;
    memcpy( &m_VMID, VMID, sizeof(GUID));

    return S_OK;
}

HRESULT DesktopDMAChannel::Open(unsigned __int32 ChannelNumber)
{
    HRESULT hr;

    if ( ChannelNumber < VIRT_CHANNEL_BASE )
    {
        return E_INVALIDARG;
    }

    if ( m_State != ChannelDisconnected )
    {
        return E_INVALIDARG;
    }

    if ( m_pVT != NULL )
    {
        return E_INVALIDARG;
    }

    EnterCriticalSection(&m_ChannelLockRead);
    EnterCriticalSection(&m_ChannelLockWrite);

    hr = CoCreateInstance(CLSID_DeviceEmulatorVirtualTransport,
        NULL,
        CLSCTX_ALL,
        IID_IDeviceEmulatorVirtualTransport,
        (LPVOID*)&m_pVT);
    if (FAILED(hr)) {
        goto Error;
    }

    // Create the DMA channel
    hr = m_pVT->Create(&m_VMID, ChannelNumber, &m_TransportID);
    if (FAILED(hr)) {
        goto Error;
    }

    m_ChannelNumber = ChannelNumber;
    m_State         = ChannelConnected;

    LeaveCriticalSection(&m_ChannelLockWrite);
    LeaveCriticalSection(&m_ChannelLockRead);
    return S_OK;

Error:
    if ( m_pVT )
    {
        m_pVT->Delete(m_TransportID);
        m_pVT->Release();
        m_pVT = NULL;
    }
    LeaveCriticalSection(&m_ChannelLockWrite);
    LeaveCriticalSection(&m_ChannelLockRead);
    return hr;
}

void DesktopDMAChannel::Close()
{
    EnterCriticalSection(&m_ChannelLockRead);
    EnterCriticalSection(&m_ChannelLockWrite);
    if ( m_pVT )
    {
        m_pVT->Delete(m_TransportID);
        m_pVT->Release();
        m_pVT = NULL;
    }
    m_VMID = GUID_NULL;
    m_State = ChannelUninitialized;
    LeaveCriticalSection(&m_ChannelLockWrite);
    LeaveCriticalSection(&m_ChannelLockRead);
}

unsigned __int32 DesktopDMAChannel::DiscoverChannelNumber
    (unsigned __int8 * id, unsigned idLengthIn,  unsigned __int32 Timeout)
{
    DesktopDMAChannel AddressChannel(&m_VMID);

    if ( AddressChannel.Open( ADDRES_CHANNEL_NUM ) )
    {
        return 0;
    }

    unsigned __int8 Buffer[DMA_PACKET_SIZE];
    unsigned __int32 BytesRead;
    DWORD channelNumber = 0;
    while (channelNumber == 0)
    {
        BytesRead = DMA_PACKET_SIZE;
        if (FAILED(AddressChannel.Read( Buffer, &BytesRead, MINIMUM_PUBLISH_TIMEOUT ))) {
            return 0;
        }
        if (BytesRead > 4 )
        {
            unsigned __int16 currentOffset = 0;
            while ( currentOffset < ( BytesRead - 4 ) )
            {
                DWORD idLength = *(DWORD *)&Buffer[currentOffset];
                unsigned __int16 nextOffset = currentOffset + (unsigned __int16)(idLength + 2*sizeof(unsigned __int32));
                if  ( idLength == idLengthIn && nextOffset <= ( BytesRead - 4 ) &&
                      memcmp( &Buffer[currentOffset+4], id, idLength ) == 0 )
                {
                    channelNumber =  *(DWORD *)&Buffer[currentOffset + 4 + idLength];
                    break;
                }
                currentOffset = nextOffset;
            }
        }
        if ( channelNumber == 0 && Timeout > MINIMUM_PUBLISH_TIMEOUT)
        {
            Timeout -= MINIMUM_PUBLISH_TIMEOUT;
            Sleep(MINIMUM_PUBLISH_TIMEOUT);
        }
        else
        {
            break;
        }
    }
    AddressChannel.Close();

    return channelNumber;
}

HRESULT DesktopDMAChannel::Read( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout )
{
    HRESULT hr;
    ULONG WaitTime = MINIMUM_RECEIVE_TIMEOUT;
    IDeviceEmulatorVirtualTransport      * pVT = NULL;

    if ( *Count != DMA_PACKET_SIZE )
        return E_INVALIDARG;

    if ( m_State != ChannelConnected )
        return CHANNEL_DISCONNECTED;

    // Loop until the timeout time has been reached, but always call Receive() at least
    // once.
    USHORT CountInternal;
    do {
        EnterCriticalSection(&m_ChannelLockRead);
        pVT = m_pVT; 
        if ( pVT != NULL )
            pVT->AddRef();
        LeaveCriticalSection(&m_ChannelLockRead);

        if ( !pVT )
            return CHANNEL_DISCONNECTED;

        CountInternal = DMA_PACKET_SIZE;
        hr = pVT->Receive(m_TransportID, (BYTE*)Buffer, &CountInternal, WaitTime);
        pVT->Release();

        if (FAILED(hr)) {
            m_State = ChannelDisconnected;
            *Count = 0;
            return hr;
        }

        if (CountInternal == 0) {
            if (Timeout >= WaitTime )
            {
                Timeout =  ( Timeout != INFINITE) ? Timeout - WaitTime : Timeout;
            } else {
                Timeout = 0;
            }
        }
    } while (Timeout && CountInternal == 0 && m_State == ChannelConnected);

    *Count = CountInternal;

    if (*Count == 0) {
        return S_OK;
    }

    return S_OK;
}

HRESULT DesktopDMAChannel::Write( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout )
{
    HRESULT hr;
    IDeviceEmulatorVirtualTransport      * pVT = NULL;

    if (Count == NULL || Buffer == NULL || *Count > DMA_PACKET_SIZE)
    {
        hr = E_INVALIDARG;
        goto Error;
    }

    if ( m_State != ChannelConnected )
        return CHANNEL_DISCONNECTED;

RetryWrite:
    EnterCriticalSection(&m_ChannelLockWrite);
    pVT = m_pVT; 
    if ( pVT != NULL )
        pVT->AddRef();
    LeaveCriticalSection(&m_ChannelLockWrite);

    if ( !pVT )
        return CHANNEL_DISCONNECTED;

    USHORT CountInternal = (USHORT)*Count;
    hr = pVT->Send(m_TransportID, (BYTE*)Buffer, CountInternal );
    pVT->Release();

    if (FAILED(hr)) {
        if ( hr == QUEUE_FULL )
        {
            if ( Timeout > MINIMUM_SEND_TIMEOUT)
            {
                Timeout -= MINIMUM_SEND_TIMEOUT;
                Sleep(MINIMUM_SEND_TIMEOUT);
                if (  m_State == ChannelConnected )
                    goto RetryWrite;
            }
        }
        else if (hr == CHANNEL_DISCONNECTED )
        {
            m_State = ChannelDisconnected;
        }
        else 
        {
            m_State = ChannelDisconnected;
        }
    }

    *Count = CountInternal;
    return hr;

Error:
    return hr;
}

unsigned __int32 DesktopDMAChannel::getChannelNumber()
{
    return m_ChannelNumber;
}

bool DesktopDMAChannel::isConnected()
{
    return ( m_State == ChannelConnected );
}
