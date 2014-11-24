/*++

Copyright (c) 2003 Microsoft Corporation

--*/

#ifndef DESKTOP_CHANNEL_H__
#define DESKTOP_CHANNEL_H__

#include "DECOMInterfaces_h.h"

#define DMA_PACKET_SIZE (unsigned __int16)4096

#define CHANNEL_DISCONNECTED    HRESULT_FROM_WIN32(ERROR_NOT_READY)
#define QUEUE_FULL              HRESULT_FROM_WIN32(ERROR_NOT_ENOUGH_MEMORY)
#define CHANNEL_TIMEOUT         HRESULT_FROM_WIN32( WAIT_TIMEOUT )

#define MINIMUM_SEND_TIMEOUT    100
#define MINIMUM_RECEIVE_TIMEOUT 1000
#define MINIMUM_PUBLISH_TIMEOUT 1000

enum {
    ChannelUninitialized = 0x1,
    ChannelDisconnected  = 0x2,
    ChannelConnected     = 0x3,
};

class DesktopDMAChannel
{
public:
    DesktopDMAChannel();
    DesktopDMAChannel(GUID * VMID);
    ~DesktopDMAChannel();

    HRESULT Init(GUID * VMID);
    unsigned __int32 DiscoverChannelNumber(unsigned __int8 * id, unsigned idLengthIn,  unsigned __int32 Timeout);

    HRESULT Open(unsigned __int32 ChannelNumber);
    void Close();

    HRESULT Read( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout );
    HRESULT Write( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout );

    unsigned __int32 getChannelNumber();
    bool isConnected();

private:
    IDeviceEmulatorVirtualTransport      *m_pVT;
    CRITICAL_SECTION                      m_ChannelLockWrite;
    CRITICAL_SECTION                      m_ChannelLockRead;

    GUID   m_VMID;
    DWORD  m_ChannelNumber;
    ULONG  m_TransportID;
    DWORD  m_State;
};

#endif // DESKTOP_CHANNEL_H__
