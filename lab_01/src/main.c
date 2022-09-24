#include <stdio.h>

#include "pkg.h"

int main()
{
    char key[65];

    int rc = get_key(KEY_FILENAME, key);
    if (rc != 0)
    {
        printf("Error of get_key!\n");
        return rc;
    }

    rc = check_key(key);
    if (rc != 0)
    {
        printf("Error of check_key! No access.\n");
        return rc;
    }

    printf("Hello, world!\n");

    return 0;
}
