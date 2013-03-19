
/*
 * ��������� DLL-���������� Sphinx Reader c ������� �����.
 * ��. ������������ ��� ��������� ��������� ����������.
 *
 * ������� ������:
 *
 * 1.0     08 ������ 2008     ������ ������.
 * 1.1     10 ������ 2008     ����������� � ��������� ����������� �� ���������� ������� 2.0.
 * 1.2     19 ������� 2008    ��������� ������� SpnxReaderReceiveW26T.
 * 1.3     16 ���� 2009       �������� ���������� ������� �� C �����
 *
 */

#ifndef SPNXREADER_H
#define SPNXREADER_H

#ifdef SPNXREADER_EXPORTS
#define SPNXREADER_API __declspec(dllexport)
#else
#define SPNXREADER_API __declspec(dllimport)
#endif

#include <windows.h>

extern "C" {

/*
 * HANDLE ������������ �����������.
 */
typedef PVOID SPNX_HANDLE;

/*
 * �������� "�������" ������������ � ������� ����������� �����������.
 *
 * pxHandle - ��������� �� SPNX_HANDLE, � ������� ����� ������� HANDLE � ������ ������.
 *
 * ���������� TRUE, ���� ������� ����������� �������, � ��� HANDLE ��� ������� � *pxHandle.
 * ���������� FALSE, ���� ������� ����������� �� �������.
 */
SPNXREADER_API BOOL SpnxReaderOpen(SPNX_HANDLE *pxHandle);

/*
 * "���������" ����� �������� �������� SpnxReaderOpen ����������� �����������.
 *
 * pxHandle - ��������� �� SPNX_HANDLE, ���������� ����� �� ������� SpnxReaderOpen.
 * 
 * ��������! ����� ������ ���� ������� �� ������ pxHandle ������������ NULL � ������
 * HANDLE ��������� ���� ����������.
 */
SPNXREADER_API void SpnxReaderClose(SPNX_HANDLE *pxHandle);

/*
 * �������� �������� ��������� Wiegand-26 ��� �� ����� ��������� ������������ �����������.
 *
 * xHandle - SPNX_HANDLE, ���������� ����� �� ������� SpnxReaderOpen.
 * pBuffer - �����, ���� ����� ������� ���������� ���.
 * nBufferSize - ������ ������. ������ ���� ������� 3 �����, ����� ������� Wiegand-26 ���.
 *
 * ���������� TRUE � ������ ������. � ����� ������ � pBuffer ��� ������� ���������� ���.
 * ���������� FALSE, ���� �������� ��� �� �������. ��. ������������ ��� ��������� ���������
 * ������ �����.
 *
 * ������� ��������� ���������� ����� �� ������� ��������� ���� ��� ����������z ������.
 */
SPNXREADER_API BOOL SpnxReaderReceiveW26(SPNX_HANDLE xHandle, LPVOID pBuffer, DWORD nBufferSize);

/*
 * �������� �������� ��������� Wiegand-26 ��� �� ����� ��������� ������������ �����������.
 *
 * xHandle - SPNX_HANDLE, ���������� ����� �� ������� SpnxReaderOpen.
 * pBuffer - �����, ���� ����� ������� ���������� ���.
 * nBufferSize - ������ ������. ������ ���� ������� 3 �����, ����� ������� Wiegand-26 ���.
 * nTimeout - ������� ���������� ����� ����.
 *
 * ���������� ���-�� ���������� ���� ������ � ������ ������. � ����� ������ � pBuffer ��� �������
 * ���������� ���.
 * ���������� 0, ���� ��� �� ��� ������� � ���������� �������.
 * ���������� -1, ���� �������� ��� �� �������. ��. ������������ ��� ��������� ���������
 * ������ �����.
 */
SPNXREADER_API DWORD SpnxReaderReceiveW26T(SPNX_HANDLE xHandle, LPVOID pBuffer, DWORD nBufferSize, DWORD nTimeout);

} // extern "C" 

#endif // SPNXREADER_H
