#include <stdio.h>
#include <unistd.h>
#include <sys/wait.h>

#include "pkg.h"
#include "installer.h"

void compile()
{
	int child_id;

	if ((child_id = fork()) == -1)
	{
		return;
	}	
	else if (!child_id)
	{
		// child process
		execlp("make", "make", "app", NULL);
	}
	
	if (child_id)
	{
		// main process
		int status;
		wait(&status);
	}
}

int write_key(char *key)
{
	FILE *f = fopen(KEY_FILENAME, "w");
	if (f == NULL)
	{
		return -1;
	}

	fprintf(f, "%s", key);

	return 0;
}

int main()
{
	char key[65];
	int rc = get_machine_key(key);
	if (rc != 0)
	{
		printf("ERROR OF GET_MACHINE_KEY\n");
		return rc;
	}

	//printf("MACHINE_KEY: %s\n", key);

	write_key(key);

	compile();

	return 0;
}
