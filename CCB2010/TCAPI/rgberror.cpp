#include "StdAfx.h"
#include "./include/rgbdefs.h"
/*
* This cpp source file only exists to remove what is at least 2 printed pages of code
* from the sample application.  This function adds nothing to the explanation of how
* to write an application for the RGB Capture card, but does make any user-seen error
* messages instantly understandable.
*/

char * RGBErrorToString(RGBERROR error)
{
#define ErrorCase(x) case x: return #x;
   switch(error)
   {
      ErrorCase(RGBERROR_NO_ERROR)
      ErrorCase(RGBERROR_DRIVER_NOT_FOUND)
      ErrorCase(RGBERROR_UNABLE_TO_LOAD_DRIVER)
      ErrorCase(RGBERROR_HARDWARE_NOT_FOUND)
      ErrorCase(RGBERROR_INVALID_INDEX)
      ErrorCase(RGBERROR_DEVICE_IN_USE)
      ErrorCase(RGBERROR_INVALID_HRGBCAPTURE)
      ErrorCase(RGBERROR_INVALID_POINTER)
      ErrorCase(RGBERROR_INVALID_SIZE)
      ErrorCase(RGBERROR_INVALID_FLAGS)
      ErrorCase(RGBERROR_INVALID_DEVICE)
      ErrorCase(RGBERROR_INVALID_INPUT)
      ErrorCase(RGBERROR_INVALID_FORMAT)
      ErrorCase(RGBERROR_INVALID_VDIF_CLOCKS)
      ErrorCase(RGBERROR_INVALID_PHASE)
      ErrorCase(RGBERROR_INVALID_BRIGHTNESS)
      ErrorCase(RGBERROR_INVALID_CONTRAST)
      ErrorCase(RGBERROR_INVALID_BLACKLEVEL)
      ErrorCase(RGBERROR_INVALID_SAMPLERATE)
      ErrorCase(RGBERROR_INVALID_PIXEL_FORMAT)
      ErrorCase(RGBERROR_INVALID_HWND)
      ErrorCase(RGBERROR_INSUFFICIENT_RESOURCES)
      ErrorCase(RGBERROR_INSUFFICIENT_BUFFERS)
      ErrorCase(RGBERROR_INSUFFICIENT_MEMORY)
      ErrorCase(RGBERROR_SIGNAL_NOT_DETECTED)
      ErrorCase(RGBERROR_INVALID_SYNCEDGE)
      ErrorCase(RGBERROR_OLD_FIRMWARE)
      ErrorCase(RGBERROR_HWND_AND_FRAMECAPTUREDFN)
      ErrorCase(RGBERROR_HSCALED_OUT_OF_RANGE)
      ErrorCase(RGBERROR_VSCALED_OUT_OF_RANGE)
      ErrorCase(RGBERROR_SCALING_NOT_SUPPORTED)
      ErrorCase(RGBERROR_BUFFER_TOO_SMALL)
      ErrorCase(RGBERROR_HSCALE_NOT_WORD_DIVISIBLE)
      ErrorCase(RGBERROR_HSCALE_NOT_DWORD_DIVISIBLE)
      ErrorCase(RGBERROR_HORADDRTIME_NOT_WORD_DIVISIBLE)
      ErrorCase(RGBERROR_HORADDRTIME_NOT_DWORD_DIVISIBLE)
      ErrorCase(RGBERROR_VERSION_MISMATCH)
      ErrorCase(RGBERROR_ACC_REALLOCATE_BUFFERS)
      ErrorCase(RGBERROR_BUFFER_NOT_VALID)
      ErrorCase(RGBERROR_BUFFERS_STILL_ALLOCATED)
      ErrorCase(RGBERROR_NO_NOTIFICATION_SET)
      ErrorCase(RGBERROR_CAPTURE_DISABLED)
      ErrorCase(RGBERROR_INVALID_PIXELFORMAT)
      ErrorCase(RGBERROR_ILLEGAL_CALL)
      ErrorCase(RGBERROR_CAPTURE_OUTSTANDING)
      ErrorCase(RGBERROR_MODE_NOT_FOUND)
      ErrorCase(RGBERROR_CANNOT_DETECT)
      ErrorCase(RGBERROR_NO_MODE_DATABASE)
      ErrorCase(RGBERROR_CANT_DELETE_MODE)
      ErrorCase(RGBERROR_MUTEX_FAILURE)
      ErrorCase(RGBERROR_THREAD_FAILURE)
      ErrorCase(RGBERROR_NO_COMPLETION)
      ErrorCase(RGBERROR_INSUFFICIENT_RESOURCES_HALLOC)
      ErrorCase(RGBERROR_INSUFFICIENT_RESOURCES_RGBLIST)
   }
   {
      static char szBuffer[128];
      wsprintf(szBuffer, "Unknown RGB Error: 0x%8.8X (see rGBDEFS.H", error);
      return szBuffer;
   }
}

