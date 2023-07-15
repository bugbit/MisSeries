#include <memory.h>
/*
Create file secret.h:
const char cliendId[] = "your client id";
const char clientSecret[] = "your client secret";

*/
#include "secrets.h"

#define MIN(a,b) ((a) < (b) ? a : b)

int getClientId(unsigned char* data, int maxlen)
{
	int size = MIN(sizeof(cliendId) - 1, maxlen);

	memcpy(data, cliendId, size);

	return size;
}

int getClientSecret(unsigned char* data, int maxlen)
{
	int size = MIN(sizeof(clientSecret) - 1, maxlen);

	memcpy(data, clientSecret, size);

	return size;
}