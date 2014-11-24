/*++

Copyright (c) 2003 Microsoft Corporation

--*/

#include "stdafx.h"
#include "DeviceChannel.h"

DeviceDMAChannel::DeviceDMAChannel()
{
    m_hDataReady = NULL;
    m_hChannelClosed = NULL;
    m_ChannelNumber = 0;
    m_hChannel = INVALID_HANDLE_VALUE;
    m_State = ChannelUninitialized;
}
DeviceDMAChannel::~DeviceDMAChannel()
{
    Close();
    if ( m_hChannelClosed != NULL )
        CloseHandle(m_hChannelClosed);
}

bool DeviceDMAChannel::Open()
{
    if (m_State != ChannelUninitialized )
    {
        MessageBoxW(NULL, L"m_State", L"DeviceDMAChannel", MB_OK);
        return false;
    }

    // Create the event close event for the channel
    m_hChannelClosed = CreateEvent(NULL, FALSE, FALSE, NULL);
    if ( m_hChannelClosed == NULL )
    {
        MessageBoxW(NULL, L"CreateEvent", L"DeviceDMAChannel", MB_OK);
        return false;
    }

    // Create a virtual channel
    m_hChannel = ::CreateFile(
                L"DMA8:", 
                GENERIC_READ | GENERIC_WRITE,
                0,
                NULL, 
                OPEN_EXISTING, 
                FILE_ATTRIBUTE_NORMAL,
                NULL);

    if ( m_hChannel == INVALID_HANDLE_VALUE  )
    {
        MessageBoxW(NULL, L"CreateFile", L"DeviceDMAChannel", MB_OK);
        return false;
    }

    // Get the channel number and the handle for the event
    DWORD bWritten  = 0;
    DWORD outBuf[2];
    if ( !DeviceIoControl( m_hChannel, IOCTL_GET_DMA_CHANNEL_INFO, NULL,
                           4, &outBuf[0], 8, &bWritten, NULL ) || bWritten != 8 )
    {
        MessageBoxW(NULL, L"IOCTL", L"DeviceDMAChannel", MB_OK);
         return false;
    }
    // Store the channel number 
    m_ChannelNumber = outBuf[0];
    m_hDataReady    = (HANDLE)outBuf[1];
    // Update the channel state
    m_State = ChannelDisconnected;

    return true;
}

void DeviceDMAChannel::Close()
{
    if (m_hDataReady != NULL)
        CloseHandle(m_hDataReady);
    if (m_hChannel != INVALID_HANDLE_VALUE)
        CloseHandle(m_hChannel);
    m_hChannel          = INVALID_HANDLE_VALUE ;
    m_hDataReady        = NULL;
    m_ChannelNumber     = 0;
    m_State             = ChannelUninitialized;
    // Make sure that anyone waiting for data is notified
    SetEvent(m_hChannelClosed);
}

bool DeviceDMAChannel::Publish(const unsigned __int8 * id, unsigned __int32 idLen)
{
    if ( m_State == ChannelUninitialized )
        return false;

    if ( id == NULL || idLen > (DMA_PACKET_SIZE - 2*sizeof(unsigned __int32)))
        return false;

    HANDLE hPublish = ::CreateFile(
                L"DMA9:", 
                GENERIC_READ | GENERIC_WRITE,
                0,
                NULL, 
                OPEN_EXISTING, 
                FILE_ATTRIBUTE_NORMAL,
                NULL);

    if ( hPublish == INVALID_HANDLE_VALUE )
        return false;

    unsigned __int32  recSize = 2*sizeof(unsigned __int32) + idLen;
    unsigned __int8 * addressRec = new unsigned __int8[recSize];
    *(unsigned __int32 *)&addressRec[0] = idLen;
    memcpy(&addressRec[sizeof(unsigned __int32)], id, idLen);
    // Write the channel number taking care to prevent alignment faults
    *((UNALIGNED DWORD *)&addressRec[idLen+sizeof(unsigned __int32)]) = m_ChannelNumber;

    DWORD dwWritten;
    BOOL bRet = ::WriteFile(
            hPublish, 
            addressRec, 
            recSize, 
            &dwWritten, 
            NULL);

    CloseHandle(hPublish);

    if (!bRet || dwWritten != recSize )
        return false;

    // Successfully published the address
    return true;
}

HRESULT DeviceDMAChannel::Read( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout )
{
    if ( m_State == ChannelUninitialized )
        return E_FAIL;

    if ( Buffer == NULL || Count == NULL )
        return E_INVALIDARG;

    HANDLE Handles[2];

    Handles[0] = m_hDataReady;
    Handles[1] = m_hChannelClosed;

RetryRead:
    // Wait for data to appear
    if ( Timeout != 0 )
    {
        DWORD dw = ::WaitForMultipleObjects(2, Handles, FALSE, Timeout);

        if ( dw == WAIT_TIMEOUT )
        {
            // Time out - no data available
            *Count = 0;
            return S_OK;
        }
        else if ( dw != WAIT_OBJECT_0 )
        {
            // Channel was closed while we were waiting
            *Count = 0;
            return CHANNEL_DISCONNECTED;
        }
    }

    *Count = 0;
    BOOL bRet = ::ReadFile(
            m_hChannel, 
            Buffer,
            DMA_PACKET_SIZE,
            (DWORD *)Count,
            NULL);

    // If there was no data
    if ( bRet && *Count == 0 )
        goto RetryRead;

    DWORD LastError = GetLastError();
    return (bRet ? S_OK : HRESULT_FROM_WIN32(LastError));
}

HRESULT DeviceDMAChannel::Write( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout )
{
    if ( m_State == ChannelUninitialized )
        return E_FAIL;

    if ( Buffer == NULL || Count == NULL )
        return E_INVALIDARG;

    // If the channel is currently disconnected check before sending
    if ( m_State == ChannelDisconnected )
    {
        if ( !isConnected() )
            return CHANNEL_DISCONNECTED;
        else
        {
            m_State = ChannelConnected;
        }
    }

    HRESULT result = S_OK;
RetryWrite:
    DWORD InternalCount = *Count;
    if ( !::WriteFile(
            m_hChannel, 
            Buffer, 
            InternalCount, 
            (DWORD *)&InternalCount, 
            NULL) )
    {
        DWORD ErrorCode = GetLastError();

        if ( ErrorCode == QUEUE_FULL_ERROR ) 
        {
            if ( Timeout > MINIMUM_SEND_TIMEOUT) 
            {
                Timeout -= MINIMUM_SEND_TIMEOUT;
                Sleep(MINIMUM_SEND_TIMEOUT);
                goto RetryWrite;
            }
            result = QUEUE_FULL;
        }
        else if ( ErrorCode == CHANNEL_DISCONNECTED_ERROR ) 
        {
            m_State = ChannelDisconnected;
            result = CHANNEL_DISCONNECTED;
        }
        else 
        {
            result = HRESULT_FROM_WIN32(ErrorCode);
        }
        InternalCount = 0;
    }

    *Count = InternalCount;
    return result;
}

unsigned __int32 DeviceDMAChannel::getChannelNumber()
{
    return m_ChannelNumber;
}

bool DeviceDMAChannel::isConnected()
{
    DWORD bWritten  = 0;
    DWORD outBuf;

    if ( m_State == ChannelUninitialized )
        return 0;

    if ( !DeviceIoControl( m_hChannel, IOCTL_GET_DMA_CHANNEL_CONNECTED, 
                           NULL, 0, &outBuf, 4, &bWritten, NULL ) || bWritten != 4 )
         return 0;

    return ( outBuf == 1 );
}

bool DeviceDMAChannel::isDataAvailable()
{
    if ( m_State == ChannelUninitialized )
        return false;

    DWORD dw = ::WaitForSingleObject( m_hDataReady, 0);

    if ( dw != WAIT_OBJECT_0)
    {
        // Time out - no data available
        return false;
    }

    return true;
}

HANDLE DeviceDMAChannel::getDataAvailableHandle()
{
    if ( m_State == ChannelUninitialized )
        return NULL;

    return m_hDataReady;
}

