SRC := ./src
INC := ./inc
OUT = ./out

CC := gcc
CFLAGS := -I$(INC) -fPIE 
LFLAGS := -static-pie -lcrypto

.PHONY: run
run: install
	./app.out

.PHONY: app
app: $(OUT)/main.o $(OUT)/pkg.o
	$(CC) $^ $(CFLAGS) -o app.out $(LFLAGS)

.PHONY: install
install: installer
	./run.out

.PHONY: installer
installer: $(OUT)/installer.o $(OUT)/pkg.o
	$(CC) $^ $(CFLAGS) -o run.out $(LFLAGS)

$(OUT)/%.o:
$(OUT)/%.o: $(SRC)/%.c $(INC)/%.h
	mkdir -p $(OUT) && $(CC) $(CFLAGS) -c $< -o $@

$(OUT)/main.o: $(SRC)/main.c
	mkdir -p $(OUT) && $(CC) $(CFLAGS) -c $< -o $@

.PHONY: $(OUT)
$(OUT):
	mkdir -p $(OUT)

.PHONY: clean
clean:
	$(RM) -r $(OUT) *.out
