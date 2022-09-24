#include <stdio.h> 									
#include <string.h>

#include <openssl/sha.h>

#include "pkg.h"

int get_key(char *filename, char *key)
{
    FILE *f = fopen(filename, "r");

    if (f == NULL)
    {
        return -1;
    }

    fscanf(f, "%s", key);
    //printf("       KEY: %s\n", key);
    fclose(f);

    return 0;
}

void sha256_string(char *string, char outputBuffer[65])
{
    unsigned char hash[SHA256_DIGEST_LENGTH];

    SHA256_CTX sha256;

    SHA256_Init(&sha256);
    SHA256_Update(&sha256, string, strlen(string));
    SHA256_Final(hash, &sha256);

    for(int i = 0; i < SHA256_DIGEST_LENGTH; i++)
    {
        sprintf(outputBuffer + (i * 2), "%02x", hash[i]);
    }

    outputBuffer[64] = 0;
}

int get_machine_key(char *hashed_key)
{
    char key[32];

    int rc = get_key(MACHINE_ID_FILENAME, key);

    if (rc != 0)
    {
        return rc;
    }

    sha256_string(key, hashed_key);

    //printf("HASHED KEY: %s\n", hashed_key);

    return 0;
}

int check_key(char *machine_id)
{
    char key[65];

    int rc = get_machine_key(key);

    if (rc != 0)
    {
        return rc;
    }

    return strcmp(key, machine_id);
}
