; main.asm
                        DEVICE ZXSPECTRUMNEXT           ; Use ZX Spectrum Next memory model
                        MMU 0 3, 0                      ; slot 0=8K page 0, s1=p1, s2=p2, s3=p3
                        MMU 4 7, 6                      ; slot 4=8K page 6, s5=p7, s6=p8, s7=p9

                        INCLUDE "constants.asm"         ; Constants
                        INCLUDE "macros.asm"            ; Macros

                        ORG $0000                       ; The entry point code is assembled to run at $0000
Start:                                                  ; but NEX can only do custom paging in the upper 16K.
                        nextreg $50, 0                  ; So the entry bank is put at $C000 and entered there.
                        jp Relocate                     ; We put the bank back at $000 asap, and jump to the
                                                        ; equivalent current position to continue setting up.

                        ORG $0018
Delay:                  DUP 5
                          nop
                        EDUP
                        ret                         
                           
                        ORG $0038
                        ei
                        ret

                        ORG $0100                       
Relocate: 
                        nextreg $07, %11                ; Next Turbo 28MHz
                        BORDER 0
                        NRREAD 6
                        //and %01000111                   ; Disable both NMI buttons, 50/60 (F3) and turbo hotkey (F8)
                        nextreg 6, a
                        nextreg $51, 1
                        nextreg $52, 2
                        nextreg $53, 3
                        nextreg $54, 6
                        nextreg $55, 7
                        nextreg $56, 8
                        

                        NRREAD 5
                        and %100                        ; Mask out 50/60 (bit 2)
                        jr nz, .Is60                    ; For 50Hz only, shorten the end of frame padding
                        ld hl, SoakUp+3                 ; from four to three pad instructions
                        ld (hl), $C9                    ; followed by RET
.Is60:
                        nextreg 67, %00010000           ; Select layer 2 first palette
                        nextreg 64, 0                   ; Reset palette index  
                        call SetPal
                        call FillPixels                                            
                        nextreg $57, 9
                        nextreg 18, 9                   ; Start of Layer 2, 16K bank
                        nextreg 112, %00010000          ; Layer 2 320x256, no offset
                        nextreg 21, %00000100           ; LSU layer order
                        nextreg 24, 0
                        nextreg 24, 255
                        nextreg 24, 0
                        nextreg 24, 255
                        nextreg 100, 33                 ; Set vertical line offset to first layer 2 line
                        nextreg 105, 128                ; Enable layer 2
                        im 1                            ; Use mode 1 interrupts, because we have RAM in slot 0
                        ei                              ; and enable interrupts                    
MainLoop:               
                        halt  
                        BORDER 0                     
.WaitLine0:             NRREAD 31
                        jr nz, .WaitLine0
                        NRREAD 30
                        jr nz, .WaitLine0
                        call PreSoak
                        BORDER2 0

                        DISPLAY "Start of Unrolled section=",$   
                        DUP 3178, RepeatValue //2169 good, 2500 bad
                          nextreg 18, (RepeatValue mod 36) + 9
                          rst $18
                        EDUP
                        DISPLAY "End of Unrolled section=",$   

                        call SoakUp
                        jp MainLoop
                        //DISPLAY "Last Unrolled Address=",$                        

SoakUp:                 // 3 for VGA50
                        // 4 for VGA60
                        DUP 4
                          ld b, (hl)
                        EDUP
                        ret 

PreSoak:                ld b, 33
.Loop                   nextreg 4, 0
                        nop
                        djnz .Loop
                        nop:nop:nop:nop
                        ret                  

SetPal:                 
                        xor a
                        ld b, a
.PalLoop:                nextreg 65, a
                        inc a
                        djnz .PalLoop
                        ret

FillPixels:
                        ld a, 18
                        ld (.Bank), a
.Loop1:                 ld b, 10
.Loop2:                 exx
                        nextreg $57, a
                        ld hl, $E000
                        ld (hl), a
                        ld de, $E001
                        ld bc, $1FFF
                        ldir
                        exx
.Bank equ $+1:          ld a, SMC
                        inc a
                        ld (.Bank), a
                        djnz FillPixels.Loop2
                        cp 96
                        jr c, FillPixels.Loop1
                        ret
pend

                        DUP 30, Fred
                          db Fred
                        EDUP

                        DISPLAY "Last Address=",$

                        SAVENEX OPEN "../../bin/L2TimingTest.nex", $C000, $0000, 0
                        SAVENEX BANK 0,1
                        SAVENEX CLOSE