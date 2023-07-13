#include <memory.h>
#include "secrets.h"

#define MIN(a,b) ((a) < (b) ? a : b)

int getClientId(unsigned char* data, int maxlen)
{
	int size = MIN(sizeof(cliendId) - 1, maxlen);

	memcpy(data, cliendId, size);

	return size;
}