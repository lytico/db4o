/*++

Copyright (c) 2003 Microsoft Corporation

--*/

#ifndef DEVICE_CHANNEL_H__
#define DEVICE_CHANNEL_H__

#define DMA_PACKET_SIZE (unsigned __int16)4096

#define CHANNEL_DISCONNECTED    HRESULT_FROM_WIN32(ERROR_NOT_READY)
#define QUEUE_FULL              HRESULT_FROM_WIN32(ERROR_NOT_ENOUGH_MEMORY)
#define CHANNEL_TIMEOUT         HRESULT_FROM_WIN32( WAIT_TIMEOUT )

#define IOCTL_GET_DMA_CHANNEL_INFO 0x80000004
#define IOCTL_GET_DMA_CHANNEL_CONNECTED 0x80000008

#define MINIMUM_SEND_TIMEOUT 100
#define CONTROL_CHANNEL_TIMEOUT 2000

enum {
    ChannelUninitialized = 0x1,
    ChannelDisconnected  = 0x2,
    ChannelConnected     = 0x3,
};

#define CHANNEL_DISCONNECTED_ERROR    0xfffffffd
#define QUEUE_FULL_ERROR              0xfffffffe

class DeviceDMAChannel //: DMAChannel - work aroud compiler bug that causes a crash on the device
{
public:
    DeviceDMAChannel();
    ~DeviceDMAChannel();

    bool Open();
    void Close();
    bool Publish(const unsigned __int8 * id, unsigned __int32 idLen);

    HRESULT Read ( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout );
    HRESULT Write( unsigned __int8 * Buffer, unsigned __int32 * Count, unsigned __int32 Timeout );

    unsigned __int32 getChannelNumber();
    bool isConnected();
    bool isDataAvailable();
    HANDLE getDataAvailableHandle();

private:
    HANDLE m_hDataReady;
    HANDLE m_hChannelClosed;
    DWORD  m_ChannelNumber;
    HANDLE m_hChannel;
    DWORD  m_State;
};

#endif /// DEVICE_CHANNEL_H__
