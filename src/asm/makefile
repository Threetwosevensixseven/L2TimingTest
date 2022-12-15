# Makefile for L2TimingTest.
# Only tested with Embarcadero MAKE 5.41 for Windows. 
# Let me know if other Windows MAKEs have issues.
# If you fix this up for linux or mac, please contribute your changes.

# Change these variables to suit your own environment
BINDIR = ..\..\bin
SJASMPLUS = ..\..\tools\sjasmplus
HDFMONKEY = ..\..\tools\hdfmonkey
EMUDIR = C:\spec\CSpect2_16_5
EMUSD = C:\spec\sd207\cspect-next-2gb.img
SYNCDIR = C:\spec\sync\ON
RM = del /F /Q
MD = md
KILL = taskkill /F /IM
STARTSYNC = 1

# Default target is to build the NEX file from source
build: 
	$(SJASMPLUS) main.asm

# Removes all build-generated files
clean:
	$(RM) "$(BINDIR)\L2TimingTest.nex"

# Builds, injects build NEX and autoexec into CSpect SD image, and starts CSpect asynchronously.
# Autoexec will skip running .fastsync (because CSpect defaults to core version 4.0.0), then run the NEX.
# Note that the NEX doesn't behave the same as on hardware, due to various CSpect bugs.
# But the build target is here in case they get fixed in future.
emu: build
	$(HDFMONKEY) put $(EMUSD) $(BINDIR)/L2TimingTest.nex Mine
	$(HDFMONKEY) put $(EMUSD) autoexec.bas NextZXOS
    -$(KILL) cspect.exe 
    start /d $(EMUDIR) cspect -w2 -zxnext -nextrom -basickeys -exit -brk -tv -emu -mmc=$(EMUSD)

# Builds, and starts CSpect with the NEX loaded directly (like F2).
# Note that the NEX doesn't behave the same as on hardware, due to various CSpect bugs.
# But the build target is here in case they get fixed in future.
emu2: build
    start /d $(BINDIR) $(EMUDIR)\cspect -w2 -zxnext -basickeys -exit -brk -tv -emu L2TimingTest.nex


# Builds, copies NEX and autoexec to NextSync sync directory, and starts starts the server asynchronously.
# Autoexec will run .fastsync then run the NEX, next time you F4 soft reset.
# The first time you make sync, run .fasysync manually first to get the files.
sync: build
	-$(MD) "$(SYNCDIR)\Mine"
	copy "$(BINDIR)\L2TimingTest.nex" "$(SYNCDIR)\Mine\*.*"
	-$(MD) "$(SYNCDIR)\NextZXOS"
	copy autoexec.bas "$(SYNCDIR)\NextZXOS\*.*"
# Warning: kills all running python processes
# Remove these two lines and start the server manually if that bothers you
	-$(KILL) py.exe
	start /d $(SYNCDIR) /min nextsync.py
