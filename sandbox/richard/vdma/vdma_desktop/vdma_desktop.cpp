//
// Copyright (c) 2003 Microsoft Corporation
//
// vdma_desktop.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include "VMIDChooser.h"
#include "DesktopChannel.h"


void outputLog(const wchar_t* filename)
{
	FILE *stream;
	char line[81];

	stream = _wfopen(filename, L"r");
	if (stream != NULL)
	{
		while(! feof(stream))
		{
			memset(line, 0, sizeof(line));
			fread(line, sizeof(char), 80, stream);
			fprintf(stderr, "%s", line);
			if (ferror(stream))
			{
				break;
			}
		}
		
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	if (argc < 3) {
		fprintf(stderr, "Please specify the CF test location: \n vdma_desktop.exe \\\\Storage Card\\CF\\Db4objects.Db4o.Tests.exe run");
		return -1;
	}
	wprintf(L"argv[1] %s\n", argv[1]);
	wprintf(L"argv[2] %s\n", argv[2]);

	char Buffer[DMA_PACKET_SIZE];
	size_t Count;
	wcstombs_s(&Count, Buffer, DMA_PACKET_SIZE, argv[1], DMA_PACKET_SIZE); 

    CoInitializeEx(NULL, COINIT_MULTITHREADED|COINIT_DISABLE_OLE1DDE);

    // Ask the user to select a running emulator to connect to
    GUID VMID;
    if (!ChooseVMID(&VMID)) {
		wchar_t GUID_string[60];
		StringFromGUID2(VMID, GUID_string, sizeof(GUID_string)/sizeof(GUID_string[0]));
        fwprintf(stderr, L"Error: ChooseVMID - %s failed\n", GUID_string);
        return -2;
    }

    // Create the DMA channel
    DesktopDMAChannel Channel(&VMID);

    // Choose a channel name.  The desktop and device sides must agree on the same
    // name, and it must be unique.  The easiest way to name a channel is to use
    // uuidgen.exe to generate a GUID.
    const char ChannelName[] = "34a0f021-362f-4e43-a223-5bdac54eb315";

    // Hook up the channel
    unsigned __int32 ChannelNumber = Channel.DiscoverChannelNumber((unsigned __int8*)ChannelName, sizeof(ChannelName), INFINITE);
    if (ChannelNumber == 0) {
        fprintf(stderr, "Error: Channel.DiscoverChannelNumber failed\n");
        return -3;
    }

    // Open the channel
    HRESULT hr = Channel.Open(ChannelNumber);
    if (FAILED(hr)) {
        fprintf(stderr, "Error: Channel.Open failed with hr=%x\n", hr);
        return -4;
    }

    hr = Channel.Write((unsigned char *)Buffer, &Count, INFINITE);
    if (FAILED(hr)) {
        fprintf(stderr, "Error: Channel.Write failed with hr=%x\n", hr);
		return -5;
    }
    memset(Buffer, 0, sizeof(Buffer)); // prove that the echo is returning
    Count = sizeof(Buffer);
    hr = Channel.Read((unsigned char *)Buffer, &Count, INFINITE);
    if (FAILED(hr)) {
        fprintf(stderr, "Error: Channel.Read failed with hr=%x\n", hr);
        return -6;
    }
    
	int exitcode = atoi(Buffer);
	if ((exitcode==0) && (strncmp("0", Buffer, Count)!=0))
	{
		fprintf(stderr, "Unexpected error!");
		exitcode = -7;
	}
	
    Channel.Close();

	printf("Return: %d\n", exitcode);

	//print test log to stderr.
	outputLog(argv[2]);
	return exitcode;
}

