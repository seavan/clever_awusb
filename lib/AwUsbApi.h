/************************************************************************
 *																		*
 *	awusbapi.h	Win2k/XP AwUsb API library header					    *
 *																		*
 ************************************************************************
 *																		*
 *	Copyright (c) 2009 Digi International            					*
 *	All rights reserved.												*
 *																		*
 ************************************************************************
 *																		*
 * $Log:: /AWUSB/Inc/AwUsbApi.h                                    $	*
 * 
 * Revision: 2   Date: 2009-10-07 22:01:17Z   User: davidi 
 * 
 * 3     12/02/03 10:58a Timj
 * Added AWUSB_STATUS_NOT_CONNECTED as return value for
 * AwUsbDisconnect().
 * 
 * 2     11/25/03 2:00p Timj
 * Add AWUSB_STATUS_DELETE_FAILURE code.
 * Add documentation.
 * 
 * 1     11/25/03 1:30p Timj
 * Add build.c module.
 * 
 * 1     11/24/03 7:16p Timj
 ************************************************************************/

typedef enum AWUSB_STATUS_tag{
	AWUSB_STATUS_CONNECTED,
	AWUSB_STATUS_ALREADY_CONNECTED,
	AWUSB_STATUS_INVALID_PARAMETER,
	AWUSB_STATUS_TIMEOUT,
	AWUSB_STATUS_CANCELLED,
	AWUSB_STATUS_SUCCESS,
	AWUSB_STATUS_CONNECTING,
	AWUSB_STATUS_IN_USE,
	AWUSB_STATUS_AVAILABLE,
	AWUSB_STATUS_PENDING,
	AWUSB_STATUS_DELETE_FAILURE,
	AWUSB_STATUS_NOT_CONNECTED
}AWUSB_STATUS, *PAWUSB_STATUS;

#ifdef __cplusplus
extern "C" {
#define	DECLSPEC_EXPORT
#else
#define	DECLSPEC_EXPORT		__declspec(dllexport)
#endif

/************************************************************************
 * AwUsbConnect
 * 	Parms:
 * 		Hub
 * 			AwUsb Hub IP Address in string format.
 * 		Status
 * 			Updated upon return from function, and, when hEvent is non-NULL, 
 * 			after the connect operation completes. See Return Values.
 * 		Timeout
 * 			Specifies how long to wait for connection to succeed, in milliseconds. 
 * 			Must be non-zero. Set value to INFINITE in order to wait forever. 
 * 			Returns AWUSB_STATUS_TIMEOUT if timeout interval elapses.
 * 		hEvent
 * 			Handle to an event which will be set to the signaled state when the 
 * 			operation has been completed. If hEvent is NULL, this function will 
 * 			block until it completes. If hEvent is non-NULL, then the function 
 * 			returns immediately, usually with AWUSB_STATUS_PENDING unless an error 
 * 			has occurred.  To create an event object, use the CreateEvent function.  
 * 			An application must wait until hEvent has been signaled before calling
 * 			CloseHandle.
 * 
 *  Returns:
 * 		AWUSB_STATUS_CONNECTED
 * 			Successfully connected to hub.
 * 		AWUSB_STATUS_TIMEOUT
 * 			Timed out trying to connect to hub.
 * 		AWUSB_STATUS_PENDING
 * 			Status pending.  This is only returned if "hEvent"
 * 			is non-null.  "Status" parameter will be updated with result
 * 			before hEvent is signalled.
 * 		AWUSB_STATUS_INVALID_PARAMETER
 * 			One of the parameters was invalid such as "Hub".
 * 		AWUSB_STATUS_ALREADY_CONNECTED
 * 			Hub was already connected when AwUsbConnect was called.
 * 		
 ************************************************************************/
__declspec(dllexport)
AWUSB_STATUS
_cdecl
AwUsbConnect (
	IN	LPCWSTR			Hub,	
	IN 	PAWUSB_STATUS	Status,
	IN	DWORD			Timeout,
	IN	HANDLE 			hEvent OPTIONAL
);


/************************************************************************
 * AwUsbDisconnect
 * 	Parms:
 * 		Hub
 * 			AwUsb Hub IP Address in string format.
 * 
 *  Returns:
 * 		AWUSB_STATUS_SUCCESS
 * 			Successfully disconnected from hub.
 * 		AWUSB_STATUS_INVALID_PARAMETER
 * 			Failed because of an invalid parameter such as invalid
 * 			HUb IP address.
 * 		AWUSB_STATUS_DELETE_FAILED
 * 			Tried to disconnect a hub configured to be permanently connected.
 * 		
 ************************************************************************/

DECLSPEC_EXPORT
AWUSB_STATUS
_cdecl
AwUsbDisconnect (
	IN LPCWSTR		Hub
); 


/************************************************************************
 * AwUsbGetConnectionStatus
 * 	Parms:
 * 		Hub
 * 			[in]  AwUsb Hub IP Address in string format.
 * 		HostIpAddr
 * 			[out] 32-bit IP Address of host (if any) to which hub is connected. 
 * 			IpAddress is in host byte order and can be converted to TCP/IP network
 * 			byte order using the WinSock htonl function.  IpAddress is only valid
 * 			when Status is AWUSB_STATUS_CONNECTED or AWUSB_STATUS_IN_USE.
 * 		Status
 * 			[out] Updated upon return from function, and, when hEvent is non-NULL,
 * 			after the status operation completes. See Return Values.
 * 		Timeout
 * 			[in] Specifies how long to wait for status from the AwUsb hub, in 
 * 			milliseconds. Must be non-zero. Maximum timeout is 10,000 ms (10secs).
 * 			Returns AWUSB_STATUS_TIMEOUT if interval elapses.
 * 		hEvent
 * 			[in] Handle to an event which will be set to the signaled state when the
 * 			operation has been completed. If hEvent is NULL, this function will block
 * 			until it completes. If hEvent is non-NULL, then the function returns 
 * 			immediately, usually with AWUSB_STATUS_PENDING unless an error has occurred.
 * 			To create an event object, use the CreateEvent function.
 * 		
 * 
 *  Returns:
 * 		AWUSB_STATUS_CONNECTED
 * 			This host is connected to hub.
 * 		AWUSB_STATUS_INVALID_PARAMETER
 * 			Failed because of an invalid parameter such as invalid
 * 			HUb IP address.
 * 		AWUSB_STATUS_IN_USE
 * 			Hub is connected to another host (not this one).
 * 			The IP address of the host will be returned in "HostIpAddr".
 * 		AWUSB_STATUS_AVAILABLE
 * 			The hub is available (not in use).
 * 		AWUSB_STATUS_TIMEOUT
 * 			The hub was not found on the network within the
 * 			specified "Timeout".
 * 		
 * 		
 ************************************************************************/
DECLSPEC_EXPORT
AWUSB_STATUS
_cdecl
AwUsbGetConnectionStatus (
	IN	LPCWSTR	Hub,
	OUT	PDWORD	IpAddress,
	OUT	PAWUSB_STATUS	Status,
	IN	DWORD	Timeout,
	IN	HANDLE 	hEvent OPTIONAL
);
#ifdef __cplusplus
}
#endif




