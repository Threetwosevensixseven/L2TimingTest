# Makefile for L2TimingTest.
# Only tested with GNU MAKE 3.81 for Windows.
# If you fix this up for linux or mac, please contribute your changes

# Change these variables to suit your own environment
BINDIR = ..\..\bin
SJASMPLUS = ..\..\tools\sjasmplus
HDFMONKEY = ..\..\tools\hdfmonkey
EMUDIR = C:\spec\CSpect3_0_2_1
EMUSD = C:\spec\sd209\cspect-next-2gb.img
SYNCDIR = C:\spec\sync\ON
RM = del /F /Q
MD = md
KILL = taskkill /F /IM
PY = python3.13.exe

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
	-$(HDFMONKEY) mkdir $(EMUSD) Mine
	$(HDFMONKEY) put $(EMUSD) $(BINDIR)/L2TimingTest.nex Mine
	$(HDFMONKEY) put $(EMUSD) autoexec.bas NextZXOS
#   Uncomment this to get the current autoexec.bas/.txt in the image back into the project:
#	$(HDFMONKEY) get $(EMUSD) /NextZXOS/autoexec.bas $(SRCDIR)/autoexec.bas
#	$(HDFMONKEY) get $(EMUSD) /NextZXOS/autoexec.txt $(SRCDIR)/autoexec.txt
	-$(KILL) cspect.exe 
	CMD /C start /d $(EMUDIR) cspect -w3 -zxnext -nextrom -basickeys -exit -brk -tv -emu -mmc=$(EMUSD)

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
	-$(KILL) $(PY)
	CMD /C start /min /d $(SYNCDIR) nextsync.py
