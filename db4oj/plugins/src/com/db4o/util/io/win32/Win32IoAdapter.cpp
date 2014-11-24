// Win32IoAdapter.cpp : Defines the entry point for the DLL application.
//

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "Win32IoAdapter.h"
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

#define ToHandle(value) reinterpret_cast<HANDLE>(value)
#define FromHandle(value) reinterpret_cast<jlong>(value)

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    openFile
 * Signature: (Ljava/lang/String;ZJ)J
 */
JNIEXPORT jlong JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_openFile
  (JNIEnv* env, jclass, jstring fname, jboolean locking, jlong)
{	
	DWORD shareMode = locking ? 0 : FILE_SHARE_READ;
	const jchar* path = env->GetStringChars(fname, NULL);
	HANDLE handle = ::CreateFile(reinterpret_cast<LPCWSTR>(path), GENERIC_READ|GENERIC_WRITE, shareMode, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL|FILE_FLAG_RANDOM_ACCESS, NULL);
	env->ReleaseStringChars(fname, path);
	return FromHandle(handle);
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    closeFile
 * Signature: (J)V
 */
JNIEXPORT void JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_closeFile
  (JNIEnv *, jclass, jlong handle)
{
	::CloseHandle(ToHandle(handle));
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    getLength
 * Signature: (J)J
 */
JNIEXPORT jlong JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_getLength
  (JNIEnv *, jclass, jlong handle)
{
	return ::GetFileSize(ToHandle(handle), NULL);
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    read
 * Signature: (J[BI)I
 */
JNIEXPORT jint JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_read
  (JNIEnv* env, jclass, jlong handle, jbyteArray bytes, jint len)
{	
	DWORD cbRead = 0;
	void* buffer = env->GetPrimitiveArrayCritical(bytes, NULL);
	if (NULL != buffer)
	{		
		::ReadFile(ToHandle(handle), buffer, len, &cbRead, NULL);
		env->ReleasePrimitiveArrayCritical(bytes, buffer, 0);
	}
	return cbRead;
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    seek
 * Signature: (JJ)V
 */
JNIEXPORT void JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_seek
  (JNIEnv *, jclass, jlong handle, jlong pos)
{
	::SetFilePointer(ToHandle(handle), pos, NULL, FILE_BEGIN);
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    sync
 * Signature: (J)V
 */
JNIEXPORT void JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_sync
  (JNIEnv *, jclass, jlong handle)
{
	::FlushFileBuffers(ToHandle(handle));
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    write
 * Signature: (J[BI)V
 */
JNIEXPORT void JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_write
  (JNIEnv* env, jclass, jlong handle, jbyteArray bytes, jint length)
{
	void* buffer = env->GetPrimitiveArrayCritical(bytes, NULL);
	if (NULL != buffer)
	{
		DWORD cbWritten = 0;
		::WriteFile(ToHandle(handle), buffer, length, &cbWritten, NULL);
		env->ReleasePrimitiveArrayCritical(bytes, buffer, 0);
	}
}

/*
 * Class:     com_db4o_util_io_Win32IoAdapter
 * Method:    copy
 * Signature: (JJJI)V
 */
JNIEXPORT void JNICALL Java_com_db4o_util_io_win32_Win32IoAdapter_copy
  (JNIEnv* env, jclass, jlong handle, jlong oldAddress, jlong newAddress, jint len)
{	
	DWORD cbRead = 0;
	HANDLE nativeHandle = ToHandle(handle);	
	void* buffer = ::LocalAlloc(LMEM_FIXED, len);
	if (NULL != buffer)
	{
		::SetFilePointer(nativeHandle, oldAddress, NULL, FILE_BEGIN);
		::ReadFile(nativeHandle, buffer, len, &cbRead, NULL);
		if (cbRead == len)
		{
			DWORD cbWritten = 0;
			::SetFilePointer(nativeHandle, newAddress, NULL, FILE_BEGIN);
			::WriteFile(nativeHandle, buffer, len, &cbWritten, NULL);
		}

		::LocalFree(buffer);
	}
	else
	{
		// env->ThrowNew
	}
}
