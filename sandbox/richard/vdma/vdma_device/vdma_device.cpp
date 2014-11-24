//
// Copyright (c) 2003 Microsoft Corporation
//
// vdma_device.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "vdma_device.h"
#include <windows.h>
#include <commctrl.h>
#include "DeviceChannel.h"

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE			g_hInst;			// current instance
HWND				g_hWndMenuBar;		// menu bar handle

// Forward declarations of functions included in this code module:
ATOM			MyRegisterClass(HINSTANCE, LPTSTR);
BOOL			InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);

HANDLE g_ExitDemoThread;

wchar_t* g_App = NULL;
wchar_t* g_Arg = NULL;
HWND g_HWnd;

int GetArguments(const char* string, char* arg) {
	int ch = ' ';
	char* pdest = strrchr(string, ch);
	strcpy(arg, pdest + 1);
	int result = pdest - string + 1;
	if (pdest == NULL) 
	{
		return -1;
	}
	else
	{
		return result;
	}
}

// This code simply echos received DMA packet data back to the desktop side
DWORD WINAPI DemoThreadProc(LPVOID lpParameter)
{
    DeviceDMAChannel Channel;

    // Choose a channel name.  The desktop and device sides must agree on the same
    // name, and it must be unique.  The easiest way to name a channel is to use
    // uuidgen.exe to generate a GUID.
    const char ChannelName[] = "34a0f021-362f-4e43-a223-5bdac54eb315";

    if (!Channel.Open()) {
        MessageBoxW(NULL, L"Channel.Open failed", L"VDMA Demo", MB_OK);
        return 0;
    }
    if (!Channel.Publish((unsigned __int8*)ChannelName, sizeof(ChannelName))) {
        MessageBoxW(NULL, L"Channel.Publish failed", L"VDMA Demo", MB_OK);
        return 0;
    }
    while (1) {
        HANDLE WaitHandles[2];
        WaitHandles[0] = g_ExitDemoThread;
        WaitHandles[1] = Channel.getDataAvailableHandle();   
        DWORD dw = WaitForMultipleObjects(2, WaitHandles, FALSE, INFINITE);
        if (dw == STATUS_WAIT_0) {
            break;
        } else if (dw == STATUS_WAIT_0+1) {
            unsigned __int8 Buffer[DMA_PACKET_SIZE];
            unsigned __int32 Count = sizeof(Buffer);
            if (FAILED(Channel.Read(Buffer, &Count, INFINITE))) {
                MessageBoxW(NULL, L"Channel.Read failed", L"VDMA Demo", MB_OK);
            } else {
			    char msg[20];

				STARTUPINFO si;
				PROCESS_INFORMATION pi;

				ZeroMemory( &si, sizeof(si) );
				si.cb = sizeof(si);
				ZeroMemory( &pi, sizeof(pi) );
				
				char arg[20];
				int Result = GetArguments((const char*)Buffer, arg);
				wchar_t App[DMA_PACKET_SIZE];
				wchar_t WArg[20];
				mbstowcs(App, (const char*)Buffer, Result);
				mbstowcs(WArg, (const char*)arg, Count - Result);
				
				g_App = App;
				g_Arg = WArg;
				SendMessage(g_HWnd, WM_PAINT, NULL, NULL);

				if (CreateProcess(App, 
						WArg,
						NULL,
						NULL,
						FALSE,
						0,
						NULL,
						NULL,
						&si,
						&pi)) 
				{
					WaitForSingleObject(pi.hProcess, INFINITE);

					DWORD exitCode = 0;
					GetExitCodeProcess(pi.hProcess, &exitCode);
					if (exitCode != 0)
					{
						_itoa(exitCode, msg, 10);
						//MessageBoxW(NULL, msg, L"Exit Code", MB_OK);
					} 
					else 
					{
						DWORD dw = GetLastError(); 
						_itoa(dw, msg, 10);
						//MessageBoxW(NULL, msg, L"Last Error", MB_OK);
					}

					CloseHandle( pi.hProcess );
					CloseHandle( pi.hThread );
				}
				else
				{
					DWORD dw = GetLastError(); 
					_itoa(dw, msg, 10);
					//MessageBoxW(NULL, L"CreateProcess failed", L"VDMA Demo", MB_OK);
				}
				
				unsigned int len = strlen(msg);
				if (FAILED(Channel.Write((unsigned __int8* )msg, &len, INFINITE))) {
					MessageBoxW(NULL, L"Channel.Write failed", L"VDMA Demo", MB_OK);
				}
            }
        } else {
            MessageBoxW(NULL, L"WaitForMultipleObjects failed", L"VDMA Demo", MB_OK);
            break;
        }
    }
    Channel.Close();
    return 0;
}



int WINAPI WinMain(HINSTANCE hInstance,
                   HINSTANCE hPrevInstance,
                   LPTSTR    lpCmdLine,
                   int       nCmdShow)
{
	MSG msg;

	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow)) 
	{
		return FALSE;
	}

	HACCEL hAccelTable;
	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_VDMA_DEVICE));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0)) 
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) 
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}

//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
ATOM MyRegisterClass(HINSTANCE hInstance, LPTSTR szWindowClass)
{
	WNDCLASS wc;

	wc.style         = CS_HREDRAW | CS_VREDRAW;
	wc.lpfnWndProc   = WndProc;
	wc.cbClsExtra    = 0;
	wc.cbWndExtra    = 0;
	wc.hInstance     = hInstance;
	wc.hIcon         = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_VDMA_DEVICE));
	wc.hCursor       = 0;
	wc.hbrBackground = (HBRUSH) GetStockObject(WHITE_BRUSH);
	wc.lpszMenuName  = 0;
	wc.lpszClassName = szWindowClass;

	return RegisterClass(&wc);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    HWND hWnd;
    TCHAR szTitle[MAX_LOADSTRING];		// title bar text
    TCHAR szWindowClass[MAX_LOADSTRING];	// main window class name

    g_hInst = hInstance; // Store instance handle in our global variable

    // SHInitExtraControls should be called once during your application's initialization to initialize any
    // of the device specific controls such as CAPEDIT and SIPPREF.
    SHInitExtraControls();

    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING); 
    LoadString(hInstance, IDC_VDMA_DEVICE, szWindowClass, MAX_LOADSTRING);

    //If it is already running, then focus on the window, and exit
    hWnd = FindWindow(szWindowClass, szTitle);	
    if (hWnd) 
    {
        // set focus to foremost child window
        // The "| 0x00000001" is used to bring any owned windows to the foreground and
        // activate them.
        SetForegroundWindow((HWND)((ULONG) hWnd | 0x00000001));
		g_HWnd = hWnd;
        return 0;
    } 

    if (!MyRegisterClass(hInstance, szWindowClass))
    {
    	return FALSE;
    }

    hWnd = CreateWindow(szWindowClass, szTitle, WS_VISIBLE,
        CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, NULL, NULL, hInstance, NULL);

    if (!hWnd)
    {
        return FALSE;
    }

    // When the main window is created using CW_USEDEFAULT the height of the menubar (if one
    // is created is not taken into account). So we resize the window after creating it
    // if a menubar is present
    if (g_hWndMenuBar)
    {
        RECT rc;
        RECT rcMenuBar;

        GetWindowRect(hWnd, &rc);
        GetWindowRect(g_hWndMenuBar, &rcMenuBar);
        rc.bottom -= (rcMenuBar.bottom - rcMenuBar.top);
		
        MoveWindow(hWnd, rc.left, rc.top, rc.right-rc.left, rc.bottom-rc.top, FALSE);
    }

    // Launch the demo DMA thread
    g_ExitDemoThread = CreateEvent(NULL, FALSE, FALSE, NULL);
    CreateThread(NULL, 0, DemoThreadProc, NULL, 0, NULL);

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

	g_HWnd = hWnd;

    return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    int wmId, wmEvent;
    PAINTSTRUCT ps;
    HDC hdc;

    static SHACTIVATEINFO s_sai;
	
    switch (message) 
    {
        case WM_COMMAND:
            wmId    = LOWORD(wParam); 
            wmEvent = HIWORD(wParam); 
            // Parse the menu selections:
            switch (wmId)
            {
                case IDM_HELP_ABOUT:
                    DialogBox(g_hInst, (LPCTSTR)IDD_ABOUTBOX, hWnd, About);
                    break;
                case IDM_OK:
                    SendMessage (hWnd, WM_CLOSE, 0, 0);				
                    break;
                default:
                    return DefWindowProc(hWnd, message, wParam, lParam);
            }
            break;
        case WM_CREATE:
            SHMENUBARINFO mbi;

            memset(&mbi, 0, sizeof(SHMENUBARINFO));
            mbi.cbSize     = sizeof(SHMENUBARINFO);
            mbi.hwndParent = hWnd;
            mbi.nToolBarId = IDR_MENU;
            mbi.hInstRes   = g_hInst;

            if (!SHCreateMenuBar(&mbi)) 
            {
                g_hWndMenuBar = NULL;
            }
            else
            {
                g_hWndMenuBar = mbi.hwndMB;
            }

            // Initialize the shell activate info structure
            memset(&s_sai, 0, sizeof (s_sai));
            s_sai.cbSize = sizeof (s_sai);
            break;
        case WM_PAINT:
            hdc = BeginPaint(hWnd, &ps);
            
            // TODO: Add any drawing code here...
			ExtTextOut(hdc, 10, 10, ETO_CLIPPED, NULL, g_App, wcslen(g_App), NULL);
			ExtTextOut(hdc, 10, 40, ETO_CLIPPED, NULL, g_Arg, wcslen(g_Arg), NULL);

            EndPaint(hWnd, &ps);
            break;
        case WM_DESTROY:
            SetEvent(g_ExitDemoThread); // tell the worker thread to quit
            CommandBar_Destroy(g_hWndMenuBar);
            PostQuitMessage(0);
            break;

        case WM_ACTIVATE:
            // Notify shell of our activate message
            SHHandleWMActivate(hWnd, wParam, lParam, &s_sai, FALSE);
            break;
        case WM_SETTINGCHANGE:
            SHHandleWMSettingChange(hWnd, wParam, lParam, &s_sai);
            break;

        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_INITDIALOG:
            {
                // Create a Done button and size it.  
                SHINITDLGINFO shidi;
                shidi.dwMask = SHIDIM_FLAGS;
                shidi.dwFlags = SHIDIF_DONEBUTTON | SHIDIF_SIPDOWN | SHIDIF_SIZEDLGFULLSCREEN | SHIDIF_EMPTYMENU;
                shidi.hDlg = hDlg;
                SHInitDialog(&shidi);
            }
            return (INT_PTR)TRUE;

        case WM_COMMAND:
            if (LOWORD(wParam) == IDOK)
            {
                EndDialog(hDlg, LOWORD(wParam));
                return TRUE;
            }
            break;

        case WM_CLOSE:
            EndDialog(hDlg, message);
            return TRUE;

#ifdef _DEVICE_RESOLUTION_AWARE
        case WM_SIZE:
            {
		DRA::RelayoutDialog(
			g_hInst, 
			hDlg, 
			DRA::GetDisplayMode() != DRA::Portrait ? MAKEINTRESOURCE(IDD_ABOUTBOX_WIDE) : MAKEINTRESOURCE(IDD_ABOUTBOX));
            }
            break;
#endif
    }
    return (INT_PTR)FALSE;
}
