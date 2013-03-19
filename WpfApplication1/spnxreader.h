
/*
 * Интерфейс DLL-библиотеки Sphinx Reader c внешним миром.
 * См. документацию для получения подробной информации.
 *
 * История версий:
 *
 * 1.0     08 января 2008     Первая версия.
 * 1.1     10 января 2008     Исправления в поддержки считывателя на аппаратной ревизии 2.0.
 * 1.2     19 февраля 2008    Добавлена функция SpnxReaderReceiveW26T.
 * 1.3     16 июля 2009       Изменены соглашения вызовов на C стиль
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
 * HANDLE контрольного считывателя.
 */
typedef PVOID SPNX_HANDLE;

/*
 * Пытается "открыть" подключенный к системе контрольный считыватель.
 *
 * pxHandle - указатель на SPNX_HANDLE, в который будет записан HANDLE в случае успеха.
 *
 * Возвращает TRUE, если открыть считыватель удалось, и его HANDLE был записан в *pxHandle.
 * Возвращает FALSE, если открыть считыватель не удалось.
 */
SPNXREADER_API BOOL SpnxReaderOpen(SPNX_HANDLE *pxHandle);

/*
 * "Закрывает" ранее открытый функцией SpnxReaderOpen контрольный считыватель.
 *
 * pxHandle - указатель на SPNX_HANDLE, полученный ранее от функции SpnxReaderOpen.
 * 
 * Внимание! После вызова этой функции по адресу pxHandle записывается NULL и данный
 * HANDLE перестает быть допустимым.
 */
SPNXREADER_API void SpnxReaderClose(SPNX_HANDLE *pxHandle);

/*
 * Пытается получить следующий Wiegand-26 код от ранее открытого контрольного считывателя.
 *
 * xHandle - SPNX_HANDLE, полученный ранее от функции SpnxReaderOpen.
 * pBuffer - буфер, куда будет записан полученный код.
 * nBufferSize - длинна буфера. Должна быть минимум 3 байта, чтобы принять Wiegand-26 код.
 *
 * Возвращает TRUE в случае успеха. В таком случае в pBuffer был помещен полученный код.
 * Возвращает FALSE, если получить код не удалось. См. документацию для выяснения возможных
 * причин этого.
 *
 * Функция блокирует вызывающий поток до момента получения кода или наступлениz ошибки.
 */
SPNXREADER_API BOOL SpnxReaderReceiveW26(SPNX_HANDLE xHandle, LPVOID pBuffer, DWORD nBufferSize);

/*
 * Пытается получить следующий Wiegand-26 код от ранее открытого контрольного считывателя.
 *
 * xHandle - SPNX_HANDLE, полученный ранее от функции SpnxReaderOpen.
 * pBuffer - буфер, куда будет записан полученный код.
 * nBufferSize - длинна буфера. Должна быть минимум 3 байта, чтобы принять Wiegand-26 код.
 * nTimeout - сколько милисекунд ждать кода.
 *
 * Возвращает кол-во полученных байт данных в случае успеха. В таком случае в pBuffer был помещен
 * полученный код.
 * Возвращает 0, если код не был получен в отведенный таймаут.
 * Возвращает -1, если получить код не удалось. См. документацию для выяснения возможных
 * причин этого.
 */
SPNXREADER_API DWORD SpnxReaderReceiveW26T(SPNX_HANDLE xHandle, LPVOID pBuffer, DWORD nBufferSize, DWORD nTimeout);

} // extern "C" 

#endif // SPNXREADER_H
