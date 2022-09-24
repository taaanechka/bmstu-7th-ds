#pragma once

int get_key(char *filename, char *key);

void sha256_string(char *string, char outputBuffer[65]);

int get_machine_key(char *key);

int check_key(char *machine_id);

#define MACHINE_ID_FILENAME "/var/lib/dbus/machine-id"

#define KEY_FILENAME "key.txt"
